using System;
using System.Data;
using System.Web;
using System.Web.UI;
using XMailAdminProxy;
using System.IO;
using WebMail;
using System.Collections.Generic;

public partial class domains_management : System.Web.UI.UserControl
{
	protected OutputDomains _outputDomains = OutputDomains.None;
	protected int _domainsCount = 0;
	protected int _pageSize = AdminPanelConstants.PageSize;
	protected bool showXMailDomains = true;
	protected int _isCustom = 0;
    protected bool _isServerExist = false;
    protected bool _isWebMailExist = false;
    protected int page = 1;

    protected int adminID = -1;

    protected string si_Type = string.Empty;
    protected string si_Name = string.Empty;
    protected string ClientPrefix = string.Empty;

    public int PageNumber
    {
        get { return page; }
        set { page = value; }
    }
    
    public string OrderBy
	{
		get { return (string)Session["wm_adm_dm_order"] ?? "name"; }
		set { Session["wm_adm_dm_order"] = value; }
	}

	public bool Asc
	{
		get { return (Session["wm_adm_dmAsc"] != null) ? Convert.ToBoolean(Session["wm_adm_dmAsc"].ToString()) : true; }
		set { Session["wm_adm_dmAsc"] = value.ToString(); }
	}
	
    public string SearchCondition
	{
		get { return (string)Session["wm_adm_dm_condition"] ?? string.Empty; }
		set { Session["wm_adm_dm_condition"] = value; }
	}

	public int PageSize
	{
		get { return _pageSize; }
		set { _pageSize = value; }
	}

    public string pluginID
    {
        get { return (string)Session["pluginID"] ?? ""; }
    }

	protected void Page_Load(object sender, EventArgs e)
	{
        ClientPrefix = this.ClientID;

        _isServerExist = Plugin.IsPluginExist(AdminPanelConstants.PluginName.Server, (Page as DefaultPage).Settings);
        _isWebMailExist = (Plugin.IsPluginExist(AdminPanelConstants.PluginName.WebMail, (Page as DefaultPage).Settings) || Plugin.IsPluginExist(AdminPanelConstants.PluginName.WebMailLite, (Page as DefaultPage).Settings));
        
        if (Session["AdminID"] != null)
        {
            int.TryParse(Session["AdminID"].ToString(), out adminID);
        }

        if (this.Request.QueryString["uid"] != null)
        {
            HFUID.Value = HttpUtility.UrlEncode(this.Request.QueryString["uid"]);
        }

        LiteralSearchOn.Text = string.Empty;
        if (SearchCondition.Trim() != string.Empty)
        {
            string CloseSearch = "<a href='?mode=close_search'>reset search</a>";
            LiteralSearchOn.Text = string.Format(@"Search result for: ""<b>{0}</b>""<br />{1}<br /><br />", SearchCondition, CloseSearch);
        }

        switch (this.OrderBy)
        {
            case "type":
                si_Type = (Asc ? @"<img src=""images/menu/order_arrow_down.gif"">" : @"<img src=""images/menu/order_arrow_up.gif"">");
                break;
            case "name":
                si_Name = (Asc ? @"<img src=""images/menu/order_arrow_down.gif"">" : @"<img src=""images/menu/order_arrow_up.gif"">");
                break;
        }

        if (AdminPanelUtils.IsSuperAdmin(Page.Session, (Page as DefaultPage).Settings))
        {
            PlugIns_Domains_domain_toolbar ctrlDomainToolbar = (PlugIns_Domains_domain_toolbar)LoadControl(@"domain-toolbar.ascx");
            if (ctrlDomainToolbar != null) PlaceHolderToolbar.Controls.Add(ctrlDomainToolbar);
        }

        if ((Request.QueryString["uid"] != null) || (Request.QueryString["mode"] == "new"))
        {
            LiteralContactsViewer_1.Text = @"
		<div class=""wm_contacts_card_line1""></div>
		<div class=""wm_contacts_card_line2""></div>
		<div class=""wm_contacts_card_line3""></div>
		<div class=""wm_contacts_card_line4""></div>
		<div class=""wm_contacts_card_line5""></div>
		    <div>
			    <table class=""wm_contacts_card_content"" id=""wm_contacts_card"">
				    <tr>
					    <td class=""wm_contacts_card_top_left"">
						    <div class=""wm_contacts_card_corner""></div>
					    </td>
					    <td class=""wm_contacts_card_top""></td>
					    <td class=""wm_contacts_card_top_right"">
						    <div class=""wm_contacts_card_corner""></div>
					    </td>
				    </tr>
				    <tr>
					    <td class=""wm_contacts_card_left""></td>
					    <td>
			    ";
            LiteralContactsViewer_2.Text = @"
					    </td>
					    <td class=""wm_contacts_card_right""></td>
				    </tr>
				    <tr>
					    <td class=""wm_contacts_card_bottom_left"">
						    <div class=""wm_contacts_card_corner""></div>
					    </td>
					    <td class=""wm_contacts_card_bottom""></td>
					    <td class=""wm_contacts_card_bottom_right"">
						    <div class=""wm_contacts_card_corner""></div>
					    </td>
				    </tr>
			    </table>
		    </div>
		<div class=""wm_contacts_card_line5""></div>
		<div class=""wm_contacts_card_line4""></div>
		<div class=""wm_contacts_card_line3""></div>
		<div class=""wm_contacts_card_line2""></div>
		<div class=""wm_contacts_card_line1""></div>";
        }
        HFPageInfo.Value = page.ToString();
        HFPageSize.Value = _pageSize.ToString();

        DataTable domains = LoadDomains();
        ShowDomains(domains, page, _pageSize, OrderBy, Asc, SearchCondition);
	}

    public DataTable LoadDomains()
    {
        DomainCollection domColl = null;
        XMDomainCollection xmDomains = new XMDomainCollection();

        DataTable domTable = new DataTable();
        domTable.Columns.Add("id", typeof(string));
        domTable.Columns.Add("name", typeof(string));
        domTable.Columns.Add("type", typeof(string));

        // xmail
        if (_isServerExist && AdminPanelUtils.IsSuperAdmin(Page.Session, (Page as DefaultPage).Settings))
        {
            xmDomains = XMDomain.GetDomains();
            XMDomainCollection xmDomsCust = XMDomain.GetCustomDomainins();
            xmDomains.Add(xmDomsCust);
        }

        // webmail
        if (_isWebMailExist)
        {
            try
            {
                if (AdminPanelUtils.IsSuperAdmin(Page.Session, (Page as DefaultPage).Settings))
                {
                    if (_isServerExist)
                    {
                        domColl = Domain.GetDomains();
                    }
                    else
                    {
                        domColl = Domain.GetDomains(new short[] { (short)IncomingMailProtocol.Pop3, (short)IncomingMailProtocol.Imap4 });
                    }
                }
                else
                {
                    domColl = Subadmin.GetDomains(adminID);
                }

                foreach (XMDomain xmDom in xmDomains)
                {
                    Domain dom = domColl.GetItem(xmDom.Name);
                    if (dom == null)
                    {
                        WebmailSettings settings = new WebMailSettingsCreator().CreateWebMailSettings();
                        Domain new_dom = new Domain(0, xmDom.Name, IncomingMailProtocol.WMServer, settings.WmServerHost, 143, settings.WmServerHost, settings.XMailSmtpPort, false);
                        new_dom.Save();
                        xmDom.Type = AdminPanelConstants.UserType.xma;
                    }
                }

                foreach (Domain dom in domColl)
                {
                    XMDomain xmDomain = xmDomains.GetItem(dom.Name);
                    if (xmDomain != null)
                    {
                        xmDomains.Remove(xmDomain);
                        xmDomains.Add(new XMDomain(dom.Name, dom.Name, dom.MailIncomingProtocol == IncomingMailProtocol.WMServer ? AdminPanelConstants.UserType.xma : AdminPanelConstants.UserType.wm));
                    }
                    else
                    {
                        xmDomains.Add(new XMDomain(dom.Name, dom.Name, dom.MailIncomingProtocol == IncomingMailProtocol.WMServer ? AdminPanelConstants.UserType.xma : AdminPanelConstants.UserType.wm));
                        if (_isServerExist &&dom.MailIncomingProtocol == IncomingMailProtocol.WMServer)
                        {
                            XMDomain.AddDomain(dom.Name);
                        }
                    }
                }

                foreach (XMDomain xmDomain in xmDomains)
                {
                    domTable.Rows.Add(xmDomain.Name, xmDomain.Name, xmDomain.Type);
                }
            }
            catch (Exception ex)
            {
                AdminPanelUtils.SetPageErrorMessage(this.Page, (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
                Log.WriteException(ex);
            }
        }
        return domTable;
    }

    void ShowDomains(DataTable domTable, int page, int pageSize, string orderBy, bool asc, string condition)
    {
        string HTMLDomains = string.Empty;
        string sortASC = (asc == true) ? "ASC" : "DESC";

        try
        {
            DataView view = domTable.DefaultView;
            view.Sort = OrderBy + " " + sortASC;
            if (condition.Trim() != string.Empty)
            {
                view.RowFilter = "name LIKE '%" + condition.Trim() + "%'";
            }
            domTable = view.ToTable();

            _domainsCount = domTable.Rows.Count;
            if ((page > (_domainsCount / _pageSize)) && SearchCondition.Trim() != string.Empty)
            {
                page = 1;
                AdminPanelUtils.SaveState("Page", page, Page.Session);
            }

            HFMaxSize.Value = _domainsCount.ToString();
            
            int maxSize = (_domainsCount - ((page - 1) * pageSize + pageSize) > 0) ? (page - 1) * pageSize + pageSize : _domainsCount;
            for (int i = (page - 1) * pageSize; i < maxSize; i++)
            {
                HTMLDomains += string.Format(@"
				<tr class=""wm_inbox_read_item"" id=""{0}_{1}"">
					<td style=""padding-left: 2px; padding-right: 2px;"" id=""none"">
						<input type=""checkbox"" />
					</td>
                    <td />
					<td class=""wm_inbox_from_subject"" style=""padding-left: 2px; padding-right: 2px;"">
                        {2}
					</td>
                    <td />
					<td class=""wm_inbox_from_subject"" style=""padding-left: 2px; padding-right: 2px;"">
						{3}
					</td>
				</tr>",
                domTable.Rows[i]["type"].ToString(), 
                HttpUtility.UrlEncode(domTable.Rows[i]["id"].ToString()),
                domTable.Rows[i]["type"].ToString() == AdminPanelConstants.UserType.wm ? @"<img src=""images/wm-domain-icon-big.png"" />" : @"<img src=""images/xmail-domain-icon-big.png"" />",
                domTable.Rows[i]["name"].ToString());
            }
            if (HTMLDomains == string.Empty)
            {
                HTMLDomains = @"
                <tr>
                    <td colspan=""5"">
                        <div class=""wm_inbox_info_message"">The result is empty</div>
                    </td>
                </tr>";
            }

            LiteralContactsGroups.Text = HTMLDomains;
        }
        catch (Exception error)
        {
            Log.WriteException(error);
            AdminPanelUtils.SetPageErrorMessage(this.Page, (error.InnerException != null ? error.InnerException.Message : error.Message));
        }
    }
   
}
