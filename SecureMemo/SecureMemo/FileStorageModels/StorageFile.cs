using System;

namespace SecureMemo.FileStorageModels
{
    [Serializable]
    public class StorageFile : StorageItemBase
    {
        public string FullPath { get; set; }
        public string Extention { get; set; }
        public long FileSize { get; set; }
        public int Attributes { get; set; }
        public int FileContentId { get; set; }
        public int Id { get; set; }
        public int DirectoryId { get; set; }
        public string FileName { get; set; }
    }
}