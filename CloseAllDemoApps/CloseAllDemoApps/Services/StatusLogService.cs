using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CloseAllDemoApps.Configuration;
using CloseAllDemoApps.Models;

namespace CloseAllDemoApps.Services
{
    public class StatusLogService
    {
        private static StatusLogService _instance;
        private const int MaxLogsize = 32768;
        private const double LogTruncateFactor = 0.8d;
        private readonly ApplicationSettings _appSettings;
        private readonly SynchronizedCollection<LogRowItem> _logRowItems;
        private readonly object _threadLockObj = new object();

        public event EventHandler LogDataWritten;

        private StatusLogService()
        {
            _appSettings = AppSettingsService.SettingsService.Settings;
            _logRowItems = new SynchronizedCollection<LogRowItem>();
        }

        public void LogException(Exception e)
        {
            WriteLogData(ApplicationLogLevels.Error, e.CreateExceptionMessage());
        }

        public void LogDebug(string message)
        {
            WriteLogData(ApplicationLogLevels.Debug, message);
        }

        public void LogInfo(string message)
        {
            WriteLogData(ApplicationLogLevels.Info, message);
        }

        public void LogError(string message)
        {
            WriteLogData(ApplicationLogLevels.Error, message);
        }

        public void ClearAllLogData()
        {
            _logRowItems.Clear();
        }

        public string GetLogdata()
        {
            lock (_threadLockObj)
            {
                StringBuilder sb = new StringBuilder();
                foreach (LogRowItem item in _logRowItems)
                {
                    sb.AppendLine(item.ToString());
                }
                return sb.ToString();
            }
        }

        public string GetLogdataReverseOrder()
        {
            lock (_threadLockObj)
            {
                StringBuilder sb = new StringBuilder();
                foreach (LogRowItem item in _logRowItems.Reverse())
                {
                    sb.AppendLine(item.ToString());
                }
                return sb.ToString();
            }
        }

        public string GetLastLogRow()
        {
            return _logRowItems.Count > 0 ? _logRowItems.Last().ToString() : null;
        }

        private void WriteLogData(ApplicationLogLevels logLevel, string message)
        {
            if (_appSettings.LogLevel > logLevel) return;
            _logRowItems.Add(LogRowItem.CreateRowItem(message));

            if (LogDataWritten != null)
                LogDataWritten.Invoke(this, new EventArgs());

            TruncateLog();
        }

        private void TruncateLog()
        {
            if (_logRowItems.Count <= MaxLogsize) return;
            int itemsToRemove = _logRowItems.Count - (int)(MaxLogsize * LogTruncateFactor);

            while (itemsToRemove > 0 && _logRowItems.Count > 0)
            {
                var lastItem = _logRowItems.Last();
                _logRowItems.Remove(lastItem);
                itemsToRemove--;
            }
        }

        public static StatusLogService Service
        {
            get { return _instance ?? (_instance = new StatusLogService()); }
        }

        internal class LogRowItem
        {
            private DateTime Timestamp { get; set; }
            private string Logdata { get; set; }

            public static LogRowItem CreateRowItem(string logText)
            {
                return new LogRowItem { Logdata = logText, Timestamp = DateTime.Now };
            }

            public override string ToString()
            {
                return Timestamp.ToString("yyyy-MM-dd HH:mm:ss - ") + Logdata;
            }
        }
    }
}
