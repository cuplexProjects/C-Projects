using System;

namespace DeleteDuplicateFiles.Models
{
    public class ScanFolderModel : IComparable<ScanFolderModel>, IEquatable<ScanFolderModel>
    {
        public string FullPath;
        public int SortOrder;

        public string Drive
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(FullPath) && FullPath.Length >= 3)
                    return FullPath.Substring(0, 3);
                return null;
            }
        }

        public ScanFolderModel()
        {
            SortOrder = -1;
        }

        public bool Contains(ScanFolderModel other)
        {
            if (FullPath == null || other.FullPath == null)
                throw new ArgumentException("FullPath property can no be null");

            if (Drive != other.Drive) return false;
            string[] pathStringsBase; 
            string[] pathStrings; 

            if (FullPath.Length <= other.FullPath.Length)
            {
                pathStringsBase = FullPath.TrimEnd('\\').Split("\\".ToCharArray());
                pathStrings = other.FullPath.TrimEnd('\\').Split("\\".ToCharArray());
            }
            else
            {
                pathStringsBase = other.FullPath.TrimEnd('\\').Split("\\".ToCharArray());
                pathStrings = FullPath.TrimEnd('\\').Split("\\".ToCharArray());
            }

            for (int i = 0; i < pathStringsBase.Length; i++)
            {
                if (pathStringsBase[i] != pathStrings[i])
                    return false;
            }

            return true;
        }

        public int CompareTo(ScanFolderModel other)
        {
            if (SortOrder >= 0 && other.SortOrder >= 0)
                return SortOrder.CompareTo(other.SortOrder);

            if (FullPath != null && other.FullPath != null)
                return string.Compare(FullPath, other.FullPath, StringComparison.Ordinal);

            if (FullPath == null)
                return 1;
            if (other.FullPath == null)
                return -1;

            return 0;
        }

        public bool Equals(ScanFolderModel other)
        {
            if (other == null)
                return false;

            if (FullPath != null && other.FullPath != null)
                return FullPath == other.FullPath;

            return GetHashCode() == other.GetHashCode();
        }

        public override string ToString()
        {
            return FullPath;
        }
    }
}