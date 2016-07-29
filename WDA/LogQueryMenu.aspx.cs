using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WDA.Class;

namespace WDA
{
    public partial class LogQueryMenu : PageUtility
    {
        #region Page_Load()
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadPage(true);

            if (!this.IsPostBack)
            {
                try
                {
                    this.HiddenShowPanel.Value = "false";

                    this.GridView1.PageSize = Convert.ToInt32(this.GetSystem("PageSize"));

                    this.InitInfo();
                }
                catch (System.Exception ex) { this.ShowMessage(ex); }
            }
        } 
        #endregion

        #region InitInfo()
        private void InitInfo()
        {
            try
            {
                this.LoadSystemOperatingPrivilegeList();
            }
            catch (System.Exception ex) { this.ShowMessage(ex); }
        }
        #endregion

        #region LoadSystemOperatingPrivilegeList()
        private void LoadSystemOperatingPrivilegeList()
        {
            this.LiteralSystemOperatingMenu.Text = "<ul id=\"TreeSystemOperating\"><li><input type=\"checkbox\"><span>系統行為</span>";

            string strSql = string.Empty;

            DataTable dt = null;

            try
            {
                strSql = this.Select.GetMessageTable(0);

                dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    this.LiteralSystemOperatingMenu.Text += string.Format("<ul id=\"node2\"><li><input type=\"checkbox\"><span>{0}<input type=\"Hidden\" value={1}></span></ul>", dt.Rows[i]["MsgText"].ToString(), dt.Rows[i]["MsgID"].ToString());
                }

                this.LiteralSystemOperatingMenu.Text += "</ul>";
            }
            catch (System.Exception ex) { this.ShowMessage(ex); }
            finally
            {
                if (dt != null) dt = null;

                this.DBConn.Dispose(); this.DBConn = null;
            }
        }
        #endregion

        #region BtnOK_Click()
        protected void BtnOK_Click(object sender, EventArgs e)
        {
            try
            {
                this.DataBind(true, false);
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

        #region DataBind()
        /// <summary>
        /// 資料繫結
        /// </summary>
        /// <param name="Anew">是否重新撈資料</param>
        /// <param name="LockPageNum">是否保留目前頁數</param>
        public void DataBind(bool Anew, bool LockPageNum)
        {
            string strSql = string.Empty;
            string where = string.Empty;
            string systemOperatingWhere = string.Empty;

            DataTable dt = null;
            try
            {
                if (Anew)
                {
                    string startTime = this.txtCreateTime.Text.Trim().Replace(StringFormatException.Mode.Sql);//開始日期
                    string endTime = this.txtEndTime.Text.Trim().Replace(StringFormatException.Mode.Sql);//結束日期

                    //ADD BY Richard 20160322 
                    if (string.IsNullOrEmpty(endTime) || string.IsNullOrEmpty(startTime))
                        startTime = startTime.Trim();
                    else
                    {
                        endTime = DateTime.Parse(endTime).AddDays(1).AddSeconds(-1).ToString("yyy/MM/dd HH:mm:ss");
                        where += string.Format("And lt.TransDateTime >= '{0}' and lt.TransDateTime <= '{1}'", startTime, endTime);
                    }

                    if (!string.IsNullOrEmpty(txtWPINNO.Text))
                    {
                        string wpinno = this.txtWPINNO.Text.Trim().Replace(StringFormatException.Mode.Sql);

                        where += string.Format("And lt.WPINNO like '%{0}'", wpinno);
                    }

                    if (!string.IsNullOrEmpty(txtUserName.Text))
                    {
                        string userName = this.txtUserName.Text.Trim().Replace(StringFormatException.Mode.Sql);

                        where += string.Format("And lt.UserName ='{0}'", userName);
                    }

                    #region 系統行為
                    string[] privilegeSO = this.HiddenSystemOperatingPrivilegeList.Value.Split('|');
                    //string[] privilegeSO = this.LiteralSystemOperatingMenu;

                    if (privilegeSO.Length > 0 && !string.IsNullOrEmpty(privilegeSO[0].Trim()))
                    {
                        systemOperatingWhere += " And mt.MsgID In (";

                        for (int i = 0; i < privilegeSO.Length; i++)
                        {
                            systemOperatingWhere += i == 0 ? privilegeSO[i] : "," + privilegeSO[i];
                        }

                        systemOperatingWhere += ")";
                    }
                    else
                    {
                        //REMARK BY RICHARD 20160322
                        //systemOperatingWhere += " And mt.MsgID In (0)";
                    }
                    #endregion

                    strSql = this.Select.LogQuery(systemOperatingWhere, where);

                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                    this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                    dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                    if (dt.Rows.Count == 0)
                    {
                        this.ShowMessage("目前查詢沒有任何資料");

                        ViewState[this.GridView1.ClientID] = dt;
                        dt.Dispose(); dt = null;
                    }
                    else
                    {
                        ViewState[this.GridView1.ClientID] = dt;
                        dt.Dispose(); dt = null;
                        this.HiddenShowPanel.Value = "true";
                    }
                }
                this.GridView1.DataBind((DataTable)ViewState[this.GridView1.ClientID], Anew, LockPageNum, this.lblTotalPage_GridView1, this.lblPage_GridView1, null);
            }
            catch (System.Exception ex)
            {
                this.ShowMessage(ex);
            }
            finally
            {
                this.DBConn.Dispose(); this.DBConn = null;
            }
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

        #region GridView1_RowDataBound()
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.RowDataBound();
        }
        #endregion
    }
}