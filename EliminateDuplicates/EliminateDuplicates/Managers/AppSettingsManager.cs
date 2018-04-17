using System;
using DeleteDuplicateFiles.Library.Events;
using DeleteDuplicateFiles.Models;
using JetBrains.Annotations;

namespace DeleteDuplicateFiles.Managers
{
    [UsedImplicitly]
    public class AppSettingsManager : ManagerBase
    {
        private ApplicationSettingsModel _applicationSettings;
        

        public event SettingsChangedEventHandler OnSettingsChanged;

        public ApplicationSettingsModel ApplicationSettings
        {
            get
            {
                return _applicationSettings;
            }
            set
            {
                if (value == null)
                {
                    return;
                }

                if (!value.Equals(_applicationSettings))
                {
                    OnSettingsChanged?.Invoke(this, new SettingsChangedEventArgs(_applicationSettings, value));
                }

                _applicationSettings = value;
            }
        }

        public bool ValidateSettings(ApplicationSettingsModel applicationSettings)
        {
            return applicationSettings.MaximumNoOfHashingThreads != 0 && applicationSettings.MasterFileSelectionMethod != 0 && applicationSettings.DeletionMode != 0 && applicationSettings.HashAlgorithm != 0;
        }

        public ApplicationSettingsModel GetDefaultSettings()
        {
            return ApplicationSettingsModel.GetDefaultSettings();
        }


    }
}