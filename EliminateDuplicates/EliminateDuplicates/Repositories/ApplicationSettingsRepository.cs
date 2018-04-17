using System;
using System.Windows.Forms;
using AutoMapper;
using DeleteDuplicateFiles.DataModels;
using DeleteDuplicateFiles.Models;
using GeneralToolkitLib.Storage.Registry;
using JetBrains.Annotations;
using Serilog;

namespace DeleteDuplicateFiles.Repositories
{
    [UsedImplicitly]
    public class ApplicationSettingsRepository : RepositoryBase
    {
        private readonly IRegistryAccess _registryService;
        private readonly IMapper _mapper;

        private static string CompanyName => Application.CompanyName;
        private string ProductName { get; } = Application.ProductName;

        public ApplicationSettingsRepository(IMapper mapper)
        {
            _mapper = mapper;
            _registryService = new RegistryAccess(CompanyName, ProductName);
        }


        public ApplicationSettingsModel LoadSettings()
        {
            ApplicationSettingsDataModel settingsData;
            try
            {
                _registryService.SetupSubKeyPathAndAccessRights();
                settingsData = _registryService.ReadObjectFromRegistry<ApplicationSettingsDataModel>();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "ApplicationSettingsService->LoadSettings");
                return null;
            }
            
            ApplicationSettingsModel applicationSettings = _mapper.Map<ApplicationSettingsModel>(settingsData);
            Log.Verbose("Successfully loaded application settings");
            
            return applicationSettings;
        }

        public void SaveSettings(ApplicationSettingsModel settings)
        {
            try
            {
                ApplicationSettingsDataModel applicationSettingsDataModel = _mapper.Map<ApplicationSettingsDataModel>(settings);
                _registryService.SaveObjectToRegistry(applicationSettingsDataModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex,"Failed to save ApplicationSettings");
            }
            
        }
    }
}
