using System;
using System.Collections;
using System.Globalization;
using System.IO;
using MailBee.Mime;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using MailBee;
using MailBee.ImapMail;
using System.Collections.Generic;

namespace WebMail
{
	public struct WMServerUser
	{
		public string Domain;
		public string Name;
		public string Password;
		public string Type;

		public static WMServerUser Parse(string src)
		{
			WMServerUser user = new WMServerUser();

			Regex r = new Regex(@"""(?<domain>[^""]*)""\t""(?<name>[^""]*)""\t""(?<password>[^""]*)""\t""(?<type>[^""]*)""");
			Match m = r.Match(src);
			if (m.Success)
			{
				user.Domain = m.Groups["domain"].Value;
				user.Name = m.Groups["name"].Value;
				user.Password = m.Groups["password"].Value;
				user.Type = m.Groups["type"].Value;
			}

			return user;
		}
	}

    /// <summary>
    /// This class implements an API for managing AfterLogic XMail Server
    /// through filesystem.
    /// </summary>
    public class WmServerFS
    {
        private string _email;
        private string _login;
        private string _domain;
        private Account _account;
        private string _maildirPath;
        private WebmailSettings _settings;

        public WmServerFS(Account acct)
        {
            try
            {
                _email = acct.Email;
                _account = acct;
                string[] temp = _email.Split('@');
                _login = temp[0];
                _domain = temp[1];

                _settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
                _maildirPath = Path.Combine(_settings.WmServerRootPath, @"domains\" + _domain + @"\" + _login + @"\Maildir");
            }
            catch { }
        }

        #region Folders

        public string[] GetFolders()
        {
            List<string> folders = new List<string>();

            if (Directory.Exists(_maildirPath))
            {
                if (Directory.Exists(_maildirPath + @"\" + Constants.MailDirStruct.New) && Directory.Exists(_maildirPath + @"\" + Constants.MailDirStruct.Cur))
                {
                    folders.Insert(0, _account.Delimiter + Constants.FolderNames.Inbox.ToUpper());
                }

                string[] subDirs = Directory.GetDirectories(_maildirPath);
                foreach (string subDir in subDirs)
                {
                    string dirName = Path.GetFileName(subDir);
                    if (dirName.Length > 0 && dirName[0].ToString() == _account.Delimiter && Directory.Exists(subDir))
                    {
                        folders.Add(Utils.ConvertFromUtf7Modified(dirName));
                    }
                }
            }
            return folders.ToArray();
        }

        public bool CreateFolder(string fullName)
        {
            bool result = false;
            if (fullName.ToLower() == Constants.FolderNames.InboxLower)
            {
                return false;
            }

            fullName = Utils.ConvertToUtf7Modified(fullName);

            if (Directory.Exists(_maildirPath) && !Directory.Exists(_maildirPath + @"\" + fullName))
            {
                result = Directory.CreateDirectory(_maildirPath + @"\" + fullName).Exists;

                FileStream fs = File.Open(_maildirPath + @"\" + fullName + @"\" + ".info.tab", FileMode.Create, FileAccess.ReadWrite);

                StringWriter sw = new StringWriter();
                sw.WriteLine("IS.SUBSCRIBED 1");
                sw.WriteLine("UIDVALIDITY " + Utils.GetUnixTimeStamp().ToString());
                sw.WriteLine("UIDNEXT 1");

                byte[] bytes = Encoding.UTF8.GetBytes(sw.ToString());
                fs.Write(bytes, 0, bytes.Length - 1);
                fs.Close();

                File.AppendAllText(_maildirPath + @"\.sub.tab", fullName + "\r\n");

                result = Directory.CreateDirectory(_maildirPath + @"\" + fullName + @"\" + Constants.MailDirStruct.Cur).Exists;
                result = Directory.CreateDirectory(_maildirPath + @"\" + fullName + @"\" + Constants.MailDirStruct.New).Exists;
                result = Directory.CreateDirectory(_maildirPath + @"\" + fullName + @"\" + Constants.MailDirStruct.Tmp).Exists;
            }
            return result;
        }

        public bool UpdateFolder(string fullName, string newfullName)
        {
            bool result = false;
            fullName = Utils.ConvertToUtf7Modified(fullName);
            newfullName = Utils.ConvertToUtf7Modified(newfullName);
            string fullNamePath = _changePath(fullName);
            string newfullNamePath = _changePath(newfullName);
            if (fullName.ToLower() == Constants.FolderNames.InboxLower)
            {
                return false;
            }
            if (Directory.Exists(fullNamePath) && !Directory.Exists(newfullNamePath))
            {
                Directory.Move(fullNamePath, newfullNamePath);
                result = true;
            }
            return result;
        }

        public bool DeleteFolder(string fullName)
        {
            bool result = false;
            fullName = Utils.ConvertToUtf7Modified(fullName);
            switch (fullName.ToLower())
            {
                case Constants.FolderNames.InboxLower:
                case Constants.FolderNames.SentItemsLower:
                case Constants.FolderNames.DraftsLower:
                case Constants.FolderNames.TrashLower:
                case Constants.FolderNames.SpamLower:
                    break;
                default:
                    if (Directory.Exists(_maildirPath + @"\" + fullName + @"\" + Constants.MailDirStruct.Cur))
                        Directory.Delete(_maildirPath + @"\" + fullName + @"\" + Constants.MailDirStruct.Cur);
                    if (Directory.Exists(_maildirPath + @"\" + fullName + @"\" + Constants.MailDirStruct.New))
                        Directory.Delete(_maildirPath + @"\" + fullName + @"\" + Constants.MailDirStruct.New);
                    if (Directory.Exists(_maildirPath + @"\" + fullName + @"\" + Constants.MailDirStruct.Tmp))
                        Directory.Delete(_maildirPath + @"\" + fullName + @"\" + Constants.MailDirStruct.Tmp);
                    if (File.Exists(_maildirPath + @"\" + fullName + @"\" + ".info.tab"))
                        File.Delete(_maildirPath + @"\" + fullName + @"\" + ".info.tab");
                    if (Directory.Exists(_maildirPath + @"\" + fullName))
                        Directory.Delete(_maildirPath + @"\" + fullName);
                    result = true;
                    break;
            }

            return result;
        }

        #endregion

        #region Messages

        public string GetMessagePath(string uid, string folderName)
        {
            folderName = Utils.ConvertToUtf7Modified(folderName);
            string path = Path.Combine(_changePath(folderName), Constants.MailDirStruct.Cur);
            FileMessage fm = new FileMessage(path);
            fm.Load(uid, true);
            return Path.Combine(fm.FolderFullPath, fm.FileName);
        }

        public byte[] GetMessage(string uid, string folderName)
        {
            folderName = Utils.ConvertToUtf7Modified(folderName);
            string path = Path.Combine(_changePath(folderName), Constants.MailDirStruct.Cur);
            FileMessage fm = new FileMessage(path);
            fm.Load(uid, true);
            byte[] result = null;
            if (File.Exists(path + @"\" + fm.FileName))
            {
                result = File.ReadAllBytes(path + @"\" + fm.FileName);
            }
            return result;
        }

        public SystemMessageFlags GetMessageFlags(string uid, string folderName)
        {
            folderName = Utils.ConvertToUtf7Modified(folderName);
            string path = Path.Combine(_changePath(folderName), Constants.MailDirStruct.Cur);
            FileMessage fm = new FileMessage(path);
            fm.Load(uid, true);
            return fm.Flags;
        }

        public SystemMessageFlags GetMessageFlagsByFileName(string filename, string folderName)
        {
            folderName = Utils.ConvertToUtf7Modified(folderName);
            string path = Path.Combine(_changePath(folderName), Constants.MailDirStruct.Cur);
            FileMessage fm = new FileMessage(path);
            fm.Load(filename, false);
            return fm.Flags;
        }

        public string GetMessageFileName(string uid, string folderName)
        {
            folderName = Utils.ConvertToUtf7Modified(folderName);
            string path = Path.Combine(_changePath(folderName), Constants.MailDirStruct.Cur);
            FileMessage fm = new FileMessage(path);
            fm.Load(uid, true);
            return fm.FileName;
        }

        public bool IsMessageExist(string uid, SystemMessageFlags flags, int Size, string folderName)
        {
            folderName = Utils.ConvertToUtf7Modified(folderName);
            string path = Path.Combine(_changePath(folderName), Constants.MailDirStruct.Cur);
            string flagsStr = Utils.SystemMessageFlagsToStr(flags);
            return File.Exists(Path.Combine(path, uid + Constants.SIZE_DELIMETER + Size.ToString() + Constants.FLAG_DELIMETER + flagsStr));
        }

        public FileInfo IsMessageExist(string uid, FileInfo[] files)
        {
            foreach (FileInfo file in files)
            {
                string tuid = FileMessage.GetUIDByFileName(file.Name);
                if (tuid == uid)
                {
                    return file;
                }
            }
            return null;
        }

        public bool IsMessageExist(string uid, string folderName)
        {
            folderName = Utils.ConvertToUtf7Modified(folderName);
            string path = Path.Combine(_changePath(folderName), Constants.MailDirStruct.Cur);
            FileMessage fm = new FileMessage(path);
            fm.Load(uid, true);
            return File.Exists(Path.Combine(path, fm.FileName));
        }

        public string MoveMessage(string uid, string fromFolder, string toFolder)
        {
            fromFolder = Utils.ConvertToUtf7Modified(fromFolder);
            toFolder = Utils.ConvertToUtf7Modified(toFolder);
            string fromPath = Path.Combine(_changePath(fromFolder), Constants.MailDirStruct.Cur);
            string toPath = Path.Combine(_changePath(toFolder), Constants.MailDirStruct.Cur);

            if (fromPath == toPath) return uid;

            FileMessage fm = new FileMessage(fromPath);

            fm.Load(uid, true);

            string[] UIDArray = uid.Split('.');
            UIDArray[1] = GetNextUID(toFolder).ToString();
            string newuid = string.Join(".", UIDArray);
            string newFileName = newuid + Constants.SIZE_DELIMETER + fm.Size.ToString() + Constants.FLAG_DELIMETER + fm.FlagsStr;

            if (File.Exists(fromPath + @"\" + fm.FileName) && Directory.Exists(toPath) && !File.Exists(toPath + @"\" + fm.FileName))
            {
                File.Move(fromPath + @"\" + fm.FileName, toPath + @"\" + newFileName);
                RenewMainInfotab();
            }

            FileMessage newFileMsg = new FileMessage(toPath);
            newFileMsg.Load(newFileName, false);
            return newFileMsg.UID;
        }

        public void MoveMessagesFromNewToCur(string Folder)
        {
            Folder = Utils.ConvertToUtf7Modified(Folder);
            string Path = _changePath(Folder);
            string NewPath = Path + @"\" + Constants.MailDirStruct.New;
            string CurPath = Path + @"\" + Constants.MailDirStruct.Cur;

            if (Directory.Exists(NewPath))
            {
                DirectoryInfo dir = new DirectoryInfo(NewPath);
                FileInfo[] fileInfos = dir.GetFiles();
                foreach (FileInfo fi in fileInfos)
                {
                    if (File.Exists(NewPath + @"\" + fi.Name) && !File.Exists(CurPath + @"\" + fi.Name))
                    {
                        File.Move(NewPath + @"\" + fi.Name, CurPath + @"\" + fi.Name);
                    }
                }
            }
        }

        public void DeleteMessage(string uid, string folderName)
        {
            folderName = Utils.ConvertToUtf7Modified(folderName);
            string path = Path.Combine(_changePath(folderName), Constants.MailDirStruct.Cur);
            FileMessage fm = new FileMessage(path);
            fm.Load(uid, true);
            if (File.Exists(path + @"\" + fm.FileName))
            {
                File.Delete(path + @"\" + fm.FileName);
            }
            RenewMainInfotab();
        }

        public void MarkToDeleteMessage(string uid, string folderName)
        {
            folderName = Utils.ConvertToUtf7Modified(folderName);
            string path = Path.Combine(_changePath(folderName), Constants.MailDirStruct.Cur);
            FileMessage fm = new FileMessage(path);
            fm.Load(uid, true);
            SetMessageFlags(uid, SystemMessageFlags.Deleted, MessageFlagAction.Remove, folderName);
            fm.Save();
        }

        public void UnMarkToDeleteMessage(string uid, string folderName)
        {
            folderName = Utils.ConvertToUtf7Modified(folderName);
            string path = Path.Combine(_changePath(folderName), Constants.MailDirStruct.Cur);
            FileMessage fm = new FileMessage(path);
            fm.Load(uid, true);
            SetMessageFlags(uid, SystemMessageFlags.Deleted, MessageFlagAction.Remove, folderName);
            fm.Save();
        }

        public void SaveMessage(WebMailMessage message, string folderName)
        {
            folderName = Utils.ConvertToUtf7Modified(folderName);
            string uid = message.StrUid;
            byte[] messageRawBody = message.RawMsg;
            string path = Path.Combine(_changePath(folderName), Constants.MailDirStruct.Cur);
            string filename = uid + Constants.SIZE_DELIMETER + message.Size.ToString() + Constants.FLAG_DELIMETER + CombineFlags(message);
            if (!File.Exists(path + @"\" + filename))
            {
                File.WriteAllBytes(path + @"\" + filename, messageRawBody);
            }
            RenewMainInfotab();
        }

        public void SetMessageFlags(string uid, SystemMessageFlags flags, MessageFlagAction action, string folderName)
        {
            folderName = Utils.ConvertToUtf7Modified(folderName);
            string path = Path.Combine(_changePath(folderName), Constants.MailDirStruct.Cur);
            FileMessage fm = new FileMessage(path);
            fm.Load(uid, true);

            if (action == MessageFlagAction.Add)
            {
                fm.Flags = (fm.Flags | flags);
            }
            else if (action == MessageFlagAction.Remove)
            {
                fm.Flags = (fm.Flags & ~flags);
            }

            else if (action == MessageFlagAction.Remove)
            {
                fm.Flags = flags;
            }
            fm.Save();
        }

        #endregion

        public string CombineFlags(WebMailMessage message)
        {
            string flags = string.Empty;
            if (message.Flagged)
                flags += "F";
            if (message.Seen)
                flags += "S";
            if (message.Deleted)
                flags += "T";
            return flags;
        }

        public string _changePath(string fullName)
        {
            string path = _maildirPath;
            if (fullName.Length > 0 && fullName[0].ToString() == _account.Delimiter)
            {
                fullName = fullName.Substring(1);
            }
            if (fullName.ToUpper() != Constants.FolderNames.Inbox.ToUpper())
            {
                path += @"\" + _account.Delimiter + fullName;
            }
            return path;
        }

        public FileInfo[] _getFiles(string FullPath)
        {
            FullPath = Utils.ConvertToUtf7Modified(FullPath);
            DirectoryInfo dir = new DirectoryInfo(Path.Combine(_changePath(FullPath), Constants.MailDirStruct.Cur));
            if (dir.Exists) return dir.GetFiles();
            return (new FileInfo[] { });
        }

        public int GetNextUID(string Folder)
        {
            Folder = Utils.ConvertToUtf7Modified(Folder);

            string filePath = Path.Combine(_changePath(Folder), ".info.tab");
            int nextuid = 0;
            if (File.Exists(filePath))
            {
                string data = string.Empty;
                FileStream fs = null;
                try
                {
                    fs = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite);
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex);
                    throw new WebMailException("Can't read .info.tab file in the \""+ Folder +"\" folder.");
                }

                if (fs != null)
                {
                    byte[] bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, (int)fs.Length - 1);

                    data = Encoding.Default.GetString(bytes);

                    StringReader sr = new StringReader(data);
                    StringWriter sw = new StringWriter();

                    string line = string.Empty;
                    /**/
                    bool uidnext_flag = false;
                    /**/
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.IndexOf("UIDNEXT") >= 0)
                        {
                            string[] nextuid_str = line.Split(new char[] { ' ' });
                            if (nextuid_str.Length == 2)
                            {
                                if (!int.TryParse(nextuid_str[1], out nextuid)) nextuid = 0;
                            }
                            nextuid++;
                            line = "UIDNEXT " + (nextuid).ToString();
                            uidnext_flag = true;
                        }
                        sw.WriteLine(line);
                    }
                    byte[] new_bytes = Encoding.Default.GetBytes(sw.ToString());
                    fs.Seek(0, SeekOrigin.Begin);
                    fs.Write(new_bytes, 0, new_bytes.Length - 1);
                    fs.Close();

                    if (!uidnext_flag)
                    {
                        string logPath = Path.Combine(_changePath(Folder), "uidnext.log");
                        File.AppendAllText(logPath, "[" + DateTime.Now.ToString("HH:mm:ss") + "] - UIDNEXT not found");
                    }
                }
            }

            return nextuid;
        }

        public void RenewMainInfotab()
        {
            string filePath = Path.Combine(_changePath(""), ".info.tab");
            if (File.Exists(filePath))
            {
                File.SetLastAccessTime(filePath, DateTime.Now);
            }
        }

    }

    [Serializable]
    public struct RedirectionsList
    {
        public List<string> forwards;
        public List<string> activeForwards;
    }

	public class FileDateComparer : IComparer
	{
		#region IComparer Members

		public int Compare(object x, object y)
		{
			if (x == y) return 0;
			FileInfo fix = x as FileInfo;
			FileInfo fiy = y as FileInfo;
			if (fix == null) return -1;
			if (fiy == null) return 1;

			return fix.CreationTimeUtc.CompareTo(fiy.CreationTimeUtc);
		}

		#endregion
	}

	/// <summary>
	/// This class is a wrapper for AfterLogic XMail Server administration
    /// protocol. Implements data exchange protocol through port 6017 (by default).
	/// </summary>
	public class WMServerStorage : MailServerStorage
	{
		protected TcpClient _tcpClient = null;
		protected NetworkStream _nwStream = null;
		protected string _hostname = string.Empty;
		protected int _port = 0;
		protected WebmailResourceManager _resMan = null;

		protected string _response = string.Empty;

		public WMServerStorage(string hostname, int port, Account acct) : base(acct)
		{
			_hostname = hostname;
			_port = port;

			_resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
		}

        public WMServerStorage(string hostname, Account acct)
            : this(hostname, WMServerPort, acct)
        {
        }

		public WMServerStorage(Account acct)
			: this(WMServerHost, WMServerPort, acct)
		{
		}

		public static string WMServerHost
		{
			get
			{
				WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
				return settings.WmServerHost;
			}
		}

		public static int WMServerPort
		{
			get
			{
				WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
				return settings.XMailControlPort;
			}
		}

        public override void Connect()
        {
			ConnectAndLoginAdmin();
			UserConnect();
        }

        public void UserConnect()
        {
            string domain = EmailAddress.GetDomainFromEmail(_account.Email);
            string user = EmailAddress.GetAccountNameFromEmail(_account.MailIncomingLogin);

            WMServerUser[] wmu = GetUserList(domain, user);
            if (wmu.Length > 0 && wmu[0].Password == _account.MailIncomingPassword && wmu[0].Type == "U")
            {
            }
            else if (wmu.Length > 0 && wmu[0].Type != "M")
            {
                throw new WebMailException((new WebmailResourceManagerCreator()).CreateResourceManager().GetString("ErrorPOP3IMAP4Auth"));
            }
        }

        public override FolderCollection GetFolders()
        {
            WmServerFS wmserverFS = new WmServerFS(_account);
            FolderCollection fc = new FolderCollection();
            try
            {
                fc = _addLevelToFolderTree(wmserverFS.GetFolders());
            }
            catch { }
            return fc;
        }

        public string[] GetFoldersArray()
        {
            WmServerFS wmserverFS = new WmServerFS(_account);
            return wmserverFS.GetFolders();
        }

        // mark as spam
		public void SpamMessage(string uid, Folder fld)
		{
			WmServerFS wmserverFS = new WmServerFS(_account);
			string fullMsgPath = wmserverFS.GetMessagePath(uid, fld.FullPath);
			string user = EmailAddress.GetAccountNameFromEmail(_account.Email);
			try
			{
				ConnectAndLoginAdmin();

				SendRequest(string.Format("spamfilter	del	ham	\"{0}\"	user	{1}	{2}\r\n",
					fullMsgPath,
					_account.DomainName,
					user));

				ReceiveResponse();

				SendRequest(string.Format("spamfilter	add	spam    \"{0}\"	user	{1}	{2}\r\n",
					fullMsgPath,
					_account.DomainName,
					user));

				ReceiveResponse();
			}
			catch (Exception ex)
			{
                Log.WriteException(ex);
                throw new WebMailException(ex);
			}
		}

		public void NotSpamMessage(string uid, Folder fld)
		{
			WmServerFS wmserverFS = new WmServerFS(_account);
			string fullMsgPath = wmserverFS.GetMessagePath(uid, fld.FullPath);
			string user = EmailAddress.GetAccountNameFromEmail(_account.Email);
			try
			{
				ConnectAndLoginAdmin();

				SendRequest(string.Format("spamfilter	del	spam	\"{0}\"	user	{1}	{2}\r\n",
					fullMsgPath,
					_account.DomainName,
					user));

				ReceiveResponse();

				SendRequest(string.Format("spamfilter	add	ham	\"{0}\"	user	{1}	{2}\r\n",
					fullMsgPath,
					_account.DomainName,
					user));

				ReceiveResponse();
			}
			catch (Exception ex)
			{
                Log.WriteException(ex);
                throw new WebMailException(ex);
			}
		}

		protected virtual void ConnectAndLoginAdmin()
		{
			try
			{

				WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
				_tcpClient = new TcpClient();
				_tcpClient.Connect(_hostname, _port);
				_nwStream = _tcpClient.GetStream();

				ReceiveResponse(); // receive hello from server

				SendRequest(string.Format("{0}	{1}\r\n", settings.XMailLogin, settings.XMailPass));

				ReceiveResponse();
			}
			catch(Exception ex)
			{
                Log.WriteException(ex);
                throw new WebMailException("Couldn't connect to mail server. Please try again bit later or contact your system administrator.");
			}
		}

		public override void DeleteMessages(object[] messageIndexSet, Folder fld)
		{
			try
			{
                WmServerFS wmserverFS = new WmServerFS(_account);
                DbStorage dbStorage = DbStorageCreator.CreateDatabaseStorage(_account);
                if (messageIndexSet.Length > 0 && !_updatedFolders.ContainsKey(fld.ID))
                {
                    _updatedFolders.Add(fld.ID, fld.FullPath);
                }
                for (int i = 0; messageIndexSet.Length > i; i++)
				{
                    wmserverFS.DeleteMessage(messageIndexSet[i].ToString(), fld.FullPath);
				}
			}
			catch (Exception ex)
			{
                Log.WriteException(ex);
                throw PrepareFriendlyException(ex, _resMan.GetString("PROC_CANT_DEL_MSGS"));
			}
		}

		protected virtual void DisconnectAdmin()
		{
			if (_nwStream != null) _nwStream.Close();
			if (_tcpClient != null) _tcpClient.Close();
		}

		public override int GetFolderMessageCount(string fullPath)
		{
			try
			{
				int result = 0;
                string[] mailboxFolders = GetMailBoxFolders(fullPath);
				foreach (string path in mailboxFolders)
				{
					if (Directory.Exists(path))
					{
						DirectoryInfo dir = new DirectoryInfo(path);
						result += dir.GetFiles().Length;
					}
				}
				return result;
			}
			catch (Exception ex)
			{
                Log.WriteException(ex);
                throw PrepareFriendlyException(ex, _resMan.GetString("PROC_CANT_GET_MESSAGES_COUNT"));
			}
		}

		public override int GetFolderUnreadMessageCount(string fullPath)
		{
			return 0;
		}

		public override long GetMailStorageSize()
		{
			try
			{
				long result = 0;
				string[] mailboxFolders = GetMailBoxFolders(Constants.FolderNames.Inbox);
				foreach (string path in mailboxFolders)
				{
					if (Directory.Exists(path))
					{
						DirectoryInfo dir = new DirectoryInfo(path);
						FileInfo[] fileInfos = dir.GetFiles();
						long size = 0;
						foreach (FileInfo fi in fileInfos)
						{
							size += fi.Length;
						}
						result += size;
					}
				}
				return result;
			}
			catch (Exception ex)
			{
                Log.WriteException(ex);
                throw PrepareFriendlyException(ex, _resMan.GetString("PROC_CANT_MAIL_SIZE"));
			}
		}

        public override WebMailMessage LoadMessage(object index, Folder fld)
		{
			return LoadMessage(index, fld, false);
		}

		public override WebMailMessageCollection LoadMessageHeaders(int pageNumber, Folder fld)
		{
			WebMailMessageCollection messages = new WebMailMessageCollection();
			try
			{
				ArrayList arrFiles = new ArrayList();
                string[] mailboxFolders = GetMailBoxFolders(fld.FullPath);
				foreach (string path in mailboxFolders)
				{
					DirectoryInfo dir = new DirectoryInfo(path);
					arrFiles.AddRange(dir.GetFiles());
				}
				FileInfo[] fis = (FileInfo[])arrFiles.ToArray(typeof(FileInfo));

				Array.Sort(fis, new FileDateComparer());

				WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
				int msgPerPage = settings.MailsPerPage;
				int msgCount = fis.Length;
			
				if ((_account != null)
					&& (_account.UserOfAccount != null)
					&& (_account.UserOfAccount.Settings != null))
				{
					msgPerPage = _account.UserOfAccount.Settings.MsgsPerPage;
				}

				int startIndex = 0;
				int length = msgPerPage;
				if ((pageNumber * msgPerPage) > msgCount)
				{
					if (msgCount > msgPerPage)
					{
						length = msgCount % msgPerPage;
					}
					else
					{
						length = msgCount;
					}
				}
				else
				{
					startIndex = (msgCount - (pageNumber * msgPerPage));
				}
				for (int i = 0; i < length; i++)
				{
					if (((startIndex + i) < msgCount) && ((startIndex + i) >= 0))
					{
						messages.Add(LoadMessage(fis[startIndex + i].Name, fld, true));
					}
				}
				if (fld.SyncType == FolderSyncType.DirectMode)
				{
					CollectionBase cb = messages as CollectionBase;
					if (cb != null)
					{
						Utils.ReverseCollection(ref cb);
					}
				}
				return messages;
			}
			catch (Exception ex)
			{
                Log.WriteException(ex);
                throw PrepareFriendlyException(ex, _resMan.GetString("PROC_CANT_GET_MSG_LIST"));
			}
		}

		public override WebMailMessageCollection LoadMessageHeaders(object[] messageIndexSet, Folder fld)
		{
			return LoadMessages(messageIndexSet, fld, true);
		}

        public WebMailMessageCollection LoadMessageHeadersByFiles(FileInfo[] files, Folder fld)
        {
            _msgNumber = 1;
            _msgsCount = files.Length;
            WebMailMessageCollection messages = new WebMailMessageCollection();
            try
            {
                for (int i = 0; i < files.Length; i++)
                {
                    WebMailMessage wmMsg = LoadMessageHeadersByFile(files[i], fld);
                    messages.Add(wmMsg);
                }
                return messages;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                throw PrepareFriendlyException(ex, _resMan.GetString("PROC_CANT_GET_MSG_LIST"));
            }
            finally
            {
                _msgNumber = 0;
                _msgsCount = 0;
            }
        }
        
        public override WebMailMessageCollection LoadMessageHeaders(Folder fld)
		{
			return LoadMessages(fld, true);
		}

		public override WebMailMessageCollection LoadMessages(object[] messageIndexSet, Folder fld)
		{
			return LoadMessages(messageIndexSet, fld, false);
		}

		public override WebMailMessageCollection LoadMessages(Folder fld)
		{
			return LoadMessages(fld, false);
		}

        public override void MoveMessages(object[] messageIndexSet, Folder fromFolder, Folder toFolder)
        {
            if (messageIndexSet.Length > 0)
            {
                try
                {
                    for (int i = 0; i < messageIndexSet.Length; i++)
                    {
                        MoveMessage(messageIndexSet[i], fromFolder, toFolder);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex);
                    throw new WebMailException(ex);
                }
            }
        }

        public void MoveMessage(object messageIndex, Folder fromFolder, Folder toFolder)
        {
            WmServerFS wmserverFS = new WmServerFS(_account);
            DbStorage dbStorage = DbStorageCreator.CreateDatabaseStorage(_account);
            try
            {
                dbStorage.Connect();
                WebMailMessage message = dbStorage.LoadMessage(messageIndex, fromFolder, false);

                try
                {
                    WebMailMessage message_srv = LoadMessage(message.StrUid, fromFolder, false);

                    string newUid = wmserverFS.MoveMessage(message.StrUid, fromFolder.FullPath, toFolder.FullPath);
                    message.StrUid = newUid;
                    message.IDFolderDB = toFolder.ID;
                    dbStorage.UpdateMessage(message);
                }
                catch(Exception ex)
                {
                    Log.WriteException(ex);
                    dbStorage.DeleteMessages(new object[] { messageIndex }, fromFolder);
                }
            }
            catch (MailBeeException ex)
            {
                Log.WriteException(ex);
                throw new WebMailMailBeeException(ex);
            }
            finally
            {
                dbStorage.Disconnect();
            }
        }

        public override void SaveMessage(WebMailMessage message, Folder folder)
        {
            try
            {
                WmServerFS wmserverFS = new WmServerFS(_account);
                wmserverFS.SaveMessage(message, folder.FullPath);
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                throw new WebMailException(ex);
            }
        }
        
        public override void SaveMessages(WebMailMessageCollection messages, Folder fld) 
        {
            foreach (WebMailMessage message in messages)
            {
                SaveMessage(message, fld);
            }
        }

        public override void SetMessagesFlags(object[] messageIndexSet, SystemMessageFlags flags, MessageFlagAction flagsAction, Folder fld)
        {
            WmServerFS wmserverFS = new WmServerFS(_account);
            if (messageIndexSet.Length > 0)
            {
                try
                {
                    foreach (string uid in messageIndexSet)
                    {
                        wmserverFS.SetMessageFlags(uid, flags, flagsAction, fld.FullPath);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex);
                    throw new WebMailException(ex);
                }
            }
        }

        public override void SetMessagesFlags(SystemMessageFlags flags, MessageFlagAction flagsAction, Folder fld) 
        {
            try
            {
                string[] uids = GetUids(fld.FullPath);
                SetMessagesFlags(uids, flags, flagsAction, fld);
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                throw new WebMailException(ex);
            }
        }

        public override Dictionary<long, string> Synchronize(FolderCollection foldersTree)
		{
            WmServerFS wmserverFS = new WmServerFS(_account);
			DbStorage dbStorage = DbStorageCreator.CreateDatabaseStorage(_account);
			FolderCollection folders = new FolderCollection();
			FolderCollection.CreateFolderListFromTree(ref folders, foldersTree);

            foreach (Folder fld in folders)
            {
                ArrayList savedUids = new ArrayList();
                ArrayList serverUids = new ArrayList();
                ArrayList dbUidsToDelete = new ArrayList();
                ArrayList dbUids = new ArrayList();

                if (fld != null)
                {
                    // if folder synchronization is "don't synchronize" or "direct mode",
                    // then do not need to synchronize
                    if (fld.SyncType == FolderSyncType.DirectMode)
                    {
                        if (!_updatedFolders.ContainsKey(fld.ID))
                        {
                            _updatedFolders.Add(fld.ID, fld.FullPath);
                        }
                        continue;
                    }
                    if (fld.SyncType == FolderSyncType.DontSync) continue;

                    try
                    {
                        wmserverFS.MoveMessagesFromNewToCur(fld.FullPath);
                        //
                        FileInfo[] files = wmserverFS._getFiles(fld.FullPath);
                        //
                        dbStorage.Connect();

                        serverUids.AddRange(GetUids(files));
                        dbUids.AddRange(dbStorage.GetUids(fld));

                        if (dbUids.Count > 0)
                        {
                            WebMailMessageCollection mc = dbStorage.LoadMessagesByUids((string[])dbUids.ToArray(typeof(string)), fld, false);
                            for (int j = 0; j < mc.Count; j++ )
                            {
                                FileInfo finfo = wmserverFS.IsMessageExist(mc[j].StrUid, files);
                                if (finfo != null)
                                {
                                    SystemMessageFlags _Flgs = wmserverFS.GetMessageFlagsByFileName(finfo.Name, fld.FullPath);
                                    if (mc[j].Flags != _Flgs)
                                    {
                                        dbStorage.SetMessagesFlags(new object[] { mc[j].IDMsg }, _Flgs, MessageFlagAction.Replace, fld);
                                    }
                                }
                            }
                        }

                        serverUids.Sort(Comparer.DefaultInvariant);
                        // leave only those serverUids that don't exist in dbUids
                        // uids that also exist in serverUids, and dbUids, add to dbUidsToDelete
                        foreach (string dbUid in dbUids)
                        {
                            int index = serverUids.BinarySearch(dbUid);
                            if (index >= 0)
                            {
                                serverUids.RemoveAt(index);
                            }
                            else
                            {
                                dbUidsToDelete.Add(dbUid);
                            }
                        }

                        // if folder synchronization is "all messages" or "all headers" 
                        // then remove from db messages with uids from dbUidsToDelete
                        if ((fld.SyncType == FolderSyncType.AllEntireMessages) || (fld.SyncType == FolderSyncType.AllHeadersOnly))
                        {
                            if (dbUidsToDelete.Count > 0)
                            {
                                WebMailMessageCollection msgsToDelete = dbStorage.LoadMessagesByUids((string[])dbUidsToDelete.ToArray(typeof(string)), fld, false);
                                dbStorage.DeleteMessages(msgsToDelete.ToIDsCollection(), fld);
                                if (!_updatedFolders.ContainsKey(fld.ID))
                                {
                                    _updatedFolders.Add(fld.ID, fld.FullPath);
                                }
                            }
                        }

                        // deleting old messages from the mail server (if exhibited setting KeepMessagesOnServer)
                        if (_account.MailMode == MailMode.KeepMessagesOnServer || _account.MailMode == MailMode.KeepMessagesOnServerAndDeleteMessageWhenItsRemovedFromTrash)
                        {
                            object[] serverUidsToDelete = dbStorage.GetOldMessagesUids(_account.MailsOnServerDays);
                            if (serverUidsToDelete.Length > 0)
                            {
                                DeleteMessages(serverUidsToDelete, fld);
                            }
                        }

                        if (serverUids.Count > 0)
                        {
                            // receive new messages
                            if (!_updatedFolders.ContainsKey(fld.ID))
                            {
                                _updatedFolders.Add(fld.ID, fld.FullPath);
                            }
                            
                            List<FileInfo> serverFiles = new List<FileInfo>();
                            foreach (string serverUid in serverUids.ToArray())
                            {
                                foreach (FileInfo fi in files)
                                {
                                    if (fi.Name.Contains(serverUid))
                                    {
                                        serverFiles.Add(fi);
                                    }
                                }
                            }

                            WebMailMessageCollection newMessages = LoadMessageHeadersByFiles(serverFiles.ToArray(), fld);
                            ApplyXVirus(newMessages);
                            ApplyXSpam(newMessages);
                            ApplyFilters(newMessages, dbStorage, fld, ref savedUids);

                            if (_account.MailMode == MailMode.DeleteMessagesFromServer)
                            {
                                if ((fld.SyncType == FolderSyncType.AllHeadersOnly) || (fld.SyncType == FolderSyncType.NewHeadersOnly))
                                {
                                    Log.WriteLine("Synchronize", "Incorrect Settings: " + _account.MailMode.ToString() + " + " + fld.SyncType.ToString());
                                }
                                else
                                {
                                    DeleteMessages(newMessages.ToUidsCollection(true), null);
                                }
                            }
                        }
                    }
                    catch (WebMailException ex)
                    {
                        Log.WriteException(ex);

//                        dbStorage.SaveUids(savedUids.ToArray());
                        throw;
                    }
                    finally
                    {
//                        dbStorage.ReplaceUids(GetUids(wmserverFS._getFiles(fld.FullPath)));
                        dbStorage.Disconnect();
                    }
                }
            }
			return base.Synchronize(foldersTree);
		}

		#region Admin Functions
		
		public void AddDomain(string domain)
		{
			try
			{
				ExecuteAdminCommand(string.Format(@"domainadd	{0}", domain));
			}
			catch (WebMailWMServerException ex)
			{
                Log.WriteException(ex);
                throw PrepareFriendlyException(ex, "Create domain failed.");
			}
		}

		public void DeleteDomain(string domain)
		{
			try
			{
				ExecuteAdminCommand(string.Format(@"domaindel	{0}", domain));
			}
			catch (WebMailWMServerException ex)
			{
                Log.WriteException(ex);
                throw PrepareFriendlyException(ex, "Delete domain failed.");
			}
		}

		public string[] GetDomainList()
		{
			try
			{
				ExecuteAdminCommand(string.Format("domainlist"));
			}
			catch (WebMailWMServerException ex)
			{
                Log.WriteException(ex);
                throw PrepareFriendlyException(ex, "Retrieve domain list failed.");
			}
			if (_response != null)
			{
				string[] strs = PrepareResultList();
				ArrayList list = new ArrayList();
				foreach (string str in strs)
				{
					list.Add(str.Trim(new char[] { '"' }));
				}
				return (string[])list.ToArray(typeof(string));
			}
			return new string[0];
		}

		public bool IsDomainExists(string domain)
		{
			string[] domains = GetDomainList();
			foreach (string d in domains)
			{
				if (string.Compare(domain, d, true, CultureInfo.InvariantCulture) == 0)
				{
					return true;
				}
			}
			return false;
		}

		public void AddUser(string domain, string user, string password)
		{
			try
			{
				ExecuteAdminCommand(string.Format(@"useradd	{0}	{1}	{2}	U", domain, user, password));
			}
			catch (WebMailWMServerException ex)
			{
                Log.WriteException(ex);
                throw PrepareFriendlyException(ex, "Create account failed.");
			}
		}

		public void DeleteUser(string domain, string user)
		{
			try
			{
				ExecuteAdminCommand(string.Format(@"userdel	{0}	{1}", domain, user));
			}
			catch (WebMailWMServerException ex)
			{
                Log.WriteException(ex);
                throw PrepareFriendlyException(ex, "Delete account failed.");
			}
		}

        //public virtual WMServerUser[] GetUserList()
        //{
        //    return GetUserList(null, null);
        //}

        public virtual WMServerUser[] GetUserList(string domain)
        {
            return GetUserList(domain, null);
        }

		public WMServerUser[] GetUserList(string domain, string user)
		{
			try
			{
				string command = "userlist";
				if ((!string.IsNullOrEmpty(domain)) && (!string.IsNullOrEmpty(user)))
				{
					command = string.Format(@"userlist	{0}	{1}", domain, user);
				}
				else if (!string.IsNullOrEmpty(domain))
				{
					command = string.Format(@"userlist	{0}", domain);
				}
				ExecuteAdminCommand(command);
			}
			catch (WebMailWMServerException ex)
			{
                Log.WriteException(ex);
                throw PrepareFriendlyException(ex, "Retrieve accounts list failed.");
			}
			if (_response != null)
			{
				string[] strs = PrepareResultList();
				ArrayList list = new ArrayList();
				foreach (string str in strs)
				{
					list.Add(WMServerUser.Parse(str));
				}
				return (WMServerUser[])list.ToArray(typeof(WMServerUser));
			}
			return new WMServerUser[0];
		}

		public bool IsUserExists(string user)
		{
			return IsUserExists(null, user);
		}

		public bool IsUserExists(string domain, string user)
		{
			WMServerUser[] users = GetUserList(domain, null);
			if ((users != null) && (users.Length > 0))
			{
				foreach (WMServerUser u in users)
				{
					if (string.Compare(u.Name, user, true, CultureInfo.InvariantCulture) == 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		public void ChangeUserPassword(string domain, string user, string newPassword)
		{
			try
			{
				ExecuteAdminCommand(string.Format(@"userpasswd	{0}	{1}	{2}", domain, user, newPassword));
			}
			catch (WebMailWMServerException ex)
			{
                Log.WriteLine("ChangeUserPassword", "Changing account password failed: " + ex.Message);
                throw PrepareFriendlyException(ex, "Changing account password failed.");
			}
		}

		#endregion

		protected void SendRequest(string command)
		{
			if (_nwStream == null) throw new WebMailWMServerException(_resMan.GetString("PROC_CANT_GET_MSG_LIST"));

			byte[] sendBytes = Encoding.UTF8.GetBytes(command);
			_nwStream.Write(sendBytes, 0, sendBytes.Length);
		}

		protected string ReceiveResponse()
		{
			if (_nwStream == null) throw new WebMailWMServerException(_resMan.GetString("PROC_CANT_GET_MSG_LIST"));

			bool multiline;
			StreamReader sr = new StreamReader(_nwStream, true);
			_response = sr.ReadLine();
			if (!IsPositiveResponse(_response, out multiline))
			{
				throw new WebMailWMServerException(_response);
			}
			if (multiline)
			{
				StringBuilder sb = new StringBuilder();
				string str;
				do
				{
					str = sr.ReadLine();
					sb.Append(str + "\r\n");
				}
				while (str != ".");
				_response = sb.ToString();
			}
			return _response;
		}

		protected bool IsPositiveResponse(string response, out bool multiline)
		{
			multiline = false;
			if (!string.IsNullOrEmpty(response))
			{
				if (response.Length >= 6)
				{
					multiline = (string.Compare(response.Substring(1, 5), "00100", true, CultureInfo.InvariantCulture) == 0) ? true : false;
				}
				return (response[0] == '+') ? true : false;
			}
			return false;
		}

		protected void ExecuteAdminCommand(string command)
		{
			if (command == null) throw new WebMailWMServerException(Constants.wmServCmdNull);

			if (!command.EndsWith("\r\n")) command += "\r\n";

			try
			{
				ConnectAndLoginAdmin();

				SendRequest(command);

				ReceiveResponse();

				SendRequest("quit\r\n");

				//ReceiveResponse(); // commented: output results will be overwrite with OK by QUIT
			}
			catch (WebMailException)
			{
				throw;
			}
			catch (Exception ex)
			{
                Log.WriteException(ex);
                throw new WebMailWMServerException(ex);
			}
			finally
			{
				DisconnectAdmin();
			}
		}

		protected WebMailWMServerException PrepareFriendlyException(Exception ex, string friendlyMessage)
		{
			string message = string.Format(@"{0}

Server response: {1}", friendlyMessage, ex.Message);
			return new WebMailWMServerException(message);
		}

		private string[] PrepareResultList()
		{
			ArrayList result = Utils.Split(_response, "\r\n");
	
			string lastStr = (string)result[result.Count - 1];
			if (string.Compare(lastStr, ".", true, CultureInfo.InvariantCulture) == 0) result.RemoveAt(result.Count - 1);
	
			return (string[])result.ToArray(typeof(string));
		}

		protected string[] GetMailBoxFolders(string folder)
		{
			ArrayList list = new ArrayList();
            WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();

            WmServerFS wmserverFS = new WmServerFS(_account);

			string[] temp = _account.Email.Split('@');
			string strLogin = temp[0];
			string strDomain = temp[1];

            if (folder[0] == '.')
                folder = folder.Substring(1);
            string maildirPath = wmserverFS._changePath(folder);
            if (Directory.Exists(maildirPath))
			{
				string[] subDirs = Directory.GetDirectories(maildirPath);
				foreach (string subDir in subDirs)
				{
                    string dirName = Path.GetFileName(subDir);
                    if (dirName.Length > 0 && dirName[0] != '.' && Directory.Exists(subDir) && dirName == "cur")
                    {
                        list.Add(subDir);
                    }
				}
			}
			else
			{
				string mailboxPath = Path.Combine(settings.WmServerRootPath, @"domains\" + strDomain + @"\" + strLogin + @"\mailbox");
				list.Add(mailboxPath);
			}

			return (string[])list.ToArray(typeof(string));
		}

        protected string[] GetUids(string folder)
        {
            ArrayList result = new ArrayList();
            try
            {
                WmServerFS wmserverFS = new WmServerFS(_account);
                string maildirPath = wmserverFS._changePath(folder);
                string path = Path.Combine(maildirPath, Constants.MailDirStruct.Cur);

                if (!Directory.Exists(path))
                {
                    Log.Write("Directory is not exists: " + path);
                    return new string[0];
                }

                DirectoryInfo dir = new DirectoryInfo(path);
                FileMessage fm = new FileMessage(dir.FullName);
                FileInfo[] fileInfos = dir.GetFiles();
                foreach (FileInfo fi in fileInfos)
                {
                    string filename = fi.Name;
                    fm.Load(filename, false);
                    result.Add(fm.UID);
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                throw PrepareFriendlyException(ex, _resMan.GetString("PROC_CANT_GET_MSG_LIST"));
            }
            return (string[])result.ToArray(typeof(string));
        }

        protected string[] GetUids(FileInfo[] fileInfos)
        {
            ArrayList result = new ArrayList();
            try
            {
                foreach (FileInfo fi in fileInfos)
                {
                    result.Add(FileMessage.GetUIDByFileName(fi.Name));
                }
            }
            catch (Exception ex)
            {
                throw PrepareFriendlyException(ex, _resMan.GetString("PROC_CANT_GET_MSG_LIST"));
            }
            return (string[])result.ToArray(typeof(string));
        }

        protected string[] GetNames(string folder)
        {
            ArrayList result = new ArrayList();
            try
            {
                string[] mailboxFolders = GetMailBoxFolders(folder);
                foreach (string path in mailboxFolders)
                {
                    DirectoryInfo dir = new DirectoryInfo(path);
                    FileInfo[] fileInfos = dir.GetFiles();
                    foreach (FileInfo fi in fileInfos)
                    {
                        result.Add(fi.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                throw PrepareFriendlyException(ex, _resMan.GetString("PROC_CANT_GET_MSG_LIST"));
            }
            return (string[])result.ToArray(typeof(string));
        }
        
        protected WebMailMessage LoadMessageHeadersByFile(FileInfo file, Folder fld)        
        {
            WebMailMessage msg = new WebMailMessage(_account);
            try
            {
                MailMessage message = new MailMessage();
                message.Parser.ParseHeaderOnly = true;

                message.LoadMessage(file.FullName);

                OnMessageDownloaded(new CheckMailEventArgs(Utils.GetLocalizedFolderNameByType(fld), _msgsCount, _msgNumber++));

                WmServerFS wmServerFS = new WmServerFS(_account);
                msg.Init(message, (!string.IsNullOrEmpty(msg.StrUid)), fld);
                if (fld.SyncType == FolderSyncType.DirectMode) msg.Seen = true;
                msg.Flags = wmServerFS.GetMessageFlagsByFileName(file.Name, fld.FullPath);
                msg.StrUid = FileMessage.GetUIDByFileName(file.Name);
                msg.Size = file.Length;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                msg = null;
                throw PrepareFriendlyException(ex, _resMan.GetString("PROC_MSG_HAS_DELETED"));
            }
            return msg;
        }
        
        protected WebMailMessage LoadMessage(object index, Folder fld, bool headersOnly)
		{
			try
			{
				MailMessage message = new MailMessage();
				message.Parser.ParseHeaderOnly = headersOnly;

                string[] mailboxFolders = GetMailBoxFolders(fld.FullPath);
                WmServerFS wmServerFS = new WmServerFS(_account);
				foreach (string path in mailboxFolders)
				{
					string fullMessagePath = Path.Combine(path, wmServerFS.GetMessageFileName(index.ToString(), fld.FullPath));
					if (!headersOnly)
                        message.LoadMessage(wmServerFS.GetMessage(index.ToString(), fld.FullPath));
                    else
						message.LoadMessage(fullMessagePath);
                    
					OnMessageDownloaded(new CheckMailEventArgs(Utils.GetLocalizedFolderNameByType(fld), _msgsCount, _msgNumber++));

					WebMailMessage msg = new WebMailMessage(_account);
					msg.Init(message, (!string.IsNullOrEmpty(msg.StrUid)), fld);
					if (fld.SyncType == FolderSyncType.DirectMode) msg.Seen = true;
                    msg.Flags = wmServerFS.GetMessageFlags(index.ToString(), fld.FullPath);
					msg.StrUid = index.ToString();
					if (headersOnly)
					{
						FileInfo fi = new FileInfo(fullMessagePath);
						msg.Size = fi.Length;
					}

					return msg;
				}
			}
			catch (Exception ex)
			{
                Log.WriteException(ex);
                throw PrepareFriendlyException(ex, _resMan.GetString("PROC_MSG_HAS_DELETED"));
			}
			return null;
		}

        public override WebMailMessageCollection LoadMessages(object[] index, Folder fld, bool body_structure, MessageMode mode)
        {
            return LoadMessages(index, fld, false);
        }

		protected WebMailMessageCollection LoadMessages(object[] messageIndexSet, Folder fld, bool headersOnly)
		{
			_msgNumber = 1;
			_msgsCount = messageIndexSet.Length;
			WebMailMessageCollection messages = new WebMailMessageCollection();
			try
			{
				for(int i = 0; i < messageIndexSet.Length; i++)
				{
					WebMailMessage wmMsg = LoadMessage(messageIndexSet[i], fld, headersOnly);
					messages.Add(wmMsg);
				}
				return messages;
			}
			catch (Exception ex)
			{
                Log.WriteException(ex);
                throw PrepareFriendlyException(ex, _resMan.GetString("PROC_CANT_GET_MSG_LIST"));
			}
			finally
			{
				_msgNumber = 0;
				_msgsCount = 0;
			}
		}

		protected WebMailMessageCollection LoadMessages(Folder fld, bool headersOnly)
		{
			WebMailMessageCollection messages = new WebMailMessageCollection();
			try
			{
                string[] mailboxFolders = GetMailBoxFolders(fld.FullPath);
				foreach (string path in mailboxFolders)
				{
					DirectoryInfo dir = new DirectoryInfo(path);
					FileInfo[] fis = dir.GetFiles();
					for (int i = 0; i < fis.Length; i++)
					{
						WebMailMessage webMsg = LoadMessage(fis[i].Name, fld, headersOnly);
						messages.Add(webMsg);
					}
				}
				return messages;
			}
			catch (Exception ex)
			{
                Log.WriteException(ex);
                throw PrepareFriendlyException(ex, _resMan.GetString("PROC_CANT_GET_MSG_LIST"));
			}
		}

		public override Folder CreateFolder(Folder fld)
		{
            WmServerFS wmServerFS = new WmServerFS(_account);
            if (wmServerFS.CreateFolder(fld.FullPath)) return fld;
            return null;
        }

        public override void UpdateFolder(Folder fld)
        {
            WmServerFS wmServerFS = new WmServerFS(_account);
            if (string.Compare(fld.Name, fld.UpdateName, true, CultureInfo.InvariantCulture) != 0)
            {
                if (!string.IsNullOrEmpty(fld.UpdateFullPath))
                {
                    wmServerFS.UpdateFolder(fld.FullPath, fld.UpdateFullPath);
                }
            }
        }

        public override void DeleteFolder(Folder fld)
        {
            WmServerFS wmServerFS = new WmServerFS(_account);
            wmServerFS.DeleteFolder(fld.FullPath);
        }

        
        public virtual void ChangeAdminPassword(string newLogin, string newPassword)
		{
			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
			try
			{
				string serverPath = Path.Combine(settings.WmServerRootPath, "domains");
				if (Directory.Exists(serverPath))
				{
					string paswFilename = Path.Combine(settings.WmServerRootPath, "ctrlaccounts.tab");
					if (File.Exists(paswFilename))
					{
						using (StreamWriter sw = new StreamWriter(File.Open(paswFilename, FileMode.Create)))
						{
							sw.WriteLine(string.Format(@"""{0}""	""{1}""", newLogin, EncryptPassword(newPassword)));
						}
					}
				}
			}
			catch (Exception ex)
			{
                Log.WriteException(ex);
                throw new WebMailWMServerException("Change admin password failed.", ex);
			}
		}

		protected virtual string EncryptPassword(string password)
		{
			StringBuilder sb = new StringBuilder();
			byte[] bytes = Encoding.UTF8.GetBytes(password);
			for (int i = 0; i < bytes.Length; i++)
			{
				byte b = (byte)((bytes[i] ^ 101) & 0xFF);
				sb.Append(b.ToString("X2"));
			}
			return sb.ToString();
		}

        public static string DecryptPassword(string password)
        {
            if (password == null) return string.Empty;
            string result = string.Empty;
            if ((password.Length > 0) && (password.Length % 2 == 0))
            {
                byte[] decryptedBytes = new byte[password.Length / 2];
                int startIndex = 0;
                int index = 0;
                while (startIndex < password.Length)
                {
                    string strByte = password.Substring(startIndex, 2);
                    int b;
                    if (int.TryParse(strByte, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out b))
                    {
                        b = ((b & 0xFF) ^ 101);
                        decryptedBytes[index] = (byte)b;
                    }
                    startIndex += 2;
                    index++;
                }
                result = Encoding.UTF8.GetString(decryptedBytes);
            }
            return result;
        }

        public FolderCollection _addLevelToFolderTree(string[] folders)
        {
            return _addLevelToFolderTree(folders, "", new bool[] { false, false, false, false, false });
        }

        public FolderCollection _addLevelToFolderTree(string[] folders, string rootPrefix, bool[] param)
        {
			bool InboxAdd = param[0];
			bool SentAdd = param[1];
			bool DraftsAdd = param[2];
			bool TrashAdd = param[3];
			bool SpamAdd = param[3];
            
            FolderCollection folderCollection = new FolderCollection();
			int foldersCount = folders.Length;
			int prefixLen = rootPrefix.Length;
			
            for (int i = 0; i < foldersCount; i++)
            {
			    string folderFullName = folders[i];
                if(rootPrefix != folderFullName && folderFullName.Length > prefixLen && folderFullName.Substring(0, prefixLen) == rootPrefix && folderFullName.IndexOf(_account.Delimiter, prefixLen + 1) == -1)
			    {
					int strLen = (prefixLen == 0) ? prefixLen : prefixLen - 1;
					string name = folderFullName.Substring(strLen).Trim(_account.Delimiter[0]);
					string[] names = name.Split(_account.Delimiter[0]);
                    Folder folderObj = new Folder(_account.ID, -1, folderFullName, names[0]);

					if (prefixLen == 0)
					{
						switch (folderObj.Type)
						{
							case FolderType.Inbox:
								if (InboxAdd) folderObj.Type = FolderType.Custom;
								InboxAdd = true;
								break;
							case FolderType.SentItems:
								if (SentAdd) folderObj.Type = FolderType.Custom;
								SentAdd = true;
								break;
							case FolderType.Drafts:
								if (DraftsAdd) folderObj.Type = FolderType.Custom;
								DraftsAdd = true;
								break;
                            case FolderType.Trash:
								if (TrashAdd) folderObj.Type = FolderType.Custom;
								TrashAdd = true;
								break;
							case FolderType.Spam:
								if (SpamAdd) folderObj.Type = FolderType.Custom;
								SpamAdd = true;
								break;				
						}
					}
					else 
					{
						folderObj.Type = FolderType.Custom;
					}
					
					folderCollection.Add(folderObj);
			    	FolderCollection newCollection = _addLevelToFolderTree(folders, folderFullName + _account.Delimiter, new bool[] { InboxAdd, SentAdd, DraftsAdd, /*TrashAdd,*/ SpamAdd });
					if(newCollection.Count > 0)
					{
                        foreach (Folder f in newCollection)
                        {
                            folderObj.SubFolders.Add(f);
                        }
					}
                }
            }
            return folderCollection;
        }

        public void SetAutoResponder(bool Enable, string Subject, string Message)
        {
            if (Enable)
            {
                try
                {
                    string[] temp = _account.Email.Split('@');
                    string login = temp[0];
                    string domain = temp[1];

                    string data = string.Format("BODY \"{0}\"\nSUBJECT \"{1}\"\r\n.\r\n", _prepAutoresponderSetValue(Message), _prepAutoresponderSetValue(Subject));

                    ConnectAndLoginAdmin();
                    SendRequest(string.Format("cfgfileset\t\"domains/{0}/{1}/autoresponder.tab\"\r\n{2}", domain, login, data));
                    ReceiveResponse();
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex);
                    throw new WebMailException(ex);
                }
            }
            else
            {
                DeleteAutoResponder();
            }
        }

        public Autoresponder GetAutoResponder()
        {
            List<string> result = new List<string>();
            Autoresponder aresponder = null;
            try
            {
                string[] temp = _account.Email.Split('@');
                string login = temp[0];
                string domain = temp[1];
                string line = string.Empty;

                ConnectAndLoginAdmin();
                SendRequest(string.Format("cfgfileget\t\"domains/{0}/{1}/autoresponder.tab\"\r\n", domain, login));
                string res = ReceiveResponse();

                using (StringReader sr = new StringReader(res))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (line == ".") break;

                        string[] parts = null;
                        try
                        {
                            Regex re = new Regex("\\s+");
                            parts = re.Split(line, 2);
                            result.Add(_prepAutoresponderGetValue(_trimQuotes(parts[1])));
                        }
                        catch (Exception ex)
                        {
                            Log.WriteException(ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                return null;
            }
            if (result != null)
            {
                aresponder = new Autoresponder();
                aresponder.Enable = true;
                aresponder.Message = result[0];
                aresponder.Subject = result[1];
            }
            return aresponder;
        }

        public void DeleteAutoResponder()
        {
            try
            {
                string[] temp = _account.Email.Split('@');
                string login = temp[0];
                string domain = temp[1];

                ConnectAndLoginAdmin();
                SendRequest(string.Format("cfgfileset\t\"domains/{0}/{1}/autoresponder.tab\"\r\n.\r\n", domain, login));
                ReceiveResponse();
            }
            catch (Exception ex)
            {
                Log.WriteException(ex);
                throw new WebMailException(ex);
            }
        }

        protected string _prepAutoresponderGetValue(string _string)
        {
            _string = _string.Replace("\\r", "\r");
            _string = _string.Replace("\\n", "\n");
            _string = _string.Replace("\\t", "\t");
            _string = _string.Replace("\\\"", "\"");
            _string = _string.Replace("\\\\", "\\");
            return _string;
        }
    
        protected string _prepAutoresponderSetValue(string _string)
        {
            _string = _string.Replace("\\", "\\\\");
            _string = _string.Replace("\"", "\\\"");
            _string = _string.Replace("\r", "\\r");
            _string = _string.Replace("\n", "\\n");
            _string = _string.Replace("\t", "\\t");
            return _string;
        }

        protected string _trimQuotes(string _string)
        {
            int start = _string.IndexOf('"') + 1;
            int end = _string.LastIndexOf('"') - 1;
            _string = _string.Substring(start, end);
            return _string;
        }

        public string GetCustomDomainFile(string customDomain)
        {
            try
            {
                ExecuteAdminCommand(string.Format("custdomget\t{0}", customDomain));
            }
            catch (Exception)
            {
                throw;
            }
            return _response;
        }

        public void SetCustomDomainTab(string customDomain, string data)
        {
            string response = string.Empty;
            try
            {
                ConnectAndLoginAdmin();
                string command = string.Format("custdomset\t{0}\r\n{1}", customDomain, data);
                SendRequest(command);
                ReceiveResponse();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string[] GetCustomDomainTab(string customDomainName)
        {
            string[] result = new string[2];
            Hashtable ht = new Hashtable();

            try
            {
                string raw = GetCustomDomainFile(customDomainName);
                string line;
                result[0] = "";
                result[1] = "";
                string[] id;

                using (StringReader sr = new StringReader(raw))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (line == ".") break;
                        id = extractCustomDomainData(line);
                        line = line.Trim();
                        switch (id[0])
                        {
                            case "mailbox":
                            case "redirect":
                            case "lredirect":
                                result[0] += line + "\r\n";
                                break;
                            default:
                                result[1] += line + "\r\n";
                                break;
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Log.WriteException(error);
            }

            return result;
        }

        public string[] extractCustomDomainData(string line)
        {
            string[] parts = null;
            try
            {
                Regex re = new Regex("\\s+");
                parts = re.Split(line);
                for (int i = 0; i < parts.Length; i++) parts[i] = parts[i].Trim(new char[] { '"' });
            }
            catch (Exception error)
            {
                Log.WriteException(error);
            }
            return parts;
        }

        public string ListAliases(string domain, string alias, string username)
        {
            try
            {
                string command = "aliaslist";
                if (domain != null)
                {
                    command += string.Format("\t{0}", domain);
                }
                if (alias != null)
                {
                    command += string.Format("\t{0}", alias);
                }
                if (username != null)
                {
                    command += string.Format("\t{0}", username);
                }
                //string command = string.Format("aliaslist\t{0}\t{1}\t{2}", domain, alias, username);
                ExecuteAdminCommand(command);
            }
            catch (Exception)
            {
                throw;
            }
            return _response;
        }

        public void DelCustomDomain(string customDomain)
        {
            try
            {
                ExecuteAdminCommand(string.Format("custdomdel\t{0}", customDomain));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string extractUsersAliasName(string line)
        {
            try
            {
                Regex re = new Regex("\\s+");
                string[] parts = re.Split(line);
                return parts[1].Trim(new char[] { '"' });
            }
            catch (Exception error)
            {
                Log.WriteException(error);
            }
            return "";
        }
        
        public string[] GetUsersAliases(string dName, string uName)
        {
            List<string> userAliases = new List<string>();
            try
            {
                string raw = "";
                string line;
                raw = ListAliases(dName, "*", uName);
                using (StringReader sr = new StringReader(raw))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (line == ".") break;
                        userAliases.Add(extractUsersAliasName(line));
                    }
                }
            }
            catch (Exception error)
            {
                Log.WriteException(error);
            }
            return (userAliases.Count == 0) ? null : userAliases.ToArray();
        }
        
        public void AddAlias(string domain, string username, string alias)
        {
            string response = string.Empty;
            try
            {
                ExecuteAdminCommand(string.Format("aliasadd\t{0}\t{1}\t{2}", domain, alias, username));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteAlias(string domain, string alias)
        {
            string response = string.Empty;
            try
            {
                ExecuteAdminCommand(string.Format("aliasdel\t{0}\t{1}", domain, alias));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string[] extractMailProc(string line)
        {
            string[] parts = null;
            try
            {
                Regex re = new Regex("\\s+");
                parts = re.Split(line);
                for (int i = 0; i < parts.Length; i++) parts[i] = parts[i].Trim(new char[] { '"' });
            }
            catch (Exception error)
            {
                Log.WriteException(error);
            }
            return parts;
        }
        
        public string GetMailProcTab(string domain, string username)
        {
            string response = string.Empty;
            try
            {
                ExecuteAdminCommand(string.Format("usergetmproc\t{0}\t{1}", domain, username));
            }
            catch (Exception)
            {
                throw;
            }
            return _response;
        }

        public string[] _GetMailProcTab(string domainName, string userName)
        {
            string[] result = new string[2] { "", "" };
            try
            {
                string raw = GetMailProcTab(domainName, userName);

                string line;
                result[0] = "";
                result[1] = "";
                string[] id;
                using (StringReader sr = new StringReader(raw))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (line == ".") break;
                        id = extractMailProc(line);
                        line = line.Trim();
                        switch (id[0])
                        {
                            case "mailbox":
                            case "redirect":
                            case "lredirect":
                                result[0] += line + "\r\n";
                                break;
                            default:
                                result[1] += line + "\r\n";
                                break;
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Log.WriteException(error);
            }
            return result;
        }

        public string SetMailProcTab(string domain, string username, string data)
        {
            string response = string.Empty;
            try
            {
                ExecuteAdminCommand(string.Format("usersetmproc\t{0}\t{1}\r\n{2}", domain, username, data));
            }
            catch (Exception)
            {
                throw;
            }
            return _response;
        }
        
        public RedirectionsList GetRedirections(string dName, string uName)//
        {
            RedirectionsList result = new RedirectionsList();
            result.forwards = new List<string>();
            result.activeForwards = new List<string>();
            try
            {
                string raw = GetMailProcTab(dName, uName);
                string line = string.Empty;
                using (StringReader sr = new StringReader(raw))
                {
                    string[] data;
                    int i = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (line == ".") break;
                        data = extractMailProc(line);
                        switch (data[0])
                        {
                            case "redirect":
                                for (i = 1; i < data.Length; i++)
                                    result.forwards.Add(data[i]);
                                break;
                            case "lredirect":
                                for (i = 1; i < data.Length; i++)
                                    result.activeForwards.Add(data[i]);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Log.WriteException(error);
            }
            return result;
        }

        public void AddRedirection(string domainName, string userName, string Redirection)
        {
            try
            {
                AddDeleteRedirections(domainName, userName, "AddToRedirection", Redirection);
            }
            catch (Exception error)
            {
                Log.WriteException(error);
            }
        }

        public void AddLRedirection(string domainName, string userName, string LRedirection)
        {
            try
            {
                AddDeleteRedirections(domainName, userName, "AddToLRedirection", LRedirection);
            }
            catch (Exception error)
            {
                Log.WriteException(error);
            }
        }

        public void DeleteRedirection(string domainName, string userName, string forwardSelectedItem)
        {
            try
            {
                AddDeleteRedirections(domainName, userName, "DeleteRedirection", forwardSelectedItem);
            }
            catch (Exception error)
            {
                Log.WriteException(error);
            }
        }

        public void DeleteLRedirection(string domainName, string userName, string activeForwardSelectedItem)
        {
            try
            {
                AddDeleteRedirections(domainName, userName, "DeleteLRedirection", activeForwardSelectedItem);
            }
            catch (Exception error)
            {
                Log.WriteException(error);
            }
        }

        public void AddDeleteRedirections(string domainName, string userName, string command, string element)
        {
            try
            {
                string data = "";
                string[] mailProcTabData;
                mailProcTabData = _GetMailProcTab(domainName, userName);

                string line;
                string head = "";

                string redirection = "";
                string lredirection = "";

                bool noRedirection = true;
                bool noLRedirection = true;

                string[] id;
                int lineIndex;
                StringReader reader = new StringReader(mailProcTabData[0]);
                switch (command)
                {
                    case "AddToRedirection":
                    case "AddToLRedirection":
                        lineIndex = 0;
                        while ((line = reader.ReadLine()) != null)
                        {
                            lineIndex++;
                            id = extractMailProc(line);
                            line = line.Trim();
                            switch (id[0])
                            {
                                case "mailbox":
                                    head = line + "\r\n";
                                    break;
                                case "redirect":
                                    noRedirection = false;
                                    if (command == "AddToRedirection")
                                        redirection = line + '\t' + '\"' + element + '\"' + "\r\n";
                                    else
                                        redirection = line + "\r\n";
                                    break;
                                case "lredirect":
                                    noLRedirection = false;
                                    if (command == "AddToLRedirection")
                                        lredirection = line + '\t' + '\"' + element + '\"' + "\r\n";
                                    else
                                        lredirection = line + "\r\n";
                                    break;
                                default:
                                    break;
                            }
                        }
                        /*                    if (head == "")
                                                head = '\"' + "mailbox" + '\"' + "\r\n";
                        */
                        if (noRedirection)
                            if (command == "AddToRedirection") redirection = "redirect" + '\t' + '\"' + element + '\"' + "\r\n";
                        if (noLRedirection)
                            if (command == "AddToLRedirection") lredirection = "lredirect" + '\t' + '\"' + element + '\"' + "\r\n";
                        break;
                    case "DeleteRedirection":
                    case "DeleteLRedirection":
                        lineIndex = 0;
                        while ((line = reader.ReadLine()) != null)
                        {
                            lineIndex++;
                            id = extractMailProc(line);
                            line = line.Trim();
                            switch (id[0])
                            {
                                case "mailbox":
                                    head = line + "\r\n";
                                    break;
                                case "redirect":
                                    if (command == "DeleteRedirection")
                                    {
                                        int index = line.IndexOf(element);
                                        string before = line.Substring(0, index - 2);
                                        string after = line.Substring(index + 1 + element.Length);
                                        redirection = before + after + "\r\n";
                                        if (redirection.Length == 10) redirection = "";
                                    }
                                    else
                                    {
                                        redirection = line + "\r\n";
                                    }
                                    break;
                                case "lredirect":
                                    if (command == "DeleteLRedirection")
                                    {
                                        int index = line.IndexOf(element);
                                        string before = line.Substring(0, index - 2);
                                        string after = line.Substring(index + 1 + element.Length);
                                        lredirection = before + after + "\r\n";
                                        if (lredirection.Length == 11) lredirection = "";
                                    }
                                    else
                                    {
                                        lredirection = line + "\r\n";
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    default:
                        break;
                }
                data = head + redirection + lredirection + mailProcTabData[1];
                SetMailProcTab(domainName, userName, data);
            }
            catch (Exception error)
            {
                Log.WriteException(error);
                throw;
            }
        }

        public bool GetMailBoxInstruct(string dName, string uName)
        {
            try
            {
                string raw = "";
                string line;
                raw = GetMailProcTab(dName, uName);
                using (StringReader sr = new StringReader(raw))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Contains("mailbox")) return true;
                    }
                }
            }
            catch (Exception error)
            {
                Log.WriteException(error);
            }
            return false;
        }

        public void SetMailBoxInstruct(string dName, string uName)
        {
            string raw = "";
            try
            {
                raw = GetMailProcTab(dName, uName);
            }
            catch (Exception error)
            {
                Log.WriteException(error);
            }
            raw = "\"mailbox\"\r\n" + raw;
            try
            {
                SetMailProcTab(dName, uName, raw);
            }
            catch (Exception error)
            {
                Log.WriteException(error);
            }
        }

        public void DelMailBoxInstruct(string dName, string uName)
        {
            try
            {
                string raw = string.Empty;
                string line = string.Empty;
                string res_lines = string.Empty;
                raw = GetMailProcTab(dName, uName);
                StringWriter sw = new StringWriter();
                using (StringReader sr = new StringReader(raw))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (!line.Contains("mailbox"))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                SetMailProcTab(dName, uName, sw.ToString());
            }
            catch (Exception error)
            {
                Log.WriteException(error);
            }
        }
	}
}
