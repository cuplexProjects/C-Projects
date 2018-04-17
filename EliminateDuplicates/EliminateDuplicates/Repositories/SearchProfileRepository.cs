using System;
using System.IO;
using AutoMapper;
using DeleteDuplicateFiles.DataModels;
using DeleteDuplicateFiles.Models;
using GeneralToolkitLib.Storage;
using GeneralToolkitLib.Storage.Models;
using JetBrains.Annotations;

namespace DeleteDuplicateFiles.Repositories
{
    [UsedImplicitly]
    public class SearchProfileRepository : RepositoryBase
    {
        private readonly StorageManager _storageManager;
        private readonly IMapper _mapper;
        private const string DefaultPassword = "eMy6Pj8RHkOrl3BNwrMSONsuu9B2mpQtlbWQ8Fz0b7fV3FFUTcH74ir0Era5UQuk";

        public SearchProfileRepository(IMapper mapper)
        {
            var settings = new StorageManagerSettings(true, Environment.ProcessorCount, true, DefaultPassword);
            _storageManager = new StorageManager(settings);
            _mapper = mapper;
        }

        public SearchProfileModel LoadSearchProfileFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            var dataObj = _storageManager.DeserializeObjectFromFile<SearchProfileDataModel>(filePath, null);
            return _mapper.Map<SearchProfileModel>(dataObj);
        }

        public void SaveSearchProfile(SearchProfileModel searchProfile, string filename)
        {
            SearchProfileDataModel searchProfileDataModel = _mapper.Map<SearchProfileDataModel>(searchProfile);
            _storageManager.SerializeObjectToFile(searchProfileDataModel, filename, null);
        }
    }
}
