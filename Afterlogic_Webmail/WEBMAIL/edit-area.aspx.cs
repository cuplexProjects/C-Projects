using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace WebMail
{
	public partial class edit_area : System.Web.UI.Page
	{
		protected string _rtl = "";

		protected void Page_Load(object sender, EventArgs e)
		{
			if (Request.QueryString.Get("rtl") != null)
			{
				_rtl = (Request.QueryString.Get("rtl") == "1") ? " dir=\"rtl\"" : "";
			}
		}
	}
}
