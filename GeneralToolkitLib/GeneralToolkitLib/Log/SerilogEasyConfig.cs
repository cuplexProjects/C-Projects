using System;
using System.CodeDom;
using Serilog.Events;

namespace GeneralToolkitLib.Log
{
    /// <summary>
    /// Basic log config and logimplementation for small projects
    /// </summary>
    public class SerilogEasyConfig
    {
        /// <summary>
        /// The autoconf settings
        /// </summary>
        private readonly LogSettings _autoconfSettings;
        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public LogSettings Settings { get => _autoconfSettings; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerilogEasyConfig"/> class.
        /// </summary>
        /// <param name="logFilepath">The log filepath.</param>
        /// <param name="minLevel">The minimum level.</param>
        public SerilogEasyConfig(string logFilepath, LogEventLevel minLevel)
        {
            _autoconfSettings = new LogSettings();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerilogEasyConfig"/> class.
        /// </summary>
        /// <param name="logSettings">The log settings.</param>
        public SerilogEasyConfig(LogSettings logSettings)
        {
            _autoconfSettings = logSettings;
        }

        /// <summary>
        /// 
        /// </summary>
        public enum LogSink
        {
            /// <summary>
            /// The console
            /// </summary>
            Console,
            /// <summary>
            /// The file
            /// </summary>
            File,
            /// <summary>
            /// The rolling file
            /// </summary>
            RollingFile
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class LogSettings
        {
            /// <summary>
            /// The unique identifier
            /// </summary>
            private readonly Guid _guid;

            /// <summary>
            /// Initializes a new instance of the <see cref="LogSettings"/> class.
            /// </summary>
            public LogSettings()
            {
                _guid = Guid.NewGuid();
            }
            /// <summary>
            /// Initializes a new instance of the <see cref="LogSettings"/> class.
            /// </summary>
            /// <param name="logFilePath">The log file path.</param>
            /// <param name="minLevel">The minimum level.</param>
            public LogSettings(string logFilePath, LogEventLevel minLevel)
            {
                LogFilePath = logFilePath;
                MinLevel = minLevel;
                _guid = Guid.NewGuid();
            }
            /// <summary>
            /// Gets the instance identifier.
            /// </summary>
            /// <value>
            /// The instance identifier.
            /// </value>
            public Guid InstanceId { get => _guid;}

            /// <summary>
            /// Gets the log file path.
            /// </summary>
            /// <value>
            /// The log file path.
            /// </value>
            public string LogFilePath { get; private set; }

            /// <summary>
            /// Gets the minimum level.
            /// </summary>
            /// <value>
            /// The minimum level.
            /// </value>
            public LogEventLevel MinLevel { get; private set; }

            /// <summary>
            /// Gets the maximum size of the file.
            /// For Rolling logfile it applies to the Current log
            /// </summary>
            /// <value>
            /// The maximum size of the file.
            /// </value>
            public long MaxFileSize { get; private set; }


            /// <summary>
            /// This only applies when using a rolling log file.
            /// Gets the maximum days to keep old files.
            /// </summary>
            /// <value>
            /// The maximum days to keep old files.
            /// </value>
            public int MaxDaysToKeepOldFiles { get; private set; }

        }
    }
    
}