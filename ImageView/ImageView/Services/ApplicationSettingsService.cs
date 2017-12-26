using System;
using System.Windows.Forms;
using GeneralToolkitLib.Storage.Registry;
using ImageView.Configuration;
using ImageView.DataContracts;
using ImageView.Interfaces;
using ImageView.Library.EventHandlers;
using ImageView.Properties;
using ImageView.Storage;
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

        public ApplicationSettingsService()
        {
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
        }


        public ImageViewApplicationSettings Settings { get; private set; }
        public event EventHandler OnSettingsLoaded;
        public event EventHandler OnSettingsSaved;
        public event EventHandler OnRegistryAccessDenied;
        public event ExceptionEventHandler OnUnexpectedException;

        public bool LoadSettings()
        {
            try
            {
                _registryService.SetupSubKeyPathAndAccessRights();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "ApplicationSettingsService->LoadSettings");
                OnRegistryAccessDenied?.Invoke(this, new EventArgs());
                OnUnexpectedException?.Invoke(this, new ExceptionEventArgs(ex) { SourceClass = GetType(), TargetClass = _registryService.GetType(), FunctionName = "LoadSettings" });
                return false;
            }
            Settings = _registryService.ReadObjectFromRegistry<ImageViewApplicationSettings>();
            OnSettingsLoaded?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public void SaveSettings()
        {
            try
            {
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
    }
}