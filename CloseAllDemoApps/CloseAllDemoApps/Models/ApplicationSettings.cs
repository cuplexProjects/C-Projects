using System;

namespace CloseAllDemoApps.Models
{
    [Serializable]
    public class ApplicationSettings
    {
        private bool _alwaysOntop;
        private bool _minimizeToSystemTray;
        private string _processDescriptionFilter;
        private ApplicationLogLevels _logLevel;

        public bool AlwaysOntop
        {
            get { return _alwaysOntop; }
            set { _alwaysOntop = value; }
        }

        public bool MinimizeToSystemTray
        {
            get { return _minimizeToSystemTray; }
            set { _minimizeToSystemTray = value; }
        }

        public string ProcessDescriptionFilter
        {
            get { return _processDescriptionFilter; }
            set { _processDescriptionFilter = value; }
        }

        public ApplicationLogLevels LogLevel
        {
            get { return _logLevel; }
            set { _logLevel = value; }
        }
    }
}