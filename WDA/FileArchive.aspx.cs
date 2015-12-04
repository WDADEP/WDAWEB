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
    public partial class FileArchive : GridViewUtility
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

        protected void BtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                this.InitSQL();
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }
        }

        #region ImageBtnEdit_Click()
        protected void ImageBtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gridViewRow = (GridViewRow)((ImageButton)sender).NamingContainer;

            GridView gv = (GridView)gridViewRow.NamingContainer;

            gv.EditIndex = gridViewRow.RowIndex;

            this.DataBind(true, false);
        }
        #endregion

        #region ITImageListBtnChang_Click()
        protected void ITImageListBtnChang_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gridViewRow = (GridViewRow)((ImageButton)sender).NamingContainer;

            int DataIndex = this.GridView1.PageSize * this.GridView1.PageIndex + gridViewRow.RowIndex;

            int result = 0;

            Hashtable ht = new Hashtable();

            string strSql = string.Empty, strWhere = string.Empty;
            try
            {
                #region Update

                //ht.Add("WpoutNo", gridViewRow.Cells[3].Text.Trim());
                ht.Add("FileNo", ((TextBox)gridViewRow.Cells[0].FindControl("txtFileNo")).Text.Trim());
                ht.Add("FileDate", ((TextBox)gridViewRow.Cells[0].FindControl("txtFileDate")).Text.Trim());
                ht.Add("KeepYr", ((TextBox)gridViewRow.Cells[0].FindControl("txtKeepYr")).Text.Trim());
                ht.Add("BoxNo", ((TextBox)gridViewRow.Cells[0].FindControl("txtBoxNo")).Text.Trim());
                ht.Add("LastModifyUserID", UserInfo.UserID);
                ht.Add("LastModifyTime", "SYSDATE");

                this.DBConnTransac.GeneralSqlCmd.Command.CommandTimeout = 90;

                //Wprec
                strWhere = string.Format("And WpinNo = '{0}'\n", gridViewRow.Cells[2].Text.Trim());
                strSql = this.Update.Wprec(ht, strWhere);
                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                result = this.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql);
                if (result < 1)
                {
                    this.ShowMessage("修改失敗"); return;
                }

                //BarcodeTable
                strWhere = string.Format("And BarcodeValue = '{0}'", gridViewRow.Cells[2].Text.Trim());
                strSql = this.Update.BarcodeTable(ht, strWhere);
                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                result = this.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql);
                if (result <= 0)
                {
                    this.ShowMessage("修改失敗"); return;
                }
                else
                {
                    this.DBConnTransac.GeneralSqlCmd.Transaction.Commit();
                    this.ShowMessage("修改成功", MessageMode.INFO);
                }

                #endregion
            }
            catch (Exception ex)
            {
                try
                {
                    if (this.DBConnTransac.GeneralSqlCmd.Transaction != null) this.DBConnTransac.GeneralSqlCmd.Transaction.Rollback();
                }
                catch { }

                this.ShowMessage(ex);
            }
            finally
            {
                if (ht != null) { ht.Clear(); ht = null; }

                if (this.DBConnTransac != null) { this.DBConnTransac.Dispose(); this.DBConnTransac = null; }

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

        #region Private Method

        #region DataBind()
        /// <summary>
        /// 資料繫結
        /// </summary>
        /// <param name="Anew">是否重新撈資料</param>
        /// <param name="LockPageNum">是否保留目前頁數</param>
        private void DataBind(bool Anew, bool LockPageNum)
        {
            string strSql = string.Empty, strWhere = string.Empty;

            DataTable dt = null;
            try
            {
                if (Anew)
                {
                    string barcodeValue = this.txtBarcodeValue.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                    string wpoutNo = this.txtWpoutNo.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                    string fileNo = this.txtFileNo.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                    string fileDate = this.txtFileDate.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                    string keepYr = this.txtKeepYr.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                    string boxNo = this.txtBoxNo.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                    string onFile = this.txtOnFile.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();

                    strWhere += string.Format("And bt.FileNo != ' '\n");

                    if (barcodeValue.Length > 0) strWhere += string.Format("And bt.BarcodeValue = '{0}'\n", barcodeValue);
                    if (wpoutNo.Length > 0) strWhere += string.Format("And bt.WpoutNo = '{0}'\n", wpoutNo);
                    if (fileNo.Length > 0) strWhere += string.Format("And bt.FileNo = '{0}'\n", fileNo);
                    if (fileDate.Length > 0) strWhere += string.Format("And bt.FileDate = '{0}'\n", fileDate);
                    if (keepYr.Length > 0) strWhere += string.Format("And bt.KeepYr = '{0}'\n", keepYr);
                    if (boxNo.Length > 0) strWhere += string.Format("And bt.BoxNo = '{0}'\n", boxNo);
                    if (onFile.Length > 0) strWhere += string.Format("And bt.OnFile = N'{0}'\n", onFile);

                    strSql = this.Select.BarcodeTable(strWhere);

                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                    this.DBConn.GeneralSqlCmd.Command.CommandTimeout = 90;

                    dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                    if (dt.Rows.Count == 0)
                    {
                        this.ShowMessage("目前查詢沒有任何資料", MessageMode.INFO);
                        this.GridView1.DataBind();
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

        #region InitSQL()
        private void InitSQL()
        {
            string strSql = string.Empty;
            string where = string.Empty;
            string strWhere = string.Empty;

            try
            {
                string barcodeValue = this.txtBarcodeValue.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                string wpoutNo = this.txtWpoutNo.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                string fileNo = this.txtFileNo.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                string fileDate = this.txtFileDate.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                string keepYr = this.txtKeepYr.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                string boxNo = this.txtBoxNo.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();
                string onFile = this.txtOnFile.Text.Trim().Replace(StringFormatException.Mode.Sql).Trim();

                strWhere += string.Format("And bt.FileNo != ' '\n");

                if (barcodeValue.Length > 0) strWhere += string.Format("And bt.BarcodeValue = '{0}'\n", barcodeValue);
                if (wpoutNo.Length > 0) strWhere += string.Format("And bt.WpoutNo = '{0}'\n", wpoutNo);
                if (fileNo.Length > 0) strWhere += string.Format("And bt.FileNo = '{0}'\n", fileNo);
                if (fileDate.Length > 0) strWhere += string.Format("And bt.FileDate = '{0}'\n", fileDate);
                if (keepYr.Length > 0) strWhere += string.Format("And bt.KeepYr = '{0}'\n", keepYr);
                if (boxNo.Length > 0) strWhere += string.Format("And bt.BoxNo = '{0}'\n", boxNo);
                if (onFile.Length > 0) strWhere += string.Format("And bt.OnFile = N'{0}'\n", onFile);

                strSql = this.Select.BarcodeTable(strWhere);

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                Session["FileArchive"] = strSql;
            }
            catch (System.Exception ex) { this.ShowMessage(ex.Message); }
        }
        #endregion

        #endregion

        #region GridView Events

        #region GridView1_RowCommand()
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView gv = (GridView)sender;
            try
            {
                if (e.CommandName != "Stop") return;

                int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;

                int result = 0;

                string strSql = string.Empty, strWhere = string.Empty;

                string barcodeValue = gv.Rows[rowIndex].Cells[2].Text.Trim();

                if (e.CommandName == "Stop")
                {
                    #region Delete

                    this.DBConnTransac.GeneralSqlCmd.Command.CommandTimeout = 90;

                    //Wprec
                    strWhere = string.Format("And WpinNo = '{0}'\n", barcodeValue);
                    strSql = this.Update.WprecDelete(strWhere);
                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                    result = this.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql);
                    if (result < 1)
                    {
                        this.ShowMessage("修改失敗"); return;
                    }

                    //Wptrans
                    strWhere = string.Format("And WpinNo = '{0}'\n", barcodeValue);
                    strSql = this.Update.WptransDelete(strWhere);
                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                    result = this.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql);
                    if (result < 1)
                    {
                        this.ShowMessage("修改失敗"); return;
                    }

                    //BarcodeTable
                    strSql = this.Delete.BarcodeTable(barcodeValue);
                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);
                    result = this.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql);
                    if (result < 1)
                    {
                        this.ShowMessage("刪除失敗");
                    }
                    else
                    {
                        this.DBConnTransac.GeneralSqlCmd.Transaction.Commit();
                        this.ShowMessage("刪除成功", MessageMode.INFO);
                        this.DataBind(true, true);
                    }
                    #endregion
                }
            }
            catch (System.Exception ex)
            {
                try
                {
                    if (this.DBConnTransac.GeneralSqlCmd.Transaction != null) this.DBConnTransac.GeneralSqlCmd.Transaction.Rollback();
                }
                catch { }

                this.ShowMessage(ex);
            }
            finally
            {
                if (this.DBConnTransac != null)
                {
                    this.DBConnTransac.Dispose(); this.DBConnTransac = null;
                }
            }
        }
        #endregion

        #region GridView1_RowDataBound()
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.RowDataBound();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnDelete = (ImageButton)e.Row.Cells[1].FindControl("ImageBtnDelete");

                this.Confirm(btnDelete, ConfirmMode.Delete);
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

        #region GridView1_Sorting()
        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            e.Sorting(sender);

            this.DataBind(false, true);
        }
        #endregion

        #endregion
    }
}