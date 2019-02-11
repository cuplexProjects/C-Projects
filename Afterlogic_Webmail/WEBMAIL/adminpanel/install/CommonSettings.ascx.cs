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
using WebMail;

public partial class CommonSettings : System.Web.UI.UserControl
{
    private string _userName;
    private AdminPanelSettings apSettings = new AdminPanelSettings();

    protected int _web_step = 1;
    protected int _max_step = 7;

    protected string UserName
    {
        get { return _userName; }
        set { _userName = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        apSettings.Load();
        UserName = apSettings.User;
        install _page = Page as install;

        _web_step = _page._web_step;
        _max_step = _page._max_step;

        if (apSettings.Pass.Trim().Length > 0)
        {
            txtPassword.Attributes.Add("value", AdminPanelConstants.DummyPassword);
            txtPasswordConfirm.Attributes.Add("value", AdminPanelConstants.DummyPassword);
        }
    }

    public void SaveSettings(object sender, System.EventArgs e)
    {
        if (txtPassword.Text.Trim() != AdminPanelConstants.DummyPassword)
        {
            apSettings.Pass = txtPassword.Text;
        }
        try
        {
            apSettings.Save();
            AdminPanelUtils.SetPageReportMessage(Page, "Settings are successfully saved!");
            Response.Redirect("install.aspx?mode=connection", true);
        }
        catch(Exception ex)
        {
            Log.WriteException(ex);
            AdminPanelUtils.SetPageErrorMessage(Page, (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
        }
    }
}
