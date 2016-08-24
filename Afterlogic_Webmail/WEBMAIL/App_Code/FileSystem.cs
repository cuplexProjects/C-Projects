using System;
using System.Globalization;
using System.IO;
using MailBee;
using MailBee.Mime;

namespace WebMail
{
	/// <summary>
    /// This class implements an API for managing accounts, folders, messages
    /// through filesystem.
    /// </summary>
	public class FileSystem
	{
		protected string _rootFolder;
		protected string _accountName;
		protected int _accountID = 0;
		protected int _accountHierarchyDepth = 1;

		public string RootFolder
		{
			get { return _rootFolder; }
			set { _rootFolder = value; }
		}

		public string AccountName
		{
			get { return _accountName; }
			set { _accountName = value; }
		}

		public FileSystem(string accountName, int accountID, bool isMailFolder) : this(GetDataFolderFromSettings(isMailFolder), accountName, accountID)
		{
		}

		public FileSystem(string rootFolder, string accountName, int accountID)
		{
			_rootFolder = rootFolder;
			_accountName = accountName;
			_accountID = accountID;
		}

		public MailMessage LoadMessage(int id, string folder)
		{
            folder = FileSystem.ToValidPathName(folder);
            try
			{
				string folderName = CreateFolderFullPath(folder);
				string filename = Path.Combine(folderName, string.Format("{0}.eml", id));
				MailMessage msg = null;
				if (File.Exists(filename))
				{
					msg = new MailMessage();
					msg.LoadMessage(filename);
				}
				return msg;
			}
			catch (ArgumentException ex)
			{
                Log.WriteException(ex);
                throw new WebMailIOException(ex);
			}
			catch (MailBeeException ex)
			{
                Log.WriteException(ex);
                throw new WebMailMailBeeException(ex);
			}
		}

		public void SaveMessage(MailMessage msg, long id, string folder)
		{
            folder = FileSystem.ToValidPathName(folder);
            try
			{
				string folderName = CreateFolderFullPath(folder);
                if (!Directory.Exists(folderName))
                {
                    Directory.CreateDirectory(folderName);
                }
				string filename = Path.Combine(folderName, string.Format("{0}.eml", id));
				msg.SaveMessage(filename);
			}
			catch (ArgumentException ex)
			{
                Log.WriteException(ex);
                throw new WebMailIOException(ex);
			}
			catch (MailBeeException ex)
			{
                Log.WriteException(ex);
                throw new WebMailMailBeeException(ex);
			}
		}

		public void DeleteMessages(int[] ids, string folder)
		{
            folder = FileSystem.ToValidPathName(folder);
            try
			{
				string folderName = CreateFolderFullPath(folder);
				foreach (int id in ids)
				{
					string filename = Path.Combine(folderName, string.Format("{0}.eml", id));
					if (File.Exists(filename))
					{
						File.Delete(filename);
					}
				}
			}
			catch (ArgumentException ex)
			{
                Log.WriteException(ex);
                throw new WebMailIOException(ex);
			}
			catch (UnauthorizedAccessException ex)
			{
                Log.WriteException(ex);
                throw new WebMailIOException(ex);
			}
			catch (IOException ex)
			{
                Log.WriteException(ex);
                throw new WebMailIOException(ex);
			}
		}

		public void DeleteFolder(string folder)
		{
            folder = FileSystem.ToValidPathName(folder);
            try
			{
				string folderToDelete = CreateFolderFullPath(folder);
				if (Directory.Exists(folderToDelete))
				{
					Directory.Delete(folderToDelete, true);
				}
			}
			catch (ArgumentException ex)
			{
                Log.WriteException(ex);
                throw new WebMailIOException(ex);
			}
			catch (UnauthorizedAccessException ex)
			{
                Log.WriteException(ex);
                throw new WebMailIOException(ex);
			}
			catch (IOException ex)
			{
                Log.WriteException(ex);
                throw new WebMailIOException(ex);
			}
		}

		public void DeleteAccount()
		{
			DeleteFolder("");
		}

		public void CreateAccount()
		{
			WebmailResourceManager resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();

			if (!IsFolderNameValid(_accountName))
			{
				throw new WebMailIOException(resMan.GetString("CantCreateAccount"));
			}

			CreateFolder("");
		}

		public void RenameAccount(string newName)
		{
			WebmailResourceManager resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();

			if (!IsFolderNameValid(_accountName))
			{
				throw new WebMailIOException(resMan.GetString("PROC_CANT_UPDATE_ACCT"));
			}

			try
			{
				string sourceDirName = CreateFolderFullPath("");
				_accountName = newName;
				string destDirName = CreateFolderFullPath("");
				if (Directory.Exists(sourceDirName))
				{
					string rootDir = Path.GetDirectoryName(destDirName);
					Directory.CreateDirectory(rootDir);
					Directory.Move(sourceDirName, destDirName);
				}
				else
				{
					throw new DirectoryNotFoundException();
				}
			}
			catch (ArgumentException ex)
			{
                Log.WriteException(ex);
                throw new WebMailIOException(ex);
			}
			catch (IOException ex)
			{
                Log.WriteException(ex);
                throw new WebMailIOException(ex);
			}
			catch (UnauthorizedAccessException ex)
			{
                Log.WriteException(ex);
                throw new WebMailIOException(ex);
			}
		}

		/// <summary>
		/// Create folder on disk
		/// </summary>
		/// <param name="folder"></param>
		/// <returns>Created folder full path.</returns>
		public string CreateFolder(string folder)
		{
            folder = FileSystem.ToValidPathName(folder);
            try
			{
				string folderToCreate = CreateFolderFullPath(folder);
				if (!Directory.Exists(folderToCreate))
				{
					Directory.CreateDirectory(folderToCreate);
				}
				return folderToCreate;
			}
			catch (ArgumentException ex)
			{
                Log.WriteException(ex);
                throw new WebMailIOException(ex);
			}
			catch (UnauthorizedAccessException ex)
			{
                Log.WriteException(ex);
                throw new WebMailIOException(ex);
			}
			catch (IOException ex)
			{
                Log.WriteException(ex);
                throw new WebMailIOException(ex);
			}
		}

		public void CreateFoldersTree(FolderCollection fc)
		{
			foreach (Folder f in fc)
			{
				string delimiter = ((f.ImapFolder != null) && (f.ImapFolder.Delimiter != null)) ? f.ImapFolder.Delimiter : @"\";
                CreateFolder(f.GetFullPath(delimiter));
				if (f.SubFolders.Count > 0)
				{
					CreateFoldersTree(f.SubFolders);
				}
			}
		}

		public void RenameFolder(string oldName, string newName)
		{
            if (string.Compare(oldName, newName, true, CultureInfo.InvariantCulture) != 0)
			{
                try
				{
					string sourceDirName = CreateFolderFullPath(oldName);
					string destDirName = CreateFolderFullPath(newName);
					if (Directory.Exists(sourceDirName))
					{
						Directory.Move(sourceDirName, destDirName);
					}
				}
				catch (ArgumentException ex)
				{
                    Log.WriteException(ex);
                    throw new WebMailIOException(ex);
				}
				catch (UnauthorizedAccessException ex)
				{
                    Log.WriteException(ex);
                    throw new WebMailIOException(ex);
				}
				catch (IOException ex)
				{
                    Log.WriteException(ex);
                    throw new WebMailIOException(ex);
				}
			}
		}

		public bool IsFolderExists(string folder)
		{
			string dirName = CreateFolderFullPath(folder);
			return Directory.Exists(dirName);
		}

		public void Search()
		{
		}

		public void MoveMessages(int[] ids, string folderSrc, string folderDst)
		{
            folderSrc = FileSystem.ToValidPathName(folderSrc);
            folderDst = FileSystem.ToValidPathName(folderDst);
            try
			{
				string sourceFolder = CreateFolderFullPath(folderSrc);
				string destFolder = CreateFolderFullPath(folderDst);

				foreach (int id in ids)
				{
					string sourceFilename = Path.Combine(sourceFolder, string.Format("{0}.eml", id));
					string destFilename = Path.Combine(destFolder, string.Format("{0}.eml", id));
					if (File.Exists(sourceFilename))
					{
						File.Move(sourceFilename, destFilename);
					}
				}
			}
			catch (ArgumentException ex)
			{
                Log.WriteException(ex);
                throw new WebMailIOException(ex);
			}
			catch (UnauthorizedAccessException ex)
			{
                Log.WriteException(ex);
                throw new WebMailIOException(ex);
			}
			catch (IOException ex)
			{
                Log.WriteException(ex);
                throw new WebMailIOException(ex);
			}
		}

		protected string CreateFolderFullPath(string folder)
		{
			Log.WriteLine("CreateFolderFullPath", folder);
			folder = Utils.ConvertToUtf7Modified(folder);
			try
			{
				string returnPath = string.Empty;

				if (_rootFolder != null) returnPath = _rootFolder;

				if (_accountName != null)
				{
					EmailAddress adr = new EmailAddress();
					adr.AsString = _accountName;
					string account = adr.GetAccountName();

					for (int i = 0; i <= _accountHierarchyDepth - 1; i++)
					{
						if (i >= account.Length) break;
						returnPath = Path.Combine(returnPath, account[i].ToString());
					}
					returnPath = Path.Combine(returnPath, string.Format("{0}.{1}", _accountName, _accountID));
				}

				Log.WriteLine("CreateFolderFullPath", Path.Combine(returnPath, folder));
				return Path.Combine(returnPath, folder);
			}
			catch (ArgumentException ex)
			{
                Log.WriteException(ex);
                throw new WebMailIOException(ex);
			}
		}

		protected static string GetDataFolderFromSettings(bool isMailFolder)
		{
			string folder = (isMailFolder) ? Constants.mailFolderName : Constants.tempFolderName;
			string dataFolder = Utils.GetDataFolderPath();
			if (dataFolder != null) 
				return Path.Combine(dataFolder, folder);
			return folder;
		}

		public static bool IsFolderNameValid(string folderName)
		{
			string[] invalidNames = new string[] { "CON", "AUX", "COM1", "COM2", "COM3", "COM4", "LPT1", "LPT2", "LPT3", "PRN", "NUL"};
			foreach (string invalidName in invalidNames)
			{
				if (string.Compare(folderName, invalidName, true, CultureInfo.InvariantCulture) == 0)
				{
					return false;
				}
			}
			if (folderName.IndexOfAny(new char[] {'"', '/', '\\', '*', '?', '<', '>', '|', ':'}) >= 0)
			{
				return false;
			}
			return true;
		}
        
        public static char[] GetInvalidPathChars()
        {
            return new char[] { '?', '|', '<', '>', ':', '/', '*', '"' };
        }

        public static string ToValidPathName(string name)
        {
            char[] charArray = FileSystem.GetInvalidPathChars();

            foreach (char someChar in charArray)
            {
                name = name.Replace(someChar.ToString(), "$ch_" + ((int)someChar).ToString() + "_$");
            }
            return name;
        }

        public static string FromValidPathName(string name)
        {
            char[] charArray = FileSystem.GetInvalidPathChars();

            foreach (char someChar in charArray)
            {
                name = name.Replace("$ch_" + ((int)someChar).ToString() + "_$", someChar.ToString());
            }
            return name;
        }

	}
}
