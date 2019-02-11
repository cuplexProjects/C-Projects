using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SecureMemo.FileStorageModels
{
    [DataContract(Name = "StorageFileSystemContent")]
    public class StorageFileSystemContent
    {
        [DataMember(Name = "Directories", Order = 1)]
        public HashSet<StorageDirectory> Directories { get; set; }

        [DataMember(Name = "Files", Order = 2)]
        public HashSet<StorageFile> Files { get; set; }

        [DataMember(Name = "DirectoryIdSet", Order = 3)]
        public HashSet<int> DirectoryIdSet { get; set; }

        [DataMember(Name = "FileIdSet", Order = 4)]
        public HashSet<int> FileIdSet { get; set; }

        [DataMember(Name = "NextDirectoryId", Order = 5)]
        public int NextDirectoryId { get; set; }

        [DataMember(Name = "NextFileId", Order = 6)]
        public int NextFileId { get; set; }
    }
}