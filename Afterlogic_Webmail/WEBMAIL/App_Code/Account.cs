using System;
using System.Globalization;
using MailBee.Mime;
using System.Collections;

namespace WebMail
{
	public class AccountComparer : IComparer
	{
		#region IComparer Members

		public int Compare(object x, object y)
		{
			Account xAccount = x as Account;
			Account yAccount = y as Account;
			if (xAccount == null || yAccount == null) return 0;
			return (xAccount.Email == yAccount.Email) ? 0 : xAccount.Email.CompareTo(yAccount.Email);
		}

		#endregion
	}

    /// <summary>
    /// This class implements an API for managing users accounts.
    /// </summary>
    [Serializable]
	public class Account
	{
		#region Fields
		protected int _id;
		protected int _idUser;
		protected bool _defaultAccount;
		protected bool _deleted;
		protected string _email;
		protected string _domainName;
        protected Domain _domain;
        protected IncomingMailProtocol _mailProtocol;
		protected string _mailIncHost;
		protected string _mailIncLogin;
		protected string _mailIncPassword;
		protected int _mailIncPort;
		protected string _mailOutHost;
		protected string _mailOutLogin;
		protected string _mailOutPassword;
		protected int _mailOutPort;
		protected bool _mailOutAuthentication;
		protected string _friendlyName;
		protected bool _useFriendlyName;
		protected DefaultOrder _defaultOrder;
		protected bool _getMailAtLogin;
		protected MailMode _mailMode;
		protected short _mailsOnServerDays;
		protected string _signature;
		protected SignatureType _signatureType;
		protected SignatureOptions _signatureOptions;
		protected User _user;
		protected string _delimiter;
		protected long _mailbox_size;
        protected int _idDomain;
        protected bool _mailing_list;
        protected int _imap_quota;
        protected string _namespace;
        #endregion

		#region Properties
		public int ID
		{
			get { return _id; }
			set { _id = value; }
		}

		public int IDUser
		{
			get { return _idUser; }
			set { _idUser = value; }
		}

        public int IDDomain
        {
            get { return _idDomain; }
            set { _idDomain = value; }
        }
        
        public bool DefaultAccount
		{
			set { _defaultAccount = value;}
			get { return _defaultAccount; }
		}

		public bool Deleted
		{
			get { return _deleted; }
			set { _deleted = value; }
		}

		public string Email
		{
			get { return _email; }
			set {
				_email = value;
				_domainName = EmailAddress.GetDomainFromEmail(value);
			}
		}

		public string DomainName
		{
			get { return _domainName; }
		}

        public Domain Domain
        {
            get 
            {
                return Domain.GetDomain(IDDomain);
            }
        }
        
        public string MailIncomingHost
		{
			get { return _mailIncHost; }
			set { _mailIncHost = value; }
		}

		public string MailIncomingLogin
		{
			get { return _mailIncLogin; }
			set { _mailIncLogin = value; }
		}

		public string MailIncomingPassword
		{
			get
			{
				return _mailIncPassword;
			}
			set
			{
				_mailIncPassword = value;
			}
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

		public string MailOutgoingLogin
		{
			get { return _mailOutLogin; }
			set { _mailOutLogin = value; }
		}

		public string MailOutgoingPassword
		{
			get { return _mailOutPassword; }
			set { _mailOutPassword = value; }
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

		public string FriendlyName
		{
			get { return _friendlyName; }
			set { _friendlyName = value; }
		}

		public bool UseFriendlyName
		{
			get { return _useFriendlyName; }
			set { _useFriendlyName = value; }
		}

		public bool GetMailAtLogin
		{
			get { return _getMailAtLogin; }
			set { _getMailAtLogin = value; }
		}

		public short MailsOnServerDays
		{
			get { return _mailsOnServerDays; }
			set { _mailsOnServerDays = value; }
		}

		public string Signature
		{
			get { return _signature; }
			set { _signature = value; }
		}

		public DefaultOrder DefaultOrder
		{
			get { return _defaultOrder; }
			set { _defaultOrder = value; }
		}

		public MailMode MailMode
		{
			get { return _mailMode; }
			set { _mailMode = value; }
		}

		public SignatureType SignatureType
		{
			get { return _signatureType; }
			set { _signatureType = value; }
		}

		public SignatureOptions SignatureOptions
		{
			get { return _signatureOptions; }
			set { _signatureOptions = value; }
		}

		public IncomingMailProtocol MailIncomingProtocol
		{
			get { return _mailProtocol; }
			set { _mailProtocol = value; }
		}

		public User UserOfAccount
		{
			get { return _user; }
			set
			{
				_user = value;
				if (_user != null)
				{
					_idUser = _user.ID;
				}
			}
		}

		public string Delimiter
		{
			get { return _delimiter; }
			set { _delimiter = value; }
		}

		public long MailboxSize
		{
			get { return _mailbox_size; }
			set { _mailbox_size = value; }
		}

        public bool MailingList
        {
            get { return _mailing_list; }
            set { _mailing_list = value; }
        }

        public int Imap_quota
        {
            get { return _imap_quota; }
            set { _imap_quota = value; }
        }

        public string Namespace
        {
            get { return _namespace; }
            set { _namespace = value; }
        }
        
		
		public bool IsDemo = false;
		
		#endregion

		public Account() : this(string.Empty) {}

		public Account(string dataFolder)
		{
			WebmailSettings settings;
			if (dataFolder != string.Empty)
			{
				settings = (new WebMailSettingsCreator()).CreateWebMailSettings(dataFolder);
			}
			else
			{
				settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
			}
			_id = -1;
			_idUser = -1;
            _idDomain = 0;
			_defaultAccount = true;
			_deleted = false;
			_email = "";
			_mailProtocol = settings.IncomingMailProtocol;
			_mailIncHost = settings.IncomingMailServer;
			_mailIncLogin = "";
			_mailIncPassword = "";
			_mailIncPort = settings.IncomingMailPort;
			_mailOutHost = settings.OutgoingMailServer;
			_mailOutLogin = "";
			_mailOutPassword = "";
			_mailOutPort = settings.OutgoingMailPort;
			_mailOutAuthentication = settings.ReqSmtpAuth;
			_friendlyName = "";
			_useFriendlyName = true;
			_defaultOrder = DefaultOrder.DateDesc;
			_getMailAtLogin = true;
			_mailMode = MailMode.LeaveMessagesOnServer;
			_mailsOnServerDays = 1;
			_signature = "";
			_signatureType = SignatureType.Plain;
			_signatureOptions = SignatureOptions.DontAddSignature;
			_user = null;
			_delimiter = "/";
			_mailbox_size = 0;
            _mailing_list = false;
            _imap_quota = -1;
            _namespace = "";
        }

		public Account(int id_account, bool def_acct, bool deleted, string email, IncomingMailProtocol mail_protocol,
			string mail_inc_host, string mail_inc_login, string mail_inc_pass, int mail_inc_port, string mail_out_host,
			string mail_out_login, string mail_out_pass, int mail_out_port, bool mail_out_auth, string friendly_nm,
			bool use_friendly_nm, DefaultOrder def_order, bool getmail_at_login, MailMode mail_mode,
			short mails_on_server_days, string signature, SignatureType signature_type, SignatureOptions signature_opt,
			string delimiter, long mailbox_size, string Namespace)
		{
			_id = id_account;
			_defaultAccount = def_acct;
			_deleted = deleted;
			_email = email;
			_mailProtocol = mail_protocol;
			_mailIncHost = mail_inc_host;
			_mailIncLogin = mail_inc_login;
			_mailIncPassword = mail_inc_pass;
			_mailIncPort = mail_inc_port;
			_mailOutHost = mail_out_host;
			_mailOutLogin = mail_out_login;
			_mailOutPassword = mail_out_pass;
			_mailOutPort = mail_out_port;
			_mailOutAuthentication = mail_out_auth;
			_friendlyName = friendly_nm;
			_useFriendlyName = use_friendly_nm;
			_defaultOrder = def_order;
			_getMailAtLogin = getmail_at_login;
			_mailMode = mail_mode;
			_mailsOnServerDays = mails_on_server_days;
			_signature = signature;
			_signatureType = signature_type;
			_signatureOptions = signature_opt;
			_delimiter = delimiter;
			_mailbox_size = mailbox_size;
            _idDomain = 0;
            _mailing_list = false;
            _namespace = Namespace;
		}

        public Account(int id_account, bool def_acct, bool deleted, string email, IncomingMailProtocol mail_protocol,
			string mail_inc_host, string mail_inc_login, string mail_inc_pass, int mail_inc_port, string mail_out_host,
			string mail_out_login, string mail_out_pass, int mail_out_port, bool mail_out_auth, string friendly_nm,
			bool use_friendly_nm, DefaultOrder def_order, bool getmail_at_login, MailMode mail_mode,
			short mails_on_server_days, string signature, SignatureType signature_type, SignatureOptions signature_opt,
			string delimiter, long mailbox_size, int idDomain, bool mailing_list, int imap_quota, string Namespace)
        {
            _id = id_account;
            _defaultAccount = def_acct;
            _deleted = deleted;
            _email = email;
            _mailProtocol = mail_protocol;
            _mailIncHost = mail_inc_host;
            _mailIncLogin = mail_inc_login;
            _mailIncPassword = mail_inc_pass;
            _mailIncPort = mail_inc_port;
            _mailOutHost = mail_out_host;
            _mailOutLogin = mail_out_login;
            _mailOutPassword = mail_out_pass;
            _mailOutPort = mail_out_port;
            _mailOutAuthentication = mail_out_auth;
            _friendlyName = friendly_nm;
            _useFriendlyName = use_friendly_nm;
            _defaultOrder = def_order;
            _getMailAtLogin = getmail_at_login;
            _mailMode = mail_mode;
            _mailsOnServerDays = mails_on_server_days;
            _signature = signature;
            _signatureType = signature_type;
            _signatureOptions = signature_opt;
            _delimiter = delimiter;
            _mailbox_size = mailbox_size;
            _idDomain = idDomain;
            _mailing_list = mailing_list;
            _imap_quota = imap_quota;
            _namespace = Namespace;
        }

		public void Update(bool updateUser)
		{
			if (IsDemo) return;

			DbManagerCreator creator = new DbManagerCreator();
			DbManager dbMan = creator.CreateDbManager();
			try
			{
				dbMan.Connect();
				dbMan.UpdateAccount(this, updateUser);
			}
			finally
			{
				dbMan.Disconnect();
			}
		}

		public void ChangeAccountDefault(bool isDefault)
		{
			if ((_defaultAccount == false) && isDefault)
			{
				DbManager dbMan = (new DbManagerCreator()).CreateDbManager(this);
				try
				{
					dbMan.Connect();
					int nonDefCount = dbMan.GetNotDefaultAccountCount(_email, _mailIncLogin, _mailIncPassword);
					if (nonDefCount > 1)
					{
						throw new WebMailException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("PROC_CANT_LOG_NONDEF"));
					}
					_defaultAccount = true;
				}
				finally
				{
					dbMan.Disconnect();
				}
			}
			else if (_defaultAccount && (isDefault == false))
			{
				if (_user != null)
				{
					AccountCollection accounts = _user.GetUserAccounts();
					bool hasDefault = false;
					foreach (Account acct in accounts)
					{
						if (acct.ID == _id) continue;
						if (!acct.DefaultAccount) continue;
						hasDefault = true;
						break;
					}
					if (hasDefault) _defaultAccount = false;
				}
			}
		}

		public static Account LoginAccount(string email, string login, string password)
		{
			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
			return LoginAccount(email, login, password, settings.IncomingMailServer, settings.IncomingMailProtocol, 
				settings.IncomingMailPort, settings.OutgoingMailServer, settings.OutgoingMailPort, 
				settings.ReqSmtpAuth, false, false, "");
		}

		public static Account LoginAccount(string email, string login, string password, string incomingMailServer, 
			IncomingMailProtocol incomingMailProtocol, int incomingPort, string outgoingMailServer, 
			int outgoingMailPort, bool useSmtpAuthentication, bool signAutomatically, bool advanced_login, 
			string language)
		{
			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
			string tempLogin = login;
			if (!advanced_login)
			{
				switch (settings.HideLoginMode)
				{
					case LoginMode.HideLoginFieldLoginIsAccount:
						login = EmailAddress.Parse(email).GetAccountName();
						tempLogin = null;
						break;
					case LoginMode.HideLoginFieldLoginIsEmail:
						login = email;
						tempLogin = null;
						break;
					case LoginMode.HideEmailField:
					case LoginMode.HideEmailFieldDisplayDomainAfterLogin:
						email = string.Format("{0}@{1}", login, settings.DefaultDomainOptional);
						break;
					case LoginMode.HideEmailFieldLoginIsLoginAndDomain:
					case LoginMode.HideEmailFieldDisplayDomainAfterLoginAndLoginIsLoginAndDomain:
						email = string.Format("{0}@{1}", login, settings.DefaultDomainOptional);
						login = email;
						tempLogin = email;
						break;
				}
			}

			WebmailResourceManager resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();

			string domainName = EmailAddress.GetDomainFromEmail(email);
			Domain domain = LoadDomainFromDb(domainName);
/*
			if (domain != null && (domain.MailIncomingProtocol == IncomingMailProtocol.WMServer ||
				incomingMailProtocol == IncomingMailProtocol.WMServer) && settings.EnableWmServer)
			{
				tempLogin = EmailAddress.GetAccountNameFromEmail(tempLogin);
				if (tempLogin != null && tempLogin != EmailAddress.GetAccountNameFromEmail(email))
				{
					throw new WebMailException(resMan.GetString("ErrorPOP3IMAP4Auth"));
				}
			}
*/ 
			Account result = LoadFromDb(email, tempLogin, null);
			if (result == null)
			{
                if (!settings.AllowNewUsersRegister)
                {
                    throw new WebMailException(resMan.GetString("ErrorPOP3IMAP4Auth"));
                }
                if (domain != null)
                {
                    if (settings.AllowNewUsersRegister && domain.MailIncomingProtocol == IncomingMailProtocol.WMServer)
                    {
                        throw new WebMailException(resMan.GetString("ErrorPOP3IMAP4Auth"));
                    }
                }

				Account acct = new Account();
				if (domain != null && (domain.MailIncomingProtocol != IncomingMailProtocol.WMServer || settings.EnableWmServer))
				{
					acct.Email = email;
					if (domain.MailIncomingProtocol == IncomingMailProtocol.WMServer)
					{
						acct.MailIncomingLogin = EmailAddress.GetAccountNameFromEmail(email);
					}
					else
					{
						acct.MailIncomingLogin = login;
					}
					acct.MailIncomingPassword = password;
					acct.MailIncomingHost = domain.MailIncomingHost;
					acct.MailIncomingPort = domain.MailIncomingPort;
					acct.MailIncomingProtocol = domain.MailIncomingProtocol;
					acct.MailOutgoingHost = domain.MailOutgoingHost;
					acct.MailOutgoingPort = domain.MailOutgoingPort;
					acct.MailOutgoingAuthentication = domain.MailOutgoingAuthentication;
					acct.IDDomain = domain.ID;
				}
				else
				{
					acct.Email = email;
					acct.MailIncomingLogin = login;
					acct.MailIncomingPassword = password;
					acct.MailIncomingHost = incomingMailServer;
					acct.MailIncomingPort = incomingPort;
					acct.MailIncomingProtocol = incomingMailProtocol;
					acct.MailOutgoingHost = outgoingMailServer;
					acct.MailOutgoingPort = outgoingMailPort;
					acct.MailOutgoingAuthentication = useSmtpAuthentication;
					acct.IDDomain = 0;
				}

				bool isWmServer = (acct.MailIncomingProtocol == IncomingMailProtocol.WMServer);
				bool isIMAP4 = (acct.MailIncomingProtocol == IncomingMailProtocol.Imap4);
				if (!isWmServer)
				{
					MailServerStorage mss = MailServerStorageCreator.CreateMailServerStorage(acct);
					try
					{
						mss.Connect();
					}
					finally
					{
						if (mss.IsConnected()) mss.Disconnect();
					}
                }
                User usr = null;
                try
                {
                    usr = User.CreateUser();
                    FolderSyncType syncType = Folder.DefaultInboxSyncType;
                    if (settings.DirectModeIsDefault)
                    {
                        syncType = FolderSyncType.DirectMode;
                    }
                    if (isWmServer)
                    {
                        syncType = FolderSyncType.AllHeadersOnly;
                    }
                    if (isIMAP4)
                    {
                        syncType = FolderSyncType.DirectMode;
                    }

                    result = usr.CreateAccount(acct, syncType, advanced_login);
                }
                catch (WebMailException)
                {
                    if (null != usr)
                    {
                        User.DeleteUserSettings(usr.ID);
                    }
                    throw;
                }
            }
			else
			{
                if (result.UserOfAccount.Deleted)
                {
                    throw new WebMailException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("ErrorMaximumUsersLicenseIsExceeded"));
                }

                if (result.Deleted)
                {
                    throw new WebMailException("Your account is inactive, please contact the system administrator on this.");
                }

                if (string.Compare(result.MailIncomingPassword, password, false, CultureInfo.InvariantCulture) != 0)
				{
					result.MailIncomingPassword = password;
					MailServerStorage mss = MailServerStorageCreator.CreateMailServerStorage(result);
					try
					{
						mss.Connect();
					}
					finally
					{
						if (mss.IsConnected()) mss.Disconnect();
					}
				}

				if (result.DefaultAccount == false)
				{
					DbManagerCreator creator = new DbManagerCreator();
					DbManager dbMan = creator.CreateDbManager();
					int nonDefaultCount;
					try
					{
						dbMan.Connect();
						nonDefaultCount = dbMan.GetNotDefaultAccountCount(email, login, password);
					}
					finally
					{
						dbMan.Disconnect();
					}
					if (nonDefaultCount > 1)
					{
						throw new WebMailException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("PROC_CANT_LOG_NONDEF"));
					}
				}
			}
			if ((result != null) && (result.UserOfAccount != null) && (result.UserOfAccount.Settings != null))
			{
				result.UserOfAccount.Settings.LastLogin = DateTime.Now;
				result.UserOfAccount.Settings.LoginsCount++;
				if (!string.IsNullOrEmpty(language) && settings.AllowLanguageOnLogin)
				{
					result.UserOfAccount.Settings.DefaultLanguage = language;
				}

                if (result.MailIncomingProtocol == IncomingMailProtocol.Imap4)
                {
                    ImapStorage imapStorage = new ImapStorage(result);
                    try
                    {
                        imapStorage.Connect();
                        result.Namespace = imapStorage.GetNamespace();

                        if (settings.TakeImapQuota)
                        {
                            if (imapStorage.IsQuotaSupported())
                            {
                                if (result.Imap_quota != 0)
                                {
                                    result.Imap_quota = 1;
                                    result.UserOfAccount.Settings.MailboxLimit = imapStorage.GetMailStorageSize();
                                }
                            }
                            else
                            {
                                result.Imap_quota = -1;
                            }
                        }
                    }
                    finally
                    {
                        imapStorage.Disconnect();
                    }

                }

                result.Update(true);
			}

            // Create Trash Folder if Trash not found in the list
            if (result != null && result.MailIncomingProtocol == IncomingMailProtocol.WMServer)
            {
                MailServerStorage wmServerStorage = MailServerStorageCreator.CreateMailServerStorage(result);

                try
                {
                    wmServerStorage.Connect();
                    FolderCollection fc = wmServerStorage.GetFolders();

                    FileSystem fs = new FileSystem(result.Email, result.ID, true);
                    Folder fld = fc[FolderType.Trash];
                    if (fld == null)
                    {
                        string fullPath = result.Delimiter + Constants.FolderNames.Trash;
                        const string name = Constants.FolderNames.Trash;
                        if (!settings.StoreMailsInDb) fs.CreateFolder(Constants.FolderNames.Trash);
                        fld = new Folder(result.ID, -1, fullPath, name);
                        wmServerStorage.CreateFolder(fld);

                        DbManagerCreator creator = new DbManagerCreator();
                        DbManager dbMan = creator.CreateDbManager();
                        try
                        {
                            dbMan.Connect();
                            dbMan.CreateFolder(result.ID, -1, FolderType.Trash, name, fullPath, FolderSyncType.AllHeadersOnly, false, 3);
                        }
                        finally
                        {
                            dbMan.Disconnect();
                        }
                    }
                }
                catch { }
            }            
            
            return result;
		}

		public static Account LoadFromDb(int id_acct, int id_user)
		{
			return LoadFromDb(id_acct, id_user, true);
		}

		public static Account LoadFromDb(int id_acct, int id_user, bool with_mailing_lists)
		{
			return LoadFromDb(id_acct, id_user, with_mailing_lists, string.Empty);
		}

		public static Account LoadFromDb(int id_acct, int id_user, string dataFolder)
		{
			return LoadFromDb(id_acct, id_user, true, dataFolder);
		}

		private static Account LoadFromDb(int id_acct, int id_user, bool with_mailing_lists, string dataFolder)
		{
			Account newAccount;

			DbManagerCreator creator = new DbManagerCreator();
			DbManager dbMan;
			if (dataFolder == string.Empty)
			{
				dbMan = creator.CreateDbManager();
			}
			else
			{
				dbMan = creator.CreateDbManager(dataFolder);
			}
			try
			{
				dbMan.Connect();
				newAccount = dbMan.SelectAccountData(id_acct, id_user, with_mailing_lists);
			}
			finally
			{
				dbMan.Disconnect();
			}
			return newAccount;
		}

		public static Domain LoadDomainFromDb(string domainName)
		{
			Domain newDomain;

			DbManagerCreator creator = new DbManagerCreator();
			DbManager dbMan = creator.CreateDbManager();
			try
			{
				dbMan.Connect();
				newDomain = dbMan.SelectDomainData(domainName);
			}
			finally
			{
				dbMan.Disconnect();
			}
			return newDomain;
		}

		public static Account LoadFromDb(string email, string login, string password)
		{
			Account newAccount;

			DbManagerCreator creator = new DbManagerCreator();
			DbManager dbMan = creator.CreateDbManager();
			try
			{
				dbMan.Connect();
				newAccount = dbMan.SelectAccountData(email, login, password);
			}
			finally
			{
				dbMan.Disconnect();
			}
			return newAccount;
		}

		public static void DeleteFromDb(Account acct)
		{
			DbManagerCreator creator = new DbManagerCreator();
			DbManager dbMan = creator.CreateDbManager();
			try
			{
				dbMan.Connect();
				dbMan.DeleteAccount(acct._id);
			}
			catch (WebMailDatabaseException ex)
			{
                Log.WriteException(ex);
                throw new WebMailDatabaseException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("PROC_CANT_DEL_ACCT_BY_ID"), ex);
			}
			finally
			{
				dbMan.Disconnect();
			}

            if (acct.MailIncomingProtocol != IncomingMailProtocol.WMServer)
            {
                FileSystem fs = new FileSystem(acct._email, acct._id, true);
                fs.DeleteFolder("");

                fs = new FileSystem(acct._email, acct._id, false);
                fs.DeleteFolder("");
            }

			if (acct.MailIncomingProtocol == IncomingMailProtocol.WMServer)
			{
				WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
				try
				{
					if (settings.EnableWmServer && settings.WmAllowManageXMailAccounts)
					{
						WMServerStorage storage = new WMServerStorage(acct);
                        storage.DeleteUser(EmailAddress.GetDomainFromEmail(acct.Email), EmailAddress.GetAccountNameFromEmail(acct.MailIncomingLogin));
					}
				}
				catch (Exception ex)
				{
					Log.WriteException(ex);
				}
			}
		}

        public int CompareTo(object obj)
        {
            Account aobj = obj as Account;
            if (aobj == null) return 0;
            return (Email == aobj.Email) ? 0 : Email.CompareTo(aobj.Email);
        }

        public static void UpdateAccountsByDomain(string domain, int old_domain, int id_domain, short mail_protocol)
        {
            DbManagerCreator creator = new DbManagerCreator();
            DbManager dbMan = creator.CreateDbManager();
            try
            {
                dbMan.Connect();
                dbMan.UpdateAccountsByDomain(domain, old_domain, id_domain, mail_protocol);
            }
            catch (WebMailDatabaseException ex)
            {
                Log.WriteException(ex);
                throw new WebMailDatabaseException();
            }
            finally
            {
                dbMan.Disconnect();
            }
        }



	}// END CLASS DEFINITION Account

    public class AccountCollection : CollectionBase
    {
        public AccountCollection()
        {
        }

        public Account this[int index]
        {
            get { return ((Account)List[index]); }
            set { List[index] = value; }
        }

        public Account GetItem(int ID)
        {
            foreach (Account adm in List)
            {
                if (adm.ID == ID)
                {
                    return adm;
                }
            }
            return null;
        }

        public int Add(Account value)
        {
            return (List.Add(value));
        }

        public int IndexOf(Account value)
        {
            return (List.IndexOf(value));
        }

        public void Insert(int index, Account value)
        {
            List.Insert(index, value);
        }

        public void Remove(Account value)
        {
            List.Remove(value);
        }

        public bool Contains(Account value)
        {
            return (List.Contains(value));
        }

        public static void Sort(AccountCollection accounts)
        {
            for (int i = 0; i < accounts.Count; i++)
            {
                for (int j = 0; j < accounts.Count; j++)
                {
                    int compareResult = accounts[j].CompareTo(accounts[i]);
                    if (compareResult > 0)
                    {
                        Account temp = accounts[j];
                        accounts[j] = accounts[i];
                        accounts[i] = temp;
                    }
                }
            }
        }

    }

}
