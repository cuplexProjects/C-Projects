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

public partial class License : System.Web.UI.UserControl
{
    protected int _web_step = 1;
    protected int _max_step = 7;

    protected void Page_Load(object sender, EventArgs e)
    {
        install _page = Page as install;
        _web_step = _page._web_step;
        _max_step = _page._max_step;

        if (Page.IsPostBack)
        {
            
            Response.Redirect("install.aspx?mode=db", true);
        }
    }
}
