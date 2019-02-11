using System;
using System.IO;
using PriceCalculator.Settings;

namespace PriceCalculator.Log
{
    public static class LogWriter
    {
        private static readonly Object ThreadLock = new object();
        private static string BasePath;
        private static string _logPrefix = "";

        static LogWriter()
        {
            _logPrefix = "error_";
        }

        public static string LogPrefix
        {
            get { return _logPrefix; }
            set { _logPrefix = value; }
        }

        public static void WriteLog(string message)
        {
            BasePath = SettingsService.ApplicationDataFolder;
            string logFileName = BasePath + LogPrefix + DateTime.Today.ToString("yyyy_MM_dd") + ".log";
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
