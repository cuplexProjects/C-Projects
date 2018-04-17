using System;
using System.Collections.Generic;
using DeleteDuplicateFiles.DataModels;
using DeleteDuplicateFiles.Models;
using DeleteDuplicateFiles.Repositories;
using JetBrains.Annotations;
using Serilog;

namespace DeleteDuplicateFiles.Services
{
    [UsedImplicitly]
    public class SearchProfileService : ServiceBase
    {
        private readonly SearchProfileRepository _profileRepository;

        public SearchProfileService(SearchProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public SearchProfileModel LoadSearchProfile(string filePath)
        {
            SearchProfileModel loadedProfile = null;

            try
            {
                loadedProfile = _profileRepository.LoadSearchProfileFromFile(filePath);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to load search profile: {filePath}", filePath);
            }

            return loadedProfile ?? new SearchProfileModel(new List<ScanFolderListItem>(), new List<PreferredDirectoryDataModel>());
        }

        public void SaveSearchProfile(SearchProfileModel searchProfile, string filePath)
        {
            try
            {
                _profileRepository.SaveSearchProfile(searchProfile, filePath);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to load search profile: {filePath}", filePath);
            }
        }
    }
}