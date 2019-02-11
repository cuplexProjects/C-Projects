using System;
using System.Drawing;
using System.Windows.Forms;
using FileStructureMapper.Models;

namespace FileStructureOrganizer.UserControls
{
    public partial class ProfileUserControl : UserControl
    {
        private ProfileModel _profileModel;

        public ProfileUserControl()
        {
            InitializeComponent();
        }

        public ProfileUserControl(ProfileModel profileModel)
        {
            _profileModel = profileModel;
            InitializeComponent();
        }

        public ProfileModel SearchProfile
        {
            get => _profileModel;
            set
            {
                if (value != null)
                {
                    _profileModel = value;
                    UpdateGuiForProfile();
                }
            }
        }

        private void ProfileUserControl_Load(object sender, EventArgs e)
        {
            this.Size = pnlMain.Size;
            UpdateGuiForProfile();
        }

        private void UpdateGuiForProfile()
        {
            if (_profileModel != null)
            {
                txtBasePath.Text = _profileModel.BasePath;
                txtBasePath.ForeColor = Color.Black;

                txtProfileName.Text = _profileModel.ProfileName;
                txtBasePath.ForeColor = Color.Black;

                txtScanResults.Text = "";
                txtSeriesCount.Text = "";
            }
            else
            {
                Color foreColor = Color.Gray;
                txtBasePath.Text = "Base path: No path selected";
                txtBasePath.ForeColor = foreColor;

                txtProfileName.Text = "Profile name: No profile loaded";
                txtProfileName.ForeColor = foreColor;

                txtScanResults.Text = "Scan results: N/A";
                txtScanResults.ForeColor = foreColor;

                txtSeriesCount.Text = "Series found: N/A";
                txtSeriesCount.ForeColor = foreColor;
            }

        }
    }
}
