using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CuplexLib;

public partial class UserControls_LoginControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] is CuplexLib.User)
        {
            NotLoggedInMainWrapper.Visible = false;
            LoggedInMainWrapper.Visible = true;

            CuplexLib.User user = (CuplexLib.User)Session["User"];
            loggedInUserBox.InnerHtml = "<span>Välkommen till Cuplex</span><span>" + user.UserName + "</span>";
        }
        else
        {
            NotLoggedInMainWrapper.Visible = true;
            LoggedInMainWrapper.Visible = false;

            string script = "document.getElementById('" + this.PasswordTextBox.ClientID + "').value = '123456789';";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LoginScript", script, true);
        }

        createUserLink.HRef = cms.Current.GetRootPath + "user/createuser";
    }
    protected override void OnPreRender(EventArgs e)
    {
        CuplexLib.User user = Session["User"] as CuplexLib.User;
        logoutLink.HRef = cms.Current.GetRootPath + "logout";
        accountSettings.HRef = cms.Current.GetRootPath + "user/settings";
        suggestionLink.HRef = cms.Current.GetRootPath + "link";
        adminLink.HRef = cms.Current.GetRootPath + "admin";
        homeLink.HRef = cms.Current.GetRootPath;

        if (user != null && user.IsAdmin)
            AdminLinkPlaceholder.Visible = true;

        base.OnPreRender(e);
    }

    protected void LoginButton_Clicked(object sender, EventArgs e)
    {
        User.UserAuthenticateResponce authenticationResponce = User.AuthenticateUser(UserNameTextBox.Text, PasswordTextBox.Text);
        if (authenticationResponce == User.UserAuthenticateResponce.UnknownUser)
        {
            Session["LastError"] = "Användaren hittades inte.";
            Response.Redirect(cms.Current.GetRootPath + "user/createuser");
        }
        else if (authenticationResponce == User.UserAuthenticateResponce.PasswordIncorrect)
        {
            Session["LastError"] = "Felaktigt lösenord.";
            Response.Redirect(cms.Current.GetRootPath + "user/login");
        }
        else
        {
            AccessControl.CreateUserSession(UserNameTextBox.Text);
            Response.Redirect(cms.Current.GetRootPath);
        }
    }
}
