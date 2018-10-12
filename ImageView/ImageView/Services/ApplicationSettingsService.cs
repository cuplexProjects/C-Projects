using System;
using System.Linq;
using System.Threading;
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

        private static int SaveSettingsThreads = 0;
        private static int LoadSettingsThreads = 0;

        private ImageViewApplicationSettings _applicationSettings;

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


        public ImageViewApplicationSettings Settings
        {
            get
            {
                if (_applicationSettings == null)
                {
                    _applicationSettings = new ImageViewApplicationSettings();
                    throw new InvalidOperationException();
                }

                return _applicationSettings;
            }
            private set { _applicationSettings = value; }
        }

        public AppSettingsFileStoreDataModel FileStoredSettings { get; private set; }

        public event EventHandler OnSettingsLoaded;
        public event EventHandler OnSettingsSaved;
        public event EventHandler OnRegistryAccessDenied;
        public event ExceptionEventHandler OnUnexpectedException;

        public bool LoadSettings()
        {
            return Task.Factory.StartNew(() => LoadSettingsAsync().Result).Result;
        }

        public async Task<bool> LoadSettingsAsync()
        {
            Interlocked.Increment(location: ref LoadSettingsThreads);
            if (LoadSettingsThreads > 1)
            {
                await Task.Delay(millisecondsDelay: 10);
                Interlocked.Decrement(location: ref LoadSettingsThreads);
                return false;
            }

            bool loadedSuccessfuly = false;
            try
            {
                if (SaveSettingsThreads == 1)
                {
                    loadedSuccessfuly = await _appSettingsFileRepository.LoadSettingsAsync();

                }
                else
                {
                    _registryService.SetupSubKeyPathAndAccessRights();

                    Settings = _registryService.ReadObjectFromRegistry<ImageViewApplicationSettings>();
                    OnSettingsLoaded?.Invoke(sender: this, e: EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                Log.Error(exception: ex, messageTemplate: "ApplicationSettingsService->LoadSettingsAsync");

                OnRegistryAccessDenied?.Invoke(sender: this, e: new EventArgs());
                OnUnexpectedException?.Invoke(sender: this, e: new ExceptionEventArgs(exception: ex) { SourceClass = GetType(), TargetClass = _registryService.GetType(), FunctionName = "LoadSettingsAsync" });


            }
            finally
            {
                Interlocked.Decrement(location: ref LoadSettingsThreads);
            }


            return loadedSuccessfuly;
        }

        private void _appSettingsFileRepository_LoadSettingsCompleted(object sender, EventArgs e)
        {
            FileStoredSettings = _appSettingsFileRepository.AppSettings;
        }
        public void SaveSettings()
        {
            SaveSettingsAsync().ConfigureAwait(true);
        }

        public async Task SaveSettingsAsync()
        {
            Interlocked.Increment(ref SaveSettingsThreads);
            if (SaveSettingsThreads > 1)
            {
                await Task.Delay(10);
                Interlocked.Decrement(ref SaveSettingsThreads);
                return;
            }

            try
            {
                if (SaveSettingsThreads == 1)
                {
                    await _appSettingsFileRepository.SaveSettings();
                    Settings.RemoveDuplicateEntriesWithIgnoreCase();
                    _registryService.SaveObjectToRegistry(Settings);

                    OnSettingsSaved?.Invoke(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "SaveSettings threw en exception on");
            }
            finally
            {
                Interlocked.Decrement(ref SaveSettingsThreads);
            }
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