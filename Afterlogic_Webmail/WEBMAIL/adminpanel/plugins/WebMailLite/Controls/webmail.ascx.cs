using System.Globalization;
using System;
using WebMail;

public partial class webmail_settingsLite : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, System.EventArgs e)
	{
        if (!Page.IsPostBack)
		{
            InitData();
        }
	}

    protected void InitData()
    {
        WebmailSettings settings = new WebMailSettingsCreator().CreateWebMailSettings(AdminPanelUtils.GetWebMailDataFolder());

        txtSiteName.Value = settings.SiteName;
        txtIncomingMail.Value = settings.IncomingMailServer;
        intIncomingMailPort.Value = settings.IncomingMailPort.ToString(CultureInfo.InvariantCulture);

        if (settings.IncomingMailProtocol == IncomingMailProtocol.Pop3)
        {
            intIncomingMailProtocol.SelectedIndex = 0;
        }
        else if (settings.IncomingMailProtocol == IncomingMailProtocol.Imap4)
        {
            intIncomingMailProtocol.SelectedIndex = 1;
        }
        else if (settings.IncomingMailProtocol == IncomingMailProtocol.WMServer)
        {
            intIncomingMailProtocol.SelectedIndex = 2;
        }

        txtOutgoingMail.Value = settings.OutgoingMailServer;
        intOutgoingMailPort.Value = settings.OutgoingMailPort.ToString(CultureInfo.InvariantCulture);

        intReqSmtpAuthentication.Checked = settings.ReqSmtpAuth;
        intAllowDirectMode.Checked = settings.AllowDirectMode;
        intDirectModeIsDefault.Checked = settings.DirectModeIsDefault;

        intAttachmentSizeLimit.Value = Math.Round((double)(settings.AttachmentSizeLimit / 1024)).ToString(CultureInfo.InvariantCulture);
        intEnableAttachmentSizeLimit.Checked = settings.EnableAttachmentSizeLimit;
        intMailboxSizeLimit.Value = Math.Round((double)(settings.MailboxSizeLimit / 1024)).ToString(CultureInfo.InvariantCulture);
        intEnableMailboxSizeLimit.Checked = settings.EnableMailboxSizeLimit;

        intAllowUsersChangeEmailSettings.Checked = settings.AllowUsersChangeEmailSettings;
        intAllowUsersAddNewAccounts.Checked = settings.AllowUsersAddNewAccounts;
        intAllowUsersChangeAccountsDef.Checked = settings.AllowUsersChangeAccountsDef;

        txtDefaultUserCharset.Value = settings.DefaultUserCharset.ToString();
        intAllowUsersChangeCharset.Checked = settings.AllowUsersChangeCharset;

        if (settings.DefaultTimeZone < txtDefaultTimeZone.Items.Count)
        {
            txtDefaultTimeZone.SelectedIndex = settings.DefaultTimeZone;
        }

        intAllowUsersChangeTimeOffset.Checked = settings.AllowUsersChangeTimeZone;
        intTakeImapQuota.Checked = settings.TakeImapQuota;
    }

	#region Web Form Designer generated code
	override protected void OnInit(EventArgs e)
	{
		//
		// CODEGEN: This call is required by the ASP.NET Web Form Designer.
		//
		InitializeComponent();
		base.OnInit(e);
	}

	/// <summary>
	///		Required method for Designer support - do not modify
	///		the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent()
	{

	}
	#endregion

	protected void SaveButton_Click(object sender, System.EventArgs e)
	{
		try
		{
            WebmailSettings settings = new WebMailSettingsCreator().CreateWebMailSettings(AdminPanelUtils.GetWebMailDataFolder());

            settings.SiteName = txtSiteName.Value;

            settings.IncomingMailServer = txtIncomingMail.Value;
            try
            {
                settings.IncomingMailPort = int.Parse(intIncomingMailPort.Value, CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                AdminPanelUtils.SetPageReportMessage(this.Page, ex.Message);
            }

            switch (intIncomingMailProtocol.Value.ToLower(CultureInfo.InvariantCulture))
            {
                case "pop3":
                    settings.IncomingMailProtocol = IncomingMailProtocol.Pop3;
                    break;
                case "imap4":
                    settings.IncomingMailProtocol = IncomingMailProtocol.Imap4;
                    break;
                case "xmail":
                    settings.IncomingMailProtocol = IncomingMailProtocol.WMServer;
                    break;
            }

            settings.OutgoingMailServer = txtOutgoingMail.Value;
            try
            {
                settings.OutgoingMailPort = int.Parse(intOutgoingMailPort.Value, CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                AdminPanelUtils.SetPageReportMessage(this.Page, ex.Message);
            }

            settings.ReqSmtpAuth = intReqSmtpAuthentication.Checked;
            settings.AllowDirectMode = intAllowDirectMode.Checked;
            settings.DirectModeIsDefault = intDirectModeIsDefault.Checked;

            try
            {
                settings.AttachmentSizeLimit = long.Parse(intAttachmentSizeLimit.Value, CultureInfo.InvariantCulture) * 1024;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                AdminPanelUtils.SetPageReportMessage(this.Page, ex.Message);
            }
            settings.EnableAttachmentSizeLimit = intEnableAttachmentSizeLimit.Checked;
            try
            {
                settings.MailboxSizeLimit = long.Parse(intMailboxSizeLimit.Value, CultureInfo.InvariantCulture) * 1024;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                AdminPanelUtils.SetPageReportMessage(this.Page, ex.Message);
            }
            settings.EnableMailboxSizeLimit = intEnableMailboxSizeLimit.Checked;

            settings.AllowUsersChangeEmailSettings = intAllowUsersChangeEmailSettings.Checked;
            settings.AllowUsersAddNewAccounts = intAllowUsersAddNewAccounts.Checked;
            settings.AllowUsersChangeAccountsDef = intAllowUsersChangeAccountsDef.Checked;

            try
            {
                settings.DefaultUserCharset = int.Parse(txtDefaultUserCharset.Value, CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                AdminPanelUtils.SetPageReportMessage(this.Page, ex.Message);
            }
            settings.AllowUsersChangeCharset = intAllowUsersChangeCharset.Checked;

            settings.DefaultTimeZone = (short)txtDefaultTimeZone.SelectedIndex;
            settings.AllowUsersChangeTimeZone = intAllowUsersChangeTimeOffset.Checked;
            settings.TakeImapQuota = intTakeImapQuota.Checked;

            settings.SaveWebmailSettings(AdminPanelUtils.GetWebMailDataFolder());
            this.Context.Application.Remove(Constants.sessionSettings);

            AdminPanelUtils.SetPageReportMessage(this.Page, Constants.mailAdmSaveSuccess);
		}
		catch (Exception error)
		{
            Log.WriteException(error);
            AdminPanelUtils.SetPageErrorMessage(this.Page, Constants.mailAdmSaveUnsuccess);
		}
	}
}
