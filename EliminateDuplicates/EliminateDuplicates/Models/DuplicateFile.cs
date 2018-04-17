using System;
using System.Collections.Generic;
using System.IO;
using AutoMapper;
using DeleteDuplicateFiles.DataModels;

namespace DeleteDuplicateFiles.Models
{
    [Serializable]
    public sealed class DuplicateFile : Comparer<DuplicateFile>, IComparable<DuplicateFile>, IEquatable<DuplicateFile>
    {
        public DuplicateFile()
        {
           
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

        public List<DuplicateFile> DuplicateFiles { get; set; }

        internal int CompareTo(DuplicateFile y)
        {
            return FileSize.CompareTo(y.FileSize);
        }

        public long FileSize { get; set; }

        public override int Compare(DuplicateFile x, DuplicateFile y)
        {
            if (x == null || y == null)
            {
                if (x == null)
                {
                    return y == null ? 0 : -1;
                }

                return 0;
            }
            return x.FileSize == y.FileSize ? 0 : (x.FileSize > y.FileSize ? 1 : -1);
        }

        public override bool Equals(object obj)
        {
            return obj is DuplicateFile file &&
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

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = LastWriteTime.GetHashCode();
                hashCode = (hashCode * 397) ^ LastAccessTimeUtc.GetHashCode();
                hashCode = (hashCode * 397) ^ LastAccessTime.GetHashCode();
                hashCode = (hashCode * 397) ^ CreationTimeUtc.GetHashCode();
                hashCode = (hashCode * 397) ^ CreationTime.GetHashCode();
                hashCode = (hashCode * 397) ^ LastWriteTimeUtc.GetHashCode();
                hashCode = (hashCode * 397) ^ (FullName != null ? FullName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (HashValue != null ? HashValue.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (UniqueIdHashValue != null ? UniqueIdHashValue.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsMaster.GetHashCode();
                hashCode = (hashCode * 397) ^ FileSize.GetHashCode();
                return hashCode;
            }
        }


        int IComparable<DuplicateFile>.CompareTo(DuplicateFile other)
        {
            return FileSize.CompareTo(other.FileSize);
        }

        bool IEquatable<DuplicateFile>.Equals(DuplicateFile other)
        {
            return FileSize == other?.FileSize && HashValue == other.HashValue;

        }

        public string GetDriveLetter()
        {
           var drive= new DriveInfo(FullName);
            return drive.Name;
        }

        public string GetDirectory()
        {
            var dir = new DirectoryInfo(FullName);
            return dir.Name;

        }
    }
}