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

public partial class PlugIns_Upgrade_main : AdminPanelMainPlugin
    {
        protected UpgradeWebMail UpgradeWebMailID;
        protected UpgradeXMail UpgradeXMailID;

        protected PlugIns_Upgrade_main()
        {
            SetPluginID(AdminPanelConstants.PluginName.Upgrade);
        }

        public override bool CanLoadPlugin()
        {
            return true;
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Plugin.IsPluginExist(AdminPanelConstants.PluginName.Server, (Page as DefaultPage).Settings) &&
                Plugin.IsPluginExist(AdminPanelConstants.PluginName.WebMailLite, (Page as DefaultPage).Settings))
            {
                UpgradeXMail UpgradeXMailID = (UpgradeXMail)LoadControl(@"Controls/XMail.ascx");
                UpgradeContent.Controls.Add(UpgradeXMailID);
            }
            else if (Plugin.IsPluginExist(AdminPanelConstants.PluginName.WebMailLite, (Page as DefaultPage).Settings))
            {
                UpgradeWebMail UpgradeWebMailID = (UpgradeWebMail)LoadControl(@"Controls/WebMail.ascx");
                UpgradeContent.Controls.Add(UpgradeWebMailID);
            }

        }
    }
