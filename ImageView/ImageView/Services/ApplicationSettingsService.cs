using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeneralToolkitLib.Configuration;
using GeneralToolkitLib.Storage.Registry;
using ImageView.DataContracts;
using ImageView.Interfaces;
using ImageView.Library.EventHandlers;
using ImageView.Properties;
using ImageView.Repositories;
using ImageView.Storage;
using ImageView.Utility;
using JetBrains.Annotations;
using Serilog;

namespace ImageView.Services
{
    [UsedImplicitly]
    public class ApplicationSettingsService : ServiceBase, IExceptionEventHandler
    {
        private readonly IRegistryAccess _registryService;

        public static string CompanyName => Application.CompanyName;

        public string ProductName { get; } = Application.ProductName;

        private readonly AppSettingsFileRepository _appSettingsFileRepository;

        private static int LoadThreads = 0;

        public ApplicationSettingsService(AppSettingsFileRepository appSettingsFileRepository)
        {
            _appSettingsFileRepository = appSettingsFileRepository;
            try
            {
                // In debug mode use the local storage reg access clone
                if (ApplicationBuildConfig.DebugMode)
                {
                    _registryService = new LocalStorageRegistryAccess(CompanyName, ProductName);
                }
                else
                {
                    _registryService = new RegistryAccess(CompanyName, ProductName)
                    {
                        ShowError = true
                    };
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(AccessViolationException))
                {
                    throw;
                }

                Log.Error(ex, "Fatal error encountered when accessing the registry settings");
                MessageBox.Show(ex.Message, Resources.Fatal_error_encountered_when_accessing_the_registry_settings_please_restart_, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Settings = new ImageViewApplicationSettings();
            _appSettingsFileRepository.LoadSettingsCompleted += _appSettingsFileRepository_LoadSettingsCompleted;
        }


        public ImageViewApplicationSettings Settings { get; private set; }

        public AppSettingsFileStoreDataModel FileStoredSettings { get; private set; }

        public event EventHandler OnSettingsLoaded;
        public event EventHandler OnSettingsSaved;
        public event EventHandler OnRegistryAccessDenied;
        public event ExceptionEventHandler OnUnexpectedException;

        public async Task<bool> LoadSettings()
        {
            LoadThreads++;
            if (LoadThreads > 1)
            {
                return false;
            }

            try
            {
                await Task.Factory.StartNew(_appSettingsFileRepository.LoadSettings);
                _registryService.SetupSubKeyPathAndAccessRights();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "ApplicationSettingsService->LoadSettings");
                OnRegistryAccessDenied?.Invoke(this, new EventArgs());
                OnUnexpectedException?.Invoke(this, new ExceptionEventArgs(ex) { SourceClass = GetType(), TargetClass = _registryService.GetType(), FunctionName = "LoadSettings" });
                LoadThreads--;
                return false;
            }
            Settings = _registryService.ReadObjectFromRegistry<ImageViewApplicationSettings>();
            OnSettingsLoaded?.Invoke(this, EventArgs.Empty);
            LoadThreads--;
            return true;
        }

        private void _appSettingsFileRepository_LoadSettingsCompleted(object sender, EventArgs e)
        {
            FileStoredSettings = _appSettingsFileRepository.AppSettings;
        }

        public async Task SaveSettings()
        {
            try
            {
                await _appSettingsFileRepository.SaveSettings();
                Settings.RemoveDuplicateEntriesWithIgnoreCase();
                _registryService.SaveObjectToRegistry(Settings);

                OnSettingsSaved?.Invoke(this, new EventArgs());
            }
            catch (Exception ex)
            {
                Log.Error(ex, "SaveSettings threw en exception on");
            }
            ;
        }

        public void RegisterFormStateOnClose(Form form)
        {
            string formName = form.GetType().Name;
            bool existingForm = FileStoredSettings.FormStateDictionary.Any(x => x.Key == formName);
            if (existingForm)
            {
                FileStoredSettings.FormStateDictionary[formName] = RestoreFormState.GetFormState(form);
            }
            else
            {
                var formState = RestoreFormState.GetFormState(form);
                FileStoredSettings.FormStateDictionary.Add(formName, formState);
            }

            Task.Factory.StartNew(_appSettingsFileRepository.SaveSettings);
        }
    }
}