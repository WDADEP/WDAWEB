using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WDA.Class;

namespace WDA
{
    public partial class SystemSet : PageUtility
    {
        #region Page_Load()
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadPage(true);

            try
            {
                if (!IsPostBack)
                {
                    this.InitInfo();
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex);
            }
        }
        #endregion

        #region InitInfo()
        private void InitInfo()
        {
            try
            {
                #region TextBox
                this.txtImageIJZip.Text = this.GetSystem("ImageIJZip");
                this.txtImageIThreshold.Text = this.GetSystem("ImageIThreshold");
                this.txtImageIType.Text = this.GetSystem("ImageIType");
                this.txtExportFileFormat.Text = this.GetSystem("ExportFileFormat");
                this.txtImportFileFormat.Text = this.GetSystem("ImportFileFormat");
                this.txtISize.Text = this.GetSystem("ImageISize");
                this.txtServiceURL.Text = this.GetSystem("ServiceURL");
                this.txtViewerClassID.Text = this.GetSystem("ViewerClassID");
                this.txtViewerVersion.Text = this.GetSystem("ViewerVersion");
                this.txtViewerUploadServerURL.Text = this.GetSystem("ViewerUploadServerURL");
                #endregion

                #region DDList
                this.GetddlPageSize();
                #endregion
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex);
            }
        }
        #endregion

        #region GetddlPageSize()
        private void GetddlPageSize()
        {
            this.ddlPageSize.Items.Clear();

            List<string[]> statusList = new List<string[]>();

            for (int i = 10; i < 31; i++)
            {
                statusList.Add(new string[] { i.ToString(), i.ToString() });
            }

            for (int i = 0; i < statusList.Count; i++)
            {
                ListItem item = new ListItem(statusList[i][0], statusList[i][1]);

                if (item.Value.ToString().Equals(this.GetSystem("PageSize"))) item.Selected = true;

                item.Attributes.Add("style", string.Format("color:#{0}", statusList[i][1] == this.GetSystem("PageSize") ? "0000ff" : "000000"));

                this.ddlPageSize.Items.Add(item);
            }
        }
        #endregion

        #region UpdateDate()
        private void UpdateDate(Control Ctl)
        {
            string strSql = string.Empty;

            try
            {
                //OleDbCommand command = (OleDbCommand)this.DBConn.GeneralSqlCmd.Command;

                if (Ctl is TextBox)
                {
                    strSql = this.Update.SystemTable(((TextBox)Ctl).ID.TrimStart(new char[] { 't', 'x', 't' }), ((TextBox)Ctl).Text);

                    //command.Parameters.Clear();
                    //command.Parameters.Add(new OleDbParameter("systemname", OleDbType.VarChar)).Value = ((TextBox)Ctl).ID.TrimStart(new char[] { 't', 'x', 't' });
                    //command.Parameters.Add(new OleDbParameter("systemcomment", OleDbType.VarChar)).Value = ((TextBox)Ctl).Text;
                }
                else if (Ctl is DropDownList)
                {
                    strSql = this.Update.SystemTable(((DropDownList)Ctl).ID.TrimStart(new char[] { 'd', 'd', 'l' }), ((DropDownList)Ctl).SelectedValue.Trim());

                    //command.Parameters.Clear();
                    //command.Parameters.Add(new OleDbParameter("systemname", OleDbType.VarChar)).Value = ((DropDownList)Ctl).ID.TrimStart(new char[] { 'd', 'd', 'l' });
                    //command.Parameters.Add(new OleDbParameter("systemcomment", OleDbType.VarChar)).Value = ((DropDownList)Ctl).SelectedValue.Trim();
                }

                //strSql = this.Update.SystemTable();

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                int result = this.DBConn.GeneralSqlCmd.ExecuteNonQuery(strSql);

                if (result < 1) throw new Exception("更新系統資料失敗");
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                this.DBConn.Dispose(); this.DBConn = null;
            }
        }
        #endregion

        #region UpdateDate()
        private void UpdateDate(ListItem Litem)
        {
            string strSql = string.Empty;
            string systemComment = string.Empty;

            try
            {
                systemComment = Litem.Selected == true ? "Y" : "N";

                //OleDbCommand command = (OleDbCommand)this.DBConn.GeneralSqlCmd.Command;

                //command.Parameters.Clear();
                //command.Parameters.Add(new OleDbParameter("systemName", OleDbType.VarChar)).Value = Litem.Value.Trim();
                //command.Parameters.Add(new OleDbParameter("systemName", OleDbType.VarChar)).Value = systemComment;

                strSql = this.Update.SystemTable(Litem.Value.Trim(), systemComment);

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                int result = this.DBConn.GeneralSqlCmd.ExecuteNonQuery(strSql);

                if (result < 1) throw new Exception("更新系統資料失敗");

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

        #region BtnClear_Click()
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            this.ReloadPage();
        }
        #endregion

        #region BtnOK_Click()
        protected void BtnOK_Click(object sender, EventArgs e)
        {
            string[] arraySystemNames = this.HiddenSystemNames.Value.Trim().Split(' ');

            bool check = true;

            try
            {
                ContentPlaceHolder mainContent = (ContentPlaceHolder)Master.FindControl("MainContent");

                for (int i = 0; i < arraySystemNames.Length; i++)
                {
                    if (!string.IsNullOrEmpty(arraySystemNames[i].ToString()))
                    {
                        object item = arraySystemNames[i].Substring(12, arraySystemNames[i].Length - 12);

                        foreach (Control ctl in mainContent.Controls)
                        {
                            if (ctl.ID == item.ToString())
                            {
                                this.UpdateDate(ctl);
                            }

                            int startIndex = item.ToString().IndexOf('_') == -1 ? 0 : item.ToString().IndexOf('_');

                            if (ctl.ID == item.ToString().Remove(startIndex) && check)
                            {
                                check = false;

                                foreach (ListItem listItem in ((CheckBoxList)ctl).Items)
                                {
                                    this.UpdateDate(listItem);
                                }
                            }
                        }
                    }
                }

                this.ShowMessage("更新系統資料成功", MessageMode.INFO);

                this.HiddenSystemNames.Value = string.Empty;

                PageUtility.dv_Sys = null;
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex);
            }
        }
        #endregion
    }
}