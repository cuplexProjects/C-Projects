using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GeneralToolkitLib.ConfigHelper;

namespace GeneralToolkitLib.Log
{
    public static class LogWriter
    {
        private static readonly Object ThreadLock = new object();
        private static readonly string BasePath;
        private const string ErrorLogPrefix = "error_";
        private const string StandardLogPrefix = "info_";
        private static LogLevel _logLevel;

        static LogWriter()
        {
            BasePath = GlobalSettings.GetUserDataDirectoryPath();
            _logLevel = 0;
        }

        public static void SetMinimumLogLevel(LogLevel logLevel)
        {
            _logLevel = logLevel;
        }

        public static void LogMessage(string message, LogLevel severity)
        {
            WriteLog(message, false, severity);
        }

        public static void LogError(string message, Exception exception)
        {
            WriteLog(message + " - " + BuildExceptionMessage(exception), true, LogLevel.Error);
        }

        private static void WriteLog(string message, bool isErrorLog, LogLevel logLevel)
        {
            if (logLevel < _logLevel)
                return;

            string logFileName = BasePath + (isErrorLog ? ErrorLogPrefix : StandardLogPrefix) + DateTime.Today.ToString("yyyy_MM_dd") + ".log";
            try
            {
                //If exists then append else create new file
                lock (ThreadLock)
                {
                    FileStream fs;
                    if (File.Exists(logFileName))
                    {
                        fs = File.OpenWrite(logFileName);
                        fs.Seek(fs.Length, SeekOrigin.Begin);
                    }
                    else
                        fs = File.Create(logFileName);

                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + message);
                    sw.Flush();
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static string BuildExceptionMessage(this Exception exception)
        {
            var messages = exception.FromHierarchy(ex => ex.InnerException).Select(ex => ex.Message);
            string stackTrace = string.IsNullOrEmpty(exception.StackTrace) ? "" : "\nStack Trace:\n" + exception.StackTrace;
            return String.Join(Environment.NewLine, messages) + stackTrace;
        }

        // all error checking left out for brevity

        // a.k.a., linked list style enumerator
        private static IEnumerable<TSource> FromHierarchy<TSource>(
            this TSource source,
            Func<TSource, TSource> nextItem,
            Func<TSource, bool> canContinue)
        {
            for (var current = source; canContinue(current); current = nextItem(current))
            {
                yield return current;
            }
        }

        private static IEnumerable<TSource> FromHierarchy<TSource>(
            this TSource source,
            Func<TSource, TSource> nextItem)
            where TSource : class
        {
            return FromHierarchy(source, nextItem, s => s != null);
        }

        public enum LogLevel
        {
            Trace = 1,
            Debug = 2,
            Info = 3,
            Warning = 4,
            Error = 5
        }
    }
}