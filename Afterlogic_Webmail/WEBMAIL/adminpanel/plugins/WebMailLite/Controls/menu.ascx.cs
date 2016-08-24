using System;
using System.Data;
using System.Web.UI.WebControls;
using WebMail;

/// <summary>
///		Summary description for mailadm_menu.
/// </summary>
public partial class menuLite : System.Web.UI.UserControl
{
	protected int _selectedScreen = 0;
	protected string defaultSkin = "";
    protected bool default_domain_only = true;

	public int Screen
	{
		get { return _selectedScreen; }
		set { _selectedScreen = value; }
	}

	protected void Page_Load(object sender, System.EventArgs e)
	{
        WebmailSettings settings = new WebMailSettingsCreator().CreateWebMailSettings(AdminPanelUtils.GetWebMailDataFolder());

        divDatabase.Attributes["class"] = _selectedScreen == 1 ? "wm_selected_settings_item" : "wm_settings_item";
	    divCommon.Attributes["class"] = _selectedScreen == 3 ? "wm_selected_settings_item" : "wm_settings_item";
	    divInterface.Attributes["class"] = _selectedScreen == 4 ? "wm_selected_settings_item" : "wm_settings_item";
	    divLogin.Attributes["class"] = _selectedScreen == 5 ? "wm_selected_settings_item" : "wm_settings_item";
	    divDebug.Attributes["class"] = _selectedScreen == 6 ? "wm_selected_settings_item" : "wm_settings_item";
        if (settings.EnableWmServer)
        {
            divServer.Attributes["class"] = _selectedScreen == 8 ? "wm_selected_settings_item" : "wm_settings_item";
        }
        else
        {
            divServer.Visible = false;
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
}
