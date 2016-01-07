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
    public partial class ReservationBorrowPrint : PageUtility
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadPage(true);
        }

        protected void BtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                this.InitSQL();

                #region Monitor
                string userIP = this.Request.ServerVariables["REMOTE_ADDR"].ToString();

                this.MonitorLog.LogMonitor(string.Empty, this.UserInfo.UserName, this.UserInfo.RealName, userIP, Monitor.MSGID.WDA08, string.Empty);
                #endregion
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }
        }

        #region InitSQL()
        private void InitSQL()
        {
            string strSql = string.Empty;
            string where = string.Empty;
            string strWhere = string.Empty;

            try
            {
                strWhere += string.Format("And wb.Prtflag = '{0}' And wb.viewtype = '{1}'\n", "F", "1");

                switch (this.ddlBorrowType.SelectedValue)
                {
                    case "1":
                        //昨日的1800~今日0930
                        strWhere += string.Format("And wb.Transt > To_Date('{0} 18:00:00','YYYY/MM/DD HH24:MI:SS') And wb.Transt <= To_Date('{1} 09:30:00','YYYY/MM/DD HH24:MI:SS')\n",
                            DateTime.Today.AddDays(-1).ToString("yyyy/MM/dd"),
                            DateTime.Today.ToString("yyyy/MM/dd"));
                        break;
                    case "2":
                        //今日0930~今日1100
                        strWhere += string.Format("And wb.Transt > To_Date('{0} 09:30:00','YYYY/MM/DD HH24:MI:SS') And wb.Transt <= To_Date('{0} 11:00:00','YYYY/MM/DD HH24:MI:SS')\n",
                            DateTime.Today.ToString("yyyy/MM/dd"));
                        break;
                    case "3":
                        //今日1100~今日1430
                        strWhere += string.Format("And wb.Transt > To_Date('{0} 11:00:00','YYYY/MM/DD HH24:MI:SS') And wb.Transt <= To_Date('{0} 14:30:00','YYYY/MM/DD HH24:MI:SS')\n",
                            DateTime.Today.ToString("yyyy/MM/dd"));
                        break;
                    case "4":
                        //今日1430~今日1600
                        strWhere += string.Format("And wb.Transt > To_Date('{0} 14:30:00','YYYY/MM/DD HH24:MI:SS') And wb.Transt <= To_Date('{0} 16:00:00','YYYY/MM/DD HH24:MI:SS')\n",
                            DateTime.Today.ToString("yyyy/MM/dd"));
                        break;
                    case "5":
                        //今日1600~今日1800
                        strWhere += string.Format("And wb.Transt > To_Date('{0} 16:00:00','YYYY/MM/DD HH24:MI:SS') And wb.Transt <= To_Date('{0} 18:00:00','YYYY/MM/DD HH24:MI:SS')\n",
                            DateTime.Today.ToString("yyyy/MM/dd"));
                        break;
                }

                strSql = this.Select.WpborrowPrint(strWhere);

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                Session["ReservationBorrow"] = strSql;

                //if (dt.Rows.Count == 0)
                //{
                //    this.ShowMessage("目前查詢沒有任何資料", MessageMode.INFO);
                //}
                //else
                //{
                //    Session["ReservationBorrow"] = strSql;
                //}
            }   //
            catch (System.Exception ex) { this.ShowMessage(ex.Message); }
            finally
            {
                if (this.DBConn != null) { this.DBConn.Dispose(); this.DBConn = null; }
            }
        }
        #endregion
    }
}