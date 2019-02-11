using System;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Collections.Generic;
using System.Collections;

namespace WebMail
{
	public class MsAccessDbManager : DbManager
	{
		public MsAccessDbManager() : this((Account) null) {}

		public MsAccessDbManager(Account acct) : base(acct)
		{
			_connection = new OleDbConnection();
			_commandCreator = new MsAccessCommandCreator(_connection as OleDbConnection, new OleDbCommand());
		}

		public MsAccessDbManager(string dataFolder)
		{
			_dataFolder = dataFolder;
			_connection = new OleDbConnection();
			_commandCreator = new MsAccessCommandCreator(_connection as OleDbConnection, new OleDbCommand(), dataFolder);
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
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
                throw new WebMailDatabaseException(ex);
            }
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
                Log.WriteException(ex);
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
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}
		public override UserColumn CreateUserColumn(int id_column, int id_user, int value)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.InsertIntoAwmColumns(id_column, id_user, value);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				command.CommandText = _commandCreator.SelectIdentity();
				object obj = ExecuteScalarCommand(command);
				myTrans.Commit();

				int id = (obj != null) ? Convert.ToInt32(obj) : -1;

				return new UserColumn(id, id_column, id_user, value);
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
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
                Log.WriteException(ex);
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
					command.ExecuteNonQuery();

					command.CommandText = _commandCreator.SelectIdentity();
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
                Log.WriteException(ex);
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
				acct.IDDomain = reader.GetInt32(domainIndex);
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
						acct.ID = reader.GetInt32(index);
						break;
					case "id_user":
						acct.IDUser = reader.GetInt32(index);
						break;
					case "id_domain":
						acct.IDDomain = reader.GetInt32(index);
						break;
					case "def_acct":
						acct.DefaultAccount = reader.GetBoolean(index);
						break;
					case "deleted":
						acct.Deleted = reader.GetBoolean(index);
						break;
					case "email":
						acct.Email = reader.GetString(index);
						break;
					case "mail_protocol":
						if (acct.IDDomain == 0)
						{
							acct.MailIncomingProtocol = (IncomingMailProtocol) reader.GetInt16(index);
						}
						break;
					case "mail_inc_host":
						if (acct.IDDomain == 0)
						{
							acct.MailIncomingHost = reader.GetString(index);
						}
						break;
					case "mail_inc_login":
						acct.MailIncomingLogin = reader.GetString(index);
						break;
					case "mail_inc_pass":
						string inc_pass = reader.GetString(index);
						acct.MailIncomingPassword = Utils.DecodePassword(inc_pass);
						break;
					case "mail_inc_port":
						if (acct.IDDomain == 0)
						{
							acct.MailIncomingPort = reader.GetInt32(index);
						}
						break;
					case "mail_out_host":
						if (acct.IDDomain == 0)
						{
							acct.MailOutgoingHost = reader.GetString(index);
						}
						break;
					case "mail_out_login":
						acct.MailOutgoingLogin = reader.GetString(index);
						break;
					case "mail_out_pass":
						string out_pass = reader.GetString(index);
						acct.MailOutgoingPassword = Utils.DecodePassword(out_pass);
						break;
					case "mail_out_port":
						if (acct.IDDomain == 0)
						{
							acct.MailOutgoingPort = reader.GetInt32(index);
						}
						break;
					case "mail_out_auth":
						if (acct.IDDomain == 0)
						{
							acct.MailOutgoingAuthentication = reader.GetBoolean(index);
						}
						break;
                    case "friendly_nm":
                        acct.FriendlyName = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
                        break;
                    case "use_friendly_nm":
                        acct.UseFriendlyName = reader.GetBoolean(index);
                        break;
                    case "def_order":
						acct.DefaultOrder = (DefaultOrder)reader.GetByte(index);
						break;
					case "getmail_at_login":
						acct.GetMailAtLogin = reader.GetBoolean(index);
						break;
					case "mail_mode":
						acct.MailMode = (MailMode)reader.GetByte(index);
						break;
					case "mails_on_server_days":
						acct.MailsOnServerDays = reader.GetInt16(index);
						break;
					case "signature":
                        acct.Signature = Utils.ConvertFromDBString(_account, Convert.ToString(reader.GetValue(index)));
						break;
					case "signature_type":
						acct.SignatureType = (SignatureType)reader.GetByte(index);
						break;
					case "signature_opt":
						acct.SignatureOptions = (SignatureOptions)reader.GetByte(index);
						break;
					case "delimiter":
						acct.Delimiter = reader.GetString(index);
						break;
					case "mailbox_size":
						//different from ms sql
						acct.MailboxSize = Convert.ToInt64(reader.GetDecimal(index));
						break;
					case "mailing_list":
						acct.MailingList = reader.GetBoolean(index);
						break;
					case "domain_mail_protocol":
						if (acct.IDDomain != 0)
						{
							acct.MailIncomingProtocol = (IncomingMailProtocol)reader.GetInt16(index);
						}
						break;
					case "domain_mail_inc_host":
						if (acct.IDDomain != 0)
						{
							acct.MailIncomingHost = reader.GetString(index);
						}
						break;
					case "domain_mail_inc_port":
						if (acct.IDDomain != 0)
						{
							acct.MailIncomingPort = reader.GetInt32(index);
						}
						break;
					case "domain_mail_out_host":
						if (acct.IDDomain != 0)
						{
							acct.MailOutgoingHost = reader.GetString(index);
						}
						break;
					case "domain_mail_out_port":
						if (acct.IDDomain != 0)
						{
							acct.MailOutgoingPort = reader.GetInt32(index);
						}
						break;
					case "domain_mail_out_auth":
						if (acct.IDDomain != 0)
						{
							acct.MailOutgoingAuthentication = reader.GetBoolean(index);
						}
						break;
                    case "imap_quota":
                        acct.Imap_quota = reader.GetInt16(index);
                        break;
                    case "namespace":
                        acct.Namespace = reader.GetString(index);
                        break;
                }
			}
			return acct;
		}
        
        #region Domains Functions

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
                            dom.MailIncomingProtocol = (IncomingMailProtocol)reader.GetInt16(index);
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
                            dom.MailOutgoingAuthentication = reader.GetBoolean(index);
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
                            dom.Settings_mail_out_auth = reader.GetBoolean(index);
                            break;
                        case "allow_direct_mode":
                            dom.Allow_direct_mode = reader.GetBoolean(index);
                            break;
                        case "direct_mode_id_def":
                            dom.Direct_mode_id_def = reader.GetBoolean(index);
                            break;
                        case "attachment_size_limit":
                            dom.Attachment_size_limit = reader.GetInt32(index);
                            break;
                        case "allow_attachment_limit":
                            dom.Allow_attachment_limit = reader.GetBoolean(index);
                            break;
                        case "mailbox_size_limit":
                            dom.Mailbox_size_limit = reader.GetInt32(index);
                            break;
                        case "allow_mailbox_limit":
                            dom.Allow_mailbox_limit = reader.GetBoolean(index);
                            break;
                        case "take_quota":
                            dom.Take_quota = reader.GetBoolean(index);
                            break;
                        case "allow_new_users_change_settings":
                            dom.Allow_new_users_change_settings = reader.GetBoolean(index);
                            break;
                        case "allow_auto_reg_on_login":
                            dom.Allow_auto_reg_on_login = reader.GetBoolean(index);
                            break;
                        case "allow_users_add_accounts":
                            dom.Allow_users_add_accounts = reader.GetBoolean(index);
                            break;
                        case "allow_users_change_account_def":
                            dom.Allow_users_change_account_def = reader.GetBoolean(index);
                            break;
                        case "def_user_charset":
                            dom.Def_user_charset = reader.GetInt32(index);
                            break;
                        case "allow_users_change_charset":
                            dom.Allow_users_change_charset = reader.GetBoolean(index);
                            break;
                        case "def_user_timezone":
                            dom.Def_user_timezone = reader.GetInt32(index);
                            break;
                        case "allow_users_change_timezone":
                            dom.Allow_users_change_timezone = reader.GetBoolean(index);
                            break;
                        case "msgs_per_page":
                            dom.Msgs_per_page = reader.GetInt16(index);
                            break;
                        case "skin":
                            dom.Skin = reader.GetString(index);
                            break;
                        case "allow_users_change_skin":
                            dom.Allow_users_change_skin = reader.GetBoolean(index);
                            break;
                        case "lang":
                            dom.Lang = reader.GetString(index);
                            break;
                        case "allow_users_change_lang":
                            dom.Allow_users_change_lang = reader.GetBoolean(index);
                            break;
                        case "show_text_labels":
                            dom.Show_text_labels = reader.GetBoolean(index);
                            break;
                        case "allow_ajax":
                            dom.Allow_ajax = reader.GetBoolean(index);
                            break;
                        case "allow_editor":
                            dom.Allow_editor = reader.GetBoolean(index);
                            break;
                        case "allow_contacts":
                            dom.Allow_contacts = reader.GetBoolean(index);
                            break;
                        case "allow_calendar":
                            dom.Allow_calendar = reader.GetBoolean(index);
                            break;
                        case "hide_login_mode":
                            dom.Hide_login_mode = (LoginMode)reader.GetInt16(index);
                            break;
                        case "domain_to_use":
                            dom.Domain_to_use = reader.GetString(index);
                            break;
                        case "allow_choosing_lang":
                            dom.Allow_choosing_lang = reader.GetBoolean(index);
                            break;
                        case "allow_advanced_login":
                            dom.Allow_advanced_login = reader.GetBoolean(index);
                            break;
                        case "allow_auto_detect_and_correct":
                            dom.Allow_auto_detect_and_correct = reader.GetBoolean(index);
                            break;
                        case "global_addr_book":
                            dom.Global_addr_book = reader.GetBoolean(index);
                            break;
                        case "view_mode":
                            dom.ViewMode = (ViewMode)reader.GetByte(index);
                            break;
                    }
                }
            }
            return dom;
        }

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
                Log.WriteException(ex);
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
                      bool allow_auto_detect_and_correct, bool global_addr_book, ViewMode viewmode, SaveMail save_mail)
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
                      allow_auto_detect_and_correct, global_addr_book, (short)viewmode, (short)save_mail);
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
                Log.WriteException(ex);
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
                Log.WriteException(ex);
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
                Log.WriteException(ex);
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
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
                throw new WebMailDatabaseException(ex);
            }
        }

        public override Domain SelectDomainDataByUrl(string url)
        {
            Domain dom = null;
            IDbTransaction myTrans = null;
            try
            {
                myTrans = _connection.BeginTransaction();

                IDbCommand command = _commandCreator.SelectAwmDomainUrl(url);
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
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
                throw new WebMailDatabaseException(ex);
            }
        }

        public override DomainCollection SelectDomains(int page, int pageSize, string orderBy, bool asc, string searchCondition)
        {
            DomainCollection domains = new DomainCollection();
            IDbTransaction myTrans = null;
            try
            {
                myTrans = _connection.BeginTransaction();

                IDbCommand command = _commandCreator.SelectAwmDomains(page, pageSize, orderBy, asc, searchCondition);
                command.Transaction = myTrans;
                using (IDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Domain dom = ReadDomain(dataReader);
                        domains.Add(dom);
                    }
                }
                myTrans.Commit();

                return domains;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
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
                      bool allow_auto_detect_and_correct, bool global_addr_book, ViewMode viewmode, SaveMail save_mail)
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
                      allow_auto_detect_and_correct, global_addr_book, (short)viewmode, (short) save_mail);
                command.Transaction = myTrans;
                command.ExecuteNonQuery();
                myTrans.Commit();
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
                throw new WebMailDatabaseException(ex);
            }
        }

        #endregion

        public override int CreateSubadmin(string login, string password, string description, List<string> domains)
        {
            int result = 0;
            IDbTransaction myTrans = null;
            try
            {
                myTrans = _connection.BeginTransaction();
                IDbCommand command = _commandCreator.InsertIntoAwmSubadmins(login, password, description);
                command.Transaction = myTrans;
                command.ExecuteNonQuery();

                command.CommandText = _commandCreator.SelectIdentity();
                object obj = ExecuteScalarCommand(command);
                if (obj != null)
                {
                    result = Convert.ToInt32(obj);
                }

                foreach (string domain in domains)
                {
                    command = _commandCreator.InsertIntoAwmSubadminDomains(result, int.Parse(domain));
                    command.ExecuteNonQuery();
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

	}

}