using System.Globalization;
using System;
using WebMail;

/// <summary>
///		Summary description for mailadm_interface_settings.
/// </summary>
public partial class interface_settingsLite : System.Web.UI.UserControl
{
	protected System.Web.UI.WebControls.RangeValidator validatorMailsPerPage;

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

        intMailsPerPage.Value = settings.MailsPerPage.ToString(CultureInfo.InvariantCulture);

        string[] supportedSkins = AdminPanelUtils.GetSupportedSkins(AdminPanelUtils.GetWebMailFolder());
        for (int i = 0; i < supportedSkins.Length; i++)
        {
            txtDefaultSkin.Items.Add(supportedSkins[i]);
            if (string.Compare(supportedSkins[i], settings.DefaultSkin, true, CultureInfo.InvariantCulture) == 0)
                txtDefaultSkin.SelectedIndex = i;
        }

        string[] supportedLangs = AdminPanelUtils.GetSupportedLangs(AdminPanelUtils.GetWebMailDataFolder());
        for (int i = 0; i < supportedLangs.Length; i++)
        {
            txtDefaultLanguage.Items.Add(supportedLangs[i]);
            if (string.Compare(supportedLangs[i], settings.DefaultLanguage, true, CultureInfo.InvariantCulture) == 0)
                txtDefaultLanguage.SelectedIndex = i;
        }

        intAllowUsersChangeSkin.Checked = settings.AllowUsersChangeSkin;
        intAllowUsersChangeLanguage.Checked = settings.AllowUsersChangeLanguage;
        intShowTextLabels.Checked = settings.ShowTextLabels;
        intAllowDHTMLEditor.Checked = settings.AllowDhtmlEditor;
        intAllowContacts.Checked = settings.AllowContacts;
        intAllowCalendar.Checked = settings.AllowCalendar;

        intRightMessagePane.Checked = ((settings.ViewMode & ViewMode.WithPreviewPane) > 0) ? true : false;
        intAlwaysShowPictures.Checked = ((settings.ViewMode & ViewMode.AlwaysShowPictures) > 0) ? true : false;
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

	protected void SubmitButton_Click(object sender, System.EventArgs e)
	{
		try
		{
            WebmailSettings settings = new WebMailSettingsCreator().CreateWebMailSettings(AdminPanelUtils.GetWebMailDataFolder());

            try
            {
                settings.MailsPerPage = short.Parse(intMailsPerPage.Value, CultureInfo.InvariantCulture);
                if (settings.MailsPerPage < 0)
                {
                    settings.MailsPerPage = Math.Abs(settings.MailsPerPage);
                }
            }
            catch (Exception error)
            {
                Log.WriteException(error);
            }

            settings.DefaultSkin = txtDefaultSkin.Value;
            settings.DefaultLanguage = txtDefaultLanguage.Value;
            settings.AllowUsersChangeSkin = intAllowUsersChangeSkin.Checked;
            settings.AllowUsersChangeLanguage = intAllowUsersChangeLanguage.Checked;
            settings.ShowTextLabels = intShowTextLabels.Checked;
            settings.AllowDhtmlEditor = intAllowDHTMLEditor.Checked;
            settings.AllowContacts = intAllowContacts.Checked;
            settings.AllowCalendar = intAllowCalendar.Checked;

            ViewMode mode = ViewMode.WithoutPreviewPane;
            if (intRightMessagePane.Checked) mode |= ViewMode.WithPreviewPane;
            if (intAlwaysShowPictures.Checked) mode |= ViewMode.AlwaysShowPictures;
            settings.ViewMode = mode;

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
