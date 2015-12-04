using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;


namespace WDA.Class
{
    public class GridViewUtility : PageUtility
    {
        #region DivName
        /// <summary>
        /// Div Name
        /// </summary>
        public static string DivName
        {
            get { return "divPageLayer"; }
        }
        #endregion

        #region ScriptByOnLoadEvent
        /// <summary>
        /// OnLoad 時欲觸發的 JS
        /// </summary>
        private string _ScriptByOnLoadEvent;
        /// <summary>
        /// OnLoad 時欲觸發的 JS
        /// </summary>
        public string ScriptByOnLoadEvent
        {
            get { return this._ScriptByOnLoadEvent; }
            set { this._ScriptByOnLoadEvent = value; }
        }
        #endregion

        #region AddFunctionScript
        /// <summary>
        /// 欲加入的 JS Function
        /// </summary>
        private string _AddFunctionScript;
        /// <summary>
        /// 欲加入的 JS Function
        /// </summary>
        public string AddFunctionScript
        {
            get { return this._AddFunctionScript; }
            set { this._AddFunctionScript = value; }
        }
        #endregion

        #region OnLoadEventScript()
        /// <summary>
        /// 
        /// </summary>
        /// <param name="GV"></param>
        /// <param name="MyLiteral"></param>
        /// <param name="ScriptByOnLoadEvent"></param>
        /// <param name="AddFunctionScript"></param>
        public void OnLoadEventScript(GridView GV, Literal MyLiteral, string ScriptByOnLoadEvent, string AddFunctionScript)
        {
            MyLiteral.Text = PageLoadScriptByReport(DivName + "_" + GV.ClientID, ScriptByOnLoadEvent, AddFunctionScript);
        }
        /// <summary>
        /// 預設載入的 Script Or Style
        /// </summary>
        /// <param name="MyPage">Page Object</param>
        /// <param name="MyDivName">Div Name</param>
        /// <param name="MyScriptByOnLoadEvent">OnLoad 時欲觸發的 JS</param>
        /// <param name="MyAddFunctionScript">欲加入的 JS Function</param>
        /// <returns></returns>
        public string PageLoadScriptByReport(string MyDivName, string MyScriptByOnLoadEvent, string MyAddFunctionScript)
        {
            try
            {
                this.ScriptByOnLoadEvent = MyScriptByOnLoadEvent;

                this.AddFunctionScript = MyAddFunctionScript;

                return PageLoadScriptByReport(MyDivName);
            }
            catch (System.Exception ex) { this.Response.Write("<!--\n" + ex.ToString() + "\n-->\n"); return string.Empty; }
        }
        /// <summary>
        /// 預設載入的 Script Or Style
        /// </summary>
        /// <param name="MyPage">Page Object</param>
        /// <param name="MyDivName">Div Name</param>
        /// <returns></returns>
        public string PageLoadScriptByReport(string MyDivName)
        {
            string script = string.Empty;
            try
            {
                if (MyDivName == null || MyDivName.Length == 0) MyDivName = "pageLayer";

                script = "function initLayer() {\n"
                    + "		window.setTimeout(layerProcessor, 20);\n"
                    + "};\n"
                    + "function layerProcessor () {\n"
                    + "	if (!document.getElementById('" + MyDivName + "')) return;\n"
                    + "	var e = document.getElementById('" + MyDivName + "');\n"
                    + "	setTimeoutLayerExample.call(null, e);\n"
                    + "};\n"
                    + "function setTimeoutLayerExample(e) {\n"
                    + "	if (e) {\n"
                    + "		e.style.posLeft = 0;\n"
                    + "		e.style.width = document.body.clientWidth;\n"
                    + "		e.style.visibility = 'visible'\n"
                    + "		var x = 0;\n"
                    + "		x = document.body.scrollLeft + x;\n"
                    + "　	var y = document.body.clientHeight - e.clientHeight;\n"
                    + "　　	var diff = (document.body.scrollTop + y - new Number( e.style.top.toString().replace('px', '') ) ) * 0.4;\n"
                    + "　　	var y = document.body.scrollTop + y - diff;\n"
                    + "　　	e.style.width = document.body.clientWidth;\n"
                    + "　　	e.style.top = y;\n"
                    + "　　	e.style.left = x;\n"
                    + "		var myFunc = function() { setTimeoutLayerExample(e) };\n"
                    + "		e.threadHdle = window.setTimeout(myFunc, 100);\n"
                    + "	}\n"
                    + "};\n"
                    + "onload = function loadHdle() {\n"
                    + "	initLayer.call(null);\n"
                    + " DefaultScrollBarByLog();\n"
                    + "	LogScrollBarLeftAndTop.call(null);\n";

                if (this.ScriptByOnLoadEvent != null && this.ScriptByOnLoadEvent.Length > 0) script += "	" + ScriptByOnLoadEvent + "\n";

                script += "};\n"

                    + "function DefaultScrollBarByLog() {\n"
                    + "		if (document.getElementById('HiddenScrollBarLeftAndTop') && document.getElementById('HiddenScrollBarLeftAndTop').value.indexOf(',') > -1) {\n"
                    + "			document.body.scrollLeft = new Number(document.getElementById('HiddenScrollBarLeftAndTop').value.split(',')[0]);\n"
                    + "			document.body.scrollTop = new Number(document.getElementById('HiddenScrollBarLeftAndTop').value.split(',')[1]);\n"
                    + "		}\n"
                    + "		else if (document.body) { document.body.scrollLeft = 0; document.body.scrollTop = 0; }\n"
                    + "};\n"

                    + "function LogScrollBarLeftAndTop()\n"
                    + "{\n"
                    + "		try\n"
                    + "		{\n"
                    + "			if (document.getElementById('HiddenScrollBarLeftAndTop'))\n"
                    + "			{\n"
                    + "				document.getElementById('HiddenScrollBarLeftAndTop').value = document.body.scrollLeft;\n"
                    + "				document.getElementById('HiddenScrollBarLeftAndTop').value += \",\" + document.body.scrollTop;\n"
                    + "				DefaultScrollBarByLog();\n"
                    + "			}\n"
                    + "			setTimeout(LogScrollBarLeftAndTop, 100);\n"
                    + "		}\n"
                    + "		catch (e) {}\n"
                    + "};\n";

                if (this.AddFunctionScript != null && this.AddFunctionScript.Length > 0) script += "\n" + AddFunctionScript;

                return "<script language=\"javascript\">\n" + script + "\n</script>\n";
            }
            catch (System.Exception ex) { this.Response.Write("<!--\n" + ex.ToString() + "\n-->\n"); return string.Empty; }
        }
        #endregion

        #region Confirm()
        /// <summary>
        /// 
        /// </summary>
        public enum ConfirmMode { Start = 0, Stop = 1, Delete = 2, Modify = 3, DeleteAll = 4, Download = 5,BackPieces = 6,Cancel =7 };
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ImgBtn"></param>
        /// <param name="Mode"></param>
        public void Confirm(ImageButton ImgBtn, ConfirmMode Mode)
        {
            string message = string.Empty;

            if (Mode == ConfirmMode.Start) message = "是否確定啟用？";
            else if (Mode == ConfirmMode.Stop) message = "是否確定停用？";
            else if (Mode == ConfirmMode.Delete) message = "是否確定刪除？";
            else if (Mode == ConfirmMode.Modify) message = "是否確定變更？";
            else if (Mode == ConfirmMode.DeleteAll) message = "此動作將會連帶刪除所有相關資料,請問是否確定刪除？";
            else if (Mode == ConfirmMode.Download) message = "檔案數量多寡會等同於等待下載時間，是否確定下載？";
            else if (Mode == ConfirmMode.BackPieces) message = "是否確定退件？";
            else if (Mode == ConfirmMode.Cancel) message = "是否確定取消退件？";

            this.Confirm(ImgBtn, message);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="E"></param>
        /// <param name="ImgBtn"></param>
        /// <param name="Message"></param>
        public void Confirm(ImageButton ImgBtn, string Message)
        {
            ImgBtn.Attributes.Add("onclick", string.Format(@"javascript:return confirm('{0}')", Message));
        }
        #endregion
    }
}