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
    public partial class RoleQueryMenu : GridViewUtility
    {
        #region Page_Load()
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadPage(true);

            int rePage = this.Request.QueryString["RePage"] != null ? Convert.ToInt16(this.Request.QueryString["RePage"].Trim()) : 0;
            try
            {
                if (!IsPostBack)
                {
                    if (rePage == 1)
                    {
                        this.ShowMessage("刪除成功", MessageMode.INFO);
                    }

                    this.HiddenShowPanel.Value = "false";
                    this.InitInfo();
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }
        }

        #endregion

        #region Private Method

        #region InitInfo()
        /// <summary>
        /// 初始化資訊
        /// </summary>
        private void InitInfo()
        {
            this.GetDDLRoleName();
        }
        #endregion

        #region GetDDLRoleName()
        /// <summary>
        /// 取得角色資訊
        /// </summary>
        private void GetDDLRoleName()
        {
            string strSql = string.Empty;

            DataTable dt = null;
            try
            {
                strSql = this.Select.RoleTable();

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                this.DDLRoleName.DataSource = dt;

                this.DDLRoleName.DataTextField = "RoleName";
                this.DDLRoleName.DataValueField = "RoleID";

                this.DDLRoleName.DataBind();

                dt.Dispose(); dt = null;
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }
            finally
            {
                this.DBConn.Dispose(); this.DBConn = null;
            }
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

            DataTable dt = null;
            try
            {
                if (Anew)
                {
                    where = string.Format("And RoleID = {0}",this.DDLRoleName.SelectedValue);

                    strSql = this.Select.RoleTable(where);

                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                    this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                    dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql, this.DBSqlParamList);

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

                    this.GridView1.DataBind((DataTable)ViewState[this.GridView1.ClientID], Anew, LockPageNum, this.lblTotalPage_GridView1, this.lblPage_GridView1, null);
                }
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

        #endregion Private Method

        #region BtnAdd_Click()
        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            string strUrl = string.Format("RoleAdd.aspx");

            this.Response.Redirect(strUrl, false);
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
            this.ReloadPage();
        }
        #endregion

        #region GridView1_PageIndexChanging()
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            e.PageIndexChanging(sender);

            this.DataBind(false, false);
        }
        #endregion

        #region GridView1_RowCommand
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView gv = (GridView)sender;

            string strSql = string.Empty;
            int roleID = 0; bool check = false;

            try
            {
                if (e.CommandName != "Delete" && e.CommandName != "Modify") return;

                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;

                string strRoleID = gv.Rows[rowIndex].Cells[2].Text.Trim();

                if (e.CommandName == "Modify")
                {
                    string strUrl = string.Format("RoleEdit.aspx?RoleID={0}", strRoleID);

                    Response.Redirect(strUrl, false);
                }
                else if (e.CommandName == "Delete")
                {
                    try
                    {
                        strSql = this.Delete.RoleTable(strRoleID);

                        this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                        this.DBConnTransac.GeneralSqlCmd.Command.CommandTimeout = 90;

                        int result = this.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql);

                        if (result < 1) throw new Exception("角色刪除失敗");

                        int.TryParse(strRoleID, out roleID);

                        strSql = this.Delete.RolePrivilege(roleID);

                        this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                        this.DBConnTransac.GeneralSqlCmd.Command.CommandTimeout = 90;

                        result = this.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql);

                        //if (result < 1) throw new Exception("角色權限刪除失敗");

                        strSql = this.Delete.ViewerPrivilege(roleID);

                        this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                        this.DBConnTransac.GeneralSqlCmd.Command.CommandTimeout = 90;

                        result = this.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql);

                        //if (result < 1) throw new Exception("Viewer權限刪除失敗");

                        this.DBConnTransac.GeneralSqlCmd.Transaction.Commit();

                        check = true;
                    }
                    catch (Exception ex)
                    {
                        if (this.DBConnTransac.GeneralSqlCmd.Transaction != null) this.DBConnTransac.GeneralSqlCmd.Transaction.Rollback();

                        this.ShowMessage(ex.Message);
                    }
                    finally
                    {
                        this.DBConnTransac.Dispose(); this.DBConnTransac = null;
                    }
                    if (check)
                    {
                        string strUrl = string.Format("RoleQueryMenu.aspx?RePage=1");

                        Response.Redirect(strUrl, true);
                    }
                }
            }
            catch (System.Exception ex) { this.ShowMessage(ex.Message); }
        }
        #endregion

        #region GridView1_RowDataBound()
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnDownload = (ImageButton)e.Row.Cells[1].FindControl("ImageBtnDelete");

                this.Confirm(btnDownload, ConfirmMode.Delete);
            }
        }
        #endregion

        #region GridView1_Sorting()
        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            e.Sorting(sender);

            this.DataBind(false, true);
        }
        #endregion

        #region GridView1_RowCreated()
        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = (DataTable)ViewState[this.GridView1.ClientID];

                if (dt.Rows[0]["RoleID"].ToString() == "1")//管理員不給刪除
                {
                    e.RowVisible(sender, new int[] { 1 });
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }
            finally
            {
                dt.Dispose(); dt = null;
            }
        }
        #endregion
    }
}