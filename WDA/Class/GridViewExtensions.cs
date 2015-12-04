using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WDA.Class
{
    public static class GridViewExtensions
    {
        /// <summary>
        /// 搜尋結果 (1) 筆資料, 共 (2) 頁
        /// </summary>
        private static string _TotalPageString = "搜尋結果 (1) 筆資料, 共 (2) 頁 ";
        /// <summary>
        /// 目前頁數為 第 (1) 頁, 每頁顯示 (2) 筆
        /// </summary>
        private static string _PageString = "目前頁數為 第 (1) 頁, 每頁顯示 (2) 筆";

        #region AlignMode
        /// <summary>
        /// 選擇的模式
        /// </summary>
        public enum AlignMode { Left = 0, Center = 1, Right = 2 }
        #endregion

        #region SortIconWidth
        /// <summary>
        /// Sort Icon Width
        /// </summary>
        private static int _SortIconWidth = 12;
        /// <summary>
        /// Sort Icon Width
        /// </summary>
        public static int SortIconWidth
        {
            get { return _SortIconWidth; }
            set { _SortIconWidth = value; }
        }
        #endregion

        #region SortBy
        /// <summary>
        /// Sort By
        /// </summary>
        private static string sortBy = "SortBy";
        /// <summary>
        /// Sort By
        /// </summary>
        public static string SortBy
        {
            get { return sortBy; }
            set { sortBy = value; }
        }
        #endregion

        #region Count
        /// <summary>
        /// Count
        /// </summary>
        private static string count = "Count";
        /// <summary>
        /// Count
        /// </summary>
        public static string Count
        {
            get { return count; }
            set { count = value; }
        }
        #endregion

        #region RowCreated
        /// <summary>
        /// 標題及頁尾樣式 (跨欄合併)
        /// </summary>
        /// <param name="MyPage">Page Object</param>
        /// <param name="E">DataGridItemEventArgs</param>
        /// <param name="CellNames">標題名稱陣列 (第一層)</param>
        public static void RowCreated(this GridViewRowEventArgs E, object sender, DataTable DT, string[] CellNames)
        {
            if (E.Row.RowType != DataControlRowType.Header || E.Row.RowType == DataControlRowType.Footer)
            {
                return;
            }
            GridView gv = (GridView)sender;

            if (E.Row.RowType == DataControlRowType.Header)
            {
                if (CellNames == null || CellNames.Length == 0) return;

                for (int i = 0; i < E.Row.Cells.Count; i++)
                {
                    if (gv.AutoGenerateColumns)
                    {
                        E.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;

                        E.Row.Cells[i].Wrap = false;
                    }

                    if (i >= CellNames.Length) break;
                    try
                    {
                        if (E.Row.Cells[i].Controls.Count > 0 && E.Row.Cells[i].Controls[0].GetType().ToString() == "System.Web.UI.WebControls.DataControlLinkButton")
                        {
                            LinkButton linkButton = ((LinkButton)E.Row.Cells[i].Controls[0]);

                            linkButton.CssClass = gv.HeaderStyle.CssClass;

                            linkButton.Text = CellNames[i];
                            try
                            {
                                if (!gv.AutoGenerateColumns) linkButton.ToolTip = gv.Columns[i].SortExpression;
                                else if (i < DT.Columns.Count) linkButton.ToolTip = DT.Columns[i].ColumnName;
                            }
                            catch { }

                            linkButton.ID = linkButton.ToolTip + "_" + i.ToString();

                            linkButton.Attributes.Add("name", ((LinkButton)E.Row.Cells[i].Controls[0]).ToolTip.ToUpper());

                            using (Label sortLabel = new Label())
                            {
                                sortLabel.Attributes.Add("style", "width: " + SortIconWidth.ToString() + "px; font-size: 0px");

                                E.Row.Cells[i].Controls.Add(sortLabel);
                            }
                        }
                    }
                    catch { E.Row.Cells[i].Text = CellNames[i]; }
                }
            }

            #region 頁尾

            if (E.Row.RowType == DataControlRowType.Footer)
            {
                TableCellCollection tcl = E.Row.Cells;

                tcl.Clear();

                int cellsCount = gv.Columns.Count;

                if (gv.AutoGenerateColumns)
                {
                    cellsCount = 0;

                    for (int i = 0; i < gv.Controls[0].Controls[0].Controls.Count; i++)
                    {
                        int columnSpan = ((TableCell)gv.Controls[0].Controls[0].Controls[i]).ColumnSpan;

                        if (columnSpan == 0) columnSpan = 1;

                        cellsCount += columnSpan;
                    }
                }
                for (int i = 0; i < cellsCount; i++)
                {
                    tcl.Add(new TableHeaderCell());
                    tcl[i].CssClass = gv.FooterStyle.CssClass;
                    tcl[i].Height = 20;
                }
            }
            #endregion
        }
        /// <summary>
        /// 標題及頁尾樣式 (跨欄合併)
        /// </summary>
        /// <param name="MyPage">Page Object</param>
        /// <param name="E">DataGridItemEventArgs</param>
        /// <param name="CellNamesA">標題名稱陣列 (第一層)</param>
        /// <param name="CellNamesB">標題名稱陣列 (第二層)</param>
        public static void RowCreated(this GridViewRowEventArgs E, object sender, DataTable DT, string[] CellNamesA, string[] CellNamesB)
        {
            if (E.Row.RowType != DataControlRowType.Header || E.Row.RowType == DataControlRowType.Footer)
            {
                return;
            }
            GridView gv = (GridView)sender;

            if (E.Row.RowType == DataControlRowType.Header)
            {
                if (CellNamesA == null || CellNamesA.Length == 0) return;

                if (CellNamesB == null || CellNamesB.Length == 0) return;

                #region 取出原本的 LinkButton, 與以保留

                LinkButton[] linkButton = new LinkButton[E.Row.Cells.Count];

                Label[] lbl = new Label[E.Row.Cells.Count];

                for (int i = 0; i < E.Row.Cells.Count; i++)
                {
                    if (gv.AllowSorting)
                    {
                        try
                        {
                            if (E.Row.Cells[i].Controls.Count > 0 && E.Row.Cells[i].Controls[0].GetType().ToString() == "System.Web.UI.WebControls.DataControlLinkButton")
                            {
                                linkButton[i] = (LinkButton)E.Row.Cells[i].Controls[0];

                                linkButton[i].CssClass = gv.HeaderStyle.CssClass;
                                try
                                {
                                    if (!gv.AutoGenerateColumns) linkButton[i].ToolTip = gv.Columns[i].SortExpression;
                                    else if (i < DT.Columns.Count) linkButton[i].ToolTip = DT.Columns[i].ColumnName;
                                }
                                catch { }

                                linkButton[i].ID = linkButton[i].ToolTip.ToUpper() + "_" + i.ToString();

                                linkButton[i].Attributes.Add("name", linkButton[i].ToolTip.ToUpper());
                            }
                        }
                        catch { linkButton[i] = null; }
                    }
                    else
                    {
                        using (Label label = new Label())
                        {
                            label.CssClass = gv.HeaderStyle.CssClass;

                            label.Text = E.Row.Cells[i].Text;

                            lbl[i] = label;
                        }
                    }
                }
                #endregion

                int controlCount = 0;

                if (gv.AllowSorting) controlCount = linkButton.Length;
                else controlCount = lbl.Length;

                #region 製作格式

                TableCellCollection trA = E.Row.Cells;

                trA.Clear();

                for (int i = 0; i < CellNamesA.Length; i++)
                {
                    string[] cellArray = CellNamesA[i].Split('|');

                    if (cellArray.Length == 3)
                    {
                        #region 非跨欄

                        using (TableHeaderCell td = new TableHeaderCell())
                        {
                            td.Wrap = false;

                            td.HorizontalAlign = HorizontalAlign.Center;

                            td.CssClass = gv.HeaderStyle.CssClass;

                            if (cellArray[1].ToLower().IndexOf("rowspan=") > -1)
                            {
                                td.RowSpan = int.Parse(cellArray[1].Split('=')[1]);

                                int linkIndex = int.Parse(cellArray[2]);

                                if (gv.AllowSorting)
                                {
                                    if (linkIndex >= linkButton.Length) break;

                                    linkButton[linkIndex].Text = cellArray[0];

                                    if (linkIndex < controlCount) td.Controls.Add(linkButton[linkIndex]);
                                }
                                else
                                {
                                    lbl[i].Text = cellArray[0];

                                    if (linkIndex < controlCount) td.Controls.Add(lbl[linkIndex]);
                                }

                                if (linkIndex > controlCount)
                                {
                                    using (Label _Label = new Label())
                                    {
                                        _Label.Text = cellArray[0];

                                        trA[i].Controls.Add(_Label);
                                    }
                                }
                            }

                            using (Label sortLabel = new Label())
                            {
                                sortLabel.Attributes.Add("style", "width: " + SortIconWidth.ToString() + "px; font-size: 0px");

                                td.Controls.Add(sortLabel);
                            }
                            trA.Add(td);
                        }
                        #endregion
                    }
                    else if (cellArray.Length == 2)
                    {
                        #region 跨欄

                        using (TableHeaderCell td = new TableHeaderCell())
                        {
                            if (cellArray[1].ToLower().IndexOf("colspan=") > -1)
                            {
                                td.ColumnSpan = int.Parse(cellArray[1].Split('=')[1]);

                                td.Wrap = false;

                                td.HorizontalAlign = HorizontalAlign.Center;

                                td.CssClass = gv.HeaderStyle.CssClass;

                                using (Label _Label = new Label())
                                {
                                    _Label.Text = cellArray[0];

                                    td.Controls.Add(_Label);
                                }
                            }
                            trA.Add(td);
                        }
                        #endregion
                    }
                }

                using (TableRow trB = new TableRow())
                {
                    for (int i = 0; i < CellNamesB.Length; i++)
                    {
                        string[] cellArray = CellNamesB[i].Split('|');

                        using (TableHeaderCell td = new TableHeaderCell())
                        {
                            td.HorizontalAlign = HorizontalAlign.Center;

                            td.Wrap = false;

                            td.CssClass = gv.HeaderStyle.CssClass;

                            int linkIndex = int.Parse(cellArray[1]);

                            if (linkIndex < controlCount)
                            {
                                if (gv.AllowSorting)
                                {
                                    linkButton[linkIndex].Text = cellArray[0];

                                    td.Controls.AddAt(0, linkButton[linkIndex]);
                                }
                                else
                                {
                                    lbl[linkIndex].Text = cellArray[0];

                                    td.Controls.AddAt(0, lbl[linkIndex]);
                                }
                            }

                            if (linkIndex > controlCount)
                            {
                                using (Label label = new Label())
                                {
                                    label.Text = cellArray[0];

                                    td.Controls.AddAt(0, label);
                                }
                            }

                            using (Label _SortLabel = new Label())
                            {
                                _SortLabel.Attributes.Add("style", "width: " + SortIconWidth.ToString() + "px; font-size: 0px");

                                td.Controls.Add(_SortLabel);
                            }
                            trB.Controls.Add(td);
                        }
                    }
                    trA[E.Row.Cells.Count - 1].Controls.Add(trB);
                }
                #endregion
            }

            #region 頁尾

            if (E.Row.RowType == DataControlRowType.Footer)
            {
                TableCellCollection tcl = E.Row.Cells;

                tcl.Clear();

                int cellsCount = gv.Columns.Count;

                if (gv.AutoGenerateColumns)
                {
                    cellsCount = 0;

                    for (int i = 0; i < gv.Controls[0].Controls[0].Controls.Count; i++)
                    {
                        int columnSpan = ((TableCell)gv.Controls[0].Controls[0].Controls[i]).ColumnSpan;

                        if (columnSpan == 0) columnSpan = 1;

                        cellsCount += columnSpan;
                    }
                }

                for (int i = 0; i < cellsCount; i++)
                {
                    tcl.Add(new TableHeaderCell());
                    tcl[i].CssClass = gv.FooterStyle.CssClass;
                    tcl[i].Height = 20;
                }
            }
            #endregion
        }
        /// <summary>
        /// 標題及頁尾樣式 (跨欄合併)
        /// </summary>
        /// <param name="MyPage">Page Object</param>
        /// <param name="E">DataGridItemEventArgs</param>
        /// <param name="CellNamesA">標題名稱陣列 (第一層)</param>
        /// <param name="CellNamesB">標題名稱陣列 (第二層)</param>
        /// <param name="CellNamesC">標題名稱陣列 (第三層)</param>
        public static void RowCreated(this GridViewRowEventArgs E, object sender, DataTable DT, string[] CellNamesA, string[] CellNamesB, string[] CellNamesC)
        {
            if (E.Row.RowType != DataControlRowType.Header || E.Row.RowType == DataControlRowType.Footer)
            {
                return;
            }
            GridView gv = (GridView)sender;

            if (E.Row.RowType == DataControlRowType.Header)
            {
                if (CellNamesA == null || CellNamesA.Length == 0) return;

                if (CellNamesB == null || CellNamesB.Length == 0) return;

                if (CellNamesC == null || CellNamesC.Length == 0) return;

                #region 取出原本的 LinkButton, 與以保留

                LinkButton[] linkButton = new LinkButton[E.Row.Cells.Count];

                Label[] lbl = new Label[E.Row.Cells.Count];

                for (int i = 0; i < E.Row.Cells.Count; i++)
                {
                    if (gv.AllowSorting)
                    {
                        try
                        {
                            if (E.Row.Cells[i].Controls.Count > 0 && E.Row.Cells[i].Controls[0].GetType().ToString() == "System.Web.UI.WebControls.DataControlLinkButton")
                            {
                                linkButton[i] = (LinkButton)E.Row.Cells[i].Controls[0];

                                linkButton[i].CssClass = gv.HeaderStyle.CssClass;
                                try
                                {
                                    if (!gv.AutoGenerateColumns) linkButton[i].ToolTip = gv.Columns[i].SortExpression;
                                    else if (i < DT.Columns.Count) linkButton[i].ToolTip = DT.Columns[i].ColumnName;
                                }
                                catch { }

                                linkButton[i].ID = linkButton[i].ToolTip.ToUpper() + "_" + i.ToString();

                                linkButton[i].Attributes.Add("name", linkButton[i].ToolTip.ToUpper());
                            }
                        }
                        catch { linkButton[i] = null; }
                    }
                    else
                    {
                        using (Label label = new Label())
                        {
                            label.CssClass = gv.HeaderStyle.CssClass;

                            label.Text = E.Row.Cells[i].Text;

                            lbl[i] = label;
                        }
                    }
                }
                #endregion

                int controlCount = 0;

                if (gv.AllowSorting) controlCount = linkButton.Length;
                else controlCount = lbl.Length;

                #region 製作格式

                TableCellCollection trA = E.Row.Cells;

                trA.Clear();

                for (int i = 0; i < CellNamesA.Length; i++)
                {
                    string[] cellArray = CellNamesA[i].Split('|');

                    if (cellArray.Length == 3)
                    {
                        #region 非跨欄

                        using (TableHeaderCell td = new TableHeaderCell())
                        {
                            td.Wrap = false;

                            td.HorizontalAlign = HorizontalAlign.Center;

                            td.CssClass = gv.HeaderStyle.CssClass;

                            if (cellArray[1].ToLower().IndexOf("rowspan=") > -1)
                            {
                                td.RowSpan = int.Parse(cellArray[1].Split('=')[1]);

                                int linkIndex = int.Parse(cellArray[2]);

                                if (gv.AllowSorting)
                                {
                                    if (linkIndex >= linkButton.Length) break;

                                    linkButton[linkIndex].Text = cellArray[0];

                                    if (linkIndex < controlCount) td.Controls.Add(linkButton[linkIndex]);
                                }
                                else
                                {
                                    lbl[i].Text = cellArray[0];

                                    if (linkIndex < controlCount) td.Controls.Add(lbl[linkIndex]);
                                }

                                if (linkIndex > controlCount)
                                {
                                    using (Label _Label = new Label())
                                    {
                                        _Label.Text = cellArray[0];

                                        trA[i].Controls.Add(_Label);
                                    }
                                }
                            }

                            using (Label sortLabel = new Label())
                            {
                                sortLabel.Attributes.Add("style", "width: " + SortIconWidth.ToString() + "px; font-size: 0px");

                                td.Controls.Add(sortLabel);
                            }
                            trA.Add(td);
                        }
                        #endregion
                    }
                    else if (cellArray.Length == 2)
                    {
                        #region 跨欄

                        using (TableHeaderCell td = new TableHeaderCell())
                        {
                            if (cellArray[1].ToLower().IndexOf("colspan=") > -1)
                            {
                                td.ColumnSpan = int.Parse(cellArray[1].Split('=')[1]);

                                td.Wrap = false;

                                td.HorizontalAlign = HorizontalAlign.Center;

                                td.CssClass = gv.HeaderStyle.CssClass;

                                using (Label _Label = new Label())
                                {
                                    _Label.Text = cellArray[0];

                                    td.Controls.Add(_Label);
                                }
                            }
                            trA.Add(td);
                        }
                        #endregion
                    }
                }

                using (TableRow trB = new TableRow())
                {
                    for (int i = 0; i < CellNamesB.Length; i++)
                    {
                        string[] cellArray = CellNamesB[i].Split('|');

                        if (cellArray.Length == 3)
                        {
                            #region 非跨欄

                            using (TableHeaderCell td = new TableHeaderCell())
                            {
                                td.Wrap = false;

                                td.HorizontalAlign = HorizontalAlign.Center;

                                td.CssClass = gv.HeaderStyle.CssClass;

                                if (cellArray[1].ToLower().IndexOf("rowspan=") > -1)
                                {
                                    td.RowSpan = int.Parse(cellArray[1].Split('=')[1]);

                                    int linkIndex = int.Parse(cellArray[2]);

                                    if (gv.AllowSorting)
                                    {
                                        if (linkIndex >= linkButton.Length) break;

                                        linkButton[linkIndex].Text = cellArray[0];

                                        if (linkIndex < controlCount) td.Controls.Add(linkButton[linkIndex]);
                                    }
                                    else
                                    {
                                        lbl[i].Text = cellArray[0];

                                        if (linkIndex < controlCount) td.Controls.Add(lbl[linkIndex]);
                                    }

                                    if (linkIndex > controlCount)
                                    {
                                        using (Label _Label = new Label())
                                        {
                                            _Label.Text = cellArray[0];
                                            td.Controls.AddAt(0, _Label);
                                        }
                                    }
                                }

                                using (Label sortLabel = new Label())
                                {
                                    sortLabel.Attributes.Add("style", "width: " + SortIconWidth.ToString() + "px; font-size: 0px");

                                    td.Controls.Add(sortLabel);
                                }
                                trB.Cells.Add(td);
                            }
                            #endregion
                        }
                        else if (cellArray.Length == 2)
                        {
                            #region 跨欄

                            using (TableHeaderCell td = new TableHeaderCell())
                            {
                                if (cellArray[1].ToLower().IndexOf("colspan=") > -1)
                                {
                                    td.ColumnSpan = int.Parse(cellArray[1].Split('=')[1]);

                                    td.Wrap = false;

                                    td.HorizontalAlign = HorizontalAlign.Center;

                                    td.CssClass = gv.HeaderStyle.CssClass;

                                    using (Label _Label = new Label())
                                    {
                                        _Label.Text = cellArray[0];

                                        td.Controls.Add(_Label);
                                    }
                                }
                                trB.Cells.Add(td);
                            }
                            #endregion
                        }
                    }
                    trA[E.Row.Cells.Count - 1].Controls.Add(trB);
                }

                using (TableRow trC = new TableRow())
                {
                    for (int i = 0; i < CellNamesC.Length; i++)
                    {
                        string[] cellArray = CellNamesC[i].Split('|');

                        using (TableHeaderCell td = new TableHeaderCell())
                        {
                            td.HorizontalAlign = HorizontalAlign.Center;

                            td.Wrap = false;

                            td.CssClass = gv.HeaderStyle.CssClass;

                            int linkIndex = int.Parse(cellArray[1]);

                            if (linkIndex < controlCount)
                            {
                                if (gv.AllowSorting)
                                {
                                    linkButton[linkIndex].Text = cellArray[0];

                                    td.Controls.AddAt(0, linkButton[linkIndex]);
                                }
                                else
                                {
                                    lbl[linkIndex].Text = cellArray[0];

                                    td.Controls.AddAt(0, lbl[linkIndex]);
                                }
                            }

                            if (linkIndex > controlCount)
                            {
                                using (Label label = new Label())
                                {
                                    label.Text = cellArray[0];

                                    td.Controls.AddAt(0, label);
                                }
                            }

                            using (Label _SortLabel = new Label())
                            {
                                _SortLabel.Attributes.Add("style", "width: " + SortIconWidth.ToString() + "px; font-size: 0px");

                                td.Controls.Add(_SortLabel);
                            }
                            trC.Controls.Add(td);
                        }
                    }
                    trA[E.Row.Cells.Count - 1].Controls.Add(trC);
                }
                #endregion
            }

            #region 頁尾

            if (E.Row.RowType == DataControlRowType.Footer)
            {
                TableCellCollection tcl = E.Row.Cells;

                tcl.Clear();

                int cellsCount = gv.Columns.Count;

                if (gv.AutoGenerateColumns)
                {
                    cellsCount = 0;

                    for (int i = 0; i < gv.Controls[0].Controls[0].Controls.Count; i++)
                    {
                        int columnSpan = ((TableCell)gv.Controls[0].Controls[0].Controls[i]).ColumnSpan;

                        if (columnSpan == 0) columnSpan = 1;

                        cellsCount += columnSpan;
                    }
                }

                for (int i = 0; i < cellsCount; i++)
                {
                    tcl.Add(new TableHeaderCell());
                    tcl[i].CssClass = gv.FooterStyle.CssClass;
                    tcl[i].Height = 20;
                }
            }
            #endregion
        }
        #endregion

        #region RowVisible
        /// <summary>
        /// 隱藏欄位
        /// </summary>
        /// <param name="E"></param>
        /// <param name="sender"></param>
        /// <param name="CellIndexs"></param>
        public static void RowVisible(this GridViewRowEventArgs E, object sender, int[] CellIndexs)
        {
            if (E.Row.RowType == DataControlRowType.Header || E.Row.RowType == DataControlRowType.DataRow || E.Row.RowType == DataControlRowType.Footer)
            {
                foreach (int index in CellIndexs) E.Row.Cells[index].Visible = false;
            }
        }
        /// <summary>
        /// 隱藏欄位
        /// </summary>
        /// <param name="E"></param>
        /// <param name="sender"></param>
        /// <param name="CellIndex"></param>
        public static void RowVisible(this GridViewRowEventArgs E, object sender, int CellIndex)
        {
            if (E.Row.RowType == DataControlRowType.Header || E.Row.RowType == DataControlRowType.DataRow || E.Row.RowType == DataControlRowType.Footer)
            {
                E.Row.Cells[CellIndex].Visible = false;
            }
        }
        #endregion

        #region PageIndexChanging()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="E"></param>
        /// <param name="sender"></param>
        public static void PageIndexChanging(this GridViewPageEventArgs E, object sender)
        {
            GridView gv = (GridView)sender;

            gv.PageIndex = E.NewPageIndex;
        }
        #endregion

        #region Sorting()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="E"></param>
        /// <param name="sender"></param>
        public static void Sorting(this GridViewSortEventArgs E, object sender)
        {
            GridView gv = (GridView)sender;

            string sortBy = gv.Attributes[SortBy] == null ? E.SortExpression.Trim().ToLower() : gv.Attributes[SortBy].Trim().ToLower();

            if (sortBy.Replace(" desc", "").Replace(" asc", "") == E.SortExpression.Trim().ToLower())
            {
                if (sortBy.ToLower().IndexOf(" asc") > 0) sortBy = sortBy.ToLower().Replace(" asc", " desc");
                else if (sortBy.ToLower().IndexOf(" desc") > 0) sortBy = sortBy.ToLower().Replace(" desc", " asc");
                else sortBy = E.SortExpression.ToLower() + " asc";
            }
            else sortBy = E.SortExpression.ToLower() + " asc";

            gv.Attributes[SortBy] = sortBy;
        }
        #endregion

        #region RowDataBound()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="E"></param>
        public static void RowDataBound(this GridViewRowEventArgs E)
        {
            if (E.Row.RowType == DataControlRowType.DataRow)
            {
                E.Row.Attributes.Add("OnMouseOver", "c=this.className;this.className='MouseOver'");

                E.Row.Attributes.Add("OnMouseOut", "this.className=c");
            }
        }
        #endregion

        #region DataBind()
        /// <summary>
        /// Data Bind
        /// </summary>
        /// <param name="DT"></param>
        /// <param name="Anew"></param>
        /// <param name="LockPageNum"></param>
        public static void DataBind(this GridView GV, DataTable DT, bool Anew, bool LockPageNum)
        {
            DataBind(GV, DT, Anew, LockPageNum, null, null, null);
        }
        /// <summary>
        /// Data Bind
        /// </summary>
        /// <param name="DT"></param>
        /// <param name="Anew"></param>
        /// <param name="LockPageNum"></param>
        public static void DataBind(this GridView GV, DataTable DT, bool Anew, bool LockPageNum, Label TotalPageLabel, Label PageLabel, List<LinkButton> LinkButtons)
        {
            DataView dv = DT.DefaultView;

            if (GV.Attributes[SortBy] != null)
            {
                dv.Sort = GV.Attributes[SortBy];

                GV.Attributes.Add(SortBy, dv.Sort);
            }
            GV.DataSource = dv;

            if (Anew && !LockPageNum) GV.PageIndex = 0;
            if (DT.Rows.Count <= GV.PageSize) GV.PageIndex = 0;
            else if (DT.Rows.Count <= GV.PageCount * GV.PageSize - GV.PageSize)
            {
                if (GV.PageIndex - 1 < 0) GV.PageIndex = 0;
                else GV.PageIndex -= 1;
            }
            GV.DataBind();
            GV.Dispose();

            GV.Attributes[Count] = DT.Rows.Count.ToString();

            int count = 0;

            if ((int.Parse(GV.Attributes[Count]) % GV.PageSize) == 0) count = (int.Parse(GV.Attributes[Count]) / GV.PageSize);
            else count = (int.Parse(GV.Attributes[Count]) / GV.PageSize) + 1;

            ShowStats(GV, TotalPageLabel, PageLabel);

            PageControl(GV, LinkButtons);

            VisibleControl(GV, LinkButtons, count != 0);
        }
        #endregion

        #region PageControl()
        /// <summary>
        /// 翻頁連結控制
        /// </summary>
        /// <param name="GV"></param>
        public static void PageControl(this GridView GV, List<LinkButton> LinkButtons)
        {
            if (LinkButtons == null || LinkButtons.Count < 4) return;

            PageControl(GV, LinkButtons[0], LinkButtons[1], LinkButtons[2], LinkButtons[3]);
        }
        /// <summary>
        /// 翻頁連結控制
        /// </summary>
        /// <param name="GV"></param>
        /// <param name="FirstButton"></param>
        /// <param name="PrevButton"></param>
        /// <param name="NextButton"></param>
        /// <param name="LastButton"></param>
        private static void PageControl(this GridView GV, LinkButton FirstButton, LinkButton PrevButton, LinkButton NextButton, LinkButton LastButton)
        {
            if (FirstButton == null || PrevButton == null || NextButton == null || LastButton == null) return;

            if (GV.PageIndex == 0)
            {
                FirstButton.Enabled = PrevButton.Enabled = false;
            }
            else
            {
                FirstButton.Enabled = PrevButton.Enabled = true;
            }
            if (GV.PageIndex == (GV.PageCount - 1))
            {
                LastButton.Enabled = NextButton.Enabled = false;
            }
            else
            {
                LastButton.Enabled = NextButton.Enabled = true;
            }
        }
        #endregion

        #region ShowStats()
        /// <summary>
        /// 顯示筆數及頁數
        /// </summary>
        /// <param name="GV"></param>
        /// <param name="TotalPageLabel"></param>
        /// <param name="PageLabel"></param>
        public static void ShowStats(this GridView GV, Label TotalPageLabel, Label PageLabel)
        {
            if (TotalPageLabel != null && PageLabel != null)
            {
                TotalPageLabel.Text = " " + _TotalPageString.Replace("(1)", GV.Attributes[Count].ToString()).Replace("(2)", GV.PageCount.ToString()) + " ";

                PageLabel.Text = _PageString.Replace("(1)", (GV.PageIndex + 1).ToString()).Replace("(2)", GV.PageSize.ToString());
            }
        }
        #endregion

        #region VisibleControl()
        /// <summary>
        /// 是否顯示換頁按鈕
        /// </summary>
        /// <param name="GV"></param>
        /// <param name="IsVisible"></param>
        public static void VisibleControl(this GridView GV, List<LinkButton> LinkButtons, bool IsVisible)
        {
            if (LinkButtons == null) return;

            foreach (LinkButton linkButton in LinkButtons) linkButton.Visible = IsVisible;
        }
        #endregion

        #region CoalitionTitleCell()
        /// <summary>
        /// 組合 Title
        /// </summary>
        /// <returns></returns>
        public static string CoalitionTitleCell(this Page MyPage, string CellName)
        {
            return MyPage.CoalitionTitleCell(CellName, AlignMode.Left);
        }
        /// <summary>
        /// 組合 Title
        /// </summary>
        /// <returns></returns>
        public static string CoalitionTitleCell(this Page MyPage, string CellName, AlignMode Mode)
        {
            string colOrRowSpan = string.Empty;

            string align = Mode == AlignMode.Left ? "Left" : Mode == AlignMode.Center ? "Center" : "Right";

            string[] cellArray = CellName.Split('|');

            if (cellArray.Length >= 2)
            {
                colOrRowSpan = cellArray[1].IndexOf('=') > -1 ? " " + cellArray[1] : string.Empty;
            }
            return "<td nowrap align=\"" + align + "\"" + colOrRowSpan + "><b>" + cellArray[0] + "</b></td>";
        }
        /// <summary>
        /// 組合 Title
        /// </summary>
        /// <returns></returns>
        public static string CoalitionTitleCell(this Page MyPage, string[] CellNamesA, string[] CellNamesB, string[] CellNamesC)
        {
            string html = "<tr>";

            for (int i = 0; i < CellNamesA.Length; i++)
            {
                string[] cellArray = CellNamesA[i].Split('|');

                if (cellArray.Length == 3)
                {
                    #region 非跨欄

                    if (cellArray[1].ToLower().IndexOf("rowspan=") > -1)
                    {
                        html += string.Format("<th rowspan={0} style=\"WHITE-SPACE: nowrap\" align=\"center\">{1}</td>",
                            cellArray[1].Split('=')[1],
                            cellArray[0]);
                    }
                    #endregion
                }
                else if (cellArray.Length == 2)
                {
                    #region 跨欄

                    if (cellArray[1].ToLower().IndexOf("colspan=") > -1)
                    {
                        html += string.Format("<th colSpan={0} style=\"WHITE-SPACE: nowrap\" align=\"center\">{1}</td>",
                                cellArray[1].Split('=')[1],
                                cellArray[0]);
                    }
                    #endregion
                }
            }
            html += "</tr><tr>";

            for (int i = 0; i < CellNamesB.Length; i++)
            {
                string[] cellArray = CellNamesB[i].Split('|');

                if (cellArray.Length == 3)
                {
                    #region 非跨欄

                    if (cellArray[1].ToLower().IndexOf("rowspan=") > -1)
                    {
                        html += string.Format("<th rowspan={0} style=\"WHITE-SPACE: nowrap\" align=\"center\">{1}</td>",
                            cellArray[1].Split('=')[1],
                            cellArray[0]);
                    }
                    #endregion
                }
                else if (cellArray.Length == 2)
                {
                    #region 跨欄

                    if (cellArray[1].ToLower().IndexOf("colspan=") > -1)
                    {
                        html += string.Format("<th colSpan={0} style=\"WHITE-SPACE: nowrap\" align=\"center\">{1}</td>",
                                cellArray[1].Split('=')[1],
                                cellArray[0]);
                    }
                    #endregion
                }
            }
            html += "</tr><tr>";

            for (int i = 0; i < CellNamesC.Length; i++)
            {
                string[] cellArray = CellNamesC[i].Split('|');

                html += string.Format("<th style=\"WHITE-SPACE: nowrap\" align=\"center\">{0}</td>", cellArray[0]);
            }
            html += "</tr>";

            return html;
        }
        #endregion

        #region ExcelTitle()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MyPage"></param>
        /// <param name="FileName"></param>
        public static void ExcelTitle(this Page MyPage, string FileName)
        {
            MyPage.Response.Clear();
            MyPage.Response.AddHeader("Content-disposition", "filename=" + FileName);
            MyPage.Response.AddHeader("Content-Type", " application/octetstream");
            MyPage.Response.AddHeader("Expires", "0");
            MyPage.Response.AddHeader("Pragma", "public");
            MyPage.Response.AddHeader("Cache-Control", "max-age=31536000");

            MyPage.Response.Write("<html xmlns:o=\"urn:schemas-microsoft-com:office:office\"xmlns:x=\"urn:schemas-microsoft-com:office:excel\"xmlns=\"http://www.w3.org/TR/REC-html40\">\n");
            MyPage.Response.Write("<meta http-equiv=\"Content-Type\" content=\"charset=utf-8\">\n");
            MyPage.Response.Write("<head>						    \n");
            MyPage.Response.Write("<xml>	                                             \n");
            MyPage.Response.Write(" <x:ExcelWorkbook>                                  \n");
            MyPage.Response.Write("  <x:ExcelWorksheets>                               \n");
            MyPage.Response.Write("   <x:ExcelWorksheet>                               \n");
            MyPage.Response.Write("    <x:Name>Page1</x:Name>                          \n");
            MyPage.Response.Write("    <x:WorksheetOptions>                            \n");
            MyPage.Response.Write("     <x:DefaultRowHeight>330</x:DefaultRowHeight>   \n");
            MyPage.Response.Write("     <x:Selected/>                                  \n");
            MyPage.Response.Write("     <x:ProtectContents>False</x:ProtectContents>   \n");
            MyPage.Response.Write("     <x:ProtectObjects>False</x:ProtectObjects>     \n");
            MyPage.Response.Write("     <x:ProtectScenarios>False</x:ProtectScenarios> \n");
            MyPage.Response.Write("    </x:WorksheetOptions>                           \n");
            MyPage.Response.Write("   </x:ExcelWorksheet>                              \n");
            MyPage.Response.Write("  </x:ExcelWorksheets>                              \n");
            MyPage.Response.Write(" </x:ExcelWorkbook>                                 \n");
            MyPage.Response.Write("</xml>                                              \n");
            MyPage.Response.Write("</head>                                             \n");

            MyPage.Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
        }
        #endregion

        #region WordTitle()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MyPage"></param>
        /// <param name="FileName"></param>
        public static void WordTitle(this Page MyPage, System.Web.UI.WebControls.Literal Literal, string FileName)
        {
            MyPage.Response.Clear();
            MyPage.Response.Buffer = true;
            MyPage.Response.ContentType = "application/msword";
            MyPage.Response.AddHeader("Content-Disposition", "attachment;filenametest=" + FileName);
            MyPage.Response.Charset = "utf-8";
            MyPage.EnableViewState = false;
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(MyPage.Response.OutputStream, System.Text.Encoding.GetEncoding("utf-8")))
            {
                StringBuilder strHTMLContent = new StringBuilder();
                System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(sw);
                Literal.RenderControl(htmlWrite);
                sw.Write(strHTMLContent);
                sw.Flush();
            }
            MyPage.Response.End();
        }
        #endregion

        #region CombineFilePath()
        /// <summary>
        /// 組回路徑
        /// </summary>
        /// <param name="Path1"></param>
        /// <param name="Path2"></param>
        /// <returns></returns>
        public static string CombineFilePath(this Page MyPage, string Path1, string Path2)
        {
            if (!Path1.EndsWith(@"\") || !Path1.EndsWith("/")) Path1 = string.Format("{0}{1}", Path1, @"\");

            if (Path2.StartsWith(@"\") || Path2.StartsWith("/")) Path2 = Path2.Remove(0, 1);

            return (Path1 + Path2).Replace("/", @"\").Replace(@"\\", @"\");
        }
        #endregion

        #region PrintHTML()
        /// <summary>
        /// 列印 HTML
        /// </summary>
        /// <param name="MyPage"></param>
        /// <param name="MySB"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public static string PrintHTML(this Page MyPage, StringBuilder MySB, string UserID)
        {
            return MyPage.PrintHTML(MySB.ToString(), UserID);
        }
        /// <summary>
        /// 列印 HTML
        /// </summary>
        /// <param name="MyPage"></param>
        /// <param name="MySB"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public static string PrintHTML(this Page MyPage, String HTML, string UserID)
        {
            string fileName = string.Empty;

            string dirPath = System.Web.HttpContext.Current.Server.MapPath(".") + "\\Export";

            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);

            if (Directory.Exists(dirPath))
            {
                fileName = UserID + "_Print.htm";

                string filePath = MyPage.CombineFilePath(dirPath, fileName);

                if (!File.Exists(filePath))
                {
                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        fs.Flush(); fs.Close();
                    }
                }
                using (StreamWriter sw = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
                {
                    sw.WriteLine(HTML);
                    sw.Flush();
                }
            }
            return fileName;
        }
        #endregion
    }
}