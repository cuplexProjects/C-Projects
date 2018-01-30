using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImageView.Models.Enums;
using JetBrains.Annotations;
using Serilog;

namespace ImageView.Services
{
    [UsedImplicitly]
    public class NotificationService : ServiceBase, IDisposable
    {
        private readonly List<Notification> _notifications;
        private readonly List<INotificationTarget> _notificationTargets;
        private bool _isActive;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public NotificationService()
        {
            _notifications = new List<Notification>();
            _notificationTargets = new List<INotificationTarget>();
            _isActive = true;
            _cancellationTokenSource = new CancellationTokenSource();
            StartMessageLoop();
        }

        private void StartMessageLoop()
        {
            Task.Factory.StartNew(MessageLoop, _cancellationTokenSource.Token);
        }

        private void MessageLoop()
        {
            while (_isActive)
            {
                foreach (var notificationTarget in _notificationTargets)
                {
                    if (_notifications.Any(x => x.TargetId == notificationTarget.FormId))
                    {
                        var notification = _notifications.FirstOrDefault(x => x.TargetId == notificationTarget.FormId);
                        notificationTarget.NotificationsReceived(notification);
                        _notifications.Remove(notification);
                    }
                }
                Task.Delay(1000);
            }
        }

        public void RegisterAsNotificationReciever(INotificationTarget target)
        {
            _notificationTargets.Add(target);
        }

        public void AddNotification(Notification notification)
        {
            _notifications.Add(notification);
            var target = _notificationTargets.FirstOrDefault(x => x.FormId == notification.TargetId);
            target?.NotificationsReceived(notification);
        }

        public List<Notification> GetNotifications(Guid id)
        {
            var result = new List<Notification>();
            result.AddRange(_notifications.Where(x => x.TargetId == id));

            foreach (var notification in result)
            {
                _notifications.Remove(notification);
            }

            return result;
        }

        public void Dispose()
        {
            _isActive = false;
        }
    }

    public interface INotificationTarget
    {
        Guid FormId { get; }
        void NotificationsReceived(Notification notification);
    }

    public class Notification
    {
        public Guid TargetId { get; set; }
        public Action BackgroundJob { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}
