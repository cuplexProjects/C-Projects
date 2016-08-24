#region

using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using DeleteDuplicateFiles.Models;
using DeleteDuplicateFiles.Services;
using GeneralToolkitLib.ConfigHelper;
using GeneralToolkitLib.Converters;

#endregion

namespace DeleteDuplicateFiles
{
    public partial class frmSettings : Form
    {
        private ProgramSettings AppSettings { get; set; }

        public frmSettings()
        {
            InitializeComponent();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            AppSettingsManager.Instance.LoadSettings();
            AppSettings = AppSettingsManager.Instance.Settings;

            UpdateUIFromProgramSettings();
            SetApplicationLogFileInfo();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            UpdateProgramSettingsFromUI();
            AppSettingsManager.Instance.SaveSettings();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void UpdateProgramSettingsFromUI()
        {
            AppSettings.DeletionMode = radioRecycleBin.Checked
                ? ProgramSettings.DeletionModes.RecycleBin
                : ProgramSettings.DeletionModes.Permanent;
            AppSettings.HashAlgorithm = radioCRC32.Checked
                ? ProgramSettings.HashAlgorithms.CRC32
                : ProgramSettings.HashAlgorithms.MD5;
            AppSettings.MasterFileSelectionMethod = radioNewestDate.Checked
                ? ProgramSettings.MasterFileSelectionMethods.NewestModifiedDate
                : ProgramSettings.MasterFileSelectionMethods.OldestModifiedDate;
            AppSettings.MaximumNoOfHashingThreads = Convert.ToInt32(numericMaximumNoOfHashThreads.Value);
            AppSettings.IgnoreHiddenFilesAndDirectories = chkIgnoreHiddenFilesAndDirs.Checked;
            AppSettings.IgnoreSystemFilesAndDirectories = chkIgnoreSystemFiles.Checked;
        }

        private void SetApplicationLogFileInfo()
        {
            string path = GlobalSettings.GetUserDataDirectoryPath();

            try
            {
                if (!Directory.Exists(path))
                    return;

                string[] logFileItems = Directory.GetFiles(path, "*.log");
                int numberOfLogFiles = logFileItems.Length;
                long totalLogFileSize = 0;

                foreach (string fileItem in logFileItems)
                {
                    FileInfo fileInfo = new FileInfo(fileItem);
                    totalLogFileSize += fileInfo.Length;
                }

                lblNumberOfLogFiles.Text = numberOfLogFiles.ToString();
                lblLogFileDiskUsage.Text = GeneralConverters.FileSizeToStringFormater.ConvertFileSizeToString(totalLogFileSize, 2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"There was an exception when accessing the log files", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateUIFromProgramSettings()
        {
            radioRecycleBin.Checked = AppSettings.DeletionMode == ProgramSettings.DeletionModes.RecycleBin;
            radioCRC32.Checked = AppSettings.HashAlgorithm == ProgramSettings.HashAlgorithms.CRC32;
            radioNewestDate.Checked = AppSettings.MasterFileSelectionMethod ==
                                      ProgramSettings.MasterFileSelectionMethods.NewestModifiedDate;
            numericMaximumNoOfHashThreads.Value = AppSettings.MaximumNoOfHashingThreads;
            chkIgnoreHiddenFilesAndDirs.Checked = AppSettings.IgnoreHiddenFilesAndDirectories;
            chkIgnoreSystemFiles.Checked = AppSettings.IgnoreSystemFilesAndDirectories;
        }

        private void btnOptimizeDb_Click(object sender, EventArgs e)
        {
            btnOptimizeDb.Enabled = false;
            ComputedHashService.Instance.RemoveDeletedFilesFromDataBase();
        }

        public void EnableOptimizeDbButton()
        {
            btnOptimizeDb.Enabled = true;
        }

        private void btnOpenDataFolderInExplorer_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(GlobalSettings.GetUserDataDirectoryPath());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"There was an exception when trying to open the logData directory", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClearAllLogFiles_Click(object sender, EventArgs e)
        {
            string path = GlobalSettings.GetUserDataDirectoryPath();

            try
            {
                string[] logFileItems = Directory.GetFiles(path, "*.log");

                foreach (string fileItem in logFileItems)
                {
                    FileInfo fileInfo = new FileInfo(fileItem);
                    fileInfo.Delete();
                }

                lblNumberOfLogFiles.Text = "0";
                lblLogFileDiskUsage.Text = GeneralConverters.FileSizeToStringFormater.ConvertFileSizeToString(0, 2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"There was an exception when trying to remove all existing log files", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}