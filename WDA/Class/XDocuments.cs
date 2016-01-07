using DBLibUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace WDA.Class
{
    public abstract class XDocuments
    {
        #region InitXDocuments
        /// <summary>
        /// 初始化 XDocument 物件
        /// </summary>
        /// <param name="FirstElement">第一層節點名稱</param>
        public XDocument InitXDocuments(string FirstElement)
        {
            return this.InitXDocuments(FirstElement, string.Empty);
        }
        /// <summary>
        /// 初始化 XmlDocument 物件
        /// </summary>
        /// <param name="FirstElement">第一層節點名稱</param>
        /// <param name="XmlNamesPace">命名空間</param>
        public XDocument InitXDocuments(string FirstElement, string XmlNamesPace)
        {
            XDocument xmlDoc = new XDocument(new XDeclaration(new XDeclaration("1.0", "UTF-8", null)));
            XElement firstElement = XmlNamesPace.Length == 0 ? new XElement(FirstElement) : new XElement(XmlNamesPace + FirstElement);
            xmlDoc.AddFirst(firstElement);
            return xmlDoc;
        }
        #endregion

        #region CreateChildNode()
        /// <summary>
        /// 建立新節點
        /// </summary>
        /// <param name="ParentNode">父節點</param>
        /// <param name="XmlNamesPace">命名空間</param>
        /// <param name="NodeName">節點名稱</param>
        /// <param name="InnerText">內容</param>
        /// <returns></returns>
        protected XElement CreateChildNode(XElement ParentNode, string NodeName, string InnerText)
        {
            return this.CreateChildNode(ParentNode, string.Empty, NodeName, InnerText);
        }
        /// <summary>
        /// 建立新節點
        /// </summary>
        /// <param name="ParentNode">父節點</param>
        /// <param name="XmlNamesPace">命名空間</param>
        /// <param name="NodeName">節點名稱</param>
        /// <param name="InnerText">內容</param>
        /// <returns></returns>
        protected XElement CreateChildNode(XElement ParentNode, string XmlNamesPace, string NodeName, string InnerText)
        {
            XElement el = XmlNamesPace.Length == 0 ? new XElement(NodeName, InnerText) : new XElement(XNamespace.Get(XmlNamesPace) + NodeName, InnerText);
            ParentNode.Add(el);
            return el;
        }
        #endregion

        #region CreateAttribute()
        /// <summary>
        /// 建立新節點屬性
        /// </summary>
        /// <param name="ParentNode">父節點</param>
        /// <param name="XmlNamesPace">命名空間</param>
        /// <param name="AttrName">屬性名稱</param>
        /// <param name="AttrValue">屬性內容</param>
        /// <returns></returns>
        protected XElement CreateAttribute(XElement ParentNode, string AttrName, string AttrValue)
        {
            return this.CreateAttribute(ParentNode, string.Empty, AttrName, AttrValue);
        }
        /// <summary>
        /// 建立新節點屬性
        /// </summary>
        /// <param name="ParentNode">父節點</param>
        /// <param name="XmlNamesPace">命名空間</param>
        /// <param name="AttrName">屬性名稱</param>
        /// <param name="AttrValue">屬性內容</param>
        /// <returns></returns>
        protected XElement CreateAttribute(XElement ParentNode, string XmlNamesPace, string AttrName, string AttrValue)
        {
            XName attrName = XmlNamesPace.Length == 0 ? AttrName : XNamespace.Get(XmlNamesPace) + AttrName;

            ParentNode.SetAttributeValue(attrName, AttrValue);
            return ParentNode;
        }
        #endregion

        #region FirstName
        /// <summary>
        /// First name
        /// </summary>
        private string _FirstName = string.Empty;
        /// <summary>
        /// First name
        /// </summary>
        /// <returns></returns>
        public string FirstName
        {
            get { return this._FirstName; }
            set { this._FirstName = value; }
        }
        #endregion

        #region ConfigurationNodeInfos
        /// <summary>
        /// 文件項目節點
        /// </summary>
        public class ConfigurationNodeInfos
        {
            public string UserID = string.Empty;
            public string UserName = string.Empty;
            public string RoleName = string.Empty;

            public string UnitID = string.Empty;
            public string UnitCode = string.Empty;
            public string UnitName = string.Empty;

            public string ImportFileFormat = string.Empty;
            public string ExportFileFormat = string.Empty;

            public string ImageIJZip = string.Empty;
            public string ImageISize = string.Empty;
            public string ImageIThreshold = string.Empty;
            public string ImageIType = string.Empty;

            public string BarcodeTypeSplit = string.Empty;
            public string BarcodeTypeValue = string.Empty;

            //public string SegmentModeDefault = string.Empty;
            //public string SegmentModeDelete = string.Empty;
            //public string SegmentModeShowUI = string.Empty;

            //public string IdentifyAreaLeftTop = string.Empty;
            //public string IdentifyAreaLeftBottom = string.Empty;
            //public string IdentifyAreaRightTop = string.Empty;
            //public string IdentifyAreaRightBottom = string.Empty;

            public string ServiceURL = string.Empty;
            public string ServiceClassName = string.Empty;

            //public string ConnectTest = string.Empty;
            //public string GetImage = string.Empty;
            //public string UploadData = string.Empty;
            //public string SetImage = string.Empty;
            //public string SetTempImage = string.Empty;
            //public string DeleteImage = string.Empty;
            //public string RemoveTempAreaImage = string.Empty;
            //public string GetDrawInfo = string.Empty;
            //public string GetTempArea = string.Empty;
            //public string GetCaseFormat = string.Empty;

            //public string JavaScriptMethodName = string.Empty;

            //public string BatchAPDirPath = string.Empty;
            //public string BatchAPName = string.Empty;
            //public string BatchArguments = string.Empty;
            //public string BatchUploadTime = string.Empty;

            //public string SMTPServer = string.Empty;
            //public string Port = string.Empty;
            //public string ID = string.Empty;
            //public string PWD = string.Empty;
            //public string Sender = string.Empty;
            //public string Subject = string.Empty;
            //public string Address = string.Empty;

            //public string ClientLogPath = string.Empty;
            //public string KeepDay = string.Empty;

            public string TextFont = string.Empty;
            public string Position = string.Empty;

            public string WatermarkSwitch = string.Empty;
            public string WatermarkViewer = string.Empty;
            public string WatermarkTextSwitch = string.Empty;
            public string WatermarkTextAngle = string.Empty;
            public string WatermarkTextOpenness = string.Empty;
            public string WatermarkTextPaddingTop = string.Empty;
            public string WatermarkTextPaddingLeft = string.Empty;
            public string WatermarkTextPaddingRight = string.Empty;
            public string WatermarkTextPaddingBottom = string.Empty;
            public string WatermarkTextFont = string.Empty;
            public string WatermarkTextColor = string.Empty;
            public string WatermarkText = string.Empty;
            public string WatermarkTextPosition = string.Empty;
            public string WatermarkImageSwitch = string.Empty;
            public string WatermarkImageAngle = string.Empty;
            public string WatermarkImageOpenness = string.Empty;
            public string WatermarkImagePaddingTop = string.Empty;
            public string WatermarkImagePaddingLeft = string.Empty;
            public string WatermarkImagePaddingRight = string.Empty;
            public string WatermarkImagePaddingBottom = string.Empty;
            public string WatermarkImageUrl = string.Empty;
            public string WatermarkImagePosition = string.Empty;
        }
        #endregion
    }

    #region ViewerDocument
    /// <summary>
    /// Viewer Document
    /// </summary>
    public class ViewerDocument : XDocuments
    {
        #region DBConn
        /// <summary>
        /// 資料庫物件
        /// </summary>
        private DBLib.IDBConn _DBConn = null;
        /// <summary>
        /// 資料庫物件
        /// </summary>
        protected DBLib.IDBConn DBConn
        {
            get
            {
                if (this._DBConn == null) this._DBConn = new DBLib.DBConn(); return this._DBConn;
            }
            set
            {
                this._DBConn = value;
            }
        }
        #endregion

        #region SqlCommand
        /// <summary>
        /// Sql Command (Select)
        /// </summary>
        private SqlCommand.Select _Select = null;
        /// <summary>
        /// Sql Command (Select)
        /// </summary>
        protected SqlCommand.Select Select
        {
            get
            {
                if (this._Select == null) this._Select = new SqlCommand.Select(); return this._Select;
            }
        }
        /// <summary>
        /// Sql Command (Update)
        /// </summary>
        private SqlCommand.Update _Update = null;
        /// <summary>
        /// Sql Command (Update)
        /// </summary>
        protected SqlCommand.Update Update
        {
            get
            {
                if (this._Update == null) this._Update = new SqlCommand.Update(); return this._Update;
            }
        }
        /// <summary>
        /// Sql Command (Delete)
        /// </summary>
        private SqlCommand.Delete _Delete = null;
        /// <summary>
        /// Sql Command (Delete)
        /// </summary>
        protected SqlCommand.Delete Delete
        {
            get
            {
                if (this._Delete == null) this._Delete = new SqlCommand.Delete(); return this._Delete;
            }
        }
        /// <summary>
        /// Sql Command (Insert)
        /// </summary>
        private SqlCommand.Insert _Insert = null;
        /// <summary>
        /// Sql Command (Insert)
        /// </summary>
        protected SqlCommand.Insert Insert
        {
            get
            {
                if (this._Insert == null) this._Insert = new SqlCommand.Insert(); return this._Insert;
            }
        }
        #endregion

        #region XmlDoc
        /// <summary>
        /// Xml Document
        /// </summary>
        private XDocument _XmlDoc = null;
        /// <summary>
        /// Xml Document
        /// </summary>
        public XDocument XmlDoc
        {
            get
            {
                if (this._XmlDoc == null) this._XmlDoc = this.InitXDocuments(this.FirstName);

                return this._XmlDoc;
            }
            set { this._XmlDoc = value; }
        }
        #endregion

        #region 建構函式
        /// <summary>
        /// 
        /// </summary>
        public ViewerDocument() { this.FirstName = "Changingtec"; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="XmlDoc">XmlDocument</param>
        public ViewerDocument(XDocument XmlDoc) { this.FirstName = "Changingtec"; this.XmlDoc = XmlDoc; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="XmlDoc">XmlDocument</param>
        public ViewerDocument(ref XDocument XmlDoc) { this.FirstName = "Changingtec"; XmlDoc = this.XmlDoc; }
        #endregion

        #region CreateViewerElement()
        /// <summary>
        /// 建立Changingtec下第一層
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public XElement CreateViewerElement(string[] Attribute)
        {
            XElement el = new XElement("Viewer");

            this.CreateAttribute(el, "UploadMode", Attribute[0]);
            this.CreateAttribute(el, "DocClassDefMode", Attribute[1]);
            this.CreateAttribute(el, "Type", Attribute[2]);

            return el;
        }
        #endregion

        #region CreateViewerElementByDocNumbe()
        /// <summary>
        /// 建立Changingtec下第一層
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public XElement CreateViewerElementByDocNumbe(string[] Attribute, string DocNumber)
        {
            XElement el = new XElement("Viewer");

            this.CreateAttribute(el, "UploadMode", Attribute[0]);
            this.CreateAttribute(el, "DocClassDefMode", Attribute[1]);
            this.CreateAttribute(el, "Type", Attribute[2]);
            this.CreateAttribute(el, "DocNumber", DocNumber);

            return el;
        }
        #endregion

        #region CreateConfigurationElement()
        /// <summary>
        /// 建立Tag Configuation
        /// </summary>
        /// <param name="HT"></param>
        /// <returns></returns>
        public XElement CreateConfigurationElement(ConfigurationNodeInfos Infos)
        {
            XElement el = new XElement("Configuration");

            #region User Node
            XElement elUser = new XElement("User");

            el.Add(elUser);

            this.CreateAttribute(elUser, "ID", Infos.UserID);
            this.CreateAttribute(elUser, "Name", Infos.RoleName);
            this.CreateAttribute(elUser, "Account", Infos.UserName);
            #endregion

            #region Unit Node
            XElement elUnit = new XElement("Unit");

            el.Add(elUnit);

            this.CreateAttribute(elUnit, "ID", Infos.UnitID.ToString());
            this.CreateAttribute(elUnit, "NO", Infos.UnitCode);
            this.CreateAttribute(elUnit, "Name", Infos.UnitName);
            #endregion

            #region Import Node
            XElement elImport = new XElement("Import");

            el.Add(elImport);

            this.CreateAttribute(elImport, "FileFormat", Infos.ImportFileFormat);

            #endregion

            #region Export Node
            XElement elExport = new XElement("Export");

            el.Add(elExport);

            this.CreateAttribute(elExport,"FileFormat", Infos.ExportFileFormat);

            #endregion

            #region Image Node
            XElement elImage = new XElement("Image");

            el.Add(elImage);

            this.CreateAttribute(elImage, "IType", Infos.ImageIType);
            this.CreateAttribute(elImage, "IJZip", Infos.ImageIJZip);
            this.CreateAttribute(elImage, "ISize", Infos.ImageISize);
            this.CreateAttribute(elImage, "IThreshold", Infos.ImageIThreshold);
            #endregion

            #region Service Node
            XElement elService = new XElement("Service");

            el.Add(elService);

            this.CreateAttribute( elService, "URL", Infos.ServiceURL);
            this.CreateAttribute( elService, "ClassName", Infos.ServiceClassName);
            #endregion

            #region Service Node
            XElement elPrint = new XElement("Print");

            el.Add(elPrint);

            this.CreateAttribute(elPrint, "TextFont", Infos.TextFont);
            this.CreateAttribute(elPrint, "Position", Infos.Position);
            #endregion

            #region Watermark Node
            XElement elWatermark = new XElement("Watermark");

            el.Add(elWatermark);

            this.CreateAttribute(elWatermark, "Switch", Infos.WatermarkSwitch);
            this.CreateAttribute(elWatermark, "Viewer", Infos.WatermarkViewer);


            #region Text
            XElement elText = new XElement("Text");

            elWatermark.Add(elText);
            this.CreateAttribute(elText, "Switch", Infos.WatermarkTextSwitch);

            XElement elWatermarkTextAngle = new XElement("Angle");
            elWatermarkTextAngle.Value = Infos.WatermarkTextAngle;
            elText.Add(elWatermarkTextAngle);

            XElement elWatermarkTextOpenness = new XElement("Openness");
            elWatermarkTextOpenness.Value = Infos.WatermarkTextOpenness;
            elText.Add(elWatermarkTextOpenness);

            XElement elWatermarkTextPadding = new XElement("Padding");
            this.CreateAttribute(elWatermarkTextPadding, "Top", Infos.WatermarkTextPaddingTop);
            this.CreateAttribute(elWatermarkTextPadding, "Left", Infos.WatermarkTextPaddingLeft);
            this.CreateAttribute(elWatermarkTextPadding, "Right", Infos.WatermarkTextPaddingRight);
            this.CreateAttribute(elWatermarkTextPadding, "Bottom", Infos.WatermarkTextPaddingBottom);
            elText.Add(elWatermarkTextPadding);

            XElement elWatermarkTextFont = new XElement("TextFont");
            elWatermarkTextFont.Value = Infos.WatermarkTextFont;
            elText.Add(elWatermarkTextFont);

            XElement elWatermarkTextColor = new XElement("TextColor");
            elWatermarkTextColor.Value = Infos.WatermarkTextColor;
            elText.Add(elWatermarkTextColor);

            XElement elWatermarkText = new XElement("Text");
            elWatermarkText.Value = Infos.WatermarkText;
            elText.Add(elWatermarkText);

            XElement elWatermarkTextPosition = new XElement("Position");
            elWatermarkTextPosition.Value = Infos.WatermarkTextPosition;
            elText.Add(elWatermarkTextPosition);
            #endregion

            #region Image
            XElement elWatermarkImage = new XElement("Image");

            elWatermark.Add(elWatermarkImage);

            this.CreateAttribute(elWatermarkImage, "Switch", Infos.WatermarkImageSwitch);

            XElement elWatermarkImageAngle = new XElement("Angle");
            elWatermarkImageAngle.Value = Infos.WatermarkImageAngle;
            elWatermarkImage.Add(elWatermarkImageAngle);

            XElement elWatermarkImageOpenness = new XElement("Openness");
            elWatermarkImageOpenness.Value = Infos.WatermarkImageOpenness;
            elWatermarkImage.Add(elWatermarkImageOpenness);

            XElement elWatermarkImagePadding = new XElement("Padding");
            this.CreateAttribute(elWatermarkImagePadding, "Top", Infos.WatermarkImagePaddingTop);
            this.CreateAttribute(elWatermarkImagePadding, "Left", Infos.WatermarkImagePaddingLeft);
            this.CreateAttribute(elWatermarkImagePadding, "Right", Infos.WatermarkImagePaddingRight);
            this.CreateAttribute(elWatermarkImagePadding, "Bottom", Infos.WatermarkImagePaddingBottom);
            elWatermarkImage.Add(elWatermarkImagePadding);

            XElement elWatermarkImageUrl = new XElement("ImageURL");
            elWatermarkImageUrl.Value = Infos.WatermarkImageUrl;
            elWatermarkImage.Add(elWatermarkImageUrl);

            XElement elWatermarkImagePosition = new XElement("Position");
            elWatermarkImagePosition.Value = Infos.WatermarkImagePosition;
            elWatermarkImage.Add(elWatermarkImagePosition);
            #endregion

            #endregion

            #region DeleteImage Node
            XElement elDeleteImageMethod = new XElement("DeleteImage");

            el.Add(elDeleteImageMethod);

            this.CreateAttribute(elDeleteImageMethod, "ToGarbage", "N");
            this.CreateAttribute(elDeleteImageMethod, "CanDeleteGarbage", "Y");
            #endregion

            return el;
        }
        #endregion

        #region CreateUISettingElement()
        /// <summary>
        /// 建立Tag UISetting
        /// </summary>
        /// <returns></returns>
        public XElement CreateUISettingElement(DataTable DT)
        {
            XElement el = new XElement("UISetting");

            XElement elToolBar = new XElement(DT.Rows[0]["XmlName"].ToString());

            string visible = DT.Rows[0]["Visible"].ToString();

            this.CreateAttribute(elToolBar, "Visible", visible);

            string PrivName = string.Empty;

            for (int i = 1; i < DT.Rows.Count; i++)
            {
                PrivName = DT.Rows[i]["XmlName"].ToString();
                visible = DT.Rows[i]["Visible"].ToString();

                XElement elGroup = new XElement("Group");

                this.CreateAttribute(elGroup, "Name", PrivName);
                this.CreateAttribute(elGroup, "Visible", visible);

                elToolBar.Add(elGroup);
            }

            el.Add(elToolBar);
            return el;
        }
        #endregion

        #region CreateDrawSettingElement()
        /// <summary>
        /// 建立Tag IndexClass
        /// </summary>
        /// <returns></returns>
        public XElement CreateDrawSettingElement(Hashtable HT)
        {
            XElement elDrawSetting = new XElement("DrawSetting");

            this.CreateAttribute(elDrawSetting, "Show", HT["Show"].ToString());
            this.CreateAttribute(elDrawSetting, "Edit", HT["Edit"].ToString());
            this.CreateAttribute(elDrawSetting, "Edited", HT["Edited"].ToString());
            this.CreateAttribute(elDrawSetting, "Delete", HT["Delete"].ToString());

            return elDrawSetting;
        }
        #endregion

        #region CreateIndexClassElement()
        /// <summary>
        /// 建立Tag IndexClass
        /// </summary>
        /// <returns></returns>
        public XElement CreateIndexClassElement(DataTable DT,string Regular)
        {
            XElement elIndexClass = new XElement("IndexClass");

            string ID = string.Empty;
            string Name = string.Empty;
            string RuleID = string.Empty;
            string CanEmpty = string.Empty;
            string IndexType = string.Empty;
            string Multiline = string.Empty;
            string BarcodePreFixedName = string.Empty;
            int Attrib = 0;

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                Attrib = Convert.ToInt32(DT.Rows[i]["Attrib"].ToString());
                ID = DT.Rows[i]["IndexClassID"].ToString();
                Name = DT.Rows[i]["IndexClassName"].ToString();
                RuleID = DT.Rows[i]["CheckRuleID"].ToString();
                CanEmpty = (Attrib & 2) > 0 ? "N" : "Y";
                IndexType = "1";
                Multiline = "N";
                BarcodePreFixedName = DT.Rows[i]["BarcodePreFixedName"].ToString();

                XElement elItem = new XElement("Item");

                this.CreateAttribute(elItem, "ID", ID);
                this.CreateAttribute(elItem, "Name", Name);
                this.CreateAttribute(elItem, "RuleID", RuleID);
                this.CreateAttribute(elItem, "CanEmpty", CanEmpty);
                this.CreateAttribute(elItem, "Type", IndexType);
                this.CreateAttribute(elItem, "Multiline", Multiline);
                this.CreateAttribute(elItem, "BarcodePreFixedName", BarcodePreFixedName);
                this.CreateAttribute(elItem, "Regular", Regular);

                elIndexClass.Add(elItem);
            }
            return elIndexClass;
        }
        #endregion

        #region CreateIndexGroupNode()
        /// <summary>
        /// 建立案件下的IndexGroup層級
        /// </summary>
        /// <returns></returns>
        public XElement CreateIndexGroupNode(DataTable dt, string IndexClassIDSet)
        {
            XElement elIndexGroup = new XElement("IndexGroup");

            string[] IndexClassIDArray = IndexClassIDSet.Split(',');

            for (int a = 0; a < IndexClassIDArray.Length; a++)
            {
                XElement elIndex = new XElement("Index");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["IndexClassID"].ToString() == IndexClassIDArray[a])
                    {
                        this.CreateAttribute(elIndex, "ITID", dt.Rows[i]["ITID"].ToString());
                        this.CreateAttribute(elIndex, "IndexClassID", dt.Rows[i]["IndexClassID"].ToString());
                        this.CreateAttribute(elIndex, "Text", dt.Rows[i]["IndexText"].ToString().EncryptBase64());
                        this.CreateAttribute(elIndex, "CreateUserID", dt.Rows[i]["CreateUserID"].ToString());
                        this.CreateAttribute(elIndex, "CreateDateTime", dt.Rows[i]["CreateTime"].ToString());

                        break;
                    }
                    else
                    {
                        this.CreateAttribute(elIndex, "ITID", string.Empty);
                        this.CreateAttribute(elIndex, "IndexClassID", IndexClassIDArray[a]);
                        this.CreateAttribute(elIndex, "Text", string.Empty);
                        this.CreateAttribute(elIndex, "CreateUserID", string.Empty);
                        this.CreateAttribute(elIndex, "CreateDateTime", string.Empty);
                    }
                }

                elIndexGroup.Add(elIndex);
            }

            return elIndexGroup;
        }
        #endregion

        #region CreateDataSettingElement()
        /// <summary>
        /// 建立案件下的層級
        /// </summary>
        /// <returns></returns>
        public XElement CreateDataSettingElement(Hashtable HT)
        {
            XElement elItem = new XElement("Item"); 

            foreach (string key in HT.Keys)
            {
                this.CreateAttribute(elItem, key, HT[key].ToString().Trim());
            }

            return elItem;
        }
        #endregion

        #region CreateFileItemsElement()
        /// <summary>
        /// 建立Data Setting內的案件層
        /// </summary>
        /// <param name="HT"></param>
        /// <returns></returns>
        public XElement CreateFileItemsElement(ref XElement XmlNodeFileGroup, DataTable DT, string GuidID)
        {
            string where = string.Empty;
            string strSql = string.Empty;
            int seq = 1;
            try
            {
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    XElement elItem = new XElement("Item");

                    this.CreateAttribute(elItem, "Name", DT.Rows[i]["FileName"].ToString());
                    this.CreateAttribute(elItem, "ID", DT.Rows[i]["FileID"].ToString());
                    this.CreateAttribute(elItem, "ParentID", GuidID);
                    this.CreateAttribute(elItem, "NodeType", "11");
                    this.CreateAttribute(elItem, "AllowInto", "N");
                    this.CreateAttribute(elItem, "AllowMove", "Y");
                    this.CreateAttribute(elItem, "Keep", "N");
                    this.CreateAttribute(elItem, "CreateTime", DT.Rows[i]["CreateTime"].ToString());

                    this.CreateAttribute(elItem, "NewItem", "N");
                    this.CreateAttribute(elItem, "UpdateData", "N");
                    this.CreateAttribute(elItem, "UpdateImage", "N");
                    this.CreateAttribute(elItem, "DrawPath", string.Empty);
                    this.CreateAttribute(elItem, "Seq", seq.ToString());

                    XElement elImage = new XElement("Image");

                    this.CreateAttribute(elImage, "Transform", "N");
                    this.CreateAttribute(elImage, "Draw", "N");
                    this.CreateAttribute(elImage, "FileRoot", DT.Rows[i]["FileRoot"].ToString());
                    this.CreateAttribute(elImage, "FilePath", DT.Rows[i]["FilePath"].ToString());

                    elItem.Add(elImage);

                    XmlNodeFileGroup.Add(elItem);
                    seq += 1;
                    
                }
                return XmlNodeFileGroup;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }
        #endregion

        #region CreateFileItemsElement()
        /// <summary>
        /// 建立Data Setting內的案件層
        /// </summary>
        /// <param name="HT"></param>
        /// <returns></returns>
        public XElement CreateFileItemsElement(ref XElement XmlNodeFileGroup, DataRow[] DR, string GuidID)
        {
            string where = string.Empty;
            string strSql = string.Empty;
            int seq = 1;
            try
            {
                foreach (DataRow dr in DR)
                {
                    XElement elItem = new XElement("Item");

                    this.CreateAttribute(elItem, "Name", dr["FileName"].ToString());
                    this.CreateAttribute(elItem, "ID", dr["FileID"].ToString());
                    this.CreateAttribute(elItem, "ParentID", GuidID);
                    this.CreateAttribute(elItem, "NodeType", "11");
                    this.CreateAttribute(elItem, "AllowInto", "N");
                    this.CreateAttribute(elItem, "AllowMove", "Y");
                    this.CreateAttribute(elItem, "Keep", "N");
                    this.CreateAttribute(elItem, "Open", "N");
                    this.CreateAttribute(elItem, "CreateTime", dr["CreateTime"].ToString());

                    this.CreateAttribute(elItem, "NewItem", "N");
                    this.CreateAttribute(elItem, "UpdateData", "N");
                    this.CreateAttribute(elItem, "UpdateImage", "N");
                    this.CreateAttribute(elItem, "DrawPath", string.Empty);
                    this.CreateAttribute(elItem, "Seq", seq.ToString());

                    XElement elImage = new XElement("Image");

                    this.CreateAttribute(elImage, "Transform", "Y");
                    this.CreateAttribute(elImage, "Draw", "N");
                    this.CreateAttribute(elImage, "FileRoot", dr["FileRoot"].ToString());
                    this.CreateAttribute(elImage, "FilePath", dr["FilePath"].ToString());

                    elItem.Add(elImage);

                    XmlNodeFileGroup.Add(elItem);
                    seq += 1;
                }

                return XmlNodeFileGroup;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }
        #endregion

        #region CreateAllFileItemsElement()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="HT"></param>
        /// <returns></returns>
        public XElement CreateAllFileItemsElement(ref XElement XmlNodeFileGroup, DataTable DT, string GuidID)
        {
            string where = string.Empty;
            string strSql = string.Empty;
            int seq = 1;
             int seqSub = 1;
            try
            {
                DataRow[] fileRows = DT.Select("FileStatus= 1");

                for (int i = 0; i < fileRows.Length; i++)
                {
                    XElement elRootItem = new XElement("Item");

                    this.CreateAttribute(elRootItem, "Name", fileRows[i]["FileName"].ToString());
                    this.CreateAttribute(elRootItem, "ID", fileRows[i]["FileID"].ToString());
                    this.CreateAttribute(elRootItem, "ParentID", GuidID);
                    this.CreateAttribute(elRootItem, "NodeType", "11");
                    this.CreateAttribute(elRootItem, "AllowInto", "N");
                    this.CreateAttribute(elRootItem, "AllowMove", "Y");
                    this.CreateAttribute(elRootItem, "Keep", "N");
                    this.CreateAttribute(elRootItem, "CreateTime", fileRows[i]["CreateTime"].ToString());

                    this.CreateAttribute(elRootItem, "NewItem", "N");
                    this.CreateAttribute(elRootItem, "UpdateData", "N");
                    this.CreateAttribute(elRootItem, "UpdateImage", "N");
                    this.CreateAttribute(elRootItem, "DrawPath", string.Empty);
                    this.CreateAttribute(elRootItem, "Seq", seq.ToString());

                    XElement elImage = new XElement("Image");

                    this.CreateAttribute(elImage, "Transform", "N");
                    this.CreateAttribute(elImage, "Draw", "N");
                    this.CreateAttribute(elImage, "FileRoot", fileRows[i]["FileRoot"].ToString());
                    this.CreateAttribute(elImage, "FilePath", fileRows[i]["FilePath"].ToString());

                    elRootItem.Add(elImage);

                    XmlNodeFileGroup.Add(elRootItem);
                    seq += 1;

                    DataRow[] fileSubRows = DT.Select(string.Format("FileID= '{0}'", fileRows[i]["FileID"].ToString()), "FileStatus");

                    for (int a = 0; a < fileSubRows.Length; a++)
                    {
                        XElement elItem = new XElement("Item");

                        this.CreateAttribute(elItem, "Name", fileRows[i]["FileName"].ToString());
                        this.CreateAttribute(elItem, "ID",Guid.NewGuid().ToString());
                        this.CreateAttribute(elItem, "ParentID", fileRows[i]["FileID"].ToString());
                        this.CreateAttribute(elItem, "NodeType", "14");
                        this.CreateAttribute(elItem, "AllowInto", "N");
                        this.CreateAttribute(elItem, "AllowMove", "N");
                        this.CreateAttribute(elItem, "Keep", "N");
                        this.CreateAttribute(elItem, "CreateTime", fileSubRows[a]["CreateTime"].ToString());

                        this.CreateAttribute(elItem, "NewItem", "N");
                        this.CreateAttribute(elItem, "UpdateData", "N");
                        this.CreateAttribute(elItem, "UpdateImage", "N");
                        this.CreateAttribute(elItem, "DrawPath", string.Empty);
                        this.CreateAttribute(elItem, "Seq", seqSub.ToString());
                        seqSub += 1;


                        XElement elSubImage = new XElement("Image");

                        this.CreateAttribute(elSubImage, "Transform", "N");
                        this.CreateAttribute(elSubImage, "Draw", "N");
                        this.CreateAttribute(elSubImage, "FileRoot", fileSubRows[a]["FileRoot"].ToString());
                        this.CreateAttribute(elSubImage, "FilePath", fileSubRows[a]["FilePath"].ToString());

                        elItem.Add(elSubImage);

                        elRootItem.Add(elItem);
                    }
                }

                return XmlNodeFileGroup;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }
        #endregion

        #region CreateBarcodeItemsElement()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="HT"></param>
        /// <returns></returns>
        public XElement CreateBarcodeItemsElement(ref XElement XmlNodeFileGroup, DataTable DT)
        {
            string where = string.Empty;
            string strSql = string.Empty;
            try
            {
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    XElement elItem = new XElement("Item");

                    elItem.Value = DT.Rows[i]["Barcodevalue"].ToString().EncryptBase64();

                    XmlNodeFileGroup.Add(elItem);
                }
                return XmlNodeFileGroup;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }

    #endregion
}