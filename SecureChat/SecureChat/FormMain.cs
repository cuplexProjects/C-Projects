using System;
using System.Windows.Forms;
using SecureChat.Settings;

namespace SecureChat
{
    public partial class frmMain : Form
    {
        private readonly ApplicationSettingsService _appSettingsService = ApplicationSettingsService.SettingsService;
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            _appSettingsService.LoadSettings();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            _appSettingsService.SaveSettings();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void showKeysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormViewKeys formViewKeys = new FormViewKeys();
            formViewKeys.ShowDialog(this);
        }


        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormApplicationSettings frmSettings = new FormApplicationSettings();
            frmSettings.ShowDialog(this);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBoxMain().ShowDialog(this);
        }

        private void userSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void createNewUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCreateNewUser frmcreateNewUser = new FormCreateNewUser();
            frmcreateNewUser.ShowDialog(this);
        }
    }
}
