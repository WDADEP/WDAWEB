using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WDA.Class;

namespace WDA
{
    public class LiteralMessageBox:System.Web.UI.WebControls.Literal
    {
        public LiteralMessageBox()
            : base()
        {
            this.Text = PageUtility.MessageBox();
        }
    }
}