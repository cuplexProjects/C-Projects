using System;
using System.Threading.Tasks;
using DeleteDuplicateFiles.DataModels;
using DeleteDuplicateFiles.Managers;
using GeneralToolkitLib.Storage;
using GeneralToolkitLib.Storage.Models;
using JetBrains.Annotations;

namespace DeleteDuplicateFiles.DataSource
{
    [UsedImplicitly]
    public class FileDataContext : DataContextBase
    {
        private FileHashCollectionDataModel _dataCollection;
        private readonly PasswordManager _passwordManager;
        private readonly AppSettingsManager _settingsManager;

        public FileDataContext(PasswordManager passwordManager, AppSettingsManager settingsManager)
        {
            _passwordManager = passwordManager;
            _settingsManager = settingsManager;
            _dataCollection = new FileHashCollectionDataModel();
        }

        public async Task LoadDatabaseAsync(string filename)
        {
            var settings = new StorageManagerSettings {NumberOfThreads = Environment.ProcessorCount, Password = _passwordManager.GetPassword(_settingsManager.ApplicationSettings.DefaultKey), UseEncryption = true, UseMultithreading = true};
            StorageManager storage = new StorageManager(settings);
            _dataCollection = await storage.DeserializeObjectFromFileAsync<FileHashCollectionDataModel>(filename, null);
        }

        public async Task SaveDatabaseAsync(string filename)
        {
            var settings = new StorageManagerSettings {NumberOfThreads = Environment.ProcessorCount, Password = _passwordManager.GetPassword(_settingsManager.ApplicationSettings.DefaultKey), UseEncryption = true, UseMultithreading = true};
            StorageManager storage = new StorageManager(settings);
            _dataCollection.LastModified = DateTime.Now;
            await storage.SerializeObjectToFileAsync(_dataCollection, filename, null);
        }

        public ComputedFileHashDataModel this[string key]
        {
            get => _dataCollection.FileHashDictionary[key];
            set => _dataCollection.FileHashDictionary[key] = value;
        }

        public bool ContainsKey(string key)
        {
            return _dataCollection.FileHashDictionary.ContainsKey(key);
        }

        public bool ContainsValue(ComputedFileHashDataModel value)
        {
            return _dataCollection.FileHashDictionary.ContainsValue(value);
        }
    }
}
