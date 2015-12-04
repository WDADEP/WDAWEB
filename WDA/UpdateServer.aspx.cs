using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using WDA.Class;

namespace WDA
{
    public partial class UpdateServer : PageUtility
    {
        #region Properties
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
        #endregion

        #region Page_Load()
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Viewer_Doc = new ViewerDocument();

                string classID = this.GetSystem("ViewerClassID");

                XElement elChangingtec = (XElement)Viewer_Doc.XmlDoc.FirstNode;

                #region ScanViewer
                XElement elViewerItem = new XElement("Item");

                elViewerItem.SetAttributeValue("ClassID", this.GetSystem("ViewerClassID"));

                elChangingtec.Add(elViewerItem);

                XElement elViewerVersion = new XElement("Version");
                elViewerVersion.Value = this.GetSystem("ViewerVersion");

                XElement elViewerUploadServerURL = new XElement("UploadServerURL");
                elViewerUploadServerURL.Value = this.GetSystem("ViewerUploadServerURL");

                elViewerItem.Add(elViewerVersion);
                elViewerItem.Add(elViewerUploadServerURL);
                #endregion

                this.Response.Clear();
                this.Response.ContentType = "text/xml";
                this.Response.Write(Viewer_Doc.XmlDoc.ToString());
                this.Response.Flush();
            }
            catch (Exception ex)
            {
                this.Response.Write(ex.Message);

                this.WriteLog(global::Log.Mode.LogMode.ERROR, string.Format("ViewerXml.Exception:{0}", ex.ToString()));
            }
            finally
            {
                if (Viewer_Doc != null) Viewer_Doc = null;

                this.DBConn.Dispose(); this.DBConn = null;
            }
            this.Response.End();
        }
        #endregion
    }
}