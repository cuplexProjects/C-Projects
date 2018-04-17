using System;
using System.Security.Policy;
using DeleteDuplicateFiles.Managers;
using DeleteDuplicateFiles.Models;
using DeleteDuplicateFiles.Repositories;
using JetBrains.Annotations;
using Serilog;

namespace DeleteDuplicateFiles.Services
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeleteDuplicateFiles.Services.ServiceBase" />

    [UsedImplicitly]
    public class AppSettingsService : ServiceBase
    {
        /// <summary>
        /// The repository
        /// </summary>
        private readonly ApplicationSettingsRepository _repository;

        private readonly AppSettingsManager _appSettingsManager;
        

        /// <summary>
        /// Initializes a new instance of the <see cref="AppSettingsService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="appSettingsManager">The application settings manager.</param>
        public AppSettingsService(ApplicationSettingsRepository repository, AppSettingsManager appSettingsManager)
        {
            _repository = repository;
            _appSettingsManager = appSettingsManager;
        }

        public event EventHandler OnSettingsLoaded;
        public event EventHandler OnSettingsSaved;


        private ApplicationSettingsModel _originalSettingsModel;

        /// <summary>
        /// The application settings
        /// </summary>
        public ApplicationSettingsModel ApplicationSettings
        {
            get { return _appSettingsManager.ApplicationSettings; }
            set
            {
                if (_appSettingsManager.ValidateSettings(value))
                {
                    _appSettingsManager.ApplicationSettings = value;
                }
            }
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public void LoadSettings()
        {
            var applicationSettings = _repository.LoadSettings();
            _originalSettingsModel = applicationSettings;

            if (applicationSettings == null)
            {
                ApplicationSettings = ApplicationSettingsModel.GetDefaultSettings();
                return;
            }

            if (!_appSettingsManager.ValidateSettings(applicationSettings))
            {
                Log.Error("Loaded application settings where corrupt, replacing with default");
                ApplicationSettings = ApplicationSettingsModel.GetDefaultSettings();
                return;
            }

            ApplicationSettings = applicationSettings;
            OnSettingsLoaded?.Invoke(this,EventArgs.Empty);
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void SaveSettings()
        {
            var settings = ApplicationSettings;

            if (settings.Equals(_originalSettingsModel))
            {
                Log.Warning("Settings saved despite being identical to the previous settings");
            }

            _repository.SaveSettings(settings);
            OnSettingsSaved?.Invoke(this, EventArgs.Empty);
        }
    }
}
