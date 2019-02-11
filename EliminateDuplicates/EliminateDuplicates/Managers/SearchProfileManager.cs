using System;
using System.Collections.Generic;
using DeleteDuplicateFiles.DataModels;
using DeleteDuplicateFiles.Models;
using DeleteDuplicateFiles.Repositories;
using JetBrains.Annotations;
using Serilog;

namespace DeleteDuplicateFiles.Managers
{
    [UsedImplicitly]
    public class SearchProfileManager : ManagerBase
    {
        private const string ProfileMeta = "2sjSoe+5K1bskBVLBgJLec87ivr33d8MAVIjNr1WmJ9Npna/RWGRdNf5RI0iGh5v+r20lrhi18/E0XHboxDL6suUkQd4u86Hm46in47IJEHFiUU0BnOUr+QlYIrj+AEO";
        private readonly SearchProfileRepository _profileRepository;

        public SearchProfileManager(SearchProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public bool HasLoadedProfile { get; private set; }

        public bool HasUnsavedProfile { get; private set; }

        public bool NewProfileCreated { get; private set; }

        public bool EmptyProfile { get; private set; }

        public SearchProfileModel SearchProfile { get; private set; }


        public bool SaveSearchProfile(string filePath)
        {
            if (SearchProfile != null)
            {
                _profileRepository.SaveSearchProfile(SearchProfile, filePath);
                HasUnsavedProfile = false;
                EmptyProfile = false;
                return true;
            }

            return false;
        }

        public  bool LoadSearchProfile(string filepath)
        {
            try
            {
                var profile = _profileRepository.LoadSearchProfileFromFile(filepath);
                if (profile != null)
                {
                    SearchProfile = profile;
                    return true;
                }
                    
            }
            catch (Exception e)
            {
                Log.Error(e, "LoadSearchProfile failed for file: {filepath}", filepath);
                
            }

            return false;
        }
        
        public void CreateNewProfile(string name)
        {
            SearchProfile = CreateDefaultProfile(name);
            EmptyProfile = true;
            NewProfileCreated = true;
        }

        private static SearchProfileModel CreateDefaultProfile(string profileName)
        {
            return new SearchProfileModel(profileName, new List<ScanFolderModel>(), new List<PreferredDirectoryDataModel>());
        }

        public void LoadeScanFolderList(List<ScanFolderModel> scanFolderList)
        {
            if (scanFolderList == null)
            {
                throw new ArgumentNullException(nameof(scanFolderList));
            }

            //This seems very unlikely
            if (SearchProfile == null)
            {
                SearchProfile = CreateDefaultProfile("New Profile");
            }

            SearchProfile.ScanFolderList = scanFolderList;
        }

        public void LoadPreferredDirecoryList(List<PreferredDirectoryDataModel> preferredDirectories)
        {
            if (preferredDirectories == null)
            {
                throw new ArgumentNullException(nameof(preferredDirectories));
            }

            //This seems very unlikely
            if (SearchProfile == null)
            {
                SearchProfile = CreateDefaultProfile("New Profile");
            }

            SearchProfile.PreferredDirecoryList = preferredDirectories;
        }
    }
}