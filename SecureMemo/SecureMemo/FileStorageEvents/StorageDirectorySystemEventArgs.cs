using System;

namespace SecureMemo.FileStorageEvents
{
    public class StorageDirectorySystemEventArgs : EventArgs
    {
        public int DirectoryId { get; set; }
        public int ParentDirectoryId { get; set; }
        public StorageFileSystemEventTypes DirectoryEventType { get; set; }
    }
}