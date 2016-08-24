using System ;
using System.Collections;
using System.Web;
using XMailAdminProxy;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using WebMail;

public partial class PlugIns_Domains_domain_edit : System.Web.UI.UserControl
{
	protected string _domainType = string.Empty;
	protected bool _isUpdate = false;
    protected bool _isNew = false;
    protected bool _isServerExist = false;
    protected bool _isWebMailExist = false;
    protected string ClientPrefix = string.Empty;
    protected string UID = string.Empty;
    protected string domain_filter = string.Empty;
    protected string pluginID = "";
    protected string _actionDomain = "";

	protected void Page_Load(object sender, EventArgs e)
	{
        AdminPanelSettings apSettings = (Page as DefaultPage).Settings;

        _isServerExist = Plugin.IsPluginExist(AdminPanelConstants.PluginName.Server, (Page as DefaultPage).Settings);
        _isWebMailExist = Plugin.IsPluginExist(AdminPanelConstants.PluginName.WebMail, (Page as DefaultPage).Settings);
        ClientPrefix = ClientID;


        if (!apSettings.AdvancedOptions) divDomainTabs.Visible = false;
        if (Session["pluginID"] != null)
        {
            pluginID = Session["pluginID"].ToString();
        }
        if (!string.IsNullOrEmpty( Request.QueryString["type"])) _domainType = Request.QueryString["type"];

        if (Request.QueryString["mode"] != null)
		{
			switch (Request.QueryString["mode"])
			{
				case "edit":
					_isUpdate = true;
					break;
                case "new":
                    _isNew = true;
                    break;
                default:
					_isNew = true;
					break;
			}
		}
        InitDomain();
    }

    protected void InitDomain()
    {
        string[] dName = new string[1];
        string domainName = "";

        if (_isUpdate)
        {
            _actionDomain = "Edit Domain";
            intIncomingMailProtocol.Attributes["class"] = "wm_hide";
            ButtonSave.Visible = false;
            trForHr.Visible = false;
            textDomainName.Attributes.Add("class", "wm_hide");

            string uid = HttpUtility.UrlDecode(this.Request.QueryString["uid"].ToString());
            SelectedValueStruct sds = AdminPanelUtils.ParseSelectedValue(uid);
            hidCustomDomain.Value = "0";
            dName[0] = sds.Name;
            string domains;

            XMLayer xmLayer = XMServer.CreateXMLayer();
            switch (sds.Type)
            {
                case AdminPanelConstants.UserType.xm:
                case AdminPanelConstants.UserType.xma:
                    try
                    {
                        xmLayer.Login();
                        domains = xmLayer.ListDomains(dName);
                        using (StringReader sr = new StringReader(domains))
                        {
                            string line = string.Empty;
                            while ((line = sr.ReadLine()) != null)
                            {
                                line = line.Trim();
                                if (line == ".") break;
                                domainName = line.Trim(new char[] { '"' });
                            }
                        }
                        textDomainName.Value = domainName;
                    }
                    catch (Exception error)
                    {
                        Log.WriteException(error);
                        AdminPanelUtils.SetPageErrorMessage(this.Page, error.Message);
                    }
                    finally
                    {
                        xmLayer.Logout();
                    }
                    break;

                case AdminPanelConstants.UserType.xmc:
                    
                    _domainType = "vir";
                    try
                    {
                        xmLayer.Login();
                        domainName = dName[0];
                        hidCustomDomain.Value = "1";
                        ButtonSave.Visible = true;
                        trForHr.Visible = true;
                        string DomainStringArray = XMDomain.GetCustomDomainTab(sds.Name)[0];

                        using (StringReader sr = new StringReader(DomainStringArray))
                        {
                            string[] data;
                            string line;
//                            RedirectionsListDDL.Items.Clear();
                            LRedirectionsListDDL.Items.Clear();

                            while ((line = sr.ReadLine()) != null)
                            {
                                line = line.Trim();
                                data = XMDomain.extractCustomDomainData(line);
                                int i;
                                switch (data[0])
                                {
                                    case "mailbox":
                                        break;
/*
                                    case "redirect":
                                        for (i = 1; i < data.Length; i++)
                                            RedirectionsListDDL.Items.Add(data[i]);
                                        break;
*/
                                    case "lredirect":
                                        for (i = 1; i < data.Length; i++)
                                            LRedirectionsListDDL.Items.Add(data[i]);
                                        break;
                                }
                            }
                            AdvancedID.Text = XMDomain.GetCustomDomainTab(domainName)[1];
                            textDomainName.Value = domainName;
                        }
                    }
                    catch (Exception error)
                    {
                        Log.WriteException(error);
                        AdminPanelUtils.SetPageErrorMessage(this.Page, error.Message);
                    }
                    finally
                    {
                        xmLayer.Logout();
                    }
                    break;

                case AdminPanelConstants.UserType.wm:
                    //Webmail Domains
                    _domainType = "rem";
                    ButtonSave.Visible = true;
                    trForHr.Visible = true;
                    try
                    {
                        Domain dom = Domain.LoadFromDb(sds.Name.Trim());
                        textDomainName.Value = dom.Name;
                        domainName = dom.Name;
                        txtIncomingMail.Value = dom.MailIncomingHost;
                        intIncomingMailPort.Value = dom.MailIncomingPort.ToString();
                        txtOutgoingMail.Value = dom.MailOutgoingHost;
                        intOutgoingMailPort.Value = dom.MailOutgoingPort.ToString();
                        intReqSmtpAuthentication.Checked = dom.MailOutgoingAuthentication;
                        hidWebMailDomainID.Value = dom.ID.ToString();
                        
                        
                        switch (dom.MailIncomingProtocol)
                        {
                            case IncomingMailProtocol.Pop3:
                                intIncomingMailProtocol.SelectedIndex = 0;
                                break;
                            case IncomingMailProtocol.Imap4:
                                intIncomingMailProtocol.SelectedIndex = 1;
                                break;
                            default:
                                intIncomingMailProtocol.SelectedIndex = 0;
                                break;
                        }
                    }
                    catch (Exception error)
                    {
                        Log.WriteException(error);
                        AdminPanelUtils.SaveState("SessPageErrorMessage", WebMail.Constants.mailAdmUpdateAccountUnsuccess, Page.Session);
                    }
                    break;
            }
            UID = HttpUtility.UrlEncode(sds.Type + "_" + domainName);

            if (AdminPanelUtils.IsSuperAdmin(Page.Session, (Page as DefaultPage).Settings))
            {
                Domain dom = new Domain();

                if (UID.Length > 0)
                {
                    try
                    {
                        SelectedValueStruct sus = AdminPanelUtils.ParseSelectedValue(uid);

                        if (sus.Type != AdminPanelConstants.UserType.xm)
                        {
                            try
                            {
                                dom = Domain.LoadFromDb(sus.Name);
                            }
                            catch { }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.WriteException(ex);
                        AdminPanelUtils.SetPageErrorMessage(this.Page, (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
                    }
                }

                if (dom != null)
                {
                    domain_filter = dom.ID.ToString();
                }
            }

            litDomainName.Text = @"<span style=""font-size: large;"">" + domainName + "</span>";

            switch (_domainType)
            {
                case "vir":
                    litDomainDescription.Text = "Defines rules for forwarding mail from this domain to other addresses.";
                    break;
                case "rem":
                    litDomainDescription.Text = "Contains users hosted by other mail services (e.g. gmx.com).";
                    break;
                default:
                    litDomainDescription.Text = "Contains users hosted by this server.";
                    break;
            }
        }
        else
        {
            _actionDomain = "Create Domain";

            divDomainName.Attributes["class"] = "wm_hide";
            
            intIncomingMailPort.Value = "110";
            intOutgoingMailPort.Value = "25";

            switch (_domainType)
            {
                case "vir":
                    litDomainDescriptionView.Text = "Defines rules for forwarding mail from this domain to other addresses.";
                    break;
                case "rem":
                    litDomainDescriptionView.Text = "Contains users hosted by other mail services (e.g. gmx.com).";
                    break;
                default:
                    litDomainDescriptionView.Text = "Contains users hosted by this server.";
                    break;
            }

        }
        switch (_domainType)
        {
            case "vir":
                tr_custom_options.Attributes["class"] = "";
                break;
            case "rem":
                tr_webmail_options.Attributes["class"] = "";
                break;
            default:
                tr_custom_options.Attributes["class"] = "wm_hide";
                tr_webmail_options.Attributes["class"] = "wm_hide";
                break;
        }

        if (!AdminPanelUtils.IsSuperAdmin(Page.Session, (Page as DefaultPage).Settings))
        {
            tr_see_webmail.Attributes["class"] = "wm_hide";       
        }
    }
    
    protected void SaveDomain_OnClick(object sender, EventArgs e)
	{
        if (_isUpdate)
        {
            UpdateDomain();
        }
        else if (AdminPanelUtils.IsSuperAdmin(Page.Session, (Page as DefaultPage).Settings))
        {
            AddNewDomain();
        }
        Response.Redirect("default.aspx?mode=domains", false);
    }

    public void UpdateDomain()
    {

        switch (_domainType)
        {
            case "vir":
            case "reg":
                this.UpdateXMailDomain();
                break;
            case "rem":
                this.UpdateWebMailDomain();
                break;
        }

        AdminPanelUtils.SaveState("SessPageReportMessage", WebMail.Constants.mailAdmUpdateAccountSuccess, Page.Session);
        InitDomain();
    }

    public void UpdateXMailDomain()
    {
        try
        {
            XMLayer xmLayer = XMServer.CreateXMLayer();
            xmLayer.Login();

            if (Int32.Parse(hidCustomDomain.Value) == 1)
            {
                // edit custom domain
                StringBuilder redirect = new StringBuilder("redirect");
/*
                if (Request.Params[RedirectionsListDDL.UniqueID] != null)
                {
                    string[] RedirectionsList = Request.Params[RedirectionsListDDL.UniqueID].ToString().Split(',');
                    for (int i = 0; i < RedirectionsList.Length; i++)
                        redirect.AppendFormat("\t\"{0}\"", RedirectionsList[i]);
                }
*/
                StringBuilder lredirect = new StringBuilder("lredirect");
                if (Request.Params[LRedirectionsListDDL.UniqueID] != null)
                {
                    string[] LRedirectionsList = Request.Params[LRedirectionsListDDL.UniqueID].ToString().Split(',');
                    for (int i = 0; i < LRedirectionsList.Length; i++)
                    {
                        lredirect.AppendFormat("\t\"{0}\"", LRedirectionsList[i]);
                    }
                }
                string data = string.Format("{0}\r\n{1}\r\n{2}\r\n.\r\n", redirect.ToString(), lredirect.ToString(), AdvancedID.Text);
                xmLayer.SetCustomDomainTab(textDomainName.Value.Trim(), data);
            }

            xmLayer.Logout();
        }
        catch (Exception ex)
        {
            Log.WriteException(ex);
            AdminPanelUtils.SetPageErrorMessage(this.Page, ex.Message);
        }
    }

    public void UpdateWebMailDomain()
    {
        try
        {
            string domainName = textDomainName.Value;
            string incomingMail = txtIncomingMail.Value;
            string outgoingMail = txtOutgoingMail.Value;
            int domainID = Int32.Parse(hidWebMailDomainID.Value);

            if (!AdminPanelUtils.IsSuperAdmin(Page.Session, (Page as DefaultPage).Settings))
            {
                if (!Subadmin.IsSubadminDomain(int.Parse(Session["AdminID"].ToString()), domainID))
                {
                    return;
                }
            }
            
            bool useSmtpAuth = intReqSmtpAuthentication.Checked;
            int incomingPort = 0;
            int.TryParse(intIncomingMailPort.Value, out incomingPort);
            int outgoingPort = 0;
            int.TryParse(intOutgoingMailPort.Value, out outgoingPort);
            IncomingMailProtocol mail_protocol = IncomingMailProtocol.Pop3;
            switch (intIncomingMailProtocol.SelectedIndex)
            {
                case 0:
                    mail_protocol = IncomingMailProtocol.Pop3;
                    break;
                case 1:
                    mail_protocol = IncomingMailProtocol.Imap4;
                    break;
                case 2:
                    mail_protocol = IncomingMailProtocol.WMServer;
                    break;
            }
            Domain dom = Domain.GetDomain(domainID);
            
            dom.Name = domainName;
            dom.MailIncomingProtocol = mail_protocol;
            dom.MailIncomingHost = incomingMail;
            dom.MailIncomingPort = incomingPort;
            dom.MailOutgoingHost = outgoingMail;
            dom.MailOutgoingPort = outgoingPort;
            dom.MailOutgoingAuthentication = useSmtpAuth;
            
            
            dom.Global_addr_book = false;
             
            dom.Save();
        }
        catch (Exception error)
        {
            Log.WriteException(error);
            AdminPanelUtils.SetPageErrorMessage(this.Page, (error.InnerException != null ? error.InnerException.Message : error.Message));
        }
    }
    
    public void AddNewDomain()
    {
        // new domain
        bool result = false;
        switch (_domainType)
        {
            case "vir":
            case "reg":
                result = this.AddNewXMailDomain();
                break;
            case "rem":
                result = this.AddNewWebMailDomain();
                break;
        }
        if (result)
        {
            string uid = "";
            switch (_domainType)
            {
                case "reg":
                    if (_isServerExist && _isWebMailExist)
                        uid = "xma_" + textDomainName.Value;
                    else
                        uid = "xm_" + textDomainName.Value;
                    break;
                case "rem":
                    uid = "wm_" + textDomainName.Value;
                    break;
            }
            
            
            Response.Redirect("default.aspx?plugin=" + pluginID.ToString() + "&mode=domains", false);
            
        }
    }

    public bool AddNewXMailDomain()
    {
        string domainName = textDomainName.Value;
        bool result = false;
        AdminPanelUtils.SaveState("selectedDomain", null, Page.Session);
        XMLayer xmLayer = XMServer.CreateXMLayer();
        if (_domainType == "vir")
        {
            try
            {
                xmLayer.Login();
                // create custom domain
                StringBuilder redirect = new StringBuilder("redirect");
                if (Request.Params[RedirectionsListDDL.UniqueID] != null)
                {
                    string[] RedirectionsList = Request.Params[RedirectionsListDDL.UniqueID].ToString().Split(',');
                    for (int i = 0; i < RedirectionsList.Length; i++)
                    {
                        redirect.AppendFormat("\t\"{0}\"", RedirectionsList[i]);
                    }
                }

                StringBuilder lredirect = new StringBuilder("lredirect");
                if (Request.Params[LRedirectionsListDDL.UniqueID] != null)
                {
                    string[] LRedirectionsList = Request.Params[LRedirectionsListDDL.UniqueID].ToString().Split(',');
                    for (int i = 0; i < LRedirectionsList.Length; i++)
                    {
                        lredirect.AppendFormat("\t\"{0}\"", LRedirectionsList[i]);
                    }
                }
                string data = string.Format("{0}\r\n{1}\r\n{2}\r\n.\r\n", redirect.ToString(), lredirect.ToString(), AdvancedID.Text);
                xmLayer.SetCustomDomainTab(domainName, data);
                xmLayer.Logout();
                result = true;
            }
            catch (XMailException error)
            {
                Log.WriteException(error);
                AdminPanelUtils.SetPageErrorMessage(this.Page, (error.InnerException != null ? error.InnerException.Message : error.Message));
            }
        }
        else
        {
            try
            {
                xmLayer.Login();
                if (!xmLayer.DomainExist(domainName))
                {
                    xmLayer.AddDomain(domainName);
                }
                xmLayer.Logout();
                result = true;
            }
            catch (XMailException error)
            {
                Log.WriteException(error);
                AdminPanelUtils.SetPageErrorMessage(this.Page, error.Message);
            }

            // create webmail domain
            if (_isWebMailExist && result)
            {
                WebmailSettings wmSettings = (new WebMailSettingsCreator()).CreateWebMailSettings(AdminPanelUtils.GetWebMailDataFolder());

                try
                {
                    Domain dom = Domain.GetDomain(domainName);
                    if (dom == null)
                    {
                        int SmtpPort = wmSettings.XMailSmtpPort;
                        string SmtpHost = wmSettings.WmServerHost;
                        int ImapPort = 143;
                        string ImapHost = wmSettings.WmServerHost;
                        Domain new_dom = new Domain(0, domainName, IncomingMailProtocol.WMServer, ImapHost, ImapPort, SmtpHost, SmtpPort, false);
                        
                        
                        new_dom.Global_addr_book = false;
                        
                        new_dom.Save();
                        result = true;
                    }
                    else
                    {
                        result = false;
                        AdminPanelUtils.SaveState("SessPageErrorMessage", "Such domain already exists.", Page.Session);
                    }
                }
                catch (Exception error)
                {
                    result = false;
                    Log.WriteException(error);
                    AdminPanelUtils.SetPageErrorMessage(this.Page, (error.InnerException != null ? error.InnerException.Message : error.Message));
                }
            }
        }
        if (result == false)
        {
            try
            {
                xmLayer.Login();
                xmLayer.DeleteDomain(domainName);
                xmLayer.Logout();
            }
            catch (Exception error)
            {
                Log.WriteException(error);
            }
        }
        return result;
    }

    public bool AddNewWebMailDomain()
    {
        bool result = false;
        // create webmail domain
        string domainName = textDomainName.Value;
        string incomingMail = txtIncomingMail.Value;
        string outgoingMail = txtOutgoingMail.Value;
        bool useSmtpAuth = intReqSmtpAuthentication.Checked;
        int incomingPort = 0;
        int.TryParse(intIncomingMailPort.Value, out incomingPort);
        int outgoingPort = 0;
        int.TryParse(intOutgoingMailPort.Value, out outgoingPort);
        IncomingMailProtocol mail_protocol = IncomingMailProtocol.Pop3;
        switch (intIncomingMailProtocol.SelectedIndex)
        {
            case 0:
                mail_protocol = IncomingMailProtocol.Pop3;
                break;
            case 1:
                mail_protocol = IncomingMailProtocol.Imap4;
                break;
            case 2:
                mail_protocol = IncomingMailProtocol.WMServer;
                break;
        }

        try
        {
            Domain dom = Domain.GetDomain(domainName);
            if (dom == null)
            {
                Domain new_dom = new Domain(0, domainName, mail_protocol, incomingMail, incomingPort, outgoingMail, outgoingPort, useSmtpAuth);
                
                
                new_dom.Global_addr_book = false;
                
                new_dom.Save();
                result = true;
            }
            else
            {
                result = false;
                AdminPanelUtils.SaveState("SessPageErrorMessage", "Such domain already exists.", Page.Session);
            }
        }
        catch (Exception error)
        {
            result = false;
            Log.WriteException(error);
            AdminPanelUtils.SetPageErrorMessage(this.Page, (error.InnerException != null ? error.InnerException.Message : error.Message));
        }
        return result;
    }

}
