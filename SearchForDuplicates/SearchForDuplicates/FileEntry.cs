using System;
using System.Runtime.Serialization;

namespace SearchForDuplicates
{
    [Serializable]
    [DataContract]
    public class FileEntry
    {
        [DataMember(Name = "FileName", Order = 1)]
        public string FileName { get; set; }

        [DataMember(Name = "FilePath", Order = 2)]
        public string FilePath { get; set; }

        [DataMember(Name = "FileSize", Order = 3)]
        public long FileSize { get; set; }

        [DataMember(Name = "CreateDate", Order = 4)]
        public DateTime CreateDate { get; set; }

        [DataMember(Name = "ModifiedDate", Order = 5)]
        public DateTime ModifiedDate { get; set; }

        [DataMember(Name = "LastAccessTime", Order = 6)]
        public DateTime LastAccessTime { get; set; }
    }
}