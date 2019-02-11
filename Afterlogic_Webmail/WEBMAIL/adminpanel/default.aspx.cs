using System;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;
using MailBee.Pop3Mail;
using WebMail;

public partial class DefaultPage : Page
{
    protected AdminPanelSettings _settings;
    protected string _helpUrl;
    
    public string PageTitle
    {
        set
        {
            Title += value;
        }
    }

    public string HelpUrl
    {
        set
        {
            _helpUrl = value;
        }
        get
        {
            return _helpUrl;
        }
    }

    public string Body;

    public AdminPanelSettings Settings
    {
        set
        {
            _settings = value;
        }
        get
        {
            if (!_settings.Loaded)
            {
                return _settings.Load();
            }
            else
            {
                return _settings;
            }
        }
    }

    public DefaultPage()
    {
        _settings = new AdminPanelSettings().Load();
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        Session.Timeout = AdminPanelConstants.SessionTimeout;

        string mode = (!string.IsNullOrEmpty(Request.QueryString["mode"]) ? Request.QueryString["mode"] : string.Empty);
        if (!Page.IsPostBack)
        {
            switch (mode)
            {
                case "logout":
                    Session.Clear();
                    Session.Abandon();
                    break;
            }
        }
        
        if (Session["AUTH"] == null)
        {
            Control ctrl = LoadControl("core/Login.ascx");
            if (ctrl != null)
            {
                mainPlaceHolder.Controls.Add(ctrl);
            }
        }
        else
        {
            if (File.Exists(Path.Combine(Page.MapPath(""), "install.htm")))
            {
                AdminPanelUtils.SetPageErrorMessage(Page, @"Please delete install.htm file.");
            }

            Control ctrl = LoadControl("core/AdminPanel.ascx");
            if (ctrl != null)
            {
                mainPlaceHolder.Controls.Add(ctrl);
            }
        }

    }

}
