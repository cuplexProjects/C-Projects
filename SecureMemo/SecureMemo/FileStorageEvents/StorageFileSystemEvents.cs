namespace SecureMemo.FileStorageEvents
{
    public delegate void StorageFileEventHandler(object sender, StorageFileSystemEventArgs e);

    public delegate void StorageDirectoryEventHandler(object sender, StorageDirectorySystemEventArgs e);
}