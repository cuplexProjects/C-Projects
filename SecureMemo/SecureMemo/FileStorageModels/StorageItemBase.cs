using System;

namespace SecureMemo.FileStorageModels
{
    [Serializable]
    public abstract class StorageItemBase
    {
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int SortOrder { get; set; }
    }
}