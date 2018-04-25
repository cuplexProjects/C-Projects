using System;
using System.Threading;
using DeleteDuplicateFiles.Library.Events;
using DeleteDuplicateFiles.Managers.Interfaces;
using DeleteDuplicateFiles.Models;
using DeleteDuplicateFiles.Repositories;
using JetBrains.Annotations;
using Serilog;

namespace DeleteDuplicateFiles.Managers
{
    [UsedImplicitly]
    public class AppSettingsManager : ManagerBase, ISettingsManager, IDisposable
    {
        private readonly SettingsRepository _settingsRepository;
        private readonly Timer _timer;
        private ApplicationSettingsModel _applicationSettings;
        private ApplicationSettingsModel _applicationSettingsOrig;

        public AppSettingsManager(SettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
            ApplicationSettings = GetDefaultSettings();

            _timer = new Timer(CheckForModifiedSettings, this, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(1));
        }

        public void LoadSettings()
        {
            var settings = _settingsRepository.LoadSettings();
            if (!ValidateSettings(settings))
            {
                Log.Error("Loaded Settings failed validation! Settings: {settings}", settings);
                return;
            }

            ApplicationSettings = _settingsRepository.LoadSettings();
        }

        public void SaveSettings()
        {
            _settingsRepository.SaveSettings(ApplicationSettings);
        }



        public ApplicationSettingsModel ApplicationSettings
        {
            get

            {
                if (_applicationSettingsOrig == null)
                {
                    _applicationSettingsOrig = _applicationSettings;
                }

                return _applicationSettings;
            }
            private set => _applicationSettings = value;
        }

        public event SettingsChangedEventHandler OnSettingsChanged;

        private bool ValidateSettings(ApplicationSettingsModel applicationSettings)
        {
            return applicationSettings.MaximumNoOfHashingThreads != 0 && applicationSettings.MasterFileSelectionMethod != 0 && applicationSettings.DeletionMode != 0 && applicationSettings.HashAlgorithm != 0;
        }


        private static ApplicationSettingsModel GetDefaultSettings()
        {
            return new ApplicationSettingsModel
            {
                //Default value
                MaximumNoOfHashingThreads = Environment.ProcessorCount,
                IgnoreSystemFilesAndDirectories = true,
                IgnoreHiddenFilesAndDirectories = true,
                DeletionMode = ApplicationSettingsModel.DeletionModes.RecycleBin,
                HashAlgorithm = ApplicationSettingsModel.HashAlgorithms.MD5,
                MasterFileSelectionMethod = ApplicationSettingsModel.MasterFileSelectionMethods.OldestModifiedDate,
                LastProfileFilePath = null,
            };
        }

        private void CheckForModifiedSettings(object state)
        {
            if (!_applicationSettings.Equals(_applicationSettingsOrig))
            {
                OnSettingsChanged?.Invoke(this,new SettingsChangedEventArgs(_applicationSettingsOrig,_applicationSettings));
            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}