using System;
using System.Windows.Forms;
using DeleteDuplicateFiles.Models;
using GeneralToolkitLib.Storage.Registry;
using JetBrains.Annotations;

namespace DeleteDuplicateFiles.Managers
{
    [UsedImplicitly]
    public class AppSettingsManager : ManagerBase, IDisposable
    {
        private SettingsState _settingsState;


        public AppSettingsManager()
        {
            Settings = new ProgramSettings();
            Settings.PropertyChanged += Settings_PropertyChanged;
            _settingsState = SettingsState.None;
        }

        private void Settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            _settingsState = _settingsState | SettingsState.IsDirty;
        }

        public ProgramSettings Settings { get; private set; }

    

        public bool HasLoadedSettings => (_settingsState & SettingsState.HasLoadedSettings) == SettingsState.HasLoadedSettings;
        public bool HasSavedSettings => (_settingsState & SettingsState.HasSavedSettings) == SettingsState.HasSavedSettings;

        public void Dispose()
        {
           
        }

        public void LoadSettings()
        {
            var modifyRegistry = new RegistryAccess(Application.ProductName);

            if (Settings != null)
                Settings.PropertyChanged -= Settings_PropertyChanged;

            Settings = modifyRegistry.ReadObjectFromRegistry<ProgramSettings>() ?? new ProgramSettings();
            Settings.PropertyChanged += Settings_PropertyChanged;
            _settingsState = _settingsState | SettingsState.HasLoadedSettings;
            RemoveIsDirty();
        }

        public void SaveSettings()
        {
            var modifyRegistry = new RegistryAccess(Application.ProductName);
            modifyRegistry.SaveObjectToRegistry(Settings);
            _settingsState = _settingsState | SettingsState.HasSavedSettings;
            RemoveIsDirty();
        }

        private void RemoveIsDirty()
        {
            if ((_settingsState & SettingsState.IsDirty) == SettingsState.IsDirty)
            {
                _settingsState = _settingsState ^ SettingsState.IsDirty;
            }
        }

        public bool HasStateFlag(SettingsState settingsState)
        {
            return (_settingsState & settingsState) > 0;
        }
    }

    [Flags]
    public enum SettingsState
    {
        None = 0,
        HasLoadedSettings = 1,
        HasSavedSettings = 2,
        IsDirty = 4
    }
}