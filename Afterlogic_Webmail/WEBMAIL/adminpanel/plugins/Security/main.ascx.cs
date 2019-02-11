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
using System.Globalization;

public partial class PlugIns_Security_main : AdminPanelMainPlugin
{
    public enum CommonScreen
    {
        Auth = 1
    }

    protected PlugIns_Security_main()
    {
        SetPluginID(AdminPanelConstants.PluginName.Security);
    }

    public override bool CanLoadPlugin()
    {
        bool result = false;
        if (Plugin.IsPluginExist(MapPath("../../"), AdminPanelConstants.PluginName.WebMailLite))
        {
            result = true;
        }
        else
        {
            result = AdminPanelUtils.ValidationLicenseKey((Page as DefaultPage).Settings.LicenseKey);
        }
        return (result && AdminPanelUtils.IsSuperAdmin(Session, (Page as DefaultPage).Settings));
    }

    protected CommonScreen _selectedScreen = CommonScreen.Auth;

    protected void Page_Load(object sender, EventArgs e)
    {
        string mode = !string.IsNullOrEmpty(Request.QueryString["mode"]) ? Request.QueryString["mode"] : "auth";
        if (mode == "auth")
        {
            _selectedScreen = CommonScreen.Auth;
            Control ctrl = LoadControl(@"security-settings.ascx");
            if (ctrl != null) ContentPlaceHolder.Controls.Add(ctrl);
        }
    }
}
