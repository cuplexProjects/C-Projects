using System.Linq;
using BackupService.Settings;
using BackupService.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using GeneralToolkitLib.Converters;

namespace BackupService
{
    public partial class FormSettings : Form
    {
        public BackupSettings Settings
        {
            get { return SettingsService.Instance.Settings; }
        }

        public delegate void UpdateFromNativeThread();

        public FormSettings()
        {
            InitializeComponent();
        }

        private void AppendSettingsFromGui()
        {
            Settings.StartDate = dtPickerStartDate.Value;
            Settings.BackupTime = GeneralConverters.GetSecondsFromDateTime(dtPickerStartTime.Value);

            if (radioButtonOne.Checked)
                Settings.BackupRecurrence = BackupFrequences.Once;
            else if (radioButtonDay.Checked)
                Settings.BackupRecurrence = BackupFrequences.Daily;
            else if (radioButtonWeek.Checked)
                Settings.BackupRecurrence = BackupFrequences.Weekly;
            else if (radioButtonMonth.Checked)
                Settings.BackupRecurrence = BackupFrequences.Monthly;            
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            dtPickerStartDate.Value = DateTime.Today;
            dtPickerStartTime.Value = DateTime.Today;
            
            txtDestinationFile.Text = saveFileDialogSetDestination.FileName;
            SettingsService.Instance.OnUpdateDataSourceAttribute += Instance_OnUpdateDataSourceAttribute;
        }

        void Instance_OnUpdateDataSourceAttribute(object sender, EventArgs e)
        {
            this.Invoke(new UpdateFromNativeThread(testUpdate));             
        }

        public void testUpdate()
        {
            this.SuspendLayout();
            SettingsService.Instance.HandleUpdateOnNativeThread();
            this.ResumeLayout();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            saveFileDialogSetDestination.Filter = "Backup service container (*.bsc)|*.bsc";
            if (saveFileDialogSetDestination.ShowDialog()== System.Windows.Forms.DialogResult.OK)
            {
                txtDestinationFile.Text = saveFileDialogSetDestination.FileName;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtDestinationFile.Text = "";
        }

        private void BackupFolderList_KeyUp(object sender, KeyEventArgs e)
        {
            Queue<BackupFolder> deleteQueue = new Queue<BackupFolder>();
            if (e.KeyData== Keys.Delete)
            {
                foreach (DataGridViewRow row in dataGridViewBackupFolders.SelectedRows)
                {
                    BackupFolder bc = row.DataBoundItem as BackupFolder;
                    if (bc != null)
                        deleteQueue.Enqueue(bc);                       
                }
                
                e.Handled = true;
                while (deleteQueue.Count > 0)
                    Settings.BackupFolders.Remove(deleteQueue.Dequeue());
            }
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {

        }

        private void btnLoadSettings_Click(object sender, EventArgs e)
        {

        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            if (Settings.UsingDefaultPassword)
            {
                if (MessageBox.Show(this,"Warning, password is not set! Would you like to save using default?","No password set" , MessageBoxButtons.OKCancel)== System.Windows.Forms.DialogResult.Cancel)
                    return;
            }
            SettingsService.Instance.SaveSettings();
        }

        private void btnSetPassword_Click(object sender, EventArgs e)
        {
            FormSetPassword setPasswordForm = new FormSetPassword();
            setPasswordForm.ShowDialog(this);
            if (setPasswordForm.VerifiedPassword != null)
            {
                Settings.SetPassword(setPasswordForm.VerifiedPassword);
            }
        }

        private void dataGridViewBackupFolders_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                String[] fileDropArray = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string path in fileDropArray)
                {
                    if (!Directory.Exists(path))
                    {
                        e.Effect = DragDropEffects.None;
                        return;
                    }
                }

                e.Effect = DragDropEffects.Copy;
            } 
        }

        private void dataGridViewBackupFolders_DragDrop(object sender, DragEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                String[] fileDropArray = (string[])e.Data.GetData(DataFormats.FileDrop);
                if(fileDropArray.Any(path => !Directory.Exists(path)))
                {
                    return;
                }

                for (int i = 0; i < fileDropArray.Length; i++)
                {
                    string path = fileDropArray[i];
                    if (!Settings.TryAddBackupFolder(path, i))
                        sb.AppendLine("Could not add folder " + path + " because its parent or sub folder is already added");
                }
            }
            if (sb.Length > 0)
                MessageBox.Show(sb.ToString());
        }

        private void dataGridViewBackupFolders_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {

        }

        private void FormSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            ReleaseAllResources();
        }

        private void ReleaseAllResources()
        {
            DirectoryComputeService.Instance.Dispose();
        }        
    }
}
