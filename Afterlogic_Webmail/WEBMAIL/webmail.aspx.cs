using System;
using System.Web.UI;
using System.IO;
using MailBee.Mime;
using System.Globalization;

namespace WebMail
{
	/// <summary>
	/// Summary description for webmail.
	/// </summary>
	public partial class webmail : Page
	{
		protected WebmailResourceManager _resMan = null;

		protected string defaultTitle = "AfterLogic WebMail Pro 5.0";
		protected string defaultSkin = Constants.defaultSkinName;
		protected string defaultLang = Constants.defaultLang;
		protected string WmVersion = "500";
        protected string _WmVersion = String.Empty;
		protected string check = "0";
		protected string start = "0";
		protected string to = string.Empty;
        Account acct = null;

		protected string jsClearDefaultTitle = string.Empty;
		protected string jsClearDefaultSkin = string.Empty;
		protected string jsClearStart = string.Empty;
		protected string jsClearToAddr = string.Empty;
		protected bool _stylesRtl = false;
		protected string _rtl = "false";
        protected string CSType = "false";
        protected string hMailServer = "false";

		protected void Page_Load(object sender, EventArgs e)
		{
            if (File.Exists(Path.Combine(MapPath(""), "VERSION")))
            {
                _WmVersion = File.ReadAllText(Path.Combine(MapPath(""), "VERSION"));
            }
            
            HMailServer hmailserver = new HMailServer();
            if (hmailserver.IsLoaded)
            {
                hMailServer = "true";
            }
            
            acct = (Account)Session[Constants.sessionAccount];
            if (acct == null)
            {
                Response.Redirect("default.aspx?error=1", true);
            }

            if (File.Exists(Path.Combine(Page.MapPath(""), "cs")))
            {
                CSType = "true";
            }
            
            if (Request.QueryString.Get("check") != null)
			{
				check = Request.QueryString.Get("check");
			}
			if (Request.QueryString.Get("start") != null)
			{
				start = Request.QueryString.Get("start");
			}
			if (Request.QueryString.Get("to") != null)
			{
                to = Request.QueryString.Get("to");
			}
			jsClearStart = Utils.EncodeJsSaveString(start);
			jsClearToAddr = Utils.EncodeJsSaveString(to);
			_resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();

			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();

            if (acct != null)
			{
				if ((acct.UserOfAccount != null) && (acct.UserOfAccount.Settings != null))
				{
					if (acct.UserOfAccount.Settings.RTL)
					{
						_stylesRtl = true;
						_rtl = "true";
					}
					defaultTitle = settings.SiteName;

                    defaultSkin = acct.UserOfAccount.Settings.DefaultSkin;
                    string[] supportedSkins = Utils.GetSupportedSkins((Page != null) ? Page.MapPath("skins") : string.Empty);
                    if (Utils.GetCurrentSkinIndex(supportedSkins, defaultSkin) < 0)
                    {
                        if (supportedSkins.Length > 0)
                        {
                            defaultSkin = supportedSkins[0];
                        }
                    }

					defaultLang = acct.UserOfAccount.Settings.DefaultLanguage;
					jsClearDefaultTitle = Utils.EncodeJsSaveString(defaultTitle);
					jsClearDefaultSkin = Utils.EncodeJsSaveString(defaultSkin);
					return;
				}
			}
			defaultTitle = settings.SiteName;
			defaultSkin = settings.DefaultSkin;
			defaultLang = settings.DefaultLanguage;
			jsClearDefaultTitle = Utils.EncodeJsSaveString(defaultTitle);
			jsClearDefaultSkin = Utils.EncodeJsSaveString(defaultSkin);
		}
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion
	}
}
