using System;
using System.Collections.Generic;

namespace SecureMemo.FileStorageModels
{
    [Serializable]
    public class StorageDirectory : StorageItemBase
    {
        public StorageDirectory()
        {
            SubDirectories = new List<StorageDirectory>();
            Files = new List<StorageFile>();
        }

        public bool IsRoot
        {
            get { return ParentDirectory == null; }
        }

        public StorageDirectory ParentDirectory { get; set; }
        public List<StorageDirectory> SubDirectories { get; protected set; }
        public List<StorageFile> Files { get; protected set; }
        public string FullPath { get; set; }
        public string DirectoryName { get; set; }
        public int ParentId { get; set; }
        public int Id { get; set; }
    }
}