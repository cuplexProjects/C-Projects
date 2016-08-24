using System;
using System.Web.UI;
using System.Globalization;
using WebMail;

public partial class PlugIns_WebMailLite_main : AdminPanelMainPlugin
{
	public enum MailAdmScreen
	{
		Login = 0,
		DatabaseSettings = 1,
		WebMailSettings = 2,
		InterfaceSettings = 3,
		LoginSettings = 4,
		DebugSettings = 5,
		MailServer = 6
	}

    protected PlugIns_WebMailLite_main()
    {
        SetPluginID(AdminPanelConstants.PluginName.WebMailLite);
    }

    public override bool CanLoadPlugin()
    {
        return true;
    }

    public override void InitPlugin()
    {
        (Page as DefaultPage).HelpUrl = "http://www.afterlogic.com/wiki/WebMail_Lite_5_.NET_documentation";
    }

    protected void Page_Load(object sender, EventArgs e)
	{
        string mode = !string.IsNullOrEmpty(Request.QueryString["mode"]) ? Request.QueryString["mode"] : "webmail";

        switch (mode)
        {
            case "db":
                MenuID.Screen = (int)MailAdmScreen.DatabaseSettings;
                database_settingsLite dbSettingsID = (database_settingsLite)LoadControl(@"Controls\database.ascx");
                if (dbSettingsID != null)
                {
                    dbSettingsID.ID = "dbSettingsID";
                    ContentPlaceHolder.Controls.Add(dbSettingsID);
                }
                break;
            case "webmail":
                MenuID.Screen = (int)MailAdmScreen.WebMailSettings;
                webmail_settingsLite wmSettingsID = (webmail_settingsLite)LoadControl(@"Controls\webmail.ascx");
                if (wmSettingsID != null)
                {
                    wmSettingsID.ID = "wmSettingsID";
                    ContentPlaceHolder.Controls.Add(wmSettingsID);
                }
                break;
            case "interface":
                MenuID.Screen = (int)MailAdmScreen.InterfaceSettings;
                interface_settingsLite ifcSettingsID = (interface_settingsLite)LoadControl(@"Controls\interface.ascx");
                if (ifcSettingsID != null)
                {
                    ifcSettingsID.ID = "ifcSettingsID";
                    ContentPlaceHolder.Controls.Add(ifcSettingsID);
                }
                break;
            case "login":
                MenuID.Screen = (int)MailAdmScreen.LoginSettings;
                login_settingsLite lgnSettingsID = (login_settingsLite)LoadControl(@"Controls\login.ascx");
                if (lgnSettingsID != null)
                {
                    lgnSettingsID.ID = "lgnSettingsID";
                    ContentPlaceHolder.Controls.Add(lgnSettingsID);
                }
                break;
            case "debug":
                MenuID.Screen = (int)MailAdmScreen.DebugSettings;
                debug_settingsLite dbgSettingsID = (debug_settingsLite)LoadControl(@"Controls\debug.ascx");
                if (dbgSettingsID != null)
                {
                    dbgSettingsID.ID = "dbgSettingsID";
                    ContentPlaceHolder.Controls.Add(dbgSettingsID);
                }
                break;
            case "integration":
                MenuID.Screen = (int)MailAdmScreen.MailServer;
                server_integrationLite inegrSettingsID = (server_integrationLite)LoadControl(@"Controls\server-integration.ascx");
                if (inegrSettingsID != null)
                {
                    inegrSettingsID.ID = "inegrSettingsID";
                    ContentPlaceHolder.Controls.Add(inegrSettingsID);
                }
                break;
        }
	}
}
