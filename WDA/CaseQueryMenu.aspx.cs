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
    public partial class CaseQueryMenu : GridViewUtility
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

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        #region GridView1_Sorting()
        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            e.Sorting(sender);

            this.DataBind(false, true);
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
                    if (!string.IsNullOrEmpty(this.txtWPINNO.Text.Trim()))
                    {
                        string wpinno = this.txtWPINNO.Text.Trim().Replace(StringFormatException.Mode.Sql);//開始日期

                        where += string.Format("And bt.Barcodevalue = '{0}'", wpinno);
                    }

                    if (!string.IsNullOrEmpty(this.txtUserName.Text.Trim()))
                    {
                        string userName = this.txtUserName.Text.Trim().Replace(StringFormatException.Mode.Sql);//開始日期

                        where += string.Format("And ut.UserName = '{0}'", userName);
                    }

                    if (!string.IsNullOrEmpty(txtCreateTime.Text))
                    {
                        string startTime = this.txtCreateTime.Text.Trim().Replace(StringFormatException.Mode.Sql);//開始日期
                        string endTime = this.txtEndTime.Text.Trim().Replace(StringFormatException.Mode.Sql);//結束日期

                        endTime = DateTime.Parse(endTime).AddDays(1).AddSeconds(-1).ToString("yyyy/MM/dd HH:mm:ss");

                        where += string.Format("And ct.CreateTime BETWEEN TO_DATE ('{0}', 'yyyy/mm/dd')AND TO_DATE ('{1}', 'YYYY/MM/DD HH24:MI:SS')", startTime, endTime);
                    }

                    strSql = this.Select.UploadCaseQuery(where);

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

        #region ImgBtnProduction_Click()
        protected void ImgBtnProduction_Click(object sender, ImageClickEventArgs e)
        {
             GridViewRow gridViewRow = (GridViewRow)((ImageButton)sender).NamingContainer;

            string caseID = gridViewRow.Cells[3].Text + "(*)";

            string strUrl = string.Format("ActiveXViewer.aspx?caseSet={0}", caseID);
            string sScript = string.Format("window.open('{0}');", strUrl);

            System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(System.Web.UI.Page), "ImgBtnProduction", sScript, true);
        } 
        #endregion

        #region GridView1_RowCreated()
        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            e.RowVisible(sender, new int[] { 0 });
        } 
        #endregion

        #region ImgBtnAllVersion_Click()
        protected void ImgBtnAllVersion_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gridViewRow = (GridViewRow)((ImageButton)sender).NamingContainer;

            string caseID = gridViewRow.Cells[3].Text + "(*)";

            string strUrl = string.Format("ActiveXViewer.aspx?Mode=A&caseSet={0}&UserSet={1}", caseID, this.UserInfo.UserID);
            string sScript = string.Format("window.open('{0}');", strUrl);

            System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(System.Web.UI.Page), "ImgBtnAllVersion", sScript, true);
        } 
        #endregion
    }
}