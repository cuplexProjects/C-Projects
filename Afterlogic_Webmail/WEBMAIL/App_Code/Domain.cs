using System;
using System.Collections;
namespace WebMail
{
    public class Domain
    {
        #region Fields
        protected int _id;
        protected string _name;
        protected IncomingMailProtocol _mailProtocol;
        protected string _mailIncHost;
        protected int _mailIncPort;
        protected string _mailOutHost;
        protected int _mailOutPort;
        protected bool _mailOutAuthentication;

        private string _url;
        private string _site_name;
        private IncomingMailProtocol _settings_mail_protocol;
        private string _settings_mail_inc_host;
        private int _settings_mail_inc_port;
        private string _settings_mail_out_host;
        private int _settings_mail_out_port;
        private bool _settings_mail_out_auth;
        private bool _allow_direct_mode;
        private bool _direct_mode_id_def;
        private long _attachment_size_limit;
        private bool _allow_attachment_limit;
        private long _mailbox_size_limit;
        private bool _allow_mailbox_limit;
        private bool _take_quota;
        private bool _allow_new_users_change_settings;
        private bool _allow_auto_reg_on_login;
        private bool _allow_users_add_accounts;
        private bool _allow_users_change_account_def;
        private int _def_user_charset;
        private bool _allow_users_change_charset;
        private int _def_user_timezone;
        private bool _allow_users_change_timezone;
        private int _msgs_per_page;
        private string _skin;
        private bool _allow_users_change_skin;
        private string _lang;
        private bool _allow_users_change_lang;
        private bool _show_text_labels;
        private bool _allow_ajax;
        private bool _allow_editor;
        private bool _allow_contacts;
        private bool _allow_calendar;
        private LoginMode _hide_login_mode;
        private string _domain_to_use;
        private bool _allow_choosing_lang;
        private bool _allow_advanced_login;
        private bool _allow_auto_detect_and_correct;
		private bool _global_addr_book;
        private ViewMode _viewmode;
        private SaveMail _save_mail;
        #endregion

        #region Properties
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public IncomingMailProtocol MailIncomingProtocol
        {
            get { return _mailProtocol; }
            set { _mailProtocol = value; }
        }

        public string MailIncomingHost
        {
            get { return _mailIncHost; }
            set { _mailIncHost = value; }
        }

        public int MailIncomingPort
        {
            get { return _mailIncPort; }
            set { _mailIncPort = value; }
        }

        public string MailOutgoingHost
        {
            get { return _mailOutHost; }
            set { _mailOutHost = value; }
        }

        public int MailOutgoingPort
        {
            get { return _mailOutPort; }
            set { _mailOutPort = value; }
        }

        public bool MailOutgoingAuthentication
        {
            get { return _mailOutAuthentication; }
            set { _mailOutAuthentication = value; }
        }

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        public string Site_name
        {
            get { return _site_name; }
            set { _site_name = value; }
        }

        public IncomingMailProtocol Settings_mail_protocol
        {
            get { return _settings_mail_protocol; }
            set { _settings_mail_protocol = value; }
        }

        public string Settings_mail_inc_host
        {
            get { return _settings_mail_inc_host; }
            set { _settings_mail_inc_host = value; }
        }

        public int Settings_mail_inc_port
        {
            get { return _settings_mail_inc_port; }
            set { _settings_mail_inc_port = value; }
        }

        public string Settings_mail_out_host
        {
            get { return _settings_mail_out_host; }
            set { _settings_mail_out_host = value; }
        }

        public int Settings_mail_out_port
        {
            get { return _settings_mail_out_port; }
            set { _settings_mail_out_port = value; }
        }

        public bool Settings_mail_out_auth
        {
            get { return _settings_mail_out_auth; }
            set { _settings_mail_out_auth = value; }
        }

        public bool Allow_direct_mode
        {
            get { return _allow_direct_mode; }
            set { _allow_direct_mode = value; }
        }

        public bool Direct_mode_id_def
        {
            get { return _direct_mode_id_def; }
            set { _direct_mode_id_def = value; }
        }

        public long Attachment_size_limit
        {
            get { return _attachment_size_limit; }
            set { _attachment_size_limit = value; }
        }

        public bool Allow_attachment_limit
        {
            get { return _allow_attachment_limit; }
            set { _allow_attachment_limit = value; }
        }

        public long Mailbox_size_limit
        {
            get { return _mailbox_size_limit; }
            set { _mailbox_size_limit = value; }
        }

        public bool Allow_mailbox_limit
        {
            get { return _allow_mailbox_limit; }
            set { _allow_mailbox_limit = value; }
        }

        public bool Take_quota
        {
            get { return _take_quota; }
            set { _take_quota = value; }
        }

        public bool Allow_new_users_change_settings
        {
            get { return _allow_new_users_change_settings; }
            set { _allow_new_users_change_settings = value; }
        }

        public bool Allow_auto_reg_on_login
        {
            get { return _allow_auto_reg_on_login; }
            set { _allow_auto_reg_on_login = value; }
        }

        public bool Allow_users_add_accounts
        {
            get { return _allow_users_add_accounts; }
            set { _allow_users_add_accounts = value; }
        }

        public bool Allow_users_change_account_def
        {
            get { return _allow_users_change_account_def; }
            set { _allow_users_change_account_def = value; }
        }

        public int Def_user_charset
        {
            get { return _def_user_charset; }
            set { _def_user_charset = value; }
        }

        public bool Allow_users_change_charset
        {
            get { return _allow_users_change_charset; }
            set { _allow_users_change_charset = value; }
        }

        public int Def_user_timezone
        {
            get { return _def_user_timezone; }
            set { _def_user_timezone = value; }
        }

        public bool Allow_users_change_timezone
        {
            get { return _allow_users_change_timezone; }
            set { _allow_users_change_timezone = value; }
        }

        public int Msgs_per_page
        {
            get { return _msgs_per_page; }
            set { _msgs_per_page = value; }
        }

        public string Skin
        {
            get { return _skin; }
            set { _skin = value; }
        }

        public bool Allow_users_change_skin
        {
            get { return _allow_users_change_skin; }
            set { _allow_users_change_skin = value; }
        }

        public string Lang
        {
            get { return _lang; }
            set { _lang = value; }
        }

        public bool Allow_users_change_lang
        {
            get { return _allow_users_change_lang; }
            set { _allow_users_change_lang = value; }
        }

        public bool Show_text_labels
        {
            get { return _show_text_labels; }
            set { _show_text_labels = value; }
        }

        public bool Allow_ajax
        {
            get { return _allow_ajax; }
            set { _allow_ajax = value; }
        }

        public bool Allow_editor
        {
            get { return _allow_editor; }
            set { _allow_editor = value; }
        }

        public bool Allow_contacts
        {
            get { return _allow_contacts; }
            set { _allow_contacts = value; }
        }

        public bool Allow_calendar
        {
            get { return _allow_calendar; }
            set { _allow_calendar = value; }
        }

        public LoginMode Hide_login_mode
        {
            get { return _hide_login_mode; }
            set { _hide_login_mode = value; }
        }

        public string Domain_to_use
        {
            get { return _domain_to_use; }
            set { _domain_to_use = value; }
        }

        public bool Allow_choosing_lang
        {
            get { return _allow_choosing_lang; }
            set { _allow_choosing_lang = value; }
        }

        public bool Allow_advanced_login
        {
            get { return _allow_advanced_login; }
            set { _allow_advanced_login = value; }
        }

        public bool Allow_auto_detect_and_correct
        {
            get { return _allow_auto_detect_and_correct; }
            set { _allow_auto_detect_and_correct = value; }
        }

        public bool Global_addr_book
		{
			get { return _global_addr_book; }
			set { _global_addr_book = value; }
		}
        
        public ViewMode ViewMode
        {
            get { return _viewmode; }
            set { _viewmode = value; }
        }

        public SaveMail SaveMail
        {
            get { return _save_mail; }
            set { _save_mail = value; }
        }

        #endregion

        public Domain()
        {
            WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
            _id = -1;
            _name = "";
            _mailProtocol = settings.IncomingMailProtocol;
            _mailIncHost = settings.IncomingMailServer;
            _mailIncPort = settings.IncomingMailPort;
            _mailOutHost = settings.OutgoingMailServer;
            _mailOutPort = settings.OutgoingMailPort;
            _mailOutAuthentication = settings.ReqSmtpAuth;

            _url = "";
            _site_name = settings.SiteName;
            _settings_mail_protocol = settings.IncomingMailProtocol;
            _settings_mail_inc_host = settings.IncomingMailServer;
            _settings_mail_inc_port = settings.IncomingMailPort;
            _settings_mail_out_host = settings.OutgoingMailServer;
            _settings_mail_out_port = settings.OutgoingMailPort;
            _settings_mail_out_auth = settings.ReqSmtpAuth;
            _allow_direct_mode = settings.AllowDirectMode;
            _direct_mode_id_def = settings.DirectModeIsDefault;
            _attachment_size_limit = settings.AttachmentSizeLimit;
            _allow_attachment_limit = settings.EnableAttachmentSizeLimit;
            _mailbox_size_limit = settings.MailboxSizeLimit;
            _allow_mailbox_limit = settings.EnableMailboxSizeLimit;
            _take_quota = settings.TakeImapQuota;
            _allow_new_users_change_settings = settings.AllowUsersChangeEmailSettings;
            _allow_auto_reg_on_login = settings.AllowNewUsersRegister;
            _allow_users_add_accounts = settings.AllowUsersAddNewAccounts;
            _allow_users_change_account_def = settings.AllowUsersChangeAccountsDef;
            _def_user_charset = settings.DefaultUserCharset;
            _allow_users_change_charset = settings.AllowUsersChangeCharset;
            _def_user_timezone = settings.DefaultTimeZone;
            _allow_users_change_timezone = settings.AllowUsersChangeTimeZone;
            _msgs_per_page = settings.MailsPerPage;
            _skin = settings.DefaultSkin;
            _allow_users_change_skin = settings.AllowUsersChangeSkin;
            _lang = settings.DefaultLanguage;
            _allow_users_change_lang = settings.AllowUsersChangeLanguage;
            _show_text_labels = settings.ShowTextLabels;
            _allow_ajax = settings.AllowAjax;
            _allow_editor = settings.AllowDhtmlEditor;
            _allow_contacts = settings.AllowContacts;
            _allow_calendar = settings.AllowCalendar;
            _hide_login_mode = settings.HideLoginMode;
            _domain_to_use = settings.DefaultDomainOptional;
            _allow_choosing_lang = settings.AllowLanguageOnLogin;
            _allow_advanced_login = settings.AllowAdvancedLogin;
            _allow_auto_detect_and_correct = settings.AutomaticCorrectLoginSettings;
			_global_addr_book = false;
            _viewmode = settings.ViewMode;
            _save_mail = SaveMail.Always;
        }

        public Domain(int id_account, string name, IncomingMailProtocol mail_protocol,
                      string mail_inc_host, int mail_inc_port, string mail_out_host,
                      int mail_out_port, bool mail_out_auth)
        {
            _id = id_account;
            _name = name;
            _mailProtocol = mail_protocol;
            _mailIncHost = mail_inc_host;
            _mailIncPort = mail_inc_port;
            _mailOutHost = mail_out_host;
            _mailOutPort = mail_out_port;
            _mailOutAuthentication = mail_out_auth;

            WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();

            _url = string.Empty;
            _site_name = settings.SiteName;
            _settings_mail_protocol = settings.IncomingMailProtocol;
            _settings_mail_inc_host = settings.IncomingMailServer;
            _settings_mail_inc_port = settings.IncomingMailPort;
            _settings_mail_out_host = settings.OutgoingMailServer;
            _settings_mail_out_port = settings.OutgoingMailPort;
            _settings_mail_out_auth = settings.ReqSmtpAuth;
            _allow_direct_mode = settings.AllowDirectMode;
            _direct_mode_id_def = settings.DirectModeIsDefault;
            _attachment_size_limit = settings.AttachmentSizeLimit;
            _allow_attachment_limit = settings.EnableAttachmentSizeLimit;
            _mailbox_size_limit = settings.MailboxSizeLimit;
            _allow_mailbox_limit = settings.EnableMailboxSizeLimit;
            _take_quota = settings.TakeImapQuota;
            _allow_new_users_change_settings = settings.AllowUsersChangeEmailSettings;
            _allow_auto_reg_on_login = settings.AllowNewUsersRegister;
            _allow_users_add_accounts = settings.AllowUsersAddNewAccounts;
            _allow_users_change_account_def = settings.AllowUsersChangeAccountsDef;
            _def_user_charset = settings.DefaultUserCharset;
            _allow_users_change_charset = settings.AllowUsersChangeCharset;
            _def_user_timezone = settings.DefaultTimeZone;
            _allow_users_change_timezone = settings.AllowUsersChangeTimeZone;
            _msgs_per_page = settings.MailsPerPage;
            _skin = settings.DefaultSkin;
            _allow_users_change_skin = settings.AllowUsersChangeSkin;
            _lang = settings.DefaultLanguage;
            _allow_users_change_lang = settings.AllowUsersChangeLanguage;
            _show_text_labels = settings.ShowTextLabels;
            _allow_ajax = settings.AllowAjax;
            _allow_editor = settings.AllowDhtmlEditor;
            _allow_contacts = settings.AllowContacts;
            _allow_calendar = settings.AllowCalendar;
            _hide_login_mode = settings.HideLoginMode;
            _domain_to_use = settings.DefaultDomainOptional;
            _allow_choosing_lang = settings.AllowLanguageOnLogin;
            _allow_advanced_login = settings.AllowAdvancedLogin;
            _allow_auto_detect_and_correct = settings.AutomaticCorrectLoginSettings;
            _viewmode = settings.ViewMode;
            _save_mail = SaveMail.Always;
        }

        public Domain(int id_account, string name, IncomingMailProtocol mail_protocol,
                      string mail_inc_host, int mail_inc_port, string mail_out_host,
                      int mail_out_port, bool mail_out_auth, string url, string site_name,
                      IncomingMailProtocol settings_mail_protocol, string settings_mail_inc_host,
                      int settings_mail_inc_port, string settings_mail_out_host,
                      int settings_mail_out_port, bool settings_mail_out_auth,
                      bool allow_direct_mode, bool direct_mode_id_def, long attachment_size_limit,
                      bool allow_attachment_limit, long mailbox_size_limit, bool allow_mailbox_limit,
                      bool take_quota, bool allow_new_users_change_settings, bool allow_auto_reg_on_login,
                      bool allow_users_add_accounts, bool allow_users_change_account_def,
                      int def_user_charset, bool allow_users_change_charset, int def_user_timezone,
                      bool allow_users_change_timezone, int msgs_per_page, string skin,
                      bool allow_users_change_skin, string lang, bool allow_users_change_lang,
                      bool show_text_labels, bool allow_ajax, bool allow_editor, bool allow_contacts,
                      bool allow_calendar, LoginMode hide_login_mode,
                      string domain_to_use, bool allow_choosing_lang, bool allow_advanced_login,
                      bool allow_auto_detect_and_correct, bool contacts_sharing, ViewMode viewmode, SaveMail save_mail)
        {
            _id = id_account;
            _name = name;
            _mailProtocol = mail_protocol;
            _mailIncHost = mail_inc_host;
            _mailIncPort = mail_inc_port;
            _mailOutHost = mail_out_host;
            _mailOutPort = mail_out_port;
            _mailOutAuthentication = mail_out_auth;

            _url = url;
            _site_name = site_name;
            _settings_mail_protocol = settings_mail_protocol;
            _settings_mail_inc_host = settings_mail_inc_host;
            _settings_mail_inc_port = settings_mail_inc_port;
            _settings_mail_out_host = settings_mail_out_host;
            _settings_mail_out_port = settings_mail_out_port;
            _settings_mail_out_auth = settings_mail_out_auth;
            _allow_direct_mode = allow_direct_mode;
            _direct_mode_id_def = direct_mode_id_def;
            _attachment_size_limit = attachment_size_limit;
            _allow_attachment_limit = allow_attachment_limit;
            _mailbox_size_limit = mailbox_size_limit;
            _allow_mailbox_limit = allow_mailbox_limit;
            _take_quota = take_quota;
            _allow_new_users_change_settings = allow_new_users_change_settings;
            _allow_auto_reg_on_login = allow_auto_reg_on_login;
            _allow_users_add_accounts = allow_users_add_accounts;
            _allow_users_change_account_def = allow_users_change_account_def;
            _def_user_charset = def_user_charset;
            _allow_users_change_charset = allow_users_change_charset;
            _def_user_timezone = def_user_timezone;
            _allow_users_change_timezone = allow_users_change_timezone;
            _msgs_per_page = msgs_per_page;
            _skin = skin;
            _allow_users_change_skin = allow_users_change_skin;
            _lang = lang;
            _allow_users_change_lang = allow_users_change_lang;
            _show_text_labels = show_text_labels;
            _allow_ajax = allow_ajax;
            _allow_editor = allow_editor;
            _allow_contacts = allow_contacts;
            _allow_calendar = allow_calendar;
            _hide_login_mode = hide_login_mode;
            _domain_to_use = domain_to_use;
            _allow_choosing_lang = allow_choosing_lang;
            _allow_advanced_login = allow_advanced_login;
            _allow_auto_detect_and_correct = allow_auto_detect_and_correct;
			_global_addr_book = contacts_sharing;
            _viewmode = viewmode;
            _save_mail = save_mail;
        }

        public static Domain LoadFromDb(string name)
        {
			DbManagerCreator creator = new DbManagerCreator();
			DbManager dbMan = creator.CreateDbManager();
            Domain dom = null;
			try
			{
				dbMan.Connect();
                dom = dbMan.SelectDomainData(name);
			}
			finally
			{
				dbMan.Disconnect();
			}
			return dom;
        }

        public static Domain LoadFromDb(int id)
        {
            DbManagerCreator creator = new DbManagerCreator();
            DbManager dbMan = creator.CreateDbManager();
            Domain dom = null;
            try
            {
                dbMan.Connect();
                dom = dbMan.SelectDomainData(id);
            }
            finally
            {
                dbMan.Disconnect();
            }
            return dom;
        }

        public static void Delete(int ID)
        {
            DbManagerCreator creator = new DbManagerCreator();
            DbManager dbMan = creator.CreateDbManager();
            try
            {
                dbMan.Connect();
                dbMan.DeleteDomain(ID);
            }
            catch (WebMailDatabaseException ex)
            {
                throw new WebMailDatabaseException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("PROC_CANT_DEL_ACCT_BY_ID"), ex);
            }
            finally
            {
                dbMan.Disconnect();
            }
        }

        public static DomainCollection GetDomains()
        {
            DbManagerCreator creator = new DbManagerCreator();
            DbManager dbMan = creator.CreateDbManager();
            DomainCollection domains = null;
            try
            {
                dbMan.Connect();
                domains = dbMan.SelectDomains();
            }
            catch (WebMailDatabaseException)
            {
                throw;
            }
            finally
            {
                dbMan.Disconnect();
            }
            return domains;
        }

        public static DomainCollection GetDomains(short[] MailProtocols)
        {
            DbManagerCreator creator = new DbManagerCreator();
            DbManager dbMan = creator.CreateDbManager();
            DomainCollection domains = null;
            try
            {
                dbMan.Connect();
                domains = dbMan.SelectDomains(MailProtocols);
            }
            catch (WebMailDatabaseException)
            {
                throw;
            }
            finally
            {
                dbMan.Disconnect();
            }
            return domains;
        }

        public static Domain GetDomain(int id)
        {
            DbManagerCreator creator = new DbManagerCreator();
            DbManager dbMan = creator.CreateDbManager();
            Domain domain = null;
            try
            {
                dbMan.Connect();
                domain = dbMan.SelectDomainData(id);
            }
            catch (WebMailDatabaseException)
            {
                throw;
            }
            finally
            {
                dbMan.Disconnect();
            }
            return domain;
        }

        public static Domain GetDomain(string name)
        {
            DbManagerCreator creator = new DbManagerCreator();
            DbManager dbMan = creator.CreateDbManager();
            Domain domain = null;
            try
            {
                dbMan.Connect();
                domain = dbMan.SelectDomainData(name);
            }
            catch (WebMailDatabaseException)
            {
                throw;
            }
            finally
            {
                dbMan.Disconnect();
            }
            return domain;
        }

        public static AccountCollection GetAccounts(int id_domain)
        {
            DbManagerCreator creator = new DbManagerCreator();
            DbManager dbMan = creator.CreateDbManager();
            AccountCollection accounts = null;
            try
            {
                dbMan.Connect();
                accounts = dbMan.SelectDomainAccounts(id_domain);
            }
            catch (WebMailDatabaseException)
            {
                throw;
            }
            finally
            {
                dbMan.Disconnect();
            }
            return accounts;
        }

        public void Save()
        {
            DbManagerCreator creator = new DbManagerCreator();
            DbManager dbMan = creator.CreateDbManager();
            WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();

            if (Domain.LoadFromDb(Name) == null)
            {
                try
                {
                    dbMan.Connect();
                    if (settings.EnableWmServer && this.MailIncomingProtocol == IncomingMailProtocol.WMServer)
                    {
                        WMServerStorage storage = new WMServerStorage(null);
                        try
                        {
                            storage.AddDomain(Name);
                        }
                        catch
                        {
                        }
                        try
                        {
                            dbMan.CreateDomain(Name, MailIncomingProtocol, MailIncomingHost, MailIncomingPort, MailOutgoingHost,
                              MailOutgoingPort, MailOutgoingAuthentication, Url, Site_name,
                              Settings_mail_protocol, Settings_mail_inc_host,
                              Settings_mail_inc_port, Settings_mail_out_host,
                              Settings_mail_out_port, Settings_mail_out_auth,
                              Allow_direct_mode, Direct_mode_id_def, Attachment_size_limit,
                              Allow_attachment_limit, Mailbox_size_limit, Allow_mailbox_limit,
                              Take_quota, Allow_new_users_change_settings, Allow_auto_reg_on_login,
                              Allow_users_add_accounts, Allow_users_change_account_def,
                              Def_user_charset, Allow_users_change_charset, Def_user_timezone,
                              Allow_users_change_timezone, Msgs_per_page, Skin,
                              Allow_users_change_skin, Lang, Allow_users_change_lang,
                              Show_text_labels, Allow_ajax, Allow_editor, Allow_contacts,
                              Allow_calendar, Hide_login_mode,
                              Domain_to_use, Allow_choosing_lang, Allow_advanced_login,
                              Allow_auto_detect_and_correct, Global_addr_book, ViewMode, SaveMail);
                        }
                        catch (Exception ex)
                        {
                            Log.WriteException(ex);
                        }
                    }
                    else
                    {
                        dbMan.CreateDomain(Name, MailIncomingProtocol,
                          MailIncomingHost, MailIncomingPort, MailOutgoingHost,
                          MailOutgoingPort, MailOutgoingAuthentication, Url, Site_name,
                          Settings_mail_protocol, Settings_mail_inc_host,
                          Settings_mail_inc_port, Settings_mail_out_host,
                          Settings_mail_out_port, Settings_mail_out_auth,
                          Allow_direct_mode, Direct_mode_id_def, Attachment_size_limit,
                          Allow_attachment_limit, Mailbox_size_limit, Allow_mailbox_limit,
                          Take_quota, Allow_new_users_change_settings, Allow_auto_reg_on_login,
                          Allow_users_add_accounts, Allow_users_change_account_def,
                          Def_user_charset, Allow_users_change_charset, Def_user_timezone,
                          Allow_users_change_timezone, Msgs_per_page, Skin,
                          Allow_users_change_skin, Lang, Allow_users_change_lang,
                          Show_text_labels, Allow_ajax, Allow_editor, Allow_contacts,
                          Allow_calendar, Hide_login_mode,
                          Domain_to_use, Allow_choosing_lang, Allow_advanced_login,
                          Allow_auto_detect_and_correct, Global_addr_book, ViewMode, SaveMail);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex);
                    throw;
                }
                finally
                {
                    dbMan.Disconnect();
                }
            }
            else
            {
                try
                {
                    dbMan.Connect();
                    dbMan.UpdateDomain(ID, Name, MailIncomingProtocol,
                      MailIncomingHost, MailIncomingPort, MailOutgoingHost,
                      MailOutgoingPort, MailOutgoingAuthentication, Url, Site_name,
                      Settings_mail_protocol, Settings_mail_inc_host,
                      Settings_mail_inc_port, Settings_mail_out_host,
                      Settings_mail_out_port, Settings_mail_out_auth,
                      Allow_direct_mode, Direct_mode_id_def, Attachment_size_limit,
                      Allow_attachment_limit, Mailbox_size_limit, Allow_mailbox_limit,
                      Take_quota, Allow_new_users_change_settings, Allow_auto_reg_on_login,
                      Allow_users_add_accounts, Allow_users_change_account_def,
                      Def_user_charset, Allow_users_change_charset, Def_user_timezone,
                      Allow_users_change_timezone, Msgs_per_page, Skin,
                      Allow_users_change_skin, Lang, Allow_users_change_lang,
                      Show_text_labels, Allow_ajax, Allow_editor, Allow_contacts,
                      Allow_calendar, Hide_login_mode,
                      Domain_to_use, Allow_choosing_lang, Allow_advanced_login,
                      Allow_auto_detect_and_correct, Global_addr_book, ViewMode, SaveMail);
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex);
                    throw;
                }
                finally
                {
                    dbMan.Disconnect();
                }
            }
        }

        public void Delete()
        {
            DbManagerCreator creator = new DbManagerCreator();
            DbManager dbMan = creator.CreateDbManager();
            WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();

            if (Domain.LoadFromDb(this.Name) != null)
            {
                try
                {
                    dbMan.Connect();
                    if (settings.EnableWmServer && this.MailIncomingProtocol == IncomingMailProtocol.WMServer)
                    {
                        WMServerStorage storage = new WMServerStorage(null);
                        try
                        {
                            storage.DeleteDomain(this.Name);
                            dbMan.DeleteDomain(_id);
                        }
                        catch (Exception ex)
                        {
                            Log.WriteException(ex);
                        }
                    }
                    else
                    {
                        dbMan.DeleteDomain(_id);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex);
                }
                finally
                {
                    dbMan.Disconnect();
                }
            }
        }
    }

    public class DomainCollection : CollectionBase
    {
        public DomainCollection()
        {
        }

        public Domain this[int index]
        {
            get { return ((Domain)List[index]); }
            set { List[index] = value; }
        }

        public Domain GetItem(int ID)
        {
            foreach (Domain dom in List)
            {
                if (dom.ID == ID)
                {
                    return dom;
                }
            }
            return null;
        }

        public Domain GetItem(string Name)
        {
            foreach (Domain dom in List)
            {
                if (dom.Name == Name)
                {
                    return dom;
                }
            }
            return null;
        }

        public int Add(Domain value)
        {
            return (List.Add(value));
        }

        public int IndexOf(Domain value)
        {
            return (List.IndexOf(value));
        }

        public void Insert(int index, Domain value)
        {
            List.Insert(index, value);
        }

        public void Remove(Domain value)
        {
            List.Remove(value);
        }

        public bool Contains(Domain value)
        {
            return (List.Contains(value));
        }
    }
}
