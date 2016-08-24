using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Data.SqlClient;


namespace Geo_IpImport
{
    /*
    public class DBImport
    {
        private Thread workerThread;
        private SqlConnection conn;
        private bool runImport;
        private SetProgress progressCallback;
        private List<ImportItem> importRowList;
        private string errorMessage;
        private bool _isRunning;
        private bool _ignoreInvalidRows;

        public bool IsRunning 
        {
            get { return _isRunning; }
        }

        public event LogEventHandler OnError;
        public event EventHandler OnComplete;

        public void CancelImport()
        {
            runImport = false;
        }

        public void DoImport(List<ImportItem> importRowList, ConnectionSettings dbConnSettings, SetProgress progressCallback, bool ignoreInvalidRows)
        {
            runImport = true;
            _ignoreInvalidRows = ignoreInvalidRows;
            conn = new SqlConnection(SQLHelper.GetConnectionString(dbConnSettings.Server, dbConnSettings.Database, dbConnSettings.Username, dbConnSettings.Password, dbConnSettings.UseWindowsAuth));
            conn.Open();

            this.progressCallback = progressCallback;
            this.importRowList = importRowList;
            this.errorMessage = null;

            workerThread = new Thread(_doImport);
            workerThread.Start();
        }

        public string GetErrorMessage()
        {
            return errorMessage;
        }

        private void _doImport()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("TRUNCATE TABLE [IpLookup]", conn);
                cmd.ExecuteNonQuery();
                int cnt = 0;
                _isRunning = true;

                foreach (var importRow in importRowList)
                {
                    cmd.CommandText = String.Format(@"INSERT INTO [IpLookup] ([IPFrom]
                  ,[IPTo]
                  ,[Registry]
                  ,[Assigned]
                  ,[Ctry]
                  ,[Cntry]
                  ,[Country])
                   VALUES({0},{1},'{2}',{3},'{4}','{5}','{6}')"
                        , importRow.IPfrom, importRow.IPto, importRow.Registry, importRow.Assigned, importRow.Ctry, importRow.Cntry, importRow.Country.Replace("'", "''"));

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch(Exception innerException)
                    {
                        if(!_ignoreInvalidRows)
                            throw;

                        if(this.OnError != null)
                            this.OnError.Invoke(this, new ErrorEventArgs(innerException));
                    }

                    if (progressCallback != null)
                        progressCallback(cnt++ / (double)importRowList.Count);

                    if (!runImport)
                        break;
                }

                conn.Close();

                if(OnComplete != null)
                    OnComplete.Invoke(this, new EventArgs());
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                if (OnError != null)
                    this.OnError.Invoke(this, new ErrorEventArgs(ex));
            }

            _isRunning = false;
        }
    }*/
}