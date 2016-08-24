using System;
using System.IO;
using System.Reflection;

namespace SynchronizeTime.Common
{
    public static class LogWriter
    {
        private static readonly Object ThreadLock = new object();
        private static readonly string BasePath;
        private static readonly string LogPrefix = "";

        static LogWriter()
        {
            LogPrefix = "TimeSync_";
            BasePath = GeneralConverters.GetDirectoryNameFromPath(Assembly.GetCallingAssembly().Location, true);
        }

        public static void WriteLog(string message)
        {
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
    }
}