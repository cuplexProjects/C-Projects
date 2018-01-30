using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImageView.Models.Enums;
using JetBrains.Annotations;
using Serilog;

namespace ImageView.Services
{
    [UsedImplicitly]
    public class StartupService : ServiceBase
    {
        private readonly ApplicationSettingsService _applicationSettingsService;
        private readonly NotificationService _notificationService;
        private readonly UpdateService _updateService;
        private readonly Guid _mainFormId;

        public StartupService(ApplicationSettingsService applicationSettingsService, NotificationService notificationService, UpdateService updateService)
        {
            _applicationSettingsService = applicationSettingsService;
            _notificationService = notificationService;
            _updateService = updateService;
            _mainFormId = Guid.Parse("C1052D7F-1625-42D0-8DA6-01520211484C");
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
            var taskList = new List<Task>();
            var settings = _applicationSettingsService.Settings;

            if (settings.AutomaticUpdateCheck)
            {
                if (settings.LastUpdateCheck.AddDays(1) < DateTime.Now)
                {
                    bool isLatestVersion = await _updateService.IsLatestVersion();
                    settings.LastUpdateCheck = DateTime.Now;
                    _applicationSettingsService.SaveSettings();

                    if (!isLatestVersion)
                    {
                        taskList.Add(new Task(EnqueueUpdateRequest));
                    }
                }
            }

            taskList.Add(TestJob());
            foreach (var task in taskList)
            {
                task.Start();
            }

            await Task.WhenAll(taskList);
        }

        private void EnqueueUpdateRequest()
        {
            var notification = new Notification
            {
                NotificationType = NotificationType.UpdateApplication,
                TargetId = _mainFormId,
                BackgroundJob = UpdateProgramJob
            };

            _notificationService.AddNotification(notification);
        }

        private void UpdateProgramJob()
        {
            Task.Factory.StartNew(async () =>
            {
                await UpdateProgramJobAsync();
            });
        }

        private async Task UpdateProgramJobAsync()
        {
            await _updateService.DownloadAndRunLatestVersionInstaller();
        }

        private async Task<bool> TestJob()
        {
            await Task.Delay(5000);
            return true;
        }
    }
}
