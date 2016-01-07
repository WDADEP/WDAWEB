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
    public partial class DulyAdjustedQuery : PageUtility
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

                    this.lblReceiver.Text = this.UserInfo.UserName;

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
                this.DataBind(true,false);

                #region Monitor
                string wpinno = string.Empty;

                if (!string.IsNullOrEmpty(this.txtWpinno.Text.Trim()))
                {
                    wpinno = this.txtWpinno.Text.Trim().Replace(StringFormatException.Mode.Sql);
                }

                string userIP = this.Request.ServerVariables["REMOTE_ADDR"].ToString();

                this.MonitorLog.LogMonitor(wpinno, this.UserInfo.UserName, this.UserInfo.RealName, userIP, Monitor.MSGID.WDA11, string.Empty);
                #endregion
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
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
                    if (!string.IsNullOrEmpty(txtWpinno.Text))
                    {
                        string wpinno = this.txtWpinno.Text.Trim().Replace(StringFormatException.Mode.Sql);
                        where += string.Format("And fb.Wpinno ='{0}'", wpinno);
                    }
                    if (!string.IsNullOrEmpty(txtWpindate.Text))
                    {
                        string wpindate = this.txtWpindate.Text.Trim().Replace(StringFormatException.Mode.Sql);
                        string wpindateEnd = DateTime.Parse(wpindate).AddDays(1).AddSeconds(-1).ToString("yyyy/MM/dd HH:mm:ss");

                        where += string.Format("And fb.Wpindate BETWEEN TO_DATE ('{0}', 'yyyy/mm/dd')AND TO_DATE ('{1}', 'YYYY/MM/DD HH24:MI:SS')", wpindate, wpindateEnd);
                        //where += string.Format("And fb.Wpindate ='{0}'", wpindate);
                    }
                    if (!string.IsNullOrEmpty(txtWpoutNo.Text))
                    {
                        string wpoutNo = this.txtWpoutNo.Text.Trim().Replace(StringFormatException.Mode.Sql);
                        where += string.Format("And fb.WpoutNo ='{0}'", wpoutNo);
                    }
                    if (!string.IsNullOrEmpty(txtWpoutdate.Text))
                    {
                        string wpoutdate = this.txtWpoutdate.Text.Trim().Replace(StringFormatException.Mode.Sql);
                        string wpoutdateEnd = DateTime.Parse(wpoutdate).AddDays(1).AddSeconds(-1).ToString("yyyy/MM/dd HH:mm:ss");

                        where += string.Format("And fb.wpoutdate BETWEEN TO_DATE ('{0}', 'yyyy/mm/dd')AND TO_DATE ('{1}', 'YYYY/MM/DD HH24:MI:SS')", wpoutdate, wpoutdateEnd);

                        //where += string.Format("And fb.wpoutdate ='{0}'", wpoutdate);
                    }
                    if (!string.IsNullOrEmpty(txtCaseno.Text))
                    {
                        string caseno = this.txtCaseno.Text.Trim().Replace(StringFormatException.Mode.Sql);
                        where += string.Format("And fb.Caseno ='{0}'", caseno);
                    }
                    //if (!string.IsNullOrEmpty(txtApplkind.Text))
                    if (this.ddlApplkind.SelectedIndex > 0)
                    {
                        //string applkind = this.txtApplkind.Text.Trim().Replace(StringFormatException.Mode.Sql);
                        string applkind = this.ddlApplkind.SelectedValue;
                        where += string.Format("And fb.Applkind ='{0}'", applkind);
                    }
                    if (!string.IsNullOrEmpty(txtCommname.Text))
                    {
                        string commname = this.txtCommname.Text.Trim().Replace(StringFormatException.Mode.Sql);
                        where += string.Format("And fb.Commname ='{0}'", commname);
                    }
                    //if (!string.IsNullOrEmpty(txtReceiver.Text))
                    //{
                    //    string receiver = this.txtReceiver.Text.Trim().Replace(StringFormatException.Mode.Sql);
                    //    where += string.Format("And fb.Receiver ='{0}'", receiver);
                    //}

                    where += string.Format("And fb.Receiver ='{0}'", this.UserInfo.UserName);
                    if (!string.IsNullOrEmpty(txtTranst.Text))
                    {
                        string transt = this.txtTranst.Text.Trim().Replace(StringFormatException.Mode.Sql);
                        where += string.Format("And fb.Transt ='{0}'", transt);
                    }
                    if (!string.IsNullOrEmpty(txtGetime.Text))
                    {
                        string getime = this.txtGetime.Text.Trim().Replace(StringFormatException.Mode.Sql);
                        string getimeEnd = DateTime.Parse(getime).AddDays(1).AddSeconds(-1).ToString("yyyy/MM/dd HH:mm:ss");

                        where += string.Format("And fb.Getime BETWEEN TO_DATE ('{0}', 'yyyy/mm/dd')AND TO_DATE ('{1}', 'YYYY/MM/DD HH24:MI:SS')", getime, getimeEnd);

                        //where += string.Format("And fb.Getime ='{0}'", getime);
                    }
                    if (!string.IsNullOrEmpty(txtWorkerid.Text))
                    {
                        string workerid = this.txtWorkerid.Text.Trim().Replace(StringFormatException.Mode.Sql);
                        where += string.Format("And fb.Workerid ='{0}'", workerid);
                    }

                    DataTable dt = null;

                    strSql = strSql = this.Select.CaseQuery(where);

                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                    this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                    dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                    if (dt.Rows.Count == 0)
                    {
                        this.ShowMessage("目前查詢沒有任何資料", MessageMode.INFO);
                        this.GridView1.DataBind();
                        dt.Dispose(); dt = null;
                        return;
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
            catch (System.Exception ex) { this.ShowMessage(ex); }
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

        #region GridView1_RowDataBound()
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.RowDataBound();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ImgBtnProduction = (ImageButton)e.Row.FindControl("ImgBtnProduction");

                switch (e.Row.Cells[13].Text)
                {
                    case "1": e.Row.Cells[13].Text = "紙本"; ImgBtnProduction.Enabled = false;
                        ImgBtnProduction.ImageUrl = "~/Images/stop.gif";
                        break;
                    case "2": e.Row.Cells[13].Text = "電子"; ImgBtnProduction.Enabled = true;
                        ImgBtnProduction.ImageUrl = "~/Images/view.gif";
                        break;
                    case "99": e.Row.Cells[13].Text = "其它";
                        break;
                }
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

        #region ImgBtnProduction_Click()
        protected void ImgBtnProduction_Click(object sender, ImageClickEventArgs e)
        {
            string strSql = string.Empty;
            string where = string.Empty;

            DataTable dt = null;
            string priv = "0";
            try
            {

                GridViewRow gridViewRow = (GridViewRow)((ImageButton)sender).NamingContainer;

                where = string.Format("And ct.CASEID='{0}'", gridViewRow.Cells[1].Text);

                string caseID = gridViewRow.Cells[1].Text + "(*)";

                strSql = this.Select.GetAllWpinno(where);

                dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //if (dt.Rows[i]["IMAGEPRIV"].ToString() == "1" || dt.Rows[i]["IMAGEPRIV"].ToString() == "0")
                    if (dt.Rows[i]["IMAGEPRIV"].ToString() == "1")
                    {
                        priv = "1";
                        break;
                    }

                    if (dt.Rows[i]["IMAGEPRIV"].ToString() == "2") { priv = "2561"; }
                    //priv = dt.Rows[i]["IMAGEPRIV"].ToString().CompareTo(priv) > 0 ? dt.Rows[i]["IMAGEPRIV"].ToString() : priv;
                }

                string strUrl = string.Format("ActiveXViewer.aspx?caseSet={0}&Mode=D&PriID={1}", caseID, priv);
                string sScript = string.Format("window.open('{0}');", strUrl);

                System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(System.Web.UI.Page), "ImageBtnQuery", sScript, true);
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex);
            }
            finally
            {
                this.DBConn.Dispose(); this.DBConn = null;
            }


        } 
        #endregion

        #region GridView1_RowCreated()
        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            e.RowVisible(sender, new int[] { 1 });
        } 
        #endregion
    }
}