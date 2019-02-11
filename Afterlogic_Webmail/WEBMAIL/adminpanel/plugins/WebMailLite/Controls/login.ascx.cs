using System;
using WebMail;

/// <summary>
///		Summary description for mailadm_login_settings.
/// </summary>
public partial class login_settingsLite : System.Web.UI.UserControl
{

    public string clientPrefix;

    protected void Page_Load(object sender, System.EventArgs e)
	{
        clientPrefix = ClientID;

        if (!Page.IsPostBack)
		{
            InitData();
		}
	}

    protected void InitData()
    {
        WebmailSettings settings = new WebMailSettingsCreator().CreateWebMailSettings(AdminPanelUtils.GetWebMailDataFolder());

        switch (settings.HideLoginMode)
        {
            case LoginMode.Default:
                standartLoginRadio.Checked = true;
                break;
            case LoginMode.HideLoginFieldLoginIsEmail:
                hideLoginRadio.Checked = true;
                hideLoginSelect.SelectedIndex = 0;
                break;
            case LoginMode.HideLoginFieldLoginIsAccount:
                hideLoginRadio.Checked = true;
                hideLoginSelect.SelectedIndex = 1;
                break;
            case LoginMode.HideEmailField:
                hideEmailRadio.Checked = true;
                break;
            case LoginMode.HideEmailFieldDisplayDomainAfterLogin:
                hideEmailRadio.Checked = true;
                intDisplayDomainAfterLoginField.Checked = true;
                break;
            case LoginMode.HideEmailFieldLoginIsLoginAndDomain:
                hideEmailRadio.Checked = true;
                intLoginAsConcatination.Checked = true;
                break;
            case LoginMode.HideEmailFieldDisplayDomainAfterLoginAndLoginIsLoginAndDomain:
                hideEmailRadio.Checked = true;
                intDisplayDomainAfterLoginField.Checked = true;
                intLoginAsConcatination.Checked = true;
                break;
            default:
                standartLoginRadio.Checked = true;
                break;
        }
        txtUseDomain.Value = settings.DefaultDomainOptional;

        intAllowAdvancedLogin.Checked = settings.AllowAdvancedLogin;
        intAutomaticCorrectLogin.Checked = settings.AutomaticCorrectLoginSettings;
        intAllowLangOnLogin.Checked = settings.AllowLanguageOnLogin;
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

            if (standartLoginRadio.Checked)
            {
                settings.HideLoginMode = LoginMode.Default;
            }
            else if (hideLoginRadio.Checked)
            {
                if (hideLoginSelect.SelectedIndex == 0)
                {
                    settings.HideLoginMode = LoginMode.HideLoginFieldLoginIsEmail;
                }
                else
                {
                    settings.HideLoginMode = LoginMode.HideLoginFieldLoginIsAccount;
                }
            }
            else
            {
                if ((intDisplayDomainAfterLoginField.Checked) && (intLoginAsConcatination.Checked))
                {
                    settings.HideLoginMode = LoginMode.HideEmailFieldDisplayDomainAfterLoginAndLoginIsLoginAndDomain;
                }
                else if (intDisplayDomainAfterLoginField.Checked)
                {
                    settings.HideLoginMode = LoginMode.HideEmailFieldDisplayDomainAfterLogin;
                }
                else if (intLoginAsConcatination.Checked)
                {
                    settings.HideLoginMode = LoginMode.HideEmailFieldLoginIsLoginAndDomain;
                }
                else
                {
                    settings.HideLoginMode = LoginMode.HideEmailField;
                }
            }
            settings.DefaultDomainOptional = txtUseDomain.Value;

            settings.AllowAdvancedLogin = intAllowAdvancedLogin.Checked;
            settings.AutomaticCorrectLoginSettings = intAutomaticCorrectLogin.Checked;
            settings.AllowLanguageOnLogin = intAllowLangOnLogin.Checked;

            settings.SaveWebmailSettings(AdminPanelUtils.GetWebMailDataFolder());
            Context.Application.Remove(Constants.sessionSettings);
            AdminPanelUtils.SetPageReportMessage(Page, Constants.mailAdmSaveSuccess);
		}
		catch (Exception error)
		{
            Log.WriteException(error);
            AdminPanelUtils.SetPageErrorMessage(Page, Constants.mailAdmSaveUnsuccess);
		}
	}
}
