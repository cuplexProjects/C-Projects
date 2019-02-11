using System;
using System.Windows.Forms;
using CloseAllDemoApps.Models;

namespace CloseAllDemoApps
{
    public partial class FormSettings : Form
    {
        private ApplicationSettings applicationSettings;
        public FormSettings()
        {
            InitializeComponent();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {

        }

        private void btnRestoreDefault_Click(object sender, EventArgs e)
        {
            
            UpdateGui();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void UpdateGui()
        {
            chkAlwaysOntop.Checked = applicationSettings.AlwaysOntop;
            chkMinimizeToSystemTray.Checked = applicationSettings.MinimizeToSystemTray;
            txtProcessDescriptionFilter.Text = applicationSettings.ProcessDescriptionFilter;
        }

        private void txtCloseAllGlobalShortcut_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void txtCloseAllGlobalShortcut_KeyUp(object sender, KeyEventArgs e)
        {

        }
    }
}