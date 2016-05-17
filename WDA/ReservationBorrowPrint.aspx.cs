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

                this.MonitorLog.LogMonitor(string.Empty, this.UserInfo.UserName, this.UserInfo.RealName, userIP, Monitor.MSGID.WDA09, string.Empty);
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
                strWhere += string.Format("And wb.Prtflag In('{0}','{1}') And wb.viewtype = '{2}'\n", "F", "P","1");

                string startTime = this.txtScanCreateTime.Text.Trim().Replace(StringFormatException.Mode.Sql);//開始日期
                string endTime = this.txtScanEndTime.Text.Trim().Replace(StringFormatException.Mode.Sql);//結束日期

                //endTime = DateTime.Parse(endTime).AddDays(1).AddSeconds(-1).ToString("yyyy/MM/dd HH:mm:ss");

                where += string.Format(" And ct.CreateTime Between '{0}' And '{1}'", startTime, endTime);

                switch (this.ddlCreateTime.SelectedValue)
                {
                    case "1":
                        switch (this.ddlEndTime.SelectedValue)
                        {
                            case "1":
                                strWhere += string.Format("And wb.APPROVEDATE >= To_Date('{0} 09:30:00','YYYY/MM/DD HH24:MI:SS') And wb.APPROVEDATE <= To_Date('{1} 09:30:00','YYYY/MM/DD HH24:MI:SS')\n",
                          startTime, endTime);
                                break;
                            case "2":
                                strWhere += string.Format("And wb.APPROVEDATE >= To_Date('{0} 09:30:00','YYYY/MM/DD HH24:MI:SS') And wb.APPROVEDATE <= To_Date('{1} 11:00:00','YYYY/MM/DD HH24:MI:SS')\n",
                          startTime, endTime);
                                break;
                            case "3":
                                strWhere += string.Format("And wb.APPROVEDATE >= To_Date('{0} 09:30:00','YYYY/MM/DD HH24:MI:SS') And wb.APPROVEDATE <= To_Date('{1} 14:30:00','YYYY/MM/DD HH24:MI:SS')\n",
                          startTime, endTime);
                          break;
                            case "4":
                          strWhere += string.Format("And wb.APPROVEDATE >= To_Date('{0} 09:30:00','YYYY/MM/DD HH24:MI:SS') And wb.APPROVEDATE <= To_Date('{1} 16:00:00','YYYY/MM/DD HH24:MI:SS')\n",
                          startTime, endTime);
                                break;
                        }
                        break;
                    case "2":
                        switch (this.ddlEndTime.SelectedValue)
                        {
                            case "1":
                                strWhere += string.Format("And wb.APPROVEDATE >= To_Date('{0} 11:00:00','YYYY/MM/DD HH24:MI:SS') And wb.APPROVEDATE <= To_Date('{1} 09:30:00','YYYY/MM/DD HH24:MI:SS')\n",
                          startTime, endTime);
                                break;
                            case "2":
                                strWhere += string.Format("And wb.APPROVEDATE >= To_Date('{0} 11:00:00','YYYY/MM/DD HH24:MI:SS') And wb.APPROVEDATE <= To_Date('{1} 11:00:00','YYYY/MM/DD HH24:MI:SS')\n",
                          startTime, endTime);
                                break;
                            case "3":
                                strWhere += string.Format("And wb.APPROVEDATE >= To_Date('{0} 11:00:00','YYYY/MM/DD HH24:MI:SS') And wb.APPROVEDATE <= To_Date('{1} 14:30:00','YYYY/MM/DD HH24:MI:SS')\n",
                      startTime, endTime);
                                break;
                            case "4":
                                strWhere += string.Format("And wb.APPROVEDATE >= To_Date('{0} 11:00:00','YYYY/MM/DD HH24:MI:SS') And wb.APPROVEDATE <= To_Date('{1} 16:00:00','YYYY/MM/DD HH24:MI:SS')\n",
                                startTime, endTime);
                                break;
                        }
                        break;
                    case "3":
                        switch (this.ddlEndTime.SelectedValue)
                        {
                            case "1":
                                strWhere += string.Format("And wb.APPROVEDATE >= To_Date('{0} 14:30:00','YYYY/MM/DD HH24:MI:SS') And wb.APPROVEDATE <= To_Date('{1} 09:30:00','YYYY/MM/DD HH24:MI:SS')\n",
                          startTime, endTime);
                                break;
                            case "2":
                                strWhere += string.Format("And wb.APPROVEDATE >= To_Date('{0} 14:30:00','YYYY/MM/DD HH24:MI:SS') And wb.APPROVEDATE <= To_Date('{1} 11:00:00','YYYY/MM/DD HH24:MI:SS')\n",
                          startTime, endTime);
                                break;
                            case "3":
                                strWhere += string.Format("And wb.APPROVEDATE >= To_Date('{0} 14:30:00','YYYY/MM/DD HH24:MI:SS') And wb.APPROVEDATE <= To_Date('{1} 14:30:00','YYYY/MM/DD HH24:MI:SS')\n",
                      startTime, endTime);
                                break;
                            case "4":
                                strWhere += string.Format("And wb.APPROVEDATE >= To_Date('{0} 14:30:00','YYYY/MM/DD HH24:MI:SS') And wb.APPROVEDATE <= To_Date('{1} 16:00:00','YYYY/MM/DD HH24:MI:SS')\n",
                                startTime, endTime);
                                break;
                        }
                        break;
                    case "4":
                        switch (this.ddlEndTime.SelectedValue)
                        {
                            case "1":
                                strWhere += string.Format("And wb.APPROVEDATE >= To_Date('{0} 16:00:00','YYYY/MM/DD HH24:MI:SS') And wb.APPROVEDATE <= To_Date('{1} 09:30:00','YYYY/MM/DD HH24:MI:SS')\n",
                          startTime, endTime);
                                break;
                            case "2":
                                strWhere += string.Format("And wb.APPROVEDATE >= To_Date('{0} 16:00:00','YYYY/MM/DD HH24:MI:SS') And wb.APPROVEDATE <= To_Date('{1} 11:00:00','YYYY/MM/DD HH24:MI:SS')\n",
                          startTime, endTime);
                                break;
                            case "3":
                                strWhere += string.Format("And wb.APPROVEDATE >= To_Date('{0} 16:00:00','YYYY/MM/DD HH24:MI:SS') And wb.APPROVEDATE <= To_Date('{1} 14:30:00','YYYY/MM/DD HH24:MI:SS')\n",
                      startTime, endTime);
                                break;
                            case "4":
                                strWhere += string.Format("And wb.APPROVEDATE >= To_Date('{0} 16:00:00','YYYY/MM/DD HH24:MI:SS') And wb.APPROVEDATE <= To_Date('{1} 16:00:00','YYYY/MM/DD HH24:MI:SS')\n",
                                startTime, endTime);
                                break;
                        }
                        break;
                    //case "5":
                    //    //今日1600~今日1800
                    //    strWhere += string.Format("And wb.Transt > To_Date('{0} 16:00:00','YYYY/MM/DD HH24:MI:SS') And wb.Transt <= To_Date('{0} 18:00:00','YYYY/MM/DD HH24:MI:SS')\n",
                    //        DateTime.Today.ToString("yyyy/MM/dd"));
                    //    break;
                }

                //strWhere += string.Format("And wb.Transt > To_Date('{0} 16:00:00','YYYY/MM/DD HH24:MI:SS') And wb.Transt <= To_Date('{1} 16:00:00','YYYY/MM/DD HH24:MI:SS')\n",
                //    DateTime.Today.AddDays(-1).ToString("yyyy/MM/dd"), DateTime.Today.AddDays(1).ToString("yyyy/MM/dd"));

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