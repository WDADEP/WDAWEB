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
    public partial class FileSearch : PageUtility
    {
        #region Page_Load()
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadPage(true);

            try
            {
                if (!IsPostBack)
                {
                    string sScript = "$('#divPanel').hide();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "FileSearch", sScript, true);

                    this.GridView2.PageSize = Convert.ToInt32(this.GetSystem("PageSize"));
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex);
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
        /// 
        /// </summary>
        /// <param name="MyDataGrid">DataGrid Object</param>
        /// <param name="Anew">是否重新撈資料</param>
        /// <param name="LockPageNum">是否保留目前頁數</param>
        /// <param name="SelectIndex">索引類型</param>
        public void DataBind(bool Anew, bool LockPageNum)
        {
            try
            {
                string strSql = string.Empty;
                string where = string.Empty;

                if (Anew)
                {
                    DataTable dtWpborrow = null;
                    //ADD BY RICHARD 20160715
                    DataTable dtUser = null;


                    string wpinno = string.Empty;
                    string strUserID = string.Empty;


                    //取得調檔人ID
                    if (!string.IsNullOrEmpty(this.TxtBorrowUserName.Text.Trim()))
                    {
                        where = string.Format(" AND RealName = '{0}' ", this.TxtBorrowUserName.Text.Trim());
                        strSql = this.Select.UserTable(where);
                        this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                        dtUser = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);
                        if (dtUser.Rows.Count > 0)
                        {
                            strUserID = dtUser.Rows[0]["UserName"].ToString();
                        }
                        dtUser.Dispose();
                    }

                    //調檔人為空
                    if (string.IsNullOrEmpty(strUserID))
                    {
                        this.ShowMessage("目前查詢沒有任何資料", MessageMode.INFO);
                        string sScript = "$('#divPanel').hide();";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "FileSearch", sScript, true);                        
                    }
                    else //調檔人有值
                    {
                        //收文號為空
                        if (string.IsNullOrEmpty(this.TxtWPINNO.Text.Trim()))
                        {
                            where = string.Format("And WP.RECEIVER ='{0}' AND WP.REDATE IS NULL AND ((fb.chk='Y' And wp.viewtype =2) or(fb.chk='N' And wp.viewtype =1)) ", strUserID);
                            strSql = this.Select.FileQuery(where);

                            this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                            this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                            dtWpborrow = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                            ViewState[this.GridView2.ClientID] = dtWpborrow;
                            this.GridView2.DataBind((DataTable)ViewState[this.GridView2.ClientID], Anew, LockPageNum);

                        }
                        else //收文號有值
                        {
                            wpinno = this.TxtWPINNO.Text.Trim();

                            where = string.Format("And WP.WPINNO ='{0}' And WP.RECEIVER ='{1}' AND WP.REDATE IS NULL AND ((fb.chk='Y' And wp.viewtype =2) or(fb.chk='N' And wp.viewtype =1)) ", wpinno, strUserID);

                            strSql = this.Select.FileQuery(where);
                            this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                            this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;
                            Session["FileWpborrowQuery"] = strSql;
                            dtWpborrow = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                            if (dtWpborrow.Rows.Count == 0)
                                this.ShowMessage("目前查詢沒有任何資料", MessageMode.INFO);
                            else
                            {
                                ViewState[this.GridView2.ClientID] = dtWpborrow;
                                this.GridView2.DataBind((DataTable)ViewState[this.GridView2.ClientID], Anew, LockPageNum);
                            }
                        }

                    }

                }

            }
            catch (System.Exception ex)
            {
                this.ShowMessage(ex);
            }
            finally
            {
                this.DBConn.Dispose(); 
                this.DBConn = null;
            }
        }
        #endregion

        #region GridView2_Sorting()
        protected void GridView2_Sorting(object sender, GridViewSortEventArgs e)
        {
            e.Sorting(sender);

            this.DataBind(false, true);
        }
        #endregion

        #region GridView1_PageIndexChanging()
        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            e.PageIndexChanging(sender);

            this.DataBind(false, false);
        }
        #endregion

    }
}