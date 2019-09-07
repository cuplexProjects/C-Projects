using System;
using System.Drawing;
using System.Windows.Forms;
using GeneralToolkitLib.Utility;
using SecureMemo.DataModels;
using SecureMemo.Services;
using Serilog;

namespace SecureMemo
{
    public partial class FormSettings : Form
    {
        private readonly SecureMemoFontSettings _fontSettings;
        private readonly AppSettingsService _appSettingsService;
        private Font _selectedFont;

        public FormSettings(AppSettingsService appSettingsService)
        {
            _fontSettings = appSettingsService.Settings.FontSettings;
            _appSettingsService = appSettingsService;
            InitializeComponent();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            _selectedFont = new Font(_fontSettings.FontFamily, 12f);
            LoadFontSettings(_selectedFont);
            LoadGeneralSettings();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            _fontSettings.FontFamily = _selectedFont.FontFamily;
            _fontSettings.FontFamilyName = _selectedFont.FontFamily.GetName(0);
            _fontSettings.FontSize = _selectedFont.Size;
            _fontSettings.Style = _selectedFont.Style;

            _appSettingsService.Settings.AlwaysOntop = chkAlwaysOntop.Checked;
            _appSettingsService.Settings.DefaultEmptyTabPages = Convert.ToInt32(numericUpDownTabPages.Value);

            if (chkSyncDatabase.Checked && !FileSystem.IsValidDirectory(txtSyncDatabaseDirectory.Text))
            {
                MessageBox.Show("The selected directory is invalid", "Invalid directory", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _appSettingsService.Settings.UseSharedSyncFolder = chkSyncDatabase.Checked;
            if (_appSettingsService.Settings.UseSharedSyncFolder)
                _appSettingsService.Settings.SyncFolderPath = txtSyncDatabaseDirectory.Text;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnShowFontDialog_Click(object sender, EventArgs e)
        {
            try
            {
                fontDialog1.Font = _selectedFont;
                if (fontDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    _selectedFont = fontDialog1.Font;
                    LoadFontSettings(_selectedFont);
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Show font dialog error.");
                MessageBox.Show(this, exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadGeneralSettings()
        {
            chkAlwaysOntop.Checked = _appSettingsService.Settings.AlwaysOntop;
            numericUpDownTabPages.Value = _appSettingsService.Settings.DefaultEmptyTabPages;
            chkSyncDatabase.Checked = _appSettingsService.Settings.UseSharedSyncFolder;
            txtSyncDatabaseDirectory.Text = _appSettingsService.Settings.SyncFolderPath;
        }

        private void LoadFontSettings(Font font)
        {
            var displayFont = new Font(font.FontFamily, 11f);
            txtFontFamily.Font = displayFont;
            txtFontSize.Font = displayFont;
            txtFontStyle.Font = displayFont;

            txtFontFamily.Text = font.FontFamily.GetName(0);
            txtFontSize.Text = Math.Round(font.SizeInPoints, 1).ToString();
            txtFontStyle.Text = font.Style.ToString();
        }

        private void chkSyncDatabase_CheckedChanged(object sender, EventArgs e)
        {
            UpdateBrowseEnableState();
        }

        private void btnBrowseFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowseForSyncDirectory.ShowDialog(this) == DialogResult.OK)
                txtSyncDatabaseDirectory.Text = folderBrowseForSyncDirectory.SelectedPath;
        }

        private void UpdateBrowseEnableState()
        {
            btnBrowseFolder.Enabled = chkSyncDatabase.Checked;
        }
    }
}