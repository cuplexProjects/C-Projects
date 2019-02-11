using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IpLocationUpdate
{
    public partial class frmMain : Form
    {
        private List<IpLocation> ipLocationList;
        private DatabaseHandler dbHandler;

        public frmMain()
        {
            InitializeComponent();
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            drpAuthentication.Items.Add("Windows Authentication");
            drpAuthentication.Items.Add("SQL Server Authentication");
            drpAuthentication.SelectedIndex = 0;

            if (drpSQLServer.Items.Count > 0)
                drpSQLServer.SelectedIndex = 0;
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Geo IP file|*.csv";
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
                txtGeoIpInputFile.Text = openFileDialog1.FileName;            
        }

        private void drpAuthentication_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpAuthentication.SelectedIndex == 0)
            {
                txtUserName.Enabled = false;
                txtPassword.Enabled = false;
            }
            else
            {
                txtUserName.Enabled = true;
                txtPassword.Enabled = true;
            }
        }

        private void btnParseFile_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtGeoIpInputFile.Text))
            {
                MessageBox.Show("No input file selected!");
                return;
            }

            txtGeoIpStatus.Text = "Reading file...";
            ipLocationList = IpLocation.ParseInputFile(txtGeoIpInputFile.Text);
            txtGeoIpStatus.Text = ipLocationList.Count + " items loaded";
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string SqlServer = drpSQLServer.Text;

            dbHandler = new DatabaseHandler();
            if (drpAuthentication.SelectedIndex == 0)
                txtSQLConnectionStatus.Text = dbHandler.OpenConnection(SqlServer);
            else
                txtSQLConnectionStatus.Text = dbHandler.OpenConnection(SqlServer, txtUserName.Text, txtPassword.Text);

            if (dbHandler.ConnectionIsOpen())
            {
                drpDatabase.Enabled = true;
                drpDBTable.Enabled = true;

                drpDatabase.Items.Clear();
                drpDatabase.DataSource = dbHandler.GetDatabaseList().ToArray();
                if (drpDatabase.Items.Count > 0)
                    drpDatabase.SelectedIndex = 0;
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (dbHandler != null)
            {
                dbHandler.CloseConnection();

                drpDatabase.Enabled = false;
                drpDBTable.Enabled = false;
                txtSQLConnectionStatus.Text = "Connection closed";
                dbHandler = null;
            }
        }

        private async void btnDiscover_Click(object sender, EventArgs e)
        {
            List<SQLServerInstance> serverList = await DatabaseHandler.GetLocalSQLServerInstancesAsync();

            drpSQLServer.Items.Clear();
            foreach (SQLServerInstance instance in serverList)
                drpSQLServer.Items.Add(instance.ToString());

            if (drpSQLServer.Items.Count > 0)
                drpSQLServer.SelectedIndex = 0;
        }

        private void drpDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dbHandler != null && dbHandler.ConnectionIsOpen())
            {
                try
                {
                    drpDBTable.Items.Clear();
                    string[] listItems = dbHandler.GetDatabaseTables(drpDatabase.Text).ToArray();
                    drpDBTable.DataSource = listItems;

                    if (drpDBTable.Items.Count > 0)
                        drpDBTable.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnTransfer_Click(object sender, EventArgs e)
        {
            if (dbHandler != null && dbHandler.ConnectionIsOpen() && ipLocationList != null && ipLocationList.Count > 0)
            {
                btnTransfer.Enabled = false;
                btnAbortTransfer.Enabled = true;

                string responceText = await performDbUpdateAsync();
                txtUpdateDBStatus.Text = responceText;
                pbarTransfer.Value = 100;
            }
        }

        async Task<string> performDbUpdateAsync()
        {
            Task<string> getStringTask = dbHandler.InsertIpLocationListAsync(ipLocationList, drpDatabase.Text, drpDBTable.Text, insertCallback);

            SetUpdateInitText();

            string responceText = await getStringTask;
            btnTransfer.Enabled = (await getStringTask) != null;
            btnAbortTransfer.Enabled = (await getStringTask) == null;

            return responceText;
        }

        void SetUpdateInitText()
        {
            txtUpdateDBStatus.Text = "Performing db update";
        }

        private void updateStatusTextWithNativeThread(double percentDone, int itemsInserted)
        {
            pbarTransfer.Value = Convert.ToInt32(Math.Round(percentDone));
            pbarTransfer.Update();

            txtUpdateDBStatus.Text = "Performing db update\r\n Items inserted: " + itemsInserted;
        }

        private void insertCallback(double percentDone, int itemsInserted)
        {
            if (percentDone >= 0 && percentDone <= 100)
            {
                this.Invoke(new DatabaseHandler.ProgressCallback(updateStatusTextWithNativeThread), percentDone, itemsInserted);
            }
        }

        private void btnAbortTransfer_Click(object sender, EventArgs e)
        {
            dbHandler.AbortDbInsertOperation();
        }
    }
}