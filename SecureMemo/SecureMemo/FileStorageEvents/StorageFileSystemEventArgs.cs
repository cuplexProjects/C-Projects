using System;

namespace SecureMemo.FileStorageEvents
{
    public class StorageFileSystemEventArgs : EventArgs
    {
        public int FileId { get; set; }
        public int DirectoryId { get; set; }
        public StorageFileSystemEventTypes FileEvent { get; set; }
    }

    public enum StorageFileSystemEventTypes
    {
        Created,
        Deleted,
        Renamed
    }
}