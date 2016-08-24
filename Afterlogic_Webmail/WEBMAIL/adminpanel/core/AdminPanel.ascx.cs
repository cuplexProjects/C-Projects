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
using System.IO;
using System.Text;

namespace WebMail.adminpanel
{
    public partial class AdminPanel : System.Web.UI.UserControl
    {
        protected string pluginID = string.Empty;
        protected PluginCollection installedPlugins = new PluginCollection();
        protected string rootFolder;

        protected void Page_Load(object sender, EventArgs e)
        {
            pluginID = LoadPluginID();

            rootFolder = Page.MapPath("");

            if (!Page.IsPostBack)
            {
                pluginID = (!string.IsNullOrEmpty(Request.QueryString["plugin"]) ? Request.QueryString["plugin"] : pluginID);
            }
            try
            {
                PluginCollection Plugins = Plugin.GetPlugins(rootFolder, Page.Session, (Page as DefaultPage).Settings);

                installedPlugins.Clear();
                foreach (Plugin pl in Plugins)
                {
                    AdminPanelMainPlugin pctrl = (AdminPanelMainPlugin)LoadControl("../plugins/" + pl.FolderName + "/main.ascx");
                    pctrl.InitPlugin();
                    if (pctrl.CanLoadPlugin())
                    {
                        installedPlugins.Add(pl);
                    }
                }

                string mode = (!string.IsNullOrEmpty(Request.QueryString["mode"]) ? Request.QueryString["mode"] : string.Empty);
                if (!Page.IsPostBack)
                {
                    switch (mode)
                    {
                        case "help":
                            Response.Redirect( (Page as DefaultPage).HelpUrl, false);
                            break;
                    }
                }

                if (string.IsNullOrEmpty(pluginID) && installedPlugins.Count > 0)
                {
                    pluginID = installedPlugins[0].ID;
                }
                Plugin plugin = LoadPlugin(pluginID);
                if (plugin != null)
                {
                    SavePluginID(plugin);
                }
                else
                {
                    AdminPanelUtils.SetPageErrorMessage(Page, "Plugin not found!");
                }

                WebMail.adminpanel.core.Menu ctrl = (WebMail.adminpanel.core.Menu)LoadControl("Menu.ascx");
                if (ctrl != null)
                {
                    ctrl.Plugins = installedPlugins;
                    ctrl.pluginID = pluginID;
                    menuPlaceHolder.Controls.Add(ctrl);
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                AdminPanelUtils.SetPageErrorMessage(Page, ex.Message + ": " + (ex.InnerException != null ? ex.InnerException.Message : ex.Message));
            }
        }

        protected Plugin LoadPlugin(string pID)
        {
            Plugin result = null;
            if (pID == string.Empty)
            {
                if (installedPlugins.Count > 0)
                {
                    result = installedPlugins[0];
                }
            }
            else
            {
                result = installedPlugins[pID];
            }

            if (result != null)
            {
                if (Plugin.IsPluginExist(rootFolder, result.FolderName))
                {
                    AdminPanelMainPlugin plugin = (AdminPanelMainPlugin)LoadControl(string.Format("../Plugins/{0}/main.ascx", result.FolderName));

                    if (plugin != null && plugin.PluginID == result.ID && plugin.CanLoadPlugin())
                    {
                        plugin.ID = string.Format("PluginContentID{0}", pluginID);
                        contentPlaceHolder.Controls.Add(plugin);
                        (Page as DefaultPage).PageTitle = result.Caption;
                    }
                    else
                    {
                        result = null;
                    }
                }
            }
            return result;
        }

        protected void SavePluginID(Plugin plugin)
        {
            if (string.IsNullOrEmpty(plugin.Target))
            {
                Session["pluginID"] = plugin.ID;
            }
        }

        protected string LoadPluginID()
        {
            string result = string.Empty;
            if (Session["pluginID"] != null)
            {
                result = Session["pluginID"].ToString();
            }
            return result;
        }
    }
}