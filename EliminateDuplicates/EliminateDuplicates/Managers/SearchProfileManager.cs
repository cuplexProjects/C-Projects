using System;
using System.Collections.Generic;
using DeleteDuplicateFiles.Models;
using GeneralToolkitLib.Storage;
using GeneralToolkitLib.Storage.Models;

namespace DeleteDuplicateFiles.Managers
{
    public class SearchProfileManager : ManagerBase
    {
        private readonly StorageManager _storageManager;

        private const string ProfileMeta = "2sjSoe+5K1bskBVLBgJLec87ivr33d8MAVIjNr1WmJ9Npna/RWGRdNf5RI0iGh5v+r20lrhi18/E0XHboxDL6suUkQd4u86Hm46in47IJEHFiUU0BnOUr+QlYIrj+AEO";
        public bool HasLoadedProfile { get; private set; }
        public bool HasUnsavedProfile { get; private set; }
        public bool NewProfileCreated { get; private set; }
        public bool EmptyProfile { get; private set; }

        public event EventHandler OnProfileDataDataChanged;
        public SearchProfile CurrentProfile { get; private set; }

        public SearchProfileManager()
        {
            _storageManager = new StorageManager(StorageManagerSettings.GetDefaultSettings());
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
                CurrentProfile = loadedProfile;

                if (CurrentProfile.PreferredDirecoryList == null)
                    CurrentProfile.PreferredDirecoryList = new List<PreferredDirectory>();
                else
                    CurrentProfile.PreferredDirecoryList.Sort();
            }
        }

        public bool SaveSearchProfile(string filePath)
        {
            if (CurrentProfile != null && _storageManager.SerializeObjectToFile(CurrentProfile, filePath, null))
            {
                HasUnsavedProfile = false;
                EmptyProfile = false;
                return true;
            }
            return false;
        }

        public void CreateNewProfile(string name)
        {
            CurrentProfile?.Dispose();
            CurrentProfile = SearchProfile.CreateDefaultProfile(name);
            CurrentProfile.PropertyChanged += _searchProfile_PropertyChanged;

            EmptyProfile = true;
            OnProfileDataDataChanged?.Invoke(this, new EventArgs());
            NewProfileCreated = true;
        }
    }
}