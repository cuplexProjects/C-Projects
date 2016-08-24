using System;
using System.Threading;
using SynchronizeTime.Common;

namespace SynchronizeTime.Core
{
    public class TimeSyncServiceCore : IDisposable
    {
        private TimeServerHelper timeServerHelper;
        private int timeServerListIndex;
        private ServiceConfiguration serviceConfig;
        private const int CONNECTION_RETRY_INTERVAL = 10000;

        public bool IsRunning
        {
            get { return this._running; }

        }

        public bool Initialized
        {
            get { return this._initialized; }

        }
        
        private bool _running;
        private bool _initialized;
        private Thread workerThread;
        private readonly ManualResetEvent workerThreadResetEvent;

        public TimeSyncServiceCore()
        {
            timeServerHelper = new TimeServerHelper();
            
            this.workerThread = new Thread(this.TimeSyncLoop);
            this.workerThreadResetEvent = new ManualResetEvent(true);
        }

        public bool Initialize()
        {
            try
            {
                serviceConfig = ServiceConfiguration.LoadConfiguration();
            }
            catch(Exception ex)
            {
                LogWriter.WriteLog("Exception in TimeSync.Initialize() - " + ex.Message);
                return false;
            }

            _initialized = true;
            return true;
        }

        public void Start()
        {
            if (Initialized && !IsRunning)
            {
                this.workerThread.Start();
                this._running = true;
            }
        }

        public void Stop()
        {
            if(this.workerThreadResetEvent != null)
                this.workerThreadResetEvent.Set();

            this.workerThread = null;
            this._running = false;
            timeServerHelper = null;
            _initialized = false;
        }

        private void TimeSyncLoop()
        {
            while(this._running)
            {
                try
                {
                    if (serviceConfig.TimeServerList.Count > 0)
                    {
                        try
                        {
                            int timeServersToTry = serviceConfig.TimeServerList.Count;

                            while (this._running && timeServersToTry > 0)
                            {
                                timeServerHelper.TimeServerUrl = serviceConfig.TimeServerList[timeServerListIndex];
                                if (timeServerHelper.SynchronizeTime())
                                {
                                    LogWriter.WriteLog("Successfully synchronized time using: " + timeServerHelper.TimeServerUrl);
                                    timeServersToTry = 0;
                                }
                                else
                                {
                                    LogWriter.WriteLog("Failed to synchronize time using: " + timeServerHelper.TimeServerUrl);
                                    timeServersToTry--;
                                    this.workerThreadResetEvent.Reset();
                                    this.workerThreadResetEvent.WaitOne(CONNECTION_RETRY_INTERVAL);

                                    if (this._running && timeServersToTry > 0)
                                    {
                                        timeServerListIndex = (timeServerListIndex + 1) % serviceConfig.TimeServerList.Count;
                                        string nextTimeServer = serviceConfig.TimeServerList[timeServerListIndex];
                                        LogWriter.WriteLog("Switching to time server: " + nextTimeServer);    
                                    }
                                }
                            }
                            
                        }
                        catch(Exception ex)
                        {
                            LogWriter.WriteLog(ex.Message);
                        }
                    }

                    this.workerThreadResetEvent.Reset();
                    this.workerThreadResetEvent.WaitOne(Math.Max(serviceConfig.SyncInterval * 1000, 15000));
                }
                catch(Exception ex)
                {
                    LogWriter.WriteLog(ex.Message);
                }
            }
        }

        public void Dispose()
        {
            this.Stop();
        }
    }
}
