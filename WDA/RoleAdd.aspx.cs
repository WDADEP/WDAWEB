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
    public partial class RoleAdd : PageUtility
    {
        #region Page_Load()
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadPage(true);

            int rePage = this.Request.QueryString["RePage"] != null ? Convert.ToInt16(this.Request.QueryString["RePage"].Trim()) : 0;

            try
            {

                if (rePage == 1)
                {
                    this.ShowMessage("新增成功", MessageMode.INFO);
                    rePage = 0;
                }

                if (!IsPostBack)
                {
                    this.RegisterClientScript();

                    this.InitInfo();
                }
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message);
            }
        }

        #endregion

        protected void BtnOK_Click(object sender, EventArgs e)
        {
            bool check = false;

            string strSql = string.Empty, checkMessage = string.Empty;

            int roleID = 0;

            string strPrivilege = string.Empty, strViewerPrivilege = string.Empty, strViewerAreaPrivilege = string.Empty;

            string[] arr_Privilege = null, arr_ViewerPrivilege = null;

            try
            {
                //取得勾選節點
                this.GetSelectedNodesValue(this.treeViewRolePrivilege.Nodes, ref strPrivilege);
                this.GetSelectedNodesValue(this.treeViewViewerPrivilege.Nodes, ref strViewerPrivilege);

                if (String.IsNullOrWhiteSpace(this.txtRoleName.Text)) { checkMessage += "請輸入角色名稱<br /><br />"; }
                if (strPrivilege.Length == 0) { checkMessage += "請勾選網頁權限<br /><br />"; }
                if (strViewerPrivilege.Length == 0) { checkMessage += "請勾選影像預覽權限<br /><br />"; }
                if (checkMessage.Length > 0)
                {
                    this.ShowMessage(checkMessage, MessageMode.INFO); return;
                }

                string strSqlRoleID = this.Select.GetMaxRoleID();

                roleID = Convert.ToInt32(this.DBConn.GeneralSqlCmd.ExecuteByColumnName(strSqlRoleID, "RoleID"));
            }
            catch (System.Exception ex)
            {
                this.ShowMessage(ex.Message);
            }
            finally
            {
                this.DBConn.Dispose(); this.DBConn = null;
            }

            try
            {
                int result = -1;

                #region 角色權限
                arr_Privilege = strPrivilege.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < arr_Privilege.Length; i++)
                {
                    this.DBSqlParamList.Clear();
                    this.DBSqlParamList.Add(new OleDbParameter("RoleID", roleID));
                    this.DBSqlParamList.Add(new OleDbParameter("PrivID", arr_Privilege[i].Replace("NODE", "")));

                    strSql = this.Insert.RolePrivilege(this.DBSqlParamList);

                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql, this.DBSqlParamList);

                    this.DBConnTransac.GeneralSqlCmd.Command.Parameters.Clear();

                    result = this.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql, this.DBSqlParamList);

                    if (result < 1) throw new Exception("角色權限新增失敗");
                }
                #endregion

                #region 角色
                this.DBSqlParamList.Clear();
                this.DBSqlParamList.Add(new OleDbParameter("RoleID", roleID));
                this.DBSqlParamList.Add(new OleDbParameter("RoleName", this.txtRoleName.Text));
                this.DBSqlParamList.Add(new OleDbParameter("Comments", this.txtComments.Text));

                strSql = this.Insert.RoleTable(this.DBSqlParamList);

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql, this.DBSqlParamList);

                this.DBConnTransac.GeneralSqlCmd.Command.Parameters.Clear();

                result = this.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql, this.DBSqlParamList);

                if (result < 1) throw new Exception("角色新增失敗");
                #endregion

                #region Viewer預覽權限
                arr_ViewerPrivilege = strViewerPrivilege.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < arr_ViewerPrivilege.Length; i++)
                {
                    if ("NODE1".Equals(arr_ViewerPrivilege[i])) continue;  //ToolBar 不算權限
                    this.DBSqlParamList.Clear();
                    this.DBSqlParamList.Add(new OleDbParameter("RoleID", roleID));
                    this.DBSqlParamList.Add(new OleDbParameter("ViewerPrivID", arr_ViewerPrivilege[i].Replace("NODE", "")));

                    strSql = this.Insert.ViewerRolePrivilegeTable(this.DBSqlParamList);

                    this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql, this.DBSqlParamList);

                    this.DBConnTransac.GeneralSqlCmd.Command.Parameters.Clear();

                    result = this.DBConnTransac.GeneralSqlCmd.ExecuteNonQuery(strSql, this.DBSqlParamList);

                    if (result < 1) throw new Exception("Viewer預覽權限新增失敗");
                }
                #endregion

                this.DBConnTransac.GeneralSqlCmd.Transaction.Commit();

                check = true;
            }
            catch (System.Exception ex)
            {
                try
                {
                    this.DBConnTransac.GeneralSqlCmd.Transaction.Rollback();
                }
                catch { }

                this.ShowMessage(ex);
            }
            finally
            {
                this.DBConnTransac.Dispose(); this.DBConnTransac = null;
            }

            if (check)
            {
                string strUrl = string.Format("RoleAdd.aspx?RePage=1");

                Response.Redirect(strUrl, true);
            }
        }

        protected void BtnClear_Click(object sender, EventArgs e)
        {
            this.ReloadPage();
        }

        #region Private Method

        #region RegisterClientScript()
        /// <summary>
        /// 前端註冊
        /// </summary>
        private void RegisterClientScript()
        {
            this.ChkRolePrivilege.Attributes.Add("onclick", "TreeViewCheckAll(MainContent_treeViewRolePrivilege);");

            this.ChkViewerPrivilege.Attributes.Add("onclick", "TreeViewCheckAll(MainContent_treeViewViewerPrivilege);");

            this.treeViewRolePrivilege.Attributes.Add("onclick", "OnTreeNodeChecked();");

            this.treeViewViewerPrivilege.Attributes.Add("onclick", "OnTreeNodeChecked();");
        }
        #endregion

        #region InitInfo()
        /// <summary>
        /// 
        /// </summary>
        private void InitInfo()
        {
            try
            {
                this.LoadRolePrivilegeList();

                this.LoadViewerPrivilegeList();
            }
            catch (System.Exception ex)
            {
                this.ShowMessage(ex);
            }
        }
        #endregion

        #region LoadRolePrivilegeList()
        /// <summary>
        /// 載入角色網頁權限清單
        /// </summary>
        private void LoadRolePrivilegeList()
        {
            string strSql = string.Empty;

            DataTable dt = null;
            try
            {
                dt = this.BindPrivGroup(-1);

                this.BindTreeView(dt, this.treeViewRolePrivilege.Nodes, null, "PrivID", "ParentID", "PrivName");
                //this.BindTreeViewTwoLayer(dt, this.treeViewRolePrivilege);
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex);
            }
            finally
            {
                if (dt != null) dt = null;

                this.DBConn.Dispose(); this.DBConn = null;
            }
        }
        #endregion

        #region LoadViewerPrivilegeList()
        /// <summary>
        /// 載入角色Viewer預覽權限清單
        /// </summary>
        private void LoadViewerPrivilegeList()
        {
            string strSql = string.Empty;

            DataTable dt = null;
            try
            {
                strSql = this.Select.ViewerPrivilegeTable(-1);

                this.WriteLog(global::Log.Mode.LogMode.DEBUG, strSql);

                dt = this.DBConn.GeneralSqlCmd.ExecuteToDataTable(strSql);

                //影像預覽權限要寫死
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    if (dt.Rows[i]["PrivID"].ToString() == "NODE1")
                    {
                        dt.Rows[i]["PrivName"] = "影像預覽權限";
                    }
                    if (dt.Rows[i]["PrivID"].ToString() != "NODE1" &&
                      dt.Rows[i]["PrivID"].ToString() != "NODE2" &&
                      dt.Rows[i]["PrivID"].ToString() != "NODE3" &&
                      dt.Rows[i]["PrivID"].ToString() != "NODE4" &&
                      dt.Rows[i]["PrivID"].ToString() != "NODE5" &&
                      dt.Rows[i]["PrivID"].ToString() != "NODE6" &&
                      dt.Rows[i]["PrivID"].ToString() != "NODE7" &&
                      dt.Rows[i]["PrivID"].ToString() != "NODE8" &&
                      dt.Rows[i]["PrivID"].ToString() != "NODE9" &&
                      dt.Rows[i]["PrivID"].ToString() != "NODE10" &&
                      dt.Rows[i]["PrivID"].ToString() != "NODE11" &&
                      dt.Rows[i]["PrivID"].ToString() != "NODE12" &&
                      dt.Rows[i]["PrivID"].ToString() != "NODE13" &&
                      //dt.Rows[i]["PrivID"].ToString() != "NODE14" &&
                      dt.Rows[i]["PrivID"].ToString() != "NODE15")
                    {
                        dt.Rows.RemoveAt(i);
                    }
                }

                this.BindTreeView(dt, this.treeViewViewerPrivilege.Nodes, null, "PrivID", "ParentID", "PrivName");
                //this.BindTreeViewTwoLayer(dt, this.treeViewViewerPrivilege);
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex);
            }
            finally
            {
                if (dt != null) dt = null;

                this.DBConn.Dispose(); this.DBConn = null;
            }
        }

        #endregion


        #region BindTreeView()
        /// <summary>  
        /// 遞歸綁定子節點  
        /// </summary>  
        /// <param name="dt">來源的DataTable</param>  
        /// <param name="tnc">該節點的子節點集合</param>  
        /// <param name="pid_Val">該節點的父節點值</param>  
        /// <param name="id_Name">DataTable中的ID</param>  
        /// <param name="pid_Name">DataTable中的ParentID</param>  
        /// <param name="text_Name">DataTable中的Name</param>  
        private void BindTreeView(DataTable dt, TreeNodeCollection tnc, string pid_Val, string id_Name, string pid_Name, string text_Name)
        {
            DataView dv = new DataView(dt);

            TreeNode tn;

            string filter = string.IsNullOrEmpty(pid_Val) ?
                string.Format("{0} is null OR {0} = ''", pid_Name) :
                string.Format("{0} = '{1}'", pid_Name, pid_Val);

            dv.RowFilter = filter;

            foreach (DataRowView drv in dv)
            {
                tn = new TreeNode();

                if (string.IsNullOrEmpty(pid_Val)) { tn.ShowCheckBox = false; }
                tn.Value = drv[id_Name].ToString();
                tn.Text = drv[text_Name].ToString();
                tn.SelectAction = TreeNodeSelectAction.None;  //去除超連結
                tnc.Add(tn);

                BindTreeView(dt, tn.ChildNodes, tn.Value, id_Name, pid_Name, text_Name);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="tv"></param>
        private void BindTreeViewTwoLayer(DataTable dt, TreeView tv)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TreeNode tn = new TreeNode();
                tn.Value = dt.Rows[i]["PrivID"].ToString();
                tn.Text = dt.Rows[i]["PrivName"].ToString();
                tn.SelectAction = TreeNodeSelectAction.None;  //去除超連結

                if (dt.Rows[i]["ParentID"] == null || "".Equals(dt.Rows[i]["ParentID"].ToString()))
                {
                    tv.Nodes.Add(tn);
                }
                else if (tv.Nodes.Count > 0)
                {
                    tv.Nodes[0].ChildNodes.Add(tn);
                }
            }
        }
        #endregion

        #region GetSelectedNodesValue()
        /// <summary>
        /// 取所有被選中節點的值
        /// </summary>
        /// <param name="tnc"></param>
        /// <param name="value"></param>
        private void GetSelectedNodesValue(TreeNodeCollection tnc, ref string value)
        {
            foreach (TreeNode tn in tnc)
            {
                if (tn.Checked)
                {
                    value += tn.Value + ";";
                }
                if (tn.ChildNodes.Count > 0)
                {
                    GetSelectedNodesValue(tn.ChildNodes, ref value);
                }
            }
        }
        #endregion

        #endregion Private Method
    }
}