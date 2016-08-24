using System.IO;
using MailBee.ImapMail;

namespace WebMail
{
   
    public class FileMessage
    {
        #region Fields
        protected string _uid;
        protected string _fileName;
        protected string _folderFullPath;
        protected string _flagsStr;
        protected long _size;
        protected SystemMessageFlags _flags;
        #endregion

        #region Properties
        public string UID
        {
            get { return _uid; }
        }

        public string FileName
        {
            get { return _fileName; }
        }

        public string FolderFullPath
        {
            get { return _folderFullPath; }
        }

        public string FlagsStr
        {
            get { return _flagsStr; }
            set { _flagsStr = value; }
        }

        public long Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public SystemMessageFlags Flags
        {
            get { return _flags; }
            set { _flags = value; }
        }
        #endregion

        #region FileMessage Members
        public FileMessage(string FolderFullPath)
        {
            _uid = string.Empty;
            _fileName = string.Empty;
            _folderFullPath = FolderFullPath;
            _flagsStr = string.Empty;
            _flags = SystemMessageFlags.None;
        }

        public void Load(string str, bool uid)
        {
            if (uid) LoadByUid(str);
            else LoadByName(str);
        }

        public void Save()
        {
            WebmailResourceManager _resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
            _flagsStr = Utils.SystemMessageFlagsToStr(_flags);
            string FullOldFileName = Path.Combine(_folderFullPath, _fileName);
            string NewFileName;
            if (_flagsStr.Trim() != string.Empty)
                NewFileName = _uid + Constants.SIZE_DELIMETER + _size + Constants.FLAG_DELIMETER + _flagsStr;
            else
                NewFileName = _uid + Constants.SIZE_DELIMETER + _size + Constants.FLAG_DELIMETER;
            string FullFileName = Path.Combine(_folderFullPath, NewFileName);
            if (File.Exists(FullOldFileName))
            {
                File.Move(FullOldFileName, FullFileName);
            }
            else
            {
                throw new WebMailWMServerException(_resMan.GetString("PROC_MSG_HAS_DELETED"));
            }
        }

        protected void LoadByUid(string uid)
        {
            WebmailResourceManager _resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
            _uid = uid;
            if (Directory.Exists(_folderFullPath))
            {
                DirectoryInfo dir = new DirectoryInfo(_folderFullPath);
                FileInfo[] files = dir.GetFiles(uid + "*");
                foreach (FileInfo file in files)
                {
                    string tuid = GetUIDByFileName(file.Name);
                    if (tuid == uid)
                    {
                        _fileName = file.Name;
                        _flagsStr = GetFlagsByFileName(file.Name);
                        _size = GetSizeByFileName(file.Name);
                        _flags = Utils.StrToSystemMessageFlags(_flagsStr);
                        break;
                    }
                }
            }
            else
            {
                throw new WebMailWMServerException(_resMan.GetString("PROC_MSG_HAS_DELETED"));
            }
        }

        protected void LoadByName(string filename)
        {
            _fileName = filename;
            _uid = GetUIDByFileName(filename);
            _flagsStr = GetFlagsByFileName(filename);
            _size = GetSizeByFileName(filename);
            _flags = Utils.StrToSystemMessageFlags(this._flagsStr);
        }

        public static string GetUIDByFileName(string filename)
        {
            int pos = filename.LastIndexOf(Constants.SIZE_DELIMETER);
        	string result = pos != -1 ? filename.Substring(0, pos) : filename;
            return result;
        }

        protected string GetFlagsByFileName(string filename)
        {
            int pos = filename.LastIndexOf(Constants.FLAG_DELIMETER);
            string result = string.Empty;
            if (pos != -1)
                result = filename.Substring(pos + Constants.FLAG_DELIMETER.Length);
            return result;
        }

        protected long GetSizeByFileName(string filename)
        {
            int pos_flag = filename.LastIndexOf(Constants.FLAG_DELIMETER);
            int pos_size = filename.LastIndexOf(Constants.SIZE_DELIMETER);
            long result = 0;
            string resultstr = "";
            if (pos_flag != -1 && pos_size != -1)
            {
                resultstr = filename.Substring(0, pos_flag);
                resultstr = resultstr.Substring(pos_size + Constants.SIZE_DELIMETER.Length);
            }
            if (!long.TryParse(resultstr, out result))
                result = 0;
            return result;
        }
        #endregion
    }
}
