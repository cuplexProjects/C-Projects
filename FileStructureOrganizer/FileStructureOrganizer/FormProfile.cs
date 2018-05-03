using System;
using System.Windows.Forms;
using FileStructureMapper.Models;

namespace FileStructureOrganizer
{
    public partial class FormProfile : Form
    {
        private ProfileModel _profileModel;

        public ProfileModel ProfileModel => _profileModel;

        public FormProfile()
        {
            InitializeComponent();
        }

        public void CreateNewProfile()
        {
            _profileModel= new ProfileModel();
            _profileModel.ProfileName= "New Profile";
            txtProfileName.Text = 
            UpdateGui();
        }

        private void SetProfile(ProfileModel profileModel)
        {
            _profileModel = profileModel ?? throw new ArgumentNullException(nameof(profileModel));
            UpdateGui();
        }

        private void FormProfile_Load(object sender, EventArgs e)
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

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.CommonVideos;
            if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
            {
                txtBasePath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private bool ValidateProfile()
        {
            if (string.IsNullOrWhiteSpace(ProfileModel.BasePath) || )
        }

        private void UpdateGui()
        {

        }
    }
}
