using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDA.Class
{
    public class SqlCommand
    {
        #region Select
        /// <summary>
        /// Select
        /// </summary>
        public class Select
        {
            #region AllPrivilege()
            /// <summary>
            /// 所有權限資訊
            /// </summary>
            /// <returns></returns>
            public string AllPrivilege()
            {
                #region SQL Command

                string strSql = "Select\n"
                    + "	'NODE' || rp.PrivID  As PrivID\n"
                    + "	, Case When rp.ParentID = -1 Then '' Else 'NODE' || rp.ParentID End As ParentID\n"
                    + "	,rp.PrivValue\n"
                    + "	,rp.PrivName\n"
                    + "	,rp.PrivLevel\n"
                    + "	,rp.Seq\n"
                    + "From RolePrivilegeTable rp\n"
                    + "Where rp.Status = 0\n"
                    + "Order By rp.ParentID";

                #endregion

                return strSql;
            }
            #endregion

            #region BarcodeTable
            /// <summary>
            /// 索引資訊
            /// </summary>
            public string BarcodeTable()
            {
                return this.BarcodeTable(null);
            }

            public string BarcodeTable(string Where)
            {
                #region SQL Command

                string strSql = "Select bt.* ,ROW_NUMBER() OVER(ORDER BY CASEID) AS RID, ut.realname\n"
                              + "From BarcodeTable bt Left Join UserTable ut On bt.onfile = ut.username\n"
                                + "Where 1=1 {0}";
                #endregion

                strSql = string.Format(strSql, Where);

                return strSql;
            }

            // Added by Luke
            public string BarcodeTableBarcodeValues(string Where)
            {
                #region SQL Command

                string strSql = "Select BarcodeValue\n"
                              + "From BarcodeTable\n"
                                + "Where 1=1 {0}";
                #endregion

                strSql = string.Format(strSql, Where);

                if (string.IsNullOrEmpty(Where))
                    strSql = string.Format(strSql, "");
                else
                    strSql = string.Format(strSql, Where);


                return strSql;
            }

            #endregion

            #region BkwpfileTable
            /// <summary>
            /// 歸檔資訊
            /// </summary>
            /// <param name="WpinNo">收文文號</param>
            /// <param name="WpoutNo">發文文號</param>
            /// <param name="FileNo">歸檔檔號</param>
            /// <param name="FileDate">歸檔日期</param>
            /// <param name="KeepYr">保存年限</param>
            /// <param name="BoxNo">卷宗號</param>
            /// <param name="OnFile">歸檔作業者</param>
            /// <returns></returns>
            public string BkwpfileTable(string WpinNo, string WpoutNo, string FileNo, string FileDate, string KeepYr, string BoxNo, string OnFile)
            {
                #region SQL Command

                string strSql = "Select t.WPINNO,\n"
                    + "t.WPOUTNO,\n"
                    + "t.FILENO,\n"
                    + "t.FILEDATE,\n"
                    + "t.KEEPYR,\n"
                    + "t.BOXNO,\n"
                    + "t.ONFILE\n"
                    + "From BKWPFILE t\n"
                    + "Where 1 = 1\n"
                    + "And t.FILENO != ' '\n"
                    + "{0}"
                    + "Order By t.WPINNO\n";

                string where = string.Empty;

                if (WpinNo != null && WpinNo.Length > 0) where += string.Format("And t.WPINNO = '{0}'\n", WpinNo);

                if (WpoutNo != null && WpoutNo.Length > 0) where += string.Format("And t.WPOUTNO = '{0}'\n", WpoutNo);

                if (FileNo != null && FileNo.Length > 0) where += string.Format("And t.FILENO = '{0}'\n", FileNo);

                if (FileDate != null && FileDate.Length > 0) where += string.Format("And t.FILEDATE = '{0}'\n", FileDate);

                if (KeepYr != null && KeepYr.Length > 0) where += string.Format("And t.KEEPYR = '{0}'\n", KeepYr);

                if (BoxNo != null && BoxNo.Length > 0) where += string.Format("And t.BOXNO = '{0}'\n", BoxNo);

                if (OnFile != null && OnFile.Length > 0) where += string.Format("And t.ONFILE = N'{0}'\n", OnFile);

                #endregion

                strSql = string.Format(strSql, where);

                return strSql;
            }
            #endregion

            #region UploadCaseQuery
            /// <summary>
            /// 索引資訊
            /// </summary>
            public string UploadCaseQuery(string Where)
            {
                #region SQL Command

                string strSql = "Select ct.* ,bt.Barcodevalue,ut.UserName From CASETABLE ct\n"
                                + "Inner Join BarcodeTable bt On ct.caseID = bt.caseid\n"
                                + "Inner Join UserTable ut On ct.createuserid = ut.userid\n"
                                + "Where 1=1 {0}";

                #endregion

                strSql = string.Format(strSql, Where);

                return strSql;
            }
            #endregion

            #region CaseQuery
            /// <summary>
            /// 
            /// </summary>
            public string CaseQuery(string Where)
            {
                #region SQL Command

                string strSql = "SELECT fb.*,bt.CaseID,wb.ViewType,\n"
                                  + "wpr.APPLKIND as kindName\n"
                                //+ "CASE wpr.APPLKIND as kindName\n"
                                //+ "WHEN N'1' THEN '一般'\n"
                                //+ "WHEN N'01' THEN '一般'\n"
                                //+ "WHEN N'2' THEN '法制'\n"
                                //+ "WHEN N'02' THEN '法制'\n"
                                //+ "WHEN N'3' THEN '行政'\n"
                                //+ "WHEN N'03' THEN '行政'\n"
                                //+ "ELSE '其他' END As kindName\n"
                                + "From FILEBORO fb\n"
                                + "INNER JOIN WPBORROW wb ON fb.WPINNO = wb.WPINNO And fb.Receiver = wb.Receiver And fb.TRANST = wb.TRANST  \n"
                                + "Inner Join {1} wpr On wb.wpinno = wpr.wpinno\n"
                                + "Left JOIN BarcodeTable bt ON fb.WPINNO = bt.Barcodevalue\n"
                                + "WHERE wb.REDATE Is Null  {0}\n  And ((fb.chk='Y' And wb.viewtype =2) or(fb.chk='N' And wb.viewtype =1)) order by fb.wpinno";

                #endregion

                strSql = string.Format(strSql, Where, PageUtility.WprecSchema);

                return strSql;
            }
            #endregion

            #region FileArchiveCheck
            /// <summary>
            /// 
            /// </summary>
            public string FileArchiveCheck(string WprecWhere, string WptransWhere)
            {
                #region SQL Command

                //REMARK BY RICHARD 20160408
                //string strSql = "Select wp.WPINNO,wp.WpoutNo,wp.FileNo,wp.FileDate,wp.KeepYr,wp.BoxNo,bk.onfile,'1' as viewtype\n"
                string strSql = "Select wp.WPINNO\n"
                                + " From {1} wp\n"
                                + "Left Join {2} bk\n"
                                + "On wp.wpinno = bk.wpinno\n"
                                + "WHERE 1=1   {0}\n"
                                + " Union\n"
                //              + "Select wp.WPINNO,wp.WpoutNo,wp.FileNo,wp.FileDate,wp.KeepYr,wp.BoxNo,wps.RECEIVER As OnFile,'1' as viewtype\n"
                                + "Select wp.WPINNO\n"
                                + " From {1} wp\n"
                                + "Inner Join WPTRANS wps\n"
                                + "On wp.wpinno = wps.wpinno\n"
                                + "WHERE 1=1 {3}\n";

                #endregion

                strSql = string.Format(strSql, WprecWhere, PageUtility.WprecSchema, PageUtility.bkwpfileSchema, WptransWhere);

                return strSql;
            }
            #endregion

            #region FileArchive2
            /// <summary>
            /// 
            /// </summary>
            public string FileArchive2(string WptransWhere)
            {
                #region SQL Command

                //string strSql = "Select cast( bt.BarcodeValue as nvarchar2(10)) as WPINNO,bt.WpoutNo,bt.FileNo,cast(to_char(bt.FileDate,'YYYYMMDD')as nvarchar2(8) ) as FileDate,bt.KeepYr,bt.BoxNo,bt.onfile,'2' as viewtype\n"
                //                + "From BarcodeTable bt \n"
                //                + "Where 1=1 {0}"
                //                + " Union\n"
                //                + "Select wp.WPINNO,wp.WpoutNo,wp.FileNo,wp.FileDate,wp.KeepYr,wp.BoxNo,wps.RECEIVER As OnFile,'1' as viewtype\n"
                //                + " From {2} wp\n"
                //                + "Inner Join WPTRANS wps\n"
                //                + "On wp.wpinno = wps.wpinno\n"
                //                + "WHERE  1=1 {4}\n";

                string strSql = "Select ROW_NUMBER() OVER(ORDER BY wp.BoxNo) AS RID,wp.WPINNO,wp.WpoutNo,wp.FileNo,wp.FileDate,wp.KeepYr,wp.BoxNo,wps.RECEIVER As OnFile,'1' as viewtype,bt.BARCODEVALUE\n"
                   + " From {0} wp\n"
                   + "Inner Join WPTRANS wps\n"
                   + "On wp.wpinno = wps.wpinno\n"
                   + "Left Join BarcodeTable bt\n"
                   + "On wp.wpinno = bt.BARCODEVALUE\n"
                   + "WHERE 1=1 {1} Order By wp.BoxNo\n";

                #endregion

                strSql = string.Format(strSql, PageUtility.WprecSchema, WptransWhere);
                //strSql = string.Format(strSql, BarWhere, WprecWhere, PageUtility.WprecSchema, PageUtility.bkwpfileSchema,WptransWhere);

                return strSql;
            }
            #endregion

            #region FileArchive
            /// <summary>
            /// 
            /// </summary>
            public string FileArchive(string BarWhere, string WprecWhere, string WptransWhere)
            {
                #region SQL Command

                //string strSql = "Select cast( bt.BarcodeValue as nvarchar2(10)) as WPINNO,bt.WpoutNo,bt.FileNo,cast(to_char(bt.FileDate,'YYYYMMDD')as nvarchar2(8) ) as FileDate,bt.KeepYr,bt.BoxNo,bt.onfile,'2' as viewtype\n"
                //                + "From BarcodeTable bt \n"
                //                + "Where 1=1 {0}"
                //                + " Union\n"
                //                + "Select wp.WPINNO,wp.WpoutNo,wp.FileNo,wp.FileDate,wp.KeepYr,wp.BoxNo,bk.onfile,'1' as viewtype\n"
                //                + " From {2} wp\n"
                //                + "Left Join {3} bk\n"
                //                + "On wp.wpinno = bk.wpinno\n"
                //                + "WHERE  1=1 {1}\n"
                //                + " Union\n"
                //                + "Select wp.WPINNO,wp.WpoutNo,wp.FileNo,wp.FileDate,wp.KeepYr,wp.BoxNo,wps.RECEIVER As OnFile,'1' as viewtype\n"
                //                + " From {2} wp\n"
                //                + "Inner Join WPTRANS wps\n"
                //                + "On wp.wpinno = wps.wpinno\n"
                //                + "WHERE  1=1 {1}\n";


                //string strSql = "Select wp.WPINNO,wp.WpoutNo,wp.FileNo,wp.FileDate,wp.KeepYr,wp.BoxNo,bk.onfile,'1' as viewtype\n"
                //              + " From {2} wp\n"
                //              + "Left Join {3} bk\n"
                //              + "On wp.wpinno = bk.wpinno\n"
                //              + "WHERE  1=1 {1}\n"
                //              + " Union\n"
                //              + "Select wp.WPINNO,wp.WpoutNo,wp.FileNo,wp.FileDate,wp.KeepYr,wp.BoxNo,wps.RECEIVER As OnFile,'1' as viewtype\n"
                //              + " From {2} wp\n"
                //              + "Inner Join WPTRANS wps\n"
                //              + "On wp.wpinno = wps.wpinno\n"
                //              + "WHERE  1=1 {4}\n";

                string strSql = "Select  ROW_NUMBER() OVER(ORDER BY wp.BoxNo) AS RID,wp.WPINNO,wp.WpoutNo,wp.FileNo,wp.FileDate,wp.KeepYr,wp.BoxNo,NVL(wps.RECEIVER,bk.onfile) As OnFile,'1' as viewtype,bt.BARCODEVALUE\n"
                                + "From {2} wp\n"
                                + "left Join {3} bk\n"
                                + "On wp.wpinno = bk.wpinno\n"
                                + "Left Join WPTRANS wps\n"
                                + "On wp.wpinno = wps.wpinno\n"
                                + "Left Join BarcodeTable bt\n"
                                + "On wp.wpinno = bt.BARCODEVALUE\n"
                                + "WHERE  wp.FileNo is not null {4} Order By wp.BoxNo\n";

                #endregion

                strSql = string.Format(strSql, BarWhere, WprecWhere, PageUtility.WprecSchema, PageUtility.bkwpfileSchema, WptransWhere);
          
                return strSql;
            }
            #endregion

            #region FileArchiveStatisticsReport
            /// <summary>
            /// 
            /// </summary>
            public string FileArchiveStatisticsReport(string WptransWhere)
            {
                #region SQL Command

                string strSql = "Select  ROW_NUMBER() OVER(ORDER BY wt.RECEIVER) AS RID, wt.RECEIVER, count(wt.RECEIVER) AS FILECOUNT,substr(wt.TRANST,1,10) AS TRANST, wp.FILENO, NVL(wp.FILENO,0) as ISFILE  \n"
                                + "From WPTRANS wt \n"
                                + "inner Join {0} wp On wt.wpinno = wp.wpinno \n"
                                + "Left Join BarcodeTable bt On wt.wpinno = bt.BARCODEVALUE\n"
                                + "WHERE 1=1 {1} GROUP BY wt.RECEIVER,substr(wt.TRANST,1,10),wp.FILENO \n Order By wt.RECEIVER \n";

                #endregion

                strSql = string.Format(strSql, PageUtility.WprecSchema, WptransWhere);

                return strSql;
            }

            // Added by Luke 2016/09/12
            /// <summary>
            /// ScanListStatistsDetail
            /// </summary>
            /// <param name="Where"></param>
            /// <returns></returns>
            public string FileArchiveStatisticsDetail(string Where)
            {
                #region SQL Command

                string strSql = "SELECT wp.WPINNO AS BARCODEVALUE,wp.COMMNAME,wp.COMMADD,wp.WPOUTDATE,um.USERNAME as SENDMAN,wp.FILENO,wp.BOXNO,wt.RECEIVER,ut.REALNAME,tt.RECEIVER AS RECEIVER2,wp.KEEPYR,tt.TRANSTIME,wp.FILEDATE,bt.CREATETIME \n"
                                + " From WPTRANS wt \n"
                                + " INNER JOIN {0} wp ON wt.WPINNO=wp.WPINNO \n"
                                + " LEFT OUTER JOIN TRANSTABLE tt ON wt.WPINNO=tt.WPINNO \n"
                                + " LEFT OUTER JOIN BARCODETABLE bt ON wt.WPINNO=bt.BARCODEVALUE \n"
                                + " LEFT OUTER JOIN usertable ut ON ut.USERID=bt.CREATEUSERID \n"
                                + " LEFT OUTER JOIN {1} um ON wp.SENDMAN=um.userid \n"
                                + " WHERE  1=1 {2}\n ";

                #endregion

                if (!string.IsNullOrEmpty(Where))
                    strSql = string.Format(strSql, PageUtility.WprecSchema,PageUtility.UsermSchema, Where);
                else
                    strSql = string.Format(strSql, PageUtility.WprecSchema,PageUtility.UsermSchema, "");

                return strSql;
            }

            #endregion

            #region Wpborrow
            /// <summary>
            /// 
            /// </summary>
            public string Wpborrow(string Where)
            {
                #region SQL Command

                string strSql = "Select wb.WPINNO,wb.WPOUTNO,fb.commname,fb.transt,NVL(wb.hurrytime,0) as hurrytime,ut.RealName as receiver,wb.EXTENSIONDATE,wb.EXTENSIONCOUNT,dp.DEPTNAME,ut.UserName\n"
                                + " From Wpborrow wb\n"
                                + "Inner Join (Select wb.WPINNO,\n"
                                + "CASE wb.kind\n"
                                + "WHEN N'1' THEN fb.GETIME + 9\n"
                                + "WHEN N'2' THEN fb.GETIME + 18\n"
                                + "WHEN N'3' THEN fb.GETIME + 485\n"
                                + "ELSE wb.transt END As transtExtra\n"
                                + "From Wpborrow wb inner join fileboro fb on wb.wpinno=fb.wpinno WHERE  wb.REDATE IS null  And wb.EXTEN In('D','N','Z') And wb.ViewType = 1 And wb.PRTFLAG In('P','T') And NVL(wb.EXTENSIONCOUNT,0)=0)B\n"
                                + "On wb.wpinno = B.wpinno\n"
                                + "Inner Join UserTable ut\n"
                                + "On wb.receiver = ut.username\n"
                                + "Inner Join Fileboro fb\n"
                                + "On wb.wpinno = fb.wpinno\n"
                                + "Inner Join DEPT dp\n"
                                + "On ut.deptid = dp.deptid\n"
                                + "WHERE  wb.REDATE IS null  And fb.chk ='N' And wb.EXTEN In('D','N','Z') And wb.ViewType = 1  {0}\n"
                                + " Union\n"
                                + "Select wb.WPINNO,wb.WPOUTNO,fb.commname,fb.transt,NVL(wb.hurrytime,0) as hurrytime,ut.RealName as receiver,wb.EXTENSIONDATE,wb.EXTENSIONCOUNT,dp.DEPTNAME,ut.UserName\n"
                                + " From Wpborrow wb\n"
                                + "Inner Join (Select wb.WPINNO,\n"
                                + "CASE wb.kind\n"
                                + "WHEN N'1' THEN wb.EXTENSIONDATE + 9\n"
                                + "WHEN N'2' THEN wb.EXTENSIONDATE + 9\n"
                                + "WHEN N'3' THEN wb.EXTENSIONDATE + 126\n"
                                + "ELSE wb.EXTENSIONDATE END As transtExtra\n"
                                + "From Wpborrow wb WHERE  wb.REDATE IS null  And wb.EXTEN In('D','N','Z') And wb.ViewType = 1 And wb.PRTFLAG In('P','T') And NVL(wb.EXTENSIONCOUNT,0)!=0)B\n"
                                + "On wb.wpinno = B.wpinno\n"
                                + "Inner Join UserTable ut\n"
                                + "On wb.receiver = ut.username\n"
                                + "Inner Join Fileboro fb\n"
                                + "On wb.wpinno = fb.wpinno\n"
                                + "Inner Join DEPT dp\n"
                                + "On ut.deptid = dp.deptid\n"
                                + "WHERE  wb.REDATE IS null  And fb.chk ='N' And wb.EXTEN In('D','N','Z') And wb.ViewType = 1  {0}\n"
                                + "Order By receiver,transt";

                #endregion

                //strSql = string.Format(strSql, Where,PageUtility.WprecSchema);
                strSql = string.Format(strSql, Where);

                return strSql;
            }
            #endregion

            #region WpborrowByVisaExtenson
            /// <summary>
            /// 
            /// </summary>
            public string WpborrowByVisaExtenson(string Where)
            {
                #region SQL Command

                string strSql = "Select wb.VIEWTYPE,wb.EXTENSIONCOUNT\n"
                                + " From Wpborrow wb\n"
                                + "WHERE  wb.REDATE IS null  And wb.EXTEN  = 'Y' {0}";
                #endregion

                strSql = string.Format(strSql, Where);

                return strSql;
            }
            #endregion

            #region WprecInfos
            /// <summary>
            /// 
            /// </summary>
            public string WprecInfos(string Where)
            {
                #region SQL Command

                string strSql = "Select * From {1} Where 1=1 {0}";

                #endregion

                strSql = string.Format(strSql, Where, PageUtility.WprecSchema);

                return strSql;
            }
            #endregion

            #region WprecBkwfile
            /// <summary>
            /// 
            /// </summary>
            public string WprecBkwfile(string Where)
            {
                #region SQL Command

                string strSql = "Select * From {1} wp Inner Join {2} bk On wp.wpinno = bk.wpinno Where 1=1 {0}";

                #endregion

                strSql = string.Format(strSql, Where, PageUtility.WprecSchema,PageUtility.bkwpfileSchema);

                return strSql;
            }
            #endregion

            #region WprecWptrans
            /// <summary>
            /// 
            /// </summary>
            public string WprecWptrans(string Where)
            {
                #region SQL Command

                string strSql = "Select * From {1} wp Inner Join WPTRANS  wps On wp.wpinno = wps.wpinno Where 1=1 {0}";

                #endregion

                strSql = string.Format(strSql, Where, PageUtility.WprecSchema);

                return strSql;
            }
            #endregion

            #region AlsoFileInfos
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public string AlsoFileInfos(string Where)
            {
                #region SQL Command

                string strSql = "Select wpb.wpinno,wpb.wpoutno,wpb.transt,wpb.receiver,kind,wpb.EXTENSIONDATE,wpb.EXTENSIONCOUNT,wpb.Viewtype,\n"
                                + "CASE kind\n"
                                + "WHEN N'1' THEN '一般'\n"
                                + "WHEN N'2' THEN '法制'\n"
                                + "WHEN N'3' THEN '行政'\n"
                                + "ELSE '其他' END As kindName,\n"
                                + "wpb.redate,wpb.exten,ut.RealName,fb.getime\n"
                                + "From wpborrow wpb\n"
                                + "Inner Join UserTable ut On wpb.receiver = ut.UserName\n"
                                //ADD BY RICHARD 20160418 for展期日調檔新增開始起算
                                + "Inner Join FILEBORO fb On (fb.wpinno = wpb.wpinno and fb.transt=wpb.transt)\n"
                                //MODIFY BY RICHARD 20160615 delete 1=1
                                + "Where wpb.REDATE IS null  And wpb.EXTEN  In('N','D') {0} And wpb.PRTFLAG In('P','T','F')";
                #endregion

                strSql = string.Format(strSql, Where);

                return strSql;
            }
            #endregion

            #region AlsoFileInfosExten
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public string AlsoFileInfosExten(string Where)
            {
                #region SQL Command

                string strSql = "Select wpb.*\n"
                                + "From wpborrow wpb\n"
                                // MODIFY BY RICHARD 20160615 delete 1=1
                                + "Where wpb.REDATE IS null And wpb.EXTEN = 'Y' {0}";

                #endregion

                strSql = string.Format(strSql, Where);

                return strSql;
            }
            #endregion

            #region PaperAlsoFileInfos
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public string PaperAlsoFileInfos(string Where)
            {
                #region SQL Command

                string strSql = "Select wpb.wpinno,wpb.wpoutno,wpb.transt,ut.RealName as receiver,ut.TEL,wpb.kind,wpb.redate,ut2.RealName\n"
                                + "From wpborrow wpb\n"
                                + "Inner Join UserTable ut On wpb.receiver = ut.UserName\n"
                                + "Left Join UserTable ut2 On wpb.USERID = ut2.USERID\n"
                                + "Inner Join FILEBORO fb On wpb.wpinno = fb.wpinno AND wpb.TRANST = fb.TRANST AND wpb.RECEIVER = fb.RECEIVER\n"
                                //MODIFY BY 20160615 delete 1=1
                                + "Where ViewType =1 {0} Order By wpb.transt DESC";
                #endregion

                strSql = string.Format(strSql, Where);

                return strSql;
            }
            #endregion

            #region GetAlsoFileByVisa
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public string GetAlsoFileByVisa(string Where)
            {
                #region SQL Command

                string strSql = "Select wpb.wpinno,wpb.wpoutno,wpb.transt,wpb.receiver,kind,wpb.EXTENSIONDATE,NVL(wpb.EXTENSIONCOUNT,0) as EXTENSIONCOUNT, wpb.viewtype,\n"
                                + "CASE kind\n"
                                + "WHEN N'1' THEN '一般'\n"
                                + "WHEN N'2' THEN '法制'\n"
                                + "WHEN N'3' THEN '行政'\n"
                                + "ELSE '其他' END As kindName,\n"
                                + "wpb.redate,wpb.exten,ut.RealName,fb.getime\n"
                                + "From wpborrow wpb\n"
                                + "Inner Join UserTable ut On wpb.receiver = ut.UserName\n"
                                //ADD BY RICHARD 20160418 展期日為調檔新增開始起算
                                + "Inner Join FILEBORO fb On (fb.wpinno = wpb.wpinno and fb.transt=wpb.transt)\n"
                                //MODIFY BY RICHARD 20160615 delete 1=1
                                + "Where REDATE IS null And EXTEN ='Y' {0}";
                #endregion

                strSql = string.Format(strSql, Where);

                return strSql;
            }
            #endregion

            #region DulyAdjustedInfo
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public string DulyAdjustedInfo(string Where)
            {
                #region SQL Command

                string strSql = "Select wpr.wpindate,wpr.wpoutno,wpr.wpoutdate,wpr.caseno,wpr.applkind,wpr.commname,wpb.receiver,\n"
                                + "(Select ut.realname From usertable ut Where wpb.receiver = ut.username ) As rname,\n"
                                + "wpb.transt ,SYSDATE as getime,\n"
                                + "wpr.applkind as kindName\n"
                                //+ "CASE wpr.applkind\n"
                                //+ "WHEN N'1' THEN '一般'\n"
                                //+ "WHEN N'01' THEN '一般'\n"
                                //+ "WHEN N'2' THEN '法制'\n"
                                //+ "WHEN N'02' THEN '法制'\n"
                                //+ "WHEN N'3' THEN '行政'\n"
                                //+ "WHEN N'03' THEN '行政'\n"
                                //+ "ELSE '其他' END As kindName\n"
                                + "From Wpborrow wpb\n"
                                + "Inner Join {0} wpr On wpb.wpinno = wpr.wpinno\n"
                                + "Where 1=1 {1} And  wpb.Redate Is Null And PRTFLAG In('P','T') And not exists (Select wpinno from Fileboro Where CHK ='N' And wpb.wpinno = fileboro.wpinno )";


                #endregion

                strSql = string.Format(strSql, PageUtility.WprecSchema, Where);

                return strSql;
            }
            #endregion

            #region GetApproveuserID
            /// <summary>
            /// 
            /// </summary>
            public string GetApproveuserID()
            {
                #region SQL Command

                string strSql = @"Select ut.* From UserTable ut
                                  Inner Join ROLEPRIVILEGE rp On ut.RoleID = rp.RoleID
                                  Where rp.PrivID =26";

                #endregion

                return strSql;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="PrivID"></param>
            /// <param name="DEPTID"></param> 
            /// <returns></returns>
            public string GetApproveuserID(string PrivID, int DEPTID)
            {
                #region SQL Command
                string strSql = string.Format(
                    @"Select ut.* From UserTable ut
                      Inner Join ROLEPRIVILEGE rp On ut.RoleID = rp.RoleID
                      Where rp.PrivID = '{0}' AND ut.DEPTID={1} ",
                      PrivID, DEPTID);

                #endregion

                return strSql;
            }
            #endregion

            #region GetAllWpinno
            /// <summary>
            /// 取得文件LapID
            /// </summary>
            public string GetAllWpinno(string Where)
            {
                #region SQL Command

                string strSql = "Select wp.wpinno,wp.IMAGEPRIV From WPBORROW wp\n" 
                                + "Inner Join BARCODETABLE bt On wp.wpinno = bt.barcodevalue\n"
                                + "Inner Join Casetable ct on bt.caseid = ct.caseid\n"
                                //MODIFY BY RICHARD 20160615 delete 1=1
                                + "Where wp.REDATE IS null {0}";

                #endregion

                if (!string.IsNullOrEmpty(Where)) strSql = string.Format(strSql, Where);

                return strSql;
            }

            #endregion

            #region GetCaseID
            /// <summary>
            /// 取得UserID
            /// </summary>
            public string GetCaseID()
            {
                #region SQL Command

                string strSql = "Select bt.CaseID From WPBORROW wb Inner Join BARCODETABLE bt On wb.wpinno =bt.BARCODEVALUE\n"
                                 + "Where wb.wpinno =:Wpinno And wb.receiver =:UserName And wb.REDATE Is Null ";

                #endregion

                return strSql;
            }

            #endregion

            #region GetDocLapID
            /// <summary>
            /// 取得文件LapID
            /// </summary>
            public string GetDocLapID()
            {
                #region SQL Command

                string strSql = "Select * From LapTable Where BcValue =@BcValue Order By Seq";

                #endregion

                return strSql;
            }

            #endregion

            #region GetUserID
            /// <summary>
            /// 取得UserID
            /// </summary>
            public string GetUserID()
            {
                #region SQL Command

                string strSql = "Select * From UserTable Where UserName =:UserName And UserStatus=0";

                #endregion

                return strSql;
            }

            #endregion

            #region DEPT
            /// <summary>
            /// 取得部門名稱
            /// </summary>
            public string DEPT(string strDeptName)
            {
                #region SQL Command
                string strSql= string.Empty;
                if (string.IsNullOrEmpty(strDeptName))
                    strSql  = "Select DEPTID,DEPTNAME,Status From Dept order by DEPTID \n";
                else
                    strSql = string.Format("Select DEPTID,DEPTNAME,Status From Dept where DEPTNAME like '%{0}' order by DEPTID \n",strDeptName.Trim());

                #endregion

                return strSql;
            }

            #endregion

            #region GetBARCODETABLE
            /// <summary>
            /// 取得文件LapID
            /// </summary>
            public string GetBARCODETABLE(string Where)
            {
                #region SQL Command

                string strSql = "Select * From BARCODETABLE\n"
                              + "Where 1=1 {0}";

                #endregion

                if (!string.IsNullOrEmpty(Where)) strSql = string.Format(strSql, Where);

                return strSql;
            }

            #endregion

            #region GetCaseTable
            /// <summary>
            /// 取得文件LapID
            /// </summary>
            public string GetCaseTable(string Where)
            {
                #region SQL Command

                string strSql = "Select * From CaseTable\n"
                              + "Where 1=1 {0}";

                #endregion

                if (!string.IsNullOrEmpty(Where)) strSql = string.Format(strSql, Where);

                return strSql;
            }

            #endregion

            #region GetFileTable
            /// <summary>
            /// 
            /// </summary>
            public string GetFileTable(string Where)
            {
                #region SQL Command
                string strSql = @"Select * From FileTable ft Where 1=1 And FileStatus = 1 {0} Order By ft.FileName";
                //                string strSql = @"Select ft.* ,fdt.DrawXml,REPLICATE('0',3-LEN(ROW_NUMBER() OVER (PARTITION BY ft.CaseID  ORDER BY ft.CreateTime)))+CAST(ROW_NUMBER() OVER (PARTITION BY ft.CaseID  ORDER BY ft.CreateTime) AS NVARCHAR(3)) FileNumberName
                //                                    From FileTable ft 
                //                                        Left Join (
                //                                                    Select * From FileDrawTable A Where A.FileStatus = 0 
                //                                                  ) fdt
                //                                  On ft.FileID = fdt.FileID Where 1=1 {0} Order By ft.FileName";
                #endregion

                if (!string.IsNullOrEmpty(Where)) strSql = string.Format(strSql, Where);

                return strSql;
            }

            #endregion

            #region GetAllFileTable
            /// <summary>
            /// 
            /// </summary>
            public string GetAllFileTable(string Where)
            {
                #region SQL Command
                string strSql = @"Select * From FileTable ft Where 1=1  {0} Order By ft.FileName";
                #endregion

                if (!string.IsNullOrEmpty(Where)) strSql = string.Format(strSql, Where);

                return strSql;
            }

            #endregion

            #region GetDocAboveNodes
            /// <summary>
            /// 取得指定LapID的所有上層節點
            /// </summary>
            public string GetDocAboveNodes(string LapID)
            {
                #region SQL Command

                string strSql = "WITH tmpTree(LapID, ParentID, LapName ,LevelID,BcValue) AS (\n"
                    + "SELECT LapID, ParentID, LapName, LevelID,BcValue FROM LapTable WHERE LapID={0} And LapStatus=0\n"
                    + "UNION All\n"
                    + "SELECT a.LapID, a.ParentID ,a.LapName,b.LevelID-1,a.BcValue\n"
                    + "FROM LapTable a INNER JOIN tmpTree b on a.LapID=b.ParentID  WHERE  LapStatus=0\n"
                    + ")\n"
                    + "SELECT * FROM tmpTree\n";

                #endregion

                if (!string.IsNullOrEmpty(LapID)) strSql = string.Format(strSql, LapID);

                return strSql;
            }

            #endregion

            #region GetMessageTable
            /// <summary>
            /// 取得交易紀錄交易內容
            /// </summary>
            /// <param name="MsgSource"></param>
            /// <returns></returns>
            public string GetMessageTable(int MsgSource)
            {
                #region SQL Command

                string strSql = "Select MsgID,MsgText From MessageTable Where MsgStatus = 0 And MsgSource = {0}";

                #endregion

                strSql = string.Format(strSql, MsgSource);

                return strSql;
            }

            #endregion

            #region GetMaxUserID
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public string GetMaxUserID()
            {
                #region SQL Command

                string strSql = "Select (Max(UserID) + 1) As UserID From UserTable\n";

                #endregion

                return strSql;
            }
            #endregion

            #region GetMaxRoleID
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public string GetMaxRoleID()
            {
                #region SQL Command

                string strSql = "Select (Max(RoleID) + 1) As RoleID From RoleTable\n";

                #endregion

                return strSql;
            }
            #endregion

            #region GetUserAllViewerPrivilegeByViewer
            /// <summary>
            /// 
            /// </summary>
            public string GetUserAllViewerPrivilegeByViewer()
            {
                #region SQL Command
                string strSql = "Select DISTINCT ViewerPrivilege As Privilege From UserTable\n"
                               + "Inner Join UserViewerTable On UserTable.UserID = UserViewerTable.UserID where UserTable.UserID=@UserID and JobItemID=@JobItemID\n"
                               + "Union Select DISTINCT ViewerPrivilege As Privilege FROM UserRoleTable\n"
                               + "Inner Join RoleViewerTable On UserRoleTable.RoleID = RoleViewerTable.RoleID where UserRoleTable.UserID=@UserID and JobItemID=@JobItemID";
                #endregion

                return strSql;
            }
            #endregion

            #region GetViewPrivilegeName
            /// <summary>
            /// 取得Viewer權限
            /// </summary>
            public string GetViewPrivilegeName()
            {
                #region SQL Command
                string strSql = @"Select vpt.XmlName From ViewerPrivilegeTable vpt";
                #endregion

                return strSql;
            }
            #endregion

            #region GetRoleViewPrivilege
            /// <summary>
            /// 取得Viewer權限
            /// </summary>
            public string GetRoleViewPrivilege(int RoleID)
            {
                #region SQL Command

                string strSql = @"Select Sum(ViewerPriv)+1 as viewerPrivCount From ViewerRolePrivilegeTable vpt
                                  Inner Join ViewerPrivilegeTable vt On vpt.ViewerPrivID = vt.PrivID
                                  Where RoleID ={0}";

                strSql = string.Format(strSql, RoleID);

                #endregion

                return strSql;
            }
            #endregion

            #region GetDocTreeView
            public string GetDocTreeView()
            {
                return this.GetDocTreeView(null);
            }
            /// <summary>
            /// 取得文件節點
            /// </summary>
            public string GetDocTreeView(string Where)
            {
                #region SQL Command

                string strSql = "Select * From LapTable Where 1=1 And LapStatus = 0 {0} Order By Seq";

                #endregion

                if (Where != null && Where.Length > 0) strSql = string.Format(strSql, Where);
                else strSql = string.Format(strSql, "");

                return strSql;
            }

            #endregion

            #region ScanListQuery
            /// <summary>
            /// 索引資訊
            /// </summary>
            public string ScanListQuery()
            {
                return this.ScanListQuery(null);
            }
            /// <summary>
            /// 索引資訊
            /// </summary>
            public string ScanListQuery(string Where)
            {
                #region SQL Command
                //MODIFY BY RICHARD 20160408
                string strSql = " Select bt.*,ft.FileCount,ut.RealName From BARCODETABLE bt\n"
                    + "Inner Join Casetable ct On bt.caseid = ct.caseid\n"
                    + "Inner Join Usertable ut On bt.CreateUserID = ut.UserID\n"
                    + "Inner Join (Select COUNT(*) AS FileCount,a.CaseID From FileTable a GROUP BY CaseID) ft ON ct.CaseID =ft.CaseID\n"
                    + " Where 1=1\n"
                    + " {0} Order BY bt.CREATETIME\n";

                #endregion

                if (Where != null && Where.Length > 0) strSql = string.Format(strSql, Where);
                else strSql = string.Format(strSql, "");

                return strSql;
            }
            #endregion

            #region ScanListStatisticsReport
            /// <summary>
            /// 統計資訊
            /// </summary>
            public string ScanListStatists(string Where)
            {
                #region SQL Command
                //MODIFY BY RICHARD 20160908
                string strSql = " Select count(bt.BARCODEVALUE) AS SCANCOUNT,substr(bt.CREATETIME,1,10) AS SCANTIME,UT.REALNAME,ROW_NUMBER() OVER (ORDER BY substr(bt.CREATETIME,1,10) ASC) AS RID, bt.FILENO \n"
                    + " FROM BARCODETABLE bt \n"
                    + "Inner Join Casetable ct On bt.caseid = ct.caseid\n"
                    + "Inner Join Usertable ut On bt.CreateUserID = ut.UserID\n"
                    + " Where 1=1\n"
                    + " {0} GROUP BY substr(bt.CREATETIME,1,10),UT.REALNAME,bt.FILENO \n  ORDER BY substr(bt.CREATETIME,1,10) \n";
                #endregion

                if (Where != null && Where.Length > 0) strSql = string.Format(strSql, Where);
                else strSql = string.Format(strSql, "");

                return strSql;
            }

            // Added by Luke 2016/09/12
            /// <summary>
            /// ScanListStatistsDetail
            /// </summary>
            /// <param name="Where"></param>
            /// <returns></returns>
            public string ScanListStatistsDetail(string Where)
            {
                #region SQL Command

                string strSql = "SELECT bt.BARCODEVALUE,wp.COMMNAME,wp.COMMADD,wp.WPOUTDATE,um.USERNAME as SENDMAN,wp.FILENO,wp.BOXNO,wt.RECEIVER,ut.REALNAME,tt.RECEIVER AS RECEIVER2,wp.KEEPYR,tt.TRANSTIME,wp.FILEDATE,bt.CREATETIME \n"
                                + " From BARCODETABLE bt \n"
                                + " INNER JOIN usertable ut ON ut.USERID=bt.CREATEUSERID \n"
                                + " INNER JOIN {0} wp ON bt.BARCODEVALUE=wp.WPINNO \n"
                                + " INNER JOIN WPTRANS wt ON wt.WPINNO=bt.BARCODEVALUE \n"
                                + " INNER JOIN TRANSTABLE tt ON bt.BARCODEVALUE=tt.WPINNO \n"
                                + " LEFT JOIN {1} um ON wp.SENDMAN=um.userid \n"
                                + " WHERE  1=1 {2}\n ";

                #endregion

                if (!string.IsNullOrEmpty(Where))
                    strSql = string.Format(strSql, PageUtility.WprecSchema,PageUtility.UsermSchema, Where);
                else
                    strSql = string.Format(strSql, PageUtility.WprecSchema,PageUtility.UsermSchema, "");

                return strSql;
            }

            #endregion

            #region RoleInfo
            /// <summary>
            /// 角色資訊
            /// </summary>
            /// <returns></returns>
            public string RoleInfo()
            {
                return this.RoleInfo(null, null);
            }
            /// <summary>
            /// 角色資訊
            /// </summary>
            /// <param name="RoleID"></param>
            /// <param name="RoleName"></param>
            /// <returns></returns>
            public string RoleInfo(string RoleID, string RoleName)
            {
                #region SQL Command

                string strSql = "Select rt.RoleID, rt.RoleName ,NVL(rt.Comments, '') As Comments\n"
                      + "From RoleTable rt\n"
                      + "Where 1 = 1\n"
                      + "{0}\n"
                      + "Order By rt.RoleID";

                #endregion

                if (RoleID != null && RoleID.Length > 0) strSql = string.Format(strSql, string.Format("And rt.RoleID='{0}'", RoleID));

                if (RoleName != null && RoleName.Length > 0) strSql = string.Format(strSql, string.Format("And rt.RoleName='{0}'", RoleName));

                strSql = string.Format(strSql, "");

                return strSql;
            }
            #endregion

            #region RoleTable
            /// <summary>
            /// 角色資訊
            /// </summary>
            /// <returns></returns>
            public string RoleTable()
            {
                return this.RoleTable(null);
            }
            /// <summary>
            /// 角色資訊
            /// </summary>
            /// <param name="FieldName">欄位名稱</param>
            /// <returns></returns>
            public string RoleTable(string Where)
            {
                #region SQL Command

                string strSql = "Select *\n"
                    + "From RoleTable rt\n"
                    + "Where 1 = 1 {0}\n";

                #endregion

                if (!string.IsNullOrEmpty(Where)) { strSql = string.Format(strSql, Where); }
                else strSql = string.Format(strSql, "");
                return strSql;
            }
            #endregion

            #region RolePrivilegeTable()
            /// <summary>
            /// 角色權限資訊
            /// </summary>
            /// <param name="RoleID">角色 ID</param>
            /// <returns></returns>
            public string RolePrivilegeTable(int RoleID)
            {
                #region SQL Command

                string strSql = "Select\n"
                    + "	'NODE' || rp.PrivID  As PrivID\n"
                    + "	,Case When rp.ParentID = -1 Then '' Else 'NODE' || rp.ParentID End As ParentID\n"
                    + "	,rp.PrivValue\n"
                    + "	,rp.PrivName\n"
                    + "	,rp.PrivLevel\n"
                    + "	,rp.Seq\n"
                    + "	,NVL(rp.Comments, '') As Comments\n"
                    + "	,'NODE' || rt.PrivID As RolePrivID\n"
                    + "From RolePrivilegeTable rp\n"
                    + "Left Join (Select A.PrivID From RolePrivilege A Where A.RoleID = {0}) rt On (rp.PrivID = rt.PrivID)\n"
                    + "Where 1 = 1 And rp.Status = 0\n"
                    + "Order By rp.ParentID\n";

                #endregion

                strSql = string.Format(strSql, RoleID);

                return strSql;
            }
            #endregion

            #region LogQuery
            /// <summary>
            /// 案件資料查詢
            /// </summary>
            public string LogQuery(string SystemOperatingWhere,string Where)
            {
                #region SQL Command

                //MODIFY BY RICHARD 20160407 加上order by
                string strSql = "Select lt.*,mt.* From LogTable lt\n"
                                      + "Inner Join MessageTable mt On lt.TransResult = mt.MsgID\n"
                                      + "Where 1=1 {0} {1} order by lt.TRANSDATETIME \n";
                #endregion

                strSql = string.Format(strSql, SystemOperatingWhere, Where);

                return strSql;
            }
            #endregion

            #region UserTable
            /// <summary>
            /// 使用者資訊
            /// </summary>
            public string UserTable()
            {
                return this.UserTable(null);
            }
            /// <summary>
            /// 使用者資訊
            /// </summary>
            public string UserTable(string Where)
            {
                #region SQL Command

                string strSql = " Select ut.UserID,\n"
                    + " ut.UserName,\n"
                    + " ut.Password,\n"
                    + " ut.RealName,\n"
                    + " ut.UserStatus,\n"
                    + " ut.CreateTime,\n"
                    + " ut.TEL,\n"
                    + " ut.DEPTID,\n"
                    + " ut.Comments\n"
                    + " From UserTable ut\n"
                    + " Where 1=1\n"
                    + " {0}\n";

                #endregion

                if (Where != null && Where.Length > 0) strSql = string.Format(strSql, Where);
                else strSql = string.Format(strSql, "");

                return strSql;
            }
            /// <summary>
            /// 使用者資訊
            /// </summary>
            /// <param name="UserName">帳號</param>
            /// <param name="Flag">內部或外部</param>
            /// <returns></returns>
            public string UserTable(string UserName, int Flag)
            {
                #region SQL Command

                string strSql = " Select ut.UserID,\n"
                    + "ut.Password,\n"
                    + "ut.UserName,\n"
                    + "ut.RealName,\n"
                    + "ut.UserStatus,\n"
                    + "ut.EMail,\n"
                    + "ut.Comments,\n"
                    + "dep.DepID,\n"
                    + "dep.DepName,\n"
                    + "dep.DepFlag,\n"
                    + "corp.CorpID,\n"
                    + "corp.CorpName,\n"
                    + "corp.CorpFlag,\n"
                    + "IsNull(ut.RoleID, -2) As RoleID,\n"
                    + "IsNull(rt.RoleName, '') As RoleName,\n"
                    + "IsNull(Replace(Convert(NvarChar, ut.CreateTime, 120), '-', '/'), '') As CreateTime\n"
                    + "From UserTable ut\n"
                    + "Inner Join RoleTable rt On (rt.RoleID = ut.RoleID)\n"
                    + "Left Join DepTable dep On (dep.DepID = ut.DepID)\n"
                    + "Left Join CorpTable corp On (corp.CorpID = ut.CorpID)\n"
                    + "Where 1 = 1\n"
                    + "And ut.UserName='{0}'\n"
                    + "And ut.Flag={1}\n";

                #endregion

                strSql = string.Format(strSql, UserName, Flag);

                return strSql;
            }

            // Added by Luke 2016/05/30
            /// <summary>
            /// 使用者資訊之RoleID
            /// </summary>
            /// <param name="UserName">查詢條件</param>
            /// <returns></returns>
            public string RoleIDFromUserTable(string Where)
            {
                #region SQL Command

                string strSql = "Select roleid\n"
                              + "From usertable\n"
                                + "Where 1=1 {0}";
                #endregion

                if (!string.IsNullOrEmpty(Where))
                    strSql = string.Format(strSql, Where);
                else
                    strSql = string.Format(strSql, "");

                return strSql;
            }

            #endregion

            #region UserInfo
            /// <summary>
            /// 使用者資訊
            /// </summary>
            public string UserInfo()
            {
                return this.UserInfo(null);
            }
            /// <summary>
            /// 使用者資訊
            /// </summary>
            public string UserInfo(string UserID)
            {
                #region SQL Command

                string strSql = " Select ut.UserID,\n"
                    + "ut.Password,\n"
                    + "ut.UserName,\n"
                    + "ut.RealName,\n"
                    + "ut.UserStatus,\n"
                    + "ut.Comments,\n"
                    + "ut.Tel,\n"
                    + "ut.DEPTID,\n" 
                    + "NVL(rt.RoleID, 0) As RoleID,\n"
                    + "NVL(rt.RoleName, '') As RoleName\n"
                    + "From UserTable ut\n"
                    + "Inner Join RoleTable rt On (ut.RoleID = rt.RoleID)\n"
                    + "Where 1=1\n"
                    + "{0}\n";

                #endregion

                if (UserID != null && UserID.Length > 0) strSql = string.Format(strSql, string.Format("And ut.UserID='{0}'", UserID));
                else strSql = string.Format(strSql, "");

                return strSql;
            }
            #endregion

            #region UsersMaintain
            /// <summary>
            /// UsersMaintain.aspx
            /// </summary>
            /// <param name="UserName"></param>
            /// <param name="RealName"></param>
            /// <param name="UserRole"></param>
            /// <returns></returns>
            public string UsersMaintain(string UserName, string RealName, string UserRole)
            {
                #region SQL Command

                string strSql = "Select ut.UserID,\n"
                      + "ut.UserName,\n"
                      + "ut.RealName,\n"
                      + "ut.Comments,\n"
                      + "ut.UserStatus,\n"
                      + "ut.RoleID,\n"
                      + "ut.TEL,\n"
                      + "ut.DEPTID,\n"
                      + "dp.DEPTNAME,\n"
                      + "rt.RoleName\n"
                      + "From UserTable ut\n"
                      + "Left Join RoleTable rt  On ut.RoleID =rt.RoleID\n"
                      + "Left Join DEPT dp  On ut.DEPTID =dp.DEPTID\n"
                      + "Where 1 = 1 {0}\n"
                      + "Order By ut.CreateTime";

                #endregion

                string where = string.Empty;

                if (UserName != null && UserName.Length > 0) where += string.Format("And ut.UserName='{0}'\n", UserName);

                if (RealName != null && RealName.Length > 0) where += string.Format("And ut.RealName=N'{0}'\n", RealName);

                //ADD BY RICHARD 20160408
                if (UserRole != null && UserRole.Length > 0)
                {
                    if(!UserRole.Trim().Equals("0"))
                        where += string.Format("And ut.ROLEID='{0}'\n", UserRole);
                }

                strSql = string.Format(strSql, where);

                return strSql;
            }
            #endregion

            #region UsersCheck
            /// <summary>
            /// UsersMaintain.aspx
            /// </summary>
            /// <returns></returns>
            public string UsersCheck(string Where)
            {
                #region SQL Command

                string strSql = "Select *\n"
                    + "From UserTable ut\n"
                    + "Where 1=1 {0}";


                #endregion

                strSql = string.Format(strSql, Where);

                return strSql;
            }
            #endregion

            #region ViewerPrivilegeTable()
            /// <summary>
            /// Viewer預覽權限資訊
            /// </summary>
            /// <returns></returns>
            public string ViewerPrivilegeTable(int RoleID)
            {
                #region SQL Command

                string strSql = "Select\n"
                      + "'NODE' || vt.PrivID As PrivID\n"
                      + ",Case When vt.ParentID = 0 Then '' Else 'NODE' || vt.ParentID End As ParentID\n"
                      + ",vt.PrivName\n"
                      + ",vt.LevelID\n"
                      + ",vt.Seq\n"
                      + ",'NODE' || dp.ViewerPrivID As ViewerPrivID\n"
                      + "From ViewerPrivilegeTable vt\n"
                      + "Left Join (Select A.ViewerPrivID From ViewerRolePrivilegeTable A Where A.RoleID = {0}) dp On (vt.PrivID = dp.ViewerPrivID)\n"
                      + "Where 1 = 1\n"
                      + "Order By vt.ParentID\n";
                #endregion

                strSql = string.Format(strSql, RoleID);

                return strSql;
            }
            #endregion

            #region ViewerAreaPrivilegeTable()
            /// <summary>
            /// Viewer調閱權限資訊
            /// </summary>
            /// <returns></returns>
            public string ViewerAreaPrivilegeTable(int RoleID)
            {
                #region SQL Command

                string strSql = "Select\n"
                      + "'NODE' || vt.PrivID As PrivID\n"
                      + ",Case When vt.ParentID = -1 Then '' Else 'NODE' || vt.ParentID End As ParentID\n"
                      + ",vt.PrivName\n"
                      + ",vt.LevelID\n"
                      + ",vt.Seq\n"
                      + ",'NODE' || dp.ViewerPrivID As ViewerPrivID\n"
                      + "From ViewerAreaPrivilegeTable vt\n"
                      + "Left Join (Select A.ViewerPrivID From ViewerAreaRolePrivilegeTable A Where A.RoleID = {0}) dp On (vt.PrivID = dp.ViewerPrivID)\n"
                      + "Where 1 = 1\n"
                      + "Order By vt.ParentID\n";
                #endregion

                strSql = string.Format(strSql, RoleID);

                return strSql;
            }
            #endregion

            #region WpborrowPrint
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Where"></param>
            /// <returns></returns>
            public string WpborrowPrint(string Where)
            {
                #region SQL Command
                // modify by richard 20160328 add distinct
                string strSql = "Select\n"
                              + " distinct ROW_NUMBER() OVER(ORDER BY wb.wpinno) AS RID,\n" 
                              + " wb.wpinno,\n"
                    //+ " wb.receiver,\n"
                    //+ " bk.onfile,\n"
                              + " ut.tel,\n"
                              + " ut.RealName As receiver\n"
                              + "From Wpborrow wb\n"
                    //+ "Inner Join {0} wpr On wb.wpinno = wpr.wpinno\n"
                    //+ "Inner Join {1} bk On wb.wpinno = bk.wpinno\n"
                              + "Inner Join Usertable ut On wb.receiver = ut.username\n"
                              + "Where 1=1 {2}";
                              //+ "Order By wb.wpinno";
                #endregion

                strSql = string.Format(strSql, PageUtility.WprecSchema, PageUtility.bkwpfileSchema, Where);

                return strSql;
            }
            #endregion

            #region BarcodeCheck
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Where"></param>
            /// <returns></returns>
            public string BarcodeCheck(string Where)
            {
                #region SQL Command

                string strSql = "Select bt.*\n"
                              + "From BarcodeTable bt\n"
                              + "Where 1=1 {0}";
                #endregion

                strSql = string.Format(strSql,  Where);

                return strSql;
            }
            #endregion

            #region BoxNOCheck
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Where"></param>
            /// <returns></returns>
            public string BoxNOCheck(string Where)
            {
                #region SQL Command

                //MODIFY BY RICHARD 20160331 for 系統目前檔案調借設定為只要是同一卷宗號之檔案無法同時調借，除綁定卷宗號外，請加綁定檔號前4碼相同者才無法同時調借。
                string strSql = "Select bk.wpinno ,wp.RECEIVER from {0} bk\n"
                                + "Inner Join (Select boxno,fileno From {0} A Where 1=1 {1}) B On bk.boxno = B.BOXNO AND SUBSTR(bk.fileno,0,4)=SUBSTR(B.fileno,0,4) \n"
                                + "Inner Join wpborrow wp On bk.wpinno = wp.wpinno\n"
                                + "Where wp.redate Is null And wp.viewtype = 1";
                #endregion

                strSql = string.Format(strSql, PageUtility.WprecSchema, Where);

                return strSql;
            }
            #endregion

            #region WprecCheck
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Where"></param>
            /// <returns></returns>
            public string WprecCheck(string Where)
            {
                #region SQL Command

                //MODIFY BY RICHARD 20160427 for performance
                string strSql = "Select wr.WPINNO \n"
                              + "From {0} wr\n"
                              + "Where {1}\n";
                #endregion

                strSql = string.Format(strSql, PageUtility.WprecSchema, Where);

                return strSql;
            }
            #endregion

            #region WpborrowCheckByDOC
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Where"></param>
            /// <returns></returns>
            public string WpborrowCheckByDOC(string Where)
            {
                #region SQL Command

                string strSql = "Select wp.*\n"
                              + "From Wpborrow wp\n"
                              + "Where wp.REDATE IS null {0}\n";
                #endregion

                strSql = string.Format(strSql, Where);

                return strSql;
            }
            #endregion

            #region WpborrowCheckByELSC
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Where"></param>
            /// <returns></returns>
            public string WpborrowCheckByELSC(string Where)
            {
                #region SQL Command

                string strSql = "Select wp.*\n"
                              + "From Wpborrow wp\n"
                              + "Where wp.REDATE IS null {0}\n";
                #endregion

                strSql = string.Format(strSql, Where);

                return strSql;
            }
            #endregion

            #region WpborrowInfo
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Where"></param>
            /// <returns></returns>
            public string WpborrowInfo(string Where)
            {
                #region SQL Command

                string strSql = "Select\n"
                              + " wr.wpinno,\n"
                              + " wr.wpoutno,\n"
                              + " wr.wpindate,\n"
                              + " wr.caseno,\n"
                              + " wr.commname,\n"
                              + " wb.prtflag,\n"
                              + " wb.kind,\n"
                              + " wb.viewtype,\n"
                              + " wb.receiver,\n"
                              + " wb.transt,\n"
                              + " wb.redate,\n"
                              + " cl.cirlname,\n"
                              + " wr.fileno,\n"
                              + " bt.Barcodevalue,\n"
                              + " bt.onfile,\n"
                              + " wr.filedate,\n"
                              + " wt.marker\n"
                              + "From {0} wr\n"
                              + "Left Join Wptrans wt On wr.wpinno = wt.wpinno\n"
                              + "Left Join Wpborrow wb On wr.wpinno = wb.wpinno\n"
                              + "Left Join {1} cl On wr.wpkind = cl.wpkind And wr.wptype = cl.wptype\n"
                              + "Left Join BarcodeTable bt On wr.wpinno = bt.barcodevalue\n"
                              + "Where 1=1 {2}\n"
                              + "Order By wb.transt Desc";
                #endregion

                strSql = string.Format(strSql, PageUtility.WprecSchema, PageUtility.CirlmSchema, Where);

                return strSql;
            }
            #endregion

            #region WpborrowQuery
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Where"></param>
            /// <returns></returns>
            public string WpborrowQuery(string Where)
            {
                #region SQL Command

                string strSql = "Select\n"
                              + " wr.wpinno,\n"
                              + " wr.wpoutno,\n"
                              + " wr.wpindate,\n"
                              + " wr.caseno,\n"
                              + " wr.commname,\n"
                              + " wb.prtflag,\n"
                              + " wb.kind,\n"
                              + " wb.viewtype,\n"
                              + " wb.receiver,\n"
                              + " wb.transt,\n"
                              + " wb.redate,\n"
                              + " cl.cirlname,\n"
                              + " wr.fileno,\n"
                              + " wr.filedate\n"
                              + "From {0} wr\n"
                              + "Inner Join Wpborrow wb On wr.wpinno = wb.wpinno\n"
                              + "Left Join {1} cl On wr.wpkind = cl.wpkind And wr.wptype = cl.wptype\n"
                              + "Where 1=1 {2}\n";
                #endregion

                strSql = string.Format(strSql, PageUtility.WprecSchema, PageUtility.CirlmSchema, Where);

                return strSql;
            }
            #endregion

            #region WpborrowToApprove
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Where"></param>
            /// <returns></returns>
            public string WpborrowToApprove(string Where)
            {
                #region SQL Command

                string strSql = "Select\n"
                              + " wb.receiver,\n"
                              + " wb.transt,\n"
                              + " wb.REASON,\n"
                              + " wb.wpinno,\n"
                              + " wb.prtflag,\n"
                              + " ut.tel,\n"
                              + " wr.fileno,\n"
                              + " wr.boxno,\n"
                              + " wr.wpoutno,\n"
                              + " wr.commname,\n"
                              + " ut.realname,\n"
                              + " bt.onfile,\n"
                              + " wb.viewtype,\n"
                              + " Case When wb.viewtype = '1' Then '紙本調閱' When wb.viewtype = '2' Then '電子調閱' Else '其他' End As viewtypename\n"
                              + "From Wpborrow wb\n"
                              + "Inner Join Usertable ut On wb.receiver = ut.username\n"
                              + "Left Join Barcodetable bt On wb.wpinno = bt.barcodevalue\n"
                              + "Inner Join {0} wr On wb.wpinno = wr.wpinno\n"
                              + "Where 1=1 {1}";

                #endregion

                strSql = string.Format(strSql, PageUtility.WprecSchema, Where);

                return strSql;
            }
            #endregion

            #region FileQuery
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Where"></param>
            /// <returns></returns>
            public string FileQuery(string Where)
            {
                #region SQL Command

                string strSql = "SELECT ROW_NUMBER() OVER(ORDER BY WP.TRANST DESC) AS RID,WP.WPINNO,WP.TRANST,WP.RECEIVER,UT.REALNAME as REALNAME1,WP.REDATE,WP.USERID,UT2.REALNAME as REALNAME2,WP.APPROVEUSERID,UT3.REALNAME as REALNAME3,WP.APPROVEDATE,FB.WORKERID,FB.GETIME,UT.TEL FROM WPBORROW WP\n"
                                + "Left JOIN FILEBORO FB ON WP.WPINNO=FB.WPINNO AND WP.TRANST=FB.TRANST AND WP.RECEIVER=FB.RECEIVER\n"
                                + "Left JOIN USERTABLE UT ON WP.RECEIVER=UT.USERNAME\n"
                                + "Left JOIN USERTABLE UT2 ON WP.USERID=UT2.USERID\n"
                                + "Left JOIN USERTABLE UT3 ON WP.APPROVEUSERID=UT3.USERID\n"
                                + "Where 1=1 {1}\n"
                                + "ORDER BY WP.TRANST DESC";
                #endregion

                strSql = string.Format(strSql, PageUtility.WprecSchema, Where);

                return strSql;
            }
            #endregion

            #region WprecQuery
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Where"></param>
            /// <returns></returns>
            public string WprecQuery(string Where)
            {
                #region SQL Command

                string strSql = "Select\n"
                                + "wr.wpinno,\n"
                                + "wr.wpoutno,\n"
                                + "wr.wpindate,\n"
                                + "wr.caseno,\n"
                                + "wr.commname,\n"
                                + "cl.cirlname,\n"
                                + "wr.fileno,\n"
                                + "wr.filedate\n"
                                + "From {0} wr\n"
                                + "Left Join {1} cl On wr.wpkind = cl.wpkind And wr.wptype = cl.wptype\n"
                                + "Where 1=1 {2}";
                #endregion

                strSql = string.Format(strSql, PageUtility.WprecSchema, PageUtility.CirlmSchema, Where);

                return strSql;
            }
            #endregion

            #region Wprec
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Where"></param>
            /// <returns></returns>
            public string Wprec(string Where)
            {
                #region SQL Command

                string strSql = "Select wp.*,bk.onfile,um.USERNAME\n"
                                + " From {0} wp\n"
                                + "Inner Join {1} um\n"
                                + "On wp.sendman = um.userid\n"
                                + "Left Join {2} bk\n"
                                + "On wp.wpinno = bk.wpinno\n"
                                + "WHERE  1=1 {3}\n"
                                + " Union\n"
                                + "Select  wp.*,wps.RECEIVER As OnFile,um.USERNAME\n"
                                + " From {0} wp\n"
                                 + "Inner Join {1} um\n"
                                + "On wp.sendman = um.userid\n"
                                + "Inner Join WPTRANS wps\n"
                                + "On wp.wpinno = wps.wpinno\n"
                                + "WHERE  1=1 {3}\n";

                #endregion


                //#region SQL Command

                //string strSql = "Select w.*,u.* From {0} w,{1} u\n"
                //              + "Where 1=1 And w.sendman = u.userid {2}";
                //#endregion

                strSql = string.Format(strSql, PageUtility.WprecSchema, PageUtility.UsermSchema,PageUtility.bkwpfileSchema, Where);

                return strSql;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Where"></param>
            /// <returns></returns>
            public string WprecBySingle(string Where)
            {
                #region SQL Command

                string strSql = "Select  wp.*,wps.RECEIVER As OnFile,um.USERNAME\n"
                                + " From {0} wp\n"
                                + "Inner Join {1} um\n"
                                + "On wp.sendman = um.userid\n"
                                + "Inner Join WPTRANS wps\n"
                                + "On wp.wpinno = wps.wpinno\n"
                                + "WHERE  1=1 {3}\n";

                #endregion

                strSql = string.Format(strSql, PageUtility.WprecSchema, PageUtility.UsermSchema, PageUtility.bkwpfileSchema, Where);

                return strSql;
            }
            #endregion

            #region Transtable
            // Added by Luke 2016/05/30
            /// <summary>
            /// Transtable
            /// </summary>
            /// <param name="Where"></param>
            /// <returns></returns>
            public string Transtable(string Where)
            {
                #region SQL Command

                string strSql = "Select tt.RECEIVER, tt.TRANSTIME, tt.WPINNO, tt.COMMNAME, ROW_NUMBER() OVER(ORDER BY tt.TRANSTIME DESC) as RID \n"
                                + " From TRANSTABLE tt \n"
                                + " WHERE  1=1 {0}\n ";

                #endregion

                if (!string.IsNullOrEmpty(Where))
                    strSql = string.Format(strSql ,Where);
                else
                    strSql = string.Format(strSql,"");

                return strSql;
            }
            #endregion

            #region TransQueryStatisticsReport
            /// <summary>
            /// TranstableGroup
            /// </summary>
            /// <param name="Where"></param>
            /// <returns></returns>
            public string TranstableGroup(string Where)
            {
                #region SQL Command

                string strSql = "Select count(tt.TRANSTIME) AS TRANSCOUNT,substr(tt.TRANSTIME,1,10) AS TRANSTIME,tt.RECEIVER,ROW_NUMBER() OVER (ORDER BY substr(tt.TRANSTIME,1,10) ASC) AS RID, wp.FILENO, NVL(wp.FILENO,0) as ISFILE \n"
                                + " From TRANSTABLE tt \n"
                                + " INNER JOIN {0} wp ON tt.WPINNO=wp.wpinno \n"
                                + " WHERE  1=1 {1}\n ";

                #endregion

                if (!string.IsNullOrEmpty(Where))
                    strSql = string.Format(strSql, PageUtility.WprecSchema, Where);
                else
                    strSql = string.Format(strSql, PageUtility.WprecSchema, "");

                return strSql;
            }

            // Added by Luke 2016/09/12
            /// <summary>
            /// TranstableDetail
            /// </summary>
            /// <param name="Where"></param>
            /// <returns></returns>
            public string TranstableDetail(string Where)
            {
                #region SQL Command

                string strSql = "SELECT tt.WPINNO,wp.COMMNAME,wp.COMMADD,wp.WPOUTDATE,um.USERNAME as SENDMAN,wp.FILENO,wp.BOXNO,wt.RECEIVER,ut.REALNAME,tt.RECEIVER AS RECEIVER2,wp.KEEPYR,tt.TRANSTIME,wp.FILEDATE,bt.CREATETIME \n"
                                + " From TRANSTABLE tt \n"
                                + " INNER JOIN {0} wp ON tt.WPINNO=wp.WPINNO \n"
                                + " LEFT OUTER JOIN WPTRANS wt ON wt.WPINNO=tt.WPINNO \n"
                                + " LEFT OUTER JOIN BARCODETABLE bt ON bt.BARCODEVALUE=tt.WPINNO \n"
                                + " LEFT OUTER JOIN usertable ut ON ut.USERID=bt.CREATEUSERID \n"
                                + " LEFT OUTER JOIN {1} um ON wp.SENDMAN=um.userid \n"
                                + " WHERE  1=1 {2}\n ";

                #endregion

                if (!string.IsNullOrEmpty(Where))
                    strSql = string.Format(strSql, PageUtility.WprecSchema,PageUtility.UsermSchema, Where);
                else
                    strSql = string.Format(strSql, PageUtility.WprecSchema,PageUtility.UsermSchema, "");

                return strSql;
            }
            #endregion

        }
        #endregion

        #region Update
        /// <summary>
        /// Update
        /// </summary>
        public class Update
        {
            #region BackPiecesTable
            /// <summary>
            /// BranchTable
            /// </summary>
            /// <returns></returns>
            public string BackPiecesTable(string Where)
            {
                #region SQL Command

                string strSql = "Update BackPiecesTable Set\n"
                    + "	SystemCodeID = @SystemCodeID,\n"
                    + "	UserID = @UserID,\n"
                    + "	Comments = @Comments\n"
                    + "Where 1=1 {0}\n";

                #endregion

                strSql = string.Format(strSql, Where);

                return strSql;
            }
            #endregion

            #region BranchTable
            /// <summary>
            /// BranchTable
            /// </summary>
            /// <returns></returns>
            public string BranchTable(string HeadBranchID, string BranchCode)
            {
                #region SQL Command

                string strSql = "Update BranchTable Set\n"
                    + "	HeadBranchID = @HeadBranchID,\n"
                    + "	BranchCode = @BranchCode,\n"
                    + "	BranchName = @BranchName,\n"
                    + "	CorpID = @CorpID,\n"
                    + "	UserName = @UserName\n"
                    + "Where HeadBranchID = {0} And BranchCode = '{1}'\n";

                #endregion

                strSql = string.Format(strSql,
                 HeadBranchID, BranchCode);

                return strSql;
            }
            #endregion

            #region BarcodeTable
            public string BarcodeTable(string Where)
            {
                #region SQL Command

                string strSql = "Update BarcodeTable Set\n"
                    + " FileNo = null,\n"
                    + " FileDate = null,\n"
                    + " KeepYr = null,\n"
                    + " BoxNo = null,\n"
                    + " WPOUTNO = null,\n"
                    + " ONFILE = null\n"
                    + "Where 1=1 {0}\n";

                #endregion

                strSql = string.Format(strSql,
                    Where);

                return strSql;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Data"></param>
            /// <param name="Where"></param>
            /// <returns></returns>
            public string BarcodeTable(Hashtable Data, string Where)
            {
                #region SQL Command

                string strSql = "Update BarcodeTable Set\n"
                    + " FileNo = '{0}',\n"
                    + " FileDate = '{1}',\n"
                    + " KeepYr = '{2}',\n"
                    + " BoxNo = '{3}',\n"
                    + " LastModifyUserID = '{4}',\n"
                    + " LastModifyTime = {5}\n"
                    + "Where 1=1 {6}\n";

                #endregion

                strSql = string.Format(strSql,
                    Data["FileNo"].ToString(),
                    Data["FileDate"].ToString(),
                    Data["KeepYr"].ToString(),
                    Data["BoxNo"].ToString(),
                    Data["LastModifyUserID"].ToString(),
                    Data["LastModifyTime"].ToString(),
                    Where);

                return strSql;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Data"></param>
            /// <param name="Where"></param>
            /// <returns></returns>
            public string BarcodeTableByBarCodeValue(Hashtable Data, string Where)
            {
                #region SQL Command

                string strSql = "Update BarcodeTable Set\n"
                    + " BARCODEVALUE = '{0}'\n"
                    + "Where 1=1 {1}\n";

                #endregion

                strSql = string.Format(strSql,
                    Data["BARCODEVALUE"].ToString(),
                    Where);

                return strSql;
            }

            #endregion

            #region SystemCodeStatus
            /// <summary>
            /// UsersMaintain.aspx
            /// </summary>
            /// <param name="UserID"></param>
            /// <returns></returns>
            public string FormTable(string FormID, string Status)
            {
                #region SQL Command

                string strSql = "Update FormTable Set Status = {1} Where FormID = {0}";

                #endregion

                strSql = string.Format(strSql, FormID, Status);

                return strSql;
            }
            #endregion

            #region RoleTable
            /// <summary>
            /// RoleTable
            /// </summary>
            /// <param name="FieldName"></param>
            /// <returns></returns>
            public string RoleTable(string RoleName, string Comments, string RoleID)
            {
                #region SQL Command

                string strSql = "Update RoleTable Set\n"
                    + "	RoleName = '{0}',\n"
                    + "	Comments = '{1}'\n"
                    + "Where RoleID = {2}";

                #endregion

                strSql = string.Format(strSql, RoleName, Comments, RoleID);

                return strSql;
            }
            #endregion

            #region HeadBranchTable
            /// <summary>
            /// HeadBranchTable
            /// </summary>
            /// <returns></returns>
            public string HeadBranchTable(string HeadBranchID)
            {
                #region SQL Command

                string strSql = "Update HeadBranchTable Set\n"
                    + "	HeadBranchCode = @HeadBranchCode,\n"
                    + "	HeadBranchName = @HeadBranchName,\n"
                    + "	Name = @Name,\n"
                    + "	Phone = @Phone\n"
                    + "Where HeadBranchID = {0}\n";

                #endregion

                strSql = string.Format(strSql,
                 HeadBranchID);

                return strSql;
            }
            #endregion

            #region SystemTable
            /// <summary>
            /// RoleTable
            /// </summary>
            /// <param name="Data"></param>
            /// <returns></returns>
            public string SystemTable()
            {
                #region SQL Command

                string strSql = "Update SystemTable Set\n"
                    + "	SystemComment = :systemcomment\n"
                    + "Where SystemName = :systemname\n";

                #endregion

                return strSql;
            }
            public string SystemTable(string SystemName, string SystemComment)
            {
                #region SQL Command

                string strSql = "Update SystemTable Set\n"
                    + "	SystemComment = '{1}'\n"
                    + "Where SystemName = '{0}'\n";

                #endregion

                strSql = string.Format(strSql, SystemName, SystemComment);

                return strSql;
            }
            #endregion

            #region SystemCodeTable()
            public string SystemCodeTable(string SystemCodeID)
            {
                #region SQL Command

                string strSql = "Update SystemCodeTable Set\n"
                    + "	SystemCode = @SystemCode,\n"
                    + "	SystemCodeName = @SystemCodeName,\n"
                    + "	SystemCodeStatus = @SystemCodeStatus\n"
                    + "Where SystemCodeID = {0}";

                #endregion

                strSql = string.Format(strSql, SystemCodeID);

                return strSql;
            }
            #endregion

            #region SystemCodeStatus
            /// <summary>
            /// UsersMaintain.aspx
            /// </summary>
            /// <param name="UserID"></param>
            /// <returns></returns>
            public string SystemCodeStatus(string SystemCodeID, string SystemCodeStatus)
            {
                #region SQL Command

                string strSql = "Update SystemCodeTable Set SystemCodeStatus = {1} Where SystemCodeID = {0}";

                #endregion

                strSql = string.Format(strSql, SystemCodeID, SystemCodeStatus);

                return strSql;
            }
            #endregion

            #region UserMaintain_Enable
            /// <summary>
            /// 
            /// </summary>
            /// <param name="UserID"></param>
            /// <param name="Status"></param>
            /// <returns></returns>
            public string UserMaintain_Enable(string UserID, string Status)
            {
                #region SQL Command

                string strSql = "Update UserTable Set UserStatus = '{1}' Where UserID = '{0}'";

                #endregion

                strSql = string.Format(strSql, UserID, Status);

                return strSql;
            }
            #endregion

            #region CaseStatus
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public string CaseStatus(string Status, string CloseTime, string Where)
            {
                #region SQL Command

                string strSql = "Update CaseTable Set CaseStatus = {0},CloseTime = {1} Where 1=1{2}";

                #endregion

                strSql = string.Format(strSql, Status, CloseTime, Where);

                return strSql;
            }
            #endregion

            #region IndexClassTable
            /// <summary>
            /// UserTable
            /// </summary>
            /// <param name="Data"></param>
            /// <returns></returns>
            public string IndexClassTable(Hashtable Data)
            {
                #region SQL Command

                string strSql = "Update IndexClassTable Set\n"
                    + "	IndexClassCode = '{1}',\n"
                    + "	IndexClassName = '{2}',\n"
                    + "	Attrib = {3},\n"
                    + "	BarcodePreFixedName = '{4}',\n"
                    + "	CheckRuleID = {5},\n"
                    + "	CheckType = {6}\n"
                    + "Where IndexClassID = {0}\n";

                #endregion

                strSql = string.Format(strSql,
                    Data["IndexClassID"].ToString(),
                    Data["IndexClassCode"].ToString(),
                    Data["IndexClassName"].ToString(),
                    Data["Attrib"].ToString(),
                    Data["BarcodePreFixedName"].ToString(),
                    Data["CheckRuleID"].ToString(),
                    Data["CheckType"].ToString());

                return strSql;
            }
            #endregion

            #region UserTable
            /// <summary>
            /// UserTable
            /// </summary>
            /// <param name="Data"></param>
            /// <returns></returns>
            public string UserTable(Hashtable Data)
            {
                #region SQL Command

                string strSql = "Update UserTable Set\n"
                    + "	Password = '{1}'\n"
                    + "	,RealName = '{2}'\n"
                    + "	,UserUnit = {3}\n"
                    + "Where UserID = {0}\n";

                #endregion

                strSql = string.Format(strSql,
                    Data["UserID"].ToString(),
                    Data["Password"].ToString(),
                    Data["RealName"].ToString(),
                    Data["UserUnit"].ToString());

                return strSql;
            }
            #endregion

            #region DEPT
            /// <summary>
            /// DEPT
            /// </summary>
            /// <param name="DEPTID">部門代碼</param>
            /// <param name="DEPTNAME">部門名稱</param>
            /// <param name="STATUS">部門狀態</param> 
            /// <returns></returns>
            public string DEPT(string strDEPTID,string strDEPTNAME,string strStatus)
            {
                #region SQL Command

                string strSql = "Update DEPT Set\n"
                    + "	DEPTNAME = '{1}' ,\n"
                    + " Status = '{2}' \n"
                    + "Where DEPTID = {0}\n";

                #endregion

                strSql = string.Format(strSql,
                    strDEPTID.Trim(),
                    strDEPTNAME.Trim(),
                    strStatus.Trim());

                return strSql;
            }

            /// <summary>
            /// DEPT
            /// </summary>
            /// <param name="DEPTID">部門代碼</param>
            /// <param name="STATUS">部門狀態</param> 
            /// <returns></returns>
            public string DEPTSTATUS(string strDEPTID, string strStatus)
            {
                #region SQL Command

                string strSql = "Update DEPT Set\n"
                    + " Status = '{1}' \n"
                    + "Where DEPTID = {0}\n";

                #endregion

                strSql = string.Format(strSql,
                    strDEPTID.Trim(),
                    strStatus.Trim());

                return strSql;
            }

            #endregion

            #region UserTablePassWord
            /// <summary>
            /// UserTable
            /// </summary>
            /// <param name="Data"></param>
            /// <returns></returns>
            public string UserTablePassWord(Hashtable Data)
            {
                #region SQL Command

                string strSql = "Update UserTable Set\n"
                    + "	Password =N'{0}',\n"
                    + " TEL =N'{2}'\n"
                    + "Where UserID ='{1}'\n";

                #endregion

                strSql = string.Format(strSql,
                    Data["Password"].ToString(),
                     Data["UserID"].ToString(),
                      Data["TEL"].ToString());

                return strSql;
            }
            #endregion

            #region UserTableTel
            /// <summary>
            /// UserTable
            /// </summary>
            /// <param name="Data"></param>
            /// <returns></returns>
            public string UserTableTel(Hashtable Data)
            {
                #region SQL Command

                string strSql = "Update UserTable Set\n"
                    + " TEL =N'{1}'\n"
                    + "Where UserID ='{0}'\n";

                #endregion

                strSql = string.Format(strSql,
                     Data["UserID"].ToString(),
                      Data["TEL"].ToString());

                return strSql;
            }
            #endregion

            #region FILEBORO
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Data"></param>
            /// <returns></returns>
            public string FILEBORO(Hashtable Data, string Where)
            {
                #region SQL Command

                string strSql = "Update FILEBORO Set\n"
                    + "	CHK =N'{1}'\n"
                    + "Where {0}\n";

                #endregion

                strSql = string.Format(strSql, Where,
                    Data["CHK"].ToString());

                return strSql;
            }
            #endregion

            #region wpborrow
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Data"></param>
            /// <returns></returns>
            public string wpborrow(Hashtable Data, string Where)
            {
                #region SQL Command

                string strSql = "Update wpborrow Set\n"
                    + "	EXTEN =N'{1}',\n"
                    + "	APPROVEUSERID =N'{2}'\n"
                    + "Where 1=1 {0}\n";

                #endregion

                strSql = string.Format(strSql, Where,
                    Data["EXTEN"].ToString(),
                     Data["APPROVEUSERID"].ToString());

                return strSql;
            }
            #endregion

            #region wpborrowByHurry
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Data"></param>
            /// <returns></returns>
            public string wpborrowByHurry(Hashtable Data, string Where)
            {
                #region SQL Command

                string strSql = "Update wpborrow Set\n"
                    + "	HURRYDATE ='{1}',\n"
                    + "	HURRYTIME ='{2}'\n"
                    + "Where 1=1 {0}\n";

                #endregion

                strSql = string.Format(strSql, Where,
                    Data["HURRYDATE"].ToString(),
                     Data["HURRYTIME"].ToString());

                return strSql;
            }
            #endregion

            #region wpborrowByViewType
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Data"></param>
            /// <returns></returns>
            public string wpborrowByViewType(string UserID)
            {
                #region SQL Command

                string strSql = "Update WPBORROW Set REDATE = sysdate ,UserID =N'{0}'\n"
                               + "Where Transt +7 <=sysdate And ViewType = 2 And Exten='N' And NVL(EXTENSIONCOUNT,0) =0 And REDATE Is Null";

                #endregion

                strSql = string.Format(strSql, UserID);

                return strSql;
            }
            #endregion

            #region wpborrowByViewTypeExten
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Data"></param>
            /// <returns></returns>
            public string wpborrowByViewTypeExten(string UserID)
            {
                #region SQL Command

                string strSql = "Update WPBORROW Set REDATE = sysdate ,UserID =N'{0}'\n"
                               + "Where EXTENSIONDATE+7 <=sysdate And ViewType = 2 And Exten='N' And NVL(EXTENSIONCOUNT,0) !=0 And REDATE Is Null";

                #endregion

                strSql = string.Format(strSql, UserID);

                return strSql;
            }
            #endregion

            #region fbborrowByVisa
            /// <summary>
            /// UserTable
            /// </summary>
            /// <param name="Data"></param>
            /// <returns></returns>
            public string fbborrowByVisa(string Where)
            {
                #region SQL Command

                string strSql = "Update FILEBORO A Set\n"
                                + "transt =(Select  CASE kind\n"
                                + "WHEN N'1' THEN transt + 7\n"
                                + "WHEN N'2' THEN transt + 7\n"
                                + "WHEN N'3' THEN transt + 90\n"
                                + "ELSE transt END As transtExten From wpborrow wb Where 1=1 {0} And REDATE is null)\n"
                                + "Where 1=1  And EXISTS (SELECT * FROM FILEBORO fb  Inner Join \n"
                                + "wpborrow wb On fb.WPINNO = wb.WPINNO And fb.Receiver = wb.Receiver And fb.TRANST = wb.TRANST Where A.WPINNO = fb.WPINNO  {0})";
                #endregion

                strSql = string.Format(strSql, Where);

                return strSql;
            }
            #endregion

            #region wpborrowByVisa
            /// <summary>
            /// UserTable
            /// </summary>
            /// <param name="Data"></param>
            /// <returns></returns>
            public string wpborrowByVisa(Hashtable Data, string Where)
            {
                #region SQL Command

                string strSql = "Update wpborrow wb Set\n"
                                + "EXTEN ='{1}',\n"
                                + "EXTENSIONDATE ='{2}',\n"
                                + "EXTENSIONCOUNT ='{3}',\n"
                                + "VISAEXTENSIONDATE ='{4}'\n"
                                + "Where 1=1 {0}";
                #endregion

                strSql = string.Format(strSql, Where,
                    Data["EXTEN"].ToString(),
                    Data["EXTENSIONDATE"].ToString(),
                    Data["EXTENSIONCOUNT"].ToString(),
                    Data["VISAEXTENSIONDATE"].ToString());

                return strSql;
            }
            #endregion

            #region wpborrowByVisaNo
            /// <summary>
            /// UserTable
            /// </summary>
            /// <param name="Data"></param>
            /// <returns></returns>
            public string wpborrowByVisaNo(Hashtable Data, string Where)
            {
                #region SQL Command

                string strSql = "Update wpborrow wb Set\n"
                                + "EXTEN ='{1}'\n"
                                + "Where 1=1 {0}";
                #endregion

                strSql = string.Format(strSql, Where,
                    Data["EXTEN"].ToString());

                return strSql;
            }
            #endregion

            #region wpborrowPaperAlsoFile
            /// <summary>
            /// UserTable
            /// </summary>
            /// <param name="Data"></param>
            /// <returns></returns>
            public string wpborrowPaperAlsoFile(Hashtable Data, string Where)
            {
                #region SQL Command

                string strSql = "Update wpborrow Set\n"
                    + "	REDATE =N'{1}',\n"
                    + " UserID={2}\n"
                    + "Where {0}\n";

                #endregion

                strSql = string.Format(strSql, Where,
                    Data["REDATE"].ToString(),
                    Data["USERID"].ToString());

                return strSql;
            }
            #endregion

            #region WpborrowApprove
            /// <summary>
            /// 
            /// </summary>
            /// <param name="PrtFlag"></param>
            /// <param name="Where"></param>
            /// <param name="HaveReDate"></param>
            /// <returns></returns>
            public string WpborrowApprove(string PrtFlag, string Where, bool HaveReDate)
            {
                #region SQL Command

                string UpdateSql = string.Format("PrtFlag = '{0}'", PrtFlag);

                if (HaveReDate) UpdateSql += " ,ReDate = SYSDATE";

                string strSql = "Update Wpborrow Set\n"
                              + " {0}\n"
                              + "Where 1=1 {1}";

                #endregion

                strSql = string.Format(strSql, UpdateSql, Where);

                return strSql;
            }
            #endregion

            #region WpborrowApproveDate
            /// <summary>
            /// 
            /// </summary>
            /// <param name="PrtFlag"></param>
            /// <param name="Where"></param>
            /// <param name="HaveReDate"></param>
            /// <returns></returns>
            public string WpborrowApproveDate(string PrtFlag, string Where, bool HaveReDate)
            {
                #region SQL Command

                string UpdateSql = string.Format("PrtFlag = '{0}'", PrtFlag);

                if (HaveReDate) UpdateSql += " ,ReDate = SYSDATE";

                string strSql = "Update Wpborrow Set\n"
                              + " {0}\n"
                              + " ,APPROVEDATE = SYSDATE\n"
                              + "Where 1=1 {1}";

                #endregion

                strSql = string.Format(strSql, UpdateSql, Where);

                return strSql;
            }
            #endregion

            #region WptransEdit
            /// <summary>
            /// Wprec Table
            /// </summary>
            /// <param name="Data"></param>
            /// <param name="Where"></param>
            /// <returns></returns>
            public string WptransEdit(string Receiver, string Where)
            {
                #region SQL Command

                string strSql = "Update WPTRANS Set\n"
                    + "receiver = '{0}'\n"
                    + "Where 1=1 {1}\n";
                #endregion

                strSql = string.Format(strSql,
                    Receiver,
                    Where);

                return strSql;
            }
            #endregion

            #region WprecEdit
            /// <summary>
            /// Wprec Table
            /// </summary>
            /// <param name="Data"></param>
            /// <param name="Where"></param>
            /// <returns></returns>
            public string WprecEdit(Hashtable Data, string Where)
            {
                #region SQL Command

                string strSql = "Update {0} Set\n"
                    + " FileNo = '{1}',\n"
                    + " FileDate = '{2}',\n"
                    + " KeepYr = '{3}',\n"
                    + " BoxNo = '{4}'\n"
                    + "Where 1=1 {5}\n";
                #endregion

                strSql = string.Format(strSql,
                    PageUtility.WprecSchema,
                    Data["FileNo"].ToString(),
                    Data["FileDate"].ToString(),
                    Data["KeepYr"].ToString(),
                    Data["BoxNo"].ToString(),
                    Where);

                return strSql;
            }
            #endregion

            #region Wprec
            /// <summary>
            /// Wprec Table
            /// </summary>
            /// <param name="Data"></param>
            /// <param name="Where"></param>
            /// <returns></returns>
            public string Wprec(Hashtable Data, string Where)
            {
                #region SQL Command

                string strSql = "Update {0} Set\n"
                    + " FileNo = '{1}',\n"
                    + " FileDate = '{2}',\n"
                    + " KeepYr = '{3}',\n"
                    + " BoxNo = '{4}'\n"
                    + "Where 1=1 {5}\n"
                    + "And FILENO is null\n"
                    + "And FILEDATE is null\n"
                    + "And KEEPYR is null\n"
                    + "And BOXNO is null\n"
                    //ADD BY RICHARD 20160427 檢查該文號是否有發文(發文日期或發文人員有值)
                    + "And (SENDMAN is not null or WPOUTDATE is not null) \n";  

                #endregion

                strSql = string.Format(strSql,
                    PageUtility.WprecSchema,
                    Data["FileNo"].ToString(),
                    Data["FileDate"].ToString(),
                    Data["KeepYr"].ToString(),
                    Data["BoxNo"].ToString(),
                    Where);

                return strSql;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Data"></param>
            /// <param name="Where"></param>
            /// <returns></returns>
            public string WprecDelete(string Where)
            {
                #region SQL Command

                string strSql = "Update {0} Set\n"
                    + " FileNo = NULL,\n"
                    + " FileDate = NULL,\n"
                    + " KeepYr = NULL,\n"
                    + " BoxNo = NULL\n"
                    + "Where 1=1 {1}\n";

                #endregion

                strSql = string.Format(strSql, PageUtility.WprecSchema, Where);

                return strSql;
            }
            #endregion

            #region WptransDelete
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Where"></param>
            /// <returns></returns>
            public string WptransDelete(string Where)
            {
                #region SQL Command

                string strSql = "Update Wptrans Set\n"
                    + " Marker = 'N'\n"
                    + "Where 1=1 {0}\n";

                #endregion

                strSql = string.Format(strSql, Where);

                return strSql;
            }
            #endregion

            #region UploadTimeTable
            /// <summary>
            /// UploadTimeTable
            /// </summary>
            /// <param name="Data"></param>
            /// <returns></returns>
            public string UploadTimeTable(Hashtable Data)
            {
                #region SQL Command

                string strSql = "Update UploadTimeTable Set\n"
                    + "	UploadTH ={0},\n"
                    + "	UploadTM = {1},\n"
                    + "	Comments = @Comments\n"
                    + "Where UserID = @UserID\n";

                #endregion

                strSql = string.Format(strSql,
                    Data["UploadTH"].ToString(),
                    Data["UploadTM"].ToString());

                return strSql;
            }
            #endregion
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete
        /// </summary>
        public class Delete
        {
            #region BarcodeTable
            /// <summary>
            /// 刪除 BarcodeTable
            /// </summary>
            /// <param name="BarcodeValue">收文文號</param>
            /// <returns></returns>
            public string BarcodeTable(string BarcodeValue)
            {
                #region SQL Command

                string strSql = "Delete From BarcodeTable Where BarcodeValue = '{0}'";

                #endregion

                strSql = string.Format(strSql, BarcodeValue);

                return strSql;
            }

            // Added by Luke
            /// <summary>
            /// 刪除 BarcodeTable
            /// </summary>
            /// <param name="BarcodeValue">收文文號</param>
            /// <param name="CaseID">案號</param>
            /// <returns></returns>
            public string BarcodeTable(string BarcodeValue, string CaseID)
            {
                #region SQL Command

                string strSql = "Delete From BarcodeTable Where BarcodeValue = '{0}' And CaseID = '{1}'";

                #endregion

                strSql = string.Format(strSql, BarcodeValue, CaseID);

                return strSql;
            }

            #endregion

            #region BkwpfileTable
            /// <summary>
            /// 刪除 BkwpfileTable
            /// </summary>
            /// <param name="WpinNo">收文文號</param>
            /// <returns></returns>
            public string BkwpfileTable(string WpinNo)
            {
                #region SQL Command

                string strSql = "Delete From BKWPFILE Where WPINNO = '{0}'";

                #endregion

                strSql = string.Format(strSql, WpinNo);

                return strSql;
            }
            #endregion

            #region RolePrivilege
            /// <summary>
            /// RolePrivilege
            /// </summary>
            /// <param name="FieldName">欄位名稱</param>
            /// <returns></returns>
            public string RolePrivilege(string FieldName)
            {
                #region SQL Command

                string strSql = "Delete From RolePrivilege Where {0} = :{0}\n";

                #endregion

                strSql = string.Format(strSql, FieldName);

                return strSql;
            }
            #endregion

            #region RoleTable

            public string RoleTable(string RoleID)
            {
                #region SQL Command

                string strSql = "Delete  RoleTable where RoleID ='{0}'";

                #endregion

                strSql = string.Format(strSql, RoleID);

                return strSql;
            }
            #endregion

            #region RolePrivilege
            /// <summary>
            /// RoleTable
            /// </summary>
            /// <param name="Data"></param>
            /// <returns></returns>
            public string RolePrivilege(int Role)
            {
                #region SQL Command

                string strSql = "Delete From RolePrivilege Where RoleID = {0}\n";

                #endregion

                strSql = string.Format(strSql, Role);

                return strSql;
            }
            #endregion

            #region ViewerPrivilege
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Data"></param>
            /// <returns></returns>
            public string ViewerPrivilege(int Role)
            {
                #region SQL Command

                string strSql = "Delete From ViewerRolePrivilegeTable Where RoleID = {0}\n";

                #endregion

                strSql = string.Format(strSql, Role);

                return strSql;
            }
            #endregion

            #region ViewerAreaRolePrivilegeTable
            /// <summary>
            /// ViewerAreaRolePrivilegeTable
            /// </summary>
            /// <param name="FieldName">欄位名稱</param>
            /// <returns></returns>
            public string ViewerAreaRolePrivilegeTable(string FieldName)
            {
                #region SQL Command

                string strSql = "Delete From ViewerAreaRolePrivilegeTable Where {0} = @{0}\n";

                #endregion

                strSql = string.Format(strSql, FieldName);

                return strSql;
            }
            #endregion

            #region ViewerRolePrivilegeTable
            /// <summary>
            /// ViewerRolePrivilegeTable
            /// </summary>
            /// <param name="FieldName">欄位名稱</param>
            /// <returns></returns>
            public string ViewerRolePrivilegeTable(string FieldName)
            {
                #region SQL Command

                string strSql = "Delete From ViewerRolePrivilegeTable Where {0} = :{0}\n";

                #endregion

                strSql = string.Format(strSql, FieldName);

                return strSql;
            }
            #endregion

            #region Wpborrow
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Data"></param>
            /// <returns></returns>
            public string Wpborrow(Hashtable Data)
            {
                #region SQL Command

                string strSql = "Delete WPBORROW Where wpinno = '{0}' And transt = (Select MAX(t.transt) From WPBORROW t Where t.wpinno = '{0}')";

                #endregion

                strSql = string.Format(strSql,
                    Data["WpinNo"].ToString().ToString());

                return strSql;
            }
            #endregion

            #region WptransDelete

            public string WptransDelete(string WPINNO)
            {
                #region SQL Command

                string strSql = @"Delete From WPTRANS Where WPINNO ='{0}'";

                #endregion

                strSql = string.Format(strSql, WPINNO);

                return strSql;
            }
            #endregion

            #region UploadTimeTable

            public string UploadTimeTable(string UserID)
            {
                #region SQL Command

                string strSql = @"Delete  UploadTimeTable where UserID ='{0}'";

                #endregion

                strSql = string.Format(strSql, UserID);

                return strSql;
            }
            #endregion

            #region TRANSTABLE
            /// <summary>
            /// 刪除 TRANSTABLE
            /// </summary>
            /// <param name="strWhere">收文文號</param>
            /// <returns></returns>
            public string TRANSTable(string strWhere)
            {
                #region SQL Command

                string strSql = " Delete From TRANSTABLE Where 1=1 {0} \n";

                #endregion

                strSql = string.Format(strSql, strWhere);

                return strSql;
            }
            #endregion


        }
        #endregion

        #region Insert
        /// <summary>
        /// Insert
        /// </summary>
        public class Insert
        {
            #region BackPiecesTable
            /// <summary>
            /// 退卷資料表
            /// </summary>
            /// <returns></returns>
            public string BackPiecesTable()
            {
                #region SQL Command

                string strSql = "Insert Into BackPiecesTable\n"
                    + "("
                    + "SystemCodeID,"
                    + "Comments,"
                    + "CaseID,"
                    + "BackPiecesStatus,"
                    + "UserID"
                    + ")\n"
                    + "Values"
                    + "("
                    + "@SystemCodeID,\n"
                    + "@Comments,\n"
                    + "@CaseID,\n"
                    + "@BackPiecesStatus,\n"
                    + "@UserID\n"
                    + ")";

                #endregion

                return strSql;
            }
            #endregion

            #region BarcodeTable
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Data"></param>
            /// <param name="Where"></param>
            /// <returns></returns>
            public string BarcodeTable(Hashtable Data, string Where)
            {
                #region SQL Command

                string strSql = "Update BarcodeTable Set\n"
                    + " WpoutNo = '{0}',\n"
                    + " FileNo = '{1}',\n"
                    + " FileDate = '{2}',\n"
                    + " KeepYr = '{3}',\n"
                    + " BoxNo = '{4}',\n"
                    + " OnFile = N'{5}',\n"
                    + " LastModifyUserID = '{6}',\n"
                    + " LastModifyTime = {7}\n"
                    + "Where 1=1 {8}\n";

                #endregion

                strSql = string.Format(strSql,
                    Data["WpoutNo"].ToString(),
                    Data["FileNo"].ToString(),
                    Data["FileDate"].ToString(),
                    Data["KeepYr"].ToString(),
                    Data["BoxNo"].ToString(),
                    Data["OnFile"].ToString(),
                    Data["LastModifyUserID"].ToString(),
                    Data["LastModifyTime"].ToString(),
                    Where);

                return strSql;
            }

            // Added by Luke
            public string BarcodeTable(Hashtable Data)
            {
                #region SQL Command

                string strSql = "INSERT INTO BarcodeTable\n"
                              + "(CaseID\n"
                              + ",BarcodeValue\n"
                              + ",CreateTime\n"
                              + ",CreateUserID\n"
                              + ",LastModifyTime\n"
                              + ",LastModifyUserID\n"
                              + ",WpoutNo\n"
                              + ",FileNo\n"
                              + ",FileDate\n"
                              + ",KeepYr\n"
                              + ",BoxNo\n"
                              + ",OnFile)\n"
                              + "VALUES\n"
                              + "('{0}'\n"
                              + ",'{1}'\n"
                              + ",TO_DATE('{2}','YYYY/MM/DD HH24:MI:SS')\n"
                              + ",'{3}'\n"
                              + ",TO_DATE('{4}','YYYY/MM/DD HH24:MI:SS')\n"
                              + ",'{5}'\n"
                              + ",null\n"
                              + ",null\n"
                              + ",null\n"
                              + ",null\n"
                              + ",null\n"
                              + ",null)";
                #endregion

                strSql = string.Format(strSql,
                   Data["CaseID"].ToString(),
                   Data["BarcodeValue"].ToString(),
                   Data["CreateTime"].ToString(),
                   Data["CreateUserID"].ToString(),
                   Data["LastModifyTime"].ToString(),
                   Data["LastModifyUserID"].ToString());

                return strSql;
            }

            #endregion

            #region CaseTable
            /// <summary>
            ///
            /// </summary>
            /// <returns></returns>
            public string CaseTable(Hashtable Data)
            {
                #region SQL Command

                string strSql = "INSERT INTO [dbo].[CaseTable]\n"
                              + "([RepNo]\n"
                              + ",[CaseStatus]\n"
                              + ",[JobItemID]\n"
                              + ",[UserID]\n"
                              + ",[CaseUnit]\n"
                              + ",[UserUnit]\n"
                              + ",[CreateTime]\n"
                              + ",[FileTableID]\n"
                              + ",[CasePlace]\n"
                              + ",[CaseJobItemID])\n"
                              + "VALUES\n"
                              + "('{0}'\n"
                              + ",{1}\n"
                              + ",{2}\n"
                              + ",{3}\n"
                              + ",{4}\n"
                              + ",{5}\n"
                              + ",GetDate()\n"
                              + ",{6}\n"
                              + ",{7}\n"
                              + ",{8})";
                #endregion

                strSql = string.Format(strSql,
                   Data["RepNo"].ToString(),
                   Data["CaseStatus"].ToString(),
                   Data["JobItemID"].ToString(),
                   Data["UserID"].ToString(),
                   Data["CaseUnit"].ToString(),
                   Data["UserUnit"].ToString(),
                   Data["FileTableID"].ToString(),
                   Data["CasePlace"].ToString(),
                   Data["CaseJobItemID"].ToString());

                return strSql;
            }
            #endregion

            #region FILEBORO
            /// <summary>
            /// FILEBORO
            /// </summary>
            /// <param name="SqlParamList"></param>
            /// <returns></returns>
            public string FILEBORO(Hashtable Data)
            {
                #region SQL Command

                string strSql = "Insert Into Fileboro Select wpb.wpinno,wpr.WPINDATE,wpr.wpoutno,wpr.WPOUTDATE,wpr.caseno,wpr.applkind\n"
                                + ",wpr.commname,wpb.receiver,wpb.transt,\n"
                                + "CASE wpb.VIEWTYPE\n"
                                + "WHEN N'1' THEN 'N'\n"
                                + "WHEN N'2' THEN 'Y'\n"
                                + "ELSE 'N' END,\n"
                                + "SYSDATE,'{1}',IMAGEPRIV\n"
                                + "From Wpborrow wpb\n"
                                + "Inner Join {2} wpr On wpb.wpinno = wpr.wpinno\n"
                                + "Where wpb.wpinno ='{0}' And not exists (Select wpinno from Fileboro Where CHK ='N' And wpb.wpinno = fileboro.wpinno )\n"
                                + "AND wpb.REDATE Is NULL AND PRTFLAG In('P','T') AND viewtype=1";
                #endregion

                strSql = string.Format(strSql,
               Data["WPINNO"].ToString(),
               Data["WORKERID"].ToString(),
                PageUtility.WprecSchema);

                return strSql;
            }
            #endregion

            #region FileboroByElec
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Data"></param>
            /// <returns></returns>
            public string FileboroByElec(Hashtable Data)
            {
                #region SQL Command

                string strSql = "Insert Into Fileboro Select wpb.wpinno,wpr.WPINDATE,wpr.wpoutno,wpr.WPOUTDATE,wpr.caseno,wpr.applkind\n"
                              + ",wpr.commname,wpb.receiver,wpb.transt, 'Y',\n"
                              + "SYSDATE,'電子調閱',IMAGEPRIV\n"
                              + "From Wpborrow wpb\n"
                              + "Inner Join BARCODETABLE bar On wpb.wpinno = bar.barcodevalue\n"
                              + "Inner Join {2} wpr On wpb.wpinno = wpr.wpinno\n"
                              + "Where wpb.wpinno ='{0}' And Transt = TO_DATE('{1}', 'YYYY/MM/DD HH24:MI:SS') And wpb.redate Is Null And wpb.Prtflag = 'N' And wpb.viewtype = '2'";

                #endregion

                strSql = string.Format(strSql,
                    Data["WPINNO"].ToString(),
                    Data["TRANST"].ToString(),
                    PageUtility.WprecSchema);

                return strSql;
            }
            #endregion

            #region RolePrivilege
            /// <summary>
            /// RolePrivilege
            /// </summary>
            /// <param name="SqlParamList"></param>
            /// <returns></returns>
            public string RolePrivilege(List<System.Data.IDataParameter> SqlParamList)
            {
                #region SQL Command

                string strSql = "Insert Into RolePrivilege\n"
                    + "("
                    + "RoleID,"
                    + "PrivID"
                    + ")\n"
                    + "Values\n"
                    + "("
                    + "{0}"
                    + ")";

                #endregion

                #region SqlParameter

                string strSqlParameter = string.Empty;
                for (int i = 0; i < SqlParamList.Count; i++)
                {
                    if (i == 0)
                        strSqlParameter += ":" + SqlParamList[i].ParameterName;
                    else
                        strSqlParameter += ",:" + SqlParamList[i].ParameterName;
                }

                #endregion

                strSql = string.Format(strSql, strSqlParameter);

                return strSql;
            }
            #endregion

            #region RoleTable
            /// <summary>
            /// RoleTable
            /// </summary>
            /// <param name="SqlParamList"></param>
            /// <returns></returns>
            public string RoleTable(List<System.Data.IDataParameter> SqlParamList)
            {
                #region SQL Command

                string strSql = "Insert Into RoleTable\n"
                    + "("
                    + "RoleID,"
                    + "RoleName,"
                    + "Comments"
                    + ")\n"
                    + "Values\n"
                    + "("
                    + "{0}"
                    + ")";

                #endregion

                #region SqlParameter

                string strSqlParameter = string.Empty;
                for (int i = 0; i < SqlParamList.Count; i++)
                {
                    if (i == 0)
                        strSqlParameter += ":" + SqlParamList[i].ParameterName;
                    else
                        strSqlParameter += ",:" + SqlParamList[i].ParameterName;
                }

                #endregion

                strSql = string.Format(strSql, strSqlParameter);

                return strSql;
            }
            #endregion

            #region SystemCodeTable
            /// <summary>
            /// 系統代碼資料表
            /// </summary>
            /// <returns></returns>
            public string SystemCodeTable()
            {
                #region SQL Command

                string strSql = "Insert Into SystemCodeTable\n"
                    + "("
                    + "ParentID,"
                    + "LevelID,"
                    + "SystemCodeName,"
                    + "SystemCode,"
                    + "SystemCodeStatus"
                    + ")\n"
                    + "Values"
                    + "("
                    + "@ParentID,\n"
                    + "1,\n"
                    + "@SystemCodeName,\n"
                    + "@SystemCode,\n"
                    + "@SystemCodeStatus\n"
                    + ")";

                #endregion

                return strSql;
            }
            #endregion

            #region IndexClassTable
            /// <summary>
            /// UserTable
            /// </summary>
            /// <param name="SqlParamList"></param>
            /// <returns></returns>
            public string IndexClassTable(Hashtable Data)
            {
                #region SQL Command

                string strSql = "Insert Into IndexClassTable\n"
                    + "("
                    + "IndexClassCode,"
                    + "IndexClassName,"
                    + "Attrib,"
                    + "BarcodePreFixedName,"
                    + "CheckRuleID,"
                    + "CheckType"
                    + ")\n"
                    + "Values\n"
                    + "("
                    + "'{0}',"
                    + "'{1}'\n,"
                    + "{2},"
                    + "'{3}',"
                    + "{4},"
                   + "{5}"
                    + ")";

                #endregion

                strSql = string.Format(strSql,
               Data["IndexClassCode"].ToString(),
               Data["IndexClassName"].ToString(),
               Data["Attrib"].ToString(),
               Data["BarcodePreFixedName"].ToString(),
               Data["CheckRuleID"].ToString(),
               Data["CheckType"].ToString());

                return strSql;
            }
            #endregion

            #region IndexTreeTable
            /// <summary>
            /// UserTable
            /// </summary>
            /// <param name="SqlParamList"></param>
            /// <returns></returns>
            public string IndexTreeTable(Hashtable Data)
            {
                #region SQL Command

                string strSql = "INSERT INTO [dbo].[IndexTreeTable]\n"
                                + "([CaseID]\n"
                                + ",[FileID]\n"
                                + ",[LapID]\n"
                                + ",[IndexClassID]\n"
                                + ",[IndexText]\n"
                                + ",[CreateUserID]\n"
                                + ",[CreateTime])\n"
                                + "VALUES\n"
                                + "({0}\n"
                                + ",{1}\n"
                                + ",{2}\n"
                                + ",{3}\n"
                                + ",'{4}'\n"
                                + ",{5}\n"
                                + ",GetDate())";

                #endregion

                strSql = string.Format(strSql,
               Data["CaseID"].ToString(),
               Data["FileID"].ToString(),
               Data["LapID"].ToString(),
               Data["IndexClassID"].ToString(),
               Data["IndexText"].ToString(),
               Data["CreateUserID"].ToString());

                return strSql;
            }
            #endregion

            #region UserTable
            /// <summary>
            /// UserTable
            /// </summary>
            /// <param name="SqlParamList"></param>
            /// <returns></returns>
            public string UserTable(Hashtable Data)
            {
                #region SQL Command

                string strSql = "Insert Into UserTable\n"
                    + "("
                    + "UserID,"
                    + "UserName,"
                    + "Password,"
                    + "RealName,"
                    + "CreateUserID,"
                    + "RoleID,"
                    + "UserStatus,"
                    + "CreateTime,"
                    + "TEL"
                    + ")\n"
                    + "Values\n"
                    + "("
                    + "'{0}',"
                    + "'{1}',"
                    + "N'{2}'\n,"
                    + "'{3}',"
                    + "{4},"
                    + "{5},"
                    + "0,"
                    + "SYSDATE,"
                    + "{6}"
                    + ")";

                #endregion

                strSql = string.Format(strSql,
               Data["UserID"].ToString(),
               Data["UserName"].ToString(),
               Data["Password"].ToString(),
               Data["RealName"].ToString(),
               Data["CreateUserID"].ToString(),
               Data["RoleID"].ToString(),
               Data["TEL"].ToString());

                return strSql;
            }
            #endregion

            #region UploadTimeTable
            /// <summary>
            /// UploadTimeTable
            /// </summary>
            /// <param name="Data"></param>
            /// <returns></returns>
            public string UploadTimeTable(Hashtable Data)
            {
                #region SQL Command

                string strSql = "Insert Into UploadTimeTable (\n"
                    + "UserID\n"
                    + ",UploadTH\n"
                    + ",UploadTM\n"
                    + ",Comments\n"
                    + ")\n"
                    + "Values\n"
                    + "(\n"
                    + "	'{0}'\n"
                    + "	,{1}\n"
                    + "	,{2}\n"
                    + ",@Comments\n"
                    + ")\n";

                #endregion

                strSql = string.Format(strSql
                    , Data["UserID"].ToString()
                    , Data["UploadTH"].ToString()
                    , Data["UploadTM"].ToString()
                );

                return strSql;
            }
            #endregion

            #region ViewerRolePrivilegeTable
            /// <summary>
            /// ViewerRolePrivilegeTable
            /// </summary>
            /// <param name="SqlParamList"></param>
            /// <returns></returns>
            public string ViewerRolePrivilegeTable(List<System.Data.IDataParameter> SqlParamList)
            {
                #region SQL Command

                string strSql = "Insert Into ViewerRolePrivilegeTable\n"
                    + "("
                    + "RoleID,"
                    + "ViewerPrivID"
                    + ")\n"
                    + "Values\n"
                    + "("
                    + "{0}"
                    + ")";

                #endregion

                #region SqlParameter

                string strSqlParameter = string.Empty;
                for (int i = 0; i < SqlParamList.Count; i++)
                {
                    if (i == 0)
                        strSqlParameter += ":" + SqlParamList[i].ParameterName;
                    else
                        strSqlParameter += ",:" + SqlParamList[i].ParameterName;
                }

                #endregion

                strSql = string.Format(strSql, strSqlParameter);

                return strSql;
            }
            #endregion

            #region Wpborrow
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Data"></param>
            /// <returns></returns>
            public string Wpborrow(Hashtable Data)
            {
                #region SQL Command

                string strSql = "Insert Into Wpborrow (WPINNO, TRANST, RECEIVER, PRTFLAG, WPOUTNO, REDATE, KIND, VIEWTYPE, EXTEN, HURRYDATE, HURRYTIME, USERID, APPROVEUSERID,IMAGEPRIV,REASON)\n"
                              + "Values('{0}', {1}, '{2}', 'N', '{3}', NULL, '{4}', '{5}', 'N', NULL, 0, NULL, '{6}','{7}','{8}')";

                #endregion

                strSql = string.Format(strSql,
                    Data["WpinNo"].ToString(),
                    Data["Transt"].ToString(),
                    Data["Receiver"].ToString(),
                    Data["WpoutNo"].ToString(),
                    Data["Kind"].ToString(),
                    Data["ViewType"].ToString(),
                    Data["ApproveUserID"].ToString(),
                    Data["IMAGEPRIV"].ToString(),
                    Data["REASON"].ToString());

                return strSql;
            }
            #endregion

            #region Wptrans
            /// <summary>
            /// Wptrans Table
            /// </summary>
            /// <param name="Data"></param>
            /// <returns></returns>
            public string Wptrans(Hashtable Data)
            {
                #region SQL Command

                string strSql = "Insert Into Wptrans (WPINNO, TRANST, RECEIVER, PRTFLAG, MARKER)\n"
                              + "Values('{0}', '{1}', N'{2}', 'F', 'R')";

                #endregion

                strSql = string.Format(strSql,
                    Data["BarcodeValue"].ToString(),
                    Data["FileDate"].ToString(),
                    Data["OnFile"].ToString());

                return strSql;
            }
            #endregion

            #region Transtable
            // Added by Luke 2016/05/30
            /// <summary>
            /// Transtable
            /// </summary>
            /// <param name="Data"></param>
            /// <returns></returns>
            public string Transtable(Hashtable Data)
            {
                #region SQL Command

                //MODIFY BY RICHARD20160705 因TRANSDATE改為系統時間故加上N''
                string strSql = "Insert Into TRANSTABLE (WPINNO, TRANSTIME, RECEIVER, FLAG, COMMNAME, WPKIND, WPTYPE)\n"
                              + "Values('{0}', N'{1}', N'{2}', '{3}',N'{4}','{5}','{6}') \n";

                #endregion

                strSql = string.Format(strSql,
                    Data["Wpinno"].ToString(),
                    Data["Transtime"].ToString(),
                    Data["Receiver"].ToString(),
                    Data["Flag"].ToString(),
                    Data["COMMNAME"].ToString(),
                    Data["WPKIND"].ToString(),
                    Data["WPTYPE"].ToString());

                return strSql;
            }
            #endregion

        }

        #endregion
    }
}