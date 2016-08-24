using System;
using System.Globalization;
using System.IO;
using MailBee.ImapMail;

namespace WebMail
{
	public struct RenameFolderStruct
	{
		public string oldPath;
		public string newPath;
	}

	/// <summary>
	/// Folders properties and methods class.
	/// </summary>
	public class Folder : IComparable
	{
		#region Fields
		protected long _id_folder;
		protected int _id_acct;
		protected long _id_parent;
		protected FolderType _type;
		protected string _name;
        protected string _full_path;
		protected FolderSyncType _sync_type;
		protected bool _hide;
		protected short _fld_order;
		protected MailBee.ImapMail.Folder _imapFolder;

		protected string _updateName;
		protected string _updateFullPath;
		protected long _size;
		protected int _messageCount;
		protected int _unreadMessageCount;

		public const FolderSyncType DefaultInboxSyncType = FolderSyncType.AllEntireMessages;
		public const FolderSyncType DefaultFolderSyncType = FolderSyncType.AllHeadersOnly;

		protected FolderCollection _subFolders;
		#endregion
		
		#region Properties

		public long ID
		{
			get { return _id_folder; }
			set { _id_folder = value; }
		}

		public int IDAcct
		{
			get { return _id_acct; }
			set { _id_acct = value; }
		}

		public long IDParent
		{
			get { return _id_parent; }
			set { _id_parent = value; }
		}

		public FolderType Type
		{
			get { return _type; }
			set { _type = value; }
		}

		public string Name
		{
			get 
            {
                return _name; 
            }
			set 
            {
                _name = value;
            }
		}

        
        public string FullPath
		{
			get { return _full_path; }
			set { _full_path = value; }
		}

		public FolderSyncType SyncType
		{
			get { return _sync_type; }
			set { _sync_type = value; }
		}

		public bool Hide
		{
			get { return _hide; }
			set { _hide = value; }
		}

		public short FolderOrder
		{
			get { return _fld_order; }
			set { _fld_order = value; }
		}

		public FolderCollection SubFolders
		{
			get { return _subFolders; }
		}

		public MailBee.ImapMail.Folder ImapFolder
		{
			get { return _imapFolder; }
			set { _imapFolder = value; }
		}

		public string UpdateName
		{
			get
			{
				if (_updateName != null)
				{
					return _updateName;
				}
				return _name;
			}
			set { _updateName = value; }
		}

		public string UpdateFullPath
		{
			get
			{
				if (_updateFullPath != null)
				{
					return _updateFullPath;
				}
				return _full_path;
			}
			set { _updateFullPath = value; }
		}

		public long Size
		{
			get { return _size; }
			set { _size = value; }
		}

		public int MessageCount
		{
			get { return _messageCount; }
			set { _messageCount = value; }
		}

		public int UnreadMessageCount
		{
			get { return _unreadMessageCount; }
			set { _unreadMessageCount = value; }
		}

		#endregion

		public Folder()
		{
			_id_folder = -1;
			_id_acct = -1;
			_id_parent = -1;
			_type = FolderType.Custom;
			_name = "My Folder";
			_full_path = "";
			_sync_type = FolderSyncType.AllHeadersOnly;
			_hide = false;
			_fld_order = 0;
			_imapFolder = null;
			_subFolders = new FolderCollection();

			_updateName = null;
			_updateFullPath = null;
			_size = _messageCount = _unreadMessageCount = 0;
		}

		public Folder(long idFolder, int idAcct, long idParent, string name, string fullPath, FolderType type, 
			FolderSyncType syncType, bool hide, short folderOrder) : this()
		{
			_id_folder = idFolder;
			_id_acct = idAcct;
			_id_parent = idParent;
			_type = type;
			_name = name;
			_full_path = fullPath;
			_sync_type = syncType;
			_hide = hide;
			_fld_order = folderOrder;
		}

        public Folder(int idAcct, long idParent, string fullPath, string name)
            : this()
        {
            _id_acct = idAcct;
            _id_parent = idParent;
            _full_path = fullPath;

            _name = name;

            switch (_name.ToLower(CultureInfo.InvariantCulture))
            {
                case Constants.FolderNames.InboxLower:
                    _type = FolderType.Inbox;
                    break;
                case Constants.FolderNames.DraftsLower:
                    _type = FolderType.Drafts;
                    break;
                case Constants.FolderNames.SentLower:
                case Constants.FolderNames.SentItemsLower:
                    _type = FolderType.SentItems;
                    break;
                case Constants.FolderNames.TrashLower:
                    _type = FolderType.Trash;
                    break;
                case Constants.FolderNames.SpamLower:
                    _type = FolderType.Spam;
                    break;
                case Constants.FolderNames.QuarantineLower:
                    _type = FolderType.Quarantine;
                    break;
                default:
                    _type = FolderType.Custom;
                    break;
            }
        }
        
        public Folder(MailBee.ImapMail.Folder fld, bool hide): this()
		{
			_imapFolder = fld;
			_name = fld.ShortName;
			_full_path = fld.Name;
			switch (_name.ToLower(CultureInfo.InvariantCulture))
			{
				case Constants.FolderNames.InboxLower:
					_type = FolderType.Inbox;
					break;
				case Constants.FolderNames.DraftsLower:
					_type = FolderType.Drafts;
					break;
				case Constants.FolderNames.SentLower:
				case Constants.FolderNames.SentItemsLower:
					_type = FolderType.SentItems;
					break;
                case Constants.FolderNames.TrashLower:
                    _type = FolderType.Trash;
                    break;
                case Constants.FolderNames.SpamLower:
                    _type = FolderType.Spam;
                    break;
                default:
					_type = FolderType.Custom;
					break;
			}
			_hide = hide;
		}

		public static Folder InitSubFolder(Folder subFolder, Folder parentFolder)
		{
			string full_path = Path.Combine(parentFolder.FullPath, ImapUtils.ToUtf7String(subFolder.Name));
			return new Folder(subFolder.ID, parentFolder.IDAcct, parentFolder.ID, subFolder.Name, full_path, subFolder.Type, subFolder.SyncType, subFolder.Hide, subFolder.FolderOrder);
		}

		#region IComparable Members

		public int CompareTo(object obj)
		{
			if (obj == null) 
			{
				return 1;
			}
			Folder fobj = obj as Folder;
			if (fobj == null) 
			{
				throw new ArgumentException("Folder compare error (Not a Folder object)");
			}
			if (_fld_order < fobj._fld_order) return -1;
			if (_fld_order == fobj._fld_order) return 0;
			if (_fld_order > fobj._fld_order) return 1;

			throw new ArgumentException("Folder compare error");
		}

		#endregion

		public static string CreateNewFullPath(string oldFullPath, string delimiter, string newName)
		{
			string newFullPath = newName;
			int lastDelimiterIndex = oldFullPath.LastIndexOf(delimiter);
			if (lastDelimiterIndex > 0)
			{
				string parentPath = oldFullPath.Substring(0, lastDelimiterIndex);
				newFullPath = string.Format("{0}{1}{2}", parentPath, delimiter, newName);
			}
			return newFullPath;
		}

		public string GetFullPath(string delimiter)
		{
			return _full_path.Replace(delimiter, @"\");
		}

		public string GetUpdateFullPath(string delimiter)
		{
			return UpdateFullPath.Replace(delimiter, @"\");
		}

	}// END CLASS DEFINITION Folder
}
