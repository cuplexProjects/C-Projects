using System.Configuration;
using System.IO;
using System.Text;
using System;
using WebMail;

/// <summary>
///		Summary description for mailadm_debug_settings.
/// </summary>
public partial class debug_settingsLite : System.Web.UI.UserControl
{

	protected void Page_Load(object sender, EventArgs e)
	{
        WebmailSettings settings = new WebMailSettingsCreator().CreateWebMailSettings(AdminPanelUtils.GetWebMailDataFolder());
        
        if (!Page.IsPostBack)
		{
            Utils.SettingsPath = AdminPanelUtils.GetWebMailFolder();
            string logPath = Utils.GetLogFilePath();
			intEnableLogging.Checked = settings.EnableLogging;
			txtPathForLog.Value = logPath;
		}
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
            
            settings.EnableLogging = intEnableLogging.Checked;
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

	protected void ClearLogButton_ServerClick(object sender, System.EventArgs e)
	{
		try
		{
            Utils.ClearLog();
            AdminPanelUtils.SetPageReportMessage(this.Page, Constants.mailAdmLogClearSuccess);
		}
		catch (Exception error)
		{
            Log.WriteException(error);
            AdminPanelUtils.SetPageErrorMessage(this.Page, Constants.mailAdmLogClearUnsuccess);
		}
	}
}
