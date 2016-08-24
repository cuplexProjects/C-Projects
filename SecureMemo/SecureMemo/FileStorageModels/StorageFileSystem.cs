using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using GeneralToolkitLib.Log;
using SecureMemo.FileStorageEvents;

namespace SecureMemo.FileStorageModels
{
    public class StorageFileSystem
    {
        private const string fsStructureFileName = "FileSystem.smfs";
        private const string validDirectoryNameRegExp = @"^[\w\._-]+$";
        private readonly HashSet<StorageDirectory> _directories;
        private readonly HashSet<int> _directoryIdSet;
        private readonly HashSet<int> _fileIdSet;
        private readonly HashSet<StorageFile> _files;
        private readonly StorageDirectory _rootDirectory;
        private readonly Regex _validDirNameRegex;
        private int _nextDirectoryId;
        private int _nextFileId;
        private MemoryStream fileSystemMemoryCache;

        private StorageFileSystem()
        {
            fileSystemMemoryCache = new MemoryStream();
            _directories = new HashSet<StorageDirectory>();
            _files = new HashSet<StorageFile>();
            _directoryIdSet = new HashSet<int>();
            _fileIdSet = new HashSet<int>();
            _rootDirectory = CreateRooDir();
            _nextFileId = 0;
            _validDirNameRegex = new Regex(validDirectoryNameRegExp);
        }

        private StorageFileSystem(StorageFileContent storageFileSystemContent)
        {
            fileSystemMemoryCache = new MemoryStream();
            _directories = storageFileSystemContent.Directories;
            _files = storageFileSystemContent.Files;
            _directoryIdSet = storageFileSystemContent.DirectoryIdSet;
            _fileIdSet = storageFileSystemContent.FileIdSet;
            _rootDirectory = _directories.First(d => d.Id == 0);
            _nextFileId = storageFileSystemContent.NextFileId;
            _nextDirectoryId = storageFileSystemContent.NextDirectoryId;
        }

        public event StorageFileEventHandler FileStructureChanged;
        public event StorageDirectoryEventHandler DirectoryStructureChanged;

        private StorageDirectory CreateRooDir()
        {
            var root = new StorageDirectory {Id = 0, ParentId = 0, CreateDate = DateTime.Now, DirectoryName = "FSRootDir"};
            _directoryIdSet.Add(_nextDirectoryId++);
            return root;
        }

        public int CreateDirectory(StorageDirectory parentDirectory, string directoryName)
        {
            var storageDirectory = new StorageDirectory
            {
                Id = _nextDirectoryId++,
                ParentId = parentDirectory.Id,
                DirectoryName = directoryName,
                CreateDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            _directories.Add(storageDirectory);
            _directoryIdSet.Add(storageDirectory.Id);

            if (DirectoryStructureChanged != null)
                DirectoryStructureChanged.Invoke(this,
                    new StorageDirectorySystemEventArgs {DirectoryEventType = StorageFileSystemEventTypes.Created, DirectoryId = storageDirectory.Id, ParentDirectoryId = storageDirectory.ParentId});

            return storageDirectory.Id;
        }

        public bool DeleteDirectory(int directoryId)
        {
            StorageDirectory storageDirectory = _directories.FirstOrDefault(d => d.Id == directoryId);
            if (storageDirectory == null)
                return false;

            _directories.Remove(storageDirectory);
            _directoryIdSet.Remove(storageDirectory.Id);

            if (DirectoryStructureChanged != null)
                DirectoryStructureChanged.Invoke(this,
                    new StorageDirectorySystemEventArgs {DirectoryEventType = StorageFileSystemEventTypes.Deleted, DirectoryId = directoryId, ParentDirectoryId = storageDirectory.ParentId});

            return true;
        }

        public bool RenameDirectory(int directoryId, string newDirectoryName)
        {
            if (!IsValidDirectoryName(newDirectoryName))
                return false;

            StorageDirectory storageDirectory = _directories.FirstOrDefault(d => d.Id == directoryId);
            if (storageDirectory == null)
                return false;

            storageDirectory.DirectoryName = newDirectoryName;

            if (DirectoryStructureChanged != null)
                DirectoryStructureChanged.Invoke(this,
                    new StorageDirectorySystemEventArgs {DirectoryEventType = StorageFileSystemEventTypes.Renamed, DirectoryId = directoryId, ParentDirectoryId = storageDirectory.ParentId});

            return true;
        }

        public bool IsValidDirectoryName(string directoryName)
        {
            if (string.IsNullOrEmpty(directoryName))
                return false;

            return _validDirNameRegex.IsMatch(directoryName);
        }

        public int CreateFile(StorageDirectory parentDirectory, string fileName)
        {
            var storageFile = new StorageFile
            {
                CreateDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Id = _nextFileId++,
                DirectoryId = parentDirectory.Id,
                FileName = fileName
            };

            _fileIdSet.Add(storageFile.Id);
            _files.Add(storageFile);

            if (FileStructureChanged != null)
                FileStructureChanged.Invoke(this, new StorageFileSystemEventArgs {DirectoryId = storageFile.DirectoryId, FileId = storageFile.Id, FileEvent = StorageFileSystemEventTypes.Created});

            return storageFile.Id;
        }

        public StorageDirectory GetRootDirectory()
        {
            return _rootDirectory;
        }

        public StorageDirectory GetSelectedDir(string directoryName)
        {
            return null;
        }

        public StorageDirectory GetDirectory(int directoryId)
        {
            return _directories.FirstOrDefault(d => d.Id == directoryId);
        }

        public List<StorageDirectory> GetDirectories(int parentDirectoryId)
        {
            return _directories.Where(d => d.ParentId == parentDirectoryId).ToList();
        }

        public List<StorageFile> GetFiles(int directoryId)
        {
            return _files.Where(f => f.DirectoryId == directoryId).ToList();
        }

        public void SaveToFile(string DirectoryPath)
        {
            FileStream fs = null;
            try
            {
                fs = File.OpenWrite(DirectoryPath + "\\" + fsStructureFileName);
                var binaryFormatter = new BinaryFormatter();
                var fileSystemContent = new StorageFileContent
                {
                    Directories = _directories,
                    DirectoryIdSet = _directoryIdSet,
                    FileIdSet = _fileIdSet,
                    Files = _files,
                    NextDirectoryId = _nextDirectoryId,
                    NextFileId = _nextFileId
                };

                binaryFormatter.Serialize(fs, fileSystemContent);
                fs.Flush();
            }
            catch (Exception ex)
            {
                LogWriter.LogError("Exception in SaveToFile()", ex);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        public static StorageFileSystem LoadFileSystem(string DirectoryPath)
        {
            FileStream fs = null;
            try
            {
                fs = File.OpenRead(DirectoryPath + "\\" + fsStructureFileName);
                var binaryFormatter = new BinaryFormatter();
                var storageFileSystemContent = binaryFormatter.Deserialize(fs) as StorageFileContent;
                return new StorageFileSystem(storageFileSystemContent);
            }
            catch (Exception ex)
            {
                LogWriter.LogError("Error loading filesystem", ex);
                return null;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        public static StorageFileSystem CreateNewFileSystem()
        {
            var storageFileSystem = new StorageFileSystem();
            return storageFileSystem;
        }

        public class StorageFileDataRange
        {
            public long EndPosition;
            public int FileId;
            public long StartPosition;
        }
    }
}