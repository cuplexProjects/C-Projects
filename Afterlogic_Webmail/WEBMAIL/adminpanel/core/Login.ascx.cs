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

namespace WebMail.adminpanel
{
    public partial class Login : System.Web.UI.UserControl
    {
        protected string _errorMessage = string.Empty;
        protected string _errorClass = "wm_hide";

        protected void Page_Load(object sender, EventArgs e)
        {
            (Page as DefaultPage).Body = " id=\"mbody\" style=\"background-color: #e5e5e5\" onload=\"var login=document.getElementById('" + LoginID.ClientID + "'); if (login) {login.focus();};\"";
            (Page as DefaultPage).PageTitle = "Login";
        }

        protected void Enter_OnClick(object sender, EventArgs e)
        {
            AdminPanelSettings apSettings = (Page as DefaultPage).Settings;

            if ((apSettings.User != LoginID.Value) || (apSettings.Pass != PasswordID.Value))
            {
                try
                {
                    Subadmin adm = Subadmin.GetSubadmin(LoginID.Value);
                    if (adm != null)
                    {
                        if (PasswordID.Value != adm.Password)
                        {
                            _errorMessage = "Wrong login and/or password. Authentication failed.";
                            _errorClass = "wm_login_error";
                            return;
                        }
                        else
                        {
                            AdminPanelUtils.SaveState("AUTH", true, Page.Session);
                            AdminPanelUtils.SaveState("Admin", LoginID.Value, Page.Session);
                            AdminPanelUtils.SaveState("AdminID", adm.ID, Page.Session);
                            Response.Redirect("default.aspx", false);
                        }
                    }
                }
                catch { }
                _errorMessage = "Wrong login and/or password. Authentication failed.";
                _errorClass = "wm_login_error";
            }
            else
            {
                AdminPanelUtils.SaveState("AUTH", true, Page.Session);
                AdminPanelUtils.SaveState("Admin", LoginID.Value, Page.Session);
                Response.Redirect("default.aspx", false);
            }
        }
    }
}