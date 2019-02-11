using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DatabaseImport.Forms;
using DatabaseImport.Models;

namespace DatabaseImport
{
    public partial class frmMain : Form
    {
        private frmDataGrid dataGridForm;
        private List<DatabaseColumnCoupling> importSettings;

        public frmMain()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "Any File (*.*)|*.*";
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtInputFile.Text = openFileDialog1.FileName;
            }
        }

        private void chkWindowsAuthentication_CheckedChanged(object sender, EventArgs e)
        {
            txtUsername.Enabled = !chkWindowsAuthentication.Checked;
            txtPassword.Enabled = !chkWindowsAuthentication.Checked;
        }

        private void btnListDatabases_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = SQLHelper.GetConnectionString(txtServerName.Text, "", txtUsername.Text, txtPassword.Text, chkWindowsAuthentication.Checked);
                var databaseList = SQLHelper.ListDatabases(connectionString);
                cbDatabases.Items.Clear();
                cbDatabases.Enabled = true;
                foreach (var database in databaseList)
                {
                    cbDatabases.Items.Add(database);
                }

                if (cbDatabases.Items.Count > 0)
                    cbDatabases.SelectedIndex = 0;

                setEnableImportWhenValid();
            }
            catch (Exception ex)
            {
                cbDatabases.Enabled = false;
                MessageBox.Show(ex.Message);
            }
        }

        private void listDatabaseTables(string connectionString, string tableTable)
        {
            try
            {
                cbTables.Enabled = true;
                cbTables.Items.Clear();
                List<string> tableList = SQLHelper.ListDatabaseTables(connectionString, tableTable);

                cbTables.Items.Clear();
                if (tableList.Count > 0)
                {
                    foreach (string tableName in tableList)
                    {
                        cbTables.Items.Add(tableName);
                    }
                    cbTables.SelectedIndex = 0;
                }
                
            }
            catch (Exception ex)
            {
                cbTables.Enabled = false;
                MessageBox.Show(ex.Message);
            }
        }

        private void populateColumnStructure(string connectionString, string databaseName, string tableName)
        {
            List<SQLColumnStructure> columnStructures = SQLHelper.ListTableColumns(connectionString, databaseName, tableName);

            dgvTableStructure.DataSource = columnStructures;
            btnImportSettings.Enabled = true;

        }

        private void setEnableImportWhenValid()
        {
            btnImport.Enabled = txtInputFile.Text.Length > 0 && cbDatabases.Items.Count > 0 && importSettings != null;
        }

        private void cbDatabases_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDatabases.Items.Count > 0)
            {
                string connectionString = SQLHelper.GetConnectionString(txtServerName.Text, txtUsername.Text, txtPassword.Text, chkWindowsAuthentication.Checked);
                string databaseName = cbDatabases.SelectedItem.ToString();
                listDatabaseTables(connectionString, databaseName);
                if (cbTables.Items.Count > 0)
                {
                    populateColumnStructure(connectionString, databaseName, cbTables.SelectedItem.ToString());
                }
            }
        }

        private void cbTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTables.Items.Count > 0)
            {
                string connectionString = SQLHelper.GetConnectionString(txtServerName.Text, txtUsername.Text, txtPassword.Text, chkWindowsAuthentication.Checked);
                string databaseName = cbDatabases.SelectedItem.ToString();
                populateColumnStructure(connectionString, databaseName, cbTables.SelectedItem.ToString());
            }
        }

        private void btnOpenDatagridForm_Click(object sender, EventArgs e)
        {
            btnOpenDatagridForm.Enabled = false;
            btnReturnDatagrid.Enabled = true;

            dataGridForm = new frmDataGrid(DgvCallback, this.dgvTableStructure);
            dataGridForm.Show(this);
            PositionDataGridForm();
        }

        private void PositionDataGridForm()
        {
            Rectangle screenArea = Screen.GetWorkingArea(this);
            if (dataGridForm != null)
            {
                int screenCenter = screenArea.Width / 2;
                int masterFormCenter = this.Left + (this.Width / 2);

                if (masterFormCenter < screenCenter)
                    dataGridForm.Left = this.Left + this.Width + 5;
                else
                    dataGridForm.Left = this.Left - dataGridForm.Width - 5;

                dataGridForm.Top = this.Top;
            }
        }

        private void DgvCallback(DataGridView dgv)
        {
            this.panelColumnStructure.Controls.Add(dgv);
            btnOpenDatagridForm.Enabled = true;
            btnReturnDatagrid.Enabled = false;
            dataGridForm = null;
        }

        private void btnReturnDatagrid_Click(object sender, EventArgs e)
        {
            if (dataGridForm != null)
                dataGridForm.Close();

            btnOpenDatagridForm.Enabled = true;
            btnReturnDatagrid.Enabled = false;
        }

        private void btnImportSettings_Click(object sender, EventArgs e)
        {
            List<SQLColumnStructure> columnStructures = dgvTableStructure.DataSource as List<SQLColumnStructure>;
            frmImportSettings importSettingsForm = new frmImportSettings(SettingsCallback, columnStructures);

            importSettingsForm.ShowDialog(this);
            setEnableImportWhenValid();
        }

        private void SettingsCallback(List<DatabaseColumnCoupling> columnCouplings)
        {
            importSettings = columnCouplings;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}
