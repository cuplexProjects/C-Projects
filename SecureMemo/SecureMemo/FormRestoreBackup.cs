using System;
using System.Windows.Forms;
using SecureMemo.DataModels;
using SecureMemo.Storage;

namespace SecureMemo
{
    public partial class FormRestoreBackup : Form
    {
        private readonly MemoStorageService memoStorageService;

        public FormRestoreBackup()
        {
            InitializeComponent();
            memoStorageService = MemoStorageService.Instance;
        }

        private void frmRestoreBackup_Load(object sender, EventArgs e)
        {
            var datasource = memoStorageService.GetBackupFiles();
            if (datasource == null)
                return;

            foreach (BackupFileInfo backupFileInfo in datasource)
            {
                var listItem = new ListViewItem(backupFileInfo.Name) {Tag = backupFileInfo};
                listItem.SubItems.Add(new ListViewItem.ListViewSubItem(listItem, backupFileInfo.CreatedDate.ToString("yyyy-MM-dd  HH:mm:ss")));
                listViewBackupoFiles.Items.Add(listItem);
            }
        }

        private void listViewBackupoFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRestore.Enabled = listViewBackupoFiles.SelectedItems.Count > 0;
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (listViewBackupoFiles.SelectedItems.Count == 0)
                return;

            ListViewItem backupItem = listViewBackupoFiles.SelectedItems[0];
            var backupFileInfo = backupItem.Tag as BackupFileInfo;

            if (backupFileInfo == null)
                return;

            if (
                MessageBox.Show(this, "Are you sure you want to restore the following backup file? " + backupFileInfo.FullName, "Confirm Restore", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) ==
                DialogResult.OK)
            {
                try
                {
                    memoStorageService.RestoreBackup(backupFileInfo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Failed to backup database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MessageBox.Show(this, "Backup restored successfully", "Restore complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}