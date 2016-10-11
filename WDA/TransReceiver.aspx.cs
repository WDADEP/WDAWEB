using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WDA.Class;

namespace WDA
{
    // Add by Luke 2016/05/30
    public partial class TransReceiver : PageUtility
    {
        int roleid = -1;

        #region Page_Load()
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadPage(true);

            try
            {
                if (!IsPostBack)
                {
                    this.HiddenShowPanel.Value = "false";

                    this.GridView1.PageSize = 10;

                    this.TxtWpinno.Focus();

                    this.TxtReceiver.Text = this.UserInfo.RealName;
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex);
            }
        } 
        #endregion

        #region DataBind()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MyDataGrid">DataGrid Object</param>
        /// <param name="Anew">是否重新撈資料</param>
        /// <param name="LockPageNum">是否保留目前頁數</param>
        /// <param name="SelectIndex">索引類型</param>
        public void DataBind(bool Anew, bool LockPageNum)
        {
            DataTable dt = null;
            try
            {
                string strSql = string.Empty;
                string where = string.Empty;

                if (Anew)
                {

                    if (!string.IsNullOrEmpty(this.TxtReceiver.Text.Trim()))
                    {
                        string realName = this.TxtReceiver.Text.Trim();
                        where += string.Format(" And tt.RECEIVER = '{0}'", realName);

                    }

                    DateTime dTime = DateTime.Now.Date;
                    where += string.Format(" AND tt.TRANSTIME Between TO_DATE('{0}','YYYY/MM/DD HH24:MI:SS') And TO_DATE('{1}','YYYY/MM/DD HH24:MI:SS') ORDER BY tt.TRANSTIME DESC", dTime.ToString("yyyy/MM/dd HH:mm:ss"), dTime.AddDays(1.0).ToString("yyyy/MM/dd HH:mm:ss"));


                    strSql = this.Select.Transtable(where);

                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                    //Session["Transtable"] = strSql;

                    this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                    dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                    if (dt.Rows.Count == 0)
                    {
                        this.ShowMessage("目前查詢沒有任何資料", MessageMode.INFO);

                        ViewState[this.GridView1.ClientID] = dt;
                        this.HiddenShowPanel.Value = "false";
                    }
                    else
                    {
                        ViewState[this.GridView1.ClientID] = dt;
                        this.HiddenShowPanel.Value = "true";
                    }
                }

                this.GridView1.DataBind((DataTable)ViewState[this.GridView1.ClientID], Anew, LockPageNum);
            }
            catch (System.Exception ex) { this.ShowMessage(ex); }
            finally
            {
                if (dt != null) { dt.Dispose(); dt = null; }

                this.DBConn.Dispose(); this.DBConn = null;
            }
        }
        #endregion

        #region BtnOK_Click()
        protected void BtnOK_Click(object sender, EventArgs e)
        {
            try
            {

                if (this.InsertData())
                {
                    this.DataBind(true, false);
                    this.TxtWpinno.Text = "";
                    this.TxtWpinno.Focus();
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex);
            }
        }
        #endregion

        #region BtnClear_Click()
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(this.Request.Url.AbsoluteUri);
        }
        #endregion

        #region InsertData
        /// <summary>
        /// 新增資料
        /// </summary>
        /// <returns></returns>
        private bool InsertData()
        {
            string strSql = string.Empty, strWhere = string.Empty;

            int result = 0;

            Hashtable ht = new Hashtable();
            DataTable dt = null;
            try
            {
                string Wpinno = this.TxtWpinno.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                string Receiver = this.TxtReceiver.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();

                ht.Add("Wpinno", Wpinno);
                //MODIFY BY RICHARD 20160705
                //ht.Add("Transtime", "SYSDATE");
                ht.Add("Transtime", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                ht.Add("Receiver", Receiver);


                //檢查是否有同一個人重複新增相同文號 Richard 20160603
                strWhere = string.Format("And tt.WPINNO = '{0}' and tt.RECEIVER=N'{1}' \n", Wpinno,Receiver);
                strSql = this.Select.Transtable(strWhere);
                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);
                if (dt.Rows.Count > 0)
                {
                    this.ShowMessage("收文號已簽收", MessageMode.INFO);
                    this.TxtWpinno.Text = "";
                    this.TxtWpinno.Focus();
                    return false;
                }

                if (roleid == -1)
                {
                    this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;
                    strWhere = string.Format("And realname = N'{0}'\n", Receiver);
                    strSql = this.Select.RoleIDFromUserTable(strWhere);
                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                    dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);
                    if (dt.Rows.Count == 0)
                    {
                        this.ShowMessage("未查詢到收文者資料", MessageMode.INFO);
                        return false;
                    }
                    try
                    {
                        roleid = Convert.ToInt32(dt.Rows[0]["roleid"]);
                    }
                    catch (Exception) { }
                }
                char flag;
                switch (roleid)
                {
                    case 4:
                        flag = 'N';
                        break;
                    case 5:
                        flag = 'R';
                        break;
                    default:
                        flag = 'Z';
                        break;
                }
                ht.Add("Flag", flag);

                //取得相關資料
                strWhere = string.Format(" AND WPINNO = '{0}'", Wpinno);
                strSql = this.Select.WprecInfos(strWhere);
                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                DataTable dtWprec = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);


                if (dtWprec.Rows.Count > 0){
                    ht.Add("COMMNAME", dtWprec.Rows[0]["COMMNAME"].ToString());
                    ht.Add("WPKIND", dtWprec.Rows[0]["WPKIND"].ToString());
                    ht.Add("WPTYPE", dtWprec.Rows[0]["WPTYPE"].ToString());

                    //ADD BY RICHARD 20160705 檢查該文號是否有發文(發文日期或發文人員有值)
                    //strWPOUTDATE = dtWprec.Rows[0]["WPOUTDATE"].ToString().Trim();
                    //strSENDMAN = dtWprec.Rows[0]["SENDMAN"].ToString().Trim();
                    //if (string.IsNullOrEmpty(strWPOUTDATE) && string.IsNullOrEmpty(strSENDMAN))
                    //{
                    //    this.ShowMessage("發文號為空值", MessageMode.INFO);
                    //    return false;
                    //}
                }
                else
                {
                    this.ShowMessage("未查詢到收文號資料", MessageMode.INFO);
                    return false;
                }


                this.DBConnTransac.GeneralSqlCmd.Command.CommandTimeout = 90;

                strSql = this.Insert.Transtable(ht);
                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                result = this.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql);

                if (result > 0)
                {
                    #region Monitor

                    string userIP = this.Request.ServerVariables["REMOTE_ADDR"].ToString();

                    this.MonitorLog.LogMonitor(Wpinno, this.UserInfo.UserName, this.UserInfo.RealName, userIP, Monitor.MSGID.WDA16, "105簽收");
                    #endregion

                    this.DBConnTransac.GeneralSqlCmd.Transaction.Commit();
                }
                else
                {
                    this.WriteLog(global::Log.Mode.LogMode.ERROR, "Insert TRANSTABLE Fail");
                }
            }
            catch (System.Exception ex)
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
                if (ht != null) { ht.Clear(); ht = null; }

                if (this.DBConn != null) { this.DBConn.Dispose(); this.DBConn = null; }

                if (this.DBConnTransac != null) { this.DBConnTransac.Dispose(); this.DBConnTransac = null; }
            }
            return Convert.ToBoolean(result);
        }
        #endregion

        #region GridView Events

        #region GridView1_Sorting()
        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            e.Sorting(sender);

            this.DataBind(false, true);
        }
        #endregion

        #region GridView1_PageIndexChanging()
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            e.PageIndexChanging(sender);

            this.DataBind(false, false);
        }
        #endregion 

        #endregion
    }
}