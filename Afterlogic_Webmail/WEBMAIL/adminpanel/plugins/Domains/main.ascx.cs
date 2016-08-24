using System;
using System.Web;
using System.Web.UI;
using System.Globalization;
using XMailAdminProxy;
using WebMail;

public partial class PlugIns_Domains_main : AdminPanelMainPlugin
{
    protected PlugIns_Domains_main()
    {
        SetPluginID(AdminPanelConstants.PluginName.Domains);
    }

    public override bool CanLoadPlugin()
    {
        bool result = false;
        if (Plugin.IsPluginExist(MapPath("../../"), AdminPanelConstants.PluginName.WebMailLite))
        {
            result = true;
        }
        else
        {
            result = AdminPanelUtils.ValidationLicenseKey((Page as DefaultPage).Settings.LicenseKey);
        }
        return result;
    }
    
    protected void Page_Load(object sender, EventArgs e)
	{
        (Page as DefaultPage).Body = " onresize=\"ResizeElements('all');\"";

        AdminPanelUtils.ShowReportAndReportMessages(Page);
        bool result = CheckDB();

        if (result)
        {
            string mode = (!string.IsNullOrEmpty(Request.QueryString["mode"]) ? Request.QueryString["mode"] : "domains");
            domains_management domainsControl = (domains_management)LoadControl(@"domains.ascx");

            if (domainsControl != null)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["asc"]))
                {
                    domainsControl.Asc = (string.Compare(Request.QueryString["asc"], "true", true, CultureInfo.InvariantCulture) == 0) ? true : false;
                }
                if (!string.IsNullOrEmpty(Request.QueryString["order"]))
                {
                    domainsControl.OrderBy = Request.QueryString["order"];
                }
                ContentPlaceHolder.Controls.Add(domainsControl);
            }

            try
            {
                switch (mode)
                {
                    case "new":
                        if (AdminPanelUtils.IsSuperAdmin(Page.Session, (Page as DefaultPage).Settings))
                        {
                            Control ctrl = LoadControl(@"domain-edit.ascx");
                            if (ctrl != null)
                            {
                                domainsControl.FindControl("PlaceHolderDomain").Controls.Add(ctrl);
                            }
                        }
                        break;
                    case "edit":
                        {
                            if ((!string.IsNullOrEmpty(Request.QueryString["name"])) || (!string.IsNullOrEmpty(Request.QueryString["uid"])))
                            {
                                Control ctrl = LoadControl(@"domain-edit.ascx");
                                if (ctrl != null)
                                {
                                    domainsControl.FindControl("PlaceHolderDomain").Controls.Add(ctrl);
                                }
                            }
                        }
                        break;
                    case "close_search":
                        {
                            AdminPanelUtils.SaveState("wm_adm_dm_condition", null, Page.Session);
                            Response.Redirect(string.Format(@"default.aspx?mode=domains"), false);
                        }
                        break;
                }
                if (Page.IsPostBack)
                {
                    AdminPanelUtils.SaveState("Page", Request[domainsControl.FindControl("HFPageInfo").UniqueID].ToString(), Page.Session);
                    string action = Request[domainsControl.FindControl("HFAction").UniqueID].ToString();
                    string value = Request[domainsControl.FindControl("HFValue").UniqueID].ToString();

                    switch (action)
                    {
                        case "sort":
                            if (value != "")
                            {
                                bool sortOrder = true;
                                AdminPanelUtils.SaveState("wm_adm_dm_order", value, Page.Session);
                                if (Session["wm_adm_dmAsc"] != null)
                                {
                                    sortOrder = bool.Parse(Session["wm_adm_dmAsc"].ToString());
                                }
                                Session["wm_adm_dmAsc"] = !sortOrder;
                            }
                            Response.Redirect(Request.RawUrl, false);
                            break;
                        case "search":
                            AdminPanelUtils.SaveState("wm_adm_dm_condition", value, Page.Session);
                            Response.Redirect(string.Format(@"default.aspx?mode=domains"), false);
                            break;
                        case "delete":
                            if (AdminPanelUtils.IsSuperAdmin(Page.Session, (Page as DefaultPage).Settings))
                            {
                                try
                                {
                                    string[] values = value.Split(',');
                                    foreach (string val in values)
                                    {
                                        SelectedValueStruct sds = AdminPanelUtils.ParseSelectedValue(HttpUtility.UrlDecode(val));
                                        DeleteDomain(sds.Name, sds.Type);
                                        if (AdminPanelUtils.LoadState("selectedDomain", Page.Session) != null &&
                                            AdminPanelUtils.LoadState("selectedDomain", Page.Session).ToString() == val)
                                        {
                                            AdminPanelUtils.SaveState("selectedDomain", null, Page.Session);
                                        }
                                    }
                                    AdminPanelUtils.SaveState("SessPageReportMessage", WebMail.Constants.mailAdmUpdateAccountSuccess, Page.Session);
                                }
                                catch
                                {
                                    AdminPanelUtils.SaveState("SessPageErrorMessage", WebMail.Constants.mailAdmUpdateAccountUnsuccess, Page.Session);
                                }
                                Response.Redirect(string.Format(@"default.aspx?mode=domains"), false);
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                AdminPanelUtils.SetPageErrorMessage(this.Page, (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
            }
            if (AdminPanelUtils.LoadState("Page", Page.Session) != null)
            {
                domainsControl.PageNumber = Int32.Parse(AdminPanelUtils.LoadState("Page", Page.Session).ToString());
            }
        }
        else
        {
            AdminPanelUtils.SetPageErrorMessage(this.Page, "<b>Database is not configured</b>. <br />Before creating domains and users, you need to configure database connection first. <br />Please navigate WebMail / Database Settings and configure database connection there.");
        }
 	}

    public void DeleteDomain(string name, string type)
    {
        switch (type)
        {
            case AdminPanelConstants.UserType.xm:
                XMDomain.DeleteDomain(name);
                break;
            case AdminPanelConstants.UserType.xmc:
                XMDomain.DeleteCustomDomain(name);
                break;
            case AdminPanelConstants.UserType.wm:
                this.DeleteWebMailDomain(name);
                break;
            case AdminPanelConstants.UserType.xma:
                XMDomain.DeleteDomain(name);
                this.DeleteWebMailDomain(name);
                break;
            default:
                break;
        }
    }    

    public void DeleteWebMailDomain(string name)
    {
        Domain dom = new Domain();
        if (!string.IsNullOrEmpty(name))
        {
            try
            {
                dom = Domain.LoadFromDb(name);
                if (dom != null)
                {
                    dom.Delete();
                    Account.UpdateAccountsByDomain(dom.Name, dom.ID, 0, -1);
                }
            }
            catch (WebMailException ex)
            {
                Log.WriteException(ex);
                AdminPanelUtils.SetPageErrorMessage(Page, ex.Message);
            }
        }
    }

    public bool CheckDB()
    {
        bool result = true;
        if (Plugin.IsPluginExist(AdminPanelConstants.PluginName.WebMail, (Page as DefaultPage).Settings) ||
            Plugin.IsPluginExist(AdminPanelConstants.PluginName.WebMailLite, (Page as DefaultPage).Settings))
        {
            WebmailSettings wmSettings = new WebMailSettingsCreator().CreateWebMailSettings(AdminPanelUtils.GetWebMailDataFolder());

            if (!wmSettings.UseCustomConnectionString)
            {
                switch (wmSettings.DbType)
                {
                    case SupportedDatabase.MsSqlServer:
                    case SupportedDatabase.MySql:
                        if (string.IsNullOrEmpty(wmSettings.DbDsn.Trim()))
                        {
                            if (string.IsNullOrEmpty(wmSettings.DbLogin.Trim()))
                            {
                                result = false;
                            }
                        }
                        break;
                    case SupportedDatabase.MsAccess:
                        if (string.IsNullOrEmpty(wmSettings.DbPathToMdb.Trim()))
                        {
                            result = false;
                        }
                        break;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(wmSettings.DbCustomConnectionString.Trim()))
                {
                    result = false;
                }
            }
        }
        return result;
    }

}
