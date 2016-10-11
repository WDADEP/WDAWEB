using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WDA.Class;

namespace WDA
{
    public partial class FileQuery : PageUtility
    {
        #region Page_Load()
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadPage(true);

            try
            {
                if (!IsPostBack)
                {
                    string sScript = "$('#divPanel').hide();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "FileQuery", sScript, true);

                    this.GridView1.PageSize = Convert.ToInt32(this.GetSystem("PageSize"));
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
                    DataTable dt= null;
                    DataTable dtWprec = null;
                    DataTable dtWpborrow = null;
                    //ADD BY RICHARD 20160531
                    DataTable dtTRANST = null;

                    string wpinno = string.Empty;
                    //OleDbCommand command = (OleDbCommand)this.DBConn.GeneralSqlCmd.Command;

                    //command.Parameters.Clear();

                    if (!string.IsNullOrEmpty(this.TxtWPINNO.Text.Trim()))
                    {
                        wpinno = this.TxtWPINNO.Text.Trim();

                        //command.Parameters.Add(new OleDbParameter("WPINNO", OleDbType.VarChar)).Value = wpinno;

                        //where = string.Format("And WP.WPINNO =:WPINNO");
                        where = string.Format("And WP.WPINNO ='{0}' ", wpinno);
                    }

                    strSql = this.Select.Wprec(where);

                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                    this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                    dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                    //this.WriteLog(global::Log.Mode.LogMode.DEBUG, "1");

                    if (dt.Rows.Count == 2)
                    {
                        strSql = this.Select.WprecBySingle(where);

                        this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                    }

                    Session["FileWprecQuery"] = strSql;

                    //this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;
                    //this.WriteLog(global::Log.Mode.LogMode.DEBUG, "2");

                    dtWprec = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                    //this.WriteLog(global::Log.Mode.LogMode.DEBUG, "3");

                    //MODIFY BY RICHARD 20160324
                    //if (dtWprec.Rows.Count == 0)
                    //{
                    //    this.ShowMessage("目前查詢沒有任何資料", MessageMode.INFO);
                    //    return;
                    //}
                    //else
                    //{
                        ViewState[this.GridView1.ClientID] = dtWprec;

                        dtWprec.Dispose(); dtWprec = null;

                        this.GridView1.DataBind((DataTable)ViewState[this.GridView1.ClientID], Anew, LockPageNum);
                    //}

                    //MODIFY BY RICHARD 20160510 只要有做703借檔，就該顯示於707查詢，簽核後再顯示簽核資訊，而非等到檔案室調妥新增後才有借檔資料
                    //where = string.Format("And WP.WPINNO ='{0}' AND WP.PRTFLAG IN ('N','P','F') ", wpinno);
                    where = string.Format("And WP.WPINNO ='{0}' ", wpinno);

                    strSql = this.Select.FileQuery(where);

                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                    Session["FileWpborrowQuery"] = strSql;

                    this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                    dtWpborrow = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                    //MODIFY BY RICHARD 20160324
                    //if (dtWpborrow.Rows.Count == 0)
                    //{
                        //this.ShowMessage("此文號無任何調閱紀錄", MessageMode.INFO);
                        //return;
                    //}
                    //else
                    //{
                        ViewState[this.GridView2.ClientID] = dtWpborrow;

                        dtWpborrow.Dispose(); dtWpborrow = null;
                    //}
                        this.GridView2.DataBind((DataTable)ViewState[this.GridView2.ClientID], Anew, LockPageNum);
                    
                    //ADD BY RICHARD 20160531
                    where = string.Format("And tt.WPINNO ='{0}' ORDER BY tt.TRANSTIME DESC ", wpinno);
                    strSql = this.Select.Transtable(where);
                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                    dtTRANST = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);
                    ViewState[this.GridView3.ClientID] = dtTRANST;
                    this.GridView3.DataBind((DataTable)ViewState[this.GridView3.ClientID], Anew, LockPageNum);


                }

            }
            catch (System.Exception ex) { this.ShowMessage(ex); }
            finally
            {
                this.DBConn.Dispose(); this.DBConn = null;
            }
        }
        #endregion

        #region GridView1_Sorting()
        protected void GridView2_Sorting(object sender, GridViewSortEventArgs e)
        {
            e.Sorting(sender);

            this.DataBind(false, true);
        }
        #endregion

        #region GridView1_PageIndexChanging()
        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            e.PageIndexChanging(sender);

            this.DataBind(false, false);
        }
        #endregion
    }
}