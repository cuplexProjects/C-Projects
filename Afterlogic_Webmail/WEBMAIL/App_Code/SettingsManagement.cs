using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Xml;
using System.Collections;

namespace WebMail
{
    /// <summary>
    /// This class provides properties and methods for managing WebMail settings (\App_Data\settings\settings.xml)
    /// </summary>
    [Serializable]
    public class WebmailSettings
    {
        #region Fields
        protected WebmailSettings _instance;

        // WebMailPro Settings
        protected string _siteName;
        protected string _licenseKey;
        protected string _adminPassword;
        protected SupportedDatabase _dbType;
        protected string _dbLogin;
        protected string _dbPassword;
        protected string _dbName;
        protected string _dbDsn;
        protected string _dbHost;
        protected string _dbPathToMdb;
        protected bool _useCustomConnectionString;
        protected string _dbCustomConnectionString;
        protected string _dbPrefix;
        protected IncomingMailProtocol _incomingMailProtocol;
        protected string _incomingMailServer;
        protected int _incomingMailPort;
        protected string _outgoingMailServer;
        protected int _outgoingMailPort;
        protected bool _reqSmtpAuth;
        protected bool _allowAdvancedLogin;
        protected LoginMode _hideLoginMode;
        protected string _defaultDomainOptional;
        protected bool _showTextLabels;
        protected bool _automaticCorrectLoginSettings;
        protected bool _enableLogging;
        protected bool _allowAjax;
        protected short _mailsPerPage;
        protected long _attachmentSizeLimit;
        protected bool _enableAttachmentSizeLimit;
        protected long _mailboxSizeLimit;
        protected bool _enableMailboxSizeLimit;
        protected short _defaultTimeOffset;
        protected bool _allowUsersChangeTimeOffset;
        protected int _defaultUserCharset;
        protected bool _allowUsersChangeCharset;
        protected string _defaultSkin;
        protected bool _allowUsersChangeSkin;
        protected string _defaultLanguage;
        protected bool _allowUsersChangeLanguage;
        protected bool _allowDhtmlEditor;
        protected bool _allowUsersChangeEmailSettings;
        protected bool _allowDirectMode;
        protected bool _directModeIsDefault;
        protected bool _allowNewUserRegister;
        protected bool _allowUsersAddNewAccount;
        protected bool _storeMailsInDb;
        protected bool _enableWmServer;
        protected string _wmServerRootPath;
        protected string _wmServerHost;
        protected bool _wmAllowManageXMailAccounts;
        protected bool _allowContacts;
        protected bool _allowCalendar;
        protected bool _imap4DeleteLikePop3;
        protected bool _rtl;
        protected bool _allowLanguageOnLogin;
        protected bool _useDSN;
        protected bool _takeImapQuota;
        protected bool _allowUsersChangeAccountsDef;

        protected bool _allowInsertImage;
        protected bool _allowBodySize;
        protected int _maxBodySize;
        protected int _maxSubjectSize;
		protected GlobalAddressBookEnum _globalAddressBook;
        protected ViewMode _view_mode;

        // Calendar Settings
        protected int _defaultTimeFormat;
        protected int _defaultDateFormat;
        protected int _showWeekends;
        protected int _workdayStarts;
        protected int _workdayEnds;
        protected int _showWorkDay;
        protected int _weekStartsOn;

        private Hashtable _webmailTab;
        private Hashtable _ctrlAccountsTab;

        protected int _defaultTab;
        protected string _defaultCountry;
        protected int _defaultTimeZoneCalendar;
        protected int _allTimeZones;

		private bool _allowReminders;

        #endregion

        #region Properties

        public string SiteName
        {
            get { return _siteName; }
            set { _siteName = value; }
        }

        public string LicenseKey
        {
            
            
            get { return WebMail.Constants.LiteLicenseKey; }
            
            set { _licenseKey = value; }
        }

        public string AdminPassword
        {
            get { return _adminPassword; }
            set { _adminPassword = value; }
        }

        public SupportedDatabase DbType
        {
            get { return _dbType; }
            set { _dbType = value; }
        }

        public string DbLogin
        {
            get { return _dbLogin; }
            set { _dbLogin = value; }
        }

        public string DbPassword
        {
            get { return _dbPassword; }
            set { _dbPassword = value; }
        }

        public string DbName
        {
            get { return _dbName; }
            set { _dbName = value; }
        }

        public string DbDsn
        {
            get { return _dbDsn; }
            set { _dbDsn = value; }
        }

        public string DbHost
        {
            get { return _dbHost; }
            set { _dbHost = value; }
        }

        public string DbPathToMdb
        {
            get { return _dbPathToMdb; }
            set { _dbPathToMdb = value; }
        }

        public bool UseCustomConnectionString
        {
            get { return _useCustomConnectionString; }
            set { _useCustomConnectionString = value; }
        }

        public bool UseDSN
        {
            get { return _useDSN; }
            set { _useDSN = value; }
        }

        public string DbCustomConnectionString
        {
            get { return _dbCustomConnectionString; }
            set { _dbCustomConnectionString = value; }
        }

        public string DbPrefix
        {
            get { return _dbPrefix; }
            set { _dbPrefix = value; }
        }

        public IncomingMailProtocol IncomingMailProtocol
        {
            get { return _incomingMailProtocol; }
            set { _incomingMailProtocol = value; }
        }

        public string IncomingMailServer
        {
            get { return _incomingMailServer; }
            set { _incomingMailServer = value; }
        }

        public int IncomingMailPort
        {
            get { return _incomingMailPort; }
            set { _incomingMailPort = value; }
        }

        public string OutgoingMailServer
        {
            get { return _outgoingMailServer; }
            set { _outgoingMailServer = value; }
        }

        public int OutgoingMailPort
        {
            get { return _outgoingMailPort; }
            set { _outgoingMailPort = value; }
        }

        public bool ReqSmtpAuth
        {
            get { return _reqSmtpAuth; }
            set { _reqSmtpAuth = value; }
        }

        public bool AllowAdvancedLogin
        {
            get { return _allowAdvancedLogin; }
            set { _allowAdvancedLogin = value; }
        }

        public LoginMode HideLoginMode
        {
            get { return _hideLoginMode; }
            set { _hideLoginMode = value; }
        }

        public string DefaultDomainOptional
        {
            get { return _defaultDomainOptional; }
            set { _defaultDomainOptional = value; }
        }

        public bool ShowTextLabels
        {
            get { return _showTextLabels; }
            set { _showTextLabels = value; }
        }

        public bool AutomaticCorrectLoginSettings
        {
            get { return _automaticCorrectLoginSettings; }
            set { _automaticCorrectLoginSettings = value; }
        }

        public bool EnableLogging
        {
            get { return _enableLogging; }
            set { _enableLogging = value; }
        }

        public bool AllowAjax
        {
            get { return _allowAjax; }
            set { _allowAjax = value; }
        }

        public short MailsPerPage
        {
            get { return _mailsPerPage; }
            set { _mailsPerPage = value; }
        }

        public long AttachmentSizeLimit
        {
            get { return _attachmentSizeLimit; }
            set { _attachmentSizeLimit = value; }
        }

        public bool EnableAttachmentSizeLimit
        {
            get { return _enableAttachmentSizeLimit; }
            set { _enableAttachmentSizeLimit = value; }
        }

        public long MailboxSizeLimit
        {
            get { return _mailboxSizeLimit; }
            set { _mailboxSizeLimit = value; }
        }

        public bool EnableMailboxSizeLimit
        {
            get { return _enableMailboxSizeLimit; }
            set { _enableMailboxSizeLimit = value; }
        }

        public short DefaultTimeZone
        {
            get { return _defaultTimeOffset; }
            set { _defaultTimeOffset = value; }
        }

        public bool AllowUsersChangeTimeZone
        {
            get { return _allowUsersChangeTimeOffset; }
            set { _allowUsersChangeTimeOffset = value; }
        }

        public int DefaultUserCharset
        {
            get { return _defaultUserCharset; }
            set { _defaultUserCharset = value; }
        }

        public bool AllowUsersChangeCharset
        {
            get { return _allowUsersChangeCharset; }
            set { _allowUsersChangeCharset = value; }
        }

        public string DefaultSkin
        {
            get { return _defaultSkin; }
            set { _defaultSkin = value; }
        }

        public bool AllowUsersChangeSkin
        {
            get { return _allowUsersChangeSkin; }
            set { _allowUsersChangeSkin = value; }
        }

        public string DefaultLanguage
        {
            get { return _defaultLanguage; }
            set
            {
                _defaultLanguage = value;
                if (value == "Hebrew" || value == "Arabic")
                {
                    _rtl = true;
                }
                else
                {
                    _rtl = false;
                }
            }
        }

        public bool AllowUsersChangeLanguage
        {
            get { return _allowUsersChangeLanguage; }
            set { _allowUsersChangeLanguage = value; }
        }

        public bool AllowDhtmlEditor
        {
            get { return _allowDhtmlEditor; }
            set { _allowDhtmlEditor = value; }
        }

        public bool AllowUsersChangeEmailSettings
        {
            get { return _allowUsersChangeEmailSettings; }
            set { _allowUsersChangeEmailSettings = value; }
        }

        public bool AllowDirectMode
        {
            get { return _allowDirectMode; }
            set { _allowDirectMode = value; }
        }

        public bool DirectModeIsDefault
        {
            get { return _directModeIsDefault; }
            set { _directModeIsDefault = value; }
        }

        public bool AllowNewUsersRegister
        {
            
            
            get { return true; }
            
            set { _allowNewUserRegister = value; }
        }

        public bool AllowUsersAddNewAccounts
        {
            
            
            get { return false; }
            
            set { _allowUsersAddNewAccount = value; }
        }

        public bool StoreMailsInDb
        {
            get { return _storeMailsInDb; }
            set { _storeMailsInDb = value; }
        }

        public bool EnableWmServer
        {
            get { return _enableWmServer; }
            set { _enableWmServer = value; }
        }

        public string WmServerRootPath
        {
            get { return _wmServerRootPath; }
            set { _wmServerRootPath = value; }
        }

        public bool AllowContacts
        {
            get { return _allowContacts; }
            set { _allowContacts = value; }
        }

        public string WmServerHost
        {
            get { return _wmServerHost; }
            set { _wmServerHost = value; }
        }

        public bool WmAllowManageXMailAccounts
        {
            get { return _wmAllowManageXMailAccounts; }
            set { _wmAllowManageXMailAccounts = value; }
        }

        protected Hashtable WebmailTab
        {
            get { return _webmailTab; }
        }

        protected Hashtable CtrlAccountsTab
        {
            get { return _ctrlAccountsTab; }
        }

        public bool AllowCalendar
        {
            
            
            get { return false; }
            
            set { _allowCalendar = value; }
        }

        public bool Imap4DeleteLikePop3
        {
            get { return _imap4DeleteLikePop3; }
        }

        public int DefaultTimeFormat
        {
            get { return _defaultTimeFormat; }
            set { _defaultTimeFormat = value; }
        }

        public int DefaultDateFormat
        {
            get { return _defaultDateFormat; }
            set { _defaultDateFormat = value; }
        }

        public int ShowWeekends
        {
            get { return _showWeekends; }
            set { _showWeekends = value; }
        }

        public int WorkdayStarts
        {
            get { return _workdayStarts; }
            set { _workdayStarts = value; }
        }

        public int WorkdayEnds
        {
            get { return _workdayEnds; }
            set { _workdayEnds = value; }
        }

        public int ShowWorkDay
        {
            get { return _showWorkDay; }
            set { _showWorkDay = value; }
        }

        public int WeekStartsOn
        {
            get { return _weekStartsOn; }
            set { _weekStartsOn = value; }
        }

        public int DefaultTab
        {
            get { return _defaultTab; }
            set { _defaultTab = value; }
        }

        public string DefaultCountry
        {
            get { return _defaultCountry; }
            set { _defaultCountry = value; }
        }

        public int DefaultTimeZoneCalendar
        {
            get { return _defaultTimeZoneCalendar; }
            set { _defaultTimeZoneCalendar = value; }
        }

        public int AllTimeZones
        {
            get { return _allTimeZones; }
            set { _allTimeZones = value; }
        }

		public bool AllowReminders
		{
            get { return _allowReminders; }
            set { _allowReminders = value; }
		}

        public bool RTL
        {
            get { return _rtl; }
        }

        public bool AllowLanguageOnLogin
        {
            get { return _allowLanguageOnLogin; }
            set { _allowLanguageOnLogin = value; }
        }

        public int XMailControlPort
        {
            get
            {
                int port = 0;
                if (_webmailTab != null)
                {
                    if (_webmailTab[Constants.WebmailTabKeys.ControlPort] != null)
                    {
                        if (int.TryParse(_webmailTab[Constants.WebmailTabKeys.ControlPort].ToString(), out port))
                        {
                            return port;
                        }
                    }
                }
                return port;
            }
        }

        public int XMailSmtpPort
        {
            get
            {
                int port = 0;
                if (_webmailTab != null)
                {
                    if (_webmailTab[Constants.WebmailTabKeys.SmtpPort] != null)
                    {
                        if (int.TryParse(_webmailTab[Constants.WebmailTabKeys.SmtpPort].ToString(), out port))
                        {
                            return port;
                        }
                    }
                }
                return port;
            }
        }

        public string XMailLogin
        {
            get
            {
                if (_webmailTab != null)
                {
                    return (string)_ctrlAccountsTab["login"];
                }
                return null;
            }
        }

        public string XMailPass
        {
            get
            {
                if (_webmailTab != null)
                {
                    return WMServerStorage.DecryptPassword((string)_ctrlAccountsTab["password"]);
                }
                return null;
            }
        }

        public bool TakeImapQuota
        {
            get { return _takeImapQuota; }
            set { _takeImapQuota = value; }
        }

        public bool AllowUsersChangeAccountsDef
        {
            get { return _allowUsersChangeAccountsDef; }
            set { _allowUsersChangeAccountsDef = value; }
        }

        public bool AllowInsertImage
        {
            get { return _allowInsertImage; }
            set { _allowInsertImage = value; }
        }

        public bool AllowBodySize
        {
            get { return _allowBodySize; }
            set { _allowBodySize = value; }
        }

        public int MaxBodySize
        {
            get { return _maxBodySize; }
            set { _maxBodySize = value; }
        }

        public int MaxSubjectSize
        {
            get { return _maxSubjectSize; }
            set { _maxSubjectSize = value; }
        }

		public GlobalAddressBookEnum GlobalAddressBook
		{
			get { return _globalAddressBook; }
			set { _globalAddressBook = value; }
		}

        public ViewMode ViewMode
        {
            get { return _view_mode; }
            set { _view_mode = value; }
        }

        #endregion

        public WebmailSettings()
        {
            _instance = null;

            
            _siteName = "AfterLogic WebMail Lite";
            _licenseKey = string.Empty;
            _adminPassword = string.Empty;
            _dbType = SupportedDatabase.MsSqlServer;
            _dbLogin = string.Empty;
            _dbPassword = string.Empty;
            _dbName = string.Empty;
            _dbDsn = string.Empty;
            _dbHost = "localhost";
            _dbPathToMdb = "...";
            _useCustomConnectionString = false;
            _dbCustomConnectionString = "...";
            _dbPrefix = string.Empty;
            _incomingMailServer = "localhost";
            _incomingMailPort = 110;
            _outgoingMailServer = "localhost";
            _outgoingMailPort = 25;
            _reqSmtpAuth = true;
            
            _allowAdvancedLogin = false;
            
            _hideLoginMode = LoginMode.HideLoginFieldLoginIsEmail;
            _defaultDomainOptional = "localhost";
            _showTextLabels = true;
            _automaticCorrectLoginSettings = true;
            _enableLogging = false;
            _allowAjax = true;
            _mailsPerPage = 20;
            _attachmentSizeLimit = 10240000;
            _enableAttachmentSizeLimit = false;
            _mailboxSizeLimit = 102400000;
            _enableMailboxSizeLimit = true;
            _defaultTimeOffset = 0;
            _allowUsersChangeTimeOffset = true;
            _defaultUserCharset = 65001;
            _allowUsersChangeCharset = true;
            _defaultSkin = Constants.defaultSkinName;
            _allowUsersChangeSkin = true;
			_defaultLanguage = Constants.defaultLang;
            _allowUsersChangeLanguage = true;
            _allowDhtmlEditor = true;
            _allowUsersChangeEmailSettings = true;
            _allowDirectMode = true;
            _directModeIsDefault = false;
            _allowNewUserRegister = true;
            
            _allowUsersAddNewAccount = false;

            _storeMailsInDb = false;

            _enableWmServer = false;
            _wmServerRootPath = @"C:\Program Files\AfterLogic XMail Server\MailRoot\";
            _wmServerHost = "localhost";
            _wmAllowManageXMailAccounts = false;

            _allowContacts = true;
            _allowCalendar = true;
            _imap4DeleteLikePop3 = true;
            _allowLanguageOnLogin = false;

            _defaultTimeFormat = 1;
            _defaultDateFormat = 1;
            _showWeekends = 1;
            _workdayStarts = 9;
            _workdayEnds = 18;
            _showWorkDay = 1;
            _weekStartsOn = 0;
            _defaultTab = 2;
            _defaultCountry = "US";
            _defaultTimeZoneCalendar = 38;
            _allTimeZones = 0;
            _useDSN = false;
            _takeImapQuota = false;
            _allowUsersChangeAccountsDef = false;
			_allowReminders = false;
            _allowInsertImage = true;
            _allowBodySize = false;
            _maxBodySize = 600;
            _maxSubjectSize = 255;
			_globalAddressBook = GlobalAddressBookEnum.DomainWide;
            _view_mode = ViewMode.WithPreviewPane;

            // Check Right-To-Left (RTL) support
            if (_defaultLanguage == "Hebrew" || _defaultLanguage == "Arabic")
            {
                _rtl = true;
            }
            else
            {
                _rtl = false;
            }
        }

        public WebmailSettings CreateInstance()
        {
            if (_instance == null)
            {
                _instance = new WebmailSettings();
                _instance.LoadWebmailSettings();
            }

            return _instance;
        }

        public WebmailSettings CreateInstance(string dataFolder)
        {
            if (_instance == null)
            {
                _instance = new WebmailSettings();
                _instance.LoadWebmailSettings(dataFolder);
            }

            return _instance;
        }

        public void _LoadXML(XmlNode root)
        {
            for (int i = 0; i < root.ChildNodes.Count; i++)
            {
                XmlNode node = root.ChildNodes[i];
                switch (node.Name)
                {
                    case "Common":
                    case "WebMail":
                    case "Calendar":
                        if (node.ChildNodes.Count > 0)
                            _LoadXML(node);
                        break;
                    case "SiteName":
                        SiteName = node.InnerText;
                        break;
                    case "WindowTitle":
                        SiteName = node.InnerText;
                        break;
                    case "LicenseKey":
                        LicenseKey = node.InnerText;
                        break;
                    case "AdminPassword":
                        AdminPassword = node.InnerText;
                        break;
                    case "DBType":
                        if (node.InnerText != string.Empty) DbType = (SupportedDatabase)int.Parse(node.InnerText);
                        break;
                    case "DBLogin":
                        DbLogin = node.InnerText;
                        break;
                    case "DBPassword":
                        DbPassword = node.InnerText;
                        break;
                    case "DBName":
                        DbName = node.InnerText;
                        break;
                    case "DBDSN":
                        DbDsn = node.InnerText;
                        break;
                    case "DBHost":
                        DbHost = node.InnerText;
                        break;
                    case "DBPathToMdb":
                        DbPathToMdb = node.InnerText;
                        break;
                    case "UseCustomConnectionString":
                        UseCustomConnectionString = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
                        break;
                    case "UseDSN":
                        UseDSN = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
                        break;
                    case "DBCustomConnectionString":
                        DbCustomConnectionString = node.InnerText;
                        break;
                    case "DBPrefix":
                        DbPrefix = node.InnerText;
                        break;
                    case "IncomingMailProtocol":
                        if (node.InnerText != string.Empty) IncomingMailProtocol = (IncomingMailProtocol)int.Parse(node.InnerText);
                        break;
                    case "IncomingMailServer":
                        IncomingMailServer = node.InnerText;
                        break;
                    case "IncomingMailPort":
                        if (node.InnerText != string.Empty) IncomingMailPort = int.Parse(node.InnerText);
                        break;
                    case "OutgoingMailServer":
                        OutgoingMailServer = node.InnerText;
                        break;
                    case "OutgoingMailPort":
                        if (node.InnerText != string.Empty) OutgoingMailPort = int.Parse(node.InnerText);
                        break;
                    case "ReqSmtpAuth":
                        ReqSmtpAuth = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
                        break;
                    case "AllowAdvancedLogin":
                        AllowAdvancedLogin = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
                        break;
                    case "HideLoginMode":
                        HideLoginMode = (LoginMode)short.Parse(node.InnerText, CultureInfo.InvariantCulture);
                        break;
                    case "DefaultDomainOptional":
                        DefaultDomainOptional = node.InnerText;
                        break;
                    case "ShowTextLabels":
                        ShowTextLabels = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
                        break;
                    case "AutomaticCorrectLoginSettings":
                        AutomaticCorrectLoginSettings = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
                        break;
                    case "EnableLogging":
                        EnableLogging = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
                        break;
                    case "AllowAjax":
                        AllowAjax = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
                        break;
                    case "MailsPerPage":
                        if (node.InnerText != string.Empty) MailsPerPage = short.Parse(node.InnerText);
                        break;
                    case "EnableAttachmentSizeLimit":
                        EnableAttachmentSizeLimit = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
                        break;
                    case "AttachmentSizeLimit":
                        if (node.InnerText != string.Empty) AttachmentSizeLimit = long.Parse(node.InnerText);
                        break;
                    case "EnableMailboxSizeLimit":
                        EnableMailboxSizeLimit = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
                        break;
                    case "MailboxSizeLimit":
                        if (node.InnerText != string.Empty) MailboxSizeLimit = long.Parse(node.InnerText);
                        break;
                    case "AllowUsersChangeTimeZone":
                        AllowUsersChangeTimeZone = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
                        break;
                    case "DefaultUserCharset":
                        if (node.InnerText != string.Empty) DefaultUserCharset = int.Parse(node.InnerText);
                        break;
                    case "AllowUsersChangeCharset":
                        AllowUsersChangeCharset = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
                        break;
                    case "DefaultSkin":
                        DefaultSkin = node.InnerText;
                        break;
                    case "AllowUsersChangeSkin":
                        AllowUsersChangeSkin = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
                        break;
                    case "DefaultLanguage":
                        DefaultLanguage = node.InnerText;
                        break;
                    case "AllowUsersChangeLanguage":
                        AllowUsersChangeLanguage = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
                        break;
                    case "AllowDHTMLEditor":
                        AllowDhtmlEditor = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
                        break;
                    case "AllowUsersChangeEmailSettings":
                        AllowUsersChangeEmailSettings = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
                        break;
                    case "AllowDirectMode":
                        AllowDirectMode = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
                        break;
                    case "DirectModeIsDefault":
                        DirectModeIsDefault = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
                        break;

                    case "AllowInsertImage":
                        AllowBodySize = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
                        break;
                    case "AllowBodySize":
                        AllowBodySize = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
                        break;
                    case "MaxBodySize":
                        MaxBodySize = int.Parse(node.InnerText);
                        break;
                    case "MaxSubjectSize":
                        MaxSubjectSize = int.Parse(node.InnerText);
                        break;

                    case "AllowNewUsersRegister":
                        AllowNewUsersRegister = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
                        break;
                    case "AllowUsersAddNewAccounts":
                        AllowUsersAddNewAccounts = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
                        break;
                    case "StoreMailsInDb":
                        StoreMailsInDb = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
                        break;
                    case "EnableWmServer":
                        EnableWmServer = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
                        break;
                    case "WmServerRootPath":
                        WmServerRootPath = node.InnerText;
                        break;
                    case "WmServerHost":
                        WmServerHost = node.InnerText;
                        break;
                    case "WmAllowManageXMailAccounts":
                        WmAllowManageXMailAccounts = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
                        break;
                    case "AllowContacts":
                        AllowContacts = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
                        break;
                    case "AllowCalendar":
                        AllowCalendar = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
                        break;
                    case "Imap4DeleteLikePop3":
                        _imap4DeleteLikePop3 = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
                        break;
                    case "AllowLanguageOnLogin":
                        AllowLanguageOnLogin = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
                        break;
                    case "DefaultTimeZone":
                        if (node.ParentNode.Name == "Calendar")
                        {
                            if (node.InnerText != string.Empty) DefaultTimeZoneCalendar = short.Parse(node.InnerText);
                        }
                        else
                        {
                            if (node.InnerText != string.Empty) DefaultTimeZone = short.Parse(node.InnerText);
                        }
                        break;
                    case "DefaultTimeFormat":
                        DefaultTimeFormat = int.Parse(node.InnerText);
                        break;
                    case "DefaultDateFormat":
                        DefaultDateFormat = int.Parse(node.InnerText);
                        break;
                    case "ShowWeekends":
                        ShowWeekends = int.Parse(node.InnerText);
                        break;
                    case "WorkdayStarts":
                        WorkdayStarts = int.Parse(node.InnerText);
                        break;
                    case "WorkdayEnds":
                        WorkdayEnds = int.Parse(node.InnerText);
                        break;
                    case "ShowWorkDay":
                        ShowWorkDay = int.Parse(node.InnerText);
                        break;
                    case "WeekStartsOn":
                        WeekStartsOn = int.Parse(node.InnerText);
                        break;
                    case "DefaultTab":
                        DefaultTab = int.Parse(node.InnerText);
                        break;
                    case "DefaultCountry":
                        DefaultCountry = node.InnerText;
                        break;
                    case "AllTimeZones":
                        AllTimeZones = int.Parse(node.InnerText);
                        break;
                    case "TakeImapQuota":
                        TakeImapQuota = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
                        break;
                    case "AllowUsersChangeAccountsDef":
                        AllowUsersChangeAccountsDef = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
                        break;
                    case "AllowReminders":
                        AllowReminders = (string.Compare(node.InnerText, "1", true, CultureInfo.InvariantCulture) == 0) ? true : false;
                        break;
					case "GlobalAddressBook":
						GlobalAddressBook = (GlobalAddressBookEnum)Enum.Parse(typeof(GlobalAddressBookEnum), node.InnerText, true);
						break;
                    case "ViewMode":
                        ViewMode = (ViewMode)Enum.Parse(typeof(ViewMode), node.InnerText, true);
                        break;
                }
            }
        }

        public void LoadWebmailSettings()
        {
            LoadWebmailSettings(Utils.GetDataFolderPath());
        }

        public void LoadWebmailSettings(string dataFolder)
        {
            try
            {
                string filename = Path.Combine(dataFolder, @"Settings\settings.xml");
                XmlDocument doc = new XmlDocument();

                if (!File.Exists(filename))
                {
                    throw new WebMailIOException();
                }

                doc.Load(filename);
                XmlNode root = doc.DocumentElement;
                _LoadXML(root);
                _webmailTab = Utils.ReadWebmailTab(WmServerRootPath);
                _ctrlAccountsTab = Utils.ReadCtrlAccountsTab(WmServerRootPath);


            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                throw new WebMailSettingsException(ex);
            }
        }

        public void SaveWebmailSettings()
        {
            SaveWebmailSettings(Utils.GetDataFolderPath());
        }

        public void SaveWebmailSettings(string dataFolder)
        {
            try
            {
                string filename = Path.Combine(dataFolder, @"Settings\settings.xml");

                XmlDocument result = new XmlDocument();
                result.PreserveWhitespace = false;
                XmlDeclaration xmlDecl = result.CreateXmlDeclaration("1.0", "utf-8", "");
                result.AppendChild(xmlDecl);

                XmlElement settingsElem = result.CreateElement("Settings");
                result.AppendChild(settingsElem);

                XmlElement subElemCommon = result.CreateElement("Common");
                settingsElem.AppendChild(subElemCommon);

                XmlElement subElemWebMail = result.CreateElement("WebMail");
                settingsElem.AppendChild(subElemWebMail);

                XmlElement subElemCalendar = result.CreateElement("Calendar");
                settingsElem.AppendChild(subElemCalendar);

                XmlElement subElem = result.CreateElement("SiteName");
                subElem.AppendChild(result.CreateTextNode(SiteName));
                subElemCommon.AppendChild(subElem);

                subElem = result.CreateElement("LicenseKey");
                subElem.AppendChild(result.CreateTextNode(LicenseKey));
                subElemCommon.AppendChild(subElem);

                subElem = result.CreateElement("AdminPassword");
                subElem.AppendChild(result.CreateTextNode(AdminPassword));
                subElemCommon.AppendChild(subElem);

                subElem = result.CreateElement("DBType");
                subElem.AppendChild(result.CreateTextNode(((int)DbType).ToString(CultureInfo.InvariantCulture)));
                subElemCommon.AppendChild(subElem);

                subElem = result.CreateElement("DBLogin");
                subElem.AppendChild(result.CreateTextNode(DbLogin));
                subElemCommon.AppendChild(subElem);

                subElem = result.CreateElement("DBPassword");
                subElem.AppendChild(result.CreateTextNode(DbPassword));
                subElemCommon.AppendChild(subElem);

                subElem = result.CreateElement("DBName");
                subElem.AppendChild(result.CreateTextNode(DbName));
                subElemCommon.AppendChild(subElem);

                subElem = result.CreateElement("DBDSN");
                subElem.AppendChild(result.CreateTextNode(DbDsn));
                subElemCommon.AppendChild(subElem);

                subElem = result.CreateElement("DBHost");
                subElem.AppendChild(result.CreateTextNode(DbHost));
                subElemCommon.AppendChild(subElem);

                subElem = result.CreateElement("DBPathToMdb");
                subElem.AppendChild(result.CreateTextNode(DbPathToMdb));
                subElemCommon.AppendChild(subElem);

                subElem = result.CreateElement("UseCustomConnectionString");
                subElem.AppendChild(result.CreateTextNode((UseCustomConnectionString) ? "1" : "0"));
                subElemCommon.AppendChild(subElem);

                subElem = result.CreateElement("UseDSN");
                subElem.AppendChild(result.CreateTextNode((UseDSN) ? "1" : "0"));
                subElemCommon.AppendChild(subElem);

                subElem = result.CreateElement("DBCustomConnectionString");
                subElem.AppendChild(result.CreateTextNode(DbCustomConnectionString));
                subElemCommon.AppendChild(subElem);

                subElem = result.CreateElement("DBPrefix");
                subElem.AppendChild(result.CreateTextNode(DbPrefix));
                subElemCommon.AppendChild(subElem);

                subElem = result.CreateElement("DefaultSkin");
                subElem.AppendChild(result.CreateTextNode(DefaultSkin));
                subElemCommon.AppendChild(subElem);

                subElem = result.CreateElement("AllowUsersChangeSkin");
                subElem.AppendChild(result.CreateTextNode((AllowUsersChangeSkin) ? "1" : "0"));
                subElemCommon.AppendChild(subElem);

                subElem = result.CreateElement("DefaultLanguage");
                subElem.AppendChild(result.CreateTextNode(DefaultLanguage));
                subElemCommon.AppendChild(subElem);

                subElem = result.CreateElement("AllowUsersChangeLanguage");
                subElem.AppendChild(result.CreateTextNode((AllowUsersChangeLanguage) ? "1" : "0"));
                subElemCommon.AppendChild(subElem);

                subElem = result.CreateElement("IncomingMailProtocol");
                subElem.AppendChild(result.CreateTextNode(((int)IncomingMailProtocol).ToString(CultureInfo.InvariantCulture)));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("IncomingMailServer");
                subElem.AppendChild(result.CreateTextNode(IncomingMailServer));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("IncomingMailPort");
                subElem.AppendChild(result.CreateTextNode(IncomingMailPort.ToString(CultureInfo.InvariantCulture)));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("OutgoingMailServer");
                subElem.AppendChild(result.CreateTextNode(OutgoingMailServer));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("OutgoingMailPort");
                subElem.AppendChild(result.CreateTextNode(OutgoingMailPort.ToString(CultureInfo.InvariantCulture)));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("ReqSmtpAuth");
                subElem.AppendChild(result.CreateTextNode((ReqSmtpAuth) ? "1" : "0"));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("AllowAdvancedLogin");
                subElem.AppendChild(result.CreateTextNode((AllowAdvancedLogin) ? "1" : "0"));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("HideLoginMode");
                subElem.AppendChild(result.CreateTextNode(((short)HideLoginMode).ToString(CultureInfo.InvariantCulture)));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("DefaultDomainOptional");
                subElem.AppendChild(result.CreateTextNode(DefaultDomainOptional));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("ShowTextLabels");
                subElem.AppendChild(result.CreateTextNode((ShowTextLabels) ? "1" : "0"));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("AutomaticCorrectLoginSettings");
                subElem.AppendChild(result.CreateTextNode((AutomaticCorrectLoginSettings) ? "1" : "0"));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("EnableLogging");
                subElem.AppendChild(result.CreateTextNode((EnableLogging) ? "1" : "0"));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("AllowAjax");
                subElem.AppendChild(result.CreateTextNode((AllowAjax) ? "1" : "0"));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("MailsPerPage");
                subElem.AppendChild(result.CreateTextNode(MailsPerPage.ToString(CultureInfo.InvariantCulture)));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("AttachmentSizeLimit");
                subElem.AppendChild(result.CreateTextNode(AttachmentSizeLimit.ToString(CultureInfo.InvariantCulture)));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("EnableAttachmentSizeLimit");
                subElem.AppendChild(result.CreateTextNode((EnableAttachmentSizeLimit) ? "1" : "0"));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("MailboxSizeLimit");
                subElem.AppendChild(result.CreateTextNode(MailboxSizeLimit.ToString(CultureInfo.InvariantCulture)));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("EnableMailboxSizeLimit");
                subElem.AppendChild(result.CreateTextNode((EnableMailboxSizeLimit) ? "1" : "0"));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("DefaultTimeZone");
                subElem.AppendChild(result.CreateTextNode(DefaultTimeZone.ToString(CultureInfo.InvariantCulture)));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("AllowUsersChangeTimeZone");
                subElem.AppendChild(result.CreateTextNode((AllowUsersChangeTimeZone) ? "1" : "0"));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("DefaultUserCharset");
                subElem.AppendChild(result.CreateTextNode(DefaultUserCharset.ToString(CultureInfo.InvariantCulture)));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("AllowUsersChangeCharset");
                subElem.AppendChild(result.CreateTextNode((AllowUsersChangeCharset) ? "1" : "0"));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("AllowDHTMLEditor");
                subElem.AppendChild(result.CreateTextNode((AllowDhtmlEditor) ? "1" : "0"));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("AllowUsersChangeEmailSettings");
                subElem.AppendChild(result.CreateTextNode((AllowUsersChangeEmailSettings) ? "1" : "0"));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("AllowDirectMode");
                subElem.AppendChild(result.CreateTextNode((AllowDirectMode) ? "1" : "0"));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("DirectModeIsDefault");
                subElem.AppendChild(result.CreateTextNode((DirectModeIsDefault) ? "1" : "0"));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("AllowInsertImage");
                subElem.AppendChild(result.CreateTextNode((AllowInsertImage) ? "1" : "0"));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("AllowBodySize");
                subElem.AppendChild(result.CreateTextNode((AllowBodySize) ? "1" : "0"));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("MaxBodySize");
                subElem.AppendChild(result.CreateTextNode(MaxBodySize.ToString(CultureInfo.InvariantCulture)));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("MaxSubjectSize");
                subElem.AppendChild(result.CreateTextNode(MaxSubjectSize.ToString(CultureInfo.InvariantCulture)));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("AllowNewUsersRegister");
                subElem.AppendChild(result.CreateTextNode((AllowNewUsersRegister) ? "1" : "0"));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("AllowUsersAddNewAccounts");
                subElem.AppendChild(result.CreateTextNode((AllowUsersAddNewAccounts) ? "1" : "0"));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("StoreMailsInDb");
                subElem.AppendChild(result.CreateTextNode((StoreMailsInDb) ? "1" : "0"));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("EnableWmServer");
                subElem.AppendChild(result.CreateTextNode((EnableWmServer) ? "1" : "0"));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("WmServerRootPath");
                subElem.AppendChild(result.CreateTextNode(WmServerRootPath));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("WmServerHost");
                subElem.AppendChild(result.CreateTextNode(WmServerHost));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("WmAllowManageXMailAccounts");
                subElem.AppendChild(result.CreateTextNode((WmAllowManageXMailAccounts) ? "1" : "0"));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("AllowContacts");
                subElem.AppendChild(result.CreateTextNode((AllowContacts) ? "1" : "0"));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("AllowCalendar");
                subElem.AppendChild(result.CreateTextNode((AllowCalendar) ? "1" : "0"));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("Imap4DeleteLikePop3");
                subElem.AppendChild(result.CreateTextNode((_imap4DeleteLikePop3) ? "1" : "0"));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("AllowLanguageOnLogin");
                subElem.AppendChild(result.CreateTextNode((AllowLanguageOnLogin) ? "1" : "0"));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("TakeImapQuota");
                subElem.AppendChild(result.CreateTextNode((TakeImapQuota) ? "1" : "0"));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("AllowUsersChangeAccountsDef");
                subElem.AppendChild(result.CreateTextNode((AllowUsersChangeAccountsDef) ? "1" : "0"));
                subElemWebMail.AppendChild(subElem);

				subElem = result.CreateElement("GlobalAddressBook");
				subElem.AppendChild(result.CreateTextNode(GlobalAddressBook.ToString()));
				subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("ViewMode");
                subElem.AppendChild(result.CreateTextNode(ViewMode.ToString()));
                subElemWebMail.AppendChild(subElem);

                subElem = result.CreateElement("DefaultTimeFormat");
                subElem.AppendChild(result.CreateTextNode(DefaultTimeFormat.ToString()));
                subElemCalendar.AppendChild(subElem);

                subElem = result.CreateElement("DefaultDateFormat");
                subElem.AppendChild(result.CreateTextNode(DefaultDateFormat.ToString()));
                subElemCalendar.AppendChild(subElem);

                subElem = result.CreateElement("ShowWeekends");
                subElem.AppendChild(result.CreateTextNode(ShowWeekends.ToString()));
                subElemCalendar.AppendChild(subElem);

                subElem = result.CreateElement("WorkdayStarts");
                subElem.AppendChild(result.CreateTextNode(WorkdayStarts.ToString()));
                subElemCalendar.AppendChild(subElem);

                subElem = result.CreateElement("WorkdayEnds");
                subElem.AppendChild(result.CreateTextNode(WorkdayEnds.ToString()));
                subElemCalendar.AppendChild(subElem);

                subElem = result.CreateElement("ShowWorkDay");
                subElem.AppendChild(result.CreateTextNode(ShowWorkDay.ToString()));
                subElemCalendar.AppendChild(subElem);

                subElem = result.CreateElement("WeekStartsOn");
                subElem.AppendChild(result.CreateTextNode(WeekStartsOn.ToString()));
                subElemCalendar.AppendChild(subElem);

                subElem = result.CreateElement("DefaultTab");
                subElem.AppendChild(result.CreateTextNode(DefaultTab.ToString()));
                subElemCalendar.AppendChild(subElem);

                subElem = result.CreateElement("DefaultCountry");
                subElem.AppendChild(result.CreateTextNode(DefaultCountry));
                subElemCalendar.AppendChild(subElem);

                subElem = result.CreateElement("DefaultTimeZone");
                subElem.AppendChild(result.CreateTextNode(DefaultTimeZoneCalendar.ToString()));
                subElemCalendar.AppendChild(subElem);

                subElem = result.CreateElement("AllTimeZones");
                subElem.AppendChild(result.CreateTextNode(AllTimeZones.ToString()));
                subElemCalendar.AppendChild(subElem);

				subElem = result.CreateElement("AllowReminders");
				subElem.AppendChild(result.CreateTextNode((AllowReminders) ? "1" : "0"));
				subElemCalendar.AppendChild(subElem);

				result.Save(filename);

                _instance = null;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                throw new WebMailSettingsException(ex);
            }
        }
    }

    public class WebMailSettingsCreator : Control
    {
        public WebmailSettings CreateWebMailSettings()
        {
            return CreateWebMailSettings(Utils.GetDataFolderPath());
        }

        public WebmailSettings CreateWebMailSettings(string dataFolder)
        {
            WebmailSettings newSettings = new WebmailSettings();
            if (this.Context != null && this.Context.Application[Constants.sessionSettings] != null)
            {
                newSettings = (WebmailSettings)this.Context.Application[Constants.sessionSettings];
            }
            else
            {
                newSettings = newSettings.CreateInstance(dataFolder);
				if (this.Context != null)
				{
					this.Context.Application.Add(Constants.sessionSettings, newSettings);
				}
            }
            return newSettings;
        }

        public WebmailSettings CreateWebMailSettings(string dataFolder, string url)
        {
            WebmailSettings newSettings = new WebmailSettings();
            if (this.Context.Application[Constants.sessionSettings] != null)
            {
                newSettings = (WebmailSettings)this.Context.Application[Constants.sessionSettings];
            }
            else
            {
                Domain dom = null;
                
                
                (new WebMailSettingsCreator()).ResetWebMailSettings();
                newSettings = newSettings.CreateInstance(dataFolder);

                if (dom != null)
                {
                    newSettings.SiteName = dom.Site_name;
                    newSettings.IncomingMailProtocol = dom.Settings_mail_protocol;
                    newSettings.IncomingMailServer = dom.Settings_mail_inc_host;
                    newSettings.IncomingMailPort = dom.Settings_mail_inc_port;
                    newSettings.OutgoingMailServer = dom.Settings_mail_out_host;
                    newSettings.OutgoingMailPort = dom.Settings_mail_out_port;
                    newSettings.ReqSmtpAuth = dom.Settings_mail_out_auth;
                    newSettings.AllowDirectMode = dom.Allow_direct_mode;
                    newSettings.DirectModeIsDefault = dom.Direct_mode_id_def;
                    newSettings.AttachmentSizeLimit = dom.Attachment_size_limit;
                    newSettings.EnableAttachmentSizeLimit = dom.Allow_attachment_limit;
                    newSettings.MailboxSizeLimit = dom.Mailbox_size_limit;
                    newSettings.EnableMailboxSizeLimit = dom.Allow_mailbox_limit;
                    newSettings.TakeImapQuota = dom.Take_quota;
                    newSettings.AllowUsersChangeEmailSettings = dom.Allow_new_users_change_settings;
                    newSettings.AllowNewUsersRegister = dom.Allow_auto_reg_on_login;
                    newSettings.AllowUsersAddNewAccounts = dom.Allow_users_add_accounts;
                    newSettings.AllowUsersChangeAccountsDef = dom.Allow_users_change_account_def;
                    newSettings.DefaultUserCharset = dom.Def_user_charset;
                    newSettings.AllowUsersChangeCharset = dom.Allow_users_change_charset;
                    newSettings.DefaultTimeZone = (short) dom.Def_user_timezone;
                    newSettings.AllowUsersChangeTimeZone = dom.Allow_users_change_timezone;
                    newSettings.MailsPerPage = (short) dom.Msgs_per_page;
                    newSettings.DefaultSkin = dom.Skin;
                    newSettings.AllowUsersChangeSkin = dom.Allow_users_change_skin;
                    newSettings.DefaultLanguage = dom.Lang;
                    newSettings.AllowUsersChangeLanguage = dom.Allow_users_change_lang;
                    newSettings.ShowTextLabels = dom.Show_text_labels;
                    newSettings.AllowAjax = dom.Allow_ajax;
                    newSettings.AllowDhtmlEditor = dom.Allow_editor;
                    newSettings.AllowContacts = dom.Allow_contacts;
                    newSettings.AllowCalendar = dom.Allow_calendar;
                    newSettings.HideLoginMode = dom.Hide_login_mode;
                    newSettings.DefaultDomainOptional = dom.Domain_to_use;
                    newSettings.AllowLanguageOnLogin = dom.Allow_choosing_lang;
                    newSettings.AllowAdvancedLogin = dom.Allow_advanced_login;
                    newSettings.AutomaticCorrectLoginSettings = dom.Allow_auto_detect_and_correct;
                    newSettings.ViewMode = dom.ViewMode;
                }

                this.Context.Application.Add(Constants.sessionSettings, newSettings);
            }
            return newSettings;
        }

        public void ResetWebMailSettings()
        {
            if (this.Context.Application[Constants.sessionSettings] != null)
            {
                this.Context.Application.Remove(Constants.sessionSettings);
            }
        }
    }
}