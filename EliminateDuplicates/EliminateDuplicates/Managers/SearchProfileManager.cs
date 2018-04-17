using System;
using System.Collections.Generic;
using DeleteDuplicateFiles.DataModels;
using DeleteDuplicateFiles.Models;
using GeneralToolkitLib.Storage;
using GeneralToolkitLib.Storage.Models;
using JetBrains.Annotations;

namespace DeleteDuplicateFiles.Managers
{
    [UsedImplicitly]
    public class SearchProfileManager : ManagerBase
    {
        private const string ProfileMeta = "2sjSoe+5K1bskBVLBgJLec87ivr33d8MAVIjNr1WmJ9Npna/RWGRdNf5RI0iGh5v+r20lrhi18/E0XHboxDL6suUkQd4u86Hm46in47IJEHFiUU0BnOUr+QlYIrj+AEO";

        private readonly StorageManager _storageManager;

        public SearchProfileManager()
        {
            _storageManager = new StorageManager(StorageManagerSettings.GetDefaultSettings());
        }

        public bool HasLoadedProfile { get; private set; }

        public bool HasUnsavedProfile { get; private set; }

        public bool NewProfileCreated { get; private set; }

        public bool EmptyProfile { get; private set; }

        public SearchProfileModel CurrentProfileModel { get; private set; }


        public bool SaveSearchProfile(string filePath)
        {
            if (CurrentProfileModel != null && _storageManager.SerializeObjectToFile(CurrentProfileModel, filePath, null))
            {
                HasUnsavedProfile = false;
                EmptyProfile = false;
                return true;
            }

            return false;
        }

        public void ReplaceCurrentProfile(SearchProfileModel searchProfile) 
        {
            CurrentProfileModel = searchProfile ?? throw new ArgumentNullException(nameof(searchProfile));
        }

        public void CreateNewProfile(string name)
        {
            CurrentProfileModel = CreateDefaultProfile(name);
            EmptyProfile = true;
            NewProfileCreated = true;
        }

        private static SearchProfileModel CreateDefaultProfile(string profileName)
        {
            return new SearchProfileModel(profileName, new List<ScanFolderListItem>(), new List<PreferredDirectoryDataModel>());
        }

        public void LoadeScanFolderList(List<ScanFolderListItem> scanFolderList)
        {
            if (scanFolderList == null)
            {
                throw new ArgumentNullException(nameof(scanFolderList));
            }

            //This seems very unlikely
            if (CurrentProfileModel == null)
            {
                CurrentProfileModel = CreateDefaultProfile("New Profile");
            }

            CurrentProfileModel.ScanFolderList = scanFolderList;
        }

        public void LoadPreferredDirecoryList(List<PreferredDirectoryDataModel> preferredDirectories)
        {
            if (preferredDirectories == null)
            {
                throw new ArgumentNullException(nameof(preferredDirectories));
            }

            //This seems very unlikely
            if (CurrentProfileModel == null)
            {
                CurrentProfileModel = CreateDefaultProfile("New Profile");
            }

            CurrentProfileModel.PreferredDirecoryList = preferredDirectories;
        }
    }
}