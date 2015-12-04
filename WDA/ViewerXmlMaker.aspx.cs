using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using WDA.Class;

namespace WDA
{
    public partial class ViewerXmlMaker : PageUtility
    {
        #region ViewerDocument
        protected ViewerDocument Viewer_Doc
        {
            get
            {
                if (ViewState["Viewer_Doc"] == null)
                    return new ViewerDocument();
                else
                    return (ViewerDocument)(ViewState["Viewer_Doc"]);
            }

            set { ViewState["Viewer_Doc"] = value; }
        }

        protected DataTable DT_File
        {
            get
            {
                if (ViewState["DT_File"] == null)
                    return null;
                else
                    return (DataTable)(ViewState["DT_File"]);
            }

            set { ViewState["DT_File"] = value; }
        }

        protected DataTable DT_Above
        {
            get
            {
                if (ViewState["DT_Above"] == null)
                    return null;
                else
                    return (DataTable)(ViewState["DT_Above"]);
            }

            set { ViewState["DT_Above"] = value; }
        }

        protected DataTable DT_Below
        {
            get
            {
                if (ViewState["DT_Below"] == null)
                    return null;
                else
                    return (DataTable)(ViewState["DT_Below"]);
            }

            set { ViewState["DT_Below"] = value; }
        }

        protected DataTable DT_Tree
        {
            get
            {
                if (ViewState["DT_Tree"] == null)
                    return null;
                else
                    return (DataTable)(ViewState["DT_Tree"]);
            }

            set { ViewState["DT_Tree"] = value; }
        }

        protected String AllowMove
        {
            get
            {
                if (ViewState["AllowMove"] == null)
                    return string.Empty;
                else
                    return (String)(ViewState["AllowMove"]);
            }

            set { ViewState["AllowMove"] = value; }
        }

        protected String AllowInto
        {
            get
            {
                if (ViewState["AllowInto"] == null)
                    return string.Empty;
                else
                    return (String)(ViewState["AllowInto"]);
            }

            set { ViewState["AllowInto"] = value; }
        }
        #endregion

        #region Page_Load()
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Parameter
            string caseSet = this.RequestQueryString("CaseSet");
            string userSet = this.RequestQueryString("UserSet");
            string mode = this.RequestQueryString("Mode");
            string priID = this.RequestQueryString("PriID");
            //string fileID = this.RequestQueryString("FileID");
            #endregion

            XDocuments.ConfigurationNodeInfos configNodeInfos = null;
            string strSql = string.Empty;
            string viewerMode = caseSet == "-1" ? "S" : "B";

            DataTable dtToolBar = null;
            DataTable dtCaseData = null;
            DataTable dtBarcode = null;

            try
            {
                Viewer_Doc = new ViewerDocument();
                XElement elChangingtec = (XElement)Viewer_Doc.XmlDoc.FirstNode;

                #region NODE Viewer(Changingtec.ChildNodes[1])

                //<!--UploadMode : 0=在線上傳, 1=離線上傳 DocClassDefMode : 0=總表模式, 1=明台模式 Type ： S=掃描,B=調閱 -->
                string[] viewerAttribute = new string[3];

                viewerAttribute[0] = this.GetSystem("UploadMode");

                if (!string.IsNullOrEmpty(mode) && mode == "A")
                {
                    viewerAttribute[1] = "1";
                }
                else
                {
                    viewerAttribute[1] = "0";
                }

                viewerAttribute[2] = viewerMode;

                XElement elViewer = Viewer_Doc.CreateViewerElement(viewerAttribute);

                elChangingtec.AddFirst(elViewer);

                #endregion

                configNodeInfos = new XDocuments.ConfigurationNodeInfos();

                #region Node User&Unit
                Session[SessionName.UserID] = userSet;

                configNodeInfos.UserID = this.UserInfo.UserID;
                configNodeInfos.UserName = this.UserInfo.RealName;
                configNodeInfos.RoleName = this.UserInfo.RoleName;

                configNodeInfos.UnitID = "WDA";
                configNodeInfos.UnitName = "勞發署";
                configNodeInfos.UnitCode = "WDA";
                #endregion

                #region Node ImportFileFormat&ExportFileFormat
                configNodeInfos.ImportFileFormat = this.GetSystem("ImportFileFormat");
                configNodeInfos.ExportFileFormat = this.GetSystem("ExportFileFormat");
                #endregion

                #region Node Image
                configNodeInfos.ImageIJZip = this.GetSystem("ImageIJZip");
                configNodeInfos.ImageISize = this.GetSystem("ImageISize");
                configNodeInfos.ImageIThreshold = this.GetSystem("ImageIThreshold");
                configNodeInfos.ImageIType = this.GetSystem("ImageIType");
                #endregion

                #region Node Service
                configNodeInfos.ServiceURL = this.GetSystem("ServiceURL");
                configNodeInfos.ServiceClassName = this.GetSystem("ServiceClassName");
                #endregion

                #region Node Print
                configNodeInfos.TextFont = "微軟正黑體, 8pt";
                configNodeInfos.Position = "5";
                #endregion

                #region Node Watermark
                configNodeInfos.WatermarkSwitch = "Y";
                configNodeInfos.WatermarkViewer = "N";
                configNodeInfos.WatermarkTextSwitch = "Y";
                configNodeInfos.WatermarkTextAngle = "15";
                configNodeInfos.WatermarkTextOpenness = "22";
                configNodeInfos.WatermarkTextPaddingTop = "0";
                configNodeInfos.WatermarkTextPaddingLeft = "0";
                configNodeInfos.WatermarkTextPaddingRight = "0";
                configNodeInfos.WatermarkTextPaddingBottom = "0";
                configNodeInfos.WatermarkTextFont = "微軟正黑體, 24pt";
                configNodeInfos.WatermarkTextColor = "#000000";
                configNodeInfos.WatermarkText = "勞發署";
                configNodeInfos.WatermarkTextPosition ="3";

                configNodeInfos.WatermarkImageSwitch = "N";
                configNodeInfos.WatermarkImageAngle = string.Empty;
                configNodeInfos.WatermarkImageOpenness = string.Empty;
                configNodeInfos.WatermarkImagePaddingTop = string.Empty;
                configNodeInfos.WatermarkImagePaddingLeft = string.Empty;
                configNodeInfos.WatermarkImagePaddingRight = string.Empty;
                configNodeInfos.WatermarkImagePaddingBottom = string.Empty;
                configNodeInfos.WatermarkImageUrl = string.Empty;
                configNodeInfos.WatermarkImagePosition = string.Empty;
                #endregion

                XElement elConfig = Viewer_Doc.CreateConfigurationElement(configNodeInfos);

                elViewer.Add(elConfig);

                #region NODE ToolBar Group (Viewer.ChildNodes[2])

                strSql = this.Select.GetViewPrivilegeName();

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                dtToolBar = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                if (string.IsNullOrEmpty(priID))//沒帶權限時由Role權限決定
                {
                    strSql = this.Select.GetRoleViewPrivilege(this.UserInfo.RoleID);

                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                    DataTable dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                    priID = this.DBConn.GeneralSqlCmd.ExecuteByColumnName(strSql, "VIEWERPRIVCOUNT");

                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, string.Format("viewerPriv：{0}", dt.Rows[0]["VIEWERPRIVCOUNT"].ToString()));
                }

                int priv = 0;
                int.TryParse(priID, out priv);

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, string.Format("priv：{0}", priv));

                dtToolBar.Columns.Add("Visible", typeof(string));

                int value = 2;
                for (int i = 0; i < dtToolBar.Rows.Count; i++)
                {
                    int power = (int)Math.Pow(value, i);

                    string Visible = (priv & power) == power ? "Y" : "N";

                    //this.WriteLog(global::Log.Mode.LogMode.DEBUG, string.Format("XmlName：{0},Visible：{1}", dtToolBar.Rows[i]["XmlName"].ToString(), Visible));

                    dtToolBar.Rows[i]["Visible"] = Visible;
                }

                if (dtToolBar.Rows.Count > 0)
                {
                    if (caseSet == "-1")
                    {
                        //dtToolBar.Rows[4]["Visible"] = "N";
                        //dtToolBar.Rows[5]["Visible"] = "N";
                        //dtToolBar.Rows[6]["Visible"] = "N";
                        //dtToolBar.Rows[8]["Visible"] = "N";
                        //dtToolBar.Rows[9]["Visible"] = "N";
                        dtToolBar.Rows[13]["Visible"] = "N";
                    }
                    else//非掃描模式，先強制關閉掃描功能
                    {
                        dtToolBar.Rows[2]["Visible"] = "N";
                        dtToolBar.Rows[3]["Visible"] = "N";
                        //dtToolBar.Rows[4]["Visible"] = "N";
                        //dtToolBar.Rows[8]["Visible"] = "N";
                        //dtToolBar.Rows[9]["Visible"] = "N";
                        //dtToolBar.Rows[12]["Visible"] = "N";
                        dtToolBar.Rows[13]["Visible"] = "N";
                    }

                    XElement elUISetting = Viewer_Doc.CreateUISettingElement(dtToolBar);

                    elViewer.Add(elUISetting);
                }
                #endregion

                #region CaseRootNode
                XElement elDataSetting = new XElement("DataSetting");
                XElement elSubChangingtec = new XElement("Changingtec");

                elDataSetting.Add(elSubChangingtec);
                XElement elItemRoot = new XElement("Item");

                elItemRoot.SetAttributeValue("ID", "0");
                elItemRoot.SetAttributeValue("ParentID", "-1");
                elItemRoot.SetAttributeValue("Name", "檔案總管");
                elItemRoot.SetAttributeValue("NodeType", "0");
                elItemRoot.SetAttributeValue("AllowMove", "N");
                elItemRoot.SetAttributeValue("AllowInto", "N");
                elItemRoot.SetAttributeValue("LapID", "0");
                elItemRoot.SetAttributeValue("Keep", "Y");

                elSubChangingtec.Add(elItemRoot);
                #endregion

                #region 案件內層處理
                if (viewerMode == "S")
                {
                    #region 暫存區
                    XElement elItemTemp = new XElement("Item");

                    elItemTemp.SetAttributeValue("ID", "99");
                    elItemTemp.SetAttributeValue("ParentID", "0");
                    elItemTemp.SetAttributeValue("Name", "暫存區");
                    elItemTemp.SetAttributeValue("NodeType", "9");
                    elItemTemp.SetAttributeValue("AllowMove", "N");
                    elItemTemp.SetAttributeValue("AllowInto", "N");
                    elItemTemp.SetAttributeValue("Keep", "Y");

                    elItemRoot.Add(elItemTemp);
                    #endregion
                }
                else
                {
                    XElement elBarcodeList = new XElement("BarcodeList");

                    string[] caseArray = caseSet.Split(';');

                    for (int a = 0; a < caseArray.Length; a++)
                    {
                        string caseID = caseArray[a].Remove(caseArray[a].IndexOf('('));//取得參數中的案件代碼

                        string where = string.Format("And CaseID ='{0}'", caseID);

                        strSql = this.Select.GetCaseTable(where);

                        this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                        dtCaseData = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                        strSql = this.Select.GetBARCODETABLE(where);

                        this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                        dtBarcode = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                        if (dtCaseData.Rows.Count > 0)
                        {
                            XElement elCaseNode = new XElement("Item");

                            XElement elBarcode = new XElement("Barcode");

                            #region 案件層處理
                            elCaseNode.SetAttributeValue("ID", caseID);
                            elCaseNode.SetAttributeValue("ParentID", "0");
                            elCaseNode.SetAttributeValue("LapID", "-1");
                            elCaseNode.SetAttributeValue("Name", caseID);
                            elCaseNode.SetAttributeValue("NodeType", "1");
                            elCaseNode.SetAttributeValue("AllowMove", "N");
                            elCaseNode.SetAttributeValue("AllowInto", "*");
                            elCaseNode.SetAttributeValue("Keep", "Y");
                            elCaseNode.SetAttributeValue("IsCase", "Y");
                            elCaseNode.SetAttributeValue("Modify", "N");
                            elCaseNode.SetAttributeValue("CreateTime", dtCaseData.Rows[0]["CreateTime"].ToString());
                            #endregion

                            if (!string.IsNullOrEmpty(mode) && mode =="A")
                            {
                                #region 子節點處理(所有影像版本)
                                where = string.Format("And ft.CaseID ='{0}'", caseID);

                                strSql = this.Select.GetAllFileTable(where);

                                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                                this.DT_File = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                                if (this.DT_File.Rows.Count > 0)
                                {
                                    string mVaule = caseArray[a].Substring(caseArray[a].IndexOf('(') + 1, caseArray[a].IndexOf(')') - caseArray[a].IndexOf('(') - 1);

                                    if (mVaule == "*")
                                    {
                                        Viewer_Doc.CreateAllFileItemsElement(ref elCaseNode, this.DT_File, caseID);
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region 子節點處理
                                where = string.Format("And ft.CaseID ='{0}'", caseID);

                                strSql = this.Select.GetFileTable(where);

                                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                                this.DT_File = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                                if (this.DT_File.Rows.Count > 0)
                                {
                                    string mVaule = caseArray[a].Substring(caseArray[a].IndexOf('(') + 1, caseArray[a].IndexOf(')') - caseArray[a].IndexOf('(') - 1);

                                    if (mVaule == "*")
                                    {
                                        Viewer_Doc.CreateFileItemsElement(ref elCaseNode, this.DT_File, caseID);
                                    }
                                }
                                #endregion
                            }

                            #region 加入垃圾桶
                            string guidID = Guid.NewGuid().ToString();

                            XElement elItemTra = new XElement("Item");

                            elItemTra.SetAttributeValue("ID", guidID);
                            elItemTra.SetAttributeValue("ParentID", caseID);
                            elItemTra.SetAttributeValue("Name", "垃圾桶");
                            elItemTra.SetAttributeValue("NodeType", "9");
                            elItemTra.SetAttributeValue("AllowMove", "N");
                            elItemTra.SetAttributeValue("AllowInto", "N");
                            elItemTra.SetAttributeValue("Keep", "Y");

                            elCaseNode.Add(elItemTra);

                            DataRow[] drDocArray = this.DT_File.Select("FileStatus = 99");

                            Viewer_Doc.CreateFileItemsElement(ref elItemTra, drDocArray, guidID);
                            #endregion

                            elItemRoot.Add(elCaseNode);

                            #region Barcode
                            elBarcode.SetAttributeValue("Binding", caseID);
                            elBarcode.SetAttributeValue("DateTime", dtBarcode.Rows[0]["CreateTime"].ToString());

                            Viewer_Doc.CreateBarcodeItemsElement(ref elBarcode, dtBarcode);
                            elBarcodeList.Add(elBarcode);
                            #endregion
                        }
                        else { throw new Exception("查無caseSet"); }
                    }

                    #region BarcodeList
                    elViewer.Add(elBarcodeList);
                    #endregion
                }

                elViewer.Add(elDataSetting);

                #endregion

                this.Response.Clear();
                this.Response.ContentType = "text/xml";
                string xmlString = String.Concat(Viewer_Doc.XmlDoc.Declaration.ToString(), Viewer_Doc.XmlDoc.ToString());
                this.Response.Write(xmlString);
                this.Response.Flush();
            }
            catch (Exception ex)
            {
                this.Response.Write(ex.Message);
                this.WriteLog(global::Log.Mode.LogMode.ERROR, string.Format("ViewerXml.Exception:{0}", ex.ToString()));
            }
            finally
            {
                if (dtToolBar != null) { dtToolBar.Dispose(); dtToolBar = null; }
                if (dtCaseData != null) { dtCaseData.Dispose(); dtCaseData = null; }
                this.DBConn.Dispose(); this.DBConn = null;
            }
            this.Response.End();
        }
        #endregion
    }
}