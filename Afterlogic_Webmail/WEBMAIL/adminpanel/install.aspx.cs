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
using System.Globalization;
using System.IO;

public partial class install : System.Web.UI.Page
{
    protected Control ctrl = null;

    protected string _Title = string.Empty;

    protected string _Check = string.Empty;
    protected string _License = string.Empty;
    protected string _LicenseKey = string.Empty;
    protected string _DB = string.Empty;
    protected string _Common = string.Empty;
    protected string _CheckConnection = string.Empty;
    protected string _Finish = string.Empty;
    protected string _key = string.Empty;
    
    protected int _rnd = 77777;
    protected string _img_url = "";

    public string check_str = "";
    public int _step = 2;
    public int _web_step = 1;
    public int _max_step = 7;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!File.Exists(Path.Combine(MapPath(""), "install.htm"))) Response.Redirect("default.aspx", true);

        string mode = string.Empty;
        if (Request.QueryString["mode"] == null)
        {
            if (Request.QueryString["key"] != null)
            {
                Session["LicenseKey"] = Request.QueryString["key"];
                Session["EmptyKey"] = null;
            }
            else
            {
                
            }
            Response.Redirect("install.aspx?mode=check", true);
        }
        else mode = Request.QueryString["mode"].ToString().ToLower();

        _key = (Session["LicenseKey"] != null) ? Session["LicenseKey"].ToString() : string.Empty;
        
        Random rnd = new Random();
        _rnd = rnd.Next(10000, 99999);
        
        menuCheck.Attributes["class"] = "wm_install_item_noactiv";
        menuLicense.Attributes["class"]    = "wm_install_item_noactiv";
        menuLicenseKey.Attributes["class"] = "wm_install_item_noactiv";
        menuDB.Attributes["class"]         = "wm_install_item_noactiv";
        menuCommon.Attributes["class"]     = "wm_install_item_noactiv";
        menuCheckConnection.Attributes["class"] = "wm_install_item_noactiv";
        menuEnd.Attributes["class"] = "wm_install_item_noactiv";

        _Check = "<nobr><b>Compatibility Test</b></nobr>";
        _License = "<nobr><b>License Agreement</b></nobr>";
        _LicenseKey = "<nobr><b>License Key</b></nobr>";
        _DB = "<nobr><b>Database Settings</b></nobr>";
        _Common = "<nobr><b>Admin Panel Settings</b></nobr>";
        _CheckConnection = "<nobr><b>E-mail Server Test</b></nobr>";
        _Finish = "<nobr><b>Completed</b></nobr>";

        string HrefMenu = @"<a href='install.aspx?mode={0}'>{1}</a>";

        switch (mode)
        {
            case "check":
                _step = 2;
                _Title = "Compatibility Test";
                menuCheck.Attributes["class"] = "wm_selected_install_item";
                ctrl = LoadControl(@"install\CheckSettings.ascx");
                break;
            case "license":
                _step = 3;
                _Title = "License Agreement";
                _Check = string.Format(HrefMenu, "check", _Check);
                menuLicense.Attributes["class"] = "wm_selected_install_item";
                menuCheck.Attributes["class"] = "wm_install_item";
                ctrl = LoadControl(@"install\License.ascx");
                break;
            
            case "db":
                
                _step = 4;
                _Title = "Database Settings";
                _Check = string.Format(HrefMenu, "check", _Check);
                _License = string.Format(HrefMenu, "license", _License);
                _LicenseKey = string.Format(HrefMenu, "licensekey", _LicenseKey);
                menuDB.Attributes["class"] = "wm_selected_install_item";
                menuLicenseKey.Attributes["class"] = "wm_install_item";
                menuLicense.Attributes["class"] = "wm_install_item";
                menuCheck.Attributes["class"] = "wm_install_item";
                ctrl = LoadControl(@"install\DatabaseSettings.ascx");
                break;
            case "common":
                
                _step = 5;
                _Title = "Admin Panel Settings";
                _Check = string.Format(HrefMenu, "check", _Check);
                _License = string.Format(HrefMenu, "license", _License);
                _LicenseKey = string.Format(HrefMenu, "licensekey", _LicenseKey);
                _DB = string.Format(HrefMenu, "db", _DB);
                menuCommon.Attributes["class"] = "wm_selected_install_item";
                menuDB.Attributes["class"] = "wm_install_item";
                menuLicenseKey.Attributes["class"] = "wm_install_item";
                menuLicense.Attributes["class"] = "wm_install_item";
                menuCheck.Attributes["class"] = "wm_install_item";
                ctrl = LoadControl(@"install\CommonSettings.ascx");
                break;
            case "connection":
                
                _step = 6;
                _Title = "E-mail Server Test";
                _Check = string.Format(HrefMenu, "check", _Check);
                _License = string.Format(HrefMenu, "license", _License);
                _LicenseKey = string.Format(HrefMenu, "licensekey", _LicenseKey);
                _DB = string.Format(HrefMenu, "db", _DB);
                _Common = string.Format(HrefMenu, "common", _Common);
                menuCommon.Attributes["class"] = "wm_install_item";
                menuCheckConnection.Attributes["class"] = "wm_selected_install_item";
                menuDB.Attributes["class"] = "wm_install_item";
                menuLicenseKey.Attributes["class"] = "wm_install_item";
                menuLicense.Attributes["class"] = "wm_install_item";
                menuCheck.Attributes["class"] = "wm_install_item";
                ctrl = LoadControl(@"install\CheckConnection.ascx");
                break;
            case "end":
                
                _step = 7;
                _Title = "Completed";
                _Check = string.Format(HrefMenu, "check", _Check);
                _License = string.Format(HrefMenu, "license", _License);
                _LicenseKey = string.Format(HrefMenu, "licensekey", _LicenseKey);
                _DB = string.Format(HrefMenu, "db", _DB);
                _CheckConnection = string.Format(HrefMenu, "connection", _CheckConnection);
                _Common = string.Format(HrefMenu, "common", _Common);
                menuEnd.Attributes["class"] = "wm_selected_install_item";
                menuCommon.Attributes["class"] = "wm_install_item";
                menuDB.Attributes["class"] = "wm_install_item";
                menuLicenseKey.Attributes["class"] = "wm_install_item";
                menuLicense.Attributes["class"] = "wm_install_item";
                menuCheck.Attributes["class"] = "wm_install_item";
                menuCheckConnection.Attributes["class"] = "wm_install_item";
                ctrl = LoadControl(@"install\EndOfInstallation.ascx");
                break;
        }
        menuLicenseKey.Visible = false;

        _web_step = _step - 1;
        
        _max_step = 6;

        if (ctrl != null)
        {
            ContentPlaceHolder.Controls.Add(ctrl);
        }

        if (Session["EmptyKey"] == null)
        {
            
            _img_url = "http://afterlogic.com/img/wml-net-install-logo.png?key=" + _key + "&step=" + _step.ToString() + "&rnd=" + _rnd.ToString();
        }
        else
        {
            
            _img_url = "http://afterlogic.com/img/wml-net-install-logo.png?key=" + _key + "&step=" + _step.ToString() + "&rnd=" + _rnd.ToString();
        }
    }
}


