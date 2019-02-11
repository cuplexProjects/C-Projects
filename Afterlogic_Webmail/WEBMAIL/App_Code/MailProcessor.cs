using System.Globalization;
using MailBee.ImapMail;
using System.Collections.Generic;
using MailBee.Mime;

namespace WebMail
{
	/// <summary>
	/// Summary description for MailProcessor.
	/// </summary>
	public class MailProcessor
	{
		public event DownloadedMessageHandler MessageDownloaded;
		public event DeleteMessageHanlder MessageDeleted;


		protected DbStorage _dbStorage;
		protected MailServerStorage _serverStorage;

        protected bool _isMoveError = false;

        public bool IsMoveError
        {
            get { return _isMoveError; }
        }

        private MailProcessor()
		{
			_dbStorage = null;
		}

		public MailProcessor(DbStorage storage) : this()
		{
			_dbStorage = storage;
		}

		public Account MailAccount
		{
			get { return _dbStorage.Account; }
		}

		public DbStorage DatabaseStorage
		{
			get { return _dbStorage; }
			//set { _dbStorage = value; }
		}

		public MailServerStorage ServerStorage
		{
			get { return _serverStorage; }
		}

		protected virtual void OnMessageDownloaded(CheckMailEventArgs e) 
		{
			if (MessageDownloaded != null)
				MessageDownloaded(this, e);
		}

		protected virtual void OnMessageDeleted(DeleteMessageEventArgs e)
		{
			if (MessageDeleted != null)
				MessageDeleted(this, e);
		}

		public void Connect()
		{
			_dbStorage.Connect();
		}

		public void Disconnect()
		{
			if (_dbStorage.IsConnected()) _dbStorage.Disconnect();
		}

		public Folder CreateFolder(int id_parent, string parent_full_name, string name, int create)
		{
			WebmailResourceManager resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
            WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();

			if (string.IsNullOrEmpty(name))
			{
				throw new WebMailException(resMan.GetString("WarningEmptyFolderName"));
			}
			if (!FileSystem.IsFolderNameValid(name))
			{
				throw new WebMailIOException(resMan.GetString("WarningCorrectFolderName"));
			}

			Folder fld = new Folder();
			fld.IDAcct = _dbStorage.Account.ID;
			fld.IDParent = id_parent;
			fld.Name = name;
			fld.FullPath = (!string.IsNullOrEmpty(parent_full_name)) ? string.Format("{0}{1}{2}", parent_full_name, _dbStorage.Account.Delimiter, name) : name;
			//only in webmail
			if (create == 0) fld.SyncType = FolderSyncType.DontSync;
			//in webmail and mail server
            if ((create == 1 && _dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.Imap4) || (_dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.WMServer))
			{
                fld.SyncType = FolderSyncType.AllHeadersOnly;
                if (_dbStorage.Account.UserOfAccount.Settings.AllowDirectMode && settings.DirectModeIsDefault)
                {
                    fld.SyncType = FolderSyncType.DirectMode;
                }

				_serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
				ImapStorage imapStorage = _serverStorage as ImapStorage;
				if (imapStorage != null)
				{
					try
					{
						imapStorage.Connect();
						imapStorage.CreateFolder(fld);
					}
					finally
					{
						imapStorage.Disconnect();
					}
				}
                WMServerStorage wmStorage = _serverStorage as WMServerStorage;
                if (wmStorage != null)
                {
                    fld.FullPath = (!string.IsNullOrEmpty(parent_full_name)) ? string.Format("{0}{1}{2}", parent_full_name, _dbStorage.Account.Delimiter, name) : string.Format("{0}{1}", _dbStorage.Account.Delimiter, name);
                    try
                    {
                        wmStorage.Connect();
                        wmStorage.CreateFolder(fld);
                    }
                    finally
                    {
                        wmStorage.Disconnect();
                    }
                }

			}
            if (_dbStorage.Account.MailIncomingProtocol != IncomingMailProtocol.WMServer)
            {
                FileSystem fs = new FileSystem(_dbStorage.Account.Email, _dbStorage.Account.ID, true);
                fs.CreateFolder(fld.GetFullPath(_dbStorage.Account.Delimiter));
            }
			return _dbStorage.CreateFolder(fld);
		}

		public void DeleteFolders(Folder[] folders)
		{
			_serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
			ImapStorage imapStorage = _serverStorage as ImapStorage;
            WMServerStorage wmserverStorage = _serverStorage as WMServerStorage;
			if (imapStorage != null)
			{
				try
				{
					imapStorage.Connect();
					foreach (Folder imapFld in folders)
					{
                        Folder tmp = _dbStorage.GetFolder(imapFld.ID);
                        if (tmp.SyncType != FolderSyncType.DontSync)
                        {
                            imapStorage.DeleteFolder(tmp);
                        }
					}
				}
				finally
				{
					imapStorage.Disconnect();
				}
			}

            if (wmserverStorage != null)
            {
                try
                {
                    wmserverStorage.Connect();
                    foreach (Folder wmserver in folders)
                    {
                        Folder tmp = _dbStorage.GetFolder(wmserver.ID);
                        if (tmp.SyncType != FolderSyncType.DontSync)
                        {
                            wmserverStorage.DeleteFolder(tmp);
                        }
                    }
                }
                finally
                {
                    wmserverStorage.Disconnect();
                }
            }
            FileSystem fs = new FileSystem(_dbStorage.Account.Email, _dbStorage.Account.ID, true);
			foreach (Folder dbFld in folders)
			{
                if (_dbStorage.Account.MailIncomingProtocol != IncomingMailProtocol.WMServer)
                {
                    fs.DeleteFolder(dbFld.GetFullPath(_dbStorage.Account.Delimiter));
                }
				_dbStorage.DeleteFolder(dbFld);
			}
		}

		public void UpdateFolders(Folder[] folders, bool updateRemoteFolders)
		{
			FileSystem fs = new FileSystem(_dbStorage.Account.Email, _dbStorage.Account.ID, true);
			foreach (Folder dbFld in folders)
			{
				WebmailResourceManager resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
				if (string.IsNullOrEmpty(dbFld.Name))
				{
					throw new WebMailException(resMan.GetString("WarningEmptyFolderName"));
				}
				if (!FileSystem.IsFolderNameValid(dbFld.UpdateName))
				{
					throw new WebMailIOException(resMan.GetString("WarningCorrectFolderName"));
				}

                if (_dbStorage.Account.MailIncomingProtocol != IncomingMailProtocol.WMServer)
                {
                    string delimiter = _dbStorage.Account.Delimiter;
                    fs.RenameFolder(dbFld.GetFullPath(delimiter), dbFld.GetUpdateFullPath(delimiter));
                }
				_dbStorage.UpdateFolder(dbFld);
			}
			if (updateRemoteFolders)
			{
				_serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
				ImapStorage imapStorage = _serverStorage as ImapStorage;
                WMServerStorage wmServerStorage = _serverStorage as WMServerStorage;
                if (imapStorage != null)
				{
					try
					{
						imapStorage.Connect();
						foreach (Folder imapFld in folders)
						{
							if (imapFld.SyncType != FolderSyncType.DontSync)
							{
								bool exists = false;
								FolderCollection fc = _serverStorage.GetFolders();
								if (fc != null)
								{
                                    FolderCollection tmp_fc = new FolderCollection();
                                    FolderCollection.CreateFolderListFromTree(ref tmp_fc, fc);
                                    foreach (Folder f in tmp_fc)
									{
										if (string.Compare(f.FullPath, imapFld.FullPath, true, CultureInfo.InvariantCulture) == 0)
										{
											exists = true;
											break;
										}
									}
								}
								if (!exists)
								{
									imapStorage.CreateFolder(imapFld);
								}
								else
								{
									imapStorage.UpdateFolder(imapFld);
								}
							}
						}
					}
					finally
					{
						imapStorage.Disconnect();
					}
				}
                if (wmServerStorage != null)
                {
                    try
                    {
                        wmServerStorage.Connect();
                        foreach (Folder imapFld in folders)
                        {
                            if (imapFld.SyncType != FolderSyncType.DontSync)
                            {
                                bool exists = false;
                                FolderCollection fc = _serverStorage.GetFolders();
                                if (fc != null)
                                {
                                    FolderCollection tmp_fc = new FolderCollection();
                                    FolderCollection.CreateFolderListFromTree(ref tmp_fc, fc);
                                    foreach (Folder f in tmp_fc)
                                    {
                                        if (string.Compare(f.FullPath, imapFld.FullPath, true, CultureInfo.InvariantCulture) == 0)
                                        {
                                            exists = true;
                                            break;
                                        }
                                    }
                                }
                                if (!exists)
                                {
                                    wmServerStorage.CreateFolder(imapFld);
                                }
                                else
                                {
                                    wmServerStorage.UpdateFolder(imapFld);
                                }
                            }
                        }
                    }
                    finally
                    {
                        wmServerStorage.Disconnect();
                    }
                }
            }
		}

		public void SendMail(WebMailMessage message)
		{
            if (_dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.WMServer)
            {
                SmtpXMail.SendMail(_dbStorage.Account, message);
            }
            else
            {
                Smtp.SendMail(_dbStorage.Account, message);
            }
		}

        public Dictionary<long, string> Synchronize(FolderCollection folders)
		{
			_serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
            Dictionary<long, string> updatedFolders = new Dictionary<long,string>();
			if (_serverStorage != null)
			{
				try
				{
					_serverStorage.MessageDownloaded += new DownloadedMessageHandler(_serverStorage_MessageDownloaded);
					_serverStorage.Connect();
					updatedFolders = _serverStorage.Synchronize(folders);
				}
				finally
				{
					_serverStorage.MessageDownloaded -= new DownloadedMessageHandler(_serverStorage_MessageDownloaded);
					_serverStorage.Disconnect();
				}
			}
            return updatedFolders;
        }

		public void SynchronizeFolders()
		{
            if ((_dbStorage.Account != null) && (_dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.Imap4 || _dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.WMServer))
			{
				_serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
                WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
				if (_serverStorage != null)
				{
					FolderCollection serverFoldersTree = null;
					FolderCollection serverFoldersList = new FolderCollection();
					try
					{
						_serverStorage.Connect();
						serverFoldersTree = _serverStorage.GetFolders();
					}
					finally
					{
						_serverStorage.Disconnect();
					}
					if (serverFoldersTree != null)
					{
						FolderCollection.CreateFolderListFromTree(ref serverFoldersList, serverFoldersTree);
						FolderCollection dbFoldersTree = _dbStorage.GetFolders();
						FolderCollection dbFoldersList = new FolderCollection();
						FolderCollection.CreateFolderListFromTree(ref dbFoldersList, dbFoldersTree);
						foreach (Folder fld in serverFoldersList)
                        {
							Folder dbServerFolder = dbFoldersList[fld.FullPath];
							if (dbServerFolder != null)
							{
								if (dbServerFolder.Hide != fld.Hide && fld.Type == FolderType.Custom)
								{
									dbServerFolder.Hide = fld.Hide;
									UpdateFolders(new Folder[] { dbServerFolder }, false);
								}
								continue;
							}
                        	string delimiter = _dbStorage.Account.Delimiter;
                        	string folderFullName = fld.FullPath;
                        	if (_dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.WMServer)
                        		folderFullName = folderFullName.TrimStart(_dbStorage.Account.Delimiter.ToCharArray());
                        	int startIndex = 0;
                        	do
                        	{
                        		string newFolderFullName;
                        		startIndex = folderFullName.IndexOf(delimiter, startIndex);
                        		if (startIndex < 0)		
                        		{
                        			newFolderFullName = folderFullName.Substring(0, folderFullName.Length);
                        		}
                        		else
                        		{
                        			newFolderFullName = folderFullName.Substring(0, startIndex);
                        			++startIndex;
                        		}
                        		if (_dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.WMServer)
                        			newFolderFullName = _dbStorage.Account.Delimiter + newFolderFullName;

                        		Folder existFld = dbFoldersList[newFolderFullName];
                        		if (existFld != null)
                        		{
                        			fld.IDParent = existFld.ID;
                        			continue;
                        		}
                        		if (_dbStorage.Account.UserOfAccount.Settings.AllowDirectMode && settings.DirectModeIsDefault)
                        		{
                        			fld.SyncType = FolderSyncType.DirectMode;
                        		}
                        		dbFoldersList.Add(_dbStorage.CreateFolder(fld));
                        	} while (startIndex > 0);
                        }
						foreach (Folder fld in dbFoldersList)
						{
							if (serverFoldersList[fld.FullPath] != null)
							{
								continue;
							}
                            if (_dbStorage.Account.MailIncomingProtocol != IncomingMailProtocol.WMServer)
                            {
                                fld.SyncType = FolderSyncType.DontSync;
                                UpdateFolders(new Folder[] { fld }, false);
                            }
                            else
                            {
                                _dbStorage.DeleteFolder(fld);
                            }
						}
					}
				}
			}
		}

		public WebMailMessageCollection GetMessageHeaders(int pageNumber, Folder fld)
		{
			if (fld.SyncType == FolderSyncType.DirectMode)
			{
				_serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
				try
				{
					_serverStorage.Connect();
					return _serverStorage.LoadMessageHeaders(pageNumber, fld);
				}
				finally
				{
					_serverStorage.Disconnect();
				}
			}
			return _dbStorage.LoadMessageHeaders(pageNumber, fld);
		}

        public WebMailMessage GetMessage(object index, Folder fld)
        {
            return GetMessage(index, fld, false, MessageMode.None);
        }
        
        public WebMailMessage GetMessage(object index, Folder fld, bool body_structure, MessageMode mode)
		{
			if (fld.SyncType == FolderSyncType.DirectMode)
			{
				_serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
				try
				{
					_serverStorage.Connect();
					WebMailMessage serverMsg = _serverStorage.LoadMessage(index, fld, body_structure, mode);
					if (serverMsg != null)
					{
						ImapStorage imapStorage = _serverStorage as ImapStorage;
						if (imapStorage != null)
						{
							imapStorage.SetMessagesFlags(new object[] { index },  SystemMessageFlags.Seen, MessageFlagAction.Add, fld);
						}
					}
					return serverMsg;
				}
				finally
				{
					_serverStorage.Disconnect();
				}
			}

			WebMailMessage msg = _dbStorage.LoadMessage(index, fld);
			if (msg != null)
			{
				if (!msg.Downloaded)
				{
					// load message from mail server
					_serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
					try
					{
						_serverStorage.Connect();
                        object uid = (!string.IsNullOrEmpty(msg.StrUid)) ? (object)msg.StrUid : msg.IntUid;
                        msg.Flags = msg.Flags | SystemMessageFlags.Seen;
                        _serverStorage.SetMessagesFlags(new object[] { uid }, SystemMessageFlags.Seen, MessageFlagAction.Add, fld);
                        WebMailMessage serverMsg = _serverStorage.LoadMessage(uid, fld, body_structure, mode);
                        if (serverMsg != null)
						{
                            msg.Init(serverMsg.MailBeeMessage, (!string.IsNullOrEmpty(msg.StrUid)), fld);
                        }
					}
					finally
					{
						_serverStorage.Disconnect();
					}
				}
			}
			return msg;
		}

		public WebMailMessageCollection GetMessages(int id_acct, int[] id_msgs, Folder fld, bool body_structure, MessageMode mode)
		{
			if (fld.SyncType == FolderSyncType.DirectMode)
			{
				return null;
			}

			WebMailMessageCollection msgs = _dbStorage.LoadMessages(id_acct, id_msgs, fld);
            if (msgs != null && (_dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.WMServer || _dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.Imap4))
			{
				foreach (WebMailMessage msg in msgs)
				{
					if (!msg.Downloaded)
					{
						// load message from mail server
						_serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
						try
						{
							_serverStorage.Connect();
                            bool _body_structure = (body_structure && msg.Size > Constants.IMAP_OPT_MAIL_SIZE) ? true : false;
                            WebMailMessage serverMsg = _serverStorage.LoadMessage(_dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.WMServer ? msg.StrUid : msg.IntUid.ToString(), fld, _body_structure, mode);

                            if (serverMsg != null)
							{
								msg.Init(serverMsg.MailBeeMessage, true, fld);
							}
						}
						finally
						{
							_serverStorage.Disconnect();
						}
					}
				}
			}
			return msgs;
		}

        public WebMailMessageCollection GetMessages(object[] messageIndexSet, bool indexIsUid, Folder fld, bool body_structure, MessageMode mode, XMLPacketMessagesBody[] messages)
        {
            if (fld.SyncType == FolderSyncType.DirectMode)
            {
                _serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
                try
                {
                    _serverStorage.Connect();
                    if (body_structure)
                    {
                        return _serverStorage.LoadMessages(messageIndexSet, fld, body_structure, mode, messages);
                    }
                    else
                    {
                        return _serverStorage.LoadMessages(messageIndexSet, fld, body_structure, mode);
                    }
                    
                }
                finally
                {
                    _serverStorage.Disconnect();
                }
            }
            return _dbStorage.LoadMessages(messageIndexSet, fld);
        }

        public byte[] GetAttachmentPart(object index, Folder fld, string partID)
        {
            _serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
            try
            {
                _serverStorage.Connect();
                return _serverStorage.LoadAttachmentPart(index, fld, partID);
            }
            finally
            {
                _serverStorage.Disconnect();
            }
        }
        
        public FolderCollection GetFolders()
		{
			return GetFolders(false);
		}

		public FolderCollection GetFolders(bool asList)
		{
			FolderCollection fc = _dbStorage.GetFolders();
			if (asList)
			{
				FolderCollection resultList = new FolderCollection();
				FolderCollection.CreateFolderListFromTree(ref resultList, fc);
				return resultList;
			}
			return fc;
		}

		public Folder GetFolder(string folderFullName)
		{
			return _dbStorage.GetFolder(folderFullName);
		}

		public Folder GetFolder(FolderType type)
		{
			return _dbStorage.GetFolder(type);
		}

		public Folder GetFolder(long id_folder)
		{
			return _dbStorage.GetFolder(id_folder);
		}

		public int GetFolderMessageCount(Folder fld)
		{
			if (fld.SyncType == FolderSyncType.DirectMode)
			{
				_serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
				try
				{
					Log.WriteLine(@"GetFolderMessageCount", "Connect");
					_serverStorage.Connect();
					return _serverStorage.GetFolderMessageCount(fld.FullPath);
				}
				finally
				{
					Log.WriteLine(@"GetFolderMessageCount", "Disconnect");
					_serverStorage.Disconnect();
				}
			}
			return fld.MessageCount;
		}

		public int GetFolderUnreadMessageCount(Folder fld)
		{
			if (fld.SyncType == FolderSyncType.DirectMode)
			{
				_serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
				if ((_serverStorage as Pop3Storage) != null) return 0; // we can't get that info from POP3
				try
				{
					Log.WriteLine(@"GetFolderUnreadMessageCount", "Connect");
					_serverStorage.Connect();
					return _serverStorage.GetFolderUnreadMessageCount(fld.FullPath);
				}
				finally
				{
					Log.WriteLine(@"GetFolderUnreadMessageCount", "Disconnect");
					_serverStorage.Disconnect();
				}
			}
			return fld.UnreadMessageCount;
		}

		public bool MoveMessages(object[] messageIndexSet, Folder fromFolder, Folder toFolder)
		{
            bool result = true;
            _serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
			ImapStorage imapStorage = _serverStorage as ImapStorage;
			object[] uids;
			if (imapStorage != null)
			{
				WebMailMessageCollection msgs = null;
				if (fromFolder.SyncType != FolderSyncType.DirectMode)
				{
					msgs = _dbStorage.LoadMessages(messageIndexSet, fromFolder);
					uids = msgs.ToUidsCollection(false);
				}
				else
				{
					uids = messageIndexSet;
				}
				try
				{
					imapStorage.Connect();
					//------------------------------
					FolderCollection fc = new FolderCollection();
					fc.Add(toFolder);
					if ((fromFolder.SyncType == FolderSyncType.DontSync) && (toFolder.SyncType == FolderSyncType.DontSync))
					{
						//1: DontSync	|	DB Update  (id_folder_db field), file move	|	DontSync
                        // executing below
					}
                    // from - DontSync
                    // to - NewHeadersOnly, AllHeadersOnly, NewEntireMessages, AllEntireMessages
                    else if ((fromFolder.SyncType == FolderSyncType.DontSync) && 
                        (toFolder.SyncType != FolderSyncType.DontSync) && (toFolder.SyncType != FolderSyncType.DirectMode))
					{
						//2: DontSync	|	SaveMessage(Append(ToFolder)) Db+File Delete(FromFolder) Sync(ToFolder)	|	Sync
                        try
                        {
                            imapStorage.SaveMessages(msgs, toFolder);
                        }
                        catch
                        {
                            return false;
                        }
                        PurgeMessages(messageIndexSet, fromFolder);
                        imapStorage.Synchronize(fc);
						return result;
					}
                    // from - DontSync
                    // to - DirectMode
                    else if ((fromFolder.SyncType == FolderSyncType.DontSync) && (toFolder.SyncType == FolderSyncType.DirectMode))
					{
						//3: DontSync	|	SaveMessage(Append(ToFolder)) Db+File Delete(FromFolder)	|	DirectMode
                        try
                        {
                            imapStorage.SaveMessages(msgs, toFolder);
                        }
                        catch
                        {
                            return false;
                        }
                        PurgeMessages(messageIndexSet, fromFolder);
						return result;
					}
					// from - NewHeadersOnly, AllHeadersOnly, NewEntireMessages, AllEntireMessages
					// to - DontSync
					else if ((fromFolder.SyncType != FolderSyncType.DontSync) && (fromFolder.SyncType != FolderSyncType.DirectMode) && (toFolder.SyncType == FolderSyncType.DontSync))
					{
						//4: Sync	|	GetMessage(ToFolder) Db+File+Imap Delete(FromFolder) Purge(FromFolder)	|	DontSync
						msgs = imapStorage.LoadMessages(uids, fromFolder);
                        foreach (WebMailMessage msg in msgs)
                        {
                            msg.Downloaded = true;
                        }
                        try
                        {
                            SaveMessages(msgs, toFolder);
                        }
                        catch
                        {
                            return false;
                        }
                        imapStorage.DeleteMessages(uids, fromFolder);
                        _dbStorage.DeleteMessages(messageIndexSet, fromFolder); 
//                        fc.Add(fromFolder);
//                        imapStorage.Synchronize(fc);
						return result;
					}
					// from - NewHeadersOnly, AllHeadersOnly, NewEntireMessages, AllEntireMessages
					// to - NewHeadersOnly, AllHeadersOnly, NewEntireMessages, AllEntireMessages
					else if ((fromFolder.SyncType != FolderSyncType.DontSync) && (fromFolder.SyncType != FolderSyncType.DirectMode) && (toFolder.SyncType != FolderSyncType.DontSync) && (toFolder.SyncType != FolderSyncType.DirectMode))
					{
						//5: Sync	|	ImapMove Db+File+Imap Delete(FromFolder) Purge(FromFolder) Sync(ToFolder)	|	Sync
                        try
                        {
                            imapStorage.MoveMessages(uids, fromFolder, toFolder);
                        }
                        catch
                        {
                            return false;
                        }
						if (fromFolder.SyncType == FolderSyncType.NewHeadersOnly || fromFolder.SyncType == FolderSyncType.NewEntireMessages)
						{
                            imapStorage.DeleteMessages(messageIndexSet, fromFolder);
                            _dbStorage.DeleteMessages(messageIndexSet, fromFolder);
						}
						else
						{
							fc.Add(fromFolder);
						}
						imapStorage.Synchronize(fc);
						return result;
					}
					// from - NewHeadersOnly, AllHeadersOnly, NewEntireMessages, AllEntireMessages
					// to - DirectMode
					else if ((fromFolder.SyncType != FolderSyncType.DontSync) && (fromFolder.SyncType != FolderSyncType.DirectMode) && (toFolder.SyncType == FolderSyncType.DirectMode))
					{
						//6: Sync	|	ImapMove Db+File+Imap Delete(FromFolder) Purge(FromFolder)	|	DirectMode
                        try
                        {
                            imapStorage.MoveMessages(uids, fromFolder, toFolder);
                        }
                        catch
                        {
                            return false;
                        }
						if (fromFolder.SyncType == FolderSyncType.NewHeadersOnly || fromFolder.SyncType == FolderSyncType.NewEntireMessages)
						{
                            imapStorage.DeleteMessages(messageIndexSet, fromFolder);
						}
						else
						{
							fc.Add(fromFolder);
						}
						imapStorage.Synchronize(fc);
						return result;
					}
					// from - DirectMode
					// to - DontSync
					else if ((fromFolder.SyncType == FolderSyncType.DirectMode) && (toFolder.SyncType == FolderSyncType.DontSync))
					{
						//7: DirectMode	|	GetMessage(ToFolder) Imap Delete(FromFolder) Purge(FromFolder)	|	DontSync
						WebMailMessageCollection coll = imapStorage.LoadMessages(messageIndexSet, fromFolder);
                        try
                        {
                            SaveMessages(coll, toFolder);
                        }
                        catch
                        {
                            return false;
                        }
                        imapStorage.DeleteMessages(messageIndexSet, fromFolder);
						return result;
					}
                    // from - DirectMode
                    // to - NewHeadersOnly, AllHeadersOnly, NewEntireMessages, AllEntireMessages
                    else if ((fromFolder.SyncType == FolderSyncType.DirectMode) && (toFolder.SyncType != FolderSyncType.DontSync) && (toFolder.SyncType != FolderSyncType.DirectMode))
					{
						//8: DirectMode	|	ImapMove Imap Delete(FromFolder) Purge(FromFolder) Sync(ToFolder)	|	Sync
                        try
                        {
                            imapStorage.MoveMessages(uids, fromFolder, toFolder);
                        }
                        catch
                        {
                            return false;
                        }
                        imapStorage.DeleteMessages(messageIndexSet, fromFolder);
                        imapStorage.Synchronize(fc);
						return result;
					}
                    // from - DirectMode
                    // to - DirectMode
                    else if ((fromFolder.SyncType == FolderSyncType.DirectMode) && (toFolder.SyncType == FolderSyncType.DirectMode))
					{
						//9: DirectMode	|	ImapMove	|	DirectMode
                        try
                        {
                            imapStorage.MoveMessages(uids, fromFolder, toFolder);
                        }
                        catch
                        {
                            return false;
                        }
						return result;
					}
					//------------------------------
				}
				finally
				{
					imapStorage.Disconnect();
				}
			}
            if (fromFolder.SyncType != FolderSyncType.DirectMode)
            {
                if (_dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.WMServer)
                {
                    WMServerStorage wmserverStorage = _serverStorage as WMServerStorage;

                    if (wmserverStorage != null)
                    {
                        /* movement on the file system and database */
                        wmserverStorage.MoveMessages(messageIndexSet, fromFolder, toFolder);
                    }
                }
                else
                {
					_dbStorage.MoveMessages(messageIndexSet, fromFolder, toFolder);
					FileSystem fs = new FileSystem(_dbStorage.Account.Email, _dbStorage.Account.ID, true);
                    string delimiter = _dbStorage.Account.Delimiter;
                    string fromFolderName = fromFolder.GetFullPath(delimiter);
                    string toFolderName = toFolder.GetFullPath(delimiter);
                    fs.MoveMessages(DbStorage.ObjectsToInts32(messageIndexSet), fromFolderName, toFolderName);
                }
            }
            else
            {
                if (_dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.Pop3)
                {
                    if (toFolder.SyncType == FolderSyncType.DontSync)
                    {
                        Pop3Storage pop3Storage = _serverStorage as Pop3Storage;
                    	WebMailMessageCollection coll;
						try
						{
							pop3Storage.Connect();
							coll = pop3Storage.LoadMessages(messageIndexSet, fromFolder);
						}
						finally
						{
							pop3Storage.Disconnect();
						}

                        try
                        {
                            SaveMessages(coll, toFolder);
                        }
                        catch
                        {
                            return false;
                        }
                        DeleteMessages(messageIndexSet, fromFolder);
                    }
                }
                else if (_dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.WMServer)
                {
                    if (toFolder.SyncType == FolderSyncType.DontSync)
                    {
                        WMServerStorage wmStorage = _serverStorage as WMServerStorage;

                        wmStorage.Connect();
                        WebMailMessageCollection coll = wmStorage.LoadMessages(messageIndexSet, fromFolder);
                        try
                        {
                            SaveMessages(coll, toFolder);
                        }
                        catch
                        {
                            return false;
                        }
                        DeleteMessages(messageIndexSet, fromFolder);
                    }
                }
            }

            return result;
		}

		public virtual void SpamMessage(object[] messageIndexSet, Folder fld)
		{
			if (_dbStorage.Account != null)
			{
                _serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
                Folder spamFld = _dbStorage.GetFolder(FolderType.Spam);

                if (_dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.WMServer)
                {
                    if (spamFld == null)
                    {
                        WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
                        FileSystem fs = new FileSystem(_dbStorage.Account.Email, _dbStorage.Account.ID, true);
                        if (!settings.StoreMailsInDb) fs.CreateFolder(Constants.FolderNames.Spam);
                        spamFld = _dbStorage.CreateFolder(new Folder(0, _dbStorage.Account.ID, -1, Constants.FolderNames.Spam, _dbStorage.Account.Delimiter + Constants.FolderNames.Spam, FolderType.Spam, FolderSyncType.AllHeadersOnly, false, 4));
                    }

                    WMServerStorage wmStorage = _serverStorage as WMServerStorage;
                    FolderCollection serverFlds = wmStorage.GetFolders();
                    if (serverFlds[FolderType.Spam] == null)
                    {
                        wmStorage.CreateFolder(spamFld);
                    }

                    MoveMessages(messageIndexSet, fld, spamFld);

                    WebMailMessageCollection msgs = _dbStorage.LoadMessages(messageIndexSet, spamFld);
                    foreach (WebMailMessage msg in msgs)
                    {
                        wmStorage.SpamMessage(msg.StrUid, spamFld);
                    }
                }
                else
                {
                    if (spamFld == null)
                    {
                        WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
                        FileSystem fs = new FileSystem(_dbStorage.Account.Email, _dbStorage.Account.ID, true);
                        if (!settings.StoreMailsInDb) fs.CreateFolder(Constants.FolderNames.Spam);
                        spamFld = _dbStorage.CreateFolder(new Folder(0, _dbStorage.Account.ID, -1, Constants.FolderNames.Spam, _dbStorage.Account.Delimiter + Constants.FolderNames.Spam, FolderType.Spam, FolderSyncType.DontSync, false, 4));
                    }
                    MoveMessages(messageIndexSet, fld, spamFld);
                }
			}
		}

		public virtual void NotSpamMessage(object[] messageIndexSet, Folder fld)
		{
			if (_dbStorage.Account != null)
			{
				Folder inboxFld = _dbStorage.GetFolder(FolderType.Inbox);
				MoveMessages(messageIndexSet, fld, inboxFld);

                if (_dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.WMServer)
                {
                    WebMailMessageCollection msgs = _dbStorage.LoadMessages(messageIndexSet, inboxFld);

                    _serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
                    WMServerStorage wmStorage = _serverStorage as WMServerStorage;
                    foreach (WebMailMessage msg in msgs)
                    {
                        wmStorage.NotSpamMessage(msg.StrUid, inboxFld);
                    }
                }
			}
		}

        public void SetForwardedFlag(object[] messageIndexSet, Folder fld)
        {
            _serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);

            if (fld.SyncType != FolderSyncType.DirectMode)
            {
                _dbStorage.SetMessagesFlagForwarded(messageIndexSet, fld);
            }
        }
        
        public void SetFlags(object[] messageIndexSet, SystemMessageFlags flags, MessageFlagAction flagsAction, Folder fld)
		{
			_serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
			ImapStorage imapStorage = _serverStorage as ImapStorage;
			object[] uids;
			if (imapStorage != null)
			{
				if (fld.SyncType != FolderSyncType.DirectMode)
				{
					WebMailMessageCollection dbMsgs = _dbStorage.LoadMessages(messageIndexSet, fld);
					uids = dbMsgs.ToUidsCollection(false);
				}
				else
				{
					uids = messageIndexSet;
				}
				try
				{
					imapStorage.Connect();
					imapStorage.SetMessagesFlags(uids, flags, flagsAction, fld);
				}
				finally
				{
					imapStorage.Disconnect();
				}
			}
			if (fld.SyncType != FolderSyncType.DirectMode)
			{
				_dbStorage.SetMessagesFlags(messageIndexSet, flags, flagsAction, fld);
                if (_dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.WMServer)
                {
                    _serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
                    WMServerStorage wmserverStorage = _serverStorage as WMServerStorage;
                    if (wmserverStorage != null)
                    {
                        WebMailMessageCollection dbMsgs = _dbStorage.LoadMessages(messageIndexSet, fld);
                        uids = dbMsgs.ToUidsCollection(true);
                        
                        try
                        {
                            wmserverStorage.Connect();
                            wmserverStorage.SetMessagesFlags(uids, flags, flagsAction, fld);
                        }
                        finally
                        {
                            wmserverStorage.Disconnect();
                        }
                    }
                }

			}
		}

		public void SetFlags(SystemMessageFlags flags, MessageFlagAction flagsAction, Folder fld)
		{
			if (fld.SyncType == FolderSyncType.DirectMode)
			{
				_serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
				ImapStorage imapStorage = _serverStorage as ImapStorage;
				if (imapStorage != null)
				{
					try
					{
						imapStorage.Connect();
						imapStorage.SetMessagesFlags(flags, flagsAction, fld);
					}
					finally
					{
						imapStorage.Disconnect();
					}
				}
			}
			else
			{
				_dbStorage.SetMessagesFlags(flags, flagsAction, fld);
                if (_dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.WMServer)
                {
                    _serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
                    WMServerStorage wmserverStorage = _serverStorage as WMServerStorage;
                    if (wmserverStorage != null)
                    {
                        try
                        {
                            wmserverStorage.Connect();
                            wmserverStorage.SetMessagesFlags(flags, flagsAction, fld);
                        }
                        finally
                        {
                            wmserverStorage.Disconnect();
                        }
                    }
                }
			}
		}

        /*
         * 1. for DIRECT MODE deletes messages from the mail server
         * 2. to the TRASH folder and SPAM, if the protocol POP3, WMSERVER or (IMAP4 and settings.Imap4DeleteLikePop3 = true), is PurgeMessages
         * 3. for the other folders, if the protocol WMSERVER or (IMAP4 and settings.Imap4DeleteLikePop3 = true), the letter moves in trash
         * 4. if (IMAP4 and settings.Imap4DeleteLikePop3 == false) is PurgeMessages
         */
        public void DeleteMessages(object[] messageIndexSet, Folder fld)
		{
            WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
            			
            if (fld.SyncType == FolderSyncType.DirectMode && _dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.Pop3)
            {
                _serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
                try
                {
                    _serverStorage.Connect();
                    _serverStorage.DeleteMessages(messageIndexSet, fld);
                    return;
                }
                finally
                {
                    _serverStorage.Disconnect();
                }
            }
            
            if (_dbStorage.Account != null)
			{
                if (_dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.Pop3 || 
                    _dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.WMServer ||
                    _dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.Imap4 && settings.Imap4DeleteLikePop3)
				{
                    if (fld.Type == FolderType.Trash || fld.Type == FolderType.Spam)
					{
						PurgeMessages(messageIndexSet, fld);
					}
					else
					{
						Folder trash = GetFolder(FolderType.Trash);
                        if (_dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.Imap4 || _dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.WMServer)
                        {
                            if (trash == null) trash = GetFolder(Constants.FolderNames.Trash);
                            if (trash == null)
                            {
                                Folder inbox = GetFolder(FolderType.Inbox);
                                trash = GetFolder(inbox.FullPath + _dbStorage.Account.Delimiter + Constants.FolderNames.Trash);
                            }
                            DbManagerCreator creator = new DbManagerCreator();
                            DbManager dbMan = creator.CreateDbManager();
                            try
                            {
                                dbMan.Connect();
                                if (trash == null)
                                {
                                    FileSystem fs = new FileSystem(_dbStorage.Account.Email, _dbStorage.Account.ID, true);
                                    if (!settings.StoreMailsInDb) fs.CreateFolder(Constants.FolderNames.Trash);
                                    if (_dbStorage.Account.MailIncomingProtocol != IncomingMailProtocol.WMServer)
                                    {
                                        trash = dbMan.CreateFolder(_dbStorage.Account.ID, -1, FolderType.Trash, Constants.FolderNames.Trash,
                                            Constants.FolderNames.Trash, FolderSyncType.DontSync, false, 3);
                                    }
                                    else
                                    {
                                        trash = dbMan.CreateFolder(_dbStorage.Account.ID, -1, FolderType.Trash, Constants.FolderNames.Trash,
                                            _dbStorage.Account.Delimiter + Constants.FolderNames.Trash, FolderSyncType.AllHeadersOnly, false, 3);
                                        if (trash != null)
                                        {
                                            _serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
                                            try
                                            {
                                                _serverStorage.Connect();
                                                _serverStorage.CreateFolder(trash);
                                            }
                                            finally
                                            {
                                                _serverStorage.Disconnect();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    trash.Type = FolderType.Trash;
                                    dbMan.UpdateFolder(trash);
                                }
                            }
                            finally
                            {
                                dbMan.Disconnect();
                            }
                        }
						if (trash != null)
						{
                            _isMoveError = !MoveMessages(messageIndexSet, fld, trash);
						}
					}
				}
                else if (_dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.Imap4)
				{
                    PurgeMessages(messageIndexSet, fld);
				}
			}
		}

        public void NoMoveDeleteMessages(object[] messageIndexSet, Folder fld)
        {
            WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();

            if (_dbStorage.Account != null)
            {
                if (_dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.Imap4)
                {
                    PurgeMessages(messageIndexSet, fld);
                }
            }
        }

        /*
         * Removes the message from a folder fld with id_msg of messageIndexSet
         * 0. for DIRECT MODE only removes from the server
         * 1. for IMAP4 removes from the server and database
         * 2. for WMSERVER, if fld - TRASH or SPAM, deletes from the server and database
         * 3. for WMSERVER for the rest of the folders only deletes from the database
         * 4. for POP3, if (fld - TRASH or SPAM) and is setting DeleteMessageWhenItsRemovedFromTrash, removes from the server and database
         * 5. for POP3 in other cases only removes from the database
         */
        public void PurgeMessages(object[] messageIndexSet, Folder fld)
		{
			if (_dbStorage.Account != null)
			{
				object[] serverUids = null;
				bool needToDeleteFromServer = false;
				WebMailMessageCollection msgs = null;
				if (fld.SyncType != FolderSyncType.DirectMode)
				{
					msgs = _dbStorage.LoadMessagesToDelete(false, messageIndexSet, fld);
                    bool isImap = (_dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.Imap4);
                    bool isWmServer = (_dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.WMServer);
                    bool isPop3 = (_dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.Pop3);
                    bool isTrashOrSpam = (fld.Type == FolderType.Trash || fld.Type == FolderType.Spam || fld.Type == FolderType.Drafts);
                    bool isDeleteMessageWhenItsRemovedFromTrash = (_dbStorage.Account.MailMode == MailMode.DeleteMessageWhenItsRemovedFromTrash);
                    if (isImap || isWmServer && isTrashOrSpam ||
                        isPop3 && isTrashOrSpam && isDeleteMessageWhenItsRemovedFromTrash)
					{
                        bool isStrUid = !isImap;
                        if (msgs != null) serverUids = msgs.ToUidsCollection(isStrUid);
						needToDeleteFromServer = true;
					}
				}
				else
				{
					serverUids = messageIndexSet;
					needToDeleteFromServer = true;
				}
				if (needToDeleteFromServer && (serverUids != null) && (serverUids.Length > 0))
				{
					_serverStorage = MailServerStorageCreator.CreateMailServerStorage(_dbStorage.Account);
					try
					{
						_serverStorage.MessageDeleted += new DeleteMessageHanlder(_serverStorage_MessageDeleted);
						_serverStorage.Connect();
						_serverStorage.DeleteMessages(serverUids, fld);
					}
					finally
					{
						_serverStorage.MessageDeleted -= new DeleteMessageHanlder(_serverStorage_MessageDeleted);
						_serverStorage.Disconnect();
					}
				}
				if ((msgs != null) && (msgs.Count > 0))
				{
					_dbStorage.DeleteMessages(msgs.ToIDsCollection(), fld);
				}
			}
		}

		public WebMailMessageCollection SearchMessages(int page, string condition, FolderCollection folders, bool inHeadersOnly, out int searchMessagesCount)
		{
            bool folderInDirectMode = (folders[0].SyncType == FolderSyncType.DirectMode);
            bool folderInHeadersSync = ((folders[0].SyncType == FolderSyncType.AllHeadersOnly) || (folders[0].SyncType == FolderSyncType.NewHeadersOnly));
            if ((_dbStorage.Account.MailIncomingProtocol == IncomingMailProtocol.Imap4) &&
                (folders.Count == 1) && (folderInDirectMode || folderInHeadersSync && !inHeadersOnly))
            {
                ImapStorage imapStor = new ImapStorage(_dbStorage.Account); 
                try
                {
                    imapStor.Connect();
                    if (folderInDirectMode)
                    {
                        return imapStor.SearchMessages(page, condition, folders[0], inHeadersOnly, out searchMessagesCount);
                    }
                    else
                    {
                        UidCollection imapUids = imapStor.SearchMessagesUids(condition, folders[0], inHeadersOnly);
                        List<long> dbUids = _dbStorage.SearchMessagesIntUids(condition, folders[0], inHeadersOnly);
                        List<long> uidsUnion = new List<long>();
                        foreach (long dbUid in dbUids)
                        {
                            foreach (long imapUid in imapUids)
                            {
                                if (dbUid == imapUid)
                                {
                                    uidsUnion.Add(dbUid);
                                    break;
                                }
                            }
                        }
                        List<long> uidsForLoad = new List<long>();
                        searchMessagesCount = uidsUnion.Count;
                        if (uidsUnion.Count > 0)
                        {
                            int msgsOnPage = _dbStorage.Account.UserOfAccount.Settings.MsgsPerPage;
                            int firstUidIndex = (page - 1) * msgsOnPage;
                            int lastUidIndex = page * msgsOnPage - 1;
                            if (lastUidIndex >= uidsUnion.Count) lastUidIndex = uidsUnion.Count - 1;
                            if (firstUidIndex <= lastUidIndex)
                            {
                                for (int i = firstUidIndex; i <= lastUidIndex; i++)
                                {
                                    uidsForLoad.Add(uidsUnion[i]);
                                }
                            }
                        }
                        string[] uids = new string[uidsForLoad.Count];
                        int newUidIndex = 0;
                        foreach (long uid in uidsForLoad)
                        {
                            uids[newUidIndex++] = uid.ToString();
                        }
                        WebMailMessageCollection msgsCol = _dbStorage.LoadMessagesByUids(uids, folders[0], true);
                        return msgsCol;
                    }
                }
                finally
                {
                    imapStor.Disconnect();
                }
            }
			return _dbStorage.SearchMessages(page, condition, folders, inHeadersOnly, out searchMessagesCount);
		}

		public WebMailMessageCollection SearchMessages(string condition, FolderCollection folders, bool inHeadersOnly)
		{
			return _dbStorage.SearchMessages(condition, folders, inHeadersOnly);
		}

		public void SaveMessage(WebMailMessage message, Folder folder)
		{
			Account acct = _dbStorage.Account;
			bool imap4Protocol = (acct != null && acct.MailIncomingProtocol == IncomingMailProtocol.Imap4);
			bool wmserverProtocol = (acct != null && acct.MailIncomingProtocol == IncomingMailProtocol.WMServer);
			if (folder.SyncType != FolderSyncType.DontSync && imap4Protocol)
			{
				_serverStorage = MailServerStorageCreator.CreateMailServerStorage(acct);
				try
				{
					_serverStorage.Connect();
					_serverStorage.SaveMessage(message, folder);
					if (folder.SyncType != FolderSyncType.DirectMode)
					{
						FolderCollection folders = new FolderCollection();
						folders.Add(folder);
						_serverStorage.Synchronize(folders);
					}
				}
				finally
				{
					_serverStorage.Disconnect();
				}
			}
			else if (!wmserverProtocol)
			{
				_dbStorage.SaveMessage(message, folder);
			}
			else
			{
				if (message.StrUid.Trim() == string.Empty)
				{
					WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();

                    long time = Utils.GetUnixMicroTimeStamp();

                    WmServerFS wmServerFS = new WmServerFS(acct);
                    int nextuid = wmServerFS.GetNextUID(folder.Name);
                    message.StrUid = time.ToString() + "." + nextuid.ToString() + ".0." + settings.WmServerHost;
				}
				if (folder.SyncType != FolderSyncType.DirectMode)
				{
					_dbStorage.SaveMessage(message, folder);
				}

				if (folder.SyncType != FolderSyncType.DontSync)
				{
					_serverStorage = MailServerStorageCreator.CreateMailServerStorage(acct);

					try
					{
						_serverStorage.Connect();
						_serverStorage.SaveMessage(message, folder);
						if (folder.SyncType != FolderSyncType.DirectMode)
						{
							FolderCollection folders = new FolderCollection();
							folders.Add(folder);
							_serverStorage.Synchronize(folders);
						}
					}
					finally
					{
						_serverStorage.Disconnect();
					}
				}
			}
		}

        public int SaveMessageAndGetId(WebMailMessage message, Folder folder)
        {
            SaveMessage(message, folder);
            int result = -1;
            if (folder.SyncType != FolderSyncType.DirectMode)
                result = _dbStorage.GetLastMsgID();

            return result;
        }

		public void SaveMessages(WebMailMessageCollection messages, Folder fld)
		{
			_dbStorage.SaveMessages(messages, fld);
		}

		public void UpdateMessage(WebMailMessage webMsg)
		{
			_dbStorage.UpdateMessage(webMsg);
		}

		public long CalculateAccountSize(int accountId)
		{
			return _dbStorage.GetMailStorageSize();
		}

		private void _serverStorage_MessageDownloaded(object sender, CheckMailEventArgs e)
		{
			OnMessageDownloaded(e);
		}

		private void _serverStorage_MessageDeleted(object sender, DeleteMessageEventArgs e)
		{
			OnMessageDeleted(e);
		}
	}// END CLASS DEFINITION MailProcessor
}
