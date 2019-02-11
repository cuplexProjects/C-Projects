using System;
using System.Collections.Generic;
using System.IO;

namespace DeleteDuplicateFiles.Models
{
    [Serializable]
    public sealed class DuplicateFileModel : Comparer<DuplicateFileModel>, IComparable<DuplicateFileModel>, IEquatable<DuplicateFileModel>
    {
        private readonly int _hashcode;
        public DuplicateFileModel()
        {
            _hashcode = Helpers.Helpers.GenerateHashcode(this);
            DuplicateFiles= new List<DuplicateFileModel>();
        }

        public DateTime LastWriteTime { get; set; }

        public DateTime LastAccessTimeUtc { get; set; }

        public DateTime LastAccessTime { get; set; }

        public DateTime CreationTimeUtc { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime LastWriteTimeUtc { get; set; }

        public string FullName { get; set; }

        public string Name { get; set; }

        public string HashValue { get; set; }

        public string UniqueIdHashValue { get; set; }

        public bool IsMaster { get; set; }

        public List<DuplicateFileModel> DuplicateFiles { get; set; }

        internal int CompareTo(DuplicateFileModel y)
        {
            return FullName.CompareTo(y);
        }

        public long FileSize { get; set; }

        public override int Compare(DuplicateFileModel x, DuplicateFileModel y)
        {
            if (x == null || y == null)
            {
                if (x == null)
                {
                    return y == null ? 0 : -1;
                }

                return 0;
            }

            return x.CompareTo(y);
        }

        public override bool Equals(object obj)
        {
            return obj is DuplicateFileModel file &&
                   LastWriteTime == file.LastWriteTime &&
                   LastAccessTimeUtc == file.LastAccessTimeUtc &&
                   LastAccessTime == file.LastAccessTime &&
                   CreationTimeUtc == file.CreationTimeUtc &&
                   CreationTime == file.CreationTime &&
                   LastWriteTimeUtc == file.LastWriteTimeUtc &&
                   FullName == file.FullName &&
                   Name == file.Name &&
                   HashValue == file.HashValue &&
                   UniqueIdHashValue == file.UniqueIdHashValue &&
                   IsMaster == file.IsMaster &&
                   FileSize == file.FileSize;
        }

        int IComparable<DuplicateFileModel>.CompareTo(DuplicateFileModel other)
        {
            return string.Compare(FullName, other.FullName, StringComparison.Ordinal);
        }

        public string GetDriveLetter()
        {
            var drive = new DriveInfo(FullName);
            return drive.Name;
        }

        public string GetDirectory()
        {
            var dir = new DirectoryInfo(FullName);
            return dir.Name;

        }

        public bool Equals(DuplicateFileModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return LastWriteTime.Equals(other.LastWriteTime) && LastAccessTimeUtc.Equals(other.LastAccessTimeUtc) && LastAccessTime.Equals(other.LastAccessTime) && CreationTimeUtc.Equals(other.CreationTimeUtc) &&
                   CreationTime.Equals(other.CreationTime) && LastWriteTimeUtc.Equals(other.LastWriteTimeUtc) && string.Equals(FullName, other.FullName) && string.Equals(Name, other.Name) && string.Equals(HashValue, other.HashValue) &&
                   string.Equals(UniqueIdHashValue, other.UniqueIdHashValue) && IsMaster == other.IsMaster && Equals(DuplicateFiles, other.DuplicateFiles) && FileSize == other.FileSize;
        }

        public override int GetHashCode()
        {
            return _hashcode;
        }
    }
}