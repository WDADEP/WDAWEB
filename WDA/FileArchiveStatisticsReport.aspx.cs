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
    public partial class FileArchiveStatisticsReport : GridViewUtility
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
                    //ADD BY RICHARD 20160818
                    this.GetFileUserList();
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex);
            }
        }
        #endregion

        #region BtnOK_Click()
        protected void BtnOK_Click(object sender, EventArgs e)
        {
            try
            {
                this.DataBind(true, false);

                #region Monitor
                string wpinno = string.Empty;

                if (!string.IsNullOrEmpty(this.txtBarcodeValue.Text.Trim()))
                {
                    wpinno = this.txtBarcodeValue.Text.Trim().Replace(StringFormatException.Mode.Sql);
                }

                string userIP = this.Request.ServerVariables["REMOTE_ADDR"].ToString();

                //MODIFY BY RICHARD 20160407
                this.MonitorLog.LogMonitor(wpinno, this.UserInfo.UserName, this.UserInfo.RealName, userIP, Monitor.MSGID.WDA04, "統計報表");
                #endregion
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex);
            }
        }
        #endregion

        #region BtnPrint_Click
        protected void BtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                //this.InitSQL();
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }
        }
        #endregion

        #region Private Method

        #region DataBind()
        /// <summary>
        /// 資料繫結
        /// </summary>
        /// <param name="Anew">是否重新撈資料</param>
        /// <param name="LockPageNum">是否保留目前頁數</param>
        private void DataBind(bool Anew, bool LockPageNum)
        {
            string strSql = string.Empty, wptransWhere = string.Empty;

            // Added by Luke 2016/09/12
            string strDetailSql = string.Empty;
            string whereDetail = string.Empty;

            DataTable dt = null;
            try
            {
                if (Anew)
                {
                    string barcodeValue = this.txtBarcodeValue.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                    string wpoutNo = this.txtWpoutNo.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                    string fileNo = this.txtFileNo.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                    string fileStartDate = this.txtFileScanStartDate.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                    string fileEndDate = this.txtFileScanEndDate.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                    string keepYr = this.txtKeepYr.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                    string boxNoS = this.txtBoxNoS.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                    string boxNoE = this.txtBoxNoE.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                    string onFile = this.ddlOnFile.SelectedValue.Trim().Replace(StringFormatException.Mode.Sql).Trim();

                    if (barcodeValue.Length > 0)
                    {
                        wptransWhere += string.Format("And wp.wpinno = '{0}'\n", barcodeValue);
                    }

                    if (wpoutNo.Length > 0)
                    {
                        wptransWhere += string.Format("And wp.WpoutNo = '{0}'\n", wpoutNo);
                    }

                    if (fileNo.Length > 0)
                    {
                        wptransWhere += string.Format("And wp.FileNo Like '{0}%'\n", fileNo);
                    }

                    if (fileStartDate.Length > 0 && fileEndDate.Length > 0) 
                    {
                        wptransWhere += string.Format("And wp.FileDate >= '{0}'\n", fileStartDate.Replace("/", string.Empty));

                        wptransWhere += string.Format("And wp.FileDate <= '{0}'\n", fileEndDate.Replace("/", string.Empty)); 
                    }

                    if (keepYr.Length > 0)
                    {
                        wptransWhere += string.Format("And wp.KeepYr = '{0}'\n", keepYr);
                    }

                    if (boxNoS.Length > 0 && boxNoE.Length > 0)
                    {
                        wptransWhere += string.Format("And wp.BoxNo >= '{0}' And wp.BoxNo <='{1}'\n", boxNoS, boxNoE);
                    }

                    if (onFile != "選擇人員")
                    {
                        wptransWhere += string.Format("And wt.RECEIVER = N'{0}'\n", onFile);
                    }

                    //ADD BY RICHARD 20160818
                    if (this.radScanFile.SelectedValue.Trim().Equals("1"))
                    {
                        wptransWhere += string.Format(" AND bt.BARCODEVALUE is not null ");
                    }
                    else if (this.radScanFile.SelectedValue.Trim().Equals("0"))
                    {
                        wptransWhere += string.Format(" AND bt.BARCODEVALUE is null ");
                    }

                    // Added by Luke 2016/09/12
                    whereDetail = wptransWhere;        // the same filter

                    // Added by Luke 2016/09/12
                    whereDetail += "  ORDER BY wt.RECEIVER, bt.BARCODEVALUE, wp.WPINNO ";

                    strSql = this.Select.FileArchiveStatisticsReport(wptransWhere);

                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                    // Added by Luke 2016/09/12
                    strDetailSql = this.Select.FileArchiveStatisticsDetail(whereDetail);
                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strDetailSql);

                    // Added by Luke 2016/09/12
                    Session["FileArchiveStatists"] = strSql;
                    Session["FileArchiveStatistsDetail"] = strDetailSql; // used for printing report

                    this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                    dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                    if (dt.Rows.Count == 0)
                    {
                        this.ShowMessage("目前查詢沒有任何資料", MessageMode.INFO);
                        ViewState[this.GridView1.ClientID] = dt;
                        this.HiddenShowPanel.Value = "false";
                        return;
                    }
                    else
                    {
                        ViewState[this.GridView1.ClientID] = dt;
                        this.HiddenShowPanel.Value = "true";
                    }
                }
                this.GridView1.DataBind((DataTable)ViewState[this.GridView1.ClientID], Anew, LockPageNum, this.lblTotalPage_GridView1, this.lblPage_GridView1, null);
            }
            catch (System.Exception ex) { this.ShowMessage(ex); }
            finally
            {
                if (dt != null) { dt.Dispose(); dt = null; }

                if (this.DBConn != null) { this.DBConn.Dispose(); this.DBConn = null; }
            }
        }
        #endregion

        #endregion

        #region GridView Events

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

        #region GridView1_RowDataBound
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.RowDataBound();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                switch (e.Row.Cells[4].Text)
                {
                    case "0": e.Row.Cells[4].Text = "無";
                        break;
                    default: e.Row.Cells[4].Text = "有";
                        break;
                }
            }

        }
        #endregion

        #endregion

        #region GetFileUserList
        /// <summary>
        /// 取得檔案室人員列表
        /// </summary>
        private void GetFileUserList()
        {
            string strSql = string.Empty;

            DataTable dt = null;
            try
            {
                //ADD 增加部門別判斷
                strSql = this.Select.UserTable(" AND DEPTID=10 AND USERSTATUS=0 \n");

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                DataRow defaultRow = dt.NewRow();
                defaultRow["RealName"] = "選擇人員";
                dt.Rows.Add(defaultRow);

                dt.DefaultView.Sort = "UserID";
                this.ddlOnFile.DataSource = dt;

                this.ddlOnFile.DataTextField = "RealName";
                this.ddlOnFile.DataValueField = "RealName";

                this.ddlOnFile.DataBind();

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

    }
}