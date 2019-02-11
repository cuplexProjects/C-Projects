using System;

namespace SecureMemo.DataModels
{
    public class BackupFileInfo : IComparable<BackupFileInfo>
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedDate { get; set; }

        public int CompareTo(BackupFileInfo other)
        {
            return CreatedDate.CompareTo(other.CreatedDate);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}