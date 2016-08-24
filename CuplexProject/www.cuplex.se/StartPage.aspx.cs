using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

public partial class StartPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["action"] == "logout")
        {
            AccessControl.LogoutUser();
            Response.Redirect(cms.Current.GetRootPath);
            return;
        }

        ContentListBlockControl.ShowPageLinks = true;
    }   
}