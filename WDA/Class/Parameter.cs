using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WDA.Class
{
    public class Parameter
    {
        #region WebConfig
        /// <summary>
        /// Web Config Setting
        /// </summary>
        public class WebConfig
        {
            #region AD_URL
            /// <summary>
            /// SSO URL
            /// </summary>
            public static string AD_URL
            {
                get
                {
                    if (ConfigurationManager.AppSettings["AD_URL"] == null) throw new Exception("Web.Config 參數缺少 (AD_URL)");

                    return ConfigurationManager.AppSettings["AD_URL"].Trim();
                }
            }
            #endregion

            #region CheckLogin_Integrate
            /// <summary>
            /// 是否開啟介接登入驗證 0 = Close, 1 = Open
            /// </summary>
            public static bool CheckLogin_Integrate
            {
                get
                {
                    if (ConfigurationManager.AppSettings["CheckLogin_Integrate"] == null) return false;

                    return ConfigurationManager.AppSettings["CheckLogin_Integrate"].Trim() == "1";
                }
            }
            #endregion

            #region DBEncryption
            /// <summary>
            /// 
            /// </summary>
            public static string DBEncryption
            {
                get
                {
                    if (ConfigurationManager.AppSettings["DBEncryption"] == null) throw new Exception("Web.Config 參數缺少 (DBEncryption)"); ;

                    return ConfigurationManager.AppSettings["DBEncryption"].Trim();
                }
            }
            #endregion
        }
        #endregion
    }
}