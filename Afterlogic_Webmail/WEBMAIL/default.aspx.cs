using System;
using System.Configuration;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;
using System.IO;

namespace WebMail
{
	/// <summary>
	/// Summary description for _default.
	/// </summary>
	public partial class _default : Page
	{
		protected WebmailResourceManager _resMan = null;

		protected const int POP3_PROTOCOL = 0;
		protected const int IMAP4_PROTOCOL = 1;
		protected const int WMSERVER_PROTOCOL = 2;
        protected string _WmVersion = string.Empty;

		protected string WmVersion = "500";
		protected string defaultTitle = string.Empty;
		protected string defaultSkin = string.Empty;
		protected string defaultLang = string.Empty;
        protected string defaultLangName = string.Empty;
        protected string defaultIncServer = string.Empty;
		protected int defaultIncProtocol = POP3_PROTOCOL;
		protected int defaultIncPort = 110;
		protected string defaultOutServer = string.Empty;
		protected int defaultOutPort = 25;
		protected bool defaultUseSmtpAuth = true;
		protected bool defaultSignMe = false;
		protected string defaultIsAjax = "true";

        protected string defaultAllowAdvancedLogin = "true";
		protected int defaultHideLoginMode = 1;
		protected string defaultDomainOptional = "localhost";

        protected string errorClass = string.Empty;
		protected string errorDesc = string.Empty;
		protected string mode = string.Empty;
		protected string switcherHref = string.Empty;
		protected string switcherText = string.Empty;

		protected string emailClass = string.Empty;
        protected string emailTabIndex = "1";
		protected string loginClass = string.Empty;
        protected string loginTabIndex = "2";
        protected string advancedDisplay = "none";
		protected string loginWidth = "224px";
		protected string domainContent = string.Empty;
	
		protected string advancedLogin = string.Empty;
		protected string pop3Selected = string.Empty;
		protected string imap4Selected = string.Empty;
		protected string wmserverSelected = string.Empty;
		protected string smtpAuthChecked = string.Empty;
		protected string signMeChecked = string.Empty;
		protected string globalEmail = string.Empty;
		protected string globalLogin = string.Empty;
		protected string globalPassword = string.Empty;
		protected string globalIncServer = string.Empty;
		protected string globalIncProtocol = string.Empty;
		protected string globalIncPort = string.Empty;
		protected string globalOutServer = string.Empty;
		protected string globalOutPort = string.Empty;
		protected string globalUseSmtpAuth = string.Empty;
		protected string globalSignMe = string.Empty;
		protected string globalAdvancedLogin = string.Empty;

		protected bool _stylesRtl = false;
		protected string _rtl = "false";
		protected string _languageOptions = "";
		protected string _languageClassName = "wm_hide";

		protected string WmServerToHTML = string.Empty;

		protected WebmailSettings settings = null;

		public _default()
		{
		}

		protected void Page_Load(object sender, EventArgs e)
		{
            if (File.Exists(Path.Combine(MapPath(""), "VERSION")))
            {
                _WmVersion = File.ReadAllText(Path.Combine(MapPath(""), "VERSION"));
            }

            if (!IsPostBack)
			{
                Log.Write("WebMail Version: " + _WmVersion + "\r\n");
                this.Context.Application.Remove(Constants.sessionSettings);
				if (Request.QueryString.Get("mode") != null)
				{
					mode = Request.QueryString.Get("mode");
				}
				if (mode == "logout")
				{
					Log.WriteLine("Page_Load", "Session Clear");
					Session.Clear();
				}
				if (Application[Constants.appSettingsDataFolderPath] == null)
				{
                    Application[Constants.appSettingsDataFolderPath] = ConfigurationManager.AppSettings[Constants.appSettingsDataFolderPath];
				}
				Session.Remove(Constants.sessionAccount);
			}
			Dictionary<string, string> supportedLangs = new Dictionary<string,string>();
            try
			{
                settings = (new WebMailSettingsCreator()).CreateWebMailSettings(Utils.GetDataFolderPath(), Request.Url.Host);
                supportedLangs = Utils.GetSupportedLangs();
                if (settings.AllowLanguageOnLogin)
				{
					foreach (KeyValuePair<string, string> kvp in supportedLangs)
					{
                        _languageOptions += @"<a href=""#"" name=""lng_" + kvp.Key + @""" onclick=""ChangeLang(this); return false;"">" + kvp.Value  + @"</a>";
					}
					_languageClassName = "";
				}
			}
            catch (WebMailDatabaseException)
            {
            }
			catch (WebMailSettingsException)
			{
				Response.Write(@"
<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"" />
<html>
<head>
    <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
	<link rel=""shortcut icon"" href=""favicon.ico"" />
    <meta http-equiv=""Content-Script-Type"" content=""text/javascript"" />
    <meta http-equiv=""Cache-Control"" content=""private,max-age=1209600"" />
    <title>WebMail probably not configured</title>
    <link rel=""stylesheet"" href=""skins/AfterLogic/styles.css"" type=""text/css"" />
</head>
<body>
<div align=""center"" id=""content"" class=""wm_content"">
    <div class=""wm_logo"" id=""logo"" tabindex=""-1""></div>
    <div class=""wm_login_error"">WebMail could not find data folder. Check SettingsPath in Web.config</div>
</div>
    <div class=""wm_copyright"" id=""copyright"">
		Powered by <a href=""http://www.afterlogic.com/mailbee/webmail-pro.asp"" target=""_blank""> AfterLogic WebMail Pro</a><br>
Copyright &copy; 2002-2010 <a href=""http://www.afterlogic.com"" target=""_blank"">AfterLogic Corporation</a>
	</div>
</body>
</html>
");
				Response.End();
				return;
			}

			try
			{
				defaultTitle = Utils.EncodeJsSaveString(settings.SiteName);

                defaultSkin = settings.DefaultSkin;
                string[] supportedSkins = Utils.GetSupportedSkins((Page != null) ? Page.MapPath("skins") : string.Empty);
                if (Utils.GetCurrentSkinIndex(supportedSkins, defaultSkin) < 0)
                {
                    if (supportedSkins.Length > 0)
                    {
                        defaultSkin = supportedSkins[0];
                    }
                }
                defaultSkin = Utils.EncodeJsSaveString(defaultSkin);
				
                if (Request["lang"] != null && supportedLangs.ContainsKey(Request["lang"]))
                {
                    defaultLang = Request["lang"];
                    HttpCookie defLangCookie = new HttpCookie("awm_defLang");
                    defLangCookie.Value = defaultLang;
                    defLangCookie.Path = HttpContext.Current.Request.ApplicationPath;
                    Response.AppendCookie(defLangCookie);
                }
                else if (Request.Cookies["awm_defLang"] != null && supportedLangs.ContainsKey(Request.Cookies["awm_defLang"].Value))
                {
                    defaultLang = Request.Cookies["awm_defLang"].Value;
                }
                if (string.IsNullOrEmpty(defaultLang))
                {
                    defaultLang = settings.DefaultLanguage;
                }
                defaultLangName = supportedLangs[defaultLang];
                _resMan = (new WebmailResourceManagerCreator()).CreateResourceManager(defaultLang);

                if (defaultLang == "Hebrew" || defaultLang == "Arabic")
                {
                    _stylesRtl = true;
                    _rtl = "true";
                }

                defaultIncServer = settings.IncomingMailServer;
											   
				switch(settings.IncomingMailProtocol)
				{
					case IncomingMailProtocol.Pop3:
					{
						defaultIncProtocol = POP3_PROTOCOL;
						break;
					}
					case IncomingMailProtocol.Imap4:
					{
						defaultIncProtocol = IMAP4_PROTOCOL;
						break;
					}
					case IncomingMailProtocol.WMServer:
					{
						defaultIncProtocol = WMSERVER_PROTOCOL;
						break;
					}
				}

				defaultIncPort = settings.IncomingMailPort;
				defaultOutServer = settings.OutgoingMailServer;
				defaultOutPort = settings.OutgoingMailPort;
				defaultUseSmtpAuth = settings.ReqSmtpAuth;
				defaultSignMe = false;
				defaultIsAjax = settings.AllowAjax ? "true" : "false";
				defaultAllowAdvancedLogin = (settings.AllowAdvancedLogin) ? "true" : "false";
				defaultHideLoginMode = (int)settings.HideLoginMode;
				defaultDomainOptional = Utils.EncodeJsSaveString(settings.DefaultDomainOptional);

				pop3Selected = (defaultIncProtocol == POP3_PROTOCOL) ? @" selected=""selected""" : "";
				imap4Selected = (defaultIncProtocol == IMAP4_PROTOCOL) ? @" selected=""selected""" : "";
				wmserverSelected = (defaultIncProtocol == WMSERVER_PROTOCOL) ? @" selected=""selected""" : "";

				smtpAuthChecked = defaultUseSmtpAuth ? @" checked=""checked""" : "";

				signMeChecked = defaultSignMe ? @" checked=""checked""" : "";

				//for version without ajax
				errorClass = "wm_hide"; //if there is no error
				errorDesc = "";
				mode = Request["mode"]; //mode = standard|advanced|submit
				switch (mode)
				{
					case "advanced":
						DisplayAdvancedMode();
						break;
					case "submit":
						DisplayStandardMode();
						globalEmail = Request["email"];
						globalLogin = Request["login"];
						globalPassword = Request["password"];
                        globalSignMe = Request["sign_me"];
                        globalAdvancedLogin = Request["advanced_login"];//0|1
                        if (globalAdvancedLogin == "1" || (int)settings.HideLoginMode < 20)
                        {
                        	if (!Validation.CheckIt(Validation.ValidationTask.Email, globalEmail))
                            {
                                errorDesc = _resMan.GetString(Validation.ErrorMessage);
                                errorClass = "wm_login_error";
                                break;
                            }
                        	globalEmail = Validation.Corrected;
                        }
						if (globalAdvancedLogin == "1" || (int)settings.HideLoginMode != 10 && (int)settings.HideLoginMode != 11)
                        {
                        	if (!Validation.CheckIt(Validation.ValidationTask.Login, globalLogin))
                            {
                                errorDesc = _resMan.GetString(Validation.ErrorMessage);
                                errorClass = "wm_login_error";
                                break;
                            }
                        	globalLogin = Validation.Corrected;
                        }
						if (!Validation.CheckIt(Validation.ValidationTask.Password, globalPassword))
                        {
                            errorDesc = _resMan.GetString(Validation.ErrorMessage);
                            errorClass = "wm_login_error";
                            break;
                        }

                        if (globalAdvancedLogin == "1")
                        {
                            globalIncServer = Request["inc_server"];
                            globalIncProtocol = Request["inc_protocol"];
                            globalIncPort = Request["inc_port"];
                            globalOutServer = Request["out_server"];
                            globalOutPort = Request["out_port"];
                            globalUseSmtpAuth = Request["smtp_auth"];
                            if (!Validation.CheckIt(Validation.ValidationTask.INServer, globalIncServer, globalAdvancedLogin))
                            {
                                errorDesc = _resMan.GetString(Validation.ErrorMessage);
                                errorClass = "wm_login_error";
                                break;
                            }
                        	globalIncServer = Validation.Corrected;
                        	if (!Validation.CheckIt(Validation.ValidationTask.INPort, globalIncPort, globalAdvancedLogin))
                            {
                                errorDesc = _resMan.GetString(Validation.ErrorMessage);
                                errorClass = "wm_login_error";
                                break;
                            }
                        	globalIncPort = Validation.Corrected;
                        	if (!Validation.CheckIt(Validation.ValidationTask.OUTServer, globalOutServer, globalAdvancedLogin))
                            {
                                errorDesc = _resMan.GetString(Validation.ErrorMessage);
                                errorClass = "wm_login_error";
                                break;
                            }
                        	globalOutServer = Validation.Corrected;
                        	if (!Validation.CheckIt(Validation.ValidationTask.OUTPort, globalOutPort, globalAdvancedLogin))
                            {
                                errorDesc = _resMan.GetString(Validation.ErrorMessage);
                                errorClass = "wm_login_error";
                                break;
                            }
                        	globalOutPort = Validation.Corrected;
                        }
                        else
                        {
                            globalIncServer = settings.IncomingMailServer;
                            globalIncProtocol = (settings.IncomingMailProtocol == IncomingMailProtocol.Imap4)
								? "1" : "0";
                            globalIncPort = settings.IncomingMailPort.ToString();
                            globalOutServer = settings.OutgoingMailServer;
                            globalOutPort = settings.OutgoingMailPort.ToString();
                            globalUseSmtpAuth = settings.ReqSmtpAuth ? "1" : "0";
                        }


						try
						{
							bool advancedLogin = (string.Compare(globalAdvancedLogin, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
							bool signAuto = (string.Compare(globalSignMe, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
							bool useSmtpAuth = (string.Compare(globalUseSmtpAuth, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
							string language = Request["language"];
							Account acct = Account.LoginAccount(globalEmail, globalLogin, globalPassword, 
								globalIncServer, (IncomingMailProtocol) Convert.ToInt32(globalIncProtocol), 
								Convert.ToInt32(globalIncPort), globalOutServer, Convert.ToInt32(globalOutPort),
								useSmtpAuth, signAuto, advancedLogin, language);

                            if (acct != null)
                            {
                                if (globalSignMe == "1")
                                {
                                    HttpCookie idAcctCookie = new HttpCookie("awm_autologin_id");
                                    idAcctCookie.Value = acct.ID.ToString();
                                    idAcctCookie.Path = HttpContext.Current.Request.ApplicationPath;
                                    Response.AppendCookie(idAcctCookie);

                                    string hash = Utils.GetMD5DigestHexString(Utils.EncodePassword(acct.MailIncomingPassword));
                                    HttpCookie autoLoginCookie = new HttpCookie("awm_autologin_data");
                                    autoLoginCookie.Value = hash;
                                    autoLoginCookie.Path = HttpContext.Current.Request.ApplicationPath;
                                    Response.AppendCookie(autoLoginCookie);
                                }

                                Session.Add(Constants.sessionAccount, acct);
                                if (acct.GetMailAtLogin)
                                {
                                    Response.Redirect("basewebmail.aspx?check=1", false);
                                }
                                else
                                {
                                    Response.Redirect("basewebmail.aspx", false);
                                }
                            }
						}
						catch(Exception message)
						{
							Log.WriteLine("Page_Load", "Login Account");
							Log.WriteException(message);
							errorDesc = message.Message;
							errorClass = "wm_login_error"; //if the error was occured
						}
						break;
					default:
						DisplayStandardMode();
						break;
				}
				//end for version without ajax

				string error = Request["error"] ?? string.Empty;
				if (error == "1") 
				{
					errorDesc = "The previous session was terminated due to a timeout.";
					errorClass = "wm_login_error"; //if the error was occured
				} 
				else 
				{
					HttpCookie idAcctCookie = Request.Cookies["awm_autologin_id"];
					HttpCookie autoLoginCookie = Request.Cookies["awm_autologin_data"];
					if ((idAcctCookie != null) && (autoLoginCookie != null))
					{
						int id_acct = -1;
						try
						{
							id_acct = int.Parse(idAcctCookie.Value);
						}
						catch (Exception ex)
						{
							Log.WriteLine("Page_Load", "int.Parse");
							Log.WriteException(ex);
						}
						Account acct = Account.LoadFromDb(id_acct, -1, false);

						if (acct != null)
						{
							string encodePassword = Utils.GetMD5DigestHexString(Utils.EncodePassword(acct.MailIncomingPassword));
							if (string.Compare(encodePassword, autoLoginCookie.Value, true, CultureInfo.InvariantCulture) == 0)
							{
								Session.Add(Constants.sessionAccount, acct);
								idAcctCookie.Expires = DateTime.Now.AddDays(14);
								if (settings.AllowAjax)
								{
									Response.Redirect("webmail.aspx?check=1", false);
								}
								else
								{
									Response.Redirect("basewebmail.aspx?check=1", false);
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Log.WriteException(ex);
			}
		}

		protected void DisplayStandardMode()
		{
			switcherHref = "?mode=advanced";
			switcherText = _resMan.GetString("AdvancedLogin");
			advancedDisplay = @"none";
			advancedLogin = "0";
			if (settings.HideLoginMode == LoginMode.HideEmailField ||
				settings.HideLoginMode == LoginMode.HideEmailFieldDisplayDomainAfterLogin ||
				settings.HideLoginMode == LoginMode.HideEmailFieldDisplayDomainAfterLoginAndLoginIsLoginAndDomain ||
				settings.HideLoginMode == LoginMode.HideEmailFieldLoginIsLoginAndDomain)
			{
				emailClass = @" class=""wm_hide""";
                emailTabIndex = "-1";
			}
			if (settings.HideLoginMode == LoginMode.HideLoginFieldLoginIsAccount ||
				settings.HideLoginMode == LoginMode.HideLoginFieldLoginIsEmail)
			{
				loginClass = @" class=""wm_hide""";
                loginTabIndex = "-1";
			}
			if (settings.HideLoginMode == LoginMode.HideEmailFieldDisplayDomainAfterLogin ||
				settings.HideLoginMode == LoginMode.HideEmailFieldDisplayDomainAfterLoginAndLoginIsLoginAndDomain)
			{
				loginWidth = "150px";
				domainContent = "&nbsp;@" + defaultDomainOptional;
			}
		}

		protected void DisplayAdvancedMode()
		{
			switcherHref = "?mode=standard";
			switcherText = _resMan.GetString("StandardLogin");
			advancedDisplay = "block";
			advancedLogin = "1";
			emailClass = "";
			loginClass = "";
			loginWidth = "224px";
			domainContent = "";
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
