using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Serilog;

namespace ImageView.Services
{
    [UsedImplicitly]
    public class StartupService : ServiceBase
    {
        private readonly ApplicationSettingsService _applicationSettingsService;
        private readonly UpdateService _updateService;

        public StartupService(ApplicationSettingsService applicationSettingsService, UpdateService updateService)
        {
            _applicationSettingsService = applicationSettingsService;
            _updateService = updateService;
        }

        public void ScheduleAndRunStartupJobs()
        {
            Task.Factory.StartNew(async () =>
            {
                await RunStartupJobsAsync();
            });
        }

        private async Task RunStartupJobsAsync()
        {
            var settings = _applicationSettingsService.Settings;            

            if (settings.LastUpdateCheck != null && (settings.AutomaticUpdateCheck && settings.LastUpdateCheck == null || settings.LastUpdateCheck.Value.AddDays(1) < DateTime.Now))
            {
                Log.Information("Starting automatic update and install job");
                await RunUpdateCheckAndInstall();
            }
        }

        private async Task RunUpdateCheckAndInstall()
        {
            bool isLatestVersion = await _updateService.IsLatestVersion();
            if (!isLatestVersion)
            {
                await _updateService.DownloadAndRunLatestVersionInstaller();
            }

            _applicationSettingsService.Settings.LastUpdateCheck = DateTime.Now;
            _applicationSettingsService.SaveSettings();
        }
    }
}
