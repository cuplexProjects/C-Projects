using System.Collections.Generic;
using System.Configuration;

namespace SynchronizeTime.Core
{
    public class ServiceConfiguration
    {
        private readonly int _syncInterval;
        private readonly List<string> _timeServerList;
        private readonly string _serviceName;

        private ServiceConfiguration()
        {
            _timeServerList = new List<string>();
            string timeServers = ConfigurationManager.AppSettings.Get("TimeServers");
            string strSyncInterval = ConfigurationManager.AppSettings.Get("SyncInterval");
            _serviceName = ConfigurationManager.AppSettings.Get("ServiceName");

            string[] timeServerArr = timeServers.Split(';');
            foreach (string timeServer in timeServerArr)
            {
                _timeServerList.Add(timeServer);
            }
            _syncInterval = int.Parse(strSyncInterval);
        }

        public static ServiceConfiguration LoadConfiguration()
        {
            return new ServiceConfiguration();
        }

        public string ServiceName
        {
            get { return this._serviceName; }
        }

        public int SyncInterval
        {
            get { return this._syncInterval; }
        }

        public List<string> TimeServerList
        {
            get { return _timeServerList; }
        }
    }
}
