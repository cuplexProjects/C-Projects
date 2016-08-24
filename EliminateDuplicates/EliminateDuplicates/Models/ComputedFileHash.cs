#region

using System;
using System.Runtime.Serialization;

#endregion

namespace DeleteDuplicateFiles.Models
{
    [DataContract]
    [Serializable]
    public class ComputedFileHash
    {
        [DataMember(Name = "FullPath", Order = 1)]
        public string FullPath { get; set; }

        [DataMember(Name = "FileSize", Order = 2)]
        public long FileSize { get; set; }

        [DataMember(Name = "LastWriteTime", Order = 3)]
        public DateTime LastWriteTime { get; set; }

        [DataMember(Name = "CreationTime", Order = 4)]
        public DateTime CreationTime { get; set; }

        [DataMember(Name = "CRC32HashValue", Order = 5)]
        public string CRC32HashValue { get; set; }

        [DataMember(Name = "MD5HashValue", Order = 6)]
        public string MD5HashValue { get; set; }

        public string DriveLetter
        {
            get
            {
                if (FullPath != null && FullPath.Length > 3)
                    return FullPath.Substring(0, 3);
                return "";
            }
        }
    }
}