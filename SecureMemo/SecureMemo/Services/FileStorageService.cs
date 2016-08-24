using System;
using SecureMemo.FileStorageModels;

namespace SecureMemo.Services
{
    public class FileStorageService : IDisposable
    {
        private static FileStorageService _instance;

        private FileStorageService()
        {
            CreateEmptyFileSystem();
        }

        public StorageFileSystem FileSystem { get; private set; }

        public static FileStorageService Service
        {
            get { return _instance ?? (_instance = new FileStorageService()); }
        }

        public void Dispose()
        {
            _instance = null;
            FileSystem = null;
        }

        public void Save(string databaseFilePath)
        {
            FileSystem.SaveToFile(databaseFilePath);
        }

        public void Load(string databaseFilePath)
        {
            FileSystem = StorageFileSystem.LoadFileSystem(databaseFilePath);
        }

        private void CreateEmptyFileSystem()
        {
            FileSystem = StorageFileSystem.CreateNewFileSystem();
        }
    }
}