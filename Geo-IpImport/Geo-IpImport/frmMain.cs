using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Windows.Forms;
using CuplexApiCommon;
using DatabaseLib;

namespace Geo_IpImport
{
    public partial class frmMain : Form
    {
        private readonly StringBuilder _progresStringBuilder;
        private bool _dbConnectionOk;
        private DatabaseImporter _dbImport;
        private frmLogView _logViewForm;
        private readonly NameValueConfigurationCollection _connectionStrings;

        public frmMain()
        {
            InitializeComponent();
            _progresStringBuilder = new StringBuilder();
            btnImport.Enabled = true;
            _connectionStrings = GetConnectionStrings();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            
            if (_connectionStrings.Count > 0)
            {
                cbConnectionString.DataSource = _connectionStrings.AllKeys;
                cbConnectionString.SelectedIndex = 0;
                string connectionString = _connectionStrings[cbConnectionString.Text].Value;
                DBHelper.SetConnectionString(connectionString);
            }
            
            if (!string.IsNullOrEmpty(Properties.Settings.Default.ImportLocation) && Directory.Exists(Properties.Settings.Default.ImportLocation))
            {
                txtCSVDirectory.Text = Properties.Settings.Default.ImportLocation;
            }
        }

        private void UpdateImportReady(bool dbConnOk)
        {
            btnImport.Enabled = dbConnOk;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (txtCSVDirectory.Text.Length > 0 && Directory.Exists(txtCSVDirectory.Text))
                folderBrowserDialog.SelectedPath = txtCSVDirectory.Text;
            else if (!string.IsNullOrEmpty(Properties.Settings.Default.ImportLocation) && Directory.Exists(Properties.Settings.Default.ImportLocation))
            {
                folderBrowserDialog.SelectedPath = Properties.Settings.Default.ImportLocation;
            }

            if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
            {
                txtCSVDirectory.Text = folderBrowserDialog.SelectedPath;
                Properties.Settings.Default.ImportLocation = txtCSVDirectory.Text;
                Properties.Settings.Default.Save();
            }
        }

        private async void btnImport_Click(object sender, EventArgs e)
        {
            if (_dbImport != null && _dbImport.IsRunning)
            {
                MessageBox.Show("Import is already running!");
                return;
            }
            if (txtCSVDirectory.Text.Length == 0 || !Directory.Exists(txtCSVDirectory.Text))
            {
                MessageBox.Show("Invalid directory");
                return;
            }
            Properties.Settings.Default.ImportLocation = txtCSVDirectory.Text;
            Properties.Settings.Default.Save();
            try
            {
                _dbImport = new DatabaseImporter(updateProgress, UpdateProgressText);
                progressBar1.Value = 0;
                btnCancel.Enabled = true;
                chkContinueOnError.Enabled = false;
                _dbImport.OnError += dbImport_OnError;
                _dbImport.OnComplete += dbImport_OnComplete;
                ClearProgressInfo();
                txtProgressInfo.Text = "";

                if (rbCountry.Checked)
                {
                    UpdateProgressText("Starting import of GeoIp Country data");
                    await _dbImport.ImportGeoIPCountryDataAsync(txtCSVDirectory.Text, chkContinueOnError.Checked);
                }
                else if (rbCity.Checked)
                {
                    UpdateProgressText("Starting import of GeoIp City data");
                    await _dbImport.ImportGeoIPCityDataAsync(txtCSVDirectory.Text, chkContinueOnError.Checked);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private NameValueConfigurationCollection GetConnectionStrings()
        {
            NameValueConfigurationCollection connectionStrings = new NameValueConfigurationCollection();

            for (int i = 0; i < ConfigurationManager.ConnectionStrings.Count; i++)
            {
                connectionStrings.Add(new NameValueConfigurationElement(ConfigurationManager.ConnectionStrings[i].Name, ConfigurationManager.ConnectionStrings[i].ConnectionString));
            }

            return connectionStrings;
        }

        private void dbImport_OnComplete(object sender, EventArgs e)
        {
            Invoke(new EventHandler(dbImport_OnCompleteNativeThread), sender, e);
        }

        private void dbImport_OnCompleteNativeThread(object sender, EventArgs e)
        {
            chkContinueOnError.Enabled = true;
            chkContinueOnError.Enabled = true;
            btnCancel.Enabled = false;
            _dbImport = null;
            GC.Collect();
            AppendProgressInfo("Import completed");
            MessageBox.Show("Import complete!", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dbImport_OnError(object sender, ErrorEventArgs e)
        {
            Invoke(new Delegates.LogEventHandler(dbImport_OnErrorNativeThread), sender, e);
        }

        private void dbImport_OnErrorNativeThread(object sender, ErrorEventArgs e)
        {
            if (_dbImport != null)
            {
                if (_logViewForm == null || _logViewForm.IsDisposed)
                {
                    _logViewForm = new frmLogView();
                    _logViewForm.Show(this);
                }

                _logViewForm.AppendLogText(e.GetException().Message);

                btnCancel.Enabled = false;
                chkContinueOnError.Enabled = true;
                _dbImport = null;
                GC.Collect();
                MessageBox.Show(e.GetException().Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to cancel?", "Cancel?", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                _dbImport.CancelImport();
                progressBar1.Value = 0;
                btnCancel.Enabled = false;
                chkContinueOnError.Enabled = true;
            }
        }

        public void updateProgressNativeThread(double percentProgress)
        {
            progressBar1.Value = Convert.ToInt32(Math.Min(Math.Abs(percentProgress), 1)*100);
        }

        public void updateProgress(double percentProgress)
        {
            Invoke(new Delegates.SetProgress(updateProgressNativeThread), percentProgress);
        }

        public void UpdateProgressText(string info)
        {
            AppendProgressInfo(info);
            Invoke(new EventHandler(updateInfoProgressNativeThread));
        }

        private void AppendProgressInfo(string info)
        {
            lock (_progresStringBuilder)
            {
                _progresStringBuilder.AppendLine(info);
            }
        }

        private void updateInfoProgressNativeThread(object sender, EventArgs e)
        {
            lock (_progresStringBuilder)
            {
                txtProgressInfo.Text = _progresStringBuilder.ToString();    
            }
        }
        private void ClearProgressInfo()
        {
            lock (_progresStringBuilder)
            {
                _progresStringBuilder.Clear();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_logViewForm != null && !_logViewForm.IsDisposed)
            {
                _logViewForm.Close();
            }

            if (_dbImport != null && _dbImport.IsRunning)
            {
                e.Cancel = true;
                MessageBox.Show("Please cancel the import before closing");
            }
        }

        private void btnTestDBConn_Click(object sender, EventArgs e)
        {
            if (DBHelper.CanConnectToDatabase())
            {
                _dbConnectionOk = true;
                lblStatus.Text = "Ok";
            }
            else
            {
                _dbConnectionOk = false;
                lblStatus.Text = "Could not connect to database";
            }
            UpdateImportReady(_dbConnectionOk);
        }

        private string GetConnectionStringInfo(string connectionString)
        {
            string connStrTemplate = "Srv='{0}' Usr='{1}' Db='{2}'";
            string dataSource = "";
            string user = "";
            string database = "";
            string[] connStrings = connectionString.Split(";=".ToCharArray());

            for (int i = 0; i < connStrings.Length; i++)
            {
                string nextItem = i < connStrings.Length - 1 ? connStrings[i + 1] : "";
                if (connStrings[i].Equals("data source", StringComparison.InvariantCultureIgnoreCase))
                {
                    dataSource = nextItem;
                }
                else if (connStrings[i].Equals("user id", StringComparison.InvariantCultureIgnoreCase))
                {
                    user = nextItem;
                }
                else if (connStrings[i].Equals("initial catalog", StringComparison.InvariantCultureIgnoreCase))
                {
                    database = nextItem;
                }
                else if (connStrings[i].Equals("integrated security", StringComparison.InvariantCultureIgnoreCase))
                {
                    user = "Integrated Security";
                }
            }

            return string.Format(connStrTemplate, dataSource, user, database);
        }

        private void cbConnectionString_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedKey = cbConnectionString.SelectedItem as string;
            if (selectedKey != null) 
            {
                string connectionString = _connectionStrings[selectedKey].Value;
                DBHelper.SetConnectionString(connectionString);
                StatusLabelConnStr.Text = GetConnectionStringInfo(connectionString);
            }
        }
    }
}