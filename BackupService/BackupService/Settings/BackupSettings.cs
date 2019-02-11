using BackupService.Utils;
using System;
using System.ComponentModel;
using System.Linq;
using GeneralToolkitLib.Converters;

namespace BackupService.Settings
{
    [Serializable]
    public class BackupSettings
    {     
        private int _backupTime;
        public SortableBindingList<BackupFolder> BackupFolders { get; set; }
        public string BackupDestinationPath { get; set; }
        public string BackupPassword { get; protected set; }
        public bool UsingDefaultPassword { get; set; }
        public BackupFrequences BackupRecurrence { get; set; }
        public DateTime StartDate { get; set; }
        public int BackupTime {
            get { return _backupTime; }
            set { _backupTime = value % 86400; }
        }
        public void SetPassword(string password)
        {
            BackupPassword = SHA2Hash.GetSHA2Hash(BackupService.Utils.SHA2Hash.HashBits.n512, password);
            UsingDefaultPassword = false;
        }

        internal bool TryAddBackupFolder(string path, int i)
        {
            if (!path.EndsWith("\\"))
                path += "\\";

            if (!BackupFolders.Any(b => path.Contains(b.Directory) || b.Directory.Contains(path)))
            {
                BackupFolders.Add(BackupFolder.Create(path));
                return true;
            }
            return false;
        }
    }
    [Serializable]
    public class BackupDesination
    {
        public string BackupFileName { get; set; }
        public string BackupPath { get; set; }
    }

    [Serializable]
    public class BackupFolder : INotifyPropertyChanged, IComparable<BackupFolder>, IEquatable<BackupFolder>
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public static BackupFolder Create(string path)
        {
            BackupFolder bc = new BackupFolder();
            bc.Directory = path;
            bc.RequireUpdate = true;
            bc.WasDeleted = false;
            bc.UniqueId = BitConverter.ToUInt64(Guid.NewGuid().ToByteArray(), 0);
            return bc;
        }

        public void SetAsDeleted()
        {
            this.WasDeleted = true;
            this.RequireUpdate = false;
        }

        protected BackupFolder() { }        

        [Browsable(false)]
        public ulong UniqueId { get; protected set; }

        [Browsable(false)]
        public bool RequireUpdate { get; set; }
        
        public string Directory { get; set; }       

        public int SubFolders { get; set; }
        public int Files { get; set; }

        [Browsable(false)]
        public long DataSize { get; set; }

        [Browsable(false)]
        public bool WasDeleted { get; private set; }

        public string Size
        {
            get { return GeneralConverters.FormatFileSizeToString(this.DataSize); }
        }

        public void TriggerFullUpdate()
        {
            SettingsService.Instance.BackupFolderItemUpdated(this);
        }

        public void TriggerFullUpdateByNativeThread()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Files"));
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("SubFolders"));
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("DataSize"));
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Size"));                
            }
        }

        public int CompareTo(BackupFolder other)
        {
            return String.CompareOrdinal(Directory, other.Directory);
        }

        public bool Equals(BackupFolder other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(UniqueId, other.UniqueId);
        }
    }

    [Serializable]
    public enum BackupFrequences
    {
        Once = 1,
        Daily = 2,
        Weekly = 3,
        Monthly = 4,
    }
}
