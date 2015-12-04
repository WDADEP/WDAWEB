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
    public partial class UserQueryMenu : GridViewUtility
    {
        #region Page_Load()
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadPage(true);
            try
            {
                if (!IsPostBack)
                {
                    this.HiddenShowPanel.Value ="false";
                    //string sScript = "$('#divPanel').hide();";
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "UserQueryMenu", sScript, true);

                    this.GridView1.PageSize = Convert.ToInt32(this.GetSystem("PageSize"));

                    this.InitInfo();
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }
        }

        #endregion

        #region BtnAdd_Click()
        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            string strUrl = string.Format("UserAdd.aspx");

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

        #region Private Method

        #region DataBind()
        /// <summary>
        /// 資料繫結
        /// </summary>
        /// <param name="Anew">是否重新撈資料</param>
        /// <param name="LockPageNum">是否保留目前頁數</param>
        public void DataBind(bool Anew, bool LockPageNum)
        {
            string strSql = string.Empty;

            DataTable dt = null;
            try
            {
                if (Anew)
                {
                    string userName = string.Empty, realName = string.Empty;

                    string userDep = string.Empty; string userCorp = string.Empty;

                    userName = this.TxtUserName.Text.Trim().Replace(StringFormatException.Mode.Sql);

                    realName = this.TxtRealName.Text.Trim().Replace(StringFormatException.Mode.Sql);

                    strSql = this.Select.UsersMaintain(userName, realName);

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

        #region InitInfo()
        /// <summary>
        /// 
        /// </summary>
        private void InitInfo()
        {
            this.GetDDLRole();
        }
        #endregion

        #region GetDDLRole()
        /// <summary>
        /// 取得角色列表
        /// </summary>
        private void GetDDLRole()
        {
            string strSql = string.Empty;

            DataTable dt = null;
            try
            {
                strSql = this.Select.RoleTable();

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                DataRow defaultRow = dt.NewRow();
                defaultRow["RoleName"] = "請選擇";
                defaultRow["RoleID"] = "0";
                dt.Rows.Add(defaultRow);

                dt.DefaultView.Sort = "RoleID";
                this.DDLRole.DataSource = dt;

                this.DDLRole.DataTextField = "RoleName";
                this.DDLRole.DataValueField = "RoleID";

                this.DDLRole.DataBind();

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

        #endregion

        #region GridView

        #region GridView1_RowCreated()
        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            e.RowVisible(sender, new int[] { 5, 6, 7 });
        }
        #endregion

        #region GridView1_RowCommand()
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView gv = (GridView)sender;

            string strSql = string.Empty;
            try
            {
                if (e.CommandName != "Stop") return;

                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;

                string userStatus = gv.Rows[rowIndex].Cells[5].Text.Trim();

                string userId = gv.Rows[rowIndex].Cells[6].Text.Trim();

                if (e.CommandName == "Stop")
                {
                    #region Stop

                    userStatus = userStatus == "99" ? "0" : "99";

                    strSql = this.Update.UserMaintain_Enable(userId, userStatus);

                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                    int result = this.DBConn.GeneralSqlCmd.ExecuteNonQuery(strSql);

                    if (result < 1) this.ShowMessage(userStatus == "99" ? "停用失敗" : "啟用失敗");
                    else
                    {
                        this.ShowMessage(userStatus == "99" ? "停用成功" : "啟用成功", MessageMode.INFO);

                        this.DataBind(true, true);
                    }
                    #endregion
                }
            }
            catch (System.Exception ex) { this.ShowMessage(ex); }
            finally
            {
                this.DBConn.Dispose(); this.DBConn = null;
            }
        }
        #endregion

        #region GridView1_RowDataBound()
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.RowDataBound();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnStop = (ImageButton)e.Row.Cells[1].Controls[1];

                this.Confirm(btnStop, e.Row.Cells[5].Text == "99" ? ConfirmMode.Start : ConfirmMode.Stop);

                btnStop.ImageUrl = e.Row.Cells[5].Text == "99" ? "~/Images/stop02.gif" : "~/Images/stop.gif";

                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    string strSql = string.Empty;

                    DataTable dt = null;

                    try
                    {
                        strSql = this.Select.RoleTable();
                        this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                        DropDownList ddlRoleName = (DropDownList)e.Row.FindControl("ITDDLRole");

                        dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);
                        ddlRoleName.DataSource = dt;
                        ddlRoleName.DataTextField = "RoleName";
                        ddlRoleName.DataValueField = "RoleID";
                        ddlRoleName.DataBind();

                        DataRowView dr = e.Row.DataItem as DataRowView;
                        ddlRoleName.SelectedValue = dr["RoleID"].ToString();
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

        protected void ImageBtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gridViewRow = (GridViewRow)((ImageButton)sender).NamingContainer;
            GridView gv = (GridView)gridViewRow.NamingContainer;

            gv.EditIndex = gridViewRow.RowIndex;
            this.DataBind(true, false);
        }

        #endregion

        #region ITImageListBtnChang_Click()
        protected void ITImageListBtnChang_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gridViewRow = (GridViewRow)((ImageButton)sender).NamingContainer;
            int DataIndex = this.GridView1.PageSize * this.GridView1.PageIndex + gridViewRow.RowIndex;

            Hashtable ht = new Hashtable();

            try
            {
                #region Update

                #region Update UserTable

                ht.Clear();
                ht.Add("RealName", "'" + ((TextBox)gridViewRow.FindControl("TxtRealName")).Text + "'");
                ht.Add("RoleID", "'" + ((DropDownList)gridViewRow.FindControl("ITDDLRole")).SelectedValue + "'");
                ht.Add("TEL", "'" + ((TextBox)gridViewRow.FindControl("TxtTEL")).Text + "'");

                string where = string.Format("UserID={0}", gridViewRow.Cells[6].Text.Trim());

                int result = this.DBConn.GeneralSqlCmd.Update("UserTable", ht, where);

                if (result == 0)
                {
                    this.ShowMessage("修改失敗"); return;
                }
                #endregion

                this.ShowMessage("修改成功", MessageMode.INFO);

                #endregion
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex);
            }
            finally
            {
                ht = null;

                this.DBConn.Dispose(); this.DBConn = null;

                this.GridView1.EditIndex = -1;

                this.DataBind(true, false);
            }
        }
        #endregion

        #region ITImageBtnCancel_Click()
        protected void ITImageBtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gridViewRow = (GridViewRow)((ImageButton)sender).NamingContainer;
            GridView gv = (GridView)gridViewRow.NamingContainer;

            gv.EditIndex = -1;
            this.DataBind(true, true);
        }
        #endregion
    }
}