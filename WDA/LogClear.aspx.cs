using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WDA.Class;

namespace WDA
{
    public partial class LogClear : PageUtility
    {
        #region Page_Load()
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadPage(true);
        }
        #endregion

        #region BtnOK_Click()
        protected void BtnOK_Click(object sender, EventArgs e)
        {
            string where = string.Empty;

            try
            {
                string startTime = this.txtCreateTime.Text.Trim().Replace(StringFormatException.Mode.Sql);//開始日期
                string endTime = this.txtEndTime.Text.Trim().Replace(StringFormatException.Mode.Sql);//結束日期

                endTime = DateTime.Parse(endTime).AddDays(1).AddSeconds(-1).ToString("yyyy/MM/dd HH:mm:ss");

                where = string.Format("TransDateTime Between '{0}' and '{1}'", startTime, endTime);

                this.DBConn.GeneralSqlCmd.Delete("LogTable", where);

                this.ShowMessage("刪除成功", MessageMode.INFO);
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

        #region BtnClear_Click()
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(this.Request.Url.AbsoluteUri);
        }
        #endregion
    }
}