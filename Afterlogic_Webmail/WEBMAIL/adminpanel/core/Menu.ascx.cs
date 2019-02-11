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
using System.Text;
using System.ComponentModel;

namespace WebMail.adminpanel.core
{
    public partial class Menu : System.Web.UI.UserControl
    {
        public PluginCollection Plugins = new PluginCollection();
        public string pluginID;

        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder result = new StringBuilder();

            foreach (Plugin pl in Plugins)
            {

                result.AppendFormat(@"
                    <div id=""PluginHeaderID{0}"" class=""wm_accountslist_contacts{1}{4}"">
                        <a href=""default.aspx?plugin={2}"" {5}>{3}</a>
                    </div>
                    ",
                     pl.ID, (pluginID == pl.ID) ? " wm_active_tab" : "",
                     pl.ID, pl.Caption, (Plugins.IndexOf(pl) == 0) ? " first" : "",
                     !string.IsNullOrEmpty(pl.Target) ? "target=" + pl.Target : "");
            }
            PluginsMenu.Text = result.ToString();
        }
    }
}