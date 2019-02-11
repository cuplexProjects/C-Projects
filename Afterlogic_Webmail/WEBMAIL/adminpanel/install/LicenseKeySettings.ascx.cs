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
using MailBee.Pop3Mail;
using MailBee;
using WebMail;
using System.IO;

public partial class LicenseKeySettings : System.Web.UI.UserControl
{
    public string _errorMessage = string.Empty;
    protected object _EmptyKey = string.Empty;

    protected int _web_step = 1;
    protected int _max_step = 7;

    protected void Page_Load(object sender, EventArgs e)
    {
        install _page = Page as install;
        _web_step = _page._web_step;
        _max_step = _page._max_step;

        AdminPanelSettings apSettings = new AdminPanelSettings().Load();

        if (Session["LicenseKey"] != null)
        {
            txtLicenseKey.Value = Session["LicenseKey"].ToString();
        }
        else
        {
            txtLicenseKey.Value = apSettings.LicenseKey;
        }

        _EmptyKey = Session["EmptyKey"];
    }

    protected void SubmitButton_Click(object sender, System.EventArgs e)
    {
        bool flag = false;
        try
        {
            AdminPanelSettings apSettings = new AdminPanelSettings().Load();

            string licenseKeyFromForm = txtLicenseKey.Value ?? string.Empty;

            if (string.IsNullOrEmpty(licenseKeyFromForm))
            {
                apSettings.LicenseKey = licenseKeyFromForm;
                flag = false;
            }
            else
            {
                try
                {
                    Pop3.LicenseKey = licenseKeyFromForm.Substring(0, 39);
                    Pop3 pop = new Pop3();
                    flag = true;
                }
                catch (Exception error)
                {
                    Log.WriteException(error);
                    _errorMessage = "License Key is invalid";
                    GetLicenseUrl.Visible = true;
                    flag = false;
                }
                apSettings.LicenseKey = licenseKeyFromForm;
            }
            if (File.Exists(Path.Combine(AdminPanelUtils.GetWebMailDataFolder(), @"settings\settings.xml")))
            {
                WebmailSettings settings = new WebmailSettings().CreateInstance(AdminPanelUtils.GetWebMailDataFolder());
                settings.LicenseKey = licenseKeyFromForm;
                settings.SaveWebmailSettings(AdminPanelUtils.GetWebMailDataFolder());
            }

            apSettings.Save();
            _errorMessage = Constants.mailAdmSaveSuccess;

            if (flag == true)
            {
                Session["LicenseKey"] = txtLicenseKey.Value;
                Response.Redirect("install.aspx?mode=db", true);
            }
            else
            {
                _errorMessage = "Please specify valid license key.";
            }
        }
        catch (Exception error)
        {
            Log.WriteException(error);
            _errorMessage = Constants.mailAdmSaveUnsuccess;
        }
    }
}
