using System;
using System.IO;
using WebMail;


public partial class server_integrationLite : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            WebmailSettings settings = new WebmailSettings().CreateInstance(AdminPanelUtils.GetWebMailDataFolder());
            txtWmServerRootPath.Value = settings.WmServerRootPath;
            txtWmServerHostName.Value = settings.WmServerHost;
            intWmAllowManageXMailAccounts.Checked = settings.WmAllowManageXMailAccounts;
        }
    }
    
    protected void save_ServerClick(object sender, EventArgs e)
    {
        try
        {
            Context.Application.Remove(Constants.sessionSettings);
            WebmailSettings settings = new WebmailSettings().CreateInstance(AdminPanelUtils.GetWebMailDataFolder());

            settings.WmServerRootPath = txtWmServerRootPath.Value;
            settings.WmServerHost = txtWmServerHostName.Value;
            settings.WmAllowManageXMailAccounts = intWmAllowManageXMailAccounts.Checked;

            if (!settings.EnableWmServer)
            {
                settings.SaveWebmailSettings(AdminPanelUtils.GetWebMailDataFolder());
                AdminPanelUtils.SetPageReportMessage(this.Page, Constants.mailAdmSaveSuccess);
            }
            else if (txtWmServerRootPath.Value.Length > 0)
            {
                string fullPath = Path.Combine(txtWmServerRootPath.Value, "domains");
                if (!Directory.Exists(fullPath))
                {
                    throw new WebMailIOException(string.Format(@"Server Root Path '{0}' incorrect.", AdminPanelUtils.EncodeHtml(fullPath)));
                }

                Utils.SettingsPath = AdminPanelUtils.GetWebMailFolder();
                WMServerStorage storage = new WMServerStorage(txtWmServerHostName.Value, null);
                storage.GetDomainList(); // test command

                settings.SaveWebmailSettings(AdminPanelUtils.GetWebMailDataFolder());
                this.Context.Application.Remove(Constants.sessionSettings);

                AdminPanelUtils.SetPageReportMessage(this.Page, Constants.mailAdmSaveSuccess);
            }
            else
            {
                throw new WebMailIOException(@"Server Root Path not set.");
            }

        }
        catch (Exception ex)
        {
            Log.WriteException(ex);
            AdminPanelUtils.SetPageErrorMessage(this.Page, Constants.mailAdmSaveUnsuccess + "<br /> Error:" + ex.Message);
        }
    }
}
