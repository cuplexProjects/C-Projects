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
using System.Collections.Generic;
using System.IO;
using WebMail;

public partial class PlugIns_Security_security_settings : System.Web.UI.UserControl
{
    private string _userName;
    private AdminPanelSettings apSettings = new AdminPanelSettings();

    protected bool _isServerExist = false;
    protected bool _isWebMailExist = false;

    protected string UserName
    {
        get { return _userName; }
        set { _userName = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        apSettings = (Page as DefaultPage).Settings;
        _isServerExist = Plugin.IsPluginExist(AdminPanelConstants.PluginName.Server, apSettings);
        _isWebMailExist = Plugin.IsPluginExist(AdminPanelConstants.PluginName.WebMail, apSettings);

        UserName = apSettings.User;
        txtSecurityPassword.Attributes.Add("value", AdminPanelConstants.DummyPassword);
        txtSecurityPasswordConfirm.Attributes.Add("value", AdminPanelConstants.DummyPassword);
        txtHost.Value = apSettings.Host;
        txtPort.Value = apSettings.Port;

        if (!_isServerExist)
        {
            trHost.Visible = false;
            trPort.Visible = false;
        }
    }

    public void SaveSettings(object sender, System.EventArgs e)
    {
        bool flag = false;
        this.Context.Application.Remove(AdminPanelConstants.serverSettings);
        this.Context.Application.Remove(Constants.sessionSettings);

        string adminpanelxml = Path.Combine(AdminPanelUtils.GetAdminPanelDataFolderPath(), @"adminpanel.xml");
        bool isAdminpanelxmlWrite = false;
        FileStream fs;
        try
        {
            fs = File.Open(adminpanelxml, FileMode.Open, FileAccess.Write);
            fs.Close();
            isAdminpanelxmlWrite = true;
        }
        catch (UnauthorizedAccessException)
        {
            isAdminpanelxmlWrite = false;
        }

        if (isAdminpanelxmlWrite)
        {
            if (txtSecurityPassword.Text.Trim() != AdminPanelConstants.DummyPassword)
            {
                if (_isServerExist)
                {
                    try
                    {
                        if (AdminPanelControlAccounts.IsControlAccountExist(UserName))
                            AdminPanelControlAccounts.SetControlAccountPassword(UserName, txtSecurityPassword.Text);
                        else
                        {
                            AdminPanelControlAccounts.AdminPanelControlAccount ca = new AdminPanelControlAccounts.AdminPanelControlAccount();
                            ca.Name = UserName;
                            ca.Password = txtSecurityPassword.Text;
                            AdminPanelControlAccounts.AddControlAccount(ca);
                        }
                        flag = true;
                    }
                    catch (Exception ex)
                    {
                        flag = false;
                        AdminPanelUtils.SetPageErrorMessage(Page, ex.Message);
                    }
                    if (flag)
                        apSettings.Pass = txtSecurityPassword.Text;
                }
                else
                {
                    apSettings.Pass = txtSecurityPassword.Text;
                }
            }
            if (_isServerExist)
            {
                apSettings.Host = txtHost.Value;
                apSettings.Port = txtPort.Value;
            }
            try
            {
                apSettings.Save();
                AdminPanelUtils.SetPageReportMessage(Page, "Settings are successfully saved!");
            }
            catch (Exception ex)
            {
                AdminPanelUtils.SetPageErrorMessage(Page, ex.Message);
            }
        }
        else
        {
            AdminPanelUtils.SetPageErrorMessage(Page, "Failed to access \"adminpanel.xml\" config file.");
        }
    }

}
