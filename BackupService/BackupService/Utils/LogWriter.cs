using BackupService.Settings;
using System;
using System.IO;

namespace BackupService.Utils
{
    public class LogWriter
    {
        private static readonly Object ThreadLock = new object();
        private static readonly string basePath;
        private const string LogPrefix = "BackupService_";

        static LogWriter()
        {
            basePath = SettingsService.Instance.LogDirectoryPath;
            if (!basePath.EndsWith("\\"))
                basePath += "\\";
        }

        public static void WriteLog(string message)
        {
            string logFileName = basePath + LogPrefix + DateTime.Today.ToString("yyyy_MM_dd") + ".log";
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
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + message + "\n");
                    sw.Flush();
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
