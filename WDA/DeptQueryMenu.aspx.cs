using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WDA.Class;

namespace WDA
{
    public partial class DeptQueryMenu : GridViewUtility
    {
        #region Page_Load()
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadPage(true);
            try
            {
                if (!IsPostBack)
                {
                    this.HiddenShowPanel.Value = "false";

                    this.GridView1.PageSize = Convert.ToInt32(this.GetSystem("PageSize"));

                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
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

        #region private method
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
                    string userDepName = string.Empty; 

                    userDepName = this.TxtDeptName.Text.Trim().Replace(StringFormatException.Mode.Sql);

                    strSql = this.Select.DEPT(userDepName);

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

        #region GridView

        #region GridView1_RowCreated
        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            e.RowVisible(sender, new int[] { 3,4 });
        }
        #endregion

        #region GridView1_PageIndexChanging
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            e.PageIndexChanging(sender);

            this.DataBind(false, false);

        }
        #endregion

        #region GridView1_Sorting
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

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnStop = (ImageButton)e.Row.Cells[1].Controls[1];

                this.Confirm(btnStop, e.Row.Cells[3].Text == "99" ? ConfirmMode.Start : ConfirmMode.Stop);

                btnStop.ImageUrl = e.Row.Cells[3].Text == "99" ? "~/Images/stop02.gif" : "~/Images/stop.gif";

            }

        }
        #endregion

        protected void BtnAdd_Click(object sender, EventArgs e)
        {

        }


        #endregion

        protected void BtnClear_Click(object sender, EventArgs e)
        {
            this.ReloadPage();
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

                string deptStatus = gv.Rows[rowIndex].Cells[3].Text.Trim();
                string strdeptid =gv.Rows[rowIndex].Cells[4].Text.Trim();


                if (e.CommandName == "Stop")
                {
                    #region Stop

                    deptStatus = deptStatus == "99" ? "0" : "99";

                    strSql = this.Update.DEPTSTATUS(strdeptid,deptStatus);

                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                    int result = this.DBConn.GeneralSqlCmd.ExecuteNonQuery(strSql);

                    if (result < 1) this.ShowMessage(deptStatus == "99" ? "停用失敗" : "啟用失敗");
                    else
                    {
                        this.ShowMessage(deptStatus == "99" ? "停用成功" : "啟用成功", MessageMode.INFO);

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

        #region ITImageListBtnChang_Click()
        protected void ITImageListBtnChang_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gridViewRow = (GridViewRow)((ImageButton)sender).NamingContainer;
            int DataIndex = this.GridView1.PageSize * this.GridView1.PageIndex + gridViewRow.RowIndex;


            try
            {
                #region Update

                #region Update DEPT

                string deptStatus = gridViewRow.Cells[3].Text.Trim();
                string strdeptid = gridViewRow.Cells[4].Text.Trim();
                string strDeptName = ((TextBox)gridViewRow.FindControl("TxtDEPTNAME")).Text;



                string strSql = this.Update.DEPT(strdeptid, strDeptName, deptStatus);

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                int result = this.DBConn.GeneralSqlCmd.ExecuteNonQuery(strSql);

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

        #region ImageBtnEdit_Click()
        protected void ImageBtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gridViewRow = (GridViewRow)((ImageButton)sender).NamingContainer;
            GridView gv = (GridView)gridViewRow.NamingContainer;

            gv.EditIndex = gridViewRow.RowIndex;
            this.DataBind(true, false);
        }
        #endregion


    }
}