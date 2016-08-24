using System;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.Collections;
using System.Text;
using MailBee.Mime;
using MailBee.ImapMail;

namespace WebMail
{
    public class PostgreSqlDbManager : DbManager
    {
        public PostgreSqlDbManager() : this(null) { }

        public PostgreSqlDbManager(Account acct)
            : base(acct)
        {
            _connection = new OdbcConnection();
            _commandCreator = new PostgreSqlCommandCreator(_connection as OdbcConnection, new OdbcCommand());
        }

        public override Account CreateAccount(int id_user, bool def_acct, bool deleted, string email, IncomingMailProtocol mail_protocol, string mail_inc_host, string mail_inc_login, string mail_inc_pass, int mail_inc_port, string mail_out_host, string mail_out_login, string mail_out_pass, int mail_out_port, bool mail_out_auth, string friendly_nm, bool use_friendly_nm, DefaultOrder def_order, bool getmail_at_login, MailMode mail_mode, short mails_on_server_days, string signature, SignatureType signature_type, SignatureOptions signature_opt, string delimiter, long mailbox_size, string Namespace)
        {
            return CreateAccount(id_user, def_acct, deleted, email, mail_protocol, mail_inc_host, mail_inc_login, mail_inc_pass, mail_inc_port, mail_out_host, mail_out_login, mail_out_pass, mail_out_port, mail_out_auth, friendly_nm, use_friendly_nm, def_order, getmail_at_login, mail_mode, mails_on_server_days, signature, signature_type, signature_opt, delimiter, mailbox_size, 0, false, -1, Namespace);
        }

        public override Account CreateAccount(int id_user, bool def_acct, bool deleted, string email, IncomingMailProtocol mail_protocol, string mail_inc_host, string mail_inc_login, string mail_inc_pass, int mail_inc_port, string mail_out_host, string mail_out_login, string mail_out_pass, int mail_out_port, bool mail_out_auth, string friendly_nm, bool use_friendly_nm, DefaultOrder def_order, bool getmail_at_login, MailMode mail_mode, short mails_on_server_days, string signature, SignatureType signature_type, SignatureOptions signature_opt, string delimiter, long mailbox_size, int id_domain, bool mailing_list, int imap_quota, string Namespace)
        {
            IDbTransaction myTrans = null;
            try
            {
                myTrans = _connection.BeginTransaction();

                IDbCommand command = _commandCreator.InsertIntoAwmAccounts(id_user, def_acct, deleted, email, mail_protocol, mail_inc_host, mail_inc_login, Utils.EncodePassword(mail_inc_pass), mail_inc_port, mail_out_host, mail_out_login, Utils.EncodePassword(mail_out_pass), mail_out_port, mail_out_auth, friendly_nm, use_friendly_nm, def_order, getmail_at_login, mail_mode, mails_on_server_days, signature, signature_type, signature_opt, delimiter, mailbox_size, id_domain, mailing_list, imap_quota, Namespace);
                command.Transaction = myTrans;
                command.ExecuteNonQuery();

                command.CommandText = _commandCreator.SelectIdentity();
                object obj = ExecuteScalarCommand(command);
                if (obj != null)
                {
                    myTrans.Commit();
                    return new Account(Convert.ToInt32(obj), def_acct, deleted, email, mail_protocol, mail_inc_host, mail_inc_login, mail_inc_pass, mail_inc_port, mail_out_host, mail_out_login, mail_out_pass, mail_out_port, mail_out_auth, friendly_nm, use_friendly_nm, def_order, getmail_at_login, mail_mode, mails_on_server_days, signature, signature_type, signature_opt, delimiter, mailbox_size, id_domain, mailing_list, imap_quota, Namespace);
                }
            	throw new WebMailDatabaseException("Can't create account");
            }
            catch (Exception ex)
            {
                if (myTrans != null) myTrans.Rollback();
                throw new WebMailDatabaseException(ex);
            }
        }

		public override User CreateUser(bool deleted)
		{
			IDbTransaction myTrans = null;
			User result = new User();
			result.Deleted = deleted;
			UserSettings settings = new UserSettings();

			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.InsertIntoAUsers(deleted);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				command.CommandText = _commandCreator.SelectIdentity();
				object obj = ExecuteScalarCommand(command);
				if (obj != null)
				{
					result.ID = Convert.ToInt32(obj);
				}
				else
				{
					throw new WebMailDatabaseException("Can't create user");
				}

				command = _commandCreator.InsertIntoAwmSettings(result.ID, settings.MsgsPerPage, settings.WhiteListing, settings.XSpam, settings.LastLogin, settings.LoginsCount, settings.DefaultSkin, settings.DefaultLanguage, settings.DefaultCharsetInc, settings.DefaultTimeZone, settings.DefaultDateFormat, settings.HideFolders, settings.MailboxLimit, settings.AllowChangeSettings, settings.AllowDhtmlEditor, settings.AllowDirectMode, false, settings.DbCharset, settings.HorizResizer, settings.VertResizer, settings.Mark, settings.Reply, settings.ContactsPerPage, settings.DefaultCharsetOut, (byte)settings.ViewMode);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				command.CommandText = _commandCreator.SelectIdentity();
				obj = ExecuteScalarCommand(command);
				if (obj == null)
				{
					settings = null;
				}
				else
				{
					settings.ID = Convert.ToInt32(obj);
				}
				myTrans.Commit();
				result.Settings = settings;
				return result;
			}
			catch(Exception ex)
			{
				if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public override Folder CreateFolder(int id_account, long id_parent, FolderType type, string name, string full_path, FolderSyncType sync_type, bool hide, short fld_order)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.InsertIntoAwmFolders(id_account, id_parent, type, Utils.ConvertToUtf7Modified(name) + "#", Utils.ConvertToUtf7Modified(full_path) + "#", sync_type, hide, fld_order);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				command.CommandText = _commandCreator.SelectIdentity();
				object obj = ExecuteScalarCommand(command);
				long id_folder = (obj != null) ? Convert.ToInt64(obj) : -1;
				if (id_folder > -1)
				{
					command = _commandCreator.InsertIntoAwmFoldersTree(id_folder);
					command.Transaction = myTrans;
					command.ExecuteNonQuery();

					command = _commandCreator.InsertIntoAwmFoldersTree(id_folder, id_parent);
					command.Transaction = myTrans;
					command.ExecuteNonQuery();
				}
				else
				{
					throw new WebMailDatabaseException("Can't create folder");
				}

				myTrans.Commit();
				return new Folder(id_folder, id_account, id_parent, name, full_path, type, sync_type, hide, fld_order);
			}
			catch(Exception ex)
			{
				if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		protected override Account ReadAccount(IDataReader reader)
		{
			Account acct = new Account();
			DataTable schemaTable = reader.GetSchemaTable();

			const int domainIndex = 0;
			string domainFieldName = reader.GetName(domainIndex);
			if (!reader.IsDBNull(domainIndex) && domainFieldName == "id_domain")
			{
				acct.IDDomain = Convert.ToInt32(reader.GetValue(domainIndex));
			}

			foreach (DataRow row in schemaTable.Rows)
			{
				int index = (int)row[1];
				if (reader.IsDBNull(index)) continue;
				string fieldName = row[0] as string;
				if (fieldName == null) continue;
				switch (fieldName.ToLower(CultureInfo.InvariantCulture))
				{
					case "id_acct":
						acct.ID = Convert.ToInt32(reader.GetValue(index));
						break;
					case "id_user":
						acct.IDUser = Convert.ToInt32(reader.GetValue(index));
						break;
					case "id_domain":
						acct.IDDomain = Convert.ToInt32(reader.GetValue(index));
						break;
					case "def_acct":
						acct.DefaultAccount = Convert.ToBoolean(reader.GetValue(index));
						break;
					case "deleted":
						acct.Deleted = Convert.ToBoolean(reader.GetValue(index));
						break;
					case "email":
						acct.Email = Convert.ToString(reader.GetValue(index));
						break;
					case "mail_protocol":
						if (acct.IDDomain == 0)
						{
							acct.MailIncomingProtocol = (IncomingMailProtocol) Convert.ToInt16(reader.GetValue(index));
						}
						break;
					case "mail_inc_host":
						if (acct.IDDomain == 0)
						{
							acct.MailIncomingHost = Convert.ToString(reader.GetValue(index));
						}
						break;
					case "mail_inc_login":
						acct.MailIncomingLogin = Convert.ToString(reader.GetValue(index));
						break;
					case "mail_inc_pass":
						string inc_pass = Convert.ToString(reader.GetValue(index));
						acct.MailIncomingPassword = Utils.DecodePassword(inc_pass);
						break;
					case "mail_inc_port":
						if (acct.IDDomain == 0)
						{
							acct.MailIncomingPort = Convert.ToInt32(reader.GetValue(index));
						}
						break;
					case "mail_out_host":
						if (acct.IDDomain == 0)
						{
							acct.MailOutgoingHost = Convert.ToString(reader.GetValue(index));
						}
						break;
					case "mail_out_login":
						acct.MailOutgoingLogin = Convert.ToString(reader.GetValue(index));
						break;
					case "mail_out_pass":
						string out_pass = Convert.ToString(reader.GetValue(index));
						acct.MailOutgoingPassword = Utils.DecodePassword(out_pass);
						break;
					case "mail_out_port":
						if (acct.IDDomain == 0)
						{
							acct.MailOutgoingPort = Convert.ToInt32(reader.GetValue(index));
						}
						break;
					case "mail_out_auth":
						if (acct.IDDomain == 0)
						{
							acct.MailOutgoingAuthentication = Convert.ToBoolean(reader.GetValue(index));
						}
						break;
					case "friendly_nm":
						acct.FriendlyName = Convert.ToString(reader.GetValue(index));
						break;
					case "use_friendly_nm":
						acct.UseFriendlyName = Convert.ToBoolean(reader.GetValue(index));
						break;
					case "def_order":
						acct.DefaultOrder = (DefaultOrder) Convert.ToInt16(reader.GetValue(index));
						break;
					case "getmail_at_login":
						acct.GetMailAtLogin = Convert.ToBoolean(reader.GetValue(index));
						break;
					case "mail_mode":
						acct.MailMode = (MailMode) Convert.ToInt16(reader.GetValue(index));
						break;
					case "mails_on_server_days":
						acct.MailsOnServerDays = Convert.ToInt16(reader.GetValue(index));
						break;
					case "signature":
						acct.Signature = Convert.ToString(reader.GetValue(index));
						break;
					case "signature_type":
						acct.SignatureType = (SignatureType) Convert.ToInt16(reader.GetValue(index));
						break;
					case "signature_opt":
						acct.SignatureOptions = (SignatureOptions) Convert.ToInt16(reader.GetValue(index));
						break;
					case "delimiter":
						acct.Delimiter = Convert.ToString(reader.GetValue(index));
						break;
					case "mailbox_size":
						acct.MailboxSize = Convert.ToInt64(reader.GetValue(index));
						break;
					case "mailing_list":
						acct.MailingList = Convert.ToBoolean(reader.GetValue(index));
						break;
					case "domain_mail_protocol":
						if (acct.IDDomain != 0)
						{
							acct.MailIncomingProtocol = (IncomingMailProtocol)Convert.ToInt16(reader.GetValue(index));
						}
						break;
					case "domain_mail_inc_host":
						if (acct.IDDomain != 0)
						{
							acct.MailIncomingHost = Convert.ToString(reader.GetValue(index));
						}
						break;
					case "domain_mail_inc_port":
						if (acct.IDDomain != 0)
						{
							acct.MailIncomingPort = Convert.ToInt32(reader.GetValue(index));
						}
						break;
					case "domain_mail_out_host":
						if (acct.IDDomain != 0)
						{
							acct.MailOutgoingHost = Convert.ToString(reader.GetValue(index));
						}
						break;
					case "domain_mail_out_port":
						if (acct.IDDomain != 0)
						{
							acct.MailOutgoingPort = Convert.ToInt32(reader.GetValue(index));
						}
						break;
					case "domain_mail_out_auth":
						if (acct.IDDomain != 0)
						{
							acct.MailOutgoingAuthentication = Convert.ToBoolean(reader.GetValue(index));
						}
						break;
                    case "imap_quota":
                        acct.Imap_quota = Convert.ToInt16(reader.GetValue(index));
                        break;
                }
			}
			return acct;
		}

		protected override User ReadUser(IDataReader reader)
		{
			DataTable schemaTable = reader.GetSchemaTable();
			int id_user = -1;
			bool deleted = false;

			foreach (DataRow row in schemaTable.Rows)
			{
				int index = (int)row[1];
				if (reader.IsDBNull(index)) continue;
				string fieldName = row[0] as string;
				if (fieldName != null)
				{
					switch (fieldName.ToLower(CultureInfo.InvariantCulture))
					{
						case "users.id_user":
							id_user = Convert.ToInt32(reader.GetValue(index));
							break;
						case "id_user":
							id_user = Convert.ToInt32(reader.GetValue(index));
							break;
						case "deleted":
							deleted = Convert.ToBoolean(reader.GetValue(index));
							break;
					}
				}
			}
			UserSettings settings = ReadUserSettings(reader);
			return new User(id_user, deleted, settings);
		}

		protected override UserSettings ReadUserSettings(IDataReader reader)
		{
			DataTable schemaTable = reader.GetSchemaTable();
			UserSettings settings = new UserSettings();

			foreach (DataRow row in schemaTable.Rows)
			{
				int index = (int)row[1];
				if (reader.IsDBNull(index)) continue;
				string fieldName = row[0] as string;
				if (fieldName != null)
				{
					switch (fieldName.ToLower(CultureInfo.InvariantCulture))
					{
						case "id_setting":
							settings.ID = Convert.ToInt32(reader.GetValue(index));
							break;
						case "users.id_user": // for Access
							settings.IDUser = Convert.ToInt32(reader.GetValue(index));
							break;
						case "id_user":
							settings.IDUser = Convert.ToInt32(reader.GetValue(index));
							break;
						case "msgs_per_page":
							settings.MsgsPerPage = Convert.ToInt16(reader.GetValue(index));
							break;
						case "white_listing":
							settings.WhiteListing = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "x_spam":
							settings.XSpam = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "last_login":
							settings.LastLogin = reader.GetDateTime(index);
							break;
						case "logins_count":
							settings.LoginsCount = Convert.ToInt32(reader.GetValue(index));
							break;
						case "def_skin":
							settings.DefaultSkin = Convert.ToString(reader.GetValue(index));
							break;
						case "def_lang":
							settings.DefaultLanguage = Convert.ToString(reader.GetValue(index));
							break;
						case "def_charset_inc":
							settings.DefaultCharsetInc = Convert.ToInt32(reader.GetValue(index));
							break;
						case "def_charset_out":
							settings.DefaultCharsetOut = Convert.ToInt32(reader.GetValue(index));
							break;
						case "def_timezone":
							settings.DefaultTimeZone = Convert.ToInt16(reader.GetValue(index));
							break;
						case "def_date_fmt":
                            string tempDateFormat = Convert.ToString(reader.GetValue(index));
                            if (tempDateFormat.Length > 2 && tempDateFormat.Substring(tempDateFormat.Length - 2) == Constants.timeFormat)
                            {
                                settings.DefaultTimeFormat = TimeFormats.F12;
                                settings.DefaultDateFormat = tempDateFormat.Substring(0, tempDateFormat.Length - 2);
                            }
                            else
                            {
                                settings.DefaultTimeFormat = TimeFormats.F24;
                                settings.DefaultDateFormat = tempDateFormat;
                            }
							break;
						case "hide_folders":
							settings.HideFolders = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "mailbox_limit":
							settings.MailboxLimit = Convert.ToInt64(reader.GetValue(index));
							break;
						case "allow_change_settings":
							settings.AllowChangeSettings = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "allow_dhtml_editor":
							settings.AllowDhtmlEditor = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "allow_direct_mode":
							settings.AllowDirectMode = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "db_charset":
							try
							{
								settings.DbCharset = Convert.ToInt32(reader.GetValue(index));
							}
							catch
							{
								settings.DbCharset = Encoding.UTF8.CodePage;
							}
							break;
						case "horiz_resizer":
							settings.HorizResizer = Convert.ToInt16(reader.GetValue(index));
							break;
						case "vert_resizer":
							settings.VertResizer = Convert.ToInt16(reader.GetValue(index));
							break;
						case "mark":
							settings.Mark = Convert.ToByte(reader.GetValue(index));
							break;
						case "reply":
							settings.Reply = Convert.ToByte(reader.GetValue(index));
							break;
						case "contacts_per_page":
							settings.ContactsPerPage = Convert.ToInt16(reader.GetValue(index));
							break;
						case "view_mode":
							settings.ViewMode = (ViewMode)Convert.ToByte(reader.GetValue(index));
							break;
					}
				}
			}
			return settings;
		}

		protected override Filter ReadFilter(IDataReader reader)
		{
			DataTable schemaTable = reader.GetSchemaTable();
			Filter result = new Filter(_account);

			foreach (DataRow row in schemaTable.Rows)
			{
				int index = (int)row[1];
				if (reader.IsDBNull(index)) continue;
				string fieldName = row[0] as string;
				if (fieldName != null)
				{
					switch (fieldName.ToLower(CultureInfo.InvariantCulture))
					{
						case "id_filter":
							result.IDFilter = Convert.ToInt32(reader.GetValue(index));
							break;
						case "id_acct":
							result.IDAcct = Convert.ToInt32(reader.GetValue(index));
							break;
						case "field":
							result.Field = (FilterField)Convert.ToByte(reader.GetValue(index));
							break;
						case "condition":
							result.Condition = (FilterCondition)Convert.ToByte(reader.GetValue(index));
							break;
						case "filter":
							result.FilterStr = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "action":
							result.Action = (FilterAction)Convert.ToByte(reader.GetValue(index));
							break;
						case "id_folder":
							result.IDFolder = Convert.ToInt64(reader.GetValue(index));
							break;
					}
				}
			}
			return result;
		}

		protected override Folder ReadFolder(IDataReader reader)
		{
			DataTable schemaTable = reader.GetSchemaTable();
			Folder result = new Folder();

			foreach (DataRow row in schemaTable.Rows)
			{
				int index = (int)row[1];
				if (reader.IsDBNull(index)) continue;
				string fieldName = row[0] as string;
				if (fieldName != null)
				{
					switch (fieldName.ToLower(CultureInfo.InvariantCulture))
					{
						case "id_folder":
							result.ID = Convert.ToInt64(reader.GetValue(index));
							break;
						case "id_acct":
							result.IDAcct = Convert.ToInt32(reader.GetValue(index));
							break;
						case "id_parent":
							result.IDParent = Convert.ToInt64(reader.GetValue(index));
							break;
						case "type":
							result.Type = (FolderType) Convert.ToInt16(reader.GetValue(index));
							break;
						case "name":
							string folderName = Convert.ToString(reader.GetValue(index));
							if (!string.IsNullOrEmpty(folderName))
							{
								// remove trailing symbol
								folderName = folderName.Remove(folderName.Length - 1, 1);
							}
							result.Name = Utils.ConvertFromUtf7Modified(folderName);
							break;
						case "full_path":
							string folderFullName = Convert.ToString(reader.GetValue(index));
							if (!string.IsNullOrEmpty(folderFullName))
							{
								// remove trailing symbol
								folderFullName = folderFullName.Remove(folderFullName.Length - 1, 1);
							}
							result.FullPath = Utils.ConvertFromUtf7Modified(folderFullName);
							break;
						case "sync_type":
							result.SyncType = (FolderSyncType) Convert.ToInt16(reader.GetValue(index));
							break;
						case "hide":
							result.Hide = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "fld_order":
							result.FolderOrder = Convert.ToInt16(reader.GetValue(index));
							break;
					}
				}
			}
			return result;
		}

		protected override WebMailMessage ReadWebMailMessage(IDataReader reader, Folder fld)
		{
			WebMailMessage webMsg = new WebMailMessage(_account);
			DataTable schemaTable = reader.GetSchemaTable();

			foreach (DataRow row in schemaTable.Rows)
			{
				int index = (int)row[1];
				if (reader.IsDBNull(index)) continue;
				string fieldName = row[0] as string;
				if (fieldName != null)
				{
					switch (fieldName.ToLower(CultureInfo.InvariantCulture))
					{
						case "id":
							webMsg.ID = Convert.ToInt64(reader.GetValue(index));
							break;
						case "id_msg":
							webMsg.IDMsg = Convert.ToInt32(reader.GetValue(index));
							break;
						case "id_acct":
							webMsg.IDAcct = Convert.ToInt32(reader.GetValue(index));
							break;
						case "id_folder_srv":
							webMsg.IDFolderSrv = Convert.ToInt64(reader.GetValue(index));
							break;
						case "id_folder_db":
							webMsg.IDFolderDB = Convert.ToInt64(reader.GetValue(index));
							break;
						case "str_uid":
							webMsg.StrUid = Convert.ToString(reader.GetValue(index));
							break;
						case "int_uid":
							webMsg.IntUid = Convert.ToInt64(reader.GetValue(index));
							break;
						case "from_msg":
							webMsg.FromMsg = EmailAddress.Parse(Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index))));
							break;
						case "to_msg":
							webMsg.ToMsg = EmailAddressCollection.Parse(Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index))));
							break;
						case "cc_msg":
							webMsg.CcMsg = EmailAddressCollection.Parse(Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index))));
							break;
						case "bcc_msg":
							webMsg.BccMsg = EmailAddressCollection.Parse(Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index))));
							break;
						case "subject":
							webMsg.Subject = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "msg_date":
							webMsg.MsgDate = reader.GetDateTime(index);
							break;
						case "attachments":
							webMsg.Attachments = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "size":
							webMsg.Size = Convert.ToInt64(reader.GetValue(index));
							break;
						case "seen":
							webMsg.Seen = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "flagged":
							webMsg.Flagged = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "priority":
							webMsg.Priority = (MailPriority)Convert.ToInt16(reader.GetValue(index));
							break;
						case "downloaded":
							webMsg.Downloaded = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "x_spam":
							webMsg.XSpam = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "rtl":
							webMsg.Rtl = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "deleted":
							webMsg.Deleted = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "is_full":
							webMsg.IsFull = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "replied":
							webMsg.Replied = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "forwarded":
							webMsg.Forwarded = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "flags":
							webMsg.Flags = (SystemMessageFlags)Convert.ToInt16(reader.GetValue(index));
							break;
						case "body_text":
							webMsg.BodyText = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "grayed":
							webMsg.Grayed = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "folder_name":
							webMsg.FolderFullName = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "charset":
							webMsg.OverrideCharset = Convert.ToInt32(reader.GetValue(index));
							break;
					}
				}
			}
			if (fld != null) webMsg.FolderFullName = fld.FullPath;
			return webMsg;
		}

		public override AddressBookContact CreateAddressBookContact(AddressBookContact contactToCreate)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();
				int id_user = contactToCreate.IDUser;
				if (_account != null)
				{
					if (_account.UserOfAccount != null) id_user = _account.UserOfAccount.ID;
				}
                IDbCommand command = _commandCreator.InsertIntoAwmAddrBook(id_user,
                    Utils.ConvertToDBString(_account, contactToCreate.HEmail),
                    Utils.ConvertToDBString(_account, contactToCreate.FullName),
                    Utils.ConvertToDBString(_account, contactToCreate.Notes),
                    contactToCreate.UseFriendlyName,
                    Utils.ConvertToDBString(_account, contactToCreate.HStreet),
                    Utils.ConvertToDBString(_account, contactToCreate.HCity),
                    Utils.ConvertToDBString(_account, contactToCreate.HState),
                    Utils.ConvertToDBString(_account, contactToCreate.HZip),
                    Utils.ConvertToDBString(_account, contactToCreate.HCountry),
                    Utils.ConvertToDBString(_account, contactToCreate.HPhone),
                    Utils.ConvertToDBString(_account, contactToCreate.HFax),
                    Utils.ConvertToDBString(_account, contactToCreate.HMobile),
                    Utils.ConvertToDBString(_account, contactToCreate.HWeb),
                    Utils.ConvertToDBString(_account, contactToCreate.BEmail),
                    Utils.ConvertToDBString(_account, contactToCreate.BCompany),
                    Utils.ConvertToDBString(_account, contactToCreate.BStreet),
                    Utils.ConvertToDBString(_account, contactToCreate.BCity),
                    Utils.ConvertToDBString(_account, contactToCreate.BState),
                    Utils.ConvertToDBString(_account, contactToCreate.BZip),
                    Utils.ConvertToDBString(_account, contactToCreate.BCountry),
                    Utils.ConvertToDBString(_account, contactToCreate.BJobTitle),
                    Utils.ConvertToDBString(_account, contactToCreate.BDepartment),
                    Utils.ConvertToDBString(_account, contactToCreate.BOffice),
                    Utils.ConvertToDBString(_account, contactToCreate.BPhone),
                    Utils.ConvertToDBString(_account, contactToCreate.BFax),
                    Utils.ConvertToDBString(_account, contactToCreate.BWeb),
                    contactToCreate.BirthdayDay, contactToCreate.BirthdayMonth, contactToCreate.BirthdayYear,
                    Utils.ConvertToDBString(_account, contactToCreate.OtherEmail),
                    (short)contactToCreate.PrimaryEmail, contactToCreate.IDAddrPrev, contactToCreate.Tmp,
                    contactToCreate.UseFrequency, contactToCreate.AutoCreate,
                    Utils.ConvertToDBString(_account, contactToCreate.StrID),
                    contactToCreate.DateModified);
                command.Transaction = myTrans;
				command.ExecuteNonQuery();

				command.CommandText = _commandCreator.SelectIdentity();
				object obj = ExecuteScalarCommand(command);
				if (obj != null)
				{
					contactToCreate.IDAddr = Convert.ToInt64(obj);
				}
				else
				{
					throw new WebMailDatabaseException("Can't create contact");
				}
				if ((contactToCreate.Groups != null) && (contactToCreate.Groups.Length > 0))
				{
					for (int i = 0; i < contactToCreate.Groups.Length; i++)
					{
						command = _commandCreator.InsertIntoAwmAddrGroupsContacts(contactToCreate.IDAddr, contactToCreate.Groups[i].IDGroup);
						command.Transaction = myTrans;
						command.ExecuteNonQuery();
					}
				}
				myTrans.Commit();
				return contactToCreate;
			}
			catch(Exception ex)
			{
				if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public override AddressBookGroup CreateAddressBookGroup(AddressBookGroup groupToCreate)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.InsertIntoAwmAddrGroups(groupToCreate.IDUser,
					Utils.ConvertToDBString(_account, groupToCreate.GroupName),
					Utils.ConvertToDBString(_account, groupToCreate.Phone),
					Utils.ConvertToDBString(_account, groupToCreate.Fax),
					Utils.ConvertToDBString(_account, groupToCreate.Web),
					groupToCreate.Organization, groupToCreate.UseFrequency,
					Utils.ConvertToDBString(_account, groupToCreate.Email),
					Utils.ConvertToDBString(_account, groupToCreate.Company),
					Utils.ConvertToDBString(_account, groupToCreate.Street),
					Utils.ConvertToDBString(_account, groupToCreate.City),
					Utils.ConvertToDBString(_account, groupToCreate.State),
					Utils.ConvertToDBString(_account, groupToCreate.Zip),
					Utils.ConvertToDBString(_account, groupToCreate.Country),
                    Utils.ConvertToDBString(_account, groupToCreate.StrID));
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				command.CommandText = _commandCreator.SelectIdentity();
				object obj = ExecuteScalarCommand(command);
				if (obj != null)
				{
					groupToCreate.IDGroup = Convert.ToInt32(obj);
				}
				else
				{
					throw new WebMailDatabaseException("Can't create group");
				}

				command = _commandCreator.DeleteFromAwmAddrGroupContacts(groupToCreate.IDGroup);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				foreach (AddressBookContact contact in groupToCreate.Contacts)
				{
					command = _commandCreator.InsertIntoAwmAddrGroupsContacts(contact.IDAddr, groupToCreate.IDGroup);
					command.Transaction = myTrans;
					command.ExecuteNonQuery();
				}

				foreach (AddressBookContact contactToCreate in groupToCreate.NewContacts)
				{
					command = _commandCreator.InsertIntoAwmAddrBook(groupToCreate.IDUser,
						Utils.ConvertToDBString(_account, contactToCreate.HEmail),
						Utils.ConvertToDBString(_account, contactToCreate.FullName),
						Utils.ConvertToDBString(_account, contactToCreate.Notes),
						contactToCreate.UseFriendlyName,
						Utils.ConvertToDBString(_account, contactToCreate.HStreet),
						Utils.ConvertToDBString(_account, contactToCreate.HCity),
						Utils.ConvertToDBString(_account, contactToCreate.HState),
						Utils.ConvertToDBString(_account, contactToCreate.HZip),
						Utils.ConvertToDBString(_account, contactToCreate.HCountry),
						Utils.ConvertToDBString(_account, contactToCreate.HPhone),
						Utils.ConvertToDBString(_account, contactToCreate.HFax),
						Utils.ConvertToDBString(_account, contactToCreate.HMobile),
						Utils.ConvertToDBString(_account, contactToCreate.HWeb),
						Utils.ConvertToDBString(_account, contactToCreate.BEmail),
						Utils.ConvertToDBString(_account, contactToCreate.BCompany),
						Utils.ConvertToDBString(_account, contactToCreate.BStreet),
						Utils.ConvertToDBString(_account, contactToCreate.BCity),
						Utils.ConvertToDBString(_account, contactToCreate.BState),
						Utils.ConvertToDBString(_account, contactToCreate.BZip),
						Utils.ConvertToDBString(_account, contactToCreate.BCountry),
						Utils.ConvertToDBString(_account, contactToCreate.BJobTitle),
						Utils.ConvertToDBString(_account, contactToCreate.BDepartment),
						Utils.ConvertToDBString(_account, contactToCreate.BOffice),
						Utils.ConvertToDBString(_account, contactToCreate.BPhone),
						Utils.ConvertToDBString(_account, contactToCreate.BFax),
						Utils.ConvertToDBString(_account, contactToCreate.BWeb),
						contactToCreate.BirthdayDay, contactToCreate.BirthdayMonth, contactToCreate.BirthdayYear,
						Utils.ConvertToDBString(_account, contactToCreate.OtherEmail),
						(short)contactToCreate.PrimaryEmail, contactToCreate.IDAddrPrev, contactToCreate.Tmp,
						contactToCreate.UseFrequency, contactToCreate.AutoCreate,
						Utils.ConvertToDBString(_account, contactToCreate.StrID),
						contactToCreate.DateModified);
					command.Transaction = myTrans;
					command.ExecuteNonQuery();

					command.CommandText = _commandCreator.SelectIdentity();
					obj = ExecuteScalarCommand(command);
					if (obj != null)
					{
						contactToCreate.IDAddr = Convert.ToInt64(obj);
						command = _commandCreator.InsertIntoAwmAddrGroupsContacts(contactToCreate.IDAddr, groupToCreate.IDGroup);
						command.Transaction = myTrans;
						command.ExecuteNonQuery();
					}
				}

				myTrans.Commit();
				return groupToCreate;
			}
			catch(Exception ex)
			{
				if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

        public override AddressBookGroupContact[] SelectAddressBookContactsGroups(int page, int sort_field, int sort_order, int id_group, string look_for, int look_for_type)
        {
            ArrayList contacts = new ArrayList();
            IDbTransaction myTrans = null;
            try
            {
                myTrans = _connection.BeginTransaction();

                string filter = "email";
                switch (sort_field)
                {
                    case 0:
                        filter = "is_group";
                        break;
                    case 1:
                        filter = "name";
                        break;
                    case 2:
                        filter = "email";
                        break;
                    case 3:
                        filter = "frequency";
                        break;
                }

                int id_domain = -1;
                if (look_for_type == 1)
                {
                    WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
                    if (settings.GlobalAddressBook == GlobalAddressBookEnum.DomainWide)
                    {
                        IDbCommand commandSelectDomain = _commandCreator.SelectAwmDomain(_account.IDDomain);
                        commandSelectDomain.Transaction = myTrans;
                        using (IDataReader dataReader = commandSelectDomain.ExecuteReader())
                        {
                            if (dataReader.Read())
                            {
                                Domain dom = ReadDomain(dataReader);
                                if (dom.Global_addr_book)
                                {
                                    id_domain = dom.ID;
                                }
                            }
                        }
                    }
                }

                IDbCommand command;
                if (!string.IsNullOrEmpty(look_for))
                {
                    command = _commandCreator.SearchAwmAddrBookGroups(_account.UserOfAccount.ID, _account.UserOfAccount.Settings.ContactsPerPage, page, filter, sort_order, id_group, Utils.ConvertToDBString(_account, look_for), look_for_type, id_domain);
                }
                else
                {
                    command = _commandCreator.SelectAwmAddrBookGroups(_account.UserOfAccount.ID, _account.UserOfAccount.Settings.ContactsPerPage, page, sort_field, sort_order);
                }
                command.Transaction = myTrans;
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AddressBookGroupContact group_contact = new AddressBookGroupContact();
                        DataTable schemaTable = reader.GetSchemaTable();
                        foreach (DataRow row in schemaTable.Rows)
                        {
                            int index = (int)row[1];
                            if (reader.IsDBNull(index)) continue;
                            string fieldName = row[0] as string;
                            if (fieldName != null)
                            {
                                switch (fieldName.ToLower(CultureInfo.InvariantCulture))
                                {
                                    case "id":
                                        group_contact.id = Convert.ToInt64(reader.GetValue(index));
                                        break;
                                    case "name":
                                        group_contact.fullname = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
                                        break;
                                    case "email":
                                        group_contact.email = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
                                        break;
                                    case "is_group":
                                        group_contact.isGroup = Convert.ToBoolean(reader.GetValue(index));
                                        break;
                                }
                            }
                        }
                        contacts.Add(group_contact);
                    }
                }
                myTrans.Commit();
                return (AddressBookGroupContact[])contacts.ToArray(typeof(AddressBookGroupContact));
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
                throw new WebMailDatabaseException(ex);
            }
        }
        
        public override void UpdateAddressBookGroup(AddressBookGroup groupToUpdate)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.UpdateAwmAddrGroups(groupToUpdate.IDGroup, groupToUpdate.IDUser,
					Utils.ConvertToDBString(_account, groupToUpdate.GroupName),
					Utils.ConvertToDBString(_account, groupToUpdate.Phone),
					Utils.ConvertToDBString(_account, groupToUpdate.Fax),
					Utils.ConvertToDBString(_account, groupToUpdate.Web),
					groupToUpdate.Organization, groupToUpdate.UseFrequency,
					Utils.ConvertToDBString(_account, groupToUpdate.Email),
					Utils.ConvertToDBString(_account, groupToUpdate.Company),
					Utils.ConvertToDBString(_account, groupToUpdate.Street),
					Utils.ConvertToDBString(_account, groupToUpdate.City),
					Utils.ConvertToDBString(_account, groupToUpdate.State),
					Utils.ConvertToDBString(_account, groupToUpdate.Zip),
					Utils.ConvertToDBString(_account, groupToUpdate.Country),
                    Utils.ConvertToDBString(_account, groupToUpdate.StrID));
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				command = _commandCreator.DeleteFromAwmAddrGroupContacts(groupToUpdate.IDGroup);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				foreach (AddressBookContact contact in groupToUpdate.Contacts)
				{
					command = _commandCreator.InsertIntoAwmAddrGroupsContacts(contact.IDAddr, groupToUpdate.IDGroup);
					command.Transaction = myTrans;
					command.ExecuteNonQuery();
				}

				foreach (AddressBookContact contactToCreate in groupToUpdate.NewContacts)
				{
					command = _commandCreator.InsertIntoAwmAddrBook(groupToUpdate.IDUser,
						Utils.ConvertToDBString(_account, contactToCreate.HEmail),
						Utils.ConvertToDBString(_account, contactToCreate.FullName),
						Utils.ConvertToDBString(_account, contactToCreate.Notes),
						contactToCreate.UseFriendlyName,
						Utils.ConvertToDBString(_account, contactToCreate.HStreet),
						Utils.ConvertToDBString(_account, contactToCreate.HCity),
						Utils.ConvertToDBString(_account, contactToCreate.HState),
						Utils.ConvertToDBString(_account, contactToCreate.HZip),
						Utils.ConvertToDBString(_account, contactToCreate.HCountry),
						Utils.ConvertToDBString(_account, contactToCreate.HPhone),
						Utils.ConvertToDBString(_account, contactToCreate.HFax),
						Utils.ConvertToDBString(_account, contactToCreate.HMobile),
						Utils.ConvertToDBString(_account, contactToCreate.HWeb),
						Utils.ConvertToDBString(_account, contactToCreate.BEmail),
						Utils.ConvertToDBString(_account, contactToCreate.BCompany),
						Utils.ConvertToDBString(_account, contactToCreate.BStreet),
						Utils.ConvertToDBString(_account, contactToCreate.BCity),
						Utils.ConvertToDBString(_account, contactToCreate.BState),
						Utils.ConvertToDBString(_account, contactToCreate.BZip),
						Utils.ConvertToDBString(_account, contactToCreate.BCountry),
						Utils.ConvertToDBString(_account, contactToCreate.BJobTitle),
						Utils.ConvertToDBString(_account, contactToCreate.BDepartment),
						Utils.ConvertToDBString(_account, contactToCreate.BOffice),
						Utils.ConvertToDBString(_account, contactToCreate.BPhone),
						Utils.ConvertToDBString(_account, contactToCreate.BFax),
						Utils.ConvertToDBString(_account, contactToCreate.BWeb),
						contactToCreate.BirthdayDay, contactToCreate.BirthdayMonth, contactToCreate.BirthdayYear,
						Utils.ConvertToDBString(_account, contactToCreate.OtherEmail),
						(short)contactToCreate.PrimaryEmail, contactToCreate.IDAddrPrev, contactToCreate.Tmp,
                        contactToCreate.UseFrequency, contactToCreate.AutoCreate,
                        Utils.ConvertToDBString(_account, contactToCreate.StrID),
                        contactToCreate.DateModified);
                    command.Transaction = myTrans;
					object obj = ExecuteScalarCommand(command);
					if (obj != null)
					{
						contactToCreate.IDAddr = Convert.ToInt64(obj);
						command = _commandCreator.InsertIntoAwmAddrGroupsContacts(contactToCreate.IDAddr, groupToUpdate.IDGroup);
						command.Transaction = myTrans;
						command.ExecuteNonQuery();
					}
				}

				myTrans.Commit();
			}
			catch(Exception ex)
			{
				if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		protected override AddressBookContact ReadAddressBookContact(IDataReader reader)
		{
			AddressBookContact result = new AddressBookContact();
			DataTable schemaTable = reader.GetSchemaTable();

			foreach (DataRow row in schemaTable.Rows)
			{
				int index = (int)row[1];
				if (reader.IsDBNull(index)) continue;
				string fieldName = row[0] as string;
				if (fieldName != null)
				{
					switch (fieldName.ToLower(CultureInfo.InvariantCulture))
					{
						case "id_addr":
							result.IDAddr = Convert.ToInt64(reader.GetValue(index));
							break;
						case "id_user":
							result.IDUser = Convert.ToInt32(reader.GetValue(index));
							break;
						case "h_email":
							result.HEmail = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "fullname":
							result.FullName = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "notes":
							result.Notes = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "use_friendly_nm":
							result.UseFriendlyName = (Convert.ToByte(reader.GetValue(index)) == 1) ? true : false;
							break;
						case "h_street":
							result.HStreet = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "h_city":
							result.HCity = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "h_state":
							result.HState = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "h_zip":
							result.HZip = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "h_country":
							result.HCountry = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "h_phone":
							result.HPhone = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "h_fax":
							result.HFax = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "h_mobile":
							result.HMobile = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "h_web":
							result.HWeb = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "b_email":
							result.BEmail = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "b_company":
							result.BCompany = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "b_street":
							result.BStreet = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "b_city":
							result.BCity = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "b_state":
							result.BState = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "b_zip":
							result.BZip = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "b_country":
							result.BCountry = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "b_job_title":
							result.BJobTitle = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "b_department":
							result.BDepartment = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "b_office":
							result.BOffice = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "b_phone":
							result.BPhone = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "b_fax":
							result.BFax = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "b_web":
							result.BWeb = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "birthday_day":
							result.BirthdayDay = Convert.ToByte(reader.GetValue(index));
							break;
						case "birthday_month":
							result.BirthdayMonth = Convert.ToByte(reader.GetValue(index));
							break;
						case "birthday_year":
							result.BirthdayYear = Convert.ToInt16(reader.GetValue(index));
							break;
						case "other_email":
							result.OtherEmail = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
							break;
						case "primary_email":
							result.PrimaryEmail = (ContactPrimaryEmail)Convert.ToInt16(reader.GetValue(index));
							break;
						case "id_addr_prev":
							result.IDAddrPrev = Convert.ToInt64(reader.GetValue(index));
							break;
						case "tmp":
							result.Tmp = Convert.ToBoolean(reader.GetValue(index));
							break;
						case "use_frequency":
							result.UseFrequency = Convert.ToInt32(reader.GetValue(index));
							break;
						case "auto_create":
							result.AutoCreate = (Convert.ToByte(reader.GetValue(index)) == 1) ? true : false;
							break;
					}
				}
			}
			return result;
		}
        #region Domains Functions

        public override void DeleteDomain(int id_domain)
        {
            IDbTransaction myTrans = null;
            try
            {
                myTrans = _connection.BeginTransaction();

                IDbCommand command = _commandCreator.DeleteFromAwmDomains(id_domain);
                command.Transaction = myTrans;
                command.ExecuteNonQuery();
                myTrans.Commit();
            }
            catch (Exception ex)
            {
                if (myTrans != null) myTrans.Rollback();
                throw new WebMailDatabaseException(ex);
            }
        }

        public override int CreateDomain(string name, IncomingMailProtocol mail_protocol,
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
            int result = 0;
            IDbTransaction myTrans = null;
            if ((mail_inc_host.Trim().Length == 0) || (mail_out_host.Trim().Length == 0) || (name.Trim().Length == 0))
            {
                throw new Exception("Failed to save domain.");
            }
            try
            {
                myTrans = _connection.BeginTransaction();
                IDbCommand command = _commandCreator.InsertIntoAwmDomains(name, (short)mail_protocol,
                      mail_inc_host, mail_inc_port, mail_out_host,
                      mail_out_port, mail_out_auth, url, site_name,
                      (short)settings_mail_protocol, settings_mail_inc_host,
                      settings_mail_inc_port, settings_mail_out_host,
                      settings_mail_out_port, settings_mail_out_auth,
                      allow_direct_mode, direct_mode_id_def, attachment_size_limit,
                      allow_attachment_limit, mailbox_size_limit, allow_mailbox_limit,
                      take_quota, allow_new_users_change_settings, allow_auto_reg_on_login,
                      allow_users_add_accounts, allow_users_change_account_def,
                      def_user_charset, allow_users_change_charset, def_user_timezone,
                      allow_users_change_timezone, msgs_per_page, skin,
                      allow_users_change_skin, lang, allow_users_change_lang,
                      show_text_labels, allow_ajax, allow_editor, allow_contacts,
                      allow_calendar, (short)hide_login_mode,
                      domain_to_use, allow_choosing_lang, allow_advanced_login,
					  allow_auto_detect_and_correct, contacts_sharing, (short)viewmode, (short)save_mail);
                command.Transaction = myTrans;
                command.ExecuteNonQuery();

                command.CommandText = _commandCreator.SelectIdentity();
                object obj = ExecuteScalarCommand(command);
                if (obj != null)
                {
                    result = Convert.ToInt32(obj);
                }
                myTrans.Commit();
            }
            catch (Exception ex)
            {
                if (myTrans != null) myTrans.Rollback();
                throw new WebMailDatabaseException(ex);
            }
            return result;
        }

        public override int GetDomainsCount(string condition)
        {
            IDbTransaction myTrans = null;
            try
            {
                myTrans = _connection.BeginTransaction();

                IDbCommand command = _commandCreator.SelectAwmDomainsCount(condition);
                command.Transaction = myTrans;
                object obj = ExecuteScalarCommand(command);
                myTrans.Commit();

                if (obj != null)
                {
                    return Convert.ToInt32(obj);
                }
                return 0;
            }
            catch (Exception ex)
            {
                if (myTrans != null) myTrans.Rollback();
                throw new WebMailDatabaseException(ex);
            }
        }

        public override Domain SelectDomainData(int id_domain)
        {
            Domain dom = null;
            IDbTransaction myTrans = null;
            try
            {
                myTrans = _connection.BeginTransaction();

                IDbCommand command = _commandCreator.SelectAwmDomain(id_domain);
                command.Transaction = myTrans;
                using (IDataReader dataReader = command.ExecuteReader())
                {
                    if (dataReader.Read())
                    {
                        dom = ReadDomain(dataReader);
                    }
                }
                myTrans.Commit();
                return dom;
            }
            catch (Exception ex)
            {
                if (myTrans != null) myTrans.Rollback();
                throw new WebMailDatabaseException(ex);
            }
        }

        public override Domain SelectDomainData(string domain)
        {
            Domain dom = null;
            IDbTransaction myTrans = null;
            try
            {
                myTrans = _connection.BeginTransaction();

                IDbCommand command = _commandCreator.SelectAwmDomain(domain);
                command.Transaction = myTrans;
                using (IDataReader dataReader = command.ExecuteReader())
                {
                    if (dataReader.Read())
                    {
                        dom = ReadDomain(dataReader);
                    }
                }
                myTrans.Commit();
                return dom;
            }
            catch (Exception ex)
            {
                if (myTrans != null) myTrans.Rollback();
                throw new WebMailDatabaseException(ex);
            }
        }

        public override void UpdateDomain(int id_domain, string name, IncomingMailProtocol mail_protocol,
                      string mail_inc_host, int mail_inc_port, string mail_out_host,
                      int mail_out_port, bool mail_out_auth)
        {
            IDbTransaction myTrans = null;
            if ((mail_inc_host.Trim().Length == 0) || (mail_out_host.Trim().Length == 0) || (name.Trim().Length == 0))
            {
                throw new Exception("Failed to save domain.");
            }
            try
            {
                myTrans = _connection.BeginTransaction();
                IDbCommand command = _commandCreator.UpdateAwmDomains(id_domain, name, (short)mail_protocol,
                      mail_inc_host, mail_inc_port, mail_out_host,
                      mail_out_port, mail_out_auth);
                command.Transaction = myTrans;
                command.ExecuteNonQuery();
                myTrans.Commit();
            }
            catch (Exception ex)
            {
                if (myTrans != null) myTrans.Rollback();
                throw new WebMailDatabaseException(ex);
            }
        }
        
        public override void UpdateDomain(int id_domain, string name, IncomingMailProtocol mail_protocol,
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
            IDbTransaction myTrans = null;
            if ((mail_inc_host.Trim().Length == 0) || (mail_out_host.Trim().Length == 0) || (name.Trim().Length == 0))
            {
                throw new Exception("Failed to save domain.");
            }
            try
            {
                myTrans = _connection.BeginTransaction();
                IDbCommand command = _commandCreator.UpdateAwmDomains(id_domain, name, (short)mail_protocol,
                      mail_inc_host, mail_inc_port, mail_out_host,
                      mail_out_port, mail_out_auth, url, site_name,
                      (short)settings_mail_protocol, settings_mail_inc_host,
                      settings_mail_inc_port, settings_mail_out_host,
                      settings_mail_out_port, settings_mail_out_auth,
                      allow_direct_mode, direct_mode_id_def, attachment_size_limit,
                      allow_attachment_limit, mailbox_size_limit, allow_mailbox_limit,
                      take_quota, allow_new_users_change_settings, allow_auto_reg_on_login,
                      allow_users_add_accounts, allow_users_change_account_def,
                      def_user_charset, allow_users_change_charset, def_user_timezone,
                      allow_users_change_timezone, msgs_per_page, skin,
                      allow_users_change_skin, lang, allow_users_change_lang,
                      show_text_labels, allow_ajax, allow_editor, allow_contacts,
                      allow_calendar, (short)hide_login_mode,
                      domain_to_use, allow_choosing_lang, allow_advanced_login,
					  allow_auto_detect_and_correct, contacts_sharing, (short)viewmode, (short)save_mail);
                command.Transaction = myTrans;
                command.ExecuteNonQuery();
                myTrans.Commit();
            }
            catch (Exception ex)
            {
                if (myTrans != null) myTrans.Rollback();
                throw new WebMailDatabaseException(ex);
            }
        }

        protected override Domain ReadDomain(IDataReader reader)
        {
            Domain dom = new Domain();
            DataTable schemaTable = reader.GetSchemaTable();

            foreach (DataRow row in schemaTable.Rows)
            {
                int index = (int)row[1];
                if (reader.IsDBNull(index)) continue;
                string fieldName = row[0] as string;
                if (fieldName != null)
                {
                    switch (fieldName.ToLower(CultureInfo.InvariantCulture))
                    {
                        case "id_domain":
                            dom.ID = reader.GetInt32(index);
                            break;
                        case "name":
                            dom.Name = reader.GetString(index);
                            break;
                        case "mail_protocol":
                            dom.MailIncomingProtocol = (IncomingMailProtocol)Convert.ToInt16(reader.GetValue(index));
                            break;
                        case "mail_inc_host":
                            dom.MailIncomingHost = reader.GetString(index);
                            break;
                        case "mail_inc_port":
                            dom.MailIncomingPort = reader.GetInt32(index);
                            break;
                        case "mail_out_host":
                            dom.MailOutgoingHost = reader.GetString(index);
                            break;
                        case "mail_out_port":
                            dom.MailOutgoingPort = reader.GetInt32(index);
                            break;
                        case "mail_out_auth":
                            dom.MailOutgoingAuthentication = Convert.ToBoolean(reader.GetValue(index));
                            break;

                        case "url":
                            dom.Url = reader.GetString(index);
                            break;
                        case "site_name":
                            dom.Site_name = reader.GetString(index);
                            break;
                        case "settings_mail_protocol":
                            dom.Settings_mail_protocol = (IncomingMailProtocol)reader.GetInt16(index);
                            break;
                        case "settings_mail_inc_host":
                            dom.Settings_mail_inc_host = reader.GetString(index);
                            break;
                        case "settings_mail_inc_port":
                            dom.Settings_mail_inc_port = reader.GetInt32(index);
                            break;
                        case "settings_mail_out_host":
                            dom.Settings_mail_out_host = reader.GetString(index);
                            break;
                        case "settings_mail_out_port":
                            dom.Settings_mail_out_port = reader.GetInt32(index);
                            break;
                        case "settings_mail_out_auth":
                            dom.Settings_mail_out_auth = Convert.ToBoolean(reader.GetValue(index));
                            break;
                        case "allow_direct_mode":
                            dom.Allow_direct_mode = Convert.ToBoolean(reader.GetValue(index));
                            break;
                        case "direct_mode_id_def":
                            dom.Direct_mode_id_def = Convert.ToBoolean(reader.GetValue(index));
                            break;
                        case "attachment_size_limit":
                            dom.Attachment_size_limit = reader.GetInt64(index);
                            break;
                        case "allow_attachment_limit":
                            dom.Allow_attachment_limit = Convert.ToBoolean(reader.GetValue(index));
                            break;
                        case "mailbox_size_limit":
                            dom.Mailbox_size_limit = reader.GetInt64(index);
                            break;
                        case "allow_mailbox_limit":
                            dom.Allow_mailbox_limit = Convert.ToBoolean(reader.GetValue(index));
                            break;
                        case "take_quota":
                            dom.Take_quota = Convert.ToBoolean(reader.GetValue(index));
                            break;
                        case "allow_new_users_change_settings":
                            dom.Allow_new_users_change_settings = Convert.ToBoolean(reader.GetValue(index));
                            break;
                        case "allow_auto_reg_on_login":
                            dom.Allow_auto_reg_on_login = Convert.ToBoolean(reader.GetValue(index));
                            break;
                        case "allow_users_add_accounts":
                            dom.Allow_users_add_accounts = Convert.ToBoolean(reader.GetValue(index));
                            break;
                        case "allow_users_change_account_def":
                            dom.Allow_users_change_account_def = Convert.ToBoolean(reader.GetValue(index));
                            break;
                        case "def_user_charset":
                            dom.Def_user_charset = reader.GetInt32(index);
                            break;
                        case "allow_users_change_charset":
                            dom.Allow_users_change_charset = Convert.ToBoolean(reader.GetValue(index));
                            break;
                        case "def_user_timezone":
                            dom.Def_user_timezone = reader.GetInt32(index);
                            break;
                        case "allow_users_change_timezone":
                            dom.Allow_users_change_timezone = Convert.ToBoolean(reader.GetValue(index));
                            break;
                        case "msgs_per_page":
                            dom.Msgs_per_page = reader.GetInt32(index);
                            break;
                        case "skin":
                            dom.Skin = reader.GetString(index);
                            break;
                        case "allow_users_change_skin":
                            dom.Allow_users_change_skin = Convert.ToBoolean(reader.GetValue(index));
                            break;
                        case "lang":
                            dom.Lang = reader.GetString(index);
                            break;
                        case "allow_users_change_lang":
                            dom.Allow_users_change_lang = Convert.ToBoolean(reader.GetValue(index));
                            break;
                        case "show_text_labels":
                            dom.Show_text_labels = Convert.ToBoolean(reader.GetValue(index));
                            break;
                        case "allow_ajax":
                            dom.Allow_ajax = Convert.ToBoolean(reader.GetValue(index));
                            break;
                        case "allow_editor":
                            dom.Allow_editor = Convert.ToBoolean(reader.GetValue(index));
                            break;
                        case "allow_contacts":
                            dom.Allow_contacts = Convert.ToBoolean(reader.GetValue(index));
                            break;
                        case "allow_calendar":
                            dom.Allow_calendar = Convert.ToBoolean(reader.GetValue(index));
                            break;
                        case "hide_login_mode":
                            dom.Hide_login_mode = (LoginMode)reader.GetInt16(index);
                            break;
                        case "domain_to_use":
                            dom.Domain_to_use = reader.GetString(index);
                            break;
                        case "allow_choosing_lang":
                            dom.Allow_choosing_lang = Convert.ToBoolean(reader.GetValue(index));
                            break;
                        case "allow_advanced_login":
                            dom.Allow_advanced_login = Convert.ToBoolean(reader.GetValue(index));
                            break;
                        case "allow_auto_detect_and_correct":
                            dom.Allow_auto_detect_and_correct = Convert.ToBoolean(reader.GetValue(index));
                            break;
                    }
                }
            }
            return dom;
        }
        #endregion     

	}
}