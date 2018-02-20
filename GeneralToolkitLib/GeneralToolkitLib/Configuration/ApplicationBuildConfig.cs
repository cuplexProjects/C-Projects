﻿using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace GeneralToolkitLib.Configuration
{
    public static class ApplicationBuildConfig
    {
        //private static readonly string LogFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}_log.txt";

        private static string _userDataPath;

        public static string ApplicationLogFilePath(bool rollingFile)
        {
            string logFilename = Assembly.GetCallingAssembly().GetName().Name;
            if (rollingFile)
            {
                logFilename += "_log-{Date}.txt";
            }
            else
            {
                logFilename += "_log.txt";
            }


            return Path.Combine(UserDataPath, logFilename);
        }

        public static string UserDataPath => _userDataPath ?? (_userDataPath = GetUserDataPath());
        public static bool DebugMode => IsDebug(Assembly.GetCallingAssembly());

        private static string GetUserDataPath()
        {
            if (DebugMode)
            {
                return GetAssemblyPath(Assembly.GetExecutingAssembly().Location);
            }

            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + Assembly.GetEntryAssembly().GetName().Name.Replace(" ", "") + "\\";
        }

        /// <summary>
        ///     Gets the assembly folder path from the complete filename
        /// </summary>
        /// <param name="fullAssemblyPath">The full assembly path.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private static string GetAssemblyPath(string fullAssemblyPath)
        {
            if (fullAssemblyPath != null)
            {
                int lastSlash = fullAssemblyPath.LastIndexOf('\\');
                if (lastSlash > 0)
                    return fullAssemblyPath.Substring(0, lastSlash + 1);
            }
            return null;
        }

        private static bool IsRelease(Assembly assembly)
        {
            var attributes = assembly.GetCustomAttributes(typeof(DebuggableAttribute), true);
            if (attributes.Length == 0)
                return true;

            var d = (DebuggableAttribute)attributes[0];
            if ((d.DebuggingFlags & DebuggableAttribute.DebuggingModes.Default) == DebuggableAttribute.DebuggingModes.None)
                return true;

            return false;
        }

        private static bool IsDebug(Assembly assembly)
        {
            /*
             * Temporary override
                return false;            
             */

            var attributes = assembly.GetCustomAttributes(typeof(DebuggableAttribute), true);
            if (attributes.Length == 0)
                return true;

            var d = (DebuggableAttribute)attributes[0];
            if (d.IsJITTrackingEnabled) return true;
            return false;
        }
    }
}