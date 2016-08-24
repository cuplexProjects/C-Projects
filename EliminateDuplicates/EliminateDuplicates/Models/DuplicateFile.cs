#region

using System;
using System.Collections.Generic;

#endregion

namespace DeleteDuplicateFiles.Models
{
    [Serializable]
    public class DuplicateFile : Comparer<DuplicateFile>, IComparable<DuplicateFile>, IEquatable<DuplicateFile>
    {
        public string Drive {
            get
            {
                if (_drive != null)
                    return _drive;

                if (!string.IsNullOrEmpty(Dir))
                {
                    int index = Dir.IndexOf(@"\", StringComparison.Ordinal);  
                    if (index > 0)
                        _drive = Dir.Substring(0, index + 1);
                }
                else if (!string.IsNullOrEmpty(FullPath))
                {
                    int index = FullPath.IndexOf(@"\", StringComparison.Ordinal);
                    if (index > 0)
                        _drive = FullPath.Substring(0, index + 1);
                }
                return _drive;
            }
        }

        public string Dir
        {
            get { return _dir; }
            set
            {
                _dir = value;
                _drive = null;
            }
        }

        public string Filename { get; set; }

        public string FullPath
        {
            get { return _fullPath; }
            set
            {
                _fullPath = value;
                _drive = null;
            }
        }

        public long FileSize { get; set; }
        public DateTime LastWriteTime { get; set; }
        public DateTime CreationTime { get; set; }
        public string HashValue { get; set; }
        private string _drive = null;

        public string UniqueIdHashValue
        {
            get
            {
                if (_uniqueIdHashValue == null && !string.IsNullOrEmpty(FullPath))
                    _uniqueIdHashValue = ComputedHashService.Instance.GetFileUniqueId(this);

                return _uniqueIdHashValue;
            }
        }

        public bool IsMaster { get; set; }
        public List<DuplicateFile> DuplicateFiles { get; set; }

        private string _uniqueIdHashValue;
        private string _dir;
        private string _fullPath;

        public override int Compare(DuplicateFile x, DuplicateFile y)
        {
            if (x.FileSize > y.FileSize)
                return 1;
            if (x.FileSize < y.FileSize)
                return -1;

            return 0;
        }

        public int CompareTo(DuplicateFile other)
        {
            if (FileSize > other.FileSize)
                return 1;
            if (FileSize < other.FileSize)
                return -1;

            return 0;
        }

        public bool Equals(DuplicateFile other)
        {
            if (HashValue != null && other.HashValue != null)
                return HashValue == other.HashValue;

            if (UniqueIdHashValue != null && other.UniqueIdHashValue != null)
                return UniqueIdHashValue == other.UniqueIdHashValue;

            return GetHashCode() == other.GetHashCode();
        }

        public override string ToString()
        {
            return FullPath;
        }
    }
}