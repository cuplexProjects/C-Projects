using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using MailBee.ImapMail;
using MailBee.Mime;

namespace WebMail
{
	/// <summary>
	/// Summary description for DatabaseManager.
	/// </summary>
	public abstract class DbManager : IDisposable
	{
		protected bool _isConnected;
		protected int _connectionCount;
		protected bool _disposed;
		protected Account _account;
		protected IDbConnection _connection;
		protected CommandCreator _commandCreator;
		protected string _dataFolder = string.Empty;
		//protected IDbTransaction _transaction;

		public bool IsConnected
		{
			get { return _isConnected; }
		}

		public int ConnectionCount
		{
			get { return _connectionCount; }
		}

		public Account DbAccount
		{
			get { return _account; }
			set { _account = value; }
		}

		public DbManager()
		{
			_connectionCount = 0;
			_isConnected = false;
			_disposed = false;
			_account = null;
			_connection = null;
			_commandCreator = null;
		}

		public DbManager(Account acct) : this()
		{
			_account = acct;
		}

		public virtual string CreateConnectionStringFromSettings(string dataFolder)
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

			string dsn = settings.DbDsn;
			if (!settings.UseDSN) dsn = string.Empty;

			return CreateConnectionString(settings.UseCustomConnectionString,
				settings.DbCustomConnectionString,
				dsn,
				settings.DbType,
				settings.DbPathToMdb,
				settings.DbLogin,
				settings.DbPassword,
				settings.DbName,
				settings.DbHost);
		}

		public virtual string CreateConnectionStringFromSettings()
		{
			return CreateConnectionStringFromSettings(string.Empty);
		}

		public static string CreateConnectionString(bool UseCustomConnectionString, string DbCustomConnectionString, string DbDsn, SupportedDatabase DbType, string DbPathToMdb, string DbLogin, string DbPassword, string DbName, string DbHost)
		{
			if (UseCustomConnectionString)
			{
				return DbCustomConnectionString;
			}

			if ((DbType != SupportedDatabase.MsAccess) && !string.IsNullOrEmpty(DbDsn.Trim()))
			{
				return string.Format("DSN={0}", DbDsn);
			}

			switch (DbType)
			{
				case SupportedDatabase.MsAccess:
				{
					return string.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};User Id=admin;Password=;", DbPathToMdb);
				}
				case SupportedDatabase.MsSqlServer:
				{
                    if (DbName != null)
                    {
                        return string.Format(@"Persist Security Info=False;User ID={0};Password={1};Initial Catalog={2};Data Source={3};Packet Size=4096;", DbLogin, DbPassword, DbName, DbHost);
                    }
                    else
                    {
                        return string.Format(@"Persist Security Info=False;User ID={0};Password={1};Data Source={2};Packet Size=4096;", DbLogin, DbPassword, DbHost);
                    }
				}
				case SupportedDatabase.MySql:
				{
                    if (DbName != null)
                    {
                        return string.Format(@"Driver={{MySQL ODBC 3.51 Driver}};Server={0};Port=3306;Database={1};User={2};Password={3};Option=3;", DbHost, DbName, DbLogin, DbPassword);
                    }
                    else
                    {
                        return string.Format(@"Driver={{MySQL ODBC 3.51 Driver}};Server={0};Port=3306;User={1};Password={2};Option=3;", DbHost, DbLogin, DbPassword);
                    }
				}
                case SupportedDatabase.PostgreSql:
                {
                    if (DbName != null)
                    {
                        return string.Format(@"Driver={{PostgreSQL UNICODE}};Server={0};Port=5432;Database={1};Uid={2};Pwd={3};", DbHost, DbName, DbLogin, DbName, DbPassword);
                    }
                    else
                    {
                        return string.Format(@"Driver={{PostgreSQL UNICODE}};Server={0};Port=5432;Uid={1};Pwd={2};", DbHost, DbLogin, DbPassword);
                    }
                }
				default:
					return string.Empty;
			}
		}

		public virtual void Connect()
		{
			if (_dataFolder != string.Empty)
			{
				Connect(CreateConnectionStringFromSettings(_dataFolder));
			}
			else
			{
				Connect(CreateConnectionStringFromSettings());
			}
		}

		public virtual void Connect(string connectionString)
		{
			/*OdbcConnection conn = new OdbcConnection();
			conn.ConnectionString = "DSN=wm43";
			conn.Open();
			conn.Close();*/
			if (!_isConnected)
			{
				try
				{
					_connection.ConnectionString = connectionString;
					_connection.Open();
					//_transaction = _connection.BeginTransaction();
				}
				catch (Exception ex)
				{
                    Log.WriteException(ex);
                    throw new WebMailDatabaseException(ex);
				}
				_isConnected = true;
				_disposed = false;
			}
			_connectionCount++;
		}

		public virtual void Disconnect()
		{
			_connectionCount--;
			if (_connectionCount <= 0)
			{
				Dispose();
			}
		}

		#region Account and Users Functions
		/// <summary>
		/// Create Account in database, replace existing _account
		/// </summary>
        public virtual Account CreateAccount(int id_user, bool def_acct, bool deleted, string email, IncomingMailProtocol mail_protocol, string mail_inc_host, string mail_inc_login, string mail_inc_pass, int mail_inc_port, string mail_out_host, string mail_out_login, string mail_out_pass, int mail_out_port, bool mail_out_auth, string friendly_nm, bool use_friendly_nm, DefaultOrder def_order, bool getmail_at_login, MailMode mail_mode, short mails_on_server_days, string signature, SignatureType signature_type, SignatureOptions signature_opt, string delimiter, long mailbox_size, string Namespace)
        {
            return CreateAccount(id_user, def_acct, deleted, email, mail_protocol, mail_inc_host, mail_inc_login, mail_inc_pass, mail_inc_port, mail_out_host, mail_out_login, mail_out_pass, mail_out_port, mail_out_auth, friendly_nm, use_friendly_nm, def_order, getmail_at_login, mail_mode, mails_on_server_days, signature, signature_type, signature_opt, delimiter, mailbox_size, 0, false, -1, Namespace);
        }

        public virtual Account CreateAccount(int id_user, bool def_acct, bool deleted, string email, IncomingMailProtocol mail_protocol, string mail_inc_host, string mail_inc_login, string mail_inc_pass, int mail_inc_port, string mail_out_host, string mail_out_login, string mail_out_pass, int mail_out_port, bool mail_out_auth, string friendly_nm, bool use_friendly_nm, DefaultOrder def_order, bool getmail_at_login, MailMode mail_mode, short mails_on_server_days, string signature, SignatureType signature_type, SignatureOptions signature_opt, string delimiter, long mailbox_size, int id_domain, bool mailing_list, int imap_quota, string Namespace)
        {
            IDbTransaction myTrans = null;
            try
            {
                myTrans = _connection.BeginTransaction();

                IDbCommand command = _commandCreator.InsertIntoAwmAccounts(id_user, def_acct, deleted, email, mail_protocol, mail_inc_host, mail_inc_login, Utils.EncodePassword(mail_inc_pass), mail_inc_port, mail_out_host, mail_out_login, Utils.EncodePassword(mail_out_pass), mail_out_port, mail_out_auth, friendly_nm, use_friendly_nm, def_order, getmail_at_login, mail_mode, mails_on_server_days, signature, signature_type, signature_opt, delimiter, mailbox_size, id_domain, mailing_list, imap_quota, Namespace);
                command.Transaction = myTrans;
                object obj = ExecuteScalarCommand(command);
                if (obj != null)
                {
                    myTrans.Commit();
                    return new Account(Convert.ToInt32(obj), def_acct, deleted, email, mail_protocol, mail_inc_host, mail_inc_login, mail_inc_pass, mail_inc_port, mail_out_host, mail_out_login, mail_out_pass, mail_out_port, mail_out_auth, friendly_nm, use_friendly_nm, def_order, getmail_at_login, mail_mode, mails_on_server_days, signature, signature_type, signature_opt, delimiter, mailbox_size, id_domain, mailing_list, imap_quota, Namespace);
                }
            	throw new WebMailDatabaseException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("CantCreateAccount"));
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
                throw new WebMailDatabaseException(ex);
            }
        }


		public virtual User CreateUser(bool deleted)
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
				object obj = ExecuteScalarCommand(command);
				
				if (obj != null)
				{
					result.ID = Convert.ToInt32(obj);
				}
				else
				{
					throw new WebMailDatabaseException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("CantCreateUser"));
				}

				command = _commandCreator.InsertIntoAwmSettings(result.ID, settings.MsgsPerPage, settings.WhiteListing, settings.XSpam, settings.LastLogin, settings.LoginsCount, settings.DefaultSkin, settings.DefaultLanguage, settings.DefaultCharsetInc, settings.DefaultTimeZone, settings.DefaultDateFormat, settings.HideFolders, settings.MailboxLimit, settings.AllowChangeSettings, settings.AllowDhtmlEditor, settings.AllowDirectMode, false, settings.DbCharset, settings.HorizResizer, settings.VertResizer, settings.Mark, settings.Reply, settings.ContactsPerPage, settings.DefaultCharsetOut, (byte)settings.ViewMode);
				command.Transaction = myTrans;
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

		public virtual void DeleteAccount(int id)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.DeleteFromAwmAccounts(id);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				command = _commandCreator.DeleteFromAwmFilters(id, -1, -1);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				command = _commandCreator.DeleteFromAwmFoldersTree(id);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				command = _commandCreator.DeleteFromAwmFolders(id);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				command = _commandCreator.DeleteFromAwmMessages(id);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				command = _commandCreator.DeleteFromAwmMessagesBody(id);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				command = _commandCreator.DeleteFromAwmReads(id);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				myTrans.Commit();
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public virtual int GetNotDefaultAccountCount(string email, string login, string password)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.SelectAwmAccountsNonDefaultCount(email, login, Utils.EncodePassword(password));
				command.Transaction = myTrans;
				object obj = ExecuteScalarCommand(command);
				myTrans.Commit();

				if (obj != null)
				{
					return Convert.ToInt32(obj);
				}
				return 0;
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

        public virtual int GetAccountsCount(string condition)
        {
            return GetAccountsCount(condition, 0);
        }

        public virtual int GetAccountsCount(string condition, int id_domain)
        {
            IDbTransaction myTrans = null;
            try
            {
                myTrans = _connection.BeginTransaction();

                IDbCommand command = _commandCreator.SelectAwmAccountsCount(condition, id_domain);
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

        public virtual int GetAccountsCountNotDel(string condition, int id_domain)
        {
            IDbTransaction myTrans = null;
            try
            {
                myTrans = _connection.BeginTransaction();

                IDbCommand command = _commandCreator.SelectAwmAccountsCountNotDel(condition, id_domain);
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

        public virtual int GetUsersCount()
        {
            IDbTransaction myTrans = null;
            try
            {
                myTrans = _connection.BeginTransaction();

                IDbCommand command = _commandCreator.SelectAwmUsersCount();
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

        public virtual int GetUsersCountNotDel()
        {
            IDbTransaction myTrans = null;
            try
            {
                myTrans = _connection.BeginTransaction();

                IDbCommand command = _commandCreator.SelectAwmUsersCountNotDel();
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

        public virtual int GetAllUsersCount()
        {
            IDbTransaction myTrans = null;
            try
            {
                myTrans = _connection.BeginTransaction();

                IDbCommand command = _commandCreator.SelectAwmUsersCountAll();
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
        
        public virtual void MarkUserAsDeleted(int id)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.UpdateAUser(id, true);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();
				myTrans.Commit();
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

        public virtual void DeleteSettingsData(int id)
        {
            IDbTransaction myTrans = null;
            try
            {
                myTrans = _connection.BeginTransaction();

                IDbCommand command = _commandCreator.DeleteFromAwmSettings(id);
                command.Transaction = myTrans;
                command.ExecuteNonQuery();
                
                command = _commandCreator.DeleteFromAuser(id);
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

		public virtual void DeleteUser(int id)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.DeleteFromAUsers(id);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				command = _commandCreator.DeleteFromAwmSettings(id);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				command = _commandCreator.DeleteFromAwmColumns(-1, id);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				command = _commandCreator.DeleteFromAwmSenders(id);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				command = _commandCreator.DeleteFromAwmAddrGroupContactsAll(id);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				command = _commandCreator.DeleteFromAwmAddrBook(id);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				command = _commandCreator.DeleteFromAwmAddrGroups(id);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				myTrans.Commit();
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}


		public virtual void UpdateAccountDefOrder(Account acct, int def_order)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();
				IDbCommand command = _commandCreator.UpdateAwmAccountsDefOrder(acct.ID, def_order);
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

		public virtual void UpdateAccount(Account acct, bool updateUser)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

                IDbCommand command = _commandCreator.UpdateAwmAccounts(acct.ID, (acct.UserOfAccount != null) ? acct.UserOfAccount.ID : -1, acct.DefaultAccount, acct.Deleted, acct.Email, acct.MailIncomingProtocol, acct.MailIncomingHost, acct.MailIncomingLogin, Utils.EncodePassword(acct.MailIncomingPassword), acct.MailIncomingPort, acct.MailOutgoingHost, acct.MailOutgoingLogin, Utils.EncodePassword(acct.MailOutgoingPassword), acct.MailOutgoingPort, acct.MailOutgoingAuthentication, Utils.ConvertToDBString(acct, acct.FriendlyName), acct.UseFriendlyName, acct.DefaultOrder, acct.GetMailAtLogin, acct.MailMode, acct.MailsOnServerDays, Utils.ConvertToDBString(acct, acct.Signature), acct.SignatureType, acct.SignatureOptions, acct.Delimiter, acct.MailboxSize, acct.Imap_quota, acct.Namespace);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				if (updateUser)
				{
					if (acct.UserOfAccount != null)
					{
						command = _commandCreator.UpdateAUser(acct.UserOfAccount.ID, acct.UserOfAccount.Deleted);
						command.Transaction = myTrans;
						command.ExecuteNonQuery();

						if (acct.UserOfAccount.Settings != null)
						{
							UserSettings settings = acct.UserOfAccount.Settings;
                            command = _commandCreator.UpdateAwmSettings(settings.ID, settings.IDUser, settings.MsgsPerPage, settings.WhiteListing, settings.XSpam, settings.LastLogin, settings.LoginsCount, settings.DefaultSkin, settings.DefaultLanguage, settings.DefaultCharsetInc, settings.DefaultTimeZone, settings.DefaultDateFormat, settings.HideFolders, settings.MailboxLimit, settings.AllowChangeSettings, settings.AllowDhtmlEditor, settings.AllowDirectMode, false, settings.DbCharset, settings.HorizResizer, settings.VertResizer, settings.Mark, settings.Reply, settings.ContactsPerPage, settings.DefaultCharsetOut, (byte)settings.ViewMode, settings.DefaultTimeFormat, settings.AutoCheckmailInterval);
							command.Transaction = myTrans;
							command.ExecuteNonQuery();
						}
						if (acct.UserOfAccount.Columns != null)
						{
							ArrayList list = new ArrayList();
							command = _commandCreator.SelectAwmColumns(acct.IDUser);
							command.Transaction = myTrans;
							using (IDataReader reader = command.ExecuteReader())
							{
								while (reader.Read())
								{
									UserColumn column = ReadUserColumn(reader);
									if (column != null)
									{
										list.Add(column);
									}
								}
							}
							UserColumn[] existingColumns = (UserColumn[])list.ToArray(typeof(UserColumn));
							bool needToInsert = false;
							foreach (UserColumn column in acct.UserOfAccount.Columns)
							{
								if (existingColumns.Length==0) needToInsert = true;
								bool needToUpdate = false;
								foreach (UserColumn exst in existingColumns)
								{
									if (column.IDColumn == exst.IDColumn)
									{
										needToInsert = false;
										if (column.Value != exst.Value)
										{
											needToUpdate = true;
										}
									}
								}
								if (needToInsert)
								{
									command = _commandCreator.InsertIntoAwmColumns(column.IDColumn, column.IDUser, column.Value);
									command.Transaction = myTrans;
									command.ExecuteNonQuery();
									command.CommandText = _commandCreator.SelectIdentity();
									object obj = ExecuteScalarCommand(command);
								}
								if (needToUpdate)
								{
									command = _commandCreator.UpdateAwmColumns(column.IDColumn, column.IDUser, column.Value);
									command.Transaction = myTrans;
									command.ExecuteNonQuery();
								}
							}
						}
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

		public virtual void UpdateUser(User usr)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				if (usr != null)
				{
					IDbCommand command = _commandCreator.UpdateAUser(usr.ID, usr.Deleted);
					command.Transaction = myTrans;
					command.ExecuteNonQuery();

					if (usr.Settings != null)
					{
						UserSettings settings = usr.Settings;
						command = _commandCreator.UpdateAwmSettings(settings.ID, settings.IDUser, settings.MsgsPerPage, settings.WhiteListing, settings.XSpam, settings.LastLogin, settings.LoginsCount, settings.DefaultSkin, settings.DefaultLanguage, settings.DefaultCharsetInc, settings.DefaultTimeZone, settings.DefaultDateFormat, settings.HideFolders, settings.MailboxLimit, settings.AllowChangeSettings, settings.AllowDhtmlEditor, settings.AllowDirectMode, false, settings.DbCharset, settings.HorizResizer, settings.VertResizer, settings.Mark, settings.Reply, settings.ContactsPerPage, settings.DefaultCharsetOut, (byte)settings.ViewMode, settings.DefaultTimeFormat, settings.AutoCheckmailInterval);
						command.Transaction = myTrans;
						command.ExecuteNonQuery();
					}
					if (usr.Columns != null)
					{
						ArrayList list = new ArrayList();
						command = _commandCreator.SelectAwmColumns(usr.ID);
						command.Transaction = myTrans;
						using (IDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								UserColumn column = ReadUserColumn(reader);
								if (column != null)
								{
									list.Add(column);
								}
							}
						}
						UserColumn[] existingColumns = (UserColumn[])list.ToArray(typeof(UserColumn));
						bool neenToUpdate = false, needToInsert = false;
						foreach (UserColumn column in usr.Columns)
						{
							foreach (UserColumn exst in existingColumns)
							{
                                if ((column.IDColumn == exst.IDColumn) && (column.Value == exst.Value))
                                {
                                    neenToUpdate = false;
                                    needToInsert = false;
                                    break;
                                }
                                else if ((column.IDColumn == exst.IDColumn) && (column.Value != exst.Value))
                                {
                                    neenToUpdate = true;
                                    needToInsert = false;
                                    break;
                                }
                                else
                                    needToInsert = true;
							}
							if (needToInsert)
							{
								command = _commandCreator.InsertIntoAwmColumns(column.IDColumn, column.IDUser, column.Value);
								command.Transaction = myTrans;
								command.ExecuteNonQuery();
							}
							if (neenToUpdate)
							{
								command = _commandCreator.UpdateAwmColumns(column.IDColumn, column.IDUser, column.Value);
								command.Transaction = myTrans;
								command.ExecuteNonQuery();
							}
						}
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

		public virtual User SelectUser(int id)
		{
			User usr = null;
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.SelectAUsersAndAwmSettings(id);
				command.Transaction = myTrans;
				using (IDataReader dataReader = command.ExecuteReader())
				{
					if (dataReader.Read())
					{
						usr = ReadUser(dataReader);
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
			if (usr != null)
			{
				usr.Columns = SelectUserColumns(id);
			}
			return usr;
		}

        public virtual AccountCollection SelectAccountsAdminReader(int page, int pageSize, string orderBy, bool asc, string searchCondition, int id_domain)
        {
            AccountCollection acctColl = new AccountCollection();
            try
            {
                IDbCommand command = _commandCreator.SelectAwmAccountsForAdmin(page, pageSize, orderBy, asc, searchCondition, id_domain);
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Account acct = ReadAccount(reader);
                        if (acct != null)
                        {
                            acctColl.Add(acct);
                        }
                    }
                }

                foreach (Account acct in acctColl)
                {
                    acct.UserOfAccount = SelectUser(acct.IDUser);
                }
                return acctColl;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                throw new WebMailDatabaseException(ex);
            }
        }

        public virtual SubadminCollection SelectSubadmins(int page, int pageSize, string orderBy, bool asc, string searchCondition)
        {
            IDbTransaction myTrans = null;
            SubadminCollection admCollection = new SubadminCollection();
            try
            {
                myTrans = _connection.BeginTransaction();

                IDbCommand command = _commandCreator.SelectAwmSubadmins(page, pageSize, orderBy, asc, searchCondition);
                command.Transaction = myTrans;
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Subadmin adm = ReadSubadmin(reader);
                        admCollection.Add(adm);
                    }
                }
                myTrans.Commit();
                return admCollection;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
                throw new WebMailDatabaseException(ex);
            }
        }

        public virtual Subadmin SelectSubadmin(int id_admin)
        {
            Subadmin adm = null;
            IDbTransaction myTrans = null;
            try
            {
                myTrans = _connection.BeginTransaction();

                IDbCommand command = _commandCreator.SelectAwmSubadmins(id_admin);
                command.Transaction = myTrans;
                using (IDataReader dataReader = command.ExecuteReader())
                {
                    if (dataReader.Read())
                    {
                        adm = ReadSubadmin(dataReader);
                    }
                }
                myTrans.Commit();
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
                throw new WebMailDatabaseException(ex);
            }
            return adm;
        }

        public virtual Subadmin SelectSubadmin(string login)
        {
            Subadmin adm = null;
            IDbTransaction myTrans = null;
            try
            {
                myTrans = _connection.BeginTransaction();

                IDbCommand command = _commandCreator.SelectAwmSubadmins(login);
                command.Transaction = myTrans;
                using (IDataReader dataReader = command.ExecuteReader())
                {
                    if (dataReader.Read())
                    {
                        adm = ReadSubadmin(dataReader);
                    }
                }
                myTrans.Commit();
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
                throw new WebMailDatabaseException(ex);
            }
            return adm;
        }

        public virtual List<int> SelectSubadminDomains(int id_admin)
        {
            List<int> list = new List<int>();
            IDbCommand command = _commandCreator.SelectAwmSubadminDomains(id_admin);
            IDataAdapter adapter = CreateDataAdapter(this, command);
            DataSet ds = new DataSet();
            adapter.Fill(ds);

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(int.Parse(row["id_domain"].ToString()));
                }
            }
            return list;
        }

        public virtual DataTable SelectSubadminDomains(string login)
        {
            IDbCommand command = _commandCreator.SelectAwmSubadminDomains(login);
            IDataAdapter adapter = CreateDataAdapter(this, command);
            DataSet ds = new DataSet();
            adapter.Fill(ds);

            return ds.Tables[0];
        }

        public virtual List<int> SelectListSubadminDomains(string login)
        {
            List<int> result = new List<int>();

            DataTable dt = SelectSubadminDomains(login);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    result.Add(int.Parse(row["id_domain"].ToString()));
                }
            }
            return result;
        }

        public virtual int GetSubadminCount(string condition)
        {
            int result;
            IDbTransaction myTrans = null;
            try
            {
                myTrans = _connection.BeginTransaction();

                IDbCommand command = _commandCreator.SelectAwmSubadminsCount(condition);
                command.Transaction = myTrans;
                object obj = ExecuteScalarCommand(command);
                myTrans.Commit();
                result = (obj != null) ? Convert.ToInt32(obj) : -1;
            }
            catch (Exception ex)
            {
                if (myTrans != null) myTrans.Rollback();
                throw new WebMailDatabaseException(ex);
            }
            return result;
        }

        public virtual int GetSubadminCountByLogin(string login)
        {
            int result;
            IDbTransaction myTrans = null;
            try
            {
                myTrans = _connection.BeginTransaction();

                IDbCommand command = _commandCreator.SelectAwmSubadminsCountByLogin(login);
                command.Transaction = myTrans;
                object obj = ExecuteScalarCommand(command);
                myTrans.Commit();
                result = (obj != null) ? Convert.ToInt32(obj) : -1;
            }
            catch (Exception ex)
            {
                if (myTrans != null) myTrans.Rollback();
                throw new WebMailDatabaseException(ex);
            }
            return result;
        }

        public virtual long GetUserMailboxsSize(int id_user)
		{
			long result;
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.SelectMailboxsSize(id_user);
				command.Transaction = myTrans;
				object obj = ExecuteScalarCommand(command);
				myTrans.Commit();
				result = (obj != null) ? Convert.ToInt64(obj) : -1;
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
			return result;
		}

		/* !!! takes accounts with mailing_list=0 and mailing_list=1 */
		public virtual IDataReader SelectUserAccountsAdminReader(int id_user)
		{
			IDbCommand command = _commandCreator.SelectAwmAccounts(id_user, -1, true);
			return command.ExecuteReader();
		}

		/* !!! takes accounts with mailing_list=0 */
		public virtual AccountCollection SelectUserAccounts(int id_user)
		{
			AccountCollection accounts = new AccountCollection();
			User u = SelectUser(id_user);
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				if (u != null)
				{
					IDbCommand command = _commandCreator.SelectAwmAccounts(id_user, -1, false);
					command.Transaction = myTrans;
					using (IDataReader dataReader = command.ExecuteReader())
					{
						while (dataReader.Read())
						{
							Account acct = ReadAccount(dataReader);
							acct.UserOfAccount = u;
							accounts.Add(acct);
						}
					}
				}
				myTrans.Commit();

				return accounts;
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

        /* !!! takes accounts with mailing_list=0 */
        public virtual AccountCollection SelectDomainAccounts(int id_domain)
        {
            AccountCollection accounts = new AccountCollection();
            IDbTransaction myTrans = null;
            try
            {
                myTrans = _connection.BeginTransaction();

                IDbCommand command = _commandCreator.SelectAwmAccounts(id_domain);
                command.Transaction = myTrans;
                using (IDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Account acct = ReadAccount(dataReader);
                        accounts.Add(acct);
                    }
                }
                myTrans.Commit();

                return accounts;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
                throw new WebMailDatabaseException(ex);
            }
        }

		/* !!! takes account with mailing_list=0 */
		public virtual Account SelectAccountData(string email, string login, string password)
		{
			Account acct = null;
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.SelectAwmAccounts(email, login, Utils.EncodePassword(password), false);
				command.Transaction = myTrans;
				using (IDataReader dataReader = command.ExecuteReader())
				{
					if (dataReader.Read())
					{
						acct = ReadAccount(dataReader);
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
			if (acct != null)
			{
				acct.UserOfAccount = SelectUser(acct.IDUser);
			}
			return acct;
		}

		public virtual Account SelectAccountData(int id_acct, int id_user, bool with_mailing_lists)
		{
			Account acct = null;
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.SelectAwmAccounts(id_user, id_acct, with_mailing_lists);
				command.Transaction = myTrans;
				using (IDataReader dataReader = command.ExecuteReader())
				{
					if (dataReader.Read())
					{
						acct = ReadAccount(dataReader);
					}
				}
				myTrans.Commit();
				if (acct != null)
				{
					acct.UserOfAccount = SelectUser(acct.IDUser);
				}
				return acct;
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

        public virtual void MarkUsersAsDeletedByLicences(int licences_num)
        {
            IDbTransaction myTrans = null;
            try
            {
                myTrans = _connection.BeginTransaction();
                if (licences_num > 0)
                {
                    IDbCommand command = _commandCreator.UpdateAUsersAsDeleted(true);
                    command.Transaction = myTrans;
                    command.ExecuteNonQuery();

                    command = _commandCreator.UpdateAUsersByLicences(licences_num);
                    command.Transaction = myTrans;
                    command.ExecuteNonQuery();
                }
                else if (licences_num == 0)
                {
                    IDbCommand command = _commandCreator.UpdateAUsersAsDeleted(false);
                    command.Transaction = myTrans;
                    command.ExecuteNonQuery();
                }
                else
                {
                    IDbCommand command = _commandCreator.UpdateAUsersAsDeleted(true);
                    command.Transaction = myTrans;
                    command.ExecuteNonQuery();
                }
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

		#region Address Book Functions
		public virtual AddressBookContact CreateAddressBookContact(AddressBookContact contactToCreate)
		{
			if ((contactToCreate.HEmail.Trim().Length == 0)
				&& (contactToCreate.BEmail.Trim().Length == 0)
				&& (contactToCreate.OtherEmail.Trim().Length == 0)
				&& (contactToCreate.FullName.Trim().Length == 0))
			{
				WebmailResourceManager resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
				throw new WebMailException(resMan.GetString("WarningContactNotComplete"));
			}
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
					Utils.ConvertToDBString(_account, contactToCreate.HEmail.Trim()),
					Utils.ConvertToDBString(_account, contactToCreate.FullName.Trim()),
					Utils.ConvertToDBString(_account, contactToCreate.Notes.Trim()),
					contactToCreate.UseFriendlyName,
					Utils.ConvertToDBString(_account, contactToCreate.HStreet.Trim()),
					Utils.ConvertToDBString(_account, contactToCreate.HCity.Trim()),
					Utils.ConvertToDBString(_account, contactToCreate.HState.Trim()),
					Utils.ConvertToDBString(_account, contactToCreate.HZip.Trim()),
					Utils.ConvertToDBString(_account, contactToCreate.HCountry.Trim()),
					Utils.ConvertToDBString(_account, contactToCreate.HPhone.Trim()),
					Utils.ConvertToDBString(_account, contactToCreate.HFax.Trim()),
					Utils.ConvertToDBString(_account, contactToCreate.HMobile.Trim()),
					Utils.ConvertToDBString(_account, contactToCreate.HWeb.Trim()),
					Utils.ConvertToDBString(_account, contactToCreate.BEmail.Trim()),
					Utils.ConvertToDBString(_account, contactToCreate.BCompany.Trim()),
					Utils.ConvertToDBString(_account, contactToCreate.BStreet.Trim()),
					Utils.ConvertToDBString(_account, contactToCreate.BCity.Trim()),
					Utils.ConvertToDBString(_account, contactToCreate.BState.Trim()),
					Utils.ConvertToDBString(_account, contactToCreate.BZip.Trim()),
					Utils.ConvertToDBString(_account, contactToCreate.BCountry.Trim()),
					Utils.ConvertToDBString(_account, contactToCreate.BJobTitle.Trim()),
					Utils.ConvertToDBString(_account, contactToCreate.BDepartment.Trim()),
					Utils.ConvertToDBString(_account, contactToCreate.BOffice.Trim()),
					Utils.ConvertToDBString(_account, contactToCreate.BPhone.Trim()),
					Utils.ConvertToDBString(_account, contactToCreate.BFax.Trim()),
					Utils.ConvertToDBString(_account, contactToCreate.BWeb.Trim()),
					contactToCreate.BirthdayDay, contactToCreate.BirthdayMonth, contactToCreate.BirthdayYear,
					Utils.ConvertToDBString(_account, contactToCreate.OtherEmail.Trim()),
					(short)contactToCreate.PrimaryEmail, contactToCreate.IDAddrPrev, contactToCreate.Tmp,
					contactToCreate.UseFrequency, contactToCreate.AutoCreate,
					Utils.ConvertToDBString(_account, contactToCreate.StrID),
					contactToCreate.DateModified);
				command.Transaction = myTrans;
				object obj = ExecuteScalarCommand(command);

				if (obj != null)
				{
					contactToCreate.IDAddr = Convert.ToInt64(obj);
				}
				else
				{
					throw new WebMailDatabaseException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("PROC_CANT_INS_NEW_CONT"));
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

		public virtual AddressBookGroupContact[] SelectAddressBookContactsGroups(int page, int sort_field, int sort_order, int id_group, string look_for, int look_for_type)
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
										if (this is MsAccessDbManager)
										{
											group_contact.id = reader.GetInt32(index);
										}
										else
										{
											group_contact.id = reader.GetInt64(index);
										}
										break;
									case "name":
										group_contact.fullname = Utils.ConvertFromDBString(_account, reader.GetString(index));
										break;
									case "email":
										group_contact.email = Utils.ConvertFromDBString(_account, reader.GetString(index));
										break;
									case "is_group":
										group_contact.isGroup = (reader.GetInt32(index) == 1) ? true : false;
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
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public virtual AddressBookContact[] SelectAddressBookContacts(string email)
		{
			IDbCommand command = _commandCreator.SelectAwmAddrBookByEmail(_account.IDUser, Utils.ConvertToDBString(_account, email));
			return SelectAddressBookContacts(command);
		}
		
		public virtual AddressBookContact[] SelectAddressBookContacts(long id_addr)
		{
			IDbCommand command = _commandCreator.SelectAwmAddrBook(_account.IDUser, id_addr);
			return SelectAddressBookContacts(command);
		}

		public AddressBookContact[] SelectAddressBookContacts(int user_id, string str_id)
		{
			IDbCommand command = _commandCreator.SelectAwmAddrBookByStrID(user_id, Utils.ConvertToDBString(_account, str_id));//???
			return SelectAddressBookContacts(command);
		}

		protected AddressBookContact[] SelectAddressBookContacts(IDbCommand command)
		{
			ArrayList contacts = new ArrayList();
			//AddressBookContact contact = null;
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				//IDbCommand command = _commandCreator.SelectAwmAddrBook(user_id, str_id);//???
				command.Transaction = myTrans;
				using (IDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						AddressBookContact contact = ReadAddressBookContact(reader);
						if (contact == null) continue;
						contacts.Add(contact);
					}
				}
				foreach (AddressBookContact contact in contacts)
				{
					ArrayList groups = new ArrayList();
					command = _commandCreator.SelectAwmAddrGroups(_account.UserOfAccount.ID, contact.IDAddr);
					command.Transaction = myTrans;
					using (IDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							AddressBookGroup group = ReadAddressBookGroup(reader);
							if (group == null) continue;
							groups.Add(group);
						}
					}
					contact.Groups = (AddressBookGroup[])groups.ToArray(typeof(AddressBookGroup));
				}
				myTrans.Commit();
				return (AddressBookContact[])contacts.ToArray(typeof(AddressBookContact));
			}
			catch (Exception ex)
			{
				Log.WriteException(ex);
				if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public virtual void DeleteAddressBookContacts(AddressBookContact[] contactsToDelete)
		{
			if (contactsToDelete.Length <= 0) return;

			long[] ids = new long[contactsToDelete.Length];
			for (int i = 0; i < contactsToDelete.Length; i++)
			{
				ids[i] = contactsToDelete[i].IDAddr;
			}
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();
				IDbCommand command = _commandCreator.DeleteFromAwmAddrBook(_account.UserOfAccount.ID, ids);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				command = _commandCreator.DeleteFromAwmAddrGroupContacts(ids);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				myTrans.Commit();
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public virtual void UpdateAddressBookContact(AddressBookContact contactToUpdate)
		{
			if ((contactToUpdate.HEmail.Trim().Length == 0)
				&& (contactToUpdate.BEmail.Trim().Length == 0)
				&& (contactToUpdate.OtherEmail.Trim().Length == 0)
				&& (contactToUpdate.FullName.Trim().Length == 0))
			{
				WebmailResourceManager resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
				throw new WebMailException(resMan.GetString("WarningContactNotComplete"));
			}
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.UpdateAwmAddrBook(contactToUpdate.IDAddr, contactToUpdate.IDUser,
					Utils.ConvertToDBString(_account, contactToUpdate.HEmail.Trim()),
					Utils.ConvertToDBString(_account, contactToUpdate.FullName.Trim()),
					Utils.ConvertToDBString(_account, contactToUpdate.Notes.Trim()),
					contactToUpdate.UseFriendlyName,
					Utils.ConvertToDBString(_account, contactToUpdate.HStreet.Trim()),
					Utils.ConvertToDBString(_account,  contactToUpdate.HCity.Trim()),
					Utils.ConvertToDBString(_account, contactToUpdate.HState.Trim()),
					Utils.ConvertToDBString(_account, contactToUpdate.HZip.Trim()),
					Utils.ConvertToDBString(_account, contactToUpdate.HCountry.Trim()),
					Utils.ConvertToDBString(_account, contactToUpdate.HPhone.Trim()),
					Utils.ConvertToDBString(_account, contactToUpdate.HFax.Trim()),
					Utils.ConvertToDBString(_account, contactToUpdate.HMobile.Trim()),
					Utils.ConvertToDBString(_account, contactToUpdate.HWeb.Trim()),
					Utils.ConvertToDBString(_account, contactToUpdate.BEmail.Trim()),
					Utils.ConvertToDBString(_account, contactToUpdate.BCompany.Trim()),
					Utils.ConvertToDBString(_account, contactToUpdate.BStreet.Trim()),
					Utils.ConvertToDBString(_account, contactToUpdate.BCity.Trim()),
					Utils.ConvertToDBString(_account, contactToUpdate.BState.Trim()),
					Utils.ConvertToDBString(_account, contactToUpdate.BZip.Trim()),
					Utils.ConvertToDBString(_account, contactToUpdate.BCountry.Trim()),
					Utils.ConvertToDBString(_account, contactToUpdate.BJobTitle.Trim()),
					Utils.ConvertToDBString(_account, contactToUpdate.BDepartment.Trim()),
					Utils.ConvertToDBString(_account, contactToUpdate.BOffice.Trim()),
					Utils.ConvertToDBString(_account, contactToUpdate.BPhone.Trim()),
					Utils.ConvertToDBString(_account, contactToUpdate.BFax.Trim()),
					Utils.ConvertToDBString(_account, contactToUpdate.BWeb.Trim()),
					contactToUpdate.BirthdayDay, contactToUpdate.BirthdayMonth, contactToUpdate.BirthdayYear,
					Utils.ConvertToDBString(_account, contactToUpdate.OtherEmail.Trim()),
					(short)contactToUpdate.PrimaryEmail, contactToUpdate.IDAddrPrev, contactToUpdate.Tmp,
					contactToUpdate.UseFrequency, contactToUpdate.AutoCreate,
					Utils.ConvertToDBString(_account, contactToUpdate.StrID.Trim()),
					contactToUpdate.DateModified);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				command = _commandCreator.DeleteFromAwmAddrGroupContacts(contactToUpdate.IDAddr);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				if ((contactToUpdate.Groups != null) && (contactToUpdate.Groups.Length > 0))
				{
					for (int i = 0; i < contactToUpdate.Groups.Length; i++)
					{
						command = _commandCreator.InsertIntoAwmAddrGroupsContacts(contactToUpdate.IDAddr, contactToUpdate.Groups[i].IDGroup);
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

		public virtual int GetAddressBookContactsCount(int id_user, string look_for, int look_for_type)
		{
			int result;
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

                IDbCommand command = _commandCreator.SelectAddressBookContactsCount(id_user, Utils.ConvertToDBString(_account, look_for), look_for_type);
				command.Transaction = myTrans;
				object obj = ExecuteScalarCommand(command);
				myTrans.Commit();
				result = (obj != null) ? Convert.ToInt32(obj) : -1;
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
			return result;
		}

		public virtual int GetAddressBookGroupsCount(int id_user, string look_for, int look_for_type)
		{
			int result;
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

                IDbCommand command = _commandCreator.SelectAddressBookGroupsCount(id_user, Utils.ConvertToDBString(_account, look_for), look_for_type);
				command.Transaction = myTrans;
				object obj = ExecuteScalarCommand(command);
				myTrans.Commit();
				result = (obj != null) ? Convert.ToInt32(obj) : -1;
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
			return result;
		}

		public virtual void AddContactsToGroup(int id_group, AddressBookContact[] contacts)
		{
			long[] id_addrs = new long[contacts.Length];
			for (int i = 0; i < contacts.Length; i++)
			{
				id_addrs[i] = contacts[i].IDAddr;
			}
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.DeleteFromAwmAddrGroupContacts(id_group, id_addrs);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				foreach (AddressBookContact contact in contacts)
				{
					command = _commandCreator.InsertIntoAwmAddrGroupsContacts(contact.IDAddr, id_group);
					command.Transaction = myTrans;
					command.ExecuteNonQuery();
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

		public virtual AddressBookGroup CreateAddressBookGroup(AddressBookGroup groupToCreate)
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
				object obj = ExecuteScalarCommand(command);
				if (obj != null)
				{
					groupToCreate.IDGroup = Convert.ToInt32(obj);
				}
				else
				{
					throw new WebMailDatabaseException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("CantCreateContactGroup"));
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

		public virtual AddressBookGroup SelectAddressBookGroup(int id_group)
		{
			AddressBookGroup group = null;
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.SelectAwmAddrGroups(_account.UserOfAccount.ID, id_group);
				command.Transaction = myTrans;
				using (IDataReader reader = command.ExecuteReader())
				{
					if (reader.Read())
					{
						group = ReadAddressBookGroup(reader);
					}
				}
				if (group != null)
				{
					ArrayList contacts = new ArrayList();
					command = _commandCreator.SelectAwmAddrBook(_account.UserOfAccount.ID, id_group);
					command.Transaction = myTrans;
					using (IDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							AddressBookContact contact = ReadAddressBookContact(reader);
							if (contact != null)
							{
								contacts.Add(contact);
							}
						}
					}
					group.Contacts = (AddressBookContact[])contacts.ToArray(typeof(AddressBookContact));
				}
				myTrans.Commit();
				return group;
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public virtual AddressBookGroup[] SelectAddressBookGroups()
		{
			ArrayList groups = new ArrayList();
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.SelectAwmAddrGroups(_account.UserOfAccount.ID);
				command.Transaction = myTrans;
				using (IDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						AddressBookGroup group = ReadAddressBookGroup(reader);
						if (group != null)
						{
							groups.Add(group);
						}
					}
				}
				myTrans.Commit();
				return (AddressBookGroup[])groups.ToArray(typeof(AddressBookGroup));
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public virtual void DeleteAddressBookGroups(AddressBookGroup[] groupsToDelete)
		{
			if (groupsToDelete.Length <= 0) return;

			int[] id_groups = new int[groupsToDelete.Length];
			for (int i = 0; i < groupsToDelete.Length; i++)
			{
				id_groups[i] = groupsToDelete[i].IDGroup;
			}

			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();
				foreach (AddressBookGroup group in groupsToDelete)
				{
					IDbCommand command = _commandCreator.DeleteFromAwmAddrGroups(group.IDUser, group.IDGroup);
					command.Transaction = myTrans;
					command.ExecuteNonQuery();

					command = _commandCreator.DeleteFromAwmAddrGroupContacts(id_groups);
					command.Transaction = myTrans;
					command.ExecuteNonQuery();
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

		public virtual void UpdateAddressBookGroup(AddressBookGroup groupToUpdate)
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
					if (contactToCreate != null)
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
		#endregion

		#region Message Function

		public virtual void SaveMessage(int id_msg, WebMailMessage message, Folder folder)
		{
			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
			if (_account.UserOfAccount != null)
			{
				if (_account.UserOfAccount.Settings != null)
				{
					if (settings.EnableMailboxSizeLimit && _account.UserOfAccount.Settings.MailboxLimit != 0)
					{
						long allSize = GetUserMailboxsSize(_account.IDUser);
						if (_account.UserOfAccount.Settings.MailboxLimit < (allSize + message.Size))
						{
							throw new WebMailMailBoxException((new WebmailResourceManagerCreator().CreateResourceManager().GetString("ErrorGetMailLimit")));
						}
					}
				}
			}

			if (id_msg < 0)
			{
				id_msg = GetMessageID();
				id_msg = Utils.RandMsgID(++id_msg);
			}

			if (string.IsNullOrEmpty(message.BodyText))
			{
				if (message.MailBeeMessage != null)
				{
					if (string.IsNullOrEmpty(message.MailBeeMessage.BodyPlainText))
					{
						message.MailBeeMessage.MakePlainBodyFromHtmlBody();
					}
					if (message.MailBeeMessage.BodyPlainText != null && message.MailBeeMessage != null)
						message.BodyText = message.MailBeeMessage.BodyPlainText.ToLower(CultureInfo.InvariantCulture);
				}
			}

			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

                IDbCommand command = _commandCreator.InsertIntoAwmMessages(id_msg, _account.ID, folder.ID, folder.ID, message.StrUid, message.IntUid, Utils.ConvertToDBString(_account, message.FromMsg.ToString()), Utils.ConvertToDBString(_account, message.ToMsg.ToString()), Utils.ConvertToDBString(_account, message.CcMsg.ToString()), Utils.ConvertToDBString(_account, message.BccMsg.ToString()), Utils.ConvertToDBString(_account, message.Subject), message.MsgDate, message.Attachments, message.Size, message.Seen, message.Flagged, Convert.ToByte(message.Priority), Convert.ToByte(message.Sensitivity), message.Downloaded, message.XSpam, message.Rtl, message.Deleted, message.IsFull, message.Replied, message.Forwarded, Convert.ToByte(message.Flags), Utils.ConvertToDBString(_account, message.BodyText), message.Grayed, message.OverrideCharset);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				if (message.Downloaded)
				{
					if (settings.StoreMailsInDb)
					{
						command = _commandCreator.InsertIntoAwmMessagesBody(_account.ID, id_msg, message.RawMsg);
						command.Transaction = myTrans;
						command.ExecuteNonQuery();
					}
                    else if (_account.MailIncomingProtocol != IncomingMailProtocol.WMServer)
                    {
					    FileSystem fs = new FileSystem(_account.Email, _account.ID, true);
					    MailMessage tempMsg = new MailMessage(); // because we can change message in rebuild
					    tempMsg.LoadMessage(message.RawMsg);
					    fs.SaveMessage(tempMsg, id_msg, folder.GetFullPath(_account.Delimiter));
					}
				}
				_account.MailboxSize += message.Size;
				myTrans.Commit();
				_account.Update(false);
			}
			catch (Exception ex)
			{
				/* If message size exceeds the limit specified in mysql settings,
				 * then its saving falls with DbException
				 * "Lost connection to MySQL server during query".
				 * Closing and opening connect allows more work with databases.
				 * Otherwise in SavePop3Uids function InvalidOperationException
				 * "OdbcConnection does not support parallel transactions".
				 */
				try
				{
					if (myTrans != null) myTrans.Rollback();
				}
				catch
				{
					if (_connection.State != ConnectionState.Closed)
					{
						_connection.Close();
					}
					_connection.Open();
				}
                Log.WriteException(ex);
                throw new WebMailDatabaseException(ex);
			}
		}

		public virtual void DeletePop3Uids()
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.DeleteFromAwmReads(_account.ID);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();
				myTrans.Commit();
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public virtual void SavePop3Uids(object[] uids)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				foreach (string uid in uids)
				{
					IDbCommand command = _commandCreator.InsertIntoAwmReads(_account.ID, Utils.ConvertToDBString(_account, uid));
					command.Transaction = myTrans;
					command.ExecuteNonQuery();
				}
				myTrans.Commit();
			}
			catch (Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}

		}

		public virtual long SelectImapLastMessageUid(Folder fld)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.SelectTop1AwmMessages(_account.ID, fld.ID);
				command.Transaction = myTrans;
				object obj = ExecuteScalarCommand(command);
				myTrans.Commit();
				return (obj != null) ? Convert.ToInt64(obj) : 0;
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public virtual WebMailMessage SelectMessage(int id_msg, Folder fld)
		{
			IDbTransaction myTrans = null;
			WebMailMessage webMsg = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.SelectAwmMessages(_account.ID, id_msg, fld.ID);
				command.Transaction = myTrans;
				using (IDataReader reader = command.ExecuteReader())
				{
					if (reader.Read())
					{
						webMsg = ReadWebMailMessage(reader, fld);
					}
				}
				MailMessage message = LoadMailMessage(/*command, */myTrans, webMsg, fld);
				if (message != null && webMsg != null)
				{
					webMsg.Init(message, (!string.IsNullOrEmpty(webMsg.StrUid)), fld);
				}

				myTrans.Commit();
				return webMsg;
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		protected MailMessage LoadMailMessage(/*IDbCommand command, */IDbTransaction transaction, WebMailMessage webMsg, Folder fld)
		{
			if ((_account != null) && (webMsg != null) && /*(command != null) && */(fld != null))
			{
				if (webMsg.Downloaded)
				{
					// load local message
					WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
					if (!settings.StoreMailsInDb)
					{
                        if (_account.MailIncomingProtocol != IncomingMailProtocol.WMServer)
                        {
                            FileSystem fs = new FileSystem(_account.Email, _account.ID, true);
                            return fs.LoadMessage(webMsg.IDMsg, fld.GetFullPath(_account.Delimiter));
                        }
					}
					else
					{
						IDbCommand command = _commandCreator.SelectAwmMessagesBody(_account.ID, webMsg.IDMsg);
						command.Transaction = transaction;
						object obj = ExecuteScalarCommand(command);
						if (obj != null)
						{
							byte[] bytes = (byte[]) obj;
							MailMessage msg = new MailMessage();
							msg.LoadMessage(bytes);
							return msg;
						}
					}
				}
			}
			return null;
		}

        public virtual List<long> SearchMessagesIntUids(string condition, Folder folder, bool inHeadersOnly)
		{
			IDbTransaction myTrans = null;
            List<long> uids = new List<long>();
			try
			{
				myTrans = _connection.BeginTransaction();
                string order = GetOrder(_account.DefaultOrder);
                bool asc = GetAsc(_account.DefaultOrder);
                IDbCommand command = _commandCreator.SelectAwmMessagesIntUids(_account.ID, folder, order, asc);
				command.Transaction = myTrans;
				using (IDataReader reader = command.ExecuteReader())
				{
					while(reader.Read())
					{
			            DataTable schemaTable = reader.GetSchemaTable();
			            foreach (DataRow row in schemaTable.Rows)
			            {
				            int index = (int)row[1];
				            if (reader.IsDBNull(index)) continue;
				            string fieldName = row[0] as string;
				            if (fieldName != null && fieldName.ToLower(CultureInfo.InvariantCulture) == "int_uid")
					        {
							    if (this is MsAccessDbManager)
							    {
                                    uids.Add(Convert.ToInt64(reader.GetDecimal(index)));
							    }
							    else
							    {
								    uids.Add(reader.GetInt64(index));
							    }
                            }
                        }
					}
				}
				myTrans.Commit();
                return uids;
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

        public virtual WebMailMessageCollection SearchMessages(string condition, FolderCollection folders, bool inHeadersOnly)
        {
            IDbTransaction myTrans = null;
            WebMailMessageCollection messageCollection = new WebMailMessageCollection();
            try
            {
                myTrans = _connection.BeginTransaction();
                IDbCommand command = _commandCreator.SelectAwmMessages(_account.ID, Utils.ConvertToDBString(_account, condition), folders, inHeadersOnly);
                command.Transaction = myTrans;
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        WebMailMessage webMsg = ReadWebMailMessage(reader, null);
                        messageCollection.Add(webMsg);
                    }
                }
                myTrans.Commit();
                return messageCollection;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
                throw new WebMailDatabaseException(ex);
            }
        }
        
        private string GetOrder(DefaultOrder defaultOrder)
        {
            switch (defaultOrder)
            {
                case DefaultOrder.Date:
                case DefaultOrder.DateDesc:
                default:
                    return "msg_date";
                case DefaultOrder.From:
                case DefaultOrder.FromDesc:
                    return "from_msg";
                case DefaultOrder.To:
                case DefaultOrder.ToDesc:
                    return "to_msg";
                case DefaultOrder.Size:
                case DefaultOrder.SizeDesc:
                    if (this is MySqlDbManager)
                    {
                        return "size";
                    }
                    return "[size]";
                case DefaultOrder.Subj:
                case DefaultOrder.SubjDesc:
                    return "subject";
                case DefaultOrder.Attachment:
                case DefaultOrder.AttachmentDesc:
                    return "attachments";
                case DefaultOrder.Flag:
                case DefaultOrder.FlagDesc:
                    return "flagged";
            }
        }

        private bool GetAsc(DefaultOrder defaultOrder)
        {
            switch (defaultOrder)
            {
                case DefaultOrder.Date:
                case DefaultOrder.From:
                case DefaultOrder.To:
                case DefaultOrder.Size:
                case DefaultOrder.Subj:
                case DefaultOrder.Attachment:
                case DefaultOrder.Flag:
                default:
                    return true;
                case DefaultOrder.DateDesc:
                case DefaultOrder.FromDesc:
                case DefaultOrder.ToDesc:
                case DefaultOrder.SizeDesc:
                case DefaultOrder.SubjDesc:
                case DefaultOrder.AttachmentDesc:
                case DefaultOrder.FlagDesc:
                    return false;
            }
        }

        public virtual WebMailMessageCollection SearchMessages(int page, string condition, FolderCollection folders, bool inHeadersOnly, out int searchMessagesCount)
		{
			IDbTransaction myTrans = null;
			WebMailMessageCollection messageCollection = new WebMailMessageCollection();
			try
			{
				int msgsOnPage = 20;
				if ((_account.UserOfAccount != null) && (_account.UserOfAccount.Settings != null))
				{
					msgsOnPage = _account.UserOfAccount.Settings.MsgsPerPage;
				}
				myTrans = _connection.BeginTransaction();

				string order = GetOrder(_account.DefaultOrder);
				bool asc = GetAsc(_account.DefaultOrder);
				IDbCommand command = _commandCreator.SelectAwmMessages(_account.ID, page, msgsOnPage, Utils.ConvertToDBString(_account, condition), folders, inHeadersOnly, order, asc);
				command.Transaction = myTrans;
				using (IDataReader reader = command.ExecuteReader())
				{
					while(reader.Read())
					{
						WebMailMessage webMsg = ReadWebMailMessage(reader, null);
						messageCollection.Add(webMsg);
					}
				}

				command = _commandCreator.SelectAwmMessagesCount(_account.ID, page, msgsOnPage, Utils.ConvertToDBString(_account, condition), folders, inHeadersOnly, order, asc);
				command.Transaction = myTrans;
				object obj = ExecuteScalarCommand(command);
				searchMessagesCount = (obj != null) ? Convert.ToInt32(obj) : messageCollection.Count;

				myTrans.Commit();
				return messageCollection;
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public virtual WebMailMessageCollection SelectMessages(int pageNumber, DefaultOrder order, Folder fld)
		{
			IDbTransaction myTrans = null;
			WebMailMessageCollection messageCollection = new WebMailMessageCollection();
			try
			{
				myTrans = _connection.BeginTransaction();

				string orderStr = GetOrder(order);
				bool asc = GetAsc(order);
				IDbCommand command = _commandCreator.SelectAwmMessages(_account.ID, fld.ID, pageNumber, _account.UserOfAccount.Settings.MsgsPerPage, orderStr, asc);
				command.Transaction = myTrans;
				command.CommandTimeout = 60;
				using (IDataReader reader = command.ExecuteReader())
				{
					while(reader.Read())
					{
						WebMailMessage webMsg = ReadWebMailMessage(reader, fld);
						messageCollection.Add(webMsg);
					}
				}
				myTrans.Commit();
				return messageCollection;
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public virtual WebMailMessageCollection SelectMessagesOlderThanXDays(int daysCount, Folder fld)
		{
			IDbTransaction myTrans = null;
			WebMailMessageCollection messageCollection = new WebMailMessageCollection();
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.SelectAwmMessagesOlderThanXDays(_account.ID, fld.ID, daysCount);
				command.Transaction = myTrans;
				using (IDataReader reader = command.ExecuteReader())
				{
					while(reader.Read())
					{
						WebMailMessage webMsg = ReadWebMailMessage(reader, fld);
						messageCollection.Add(webMsg);
					}
				}
				myTrans.Commit();
				return messageCollection;
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public virtual void UpdateMessage(WebMailMessage message)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.UpdateAwmMessages(message.ID, message.IDMsg, message.IDAcct, message.IDFolderSrv, message.IDFolderDB, message.StrUid, message.IntUid, Utils.ConvertToDBString(_account, (message.FromMsg != null) ? message.FromMsg.ToString() : string.Empty), Utils.ConvertToDBString(_account, (message.ToMsg != null) ? message.ToMsg.ToString() : string.Empty), Utils.ConvertToDBString(_account, (message.CcMsg != null) ? message.CcMsg.ToString() : string.Empty), Utils.ConvertToDBString(_account, (message.BccMsg != null) ? message.BccMsg.ToString() : string.Empty), Utils.ConvertToDBString(_account, message.Subject), message.MsgDate, message.Attachments, message.Size, message.Seen, message.Flagged, Convert.ToByte(message.Priority), Convert.ToByte(message.Sensitivity), message.Downloaded, message.XSpam, message.Rtl, message.Deleted, message.IsFull, message.Replied, message.Forwarded, Convert.ToByte(message.Flags), Utils.ConvertToDBString(_account, message.BodyText), message.Grayed, message.OverrideCharset);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();
				myTrans.Commit();
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}
		
		#endregion

		#region Messages Functions

		public virtual WebMailMessageCollection SelectMessages(int id_acct, Folder fld, bool msgsCompletely)
		{
			IDbTransaction myTrans = null;
			WebMailMessageCollection messageCollection = new WebMailMessageCollection();
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.SelectAwmMessagesIntUids(id_acct, fld.ID, msgsCompletely);
				command.Transaction = myTrans;
				using (IDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						WebMailMessage webMsg = ReadWebMailMessage(reader, fld);
						messageCollection.Add(webMsg);
					}
				}
				myTrans.Commit();
				return messageCollection;
			}
			catch (Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public virtual WebMailMessageCollection SelectMessages(int id_acct, int[] id_msgs, Folder fld)
		{
			IDbTransaction myTrans = null;
			WebMailMessageCollection messageCollection = new WebMailMessageCollection();
			WebMailMessageCollection messageDownloadedCollection = new WebMailMessageCollection();
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.SelectAwmMessages(id_acct, id_msgs, fld.ID);
				command.Transaction = myTrans;
				using (IDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						WebMailMessage webMsg = ReadWebMailMessage(reader, fld);
						messageCollection.Add(webMsg);
					}
				}
				foreach (WebMailMessage webMsg in messageCollection)
				{
					MailMessage message = LoadMailMessage(/*command, */myTrans, webMsg, fld);
					if (message != null && webMsg != null)
					{
						webMsg.Init(message, (!string.IsNullOrEmpty(webMsg.StrUid)), fld);
					}
					messageDownloadedCollection.Add(webMsg);
				}
				myTrans.Commit();
				return messageDownloadedCollection;
			}
			catch (Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public virtual WebMailMessageCollection SelectMessages(long last_int_uid, int id_acct, Folder fld)
		{
			IDbTransaction myTrans = null;
			WebMailMessageCollection messageCollection = new WebMailMessageCollection();
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.SelectAwmMessages(id_acct, fld.ID, last_int_uid);
				command.Transaction = myTrans;
				using (IDataReader reader = command.ExecuteReader())
				{
					while(reader.Read())
					{
						WebMailMessage webMsg = ReadWebMailMessage(reader, fld);
						messageCollection.Add(webMsg);
					}
				}
				myTrans.Commit();
				return messageCollection;
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public virtual WebMailMessageCollection SelectMessages(Folder fld)
		{
			IDbTransaction myTrans = null;
			WebMailMessageCollection messageCollection = new WebMailMessageCollection();
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.SelectAwmMessages(_account.ID, fld.ID);
				command.Transaction = myTrans;
				using (IDataReader reader = command.ExecuteReader())
				{
					while(reader.Read())
					{
						WebMailMessage webMsg = ReadWebMailMessage(reader, fld);
						messageCollection.Add(webMsg);
					}
				}
				myTrans.Commit();
				return messageCollection;
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public virtual WebMailMessageCollection SelectMessages(int[] ids, Folder fld)
		{
			WebMailMessageCollection coll = new WebMailMessageCollection();
			foreach (int id in ids)
			{
				WebMailMessage msg = SelectMessage(id, fld);
				if (msg != null)
				{
					coll.Add(msg);
				}
			}
			return coll;
		}

		public virtual WebMailMessageCollection SelectMessages(string[] str_uids, Folder fld, bool useDefaultOrder)
		{
			IDbTransaction myTrans = null;
			WebMailMessageCollection messageCollection = new WebMailMessageCollection();
			try
			{
				myTrans = _connection.BeginTransaction();

                bool isImap = (_account.MailIncomingProtocol == IncomingMailProtocol.Imap4);
                string order = useDefaultOrder
                    ? GetOrder(_account.DefaultOrder) + (GetAsc(_account.DefaultOrder) ? " ASC" : " DESC")
                    : "id_msg ASC";
                IDbCommand command = _commandCreator.SelectAwmMessages(_account.ID, fld.ID, str_uids, isImap, order);
				command.Transaction = myTrans;
				using (IDataReader reader = command.ExecuteReader())
				{
					while(reader.Read())
					{
						WebMailMessage webMsg = ReadWebMailMessage(reader, fld);
						messageCollection.Add(webMsg);
					}
				}
				myTrans.Commit();
				return messageCollection;
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public virtual void SaveMessages(WebMailMessageCollection messages, Folder folder)
		{
			int id_msg = GetMessageID();

			foreach (WebMailMessage message in messages)
			{
				id_msg = Utils.RandMsgID(++id_msg);
				SaveMessage(id_msg, message, folder);
			}
		}

		public virtual WebMailMessageCollection SelectMessagesMarkAsDelete(Folder fld)
		{
			IDbTransaction myTrans = null;
			WebMailMessageCollection messageCollection = new WebMailMessageCollection();
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.SelectAwmMessagesMarkAsDelete(_account.ID, fld.ID);
				command.Transaction = myTrans;
				using (IDataReader reader = command.ExecuteReader())
				{
					while(reader.Read())
					{
						WebMailMessage webMsg = ReadWebMailMessage(reader, fld);
						messageCollection.Add(webMsg);
					}
				}
				myTrans.Commit();
				return messageCollection;
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public virtual string[] SelectUids()
		{
			ArrayList result = new ArrayList();
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.SelectAwmReads(_account.ID);
				command.Transaction = myTrans;
				using (IDataReader reader = command.ExecuteReader())
				{
					while(reader.Read())
					{
						result.Add(reader.GetString(0));
					}
				}
				myTrans.Commit();
				return (string[])result.ToArray(typeof(string));
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

        public virtual string[] SelectUids(Folder folder)
        {
            ArrayList result = new ArrayList();
            IDbTransaction myTrans = null;
            try
            {
                myTrans = _connection.BeginTransaction();

                IDbCommand command = _commandCreator.SelectAwmMessagesUids(_account.ID, folder.ID);
                command.Transaction = myTrans;
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(reader.GetString(0));
                    }
                }
                myTrans.Commit();
                return (string[])result.ToArray(typeof(string));
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
                throw new WebMailDatabaseException(ex);
            }
        }


		public virtual int GetMessageID()
		{
			IDbTransaction myTrans = null;
			int id_msg = -1;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.SelectIDMsgFromAwmMessages(_account.ID);
				command.Transaction = myTrans;
				object obj = ExecuteScalarCommand(command);
				id_msg = (obj != null) ? Convert.ToInt32(obj) : 0;
				myTrans.Commit();
				return id_msg;
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

        public virtual void SetMessagesFlagForwarded(int[] ids, Folder fld)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

                IDbCommand command = _commandCreator.UpdateAwmMessagesForwardedFlag(_account.ID, fld.ID, ids);
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

        public virtual void SetMessagesFlags(bool allMessages, /*object[] uids, bool isStrUids*/int[] ids, SystemMessageFlags flags, MessageFlagAction flagsAction, Folder fld)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.UpdateAwmMessagesFlags(_account.ID, fld.ID, allMessages, /*uids, isStrUids*/ids, flags, flagsAction);
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

		public virtual void DeleteMessages(int[] id_msgs)
		{
			if ((id_msgs == null) || (id_msgs.Length == 0)) return;

			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.DeleteFromAwmMessages(_account.ID, id_msgs);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				command = _commandCreator.DeleteFromAwmMessagesBody(_account.ID, id_msgs);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				command = _commandCreator.SelectSumSizesOfRemainMessages(_account.ID);
				command.Transaction = myTrans;
				object obj = ExecuteScalarCommand(command);
				_account.MailboxSize = (obj != null) ? Convert.ToInt64(obj) : 0; // 0 - if no messages

				myTrans.Commit();
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}
		#endregion

		#region Folders Functions
		public virtual Folder CreateFolder(int id_account, long id_parent, FolderType type, string name, string full_path, FolderSyncType sync_type, bool hide, short fld_order)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.InsertIntoAwmFolders(id_account, id_parent, type, Utils.ConvertToUtf7Modified(name) + "#", Utils.ConvertToUtf7Modified(full_path) + "#", sync_type, hide, fld_order);
				command.Transaction = myTrans;
				object obj = ExecuteScalarCommand(command);

				if (obj != null)
				{
					myTrans.Commit();
					return new Folder(Convert.ToInt64(obj), id_account, id_parent, name, full_path, type, sync_type, hide, fld_order);
				}
				throw new WebMailDatabaseException("Can't create folder");
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public virtual void DeleteFolder(long folderID)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.DeleteFromAwmFolders(_account.ID, folderID);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				command = _commandCreator.DeleteFromAwmFoldersTree(folderID);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				command = _commandCreator.DeleteFromAwmFilters(_account.ID, -1, folderID);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();

				myTrans.Commit();
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public virtual FolderCollection SelectFolders(long id_folder, string folderFullName, FolderType type)
		{
			FolderCollection coll = new FolderCollection();
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = null;
				if (id_folder > 0)
				{
					command = _commandCreator.SelectAwmFolders(_account.ID, id_folder);
				}
				else if (folderFullName != null)
				{
					command = _commandCreator.SelectAwmFolders(_account.ID, Utils.ConvertToUtf7Modified(folderFullName));
				}
				else if (type != FolderType.Custom)
				{
					command = _commandCreator.SelectAwmFolders(_account.ID, type);
				}
				else
				{
					command = _commandCreator.SelectAwmFolders(_account.ID);
				}
				command.Transaction = myTrans;
				using (IDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						coll.Add(ReadFolder(reader));
					}
				}
				for (int i = 0; i < coll.Count; i++)
				{
					// get folder size
					command = _commandCreator.SelectFolderSize(coll[i].ID, coll[i].IDAcct);
					command.Transaction = myTrans;
					object obj = ExecuteScalarCommand(command);
					if (obj != null) coll[i].Size = Convert.ToInt64(obj);

					// get folder message count
					command = _commandCreator.SelectFolderMessageCount(coll[i].ID, coll[i].IDAcct);
					command.Transaction = myTrans;
					obj = ExecuteScalarCommand(command);
					if (obj != null) coll[i].MessageCount = Convert.ToInt32(obj);

                    // get folder unread message count
					command = _commandCreator.SelectFolderUnreadMessageCount(coll[i].ID, coll[i].IDAcct);
					command.Transaction = myTrans;
					obj = ExecuteScalarCommand(command);
					if (obj != null) coll[i].UnreadMessageCount = Convert.ToInt32(obj);
					Log.WriteLine("SelectFolders", string.Format("{0}-{1}-{2}", coll[i].Size, coll[i].MessageCount, coll[i].UnreadMessageCount));
				}
				myTrans.Commit();
                foreach (Folder fld in coll)
                {
                	Log.WriteLine("SelectFolders", string.Format("ID: {0}; Path: {1};", fld.ID, fld.FullPath));
                }
				coll = FolderCollection.CreateDatabaseFolderTreeFromList(coll);
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
			return coll;
		}

		public virtual Folder SelectFolder(long id_folder)
		{
			FolderCollection fc = SelectFolders(id_folder, null, FolderType.Custom);
			if ((fc != null) && (fc.Count > 0))
			{
				return fc[0];
			}
			return null;
		}

		public virtual Folder SelectFolder(string folderFullName)
		{
			FolderCollection fc = SelectFolders(-1, folderFullName, FolderType.Custom);
			if ((fc != null) && (fc.Count > 0))
			{
				return fc[0];
			}
			return null;
		}

		public virtual Folder SelectFolder(FolderType type)
		{
			FolderCollection fc = SelectFolders(-1, null, type);
			if ((fc != null) && (fc.Count > 0))
			{
				return fc[0];
			}
			return null;
		}

        public virtual FolderCollection SelectFolderChilds(long id_parent)
        {
			FolderCollection coll = new FolderCollection();
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();
				IDbCommand command = _commandCreator.SelectAwmFoldersChilds(_account.ID, id_parent);
				command.Transaction = myTrans;
				using (IDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						coll.Add(ReadFolder(reader));
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
			return coll;
        }

		public virtual void UpdateFolder(Folder fld)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();
				IDbCommand command = _commandCreator.UpdateAwmFolders(fld.IDAcct, fld.ID, (short)fld.Type, Utils.ConvertToUtf7Modified(fld.UpdateName) + "#", Utils.ConvertToUtf7Modified(fld.UpdateFullPath) + "#", (byte)fld.SyncType, fld.Hide, fld.FolderOrder);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();
				myTrans.Commit();
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public virtual void MoveMessagesToFolder(int[] ids, Folder fromFolder, Folder toFolder)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();
				IDbCommand command = _commandCreator.UpdateAwmMessagesFolders(_account.ID, ids, fromFolder.ID, toFolder.ID);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();
				myTrans.Commit();
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public short GetMaxFolderOrder(long id_parent)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();
				IDbCommand command = _commandCreator.SelectMaxFolderOrder(_account.ID, id_parent);
				command.Transaction = myTrans;
				object obj = ExecuteScalarCommand(command);
				myTrans.Commit();
				
				if (obj != null)
				{
                    return Convert.ToInt16(obj);					
				}
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
			return 0;
		}
		#endregion

		#region Filters Function

		public virtual Filter[] SelectFilters(int id_acct, int id_filter)
		{
			ArrayList filters = new ArrayList();
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.SelectAwmFilters(id_acct, id_filter);
				command.Transaction = myTrans;
				using (IDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						Filter flt = ReadFilter(reader);
						if (flt != null)
						{
							filters.Add(flt);
						}
					}
				}
				myTrans.Commit();
				return (Filter[])filters.ToArray(typeof(Filter));
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public virtual void DeleteFilter(int id_filter)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.DeleteFromAwmFilters(_account.ID, id_filter, -1);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();
				myTrans.Commit();
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public virtual Filter CreateFilter(int id_acct, byte field, byte condition, string filter, byte action, 
			long id_folder, bool applied)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.InsertIntoAwmFilters(id_acct, field, condition, 
					Utils.ConvertToDBString(_account, filter), action, id_folder, applied);
				command.Transaction = myTrans;
				object obj = ExecuteScalarCommand(command);
				myTrans.Commit();

				int id_filter = (obj != null) ? Convert.ToInt32(obj) : -1;

				return new Filter(id_filter, id_acct, (FilterField)field, (FilterCondition)condition, filter, 
					(FilterAction)action, id_folder, applied);
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public virtual void UpdateFilter(int id_filter, int id_acct, byte field, byte condition, string filter, 
			byte action, long id_folder, bool applied)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();
				IDbCommand command = _commandCreator.UpdateAwmFilters(id_filter, id_acct, field, condition, 
					Utils.ConvertToDBString(_account, filter), action, id_folder, applied);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();
				myTrans.Commit();
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			// Check to see if Dispose has already been called.
			if(!_disposed)
			{
				// If disposing equals true, dispose all managed 
				// and unmanaged resources.
				if(disposing)
				{
					// Dispose managed resources.
					try
					{
						//if (_transaction != null) _transaction.Commit();
						if (_connection != null) _connection.Close();
					}
					catch (Exception ex)
					{
                        Log.WriteException(ex);
                        throw new WebMailDatabaseException(ex);
					}
					finally
					{
						_isConnected = false;
					}
				}
             
				// Call the appropriate methods to clean up 
				// unmanaged resources here.
				// If disposing is false, 
				// only the following code is executed.
			}
			_disposed = true;
		}

		#endregion

		#region DB Manipulation Function

        public virtual void CreateDatabase(string databaseName)
        {
            try
            {
                IDbCommand command = _commandCreator.CreateDatabase(databaseName);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new WebMailDatabaseException(ex);
            }
        }
		
        public virtual void CreateTable(string tableName, string prefix)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.CreateTable(tableName, prefix);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();
				
				myTrans.Commit();
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

        public virtual void DropTable(string tableName, string prefix)
        {
            try
            {
                IDbCommand command = _commandCreator.DropTable(tableName, prefix);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new WebMailDatabaseException(ex);
            }
        }
        
        public virtual void AlterTable(string tableName, string field, string prefix)
		{
			try
			{
				IDbCommand command = _commandCreator.AlterTable(tableName, field, prefix);
				command.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				throw new WebMailDatabaseException(ex);
			}
		}

		public virtual void CreateIndex(string prefix, string sufix, string column)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.CreateIndex(prefix, sufix, column);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();
				
				myTrans.Commit();
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public virtual string[] SelectTablesNames()
		{
			ArrayList names = new ArrayList();
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.SelectTablesNames();
				command.Transaction = myTrans;
				using (IDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						names.Add(reader.GetString(0));
					}
				}
				myTrans.Commit();
				return (string[])names.ToArray(typeof(string));
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		#endregion

		#region Senders Function

		public virtual void SetSender(string email, byte safety)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.SelectAwmSendersSafety(_account.UserOfAccount.ID, Utils.ConvertToDBString(_account, email));
				command.Transaction = myTrans;
				object obj = ExecuteScalarCommand(command);
				if (obj != null)
				{
					command = _commandCreator.UpdateAwmSenders(_account.UserOfAccount.ID, Utils.ConvertToDBString(_account, email), safety);
					command.Transaction = myTrans;
					command.ExecuteNonQuery();
				}
				else
				{
					command = _commandCreator.InsertIntoAwmSenders(_account.UserOfAccount.ID, Utils.ConvertToDBString(_account, email), safety);
					command.Transaction = myTrans;
					command.ExecuteNonQuery();
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

		public virtual byte GetSenderSafety(string email, int userId)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.SelectAwmSendersSafety(userId, Utils.ConvertToDBString(_account, email));
				command.Transaction = myTrans;
				object obj = ExecuteScalarCommand(command);
				myTrans.Commit();
				if (obj != null)
				{
					return Convert.ToByte(obj);
				}
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
			return 0;
		}

		#endregion

        #region Domains Functions

        public virtual void DeleteDomain(int id_domain)
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

        public virtual int CreateDomain(string name, IncomingMailProtocol mail_protocol,
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

        public virtual int GetDomainsCount(string condition)
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

        public virtual Domain SelectDomainData(int id_domain)
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

        public virtual Domain SelectDomainData(string domain)
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

        public virtual Domain SelectDomainDataByUrl(string url)
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

        public virtual DomainCollection SelectDomains()
        {
            DomainCollection domains = new DomainCollection();
            IDbTransaction myTrans = null;
            try
            {
                myTrans = _connection.BeginTransaction();

                IDbCommand command = _commandCreator.SelectAwmDomains();
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

        public virtual DomainCollection SelectDomains(int page, int pageSize, string orderBy, bool asc, string searchCondition)
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
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
        }

        public virtual DomainCollection SelectDomainsByAdmin(int id_admin, int page, int pageSize, string orderBy, bool asc, string searchCondition)
        {
            DomainCollection domains = new DomainCollection();
            IDbTransaction myTrans = null;
            try
            {
                myTrans = _connection.BeginTransaction();

                IDbCommand command = _commandCreator.SelectAwmDomains(id_admin, page, pageSize, orderBy, asc, searchCondition);
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

        public virtual DomainCollection SelectDomainsByAdmin(int id_admin)
        {
            DomainCollection domains = new DomainCollection();
            IDbTransaction myTrans = null;
            try
            {
                myTrans = _connection.BeginTransaction();

                IDbCommand command = _commandCreator.SelectAwmDomains(id_admin);
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

        public virtual DomainCollection SelectDomains(short[] MailProtocols)
        {
            DomainCollection domains = new DomainCollection();
            IDbTransaction myTrans = null;
            try
            {
                myTrans = _connection.BeginTransaction();

                IDbCommand command = _commandCreator.SelectAwmDomains(MailProtocols);
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

        public virtual DomainCollection SelectDomains(short[] MailProtocols, int page, int pageSize, string orderBy, bool asc, string searchCondition)
        {
            DomainCollection domains = new DomainCollection();
            IDbTransaction myTrans = null;
            try
            {
                myTrans = _connection.BeginTransaction();

                IDbCommand command = _commandCreator.SelectAwmDomains(MailProtocols, page, pageSize, orderBy, asc, searchCondition);
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

        public virtual void UpdateDomain(int id_domain, string name, IncomingMailProtocol mail_protocol,
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
        
        public virtual void UpdateDomain(int id_domain, string name, IncomingMailProtocol mail_protocol,
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
                      allow_auto_detect_and_correct, global_addr_book, (short)viewmode, (short)save_mail);
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

        public virtual void UpdateAccountsByDomain(string domain, int old_domain, int id_domain, short mail_protocol)
        {
            IDbTransaction myTrans = null;
            try
            {
                myTrans = _connection.BeginTransaction();
                IDbCommand command = _commandCreator.UpdateAccountsByDomain(domain, old_domain, id_domain, mail_protocol);
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

		protected object ExecuteScalarCommand(IDbCommand scalarCommand)
		{
			object obj = scalarCommand.ExecuteScalar();
			return (obj != DBNull.Value) ? obj : null;
		}

		protected virtual AddressBookContact ReadAddressBookContact(IDataReader reader)
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
						case "contacts.id_addr": // for Access
							result.IDAddr = (long)reader.GetInt32(index);
							break;
						case "id_addr":
							if (this is MsAccessDbManager)
								result.IDAddr = reader.GetInt32(index);
							else
								result.IDAddr = reader.GetInt64(index);
							break;
						case "id_user":
							result.IDUser = reader.GetInt32(index);
							break;
						case "h_email":
							result.HEmail = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "fullname":
							result.FullName = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "notes":
							result.Notes = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "use_friendly_nm":
							result.UseFriendlyName = reader.GetBoolean(index);
							break;
						case "h_street":
							result.HStreet = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "h_city":
							result.HCity = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "h_state":
							result.HState = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "h_zip":
							result.HZip = Utils.ConvertFromDBString(_account, reader.GetString(index)).Trim();
							break;
						case "h_country":
							result.HCountry = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "h_phone":
							result.HPhone = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "h_fax":
							result.HFax = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "h_mobile":
							result.HMobile = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "h_web":
							result.HWeb = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "b_email":
							result.BEmail = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "b_company":
							result.BCompany = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "b_street":
							result.BStreet = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "b_city":
							result.BCity = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "b_state":
							result.BState = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "b_zip":
							result.BZip = Utils.ConvertFromDBString(_account, reader.GetString(index)).Trim();
							break;
						case "b_country":
							result.BCountry = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "b_job_title":
							result.BJobTitle = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "b_department":
							result.BDepartment = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "b_office":
							result.BOffice = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "b_phone":
							result.BPhone = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "b_fax":
							result.BFax = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "b_web":
							result.BWeb = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "birthday_day":
							result.BirthdayDay = reader.GetByte(index);
							break;
						case "birthday_month":
							result.BirthdayMonth = reader.GetByte(index);
							break;
						case "birthday_year":
							result.BirthdayYear = reader.GetInt16(index);
							break;
						case "other_email":
							result.OtherEmail = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "primary_email":
							result.PrimaryEmail = (ContactPrimaryEmail)reader.GetByte(index);
							break;
						case "id_addr_prev":
							if (this is MsAccessDbManager)
								result.IDAddrPrev = Convert.ToInt64(reader.GetDecimal(index));
							else
								result.IDAddrPrev = reader.GetInt64(index);
							break;
						case "tmp":
							result.Tmp = reader.GetBoolean(index);
							break;
						case "use_frequency":
							result.UseFrequency = reader.GetInt32(index);
							break;
						case "auto_create":
							result.AutoCreate = reader.GetBoolean(index);
							break;
					}
				}
			}
			return result;
		}

		protected virtual AddressBookGroup ReadAddressBookGroup(IDataReader reader)
		{
			AddressBookGroup result = new AddressBookGroup();
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
						case "groups.id_group":
							result.IDGroup = reader.GetInt32(index);
							break;
						case "id_group":
							result.IDGroup = reader.GetInt32(index);
							break;
						case "id_user":
							result.IDUser = reader.GetInt32(index);
							break;
						case "group_nm":
							result.GroupName = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "use_frequency":
							result.UseFrequency = reader.GetInt32(index);
							break;
						case "email":
							result.Email = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "company":
							result.Company = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "street":
							result.Street = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "city":
							result.City = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "state":
							result.State = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "zip":
							result.Zip = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "country":
							result.Country = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "phone":
							result.Phone = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "fax":
							result.Fax = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "web":
							result.Web = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "organization":
							if (this is MySqlDbManager)
								result.Organization = (Convert.ToByte(reader.GetValue(index)) == 1) ? true : false;
							else
								result.Organization = reader.GetBoolean(index);
							break;
					}
				}
			}
			return result;
		}

		protected virtual WebMailMessage ReadWebMailMessage(IDataReader reader, Folder fld)
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
							if (this is MsAccessDbManager)
							{
								webMsg.ID = Convert.ToInt64(reader.GetInt32(index));
							}
							else
							{
								webMsg.ID = reader.GetInt64(index);
							}
							break;
						case "id_msg":
							webMsg.IDMsg = reader.GetInt32(index);
							break;
						case "id_acct":
							webMsg.IDAcct = reader.GetInt32(index);
							break;
						case "id_folder_srv":
							if (this is MsAccessDbManager)
							{
								webMsg.IDFolderSrv = Convert.ToInt64(reader.GetDecimal(index));
							}
							else
							{
								webMsg.IDFolderSrv = reader.GetInt64(index);
							}
							break;
						case "id_folder_db":
							if (this is MsAccessDbManager)
							{
								webMsg.IDFolderDB = Convert.ToInt64(reader.GetDecimal(index));
							}
							else
							{
								webMsg.IDFolderDB = reader.GetInt64(index);
							}
							break;
						case "str_uid":
							webMsg.StrUid = reader.GetString(index);
							break;
						case "int_uid":
							if (this is MsAccessDbManager)
							{
								webMsg.IntUid = Convert.ToInt64(reader.GetDecimal(index));
							}
							else
							{
								webMsg.IntUid = reader.GetInt64(index);
							}
							break;
						case "from_msg":
							webMsg.FromMsg = EmailAddress.Parse(Utils.ConvertFromDBString(_account, reader.GetString(index)));
							break;
						case "to_msg":
							webMsg.ToMsg = EmailAddressCollection.Parse(Utils.ConvertFromDBString(_account, reader.GetString(index)));
							break;
						case "cc_msg":
							webMsg.CcMsg = EmailAddressCollection.Parse(Utils.ConvertFromDBString(_account, reader.GetString(index)));
							break;
						case "bcc_msg":
							webMsg.BccMsg = EmailAddressCollection.Parse(Utils.ConvertFromDBString(_account, reader.GetString(index)));
							break;
						case "subject":
							webMsg.Subject = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "msg_date":
							webMsg.MsgDate = reader.GetDateTime(index);
							break;
						case "attachments":
							webMsg.Attachments = reader.GetBoolean(index);
							break;
						case "size":
							if (this is MsAccessDbManager)
							{
								webMsg.Size = Convert.ToInt64(reader.GetDecimal(index));
							}
							else
							{
								webMsg.Size = reader.GetInt64(index);
							}
							break;
						case "seen":
							webMsg.Seen = reader.GetBoolean(index);
							break;
						case "flagged":
							webMsg.Flagged = reader.GetBoolean(index);
							break;
						case "priority":
							webMsg.Priority = (MailPriority)reader.GetByte(index);
							break;
						case "downloaded":
							webMsg.Downloaded = reader.GetBoolean(index);
							break;
						case "x_spam":
							webMsg.XSpam = reader.GetBoolean(index);
							break;
						case "rtl":
							webMsg.Rtl = reader.GetBoolean(index);
							break;
						case "deleted":
							webMsg.Deleted = reader.GetBoolean(index);
							break;
						case "is_full":
							webMsg.IsFull = reader.GetBoolean(index);
							break;
						case "replied":
							webMsg.Replied = reader.GetBoolean(index);
							break;
						case "forwarded":
							webMsg.Forwarded = reader.GetBoolean(index);
							break;
						case "flags":
                            webMsg.Flags = (SystemMessageFlags)reader.GetInt32(index);
							break;
						case "body_text":
							webMsg.BodyText = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "grayed":
							webMsg.Grayed = reader.GetBoolean(index);
							break;
						case "folder_name":
                            string folderFullName = reader.GetString(index);
                            if (!string.IsNullOrEmpty(folderFullName))
                            {
                                // remove trailing symbol
                                folderFullName = folderFullName.Remove(folderFullName.Length - 1, 1);
                            }
                            webMsg.FolderFullName = Utils.ConvertFromDBString(_account, folderFullName);
							break;
						case "charset":
							webMsg.OverrideCharset = reader.GetInt32(index);
							break;
                        case "sensitivity":
                            webMsg.Sensitivity = (WebMailSensitivity)reader.GetByte(index);
                            break;
					}
				}
			}
			if (fld != null) webMsg.FolderFullName = fld.FullPath;
			return webMsg;
		}

		protected virtual Account ReadAccount(IDataReader reader)
		{
			Account acct;
			if (_dataFolder != string.Empty)
			{
				acct = new Account(_dataFolder);
			}
			else
			{
				acct = new Account();
			}
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
						acct.MailboxSize = reader.GetInt64(index);
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

        protected virtual Domain ReadDomain(IDataReader reader)
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
                            dom.Attachment_size_limit = reader.GetInt64(index);
                            break;
                        case "allow_attachment_limit":
                            dom.Allow_attachment_limit = reader.GetBoolean(index);
                            break;
                        case "mailbox_size_limit":
                            dom.Mailbox_size_limit = reader.GetInt64(index);
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
                        case "save_mail":
                            dom.SaveMail = (SaveMail)reader.GetByte(index);
                            break;
                    }
                }
            }
            return dom;
        }

		protected virtual Folder ReadFolder(IDataReader reader)
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
							if (this is MsAccessDbManager)
							{
								result.ID = Convert.ToInt64(reader.GetInt32(index));
							}
							else
							{
								result.ID = reader.GetInt64(index);
							}
							break;
						case "id_acct":
							result.IDAcct = reader.GetInt32(index);
							break;
						case "id_parent":
							if (this is MsAccessDbManager)
							{
								result.IDParent = Convert.ToInt64(reader.GetDecimal(index));
							}
							else
							{
								result.IDParent = reader.GetInt64(index);
							}
							break;
						case "type":
							result.Type = (FolderType)reader.GetInt16(index);
							break;
						case "name":
							string folderName = reader.GetString(index);
							if (!string.IsNullOrEmpty(folderName))
							{
								// remove trailing symbol
								folderName = folderName.Remove(folderName.Length - 1, 1);
							}
							result.Name = Utils.ConvertFromUtf7Modified(folderName);
							break;
						case "full_path":
							string folderFullName = reader.GetString(index);
							if (!string.IsNullOrEmpty(folderFullName))
							{
								// remove trailing symbol
								folderFullName = folderFullName.Remove(folderFullName.Length - 1, 1);
							}
							result.FullPath = Utils.ConvertFromUtf7Modified(folderFullName);
							break;
						case "sync_type":
							result.SyncType = (FolderSyncType) reader.GetByte(index);
							break;
						case "hide":
							result.Hide = reader.GetBoolean(index);
							break;
						case "fld_order":
							result.FolderOrder = reader.GetInt16(index);
							break;
					}
				}
			}
			return result;
		}

		protected virtual Filter ReadFilter(IDataReader reader)
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
							result.IDFilter = reader.GetInt32(index);
							break;
						case "id_acct":
							result.IDAcct = reader.GetInt32(index);
							break;
						case "field":
							result.Field = (FilterField)reader.GetByte(index);
							break;
						case "condition":
							result.Condition = (FilterCondition)reader.GetByte(index);
							break;
						case "filter":
							result.FilterStr = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
						case "action":
							result.Action = (FilterAction)reader.GetByte(index);
							break;
						case "id_folder":
							if (this is MsAccessDbManager)
							{
								result.IDFolder = Convert.ToInt64(reader.GetInt32(index));
							}
							else
							{
								result.IDFolder = reader.GetInt64(index);
							}
							break;
						case "applied":
							result.Applied = reader.GetBoolean(index);
							break;
					}
				}
			}
			return result;
		}

		private TempRow ReadTempRow(IDataReader reader)
		{
			DataTable schemaTable = reader.GetSchemaTable();
			TempRow result = new TempRow();

			foreach (DataRow row in schemaTable.Rows)
			{
				int index = (int)row[1];
				if (reader.IsDBNull(index)) continue;
				string fieldName = row[0] as string;
				if (fieldName != null)
				{
					switch (fieldName.ToLower(CultureInfo.InvariantCulture))
					{
						case "id_temp":
                            if (this is MsAccessDbManager)
                            {
                                result.ID = Convert.ToInt64(reader.GetInt32(index));
                            }
                            else
                            {
                                result.ID = reader.GetInt64(index);
                            }
							break;
						case "id_acct":
							result.IDAcct = reader.GetInt32(index);
							break;
						case "data_val":
							result.DataVal = Utils.ConvertFromDBString(_account, reader.GetString(index));
							break;
					}
				}
			}
			return result;
		}

		protected virtual User ReadUser(IDataReader reader)
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
							id_user = reader.GetInt32(index);
							break;
						case "id_user":
							id_user = reader.GetInt32(index);
							break;
						case "deleted":
							deleted = reader.GetBoolean(index);
							break;
					}
				}
			}
			UserSettings settings = ReadUserSettings(reader);
			return new User(id_user, deleted, settings);
		}

		protected virtual UserColumn ReadUserColumn(IDataReader reader)
		{
			DataTable schemaTable = reader.GetSchemaTable();
			UserColumn result = new UserColumn();

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
							result.ID = reader.GetInt32(index);
							break;
						case "id_user":
							result.IDUser = reader.GetInt32(index);
							break;
						case "id_column":
							result.IDColumn = reader.GetInt32(index);
							break;
						case "column_value":
							result.Value = reader.GetInt32(index);
							break;
					}
				}
			}
			return result;
		}

		protected virtual UserSettings ReadUserSettings(IDataReader reader)
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
							settings.ID = reader.GetInt32(index);
							break;
						case "users.id_user": // for Access
							settings.IDUser = reader.GetInt32(index);
							break;
						case "id_user":
							settings.IDUser = reader.GetInt32(index);
							break;
						case "msgs_per_page":
							settings.MsgsPerPage = reader.GetInt16(index);
							break;
						case "white_listing":
							settings.WhiteListing = reader.GetBoolean(index);
							break;
						case "x_spam":
							settings.XSpam = reader.GetBoolean(index);
							break;
						case "last_login":
							settings.LastLogin = reader.GetDateTime(index);
							break;
						case "logins_count":
							settings.LoginsCount = reader.GetInt32(index);
							break;
						case "def_skin":
							settings.DefaultSkin = reader.GetString(index);
							break;
						case "def_lang":
							settings.DefaultLanguage = reader.GetString(index);
							break;
						case "def_charset_inc":
							settings.DefaultCharsetInc = reader.GetInt32(index);
							break;
						case "def_charset_out":
							settings.DefaultCharsetOut = reader.GetInt32(index);
							break;
						case "def_timezone":
							settings.DefaultTimeZone = reader.GetInt16(index);
							break;
						case "def_date_fmt":
                            string tempDateFormat = reader.GetString(index);
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
							settings.HideFolders = reader.GetBoolean(index);
							break;
						case "mailbox_limit":
							if (this is MsAccessDbManager)
							{
								settings.MailboxLimit = Convert.ToInt64(reader.GetDecimal(index));
							}
							else
							{
								settings.MailboxLimit = reader.GetInt64(index);
							}
							break;
						case "allow_change_settings":
							settings.AllowChangeSettings = reader.GetBoolean(index);
							break;
						case "allow_dhtml_editor":
							settings.AllowDhtmlEditor = reader.GetBoolean(index);
							break;
						case "allow_direct_mode":
							settings.AllowDirectMode = reader.GetBoolean(index);
							break;
						case "db_charset":
							try
							{
								settings.DbCharset = reader.GetInt32(index);
							}
							catch
							{
								settings.DbCharset = Encoding.UTF8.CodePage;
							}
							break;
						case "horiz_resizer":
							settings.HorizResizer = reader.GetInt16(index);
							break;
						case "vert_resizer":
							settings.VertResizer = reader.GetInt16(index);
							break;
						case "mark":
							settings.Mark = reader.GetByte(index);
							break;
						case "reply":
							settings.Reply = reader.GetByte(index);
							break;
						case "contacts_per_page":
							settings.ContactsPerPage = reader.GetInt16(index);
							break;
						case "view_mode":
							settings.ViewMode = (ViewMode)reader.GetByte(index);
							break;
                        case "auto_checkmail_interval":
                            settings.AutoCheckmailInterval = reader.GetInt32(index);
                            break;
                    }
				}
			}
			return settings;
		}

        protected virtual Subadmin ReadSubadmin(IDataReader reader)
        {
            DataTable schemaTable = reader.GetSchemaTable();
            Subadmin result = new Subadmin();

            foreach (DataRow row in schemaTable.Rows)
            {
                int index = (int)row[1];
                if (reader.IsDBNull(index)) continue;
                string fieldName = row[0] as string;
                if (fieldName != null)
                {
                    switch (fieldName.ToLower(CultureInfo.InvariantCulture))
                    {
                        case "id_admin":
                            result.ID = reader.GetInt32(index);
                            break;
                        case "login":
                            result.Login = reader.GetString(index);
                            break;
                        case "password":
                            result.Password = AdminPanelUtils.DecryptPassword(reader.GetString(index));
                            break;
                        case "description":
                            result.Description = reader.GetString(index);
                            break;
                    }
                }
            }
            return result;
        }


		public virtual void CreateFoldersTree(FolderCollection folderList)
		{
			AddFolderNodesToDatabase(-1, folderList, 0);
		}

		protected void AddFolderNodesToDatabase(long id_parent, FolderCollection folderList, short folderOrder)
		{
			if ((folderList != null))
			{
				foreach (Folder f in folderList)
				{
                    switch (f.Type)
                    {
                        case FolderType.Inbox:
                            folderOrder = 0;
                            break;
                        case FolderType.SentItems:
                            folderOrder = 1;
                            break;
                        case FolderType.Drafts:
                            folderOrder = 2;
                            break;
                        case FolderType.Spam:
                            folderOrder = 3;
                            break;
                        case FolderType.Trash:
                            folderOrder = 4;
                            break;
                        default:
                            folderOrder++;
                            break;
                    }
                    
                    Folder insertedFolder = CreateFolder(_account.ID, id_parent, f.Type, f.Name, f.FullPath, f.SyncType, f.Hide, folderOrder++);
					if (f.SubFolders.Count > 0)
					{
						AddFolderNodesToDatabase(insertedFolder.ID, f.SubFolders, 0);
					}
				}
			}
		}

		internal static IDataAdapter CreateDataAdapter(DbManager manager, IDbCommand command)
		{
			if (manager is MsSqlDbManager)
			{
				return new SqlDataAdapter((SqlCommand)command);
			}
			if (manager is MsAccessDbManager)
			{
				return new OleDbDataAdapter((OleDbCommand)command);
			}
			if (manager is MySqlDbManager)
			{
				return new OdbcDataAdapter((OdbcCommand)command);
			}
            if (manager is PostgreSqlDbManager)
            {
                return new OdbcDataAdapter((OdbcCommand)command);
            }
            return null;
		}

		#region UserColumn Functions
		public virtual UserColumn CreateUserColumn(int id_column, int id_user, int value)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.InsertIntoAwmColumns(id_column, id_user, value);
				command.Transaction = myTrans;
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

		public virtual void DeleteUserColumn(UserColumn column)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.DeleteFromAwmColumns(column.ID, column.IDUser);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();
				myTrans.Commit();
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public virtual UserColumn[] SelectUserColumns(int id_user)
		{
			ArrayList columns = new ArrayList();
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.SelectAwmColumns(id_user);
				command.Transaction = myTrans;
				using (IDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						UserColumn column = ReadUserColumn(reader);
						if (column != null)
						{
							columns.Add(column);
						}
					}
				}
				myTrans.Commit();
				return (UserColumn[])columns.ToArray(typeof(UserColumn));
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public virtual void UpdateUserColumn(int id_column, int id_user, int value)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();
				IDbCommand command = _commandCreator.UpdateAwmColumns(id_column, id_user, value);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();
				myTrans.Commit();
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}
        
        public virtual void UpdateSubadmin(int id_admin, string login, string password, string description, List<string> domains)
        {
            IDbTransaction myTrans = null;
            try
            {
                myTrans = _connection.BeginTransaction();
                IDbCommand command = _commandCreator.UpdateAwmSubadmins(id_admin, login, password, description);
                command.Transaction = myTrans;
                command.ExecuteNonQuery();

                command = _commandCreator.DeleteFromAwmSubadminDomains(id_admin);
                command.ExecuteNonQuery();

                foreach (string domain in domains)
                {
                    command = _commandCreator.InsertIntoAwmSubadminDomains(id_admin, int.Parse(domain));
                    command.ExecuteNonQuery();
                }
                myTrans.Commit();
            }
            catch (Exception ex)
            {
                if (myTrans != null) myTrans.Rollback();
                throw new WebMailDatabaseException(ex);
            }
        }

        public virtual void DeleteSubadmin(int id_admin)
        {
            IDbTransaction myTrans = null;
            try
            {
                myTrans = _connection.BeginTransaction();

                IDbCommand command = _commandCreator.DeleteFromAwmSubadmins(id_admin);
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

        public virtual int CreateSubadmin(string login, string password, string description, List<string> domains)
        {
            int result = 0;
            IDbTransaction myTrans = null;
            try
            {
                myTrans = _connection.BeginTransaction();
                IDbCommand command = _commandCreator.InsertIntoAwmSubadmins(login, password, description);
                command.Transaction = myTrans;
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
        #endregion

		public TempRow CreateTempRow(int id_acct, string data_val)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.InsertIntoAwmTemp(id_acct, Utils.ConvertToDBString(_account, data_val));
				command.Transaction = myTrans;
				object obj = ExecuteScalarCommand(command);
				myTrans.Commit();

				long id_temp = (obj != null) ? Convert.ToInt64(obj) : -1;

				return new TempRow(id_temp, id_acct, data_val);
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public void DeleteTempRow(long id_temp)
		{
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.DeleteFromAwmTemp((_account != null) ? _account.ID : -1, id_temp);
				command.Transaction = myTrans;
				command.ExecuteNonQuery();
				myTrans.Commit();
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public TempRow[] SelectTempRow(long id_temp, string data_val)
		{
			ArrayList tempRows = new ArrayList();
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.SelectAwmTemp((_account != null) ? _account.ID : -1, id_temp, data_val);
				command.Transaction = myTrans;
				using (IDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						TempRow row = ReadTempRow(reader);
						if (row != null)
						{
							tempRows.Add(row);
						}
					}
				}
				myTrans.Commit();
				return (TempRow[])tempRows.ToArray(typeof(TempRow));
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}

		public AddressBookGroup SelectAddressBookGroup(string group_uid)
		{
			AddressBookGroup group = null;
			IDbTransaction myTrans = null;
			try
			{
				myTrans = _connection.BeginTransaction();

				IDbCommand command = _commandCreator.SelectAwmAddrGroups(_account.UserOfAccount.ID, group_uid);
				command.Transaction = myTrans;
				using (IDataReader reader = command.ExecuteReader())
				{
					if (reader.Read())
					{
						group = ReadAddressBookGroup(reader);
					}
				}
				if (group != null)
				{
					ArrayList contacts = new ArrayList();
					command = _commandCreator.SelectAwmAddrBook(_account.UserOfAccount.ID, group.IDGroup);
					command.Transaction = myTrans;
					using (IDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							AddressBookContact contact = ReadAddressBookContact(reader);
							if (contact != null)
							{
								contacts.Add(contact);
							}
						}
					}
					group.Contacts = (AddressBookContact[])contacts.ToArray(typeof(AddressBookContact));
				}
				myTrans.Commit();
				return group;
			}
			catch (Exception ex)
			{
				Log.WriteException(ex);
				if (myTrans != null) myTrans.Rollback();
				throw new WebMailDatabaseException(ex);
			}
		}
        
        public virtual int GetActiveCalendarsCount()
        {
            int result;
            IDbTransaction myTrans = null;
            try
            {
                myTrans = _connection.BeginTransaction();

                IDbCommand command = _commandCreator.SelectActiveCalendarsCount();
                command.Transaction = myTrans;
                object obj = ExecuteScalarCommand(command);
                myTrans.Commit();
                result = (obj != null) ? Convert.ToInt32(obj) : -1;
            }
            catch (Exception ex)
            {
                if (myTrans != null) myTrans.Rollback();
                throw new WebMailDatabaseException(ex);
            }
            return result;
        }

        public virtual void UpdateActiveCalendars()
        {
            IDbTransaction myTrans = null;
            try
            {
                myTrans = _connection.BeginTransaction();

                IDbCommand command = _commandCreator.UpdateActiveCalendars();
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
	}

	public class MsSqlDbManager : DbManager
	{
		public MsSqlDbManager() : this((Account) null) { }

		public MsSqlDbManager(Account acct) : base(acct)
		{
			_connection = new SqlConnection();
			_commandCreator = new MsSqlCommandCreator(_connection as SqlConnection, new SqlCommand());
		}

		public MsSqlDbManager(string dataFolder)
		{
			_dataFolder = dataFolder;
			_connection = new OdbcConnection();
			_commandCreator = new MsSqlCommandCreator(_connection as SqlConnection, new SqlCommand(), dataFolder);
		}

	}

}
