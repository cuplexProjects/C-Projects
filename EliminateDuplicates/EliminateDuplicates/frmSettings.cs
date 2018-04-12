#region

using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using DeleteDuplicateFiles.Managers;
using DeleteDuplicateFiles.Models;
using DeleteDuplicateFiles.Services;
using GeneralToolkitLib.ConfigHelper;
using GeneralToolkitLib.Converters;

#endregion

namespace DeleteDuplicateFiles
{
    public partial class FrmSettings : Form
    {
        private readonly AppSettingsManager _appSettingsManager;
        private readonly ComputedHashService _computedHashService;

        public FrmSettings(AppSettingsManager appSettingsManager, ComputedHashService computedHashService)
        {
            _appSettingsManager = appSettingsManager;
            _computedHashService = computedHashService;
            InitializeComponent();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            _appSettingsManager.LoadSettings();

            UpdateUiFromProgramSettings();
            SetApplicationLogFileInfo();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            UpdateProgramSettingsFromUi();
            _appSettingsManager.SaveSettings();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void UpdateProgramSettingsFromUi()
        {
            var settings = _appSettingsManager.Settings;

            settings.HashAlgorithm = radioCRC32.Checked
                ? ProgramSettings.HashAlgorithms.CRC32
                : ProgramSettings.HashAlgorithms.MD5;
            settings.MasterFileSelectionMethod = radioNewestDate.Checked
                ? ProgramSettings.MasterFileSelectionMethods.NewestModifiedDate
                : ProgramSettings.MasterFileSelectionMethods.OldestModifiedDate;
            settings.MaximumNoOfHashingThreads = Convert.ToInt32(numericMaximumNoOfHashThreads.Value);
            settings.IgnoreHiddenFilesAndDirectories = chkIgnoreHiddenFilesAndDirs.Checked;
            settings.IgnoreSystemFilesAndDirectories = chkIgnoreSystemFiles.Checked;
            settings.DeletionMode = radioRecycleBin.Checked
                ? ProgramSettings.DeletionModes.RecycleBin
                : ProgramSettings.DeletionModes.Permanent;
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

        private void UpdateUiFromProgramSettings()
        {
            var settings = _appSettingsManager.Settings;

            radioRecycleBin.Checked = settings.DeletionMode == ProgramSettings.DeletionModes.RecycleBin;
            radioCRC32.Checked = settings.HashAlgorithm == ProgramSettings.HashAlgorithms.CRC32;
            radioNewestDate.Checked = settings.MasterFileSelectionMethod == ProgramSettings.MasterFileSelectionMethods.NewestModifiedDate;
            numericMaximumNoOfHashThreads.Value = settings.MaximumNoOfHashingThreads;
            chkIgnoreHiddenFilesAndDirs.Checked = settings.IgnoreHiddenFilesAndDirectories;
            chkIgnoreSystemFiles.Checked = settings.IgnoreSystemFilesAndDirectories;
        }

        private void btnOptimizeDb_Click(object sender, EventArgs e)
        {
            btnOptimizeDb.Enabled = false;
            _computedHashService.RemoveDeletedFilesFromDataBase();
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