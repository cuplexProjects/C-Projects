using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using MailBee.Html;
using MailBee.ImapMail;
using MailBee.Mime;

namespace WebMail
{
    /// <summary>
    /// This class implements various base WebMail actions, e.g. managing folders/messages, 
    /// sending messages, add contacts, etc. It's abstracted from storage type, no matter if 
    /// storage is a database or a mail server.
    /// </summary>
    public class BaseWebMailActions
    {
        protected Account _acct = null;
        protected Page _webPage = null;
        protected bool _isMoveError = false;

        public Account CurrentAccount
        {
            get { return _acct; }
        }

        public Page CurrentPage
        {
            get { return _webPage; }
        }

        public bool IsMoveError
        {
            get { return _isMoveError; }
        }

        public BaseWebMailActions(Account acct, Page page)
        {
            _acct = acct;
            _webPage = page;
        }


        #region Contacts
        public void AddContacts(int id_group, AddressBookContact[] contacts)
        {
            AddressBookStorage adStorage = AddressBookStorageCreator.CreateAddressBookStorage(CurrentAccount);
            try
            {
                adStorage.Connect();
                adStorage.AddContactsToGroup(id_group, contacts);
            }
            finally
            {
                adStorage.Disconnect();
            }
        }

        public void DeleteContacts(AddressBookContact[] contacts, AddressBookGroup[] groups)
        {
            if ((_acct != null) && (_acct.UserOfAccount != null))
            {
                AddressBookStorage abStorage = AddressBookStorageCreator.CreateAddressBookStorage(_acct);
                try
                {
                    abStorage.Connect();
                    abStorage.DeleteAddressBookContactsGroups(contacts, groups);
                }
                finally
                {
                    abStorage.Disconnect();
                }
            }
            else
            {
                if (_acct == null)
                    Log.WriteLine("DeleteContacts", "Account is null.");
                else if (_acct.UserOfAccount == null)
                    Log.WriteLine("DeleteContacts", "User is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }
        
        public AddressBookContact GetContact(long id_addr)
        {
            if (_acct != null)
            {
                AddressBookStorage abStorage = AddressBookStorageCreator.CreateAddressBookStorage(_acct);
                try
                {
                    abStorage.Connect();
                    return abStorage.GetAddressBookContact(id_addr);
                }
                finally
                {
                    abStorage.Disconnect();
                }
            }
            Log.WriteLine("GetContact", "Account is null.");
            throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
        }

        public AddressBookContact GetContact(string email)
        {
            if (_acct == null)
            {
                Log.WriteLine("GetContact", "Account is null.");
                throw new WebMailSessionException(
                    (new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
            AddressBookStorage abStorage = AddressBookStorageCreator.CreateAddressBookStorage(_acct);
            try
            {
                abStorage.Connect();
                return abStorage.GetAddressBookContact(email);
            }
            finally
            {
                abStorage.Disconnect();
            }
        }

        public AddressBookGroupContact[] GetContactsGroups(int page, short sort_field, short sort_order, int id_group, string look_for, int look_for_type, out int contacts_count, out int groups_count)
        {
            if (_acct != null)
            {
                if (look_for == null) look_for = string.Empty;
                AddressBookStorage abStorage = AddressBookStorageCreator.CreateAddressBookStorage(_acct);
                try
                {
                    abStorage.Connect();

                    contacts_count = abStorage.GetAddressBookContactsCount(look_for, look_for_type);
                    groups_count = abStorage.GetAddressBookGroupsCount(look_for, look_for_type);

                    return abStorage.LoadAddressBookContactsGroups(page, sort_field, sort_order, id_group, look_for, look_for_type);
                }
                finally
                {
                    abStorage.Disconnect();
                }
            }
            Log.WriteLine("GetContactsGroups", "Account is null.");
            throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
        }

        public string GetGroupEmails(AddressBookGroup group)
        {
            if (group == null) return "";
            StringBuilder sb = new StringBuilder();
            foreach (AddressBookContact contact in group.Contacts)
            {
                switch (contact.PrimaryEmail)
                {
                    case ContactPrimaryEmail.Personal:
                        if (!string.IsNullOrEmpty(contact.HEmail)) sb.AppendFormat("{0}, ", contact.HEmail);
                        break;
                    case ContactPrimaryEmail.Business:
                        if (!string.IsNullOrEmpty(contact.BEmail)) sb.AppendFormat("{0}, ", contact.BEmail);
                        break;
                    case ContactPrimaryEmail.Other:
                        if (!string.IsNullOrEmpty(contact.OtherEmail)) sb.AppendFormat("{0}, ", contact.OtherEmail);
                        break;
                }
            }
            if (sb.Length > 1) sb.Remove(sb.Length - 2, 2); // remove last ', '
            return sb.ToString();
        }

        public AddressBookGroup GetGroup(int id_group)
        {
            if (_acct != null)
            {
                AddressBookStorage abStorage = AddressBookStorageCreator.CreateAddressBookStorage(_acct);
                AddressBookGroup group;
                try
                {
                    abStorage.Connect();
                    group = abStorage.GetAddressBookGroup(id_group);
                }
                finally
                {
                    abStorage.Disconnect();
                }
                return group;
            }
            Log.WriteLine("GetGroup", "Account is null.");
            throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
        }

        public AddressBookGroup[] GetGroups()
        {
            if (_acct != null)
            {
                AddressBookStorage abStorage = AddressBookStorageCreator.CreateAddressBookStorage(_acct);
                AddressBookGroup[] groups;
                try
                {
                    abStorage.Connect();
                    groups = abStorage.GetAddressBookGroups();
                }
                finally
                {
                    abStorage.Disconnect();
                }
                return groups;
            }
            Log.WriteLine("GetGroups", "Account is null.");
            throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
        }

        public AddressBookContact NewContact(AddressBookContact newContact)
        {
            if (_acct != null)
            {
                AddressBookStorage abStorage = AddressBookStorageCreator.CreateAddressBookStorage(_acct);
                try
                {
                    abStorage.Connect();
                    return abStorage.CreateAddressBookContact(newContact);
                }
                finally
                {
                    abStorage.Disconnect();
                }
            }
            else
            {
                Log.WriteLine("NewContact", "Account is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public void NewGroup(AddressBookGroup group)
        {
            if (_acct != null)
            {
                AddressBookStorage abStorage = AddressBookStorageCreator.CreateAddressBookStorage(_acct);
                try
                {
                    abStorage.Connect();
                    abStorage.CreateAddressBookGroup(group);
                }
                finally
                {
                    abStorage.Disconnect();
                }
            }
            else
            {
                Log.WriteLine("NewGroup", "Account is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public void UpdateContact(AddressBookContact updateContact)
        {
            if (_acct != null)
            {
                AddressBookStorage abStorage = AddressBookStorageCreator.CreateAddressBookStorage(_acct);
                try
                {
                    abStorage.Connect();
                    abStorage.UpdateAddressBookContact(updateContact);
                }
                finally
                {
                    abStorage.Disconnect();
                }
            }
            else
            {
                Log.WriteLine("UpdateContact", "Account is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public void UpdateContactsSettings(bool whiteListing, short contactsPerPage)
        {
            if ((_acct != null) && (_acct.UserOfAccount != null) && (_acct.UserOfAccount.Settings != null))
            {
                _acct.UserOfAccount.Settings.WhiteListing = whiteListing;
                _acct.UserOfAccount.Settings.ContactsPerPage = contactsPerPage;
                _acct.Update(true);
            }
            else
            {
                if (_acct == null)
                {
                    Log.WriteLine("UpdateContactSettings", "User is null.");
                }
                else if (_acct.UserOfAccount == null)
                {
                    Log.WriteLine("UpdateContactsSettings", "User is null.");
                }
                else if (_acct.UserOfAccount.Settings == null)
                {
                    Log.WriteLine("UpdateContactsSettings", "User is null.");
                }
                throw new WebMailException("User is null.");
            }
        }

        public void UpdateGroup(AddressBookGroup group)
        {
            if (_acct != null)
            {
                AddressBookStorage abStorage = AddressBookStorageCreator.CreateAddressBookStorage(_acct);
                try
                {
                    abStorage.Connect();
                    abStorage.UpdateAddressBookGroup(group);
                }
                finally
                {
                    abStorage.Disconnect();
                }
            }
            else
            {
                Log.WriteLine("UpdateGroup", "Account is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public void IncrementContactsFrequency(EmailAddressCollection emails)
        {
            DbStorage storage = DbStorageCreator.CreateDatabaseStorage(_acct);
            try
            {
                storage.Connect();
                foreach (EmailAddress email in emails)
                {
                    AddressBookContact contact = storage.GetAddressBookContact(email.Email);
                    if (contact != null)
                    {
                        contact.UseFrequency++;
                        UpdateContact(contact);
                    }
                    else
                    {
                        contact = new AddressBookContact();
                        contact.HEmail = email.Email;
                        NewContact(contact);
                    }
                }
            }
            finally
            {
                storage.Disconnect();
            }
        }
        #endregion

        #region Account
        public void DeleteAccount(int id_acct)
        {
            if ((_acct != null) && (_acct.UserOfAccount != null))
            {
                if (_acct.IsDemo) return;

                AccountCollection userAccouts = _acct.UserOfAccount.GetUserAccounts();
                if (userAccouts.Count == 1)
                {
                    Account.DeleteFromDb(_acct);
                    User.DeleteUser(_acct.IDUser);
                    _acct = null;
                    return;
                }

            	Account acct = Account.LoadFromDb(id_acct, _acct.UserOfAccount.ID, false);
            	if (acct != null)
            	{
            		if (acct.DefaultAccount)
            		{
            			bool hasDefault = false;
            			foreach (Account account in userAccouts)
            			{
            				if (account.ID != acct.ID)
            				{
            					if (account.DefaultAccount)
            					{
            						hasDefault = true;
            						break;
            					}
            				}
            			}
            			if (!hasDefault)
            			{
            				throw new WebMailException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("ACCT_CANT_DEL_LAST_DEF_ACCT"));
            			}
            		}
            		Account.DeleteFromDb(acct);
            		if (_acct.ID == id_acct)
            		{
            			userAccouts = _acct.UserOfAccount.GetUserAccounts();
            			_acct = (userAccouts.Count > 0) ? userAccouts[0] : null;
            		}
            	}
            }//if ((_acct != null) && (_acct.UserOfAccount != null))
            else
            {
                if (_acct == null)
					Log.WriteLine("DeleteAccount", "Account is null.");
                else if (_acct.UserOfAccount == null)
					Log.WriteLine("DeleteAccount", "User is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public Account GetAccount(int id_acct)
        {
            Account acct = Account.LoadFromDb(id_acct, _acct.UserOfAccount.ID, false);
            if (acct == null)
            {
                if (_acct == null)
                {
                    Log.WriteLine("GetAccount", "Account is null.");
                    throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
                }
                if (_acct.UserOfAccount == null)
                {
                    Log.WriteLine("GetAccount", "User is null.");
                    throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
                }
                acct = _acct;
            }
            return acct;
        }

        public AccountCollection GetAccounts(int user_id)
        {
            User u = User.LoadUser(user_id);
            if (u != null)
            {
                AccountCollection accounts = u.GetUserAccounts();
                AccountCollection.Sort(accounts);
                return accounts;
            }
            return null;
        }

        public Account LoginAccount(string email, string login, string password, string inc_server,
            IncomingMailProtocol inc_protocol, int inc_port, string out_server, int out_port, bool smtp_auth,
            bool sign_me, bool advanced_login, string language)
        {
            _acct = Account.LoginAccount(email, login, password, inc_server, inc_protocol, inc_port, out_server,
                out_port, smtp_auth, sign_me, advanced_login, language);

            if ((_acct == null) || (_acct.UserOfAccount == null) || (_acct.UserOfAccount.Settings == null))
            {
                if (_acct == null)
                {
                    Log.WriteLine("LoginAccount", "Account is null.");
                }
                else if (_acct.UserOfAccount == null)
                {
                    Log.WriteLine("LoginAccount", "User is null.");
                }
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
            return _acct;
        }

        public int NewAccount(bool def_acct, IncomingMailProtocol mail_protocol, int mail_inc_port, int mail_out_port, bool mail_out_auth, bool use_friendly_name, short mails_on_server_days, MailMode mail_mode, bool getmail_at_login, int inbox_sync_type, string friendly_nm, string email, string mail_inc_host, string mail_inc_login, string mail_inc_pass, string mail_out_host, string mail_out_login, string mail_out_pass)
        {
            if ((_acct != null) && (_acct.UserOfAccount != null))
            {
                if (_acct.IsDemo) return -1;

                string domainName = EmailAddress.GetDomainFromEmail(email);
                Domain domain = Account.LoadDomainFromDb(domainName);
                int idDomain = 0;
                if (domain != null)
                {
                    idDomain = domain.ID;
                    mail_inc_host = domain.MailIncomingHost;
                    mail_inc_port = domain.MailIncomingPort;
                    mail_protocol = domain.MailIncomingProtocol;
                    mail_out_host = domain.MailOutgoingHost;
                    mail_out_port = domain.MailOutgoingPort;
                    mail_out_auth = domain.MailOutgoingAuthentication;
                    if (mail_protocol == IncomingMailProtocol.WMServer)
                    {
                        mail_inc_login = EmailAddress.GetAccountNameFromEmail(email);
                        mail_out_login = email;
                    }
                }

                Account acct = new Account();
                FolderSyncType inboxSyncType = (FolderSyncType)inbox_sync_type;

                acct = _acct.UserOfAccount.CreateAccount(def_acct, false, email, mail_protocol, mail_inc_host,
                    mail_inc_login, mail_inc_pass, mail_inc_port, mail_out_host, mail_out_login, mail_out_pass,
                    mail_out_port, mail_out_auth, friendly_nm, use_friendly_name, acct.DefaultOrder,
                    getmail_at_login, mail_mode, mails_on_server_days, string.Empty, acct.SignatureType,
                    acct.SignatureOptions, acct.Delimiter, 0, inboxSyncType, false, idDomain, false, -1, "");

                WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();

                if (acct != null && acct.MailIncomingProtocol == IncomingMailProtocol.Imap4 && settings.TakeImapQuota)
                {
                    ImapStorage imapStorage = new ImapStorage(acct);
                    try
                    {
                        imapStorage.Connect();
                        acct.Namespace = imapStorage.GetNamespace();

                        if (imapStorage.IsQuotaSupported())
                        {
                            acct.Imap_quota = 0;
                        }
                        acct.Update(false);
                    }
                    finally
                    {
                        imapStorage.Disconnect();
                    }
                }

                return (acct != null) ? acct.ID : -1;
            }

            if (_acct == null)
            {
                Log.WriteLine("NewAccount", "Account is null.");
            }
            else if (_acct.UserOfAccount == null)
            {
                Log.WriteLine("NewAccount", "User is null.");
            }
            throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
        }
        
        public void UpdateAccount(int id_acct, bool def_acct, IncomingMailProtocol mail_protocol, int mail_inc_port, int mail_out_port, bool mail_out_auth, bool use_friendly_name, short mails_on_server_days, MailMode mail_mode, bool getmail_at_login, int inbox_sync_type, string friendly_nm, string email, string mail_inc_host, string mail_inc_login, string mail_inc_pass, string mail_out_host, string mail_out_login, string mail_out_pass)
        {
        	bool allowChangeSettings = false;
        	int id_user = -1;
            if ((_acct != null) && (_acct.UserOfAccount != null))
            {
                if (_acct.IsDemo) return;

                id_user = _acct.UserOfAccount.ID;
                allowChangeSettings = (_acct.UserOfAccount.Settings != null) ? _acct.UserOfAccount.Settings.AllowChangeSettings : false;
            }
            else
            {
                if (_acct == null)
                {
                    Log.WriteLine("UpdateAccount", "Account is null.");
                }
                else if (_acct.UserOfAccount == null)
                {
                    Log.WriteLine("UpdateAccount", "User is null.");
                }
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
            Account acct = Account.LoadFromDb(id_acct, id_user, false);
            if (acct != null)
            {
                acct.FriendlyName = friendly_nm;
                acct.UseFriendlyName = use_friendly_name;
                if (allowChangeSettings)
                {
                    acct.ChangeAccountDefault(def_acct);
                    acct.MailIncomingProtocol = mail_protocol;
                    acct.MailIncomingPort = mail_inc_port;
                    acct.MailOutgoingPort = mail_out_port;
                    acct.MailOutgoingAuthentication = mail_out_auth;
                    acct.MailsOnServerDays = mails_on_server_days;
                    acct.GetMailAtLogin = getmail_at_login;
                    if (acct.MailIncomingProtocol == IncomingMailProtocol.Pop3)
                    {
                        acct.MailMode = mail_mode;
                        MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(acct));
                        try
                        {
                            mp.Connect();
                            Folder fld = mp.GetFolder(FolderType.Inbox);
                            if (fld != null)
                            {
                                fld.SyncType = (FolderSyncType)inbox_sync_type;
                                mp.UpdateFolders(new Folder[] { fld }, true);
                            }
                        }
                        finally
                        {
                            mp.Disconnect();
                        }
                    }
                    if (string.Compare(acct.Email, email, true, CultureInfo.InvariantCulture) != 0)
                    {
                        FileSystem fs = new FileSystem(acct.Email, acct.ID, true);
                        fs.RenameAccount(email);
                        acct.Email = email;
                    }
                    acct.MailIncomingHost = mail_inc_host;
                    acct.MailIncomingLogin = mail_inc_login;
                    if (string.Compare(mail_inc_pass, Constants.nonChangedPassword, true, CultureInfo.InvariantCulture) != 0)
                    {
                        string old_password = acct.MailIncomingPassword;
                        Log.WriteLine("UpdateAccount", string.Format("Change password. Old: '{0}'. New: '{1}'", old_password, mail_inc_pass));
                        acct.MailIncomingPassword = mail_inc_pass;
                        _acct.MailIncomingPassword = mail_inc_pass;

                        /* Code for AfterLogic XMail server */
                        if (acct.MailIncomingProtocol == IncomingMailProtocol.WMServer)
                        {
                            WMServerStorage wmServerStorage = new WMServerStorage(acct);
                            try
                            {
                                wmServerStorage.ChangeUserPassword(acct.DomainName, acct.MailIncomingLogin, _acct.MailIncomingPassword);
                            }
                            catch(Exception ex)
                            {
                                Log.WriteException(ex);
                                acct.MailIncomingPassword = old_password;
                                _acct.MailIncomingPassword = old_password;
                                throw new WebMailException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("PROC_ERROR_ACCT_UPDATE"));
                            }
                            Log.WriteLine("UpdateAccount", string.Format("Change server password. Old: '{0}'. New: '{1}'", old_password, mail_inc_pass));
                        }
                        else
                        {
                            try
                            {
                                HMailServer hmserver = new HMailServer();
                                if (hmserver.IsLoaded)
                                {
                                    if (!hmserver.UpdateUserPassword(acct.DomainName, acct.Email, _acct.MailIncomingPassword))
                                    {
                                        throw new WebMailException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("PROC_ERROR_ACCT_UPDATE"));
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.WriteException(ex);
                                acct.MailIncomingPassword = old_password;
                                _acct.MailIncomingPassword = old_password;
                                throw new WebMailException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("PROC_ERROR_ACCT_UPDATE"));
                            }
                            Log.WriteLine("UpdateAccount", string.Format("Change server password. Old: '{0}'. New: '{1}'", old_password, mail_inc_pass));
                        }
                    }
                    acct.MailOutgoingHost = (!string.IsNullOrEmpty(mail_out_host)) ? mail_out_host : mail_inc_host;
                    acct.MailOutgoingLogin = mail_out_login.Trim();
                    if (string.Compare(mail_out_pass, Constants.nonChangedPassword, true, CultureInfo.InvariantCulture) != 0)
                    {
                        acct.MailOutgoingPassword = mail_out_pass;
                        _acct.MailOutgoingPassword = mail_out_pass;
                    }
                }
                acct.Update(false);
                if (_acct.ID == id_acct)
                {
                    _acct = acct;
                }
            }
        }

        public string GetSignature(int id_acct)
        {
            if ((_acct != null) && (_acct.UserOfAccount != null))
            {
                Account acct = Account.LoadFromDb(id_acct, _acct.UserOfAccount.ID, false);
                if (acct != null)
                {
                    return Utils.EncodeHtml(acct.Signature);
                }
            }
            else
            {
                if (_acct == null)
                {
                    Log.WriteLine("GetSignature", "Account is null.");
                }
                else if (_acct.UserOfAccount == null)
                {
                    Log.WriteLine("GetSignature", "User is null.");
                }
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
            return string.Empty;
        }

        public void UpdateSettings(bool allow_dhtml_editor, int def_charset_inc, int def_charset_out,
            string def_date_fmt, string def_lang, string def_skin, short def_timezone, short msgs_per_page,
            short contacts_per_page, byte view_mode, TimeFormats time_fmt, int auto_checkmail_interval)
        {
            WebmailSettings webSettings = (new WebMailSettingsCreator()).CreateWebMailSettings();
            if ((_acct != null) && (_acct.UserOfAccount != null) && (_acct.UserOfAccount.Settings != null))
            {
                _acct.UserOfAccount.Settings.AllowDhtmlEditor = allow_dhtml_editor;
                if (webSettings.AllowUsersChangeCharset)
                {
                    _acct.UserOfAccount.Settings.DefaultCharsetInc = def_charset_inc;
                    _acct.UserOfAccount.Settings.DefaultCharsetOut = def_charset_out;
                }
                def_date_fmt = def_date_fmt.Trim();
                if (!string.IsNullOrEmpty(def_date_fmt)) _acct.UserOfAccount.Settings.DefaultDateFormat = def_date_fmt;
                _acct.UserOfAccount.Settings.DefaultTimeFormat = time_fmt;

                if (webSettings.AllowUsersChangeLanguage)
                {
                    _acct.UserOfAccount.Settings.DefaultLanguage = def_lang;
                }
                if (webSettings.AllowUsersChangeSkin)
                {
                    string newSkin = "";
                    string[] skins = Utils.GetSupportedSkins(_webPage.MapPath("skins"));
                    if (Utils.GetCurrentSkinIndex(skins, def_skin) > -1)
                    {
                        newSkin = def_skin;
                    }
                    else
                    {
                        if (Utils.GetCurrentSkinIndex(skins, webSettings.DefaultSkin) > -1)
                        {
                            newSkin = webSettings.DefaultSkin;
                        }
                        else if (skins.Length > 0)
                        {
                            newSkin = skins[0];
                        }
                    }
                    _acct.UserOfAccount.Settings.DefaultSkin = newSkin;
                }
                if (webSettings.AllowUsersChangeTimeZone)
                {
                    _acct.UserOfAccount.Settings.DefaultTimeZone = def_timezone;
                }
				_acct.UserOfAccount.Settings.MsgsPerPage = msgs_per_page;
				if (webSettings.AllowContacts && contacts_per_page != 0)
				{
					_acct.UserOfAccount.Settings.ContactsPerPage = contacts_per_page;
				}
				_acct.UserOfAccount.Settings.ViewMode = (ViewMode)view_mode;
                _acct.UserOfAccount.Settings.AutoCheckmailInterval = auto_checkmail_interval;

                _acct.Update(true);
            }
            else
            {
                if (_acct == null)
                {
                    Log.WriteLine("UpdateSettings", "Account is NULL!!!");
                }
                else if (_acct.UserOfAccount == null)
                {
                    Log.WriteLine("UpdateSettings", "User is NULL!!!");
                }
                else if (_acct.UserOfAccount.Settings == null)
                {
                    Log.WriteLine("UpdateSettings", "Settings is NULL!!!");
                }
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public void UpdateSignature(int id_acct, int type, int opt, string str)
        {
            if ((_acct != null) && (_acct.UserOfAccount != null))
            {
                if (_acct.IsDemo) return;

                Account acct = Account.LoadFromDb(id_acct, _acct.UserOfAccount.ID, false);
                if (acct != null)
                {
                    acct.SignatureType = (SignatureType)type;
                    acct.SignatureOptions = (SignatureOptions)opt;
                    acct.Signature = str;
                    acct.Update(false);

                    if (_acct.ID == id_acct)
                    {
                        _acct = acct;
                    }
                }
            }
            else
            {
                if (_acct == null)
                {
                    Log.WriteLine("UpdateSignature", "Account is null.");
                }
                else if (_acct.UserOfAccount == null)
                {
                    Log.WriteLine("UpdateSignature", "User is null.");
                }
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public void UpdateXSpam(bool xSpam)
        {
            if (_acct.IsDemo) return;

            if ((_acct.UserOfAccount != null) && (_acct.UserOfAccount.Settings != null))
            {
                _acct.UserOfAccount.Settings.XSpam = xSpam;
                _acct.Update(true);
            }
            else
            {
                if (_acct.UserOfAccount == null)
                {
                    Log.WriteLine("UpdateXSpam", "User is null.");
                }
                else if (_acct.UserOfAccount.Settings == null)
                {
                    Log.WriteLine("UpdateXSpam", "User is null.");
                }
                throw new WebMailException("User is null.");
            }
        }

        public bool GetXSpam()
        {
            if ((_acct.UserOfAccount != null) && (_acct.UserOfAccount.Settings != null))
            {
                return _acct.UserOfAccount.Settings.XSpam;
            }
            if (_acct.UserOfAccount == null)
            {
                Log.WriteLine("UpdateXSpam", "User is null.");
            }
            else if (_acct.UserOfAccount.Settings == null)
            {
                Log.WriteLine("UpdateXSpam", "User is null.");
            }
            throw new WebMailException("User is null.");
        }

        public void UpdateDefOrder(int def_order)
        {
            if (_acct.IsDemo) return;

            DbStorage storage = DbStorageCreator.CreateDatabaseStorage(_acct);
            try
            {
                storage.Connect();
                storage.UpdateAccountDefOrder(def_order);
            }
            finally
            {
                storage.Disconnect();
            }
        }
        #endregion

        #region Filter
        public void DeleteFilter(int id_filter, int id_acct)
        {
            if ((_acct != null) && (_acct.UserOfAccount != null))
            {
                if (_acct.IsDemo) return;

                Account acct = Account.LoadFromDb(id_acct, _acct.UserOfAccount.ID, false);
                if (acct != null)
                {
                    DbStorage dbStorage = DbStorageCreator.CreateDatabaseStorage(acct);
                    try
                    {
                        dbStorage.Connect();
                        dbStorage.DeleteFilter(id_filter);
                    }
                    finally
                    {
                        dbStorage.Disconnect();
                    }
                }
            }
            else
            {
                if (_acct == null)
                    Log.WriteLine("DeleteFilter", "Account is null.");
                else if (_acct.UserOfAccount == null)
                    Log.WriteLine("DeleteFilter", "User is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public Filter GetFilter(int id_filter, int id_acct)
        {
            Filter flt = null;
            if ((_acct != null) && (_acct.UserOfAccount != null))
            {
                Account acct = Account.LoadFromDb(id_acct, _acct.UserOfAccount.ID, false);
                if (acct != null)
                {
                    DbStorage dbStorage = DbStorageCreator.CreateDatabaseStorage(acct);
                    try
                    {
                        dbStorage.Connect();
                        flt = dbStorage.GetFilter(id_filter);
                    }
                    finally
                    {
                        dbStorage.Disconnect();
                    }
                }
            }
            else
            {
                if (_acct == null)
                {
                    Log.WriteLine("GetFilter", "Account is null.");
                }
                else if (_acct.UserOfAccount == null)
                {
                    Log.WriteLine("GetFilter", "User is null.");
                }
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
            return flt;
        }

        public Filter[] GetFilters(int id_acct)
        {
            Filter[] flts = null;
            Account acct = _acct;
            if ((_acct != null) && (_acct.UserOfAccount != null))
            {
                acct = Account.LoadFromDb(id_acct, _acct.UserOfAccount.ID, false);
            }
            else
            {
                if (_acct == null)
                {
                    Log.WriteLine("GetFilters", "Account is null.");
                }
                else if (_acct.UserOfAccount == null)
                {
                    Log.WriteLine("GetFilters", "User is null.");
                }
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
            if (acct != null)
            {
                DbStorage dbStorage = DbStorageCreator.CreateDatabaseStorage(acct);
                try
                {
                    dbStorage.Connect();
                    flts = dbStorage.GetFilters();
                }
                finally
                {
                    dbStorage.Disconnect();
                }
            }
            return flts;
        }

		public void NewFilter(int id_acct, byte field, byte condition, byte action, long id_folder, string filter, 
			bool applied)
        {
            if ((_acct != null) && (_acct.UserOfAccount != null))
            {
                if (_acct.IsDemo) return;

                if ((filter == null) || (filter.Trim().Length == 0))
                {
                    WebmailResourceManager resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
                    throw new WebMailException(resMan.GetString("WarningEmptyFilter"));
                }

                Account acct = Account.LoadFromDb(id_acct, _acct.UserOfAccount.ID, false);
                if (acct != null)
                {
                    DbStorage dbStorage = DbStorageCreator.CreateDatabaseStorage(acct);
                    try
                    {
                        dbStorage.Connect();
                        dbStorage.CreateFilter(field, condition, filter, action, id_folder, applied);
                    }
                    finally
                    {
                        dbStorage.Disconnect();
                    }
                }
            }
            else
            {
                if (_acct == null)
                {
                    Log.WriteLine("NewFilter", "Account is null.");
                }
                else if (_acct.UserOfAccount == null)
                {
                    Log.WriteLine("NewFilter", "User is null.");
                }
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

		public void UpdateFilter(int id_acct, int id_filter, byte field, byte condition, byte action, long id_folder,
			string filter, bool applied)
        {
            if ((_acct != null) && (_acct.UserOfAccount != null))
            {
                if (_acct.IsDemo) return;

                if ((filter == null) || (filter.Trim().Length == 0))
                {
                    WebmailResourceManager resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
                    throw new WebMailException(resMan.GetString("WarningEmptyFilter"));
                }

                Account acct = Account.LoadFromDb(id_acct, _acct.UserOfAccount.ID, false);
                if (acct != null)
                {
                    DbStorage dbStorage = DbStorageCreator.CreateDatabaseStorage(acct);
                    try
                    {
                        dbStorage.Connect();
						dbStorage.UpdateFilter(id_filter, field, condition, filter, action, id_folder, applied);
                    }
                    finally
                    {
                        dbStorage.Disconnect();
                    }
                }
            }
            else
            {
                if (_acct == null)
                {
                    Log.WriteLine("UpdateFilter", "Account is null.");
                }
                else if (_acct.UserOfAccount == null)
                {
                    Log.WriteLine("UpdateFilter", "User is null.");
                }
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }        
        #endregion

        #region Folders
        public void DeleteFolders(int id_acct, Folder[] folders)
        {
            if ((_acct != null) && (_acct.UserOfAccount != null))
            {
                if (_acct.IsDemo) return;

                Account acct = Account.LoadFromDb(id_acct, _acct.UserOfAccount.ID, false);
                if (acct != null)
                {
                    MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(acct));
                    try
                    {
                        mp.Connect();
                        mp.DeleteFolders(folders);
                    }
                    finally
                    {
                        mp.Disconnect();
                    }
                }
            }
            else
            {
                if (_acct == null)
                    Log.WriteLine("DeleteFolders", "Account is null.");
                else if (_acct.UserOfAccount == null)
                    Log.WriteLine("DeleteFolders", "User is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public FolderCollection GetFoldersList(int id_acct, int sync)
        {
            Account acct = null;
            FolderCollection fc = new FolderCollection();
            if ((_acct != null) && (_acct.UserOfAccount != null))
            {
                acct = Account.LoadFromDb(id_acct, _acct.UserOfAccount.ID, false);
            }
            else
            {
                if (_acct == null)
                {
                    Log.WriteLine("GetFoldersList", "Account is null.");
                }
                else if (_acct.UserOfAccount == null)
                {
                    Log.WriteLine("GetFoldersList", "User is null.");
                }
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
            if (acct != null)
            {
                if (sync != -1)
                {
                    if (_acct.UserOfAccount != null) _acct = acct;
                }

                MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(acct));
                try
                {
                    mp.Connect();
                    fc = mp.GetFolders();
                    switch (sync)
                    {
                        case 2:
                            mp.SynchronizeFolders();
                            fc = mp.GetFolders();
                            break;
                    }
                    FolderCollection.SortTree(fc);
                }
                finally
                {
                    mp.Disconnect();
                }
            }
            return fc;
        }

        public Folder GetFolder(string name)
        {
            MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(_acct));
            try
            {
                mp.Connect();
                return mp.GetFolder(name);
            }
            finally
            {
                mp.Disconnect();
            }
        }

        public Folder GetFolder(long id_folder)
        {
            MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(_acct));
            try
            {
                mp.Connect();
                return mp.GetFolder(id_folder);
            }
            finally
            {
                mp.Disconnect();
            }
        }

        public Folder GetFolder(int id_acct, long id_folder)
        {
            Account acct = Account.LoadFromDb(id_acct, _acct.IDUser, false);
            MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(acct));
            try
            {
                mp.Connect();
                return mp.GetFolder(id_folder);
            }
            finally
            {
                mp.Disconnect();
            }
        }

        public Folder GetFolder(FolderType type)
        {
            MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(_acct));
            try
            {
                mp.Connect();
                return mp.GetFolder(type);
            }
            finally
            {
                mp.Disconnect();
            }
        }

        public void NewFolder(int id_acct, int id_parent, string full_name_parent, string name, int create)
        {
            if ((_acct != null) && (_acct.UserOfAccount != null))
            {
                if (_acct.IsDemo) return;

                Account acct = Account.LoadFromDb(id_acct, _acct.UserOfAccount.ID, false);
                if (acct != null)
                {
                    MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(acct));
                    try
                    {
                        mp.Connect();
                        mp.CreateFolder(id_parent, full_name_parent, name, create);
                    }
                    finally
                    {
                        mp.Disconnect();
                    }
                }
            }
            else
            {
                if (_acct == null)
                {
                    Log.WriteLine("NewFolder", "Account is null.");
                }
                else if (_acct.UserOfAccount == null)
                {
                    Log.WriteLine("NewFolder", "User is null.");
                }
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public void UpdateFolders(int id_acct, /*XmlPacketFolder*/Folder[] folders)
        {
            if ((_acct != null) && (_acct.UserOfAccount != null))
            {
                if (_acct.IsDemo) return;

                Account acct = Account.LoadFromDb(id_acct, _acct.UserOfAccount.ID, false);
                if (acct != null)
                {
                    MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(acct));
                    try
                    {
                        mp.Connect();
                        mp.UpdateFolders(folders, true);
                    }
                    finally
                    {
                        mp.Disconnect();
                    }
                }
            }
            else
            {
                if (_acct == null)
                {
                    Log.WriteLine("NewFolder", "Account is null.");
                }
                else if (_acct.UserOfAccount == null)
                {
                    Log.WriteLine("NewFolder", "User is null.");
                }
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public Folder GetTrashFolder()
        {
            if (CurrentAccount != null)
            {
                MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(CurrentAccount));
                Folder trash;
                try
                {
                    mp.Connect();
                    trash = mp.GetFolder(FolderType.Trash);
                }
                finally
                {
                    mp.Disconnect();
                }
                return trash;
            }
            Log.WriteLine("GetTrashFolder", "Account is null.");
            throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
        }

        public Folder GetSpamFolder()
        {
            if (CurrentAccount != null)
            {
                MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(CurrentAccount));
                Folder spam;
                try
                {
                    mp.Connect();
                    spam = mp.GetFolder(FolderType.Spam);
                }
                finally
                {
                    mp.Disconnect();
                }
                return spam;
            }
            Log.WriteLine("GetSpamFolder", "Account is null.");
            throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
        }
        #endregion

        #region Message
        public WebMailMessage GetMessage(int id, string uid, long id_folder, string full_name_folder, int charset, bool body_structure, MessageMode mode)
        {
        	const byte safety = 0;
            return GetMessage(id, uid, id_folder, charset, safety, true, false, body_structure, mode);
        }

    	static bool AHrefProcessDelegate(Element elem, Rule rule)
        {
            if (elem.TagName.ToLower() == "a")
            {
                TagAttribute attr = elem.GetAttributeByName("href");
				string hrefValue = (attr != null && attr.Value != null && attr.Value.Length > 1) ? attr.Value[1].ToString() : "";
                if (hrefValue == "#")
                {
                    return false;
                }
            }
            return true;
        }

        public WebMailMessageCollection GetMessages(int[] id_msgs, object[] uid_msgs, long id_folder, byte safety, bool body_structure, MessageMode mode, XMLPacketMessagesBody[] messages)
        {
            if (_acct == null)
            {
                Log.WriteLine("GetMessage", "Account is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
            MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(_acct));
            WebMailMessageCollection webMsgs = new WebMailMessageCollection();
            try
            {
                mp.Connect();
                Folder fld = mp.GetFolder(id_folder);
                if (fld == null) return null;
                WebMailMessageCollection msgs = null;

                if (fld.SyncType == FolderSyncType.DirectMode)
                {
                    msgs = mp.GetMessages(uid_msgs, true, fld, body_structure, mode, messages);
                }
                else
                {
                    msgs = mp.GetMessages(_acct.ID, id_msgs, fld, body_structure, mode);
                }

                if (msgs == null) return null;
                foreach (WebMailMessage msg in msgs)
                {
                    if (msg == null) continue;
                    if (msg.MailBeeMessage == null) continue;
                    MailMessage outputMsg = msg.MailBeeMessage.Clone();
                    bool _body_structure = (body_structure && msg.Size > Constants.IMAP_OPT_MAIL_SIZE) ? true : false;
                    WebMailMessage webMsg = PrepareMessage(msg, outputMsg, fld, safety, true, false, _body_structure);
                    webMsgs.Add(webMsg);
                }
            }
            finally
            {
                mp.Disconnect();
            }
            return webMsgs;
        }

        public WebMailMessage GetMessage(int id, string uid, long id_folder, int charset, byte safety, bool needToTrim, bool needToShowTrimMessage, bool body_structure, MessageMode mode)
        {
            WebMailMessage msg = null;
            MailMessage outputMsg = null;
            Folder fld;
            if (_acct != null)
            {
                MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(_acct));
                try
                {
                    mp.Connect();
                    fld = mp.GetFolder(id_folder);
                    if (fld != null)
                    {
						msg = mp.GetMessage((fld.SyncType != FolderSyncType.DirectMode) ? (object)id : uid, fld, body_structure, mode);
						if (msg != null)
                        {
							if (msg.OverrideCharset != charset)
							{
								msg.OverrideCharset = charset;
								mp.UpdateMessage(msg);
							}
							if (charset > 0)
                            {
                                if ((CurrentAccount.UserOfAccount != null) && (CurrentAccount.UserOfAccount.Settings != null))
                                {
                                    CurrentAccount.UserOfAccount.Settings.DefaultCharsetInc = charset;
                                    CurrentAccount.Update(true);
                                }
                                if (msg.MailBeeMessage != null)
                                {
                                    outputMsg = msg.MailBeeMessage.Clone();

                                    if (outputMsg != null)
                                    {
                                        outputMsg.Parser.EncodingOverride = (charset > 0) ? Utils.GetEncodingByCodePage(charset) : null;
                                        outputMsg.Parser.Apply();
                                    }
                                    msg.Init(outputMsg, !string.IsNullOrEmpty(msg.StrUid), fld);
                                    mp.UpdateMessage(msg);
                                }
                            }
                        }
                    }
                }
                finally
                {
                    mp.Disconnect();
                }
            }
            else
            {
                Log.WriteLine("GetMessage", "Account is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
            if (msg == null)
			{
				throw new WebMailException("Message doesn't exist.");
			}
            if (outputMsg != null)
            {
                return PrepareMessage(msg, outputMsg, fld, safety, needToTrim, needToShowTrimMessage, body_structure);
            }
            else
            {
                return PrepareMessage(msg, msg.MailBeeMessage, fld, safety, needToTrim, needToShowTrimMessage, body_structure);
            }
        }

		private WebMailMessage PrepareMessage(WebMailMessage msg, MailMessage outputMsg, Folder fld, byte safety, bool needToTrim, bool needToShowTrimMessage, bool body_structure)
		{
			if (_acct.UserOfAccount.GetSenderSafety(outputMsg.From.Email) == 1) safety = 1;
			if (outputMsg.Attachments.Count > 0)
			{
				if (body_structure)
				{
					foreach (Attachment _attach in outputMsg.Attachments)
					{
						string virtualPath = Utils.GetAttachmentDownloadLink(outputMsg, _attach, fld.Name, false, CurrentPage.Session);
						outputMsg.BodyHtmlText = outputMsg.BodyHtmlText.Replace("cid:" + _attach.ContentID, virtualPath);
					}
				}
				else
				{
					if (_webPage != null)
					{
						string tempFolder = Utils.GetTempFolderName(_webPage.Session);
						outputMsg.Parser.WorkingFolder = tempFolder;
						outputMsg.BodyHtmlText = outputMsg.GetHtmlAndSaveRelatedFiles(@"get-attachment-binary.aspx?filename=", VirtualMappingType.Static, MessageFolderBehavior.DoNotCreate);

						AttachmentCollection SavedAttachments = outputMsg.Attachments/*Utils.CreateDeepCopy(outputMsg.Attachments)*/;
						SaveAttachmentsToTempDirectory(SavedAttachments, tempFolder, outputMsg.Attachments);
					}
				}
			}

			// removing all potentially unsafe content from the HTML e-mail body
			if (!string.IsNullOrEmpty(outputMsg.BodyHtmlText))
			{
				string htmlBody = outputMsg.BodyHtmlText;
				if (needToTrim && (htmlBody.Length > Constants.BodyMaxLength))
				{
					htmlBody = htmlBody.Substring(0, Constants.BodyMaxLength);
					if (needToShowTrimMessage)
					{
						string strUid = (string.IsNullOrEmpty(msg.StrUid)) ? msg.IntUid.ToString() : msg.StrUid;
						htmlBody += Utils.GetTrimMessage(msg.IDMsg, strUid, msg.IDFolderDB, fld.FullPath, msg.OverrideCharset, 1); ;
					}
				}

				// Ex-MailBee.Html
				string[] removeTags = new string[]
					{
						"<!doctype[^>]*>",
						"</?html[^>]*>",
						"</?body[^>]*>",
						"<link[^>]*>",
						"<base[^>]*>",
						"<head[^>]*>.*?</head>",
						"<title[^>]*>.*?</title>",
						"<style[^>]*>.*?</style>",
						"<script[^>]*>.*?</script>",
						"<object[^>]*>.*?</object>",
						"<embed[^>]*>.*?</embed>",
						"<applet[^>]*>.*?</applet>",
						"<mocha[^>]*>.*?</mocha>",
						"<meta[^>]*>"
					};

				foreach (string regEx in removeTags)
				{
					htmlBody = Regex.Replace(htmlBody, regEx, string.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);
				}

				string[] removeAttributes = new string[] { "onActivate", "onAfterPrint",
									"onBeforePrint", "onAfterUpdate", "onBeforeUpdate", "onErrorUpdate",
									"onAbort", "onBeforeDeactivate", "onDeactivate", "onBeforeCopy",
									"onBeforeCut", "onBeforeEditFocus", "onBeforePaste", "onBeforeUnload",
									"onBlur", "onBounce", "onChange", "onClick", "onControlSelect",
									"onCopy", "onCut", "onDblClick", "onDrag", "onDragEnter", "onDragLeave",
									"onDragOver", "onDragStart", "onDrop", "onFilterChange", "onDragDrop",
									"onError", "onFilterChange", "onFinish", "onFocus", "onHelp", "onKeyDown",
									"onKeyPress", "onKeyUp", "onLoad", "onLoseCapture", "onMouseDown",
									"onMouseEnter", "onMouseLeave", "onMouseMove", "onMouseOut",
									"onMouseOver", "onMouseUp", "onMove", "onPaste",
									"onPropertyChange", "onReadyStateChange", "onReset", "onResize",
									"onResizeEnd", "onResizeStart", "onScroll", "onSelectStart",
									"onSelect", "onSelectionChange", "onStart", "onStop", "onSubmit",
									"onUnload"};

				foreach (string regEx in removeAttributes)
				{
					htmlBody = Regex.Replace(htmlBody, "(<[^>]*)(" + regEx + ")([^>]*>)", "${1}X_${2}${3}", RegexOptions.Singleline | RegexOptions.IgnoreCase);
				}

				if (safety == 0)
				{
					safety = 1; // sender not safety, but HTML may be safety

					//BGImagesReplacement
					htmlBody = Regex.Replace(htmlBody, "(<[^>]+style[ \t\r\n]*=[ \t\r\n]*['\"]?)url\\(", "${1}wmx_url(");
                    htmlBody = Regex.Replace(htmlBody, "(<[^>]+)background", "${1}wmx_background");

					//img
                    if (Regex.IsMatch(htmlBody, "(<img[^>]+)src"))
                    {
                        htmlBody = Regex.Replace(htmlBody, "(<img[^>]+)src", "${1}wmx_src");
                        htmlBody = Regex.Replace(htmlBody, "(<img[^>]+)wmx_src=\"get-attachment-binary", "${1}src=\"get-attachment-binary");
                        if (htmlBody.IndexOf("wmx_src") != -1)
                        {
                            safety = 0;
                        }
                    }

				}
				msg.Safety = safety;

				MatchEvaluator aHrefEvaluator = new MatchEvaluator(AHrefReplaceDelegate);
				htmlBody = Regex.Replace(htmlBody, "<a[^>]+", aHrefEvaluator, RegexOptions.Singleline | RegexOptions.IgnoreCase);

				// processing of the rule
				htmlBody = Regex.Replace(htmlBody, @"(document|window).location[^=]*=\s*[""'][^""']*[""']",
												string.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);

				outputMsg.BodyHtmlText = htmlBody;
			}
			else
			{
				msg.Safety = 1;
			}

			if (string.IsNullOrEmpty(outputMsg.BodyPlainText))
			{
				outputMsg.MakePlainBodyFromHtmlBody();
			}

			// for mailto:
			outputMsg.BodyHtmlText = outputMsg.BodyHtmlText.Replace("<a ", "<a onclick=\"return checkLinkHref(this.href);\" ");
			outputMsg.BodyPlainText = outputMsg.BodyPlainText.Replace("<a ", "<a onclick=\"return checkLinkHref(this.href);\" ");

			msg.MailBeeMessage = outputMsg;
			return msg;
		}

		private string AHrefReplaceDelegate(Match m)
		{
			string result = m.ToString();

			Regex re = new Regex("<a([^>]+href[ \t\r\n]*=[ \t\r\n]*['\"]?[#]{1})");

			Match mr = re.Match(result);

			if (!mr.Success)
			{
				result = "<a target=\"_blank\"" + result.Substring(2);
			}

			return result;
		}

		private string ImgReplaceDelegate(Match m)
		{
			string result = m.ToString();

			if (result.IndexOf("\"get-attachment-binary.aspx") == -1)
			{
				result = Regex.Replace(result, "(<img[^>]+)src", "${1}wmx_src");
			}

			return result;
		}

        private void SaveAttachmentsToTempDirectory(AttachmentCollection Attachments, string tempFolder, AttachmentCollection refAttachments)
        {
        	for (int i=0; i < Attachments.Count; i++)
            {
                string filename = Utils.CreateTempFilePath(tempFolder,
					(Attachments[i].Filename.Length > 0) ? Attachments[i].Filename : Attachments[i].Name);
                Attachments[i].Save(filename, true);
                if (Attachments[i].IsTnef)
                {
                    AttachmentCollection atCol = Attachments[i].GetAttachmentsFromTnef();
                    foreach (Attachment attach in atCol)
                    {
                        refAttachments.Add(attach);
                    }
                    SaveAttachmentsToTempDirectory(atCol, tempFolder, refAttachments);
                }
            }
        }

        public WebMailMessageCollection GetMessages(long id_folder, string full_name_folder, int page, int sort_field, int sort_order, string look_for, int search_fields, out int folderMessageCount, out int folderUnreadMessageCount)
        {
            if (_acct != null)
            {
                _acct.DefaultOrder = (DefaultOrder)(sort_field + sort_order);
                _acct.Update(false);

                WebMailMessageCollection msgsColl = new WebMailMessageCollection();
                MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(_acct));
                folderMessageCount = 0;
                folderUnreadMessageCount = 0;
                try
                {
                    mp.Connect();

                    FolderCollection fldColl = new FolderCollection();
                    Folder fld = null;
                    if (full_name_folder != null)
                    {
                        if ((id_folder == -1) && (full_name_folder.Length == 0))
                        {
                            fldColl = mp.GetFolders(true);
                            FolderCollection viewedFolders = new FolderCollection();
                            foreach (Folder f in fldColl)
                            {
                                if (!f.Hide) viewedFolders.Add(f);
                            }
                            fldColl = viewedFolders;
                        }
                        else
                        {
                            fld = mp.GetFolder(id_folder);
                            if (fld != null)
                            {
                                fldColl.Add(fld);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(look_for))
                    {
                        // search messages
                        msgsColl = mp.SearchMessages(page, look_for, fldColl, (search_fields == 0) ? true : false, out folderMessageCount);
                    }
                    else
                    {
                        // load messages
                        if (fld != null)
                        {
                            folderMessageCount = mp.GetFolderMessageCount(fld);
                            folderUnreadMessageCount = mp.GetFolderUnreadMessageCount(fld);
                            msgsColl = mp.GetMessageHeaders(page, fld);
                        }
                    }
                }
                finally
                {
                    mp.Disconnect();
                }
                return msgsColl;
            }
        	Log.WriteLine("GetMessages", "Account is null.");
        	throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
        }

        public WebMailMessageCollection GetMessages(long id_folder, string full_name_folder, int page, int sort_field, int sort_order, string look_for, int search_fields, out int folderMessageCount, out int folderUnreadMessageCount,  bool body_structure, MessageMode mode)
        {
            if (_acct != null)
            {
                _acct.DefaultOrder = (DefaultOrder)(sort_field + sort_order);
                _acct.Update(false);

                WebMailMessageCollection msgsColl = new WebMailMessageCollection();
                MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(_acct));
                folderMessageCount = 0;
                folderUnreadMessageCount = 0;
                try
                {
                    mp.Connect();

                    FolderCollection fldColl = new FolderCollection();
                    Folder fld = null;
                    if (full_name_folder != null)
                    {
                        if ((id_folder == -1) && (full_name_folder.Length == 0))
                        {
                            fldColl = mp.GetFolders(true);
                            FolderCollection viewedFolders = new FolderCollection();
                            foreach (Folder f in fldColl)
                            {
                                if (!f.Hide) viewedFolders.Add(f);
                            }
                            fldColl = viewedFolders;
                        }
                        else
                        {
                            fld = mp.GetFolder(id_folder);
                            if (fld != null)
                            {
                                fldColl.Add(fld);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(look_for))
                    {
                        // search messages
                        msgsColl = mp.SearchMessages(page, look_for, fldColl, (search_fields == 0) ? true : false, out folderMessageCount);
                    }
                    else
                    {
                        // load messages
                        if (fld != null)
                        {
                            folderMessageCount = mp.GetFolderMessageCount(fld);
                            folderUnreadMessageCount = mp.GetFolderUnreadMessageCount(fld);
                            msgsColl = mp.GetMessageHeaders(page, fld);
                        }
                    }
                }
                finally
                {
                    mp.Disconnect();
                }
                return msgsColl;
            }
            Log.WriteLine("GetMessages", "Account is null.");
            throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
        }

        public int SaveMessageToDrafts(WebMailMessage message)
        {
            int msgID = -1;
            if (_acct != null)
            {
                MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(_acct));
                try
                {
                    mp.Connect();

                    Folder draftFolder = mp.GetFolder(FolderType.Drafts);
                    if (draftFolder != null)
                    {
                        WebMailMessage msg = message;
                        msg.Seen = true;

                        if (message.IDMsg > 0)
                        {
							mp.PurgeMessages(new object[] { message.IDMsg }, draftFolder);
						}
                        msgID = mp.SaveMessageAndGetId(msg, draftFolder);
                    }
                }
                finally
                {
                    mp.Disconnect();
                }
            }
            else
            {
                Log.WriteLine("SaveMessage", "Account is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
            return msgID;
        }

        public void SendMessage(WebMailMessage msg, bool SaveMail)
        {
            if (msg.Account != _acct)
            {
                _acct = msg.Account;
            }
            if (_acct != null)
            {
				if (!string.IsNullOrEmpty(msg.MailBeeMessage.BodyHtmlText))
				{
					msg.MailBeeMessage.BodyHtmlText = "<html><body>" + msg.MailBeeMessage.BodyHtmlText + "</body></html>";
				}
            	MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(_acct));
                try
                {
                    mp.Connect();
                    mp.SendMail(msg);

                    if (SaveMail)
                    {
                        Folder sentFld = mp.GetFolder(FolderType.SentItems);
                        if (sentFld != null)
                        {
                            msg.Seen = true;
                            try
                            {
                                mp.SaveMessage(msg, sentFld);
                            }
                            catch (WebMailMailBoxException ex) { Log.WriteException(ex); }
                        }
                    }

                    Folder draftsFld = mp.GetFolder(FolderType.Drafts);
                    if (msg.IDMsg > 0 && draftsFld != null)
                    {
						mp.PurgeMessages(new object[] { msg.IDMsg }, draftsFld);
                    }
                }
                finally
                {
                    mp.Disconnect();
                }
            }
            else
            {
                Log.WriteLine("NewMessage", "Account is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public void SetForwardedFlag(object[] messageIndexSet, Folder fld)
        {
            if (CurrentAccount != null)
            {
                MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(CurrentAccount));
                try
                {
                    mp.Connect();
                    mp.SetForwardedFlag(messageIndexSet, fld);
                }
                finally
                {
                    mp.Disconnect();
                }
            }
            else
            {
                Log.WriteLine("GroupOperations", "Account is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }

        }

        public void SetFlags(object[] messageIndexSet, SystemMessageFlags flags, MessageFlagAction flagsAction, Folder fld)
        {
            if (CurrentAccount != null)
            {
                MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(CurrentAccount));
                try
                {
                    mp.Connect();
                    mp.SetFlags(messageIndexSet, flags, flagsAction, fld);
                }
                finally
                {
                    mp.Disconnect();
                }
            }
            else
            {
                Log.WriteLine("GroupOperations", "Account is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }

        }

        public bool GroupMessagesProcess(string action, MailProcessor mp, object[] messageIndexSet, Folder fld, long to_folder_id, string to_folder_full_name)
        {
            bool error = false;
            if (mp != null)
            {
                switch (action)
                {
                    case Constants.GroupOperationsRequests.Delete:
                        mp.DeleteMessages(messageIndexSet, fld);
                        error = _isMoveError = mp.IsMoveError;
                        break;
                    case Constants.GroupOperationsRequests.NoMoveDelete:
                        mp.NoMoveDeleteMessages(messageIndexSet, fld);
                        break;
                    case Constants.GroupOperationsRequests.Flag:
                        mp.SetFlags(messageIndexSet, SystemMessageFlags.Flagged, MessageFlagAction.Add, fld);
                        break;
                    case Constants.GroupOperationsRequests.MarkAllRead:
                        mp.SetFlags(SystemMessageFlags.Seen, MessageFlagAction.Add, fld);
                        break;
                    case Constants.GroupOperationsRequests.MarkAllUnread:
                        mp.SetFlags(SystemMessageFlags.Seen, MessageFlagAction.Remove, fld);
                        break;
                    case Constants.GroupOperationsRequests.MarkRead:
                        mp.SetFlags(messageIndexSet, SystemMessageFlags.Seen, MessageFlagAction.Add, fld);
                        break;
                    case Constants.GroupOperationsRequests.MarkUnread:
                        mp.SetFlags(messageIndexSet, SystemMessageFlags.Seen, MessageFlagAction.Remove, fld);
                        break;
                    case Constants.GroupOperationsRequests.MoveToFolder:
                        Folder toFolder = mp.GetFolder(to_folder_id);
                        if (toFolder != null)
                        {
                            mp.MoveMessages(messageIndexSet, fld, toFolder);
                        }
                        break;
                    case Constants.GroupOperationsRequests.Purge:
                        mp.PurgeMessages(messageIndexSet, fld);
                        break;
                    case Constants.GroupOperationsRequests.ClearSpam:
                        Folder spamFolder = GetSpamFolder();
                        mp.PurgeMessages(messageIndexSet, spamFolder);
                        break;
                    case Constants.GroupOperationsRequests.Undelete:
                        mp.SetFlags(messageIndexSet, SystemMessageFlags.Deleted, MessageFlagAction.Remove, fld);
                        break;
                    case Constants.GroupOperationsRequests.Unflag:
                        mp.SetFlags(messageIndexSet, SystemMessageFlags.Flagged, MessageFlagAction.Remove, fld);
                        break;
                    case Constants.GroupOperationsRequests.Spam:
                        mp.SpamMessage(messageIndexSet, fld);
                        break;
                    case Constants.GroupOperationsRequests.NotSpam:
                        mp.NotSpamMessage(messageIndexSet, fld);
                        break;
                }
            }
            return error;
        }        
        #endregion

        #region AutoResponder
        public void UpdateAutoResponder(int id_acct, bool Enable, string Subject, string Message)
        {
            if ((_acct != null) && (_acct.UserOfAccount != null))
            {
                if (_acct.IsDemo) return;

                Account acct = Account.LoadFromDb(id_acct, _acct.UserOfAccount.ID, false);
                if (acct != null)
                {
                    if (acct.MailIncomingProtocol == IncomingMailProtocol.WMServer)
                    {
                        WMServerStorage wmServerStorage = new WMServerStorage(acct);
                        try
                        {
                            wmServerStorage.SetAutoResponder(Enable, Subject, Message);
                        }
                        catch (Exception ex)
                        {
                            Log.WriteLine("SetAutoResponder", "Set AutoResponder failed");
                            throw new WebMailException(ex);
                        }
                    }
                    else
                    {
                        try
                        {
                            HMailServer hmserver = new HMailServer();
                            if (hmserver.IsLoaded)
                            {
                                hmserver.UpdateAutoResponder(acct.DomainName, acct.Email, Enable, Subject, Message);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.WriteLine("SetAutoResponder", "Set AutoResponder failed");
                            throw new WebMailException(ex);
                        }
                    }
                }
            }
            else
            {
                if (_acct == null)
                {
                    Log.WriteLine("UpdateAutoResponder", "Account is null.");
                }
                else if (_acct.UserOfAccount == null)
                {
                    Log.WriteLine("UpdateAutoResponder", "User is null.");
                }
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public Autoresponder GetAutoResponder(int id_acct)
        {
            if ((_acct != null) && (_acct.UserOfAccount != null))
            {
                if (_acct.IsDemo) return null;

                Account acct = Account.LoadFromDb(id_acct, _acct.UserOfAccount.ID, false);
                if (acct != null)
                {
                    if (acct.MailIncomingProtocol == IncomingMailProtocol.WMServer)
                    {
                        WMServerStorage wmServerStorage = new WMServerStorage(acct);
                        return wmServerStorage.GetAutoResponder();
                    }
                    else
                    {
                        HMailServer hmserver = new HMailServer();
                        if (hmserver.IsLoaded)
                        {
                            return hmserver.GetAutoResponder(acct.DomainName, acct.Email);
                        }
                        return new Autoresponder();
                    }
                }
            }
            else
            {
                if (_acct == null)
                {
                    Log.WriteLine("GetAutoResponder", "Account is null.");
                }
                else if (_acct.UserOfAccount == null)
                {
                    Log.WriteLine("GetAutoResponder", "User is null.");
                }
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
            return null;
        }
        #endregion

        public void UpdateCookieSettings(bool hide_folders, short horiz_resizer, short vert_resizer, byte mark, byte reply, UserColumn[] columns)
        {
            if ((_acct != null) && (_acct.UserOfAccount != null) && (_acct.UserOfAccount.Settings != null))
            {
                if (_acct.IsDemo) return;

                _acct.UserOfAccount.Settings.HideFolders = hide_folders;
                _acct.UserOfAccount.Settings.HorizResizer = horiz_resizer;
                _acct.UserOfAccount.Settings.VertResizer = vert_resizer;
                _acct.UserOfAccount.Settings.Mark = mark;
                _acct.UserOfAccount.Settings.Reply = reply;
                _acct.UserOfAccount.Columns = columns;
                _acct.Update(true);
            }
            else
            {
                if (_acct == null)
                {
                    Log.WriteLine("UpdateCookieSettings", "Account is null.");
                }
                else if (_acct.UserOfAccount == null)
                {
                    Log.WriteLine("UpdateCookieSettings", "User is null.");
                }
                else if (_acct.UserOfAccount.Settings == null)
                {
                    Log.WriteLine("UpdateCookieSettings", "User Settings is null.");
                }
				throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
        }

        public bool GroupOperations(string request, long id_folder, string folder_full_name, string search_query, int fields, long to_folder_id, string to_folder_full_name, WebMailMessageCollection messages)
        {
            bool result = true;
            if (CurrentAccount != null)
            {
                MailProcessor mp = new MailProcessor(DbStorageCreator.CreateDatabaseStorage(CurrentAccount));
                try
                {
                    mp.Connect();

                    if (messages != null)
                    {
                        if (!string.IsNullOrEmpty(search_query))
                        {
                            FolderCollection fc = new FolderCollection();
                            if ((id_folder != -1) && (!string.IsNullOrEmpty(folder_full_name)))
                            {
                                fc.Add(mp.GetFolder(id_folder));
                            }
                            else
                            {
                                fc = mp.GetFolders();
                                FolderCollection viewedFolders = new FolderCollection();
                                foreach (Folder f in fc)
                                {
                                    if (!f.Hide) viewedFolders.Add(f);
                                }
                                fc = viewedFolders;
                            }
                            WebMailMessageCollection coll = mp.SearchMessages(search_query, fc, (fields == 0) ? true : false);
                            object[] messageIndexSet = coll.ToIDsCollection();
                            // new request string 
                            switch (request)
                            {
                                case "mark_all_read":
                                    request = "mark_read";
                                    break;
                                case "mark_all_unread":
                                    request = "mark_unread";
                                    break;
                            }
                            foreach (Folder fld in fc)
                            {
                                result = GroupMessagesProcess(request, mp, messageIndexSet, fld, to_folder_id, to_folder_full_name);
                                if (result == false)
                                {
                                    return false;
                                }
                            }
                        }
                        else if ((id_folder != -1) && (!string.IsNullOrEmpty(folder_full_name)))
                        {
                            // process messages in single folder
                            Folder fld = mp.GetFolder(id_folder);

                            if (fld != null)
                            {
                                object[] messageIndexSet = null;
                                if (messages.Count > 0)
                                {
                                    messageIndexSet = new object[messages.Count];
                                    for (int i = 0; i < messages.Count; i++)
                                    {
                                        if (fld.SyncType != FolderSyncType.DirectMode)
                                        {
                                            messageIndexSet[i] = messages[i].IDMsg;
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(messages[i].StrUid))
                                            {
                                                messageIndexSet[i] = messages[i].StrUid;
                                            }
                                            else
                                            {
                                                messageIndexSet[i] = messages[i].IntUid;
                                            }
                                        }
                                    }
                                }
                                result = GroupMessagesProcess(request, mp, messageIndexSet, fld, to_folder_id, to_folder_full_name);
                            }
                        }
                        else
                        {
                            // process messages in many folders
                            for (int i = 0; i < messages.Count; i++)
                            {
                                Folder fld = mp.GetFolder(messages[i].IDFolderDB);
                                if (fld != null)
                                {
                                    result = GroupMessagesProcess(request, mp, new object[] { messages[i].IDMsg }, fld, to_folder_id, to_folder_full_name);
                                    if (result == false)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
                finally
                {
                    mp.Disconnect();
                }
            }
            else
            {
				Log.WriteLine("GroupOperations", "Account is null.");
                throw new WebMailSessionException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("SessionIsEmpty"));
            }
            return result;
        }

    }
}
