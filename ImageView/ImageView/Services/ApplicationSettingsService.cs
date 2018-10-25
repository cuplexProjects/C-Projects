using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeneralToolkitLib.Configuration;
using GeneralToolkitLib.Storage.Registry;
using ImageViewer.DataContracts;
using ImageViewer.Interfaces;
using ImageViewer.Library.EventHandlers;
using ImageViewer.Models;
using ImageViewer.Properties;
using ImageViewer.Repositories;
using ImageViewer.Storage;
using ImageViewer.Utility;
using JetBrains.Annotations;
using Serilog;

namespace ImageViewer.Services
{
    [UsedImplicitly]
    public class ApplicationSettingsService : ServiceBase, IExceptionEventHandler
    {
        private readonly IRegistryAccess _registryRepository;

        public string CompanyName { get; } = Application.CompanyName;

        public string ProductName { get; } = Application.ProductName;

        private readonly AppSettingsFileRepository _appSettingsFileRepository;
        private ImageViewApplicationSettings _applicationSettings;
        private RegistryAppSettings _registryAppSettings;


        public ApplicationSettingsService(AppSettingsFileRepository appSettingsFileRepository, IRegistryAccess registryRepository)
        {
            _registryRepository = registryRepository;
            _appSettingsFileRepository = appSettingsFileRepository;
            try
            {
                bool result = _appSettingsFileRepository.LoadSettings();
                if (!result)
                {
                    
                    _appSettingsFileRepository= new AppSettingsFileRepository();
                }
                result =result& _registryRepository.TryReadObjectFromRegistry(out _registryAppSettings);
                if (!result || _registryAppSettings == null)
                {
                    _registryAppSettings = RegistryAppSettings.CreateNew(ProductName, CompanyName);
                    _registryRepository.SaveObjectToRegistry(_registryAppSettings);
                }

            }
            catch (Exception ex)
            {

                Log.Error(ex, "Fatal error encountered when accessing the registry settings");
                _registryAppSettings = RegistryAppSettings.CreateNew(ProductName, CompanyName);
                //MessageBox.Show(ex.Message, Resources.Fatal_error_encountered_when_accessing_the_registry_settings_please_restart_, MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            _appSettingsFileRepository.LoadSettingsCompleted += _appSettingsFileRepository_LoadSettingsCompleted;
        }


        // Unit tests
        public static ApplicationSettingsService CreateService(AppSettingsFileRepository appSettingsFileRepository, LocalStorageRegistryAccess localStorageRegistry)
        {
            return new ApplicationSettingsService(appSettingsFileRepository, localStorageRegistry);
        }


        public ImageViewApplicationSettings Settings
        {
            get
            {
                if (_applicationSettings == null)
                {
                    throw new InvalidOperationException();
                }

                return _applicationSettings;
            }
            private set { _applicationSettings = value; }
        }

        public RegistryAppSettings RegistryAppSettings
        {
            get
            {
                if (_registryAppSettings == null)
                {
                    throw new InvalidOperationException();
                }
                return _registryAppSettings;
            }
        }

        public event EventHandler OnSettingsLoaded;
        public event EventHandler OnSettingsSaved;
        public event EventHandler OnRegistryAccessDenied;
        public event ExceptionEventHandler OnUnexpectedException;

        public bool LoadSettings()
        {
            bool loadedSuccessively = false;

            try
            {
                OnSettingsLoaded?.Invoke(this, EventArgs.Empty);
                loadedSuccessively = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "ErrorLoading Appsettings");
            }

            return loadedSuccessively;
        }


        private void _appSettingsFileRepository_LoadSettingsCompleted(object sender, EventArgs e)
        {
            //FileStoredSettings = _appSettingsFileRepository.AppSettings;
        }
        public bool SaveSettings()
        {
            bool result = true;

            if (_registryRepository is LocalStorageRegistryAccess registryAccessStorage)
            {
                result = registryAccessStorage.SecureSaveDatabaseToFile();
            }

            try
            {
                result = result & _appSettingsFileRepository.SaveSettings();
                Settings.RemoveDuplicateEntriesWithIgnoreCase();
                _registryRepository.SaveObjectToRegistry(Settings);

                OnSettingsSaved?.Invoke(this, new EventArgs());

            }
            catch (Exception ex)
            {
                Log.Error(ex, "SaveSettings threw en exception on");
                return false;
            }

            return result;
        }


        public void RegisterFormStateOnClose(Form form)
        {
            string formName = form.GetType().Name;
            var fileDbAppSettings = _appSettingsFileRepository.AppSettings;
            bool existingForm = fileDbAppSettings.ExtendedAppSettings.FormStateDictionary.Any(x => x.Key == formName);
            if (existingForm)
            {
                fileDbAppSettings.ExtendedAppSettings.FormStateDictionary[formName] = RestoreFormState.GetFormState(form);
            }
            else
            {
                var formState = RestoreFormState.GetFormState(form);
                fileDbAppSettings.ExtendedAppSettings.FormStateDictionary.Add(formName, formState);
            }

            _appSettingsFileRepository.NotifySettingsChanged();
            _appSettingsFileRepository.SaveSettings();
        }
    }
}