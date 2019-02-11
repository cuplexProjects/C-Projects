using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileStructureMapper.Models;

namespace FileStructureOrganizer
{
    public partial class FormMain : Form
    {
        private ProfileModel _currentProfileModel;

        public ProfileModel CurrentProfileModel
        {
            get => _currentProfileModel;
            set
            {
                if (value != null)
                {
                    _currentProfileModel = value;
                    UpdateGuiForProfile();
                }
                
            }
        }

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {

        }

        #region MainMenu Methods

        private void openSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void newSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var formProfile = new FormProfile();
            formProfile.CreateNewProfile();
            formProfile.ShowDialog(this);

        }

        private void UpdateGuiForProfile()
        {

        }

        #endregion

    }
}
