using System;
using System.Collections.Generic;
using DeleteDuplicateFiles.Models;
using GeneralToolkitLib.Storage;
using GeneralToolkitLib.Storage.Models;

namespace DeleteDuplicateFiles
{
    public class SearchProfileManager
    {
        private SearchProfile _searchProfile;
        private readonly StorageManager _storageManager;
        private const string ProfileMeta = "2sjSoe+5K1bskBVLBgJLec87ivr33d8MAVIjNr1WmJ9Npna/RWGRdNf5RI0iGh5v+r20lrhi18/E0XHboxDL6suUkQd4u86Hm46in47IJEHFiUU0BnOUr+QlYIrj+AEO";
        public bool HasLoadedProfile { get; private set; }
        public bool HasUnsavedProfile { get; private set; }
        public bool NewProfileCreated { get; private set; }
        public bool EmptyProfile { get; private set; }

        public event EventHandler OnProfileDataDataChanged;
        public SearchProfile CurrentProfile => _searchProfile;

        public SearchProfileManager()
        {
            StorageManagerSettings settings = new StorageManagerSettings(false, 1, true, ProfileMeta);
            _storageManager = new StorageManager(settings);
        }

        private void _searchProfile_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            HasUnsavedProfile = true;
        }

        public void OpenSearchProfile(string filePath)
        {
            var loadedProfile = _storageManager.DeserializeObjectFromFile<SearchProfile>(filePath, null);
            if (loadedProfile != null)
            {
                HasLoadedProfile = true;
                HasUnsavedProfile = false;
                EmptyProfile = false;
                _searchProfile = loadedProfile;
                if (_searchProfile.PreferredDirecoryList == null)
                    _searchProfile.PreferredDirecoryList = new List<PreferredDirectory>();
                else
                    _searchProfile.PreferredDirecoryList.Sort();
            }
        }

        public bool SaveSearchProfile(string filePath)
        {
            if (_searchProfile != null && _storageManager.SerializeObjectToFile(_searchProfile, filePath, null))
            {
                HasUnsavedProfile = false;
                EmptyProfile = false;
                return true;
            }
            return false;
        }

        public void CreateNewProfile(string name)
        {
            NewProfileCreated = true;
            _searchProfile?.Dispose();
            _searchProfile = new SearchProfile();
            _searchProfile.PropertyChanged += _searchProfile_PropertyChanged;
            _searchProfile.CreateNewScanFolderList();
            _searchProfile.PreferredDirecoryList = new List<PreferredDirectory>();
            _searchProfile.ProfileName = name;
            EmptyProfile = true;
            OnProfileDataDataChanged?.Invoke(this, new EventArgs());
        }
    }
}