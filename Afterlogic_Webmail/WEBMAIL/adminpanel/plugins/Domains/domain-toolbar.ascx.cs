using System;

public partial class PlugIns_Domains_domain_toolbar : System.Web.UI.UserControl
{
    protected bool _isServerExist = false;
    protected bool _isWebMailExist = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        _isServerExist = Plugin.IsPluginExist(AdminPanelConstants.PluginName.Server, (Page as DefaultPage).Settings);
        _isWebMailExist = (Plugin.IsPluginExist(AdminPanelConstants.PluginName.WebMail, (Page as DefaultPage).Settings) || Plugin.IsPluginExist(AdminPanelConstants.PluginName.WebMailLite, (Page as DefaultPage).Settings));
        div_reg.Visible = false;
        div_vir.Visible = false;
        div_rem.Visible = false;

        if (_isServerExist)
        {
            div_reg.Visible = true;
            div_vir.Visible = true;
        }
        if (_isWebMailExist)
        {
            div_rem.Visible = true;
        }
    }
}
