using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WDA.Class;

namespace WDA
{
    public partial class DulyAdjustedAdd : PageUtility
    {
        #region Page_Load()
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadPage(true);
            int rePage = this.Request.QueryString["RePage"] != null ? Convert.ToInt16(this.Request.QueryString["RePage"].Trim()) : 0;

            try
            {
                if (rePage == 1)
                {
                    this.ShowMessage("新增成功", MessageMode.INFO);
                    rePage = 0;
                }

                if (!IsPostBack)
                {
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
            string strSql = string.Empty;
            Hashtable ht = new Hashtable();

            try
            {
                #region 新增FILEBORO

                ht.Clear();
                ht.Add("WPINNO", this.txtWpinno.Text.Trim().Replace(StringFormatException.Mode.Sql));
                //ht.Add("WPINDATE", this.lblWpindate.Text);
                //ht.Add("WPOUTNO", this.lblWpoutNo.Text);
                //ht.Add("WPOUTDATE", this.lblWpoutdate.Text);
                //ht.Add("CASENO", this.lblCaseno.Text);
                //ht.Add("APPLKIND", this.lblApplkind.Text);
                //ht.Add("COMMNAME", this.lblCommname.Text);
                //ht.Add("RECEIVER", this.UserInfo.UserName);
                //ht.Add("TRANST", this.lblTranst.Text);
                //ht.Add("CHK", "N");
                //ht.Add("GETIME", this.lblGetime.Text);
                ht.Add("WORKERID", this.UserInfo.RealName);

                strSql = this.Insert.FILEBORO(ht);

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                int result = this.DBConn.GeneralSqlCmd.ExecuteNonQuery(strSql);

                if (result < 1) throw new Exception("調妥新增失敗");

                #endregion

                #region Monitor
                string wpinno = string.Empty;

                if (!string.IsNullOrEmpty(this.txtWpinno.Text.Trim()))
                {
                    wpinno = this.txtWpinno.Text.Trim().Replace(StringFormatException.Mode.Sql);
                }

                string userIP = this.Request.ServerVariables["REMOTE_ADDR"].ToString();

                this.MonitorLog.LogMonitor(wpinno, this.UserInfo.UserName, this.UserInfo.RealName, userIP, Monitor.MSGID.WDA09, string.Empty);
                #endregion
              
            }
            catch (System.Exception ex)
            {
                this.ShowMessage(ex);
            }
            finally
            {
                this.DBConn.Dispose(); this.DBConn = null;
            }

            string strUrl = string.Format("DulyAdjustedAdd.aspx?RePage=1");

            Response.Redirect(strUrl, false);
        }
        #endregion

        #region BtnClear_Click()
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(this.Request.Url.AbsoluteUri);
        } 
        #endregion

        #region BtnScan_Click()
        protected void BtnScan_Click(object sender, EventArgs e)
        {
            string wpinno = this.txtWpinno.Text.Trim().Replace(StringFormatException.Mode.Sql);

            string strUrl = string.Format("ActiveXScan.aspx?caseSet=-1&wpinno={0}", wpinno);
            string sScript = string.Format("window.open('{0}');", strUrl);

            System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(System.Web.UI.Page), "BtnScan_Click", sScript, true);
        } 
        #endregion
    }
}