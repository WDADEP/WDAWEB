using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WDA.Class;

namespace WDA
{
    public partial class ReservationBorrowApprove : GridViewUtility
    {
        #region Page_Load()
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadPage(true);
            try
            {
                if (!IsPostBack)
                {
                    this.DataBind(true, false);

                    this.GridView1.PageSize = Convert.ToInt32(this.GetSystem("PageSize"));
                }
            }
            catch (Exception ex) { this.ShowMessage(ex); }
        }
        #endregion

        #region BtnOK_Click()
        protected void BtnOK_Click(object sender, EventArgs e)
        {
            string strSql = string.Empty, strWhere = string.Empty;

            int result = 0;
            try
            {
                for (int i = 0; i < this.GridView1.Rows.Count; i++)
                {
                    string wpinNo = this.GridView1.Rows[i].Cells[1].Text.Trim();
                    string viewtype = this.GridView1.Rows[i].Cells[9].Text.Trim();
                    DateTime transt = Convert.ToDateTime(this.GridView1.Rows[i].Cells[10].Text.Trim());

                    string prtflag = ((RadioButtonList)this.GridView1.Rows[i].Cells[0].FindControl("rBtnListApprove")).SelectedValue;

                    if (!String.IsNullOrEmpty(prtflag))
                    {
                        strWhere = string.Format("And WpinNo = '{0}' And ReDate is Null And Transt = TO_DATE('{1}', 'YYYY/MM/DD HH24:MI:SS')\n",
                            wpinNo,
                            transt.ToString("yyyy/MM/dd HH:mm:ss"));

                        if (prtflag == "Z")
                            strSql = this.Update.WpborrowApprove(prtflag, strWhere, true);
                        else
                            strSql = this.Update.WpborrowApprove(prtflag, strWhere, false);

                        this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                        result = this.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql);

                        if (result <= 0)
                        {
                            this.ShowMessage("簽核失敗"); return;
                        }

                        #region 電子檔核准後直接塞 Fileboro

                        if (prtflag == "F" && viewtype == "2")
                        {
                            Hashtable ht = new Hashtable();
                            ht.Add("WPINNO", wpinNo);
                            ht.Add("TRANST", transt.ToString("yyyy/MM/dd HH:mm:ss"));
                            //ht.Add("WORKERID", this.UserInfo.UserName);

                            strSql = this.Insert.FileboroByElec(ht);
                            this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                            result = this.DBConn.GeneralSqlCmd.ExecuteNonQuery(strSql);

                            if (result < 1) throw new Exception("調妥新增失敗");
                        }

                        #endregion
                    }

                    #region Monitor

                    string userIP = this.Request.ServerVariables["REMOTE_ADDR"].ToString();

                    this.MonitorLog.LogMonitor(wpinNo, this.UserInfo.UserName, this.UserInfo.RealName, userIP, Monitor.MSGID.WDA08, string.Empty);
                    #endregion
                }

                if (result >= 1)
                {
                    this.DBConnTransac.GeneralSqlCmd.Transaction.Commit();

                    this.ShowMessage("簽核完成", MessageMode.INFO);
                }

                this.DataBind(true, true);
            }
            catch (Exception ex)
            {
                try
                {
                    if (this.DBConnTransac.GeneralSqlCmd.Transaction != null) this.DBConnTransac.GeneralSqlCmd.Transaction.Rollback();
                }
                catch { }

                this.ShowMessage(ex);
            }
            finally
            {
                if (this.DBConnTransac != null)
                {
                    this.DBConnTransac.Dispose(); this.DBConnTransac = null;
                }
                if (this.DBConn != null)
                {
                    this.DBConn.Dispose(); this.DBConn = null;
                }
            }
        }
        #endregion

        #region Private Method

        #region DataBind()
        /// <summary>
        /// 資料繫結
        /// </summary>
        /// <param name="Anew">是否重新撈資料</param>
        /// <param name="LockPageNum">是否保留目前頁數</param>
        public void DataBind(bool Anew, bool LockPageNum)
        {
            string strSql = string.Empty, strWhere = string.Empty;

            DataTable dt = null;
            try
            {
                if (Anew)
                {
                    strWhere += string.Format("And wb.Prtflag = 'N' And wb.Approveuserid = '{0}' And wb.ReDate is Null\n", UserInfo.UserID);

                    strSql = this.Select.WpborrowToApprove(strWhere);

                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                    this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                    dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                    if (dt.Rows.Count == 0)
                    {
                        //this.ShowMessage("目前沒有任何資料", MessageMode.INFO);
                        this.GridView1.DataBind();
                        this.BtnOK.Visible = false;
                        return;
                    }
                    else
                    {
                        this.BtnOK.Visible = true;
                    }

                    ViewState[this.GridView1.ClientID] = dt;
                }
                this.GridView1.DataBind((DataTable)ViewState[this.GridView1.ClientID], Anew, LockPageNum, this.lblTotalPage_GridView1, this.lblPage_GridView1, null);
            }
            catch (System.Exception ex) { this.ShowMessage(ex); }
            finally
            {
                if (dt != null) { dt.Dispose(); dt = null; }

                if (this.DBConn != null) { this.DBConn.Dispose(); this.DBConn = null; }
            }
        }
        #endregion

        #endregion

        #region GridView

        #region GridView1_RowCreated()
        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            e.RowVisible(sender, new int[] { 9});
        }
        #endregion

        #region GridView1_RowDataBound()
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.RowDataBound();
        }
        #endregion

        #region GridView1_PageIndexChanging()
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            e.PageIndexChanging(sender);

            this.DataBind(false, false);
        }
        #endregion

        #region GridView1_Sorting()
        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            e.Sorting(sender);

            this.DataBind(false, true);
        }
        #endregion

        #endregion
    }
}