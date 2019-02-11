using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CuplexLib;
using System.Web.Security;

public partial class _default : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Verify user session
        if (Session["User"] == null && Request.Cookies["UserId"] != null && Request.Cookies["CuplexAuthCookie"] != null)
        {
            string authenticationHash = Request.Cookies["CuplexAuthCookie"].Value;
            string userName = Request.Cookies["UserId"].Value;
            if (AccessControl.AuthenticateLoginTicket(userName, authenticationHash))
            {

                User usr = User.GetOneByUserName(userName);
                if (usr != null)
                {                    
                    usr.LastLogin = DateTime.Now;
                    usr.save();
                    Session["User"] = usr;
                }
            }
            else
            {
                Response.Cookies.Remove("UserId");
                Response.Cookies.Remove("CuplexAuthCookie");
            }
        }

        Settings s = Settings.GetOneFromCache("PollHeaderText");
        if (!string.IsNullOrEmpty(s.Value))
            WebbPollCaption.Text = s.Value;
        else
            WebbPollCaption.Text = "Webbfråga";

        Settings showBanners = Settings.GetOneFromCache("ShowBanners");
        if (showBanners != null)
            BannerControl1.Visible = showBanners.Value == "1";
    }
    protected override void OnPreRender(EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "MasterpageScripts", "function goHome(){document.location='" + cms.Current.GetRootPath + "';}", true);
        base.OnPreRender(e);
    }
    protected void ModalOkButton_Clicked(object sender, EventArgs e)
    {
        ModalBackgroundPanel.Visible = false;
        ModalWindow.Visible = false;
    }
}
