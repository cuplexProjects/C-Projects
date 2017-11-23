using System;
using System.Windows.Forms;
using GeneralToolkitLib.Log;
using GeneralToolkitLib.Storage.Registry;
using ImageView.DataContracts;
using ImageView.Properties;

namespace ImageView.Services
{
    public class ApplicationSettingsService
    {
        private readonly RegistryAccess _registryService;

        public ApplicationSettingsService()
        {
            try
            {
                _registryService = new RegistryAccess(Application.CompanyName, Application.ProductName)
                {
                    ShowError = true
                };
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(AccessViolationException))
                {
                    throw;
                }

                LogWriter.LogError("Fatal error encountered when accessing the registry settings", ex);
                MessageBox.Show(ex.Message,
                    Resources.Fatal_error_encountered_when_accessing_the_registry_settings_please_restart_,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Settings = new ImageViewApplicationSettings();
        }

       
        public ImageViewApplicationSettings Settings { get; private set; }

        public event EventHandler OnSettingsChanged;
        public event EventHandler OnRegistryAccessDenied;

        public bool LoadSettings()
        {
            try
            {
                _registryService.SetupSubKeyPathAndAccessRights();
            }
            catch (Exception ex)
            {
                LogWriter.LogError("Exception in ApplicationSettingsService()", ex);
                OnRegistryAccessDenied?.Invoke(this, new EventArgs());
                return false;
            }
            Settings = _registryService.ReadObjectFromRegistry<ImageViewApplicationSettings>();
            return true;
        }

        public void SaveSettings()
        {
            Settings.RemoveDuplicateEntriesWithIgnoreCase();
            _registryService.SaveObjectToRegistry(Settings);

            OnSettingsChanged?.Invoke(this, new EventArgs());
        }
    }
}