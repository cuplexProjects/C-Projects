using System;
using System.Globalization;
using MailBee.ImapMail;
using System.Collections.Generic;

namespace WebMail
{
	/// <summary>
	/// Summary description for DbStorage.
	/// </summary>
	public abstract class DbStorage	: MailStorage, IDisposable
	{
		protected DbManager _dbMan = null;
		protected bool _disposed = false;

		public DbStorage(Account account) : base(account)
		{
			DbManagerCreator creator = new DbManagerCreator();
			_dbMan = creator.CreateDbManager(account);
		}

		public override void Connect()
		{
			_dbMan.Connect();
		}

		public override bool IsConnected()
		{
			return _dbMan.IsConnected;
		}

		public override void Disconnect()
		{
			Dispose();
		}

		public override long GetMailStorageSize()
		{
			if (_account != null)
			{
				return _dbMan.GetUserMailboxsSize(_account.IDUser);
			}
			return 0;
		}


		public AddressBookContact CreateAddressBookContact(AddressBookContact contact)
		{
			string email = contact.HEmail;
			switch (contact.PrimaryEmail)
			{
				case ContactPrimaryEmail.Business:
					email = contact.BEmail;
					break;
				case ContactPrimaryEmail.Other:
					email = contact.OtherEmail;
					break;
			}
            //AddressBookContact[] dbContacts = _dbMan.SelectAddressBookContacts(email);
            //foreach (AddressBookContact dbContact in dbContacts)
            //{
            //    if (string.Compare(contact.FullName, dbContact.FullName, true, CultureInfo.InvariantCulture) == 0)
            //    {
            //        WebmailResourceManager resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
            //        throw new WebMailException(resMan.GetString("ErrorContactExists"));
            //    }
            //}
			return _dbMan.CreateAddressBookContact(contact);
		}

		public AddressBookGroupContact[] LoadAddressBookContactsGroups(int page, short sort_field, short sort_order, int id_group, string look_for, int look_for_type)
		{
			return _dbMan.SelectAddressBookContactsGroups(page, sort_field, sort_order, id_group, look_for, look_for_type);
		}

		public AddressBookContact GetAddressBookContact(long id_addr)
		{
			AddressBookContact[] contacts = _dbMan.SelectAddressBookContacts(id_addr);
			return (contacts.Length > 0) ? contacts[0] : null;
		}

		public AddressBookContact GetAddressBookContact(string email)
		{
			AddressBookContact[] contacts = _dbMan.SelectAddressBookContacts(email);
			return (contacts.Length > 0) ? contacts[0] : null;
		}

		public void DeleteAddressBookContactsGroups(AddressBookContact[] contacts, AddressBookGroup[] groups)
		{
			_dbMan.DeleteAddressBookContacts(contacts);
			_dbMan.DeleteAddressBookGroups(groups);
		}

		public void UpdateAddressBookContact(AddressBookContact contact)
		{
			_dbMan.UpdateAddressBookContact(contact);
		}

		public void AddContactsToGroup(int id_group, AddressBookContact[] contacts)
		{
			_dbMan.AddContactsToGroup(id_group, contacts);
		}

		public AddressBookGroup CreateAddressBookGroup(AddressBookGroup group)
		{
			return _dbMan.CreateAddressBookGroup(group);
		}

		public AddressBookGroup GetAddressBookGroup(int id_group)
		{
			return _dbMan.SelectAddressBookGroup(id_group);
		}

		public AddressBookGroup[] GetAddressBookGroups()
		{
			return _dbMan.SelectAddressBookGroups();
		}

		public int GetAddressBookContactsCount(string look_for, int look_for_type)
		{
			return _dbMan.GetAddressBookContactsCount(_account.UserOfAccount.ID, look_for, look_for_type);
		}

		public int GetAddressBookGroupsCount(string look_for, int look_for_type)
		{
			return _dbMan.GetAddressBookGroupsCount(_account.UserOfAccount.ID, look_for, look_for_type);
		}

		public void UpdateAddressBookGroup(AddressBookGroup group)
		{
			_dbMan.UpdateAddressBookGroup(group);
		}

		public override Folder CreateFolder(Folder fld)
		{
			short fldOrder = _dbMan.GetMaxFolderOrder(fld.IDParent);

			return _dbMan.CreateFolder(_account.ID, fld.IDParent, fld.Type, fld.Name, fld.FullPath, fld.SyncType, fld.Hide, ++fldOrder);
		}

		public override void DeleteFolder(Folder fld)
		{
			FolderCollection childFolders = _dbMan.SelectFolderChilds(fld.ID);
			foreach (Folder f in childFolders)
			{
				DeleteFolderAndRelatedMessages(f);
			}
			DeleteFolderAndRelatedMessages(fld);
		}

		protected void DeleteFolderAndRelatedMessages(Folder fld)
		{
			WebMailMessageCollection msgs = _dbMan.SelectMessages(fld);
			_dbMan.DeleteMessages(ObjectsToInts32(msgs.ToIDsCollection()));

			_dbMan.DeleteFolder(fld.ID);
		}

		public override void UpdateFolder(Folder fld)
		{
			_dbMan.UpdateFolder(fld);
		}

		public override FolderCollection GetFolders()
		{
			return _dbMan.SelectFolders(-1, null, FolderType.Custom);
		}

		public override Folder GetFolder(string folderFullName)
		{
			return _dbMan.SelectFolder(folderFullName + "#");
		}

		public Folder GetFolder(FolderType type)
		{
			return _dbMan.SelectFolder(type);
		}

		public Folder GetFolder(long id_folder)
		{
			return _dbMan.SelectFolder(id_folder);
		}

		public Filter[] GetFilters()
		{
			return _dbMan.SelectFilters(_account.ID, -1);
		}

		public Filter GetFilter(int id_filter)
		{
            Filter[] filters = _dbMan.SelectFilters(_account.ID, id_filter);
			if ((filters != null) && (filters.Length > 0))
			{
				return filters[0];
			}
			return null;
		}

		public void DeleteFilter(int id_filter)
		{
			_dbMan.DeleteFilter(id_filter);
		}

		public Filter CreateFilter(byte field, byte condition, string filter, byte action, long id_folder, bool applied)
		{
			return _dbMan.CreateFilter(_account.ID, field, condition, filter, action, id_folder, applied);
		}

		public void UpdateFilter(int id_filter, byte field, byte condition, string filter, byte action, long id_folder,
			bool applied)
		{
			_dbMan.UpdateFilter(id_filter, _account.ID, field, condition, filter, action, id_folder, applied);
		}

		/// <summary>
		/// Load message from DB.
		/// </summary>
		/// <param name="index">uid</param>
		/// <param name="fld">Folder</param>
		/// <returns>WebMailMessage or NULL</returns>
		public override WebMailMessage LoadMessage(object index, Folder fld)
		{
			return LoadMessage(index, fld, true);
		}

		public WebMailMessage LoadMessage(object index, Folder fld, bool markSeen)
		{
			int id_msg = ObjectToInt32(index);
			WebMailMessage msg = _dbMan.SelectMessage(id_msg, fld);
			if (markSeen && msg != null && !msg.Seen)
			{
				// mark as seen
				msg.Seen = true;
				_dbMan.UpdateMessage(msg);
			}
			return msg;
		}

		/// <summary>
		/// Load message from DB.
		/// </summary>
		/// <returns>WebMailMessageCollection or empty</returns>
		public override WebMailMessageCollection LoadMessages(Folder fld)
		{
			return _dbMan.SelectMessages(fld);
		}

		public WebMailMessageCollection LoadMessages(int id_acct, int[] id_msgs, Folder fld)
		{
			return _dbMan.SelectMessages(id_acct, id_msgs, fld);
		}
		/// <summary>
		/// Load message from DB.
		/// </summary>
		/// <param name="messageIndexSet">uid array</param>
		/// <param name="fld">Folder</param>
		/// <returns>WebMailMessageCollection or empty</returns>
		public override WebMailMessageCollection LoadMessages(object[] messageIndexSet, Folder fld)
		{
			int[] ids = ObjectsToInts32(messageIndexSet);

			return _dbMan.SelectMessages(ids, fld);
		}

        public virtual WebMailMessageCollection LoadMessagesByUids(string[] strUids, Folder fld, bool useDefaultOrder)
		{
            return _dbMan.SelectMessages(strUids, fld, useDefaultOrder);
		}

		public virtual WebMailMessageCollection LoadMessages(int id_acct, Folder fld, bool msgsCompletely)
		{
			return _dbMan.SelectMessages(id_acct, fld, msgsCompletely);
		}

		public virtual WebMailMessageCollection LoadMessages(long last_int_uid, int id_acct, Folder fld)
		{
			return _dbMan.SelectMessages(last_int_uid, id_acct, fld);
		}

		public virtual WebMailMessageCollection LoadMessagesToDelete(bool allMessages, object[] messageIndexSet, Folder fld)
		{
			if (!allMessages)
			{
				if (messageIndexSet != null)
				{
					return LoadMessages(messageIndexSet, fld);
				}
				return LoadMessages(fld);
			}
			return _dbMan.SelectMessagesMarkAsDelete(fld);
		}

		public override WebMailMessageCollection LoadMessageHeaders(Folder fld)
		{
			return _dbMan.SelectMessages(fld);
		}

		public override WebMailMessageCollection LoadMessageHeaders(int pageNumber, Folder fld)
		{
			WebMailMessageCollection result = null;
			if (_account != null)
			{
				result = _dbMan.SelectMessages(pageNumber, _account.DefaultOrder, fld);
			}
			else
			{
				Log.WriteLine("GetMessageHeaders", "Account is NULL");
			}
			return result;
		}

		public override void MoveMessages(object[] messageIndexSet, Folder fromFolder, Folder toFolder)
		{
			int[] ids = ObjectsToInts32(messageIndexSet);
			_dbMan.MoveMessagesToFolder(ids, fromFolder, toFolder);
		}

        public override void SetMessagesFlagForwarded(object[] messageIndexSet, Folder fld)
        {
            int[] ids = ObjectsToInts32(messageIndexSet);
            _dbMan.SetMessagesFlagForwarded(ids, fld);
        }

		public override void SetMessagesFlags(object[] messageIndexSet, SystemMessageFlags flags, MessageFlagAction flagsAction, Folder fld)
		{
			int[] ids = ObjectsToInts32(messageIndexSet);
			_dbMan.SetMessagesFlags(false, ids, flags, flagsAction, fld);
		}

		public override void SetMessagesFlags(SystemMessageFlags flags, MessageFlagAction flagsAction, Folder fld)
		{
			_dbMan.SetMessagesFlags(true, null, flags, flagsAction, fld);
		}

		public override void DeleteMessages(object[] messageIndexSet, Folder fld)
		{
			int[] ids = ObjectsToInts32(messageIndexSet);
			_dbMan.DeleteMessages(ids);

			FileSystem fs = new FileSystem(_account.Email, _account.ID, true);
			fs.DeleteMessages(ids, fld.GetFullPath(_account.Delimiter));
			_account.Update(false);
		}

		public override void SaveMessage(WebMailMessage message, Folder fld)
		{
			_dbMan.SaveMessage(-1, message, fld);
		}

		public virtual void SaveMessage(int id_msg, WebMailMessage message, Folder fld)
		{
			_dbMan.SaveMessage(id_msg, message, fld);
		}

		public override void SaveMessages(WebMailMessageCollection messages, Folder fld)
		{
			_dbMan.SaveMessages(messages, fld);
		}

		public virtual void UpdateMessage(WebMailMessage message)
		{
			_dbMan.UpdateMessage(message);
		}

		public virtual WebMailMessageCollection SearchMessages(int page, string condition, FolderCollection folders, bool inHeadersOnly, out int searchMessagesCount)
		{
			return _dbMan.SearchMessages(page, condition, folders, inHeadersOnly, out searchMessagesCount);
		}

		public virtual WebMailMessageCollection SearchMessages(string condition, FolderCollection folders, bool inHeadersOnly)
		{
			return _dbMan.SearchMessages(condition, folders, inHeadersOnly);
		}

        public virtual List<long> SearchMessagesIntUids(string condition, Folder folder, bool inHeadersOnly)
        {
            return _dbMan.SearchMessagesIntUids(condition, folder, inHeadersOnly);
        }

        public virtual string[] GetUids()
		{
			return _dbMan.SelectUids();
		}

        public virtual string[] GetUids(Folder folder)
        {
            return _dbMan.SelectUids(folder);
        }

        public virtual void CreateDatabase(string databaseName)
        {
            _dbMan.CreateDatabase(databaseName);
        }

        public virtual void CreateTable(string tableName, string prefix)
		{
			_dbMan.CreateTable(tableName, prefix);
		}

        public virtual void DropTable(string tableName, string prefix)
        {
            _dbMan.DropTable(tableName, prefix);
        }
        
        public virtual void AlterTable(string tableName, string field, string prefix)
		{
			_dbMan.AlterTable(tableName, field, prefix);
		}

		public virtual void CreateIndex(string prefix, string sufix, string column)
		{
			_dbMan.CreateIndex(prefix, sufix, column);
		}

		public virtual string[] GetTablesNames()
		{
			return _dbMan.SelectTablesNames();
		}

		public virtual object[] GetOldMessagesUids(int daysCount)
		{
			Folder fld = GetFolder(FolderType.Inbox);
			WebMailMessageCollection coll = _dbMan.SelectMessagesOlderThanXDays(daysCount, fld);
			return coll.ToUidsCollection(true);
		}

		public virtual long GetImapLastMessageUid(Folder fld)
		{
			return _dbMan.SelectImapLastMessageUid(fld);
		}

		public virtual int GetLastMsgID()
		{
			return _dbMan.GetMessageID();
		}

		public virtual void SetSender(string email, byte safety)
		{
			_dbMan.SetSender(email, safety);
		}

		public virtual byte GetSenderSafety(string email, int userId)
		{
			return _dbMan.GetSenderSafety(email, userId);
		}

		public virtual UserColumn CreateUserColumn(int id_column, int id_user, int value)
		{
			return _dbMan.CreateUserColumn(id_column, id_user, value);
		}

		public virtual void DeleteUserColumn(UserColumn column)
		{
			_dbMan.DeleteUserColumn(column);
		}

		public virtual UserColumn[] GetUserColumns(int id_user)
		{
			return _dbMan.SelectUserColumns(id_user);
		}

		public virtual void UpdateUserColumn(int id_column, int id_user, int value)
		{
			_dbMan.UpdateUserColumn(id_column, id_user, value);
		}

		public virtual void UpdateAccountDefOrder(int def_order)
		{
			_dbMan.UpdateAccountDefOrder(_account, def_order);
		}

		public virtual TempRow CreateTempRow(int id_acct, string data_val)
		{
            return _dbMan.CreateTempRow(id_acct, data_val);		
		}

		public virtual void DeleteTempRow(long id_temp)
		{
			_dbMan.DeleteTempRow(id_temp);		
		}

		public virtual TempRow GetTempRow(long id_temp, string data_val)
		{
			TempRow[] rows = _dbMan.SelectTempRow(id_temp, data_val);
			if ((rows != null) && (rows.Length > 0))
			{
				return rows[0];
			}
			return null;
		}

		internal static int ObjectToInt32(object id)
		{
			int result = -1;
			try
			{
				result = Convert.ToInt32(id, CultureInfo.InvariantCulture);
			}
			catch (FormatException ex)
			{
				Log.WriteLine("ObjectToInt", "Invalid Int32.");
				Log.WriteException(ex);
			}
			return result;
		}

		internal static int[] ObjectsToInts32(object[] ids)
		{
			int[] result = new int[ids.Length];
			for(int i = 0; i < ids.Length; i++)
			{
				int j = ObjectToInt32(ids[i]);
				if (j > 0) result[i] = j;
			}
			return result;
		}

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
					_dbMan.Disconnect();
				}
             
				// Call the appropriate methods to clean up 
				// unmanaged resources here.
				// If disposing is false, 
				// only the following code is executed.
			}
			_disposed = true;         
		}

		#endregion

		public void ReplaceUids(object[] pop3Uids)
		{
			_dbMan.DeletePop3Uids();
			_dbMan.SavePop3Uids(pop3Uids);
		}

		public void SaveUids(object[] uids)
		{
			_dbMan.SavePop3Uids(uids);
		}

		public AddressBookGroup GetAddressBookGroup(string group_uid)
		{
			return _dbMan.SelectAddressBookGroup(group_uid);
		}

		public AddressBookContact GetAddressBookContact(int user_id, string str_id)
		{
			AddressBookContact[] contacts = _dbMan.SelectAddressBookContacts(user_id, str_id);
			return (contacts.Length > 0) ? contacts[0] : null;
		}
		
        public virtual void UpdateActiveCalendars()
        {
            int cnt = _dbMan.GetActiveCalendarsCount();
            if (cnt == 0)
            {
                _dbMan.UpdateActiveCalendars();                
            }
        }
	}// END CLASS DEFINITION DbStorage

    public class DbStorageCreator
    {
        public static DbStorage CreateDatabaseStorage(Account acct)
        {
            WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
            switch (settings.DbType)
            {
                case SupportedDatabase.MsAccess:
                    return new MsAccessStorage(acct);
                case SupportedDatabase.MySql:
                    return new MySqlStorage(acct);
                case SupportedDatabase.MsSqlServer:
                    return new MsSqlStorage(acct);
                case SupportedDatabase.PostgreSql:
                    return new PostgreSqlStorage(acct);
                default:
                    return null;
            }
        }

        public static DbStorage CreateDatabaseStorage(Account acct, string dataFolderPath)
        {
            WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings(dataFolderPath);
            switch (settings.DbType)
            {
                case SupportedDatabase.MsAccess:
                    return new MsAccessStorage(acct);
                case SupportedDatabase.MySql:
                    return new MySqlStorage(acct);
                case SupportedDatabase.MsSqlServer:
                    return new MsSqlStorage(acct);
                case SupportedDatabase.PostgreSql:
                    return new PostgreSqlStorage(acct);
                default:
                    return null;
            }
        }

    }
}
