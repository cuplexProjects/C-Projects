using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using GeneralToolkitLib.Encryption.Licence;
using Serilog;


namespace SecureMemo
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();
        }

        private void frmAbout_Load(object sender, EventArgs e)
        {
            Text = "About " + AssemblyTitle;
            lblAppTitle.Text = AssemblyTitle;
            lblAppInfo1.Text = "Version: " + AssemblyVersion + " " + AssemblyCompany;
            lblAppInfo2.Text = AssemblyCopyright;

            licenceInfoControl1.NotRegisteredInfoText = "No valid licence was found";
            licenceInfoControl1.CreateRequest = CreateRequest;

            try
            {
                LicenceService.Instance.ValidateLicence();
                LicenceService.Instance.OnInitComplete += Instance_OnInitComplete;
                if (LicenceService.Instance.Initialized)
                    licenceInfoControl1.InitLicenceData(LicenceService.Instance.LicenceData);
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Exception in FormAbout.Load(): {Message}", exception.Message);
            }
        }

        private void Instance_OnInitComplete(object sender, EventArgs e)
        {
            licenceInfoControl1.InitLicenceData(LicenceService.Instance.LicenceData);
        }

        private void CreateRequest()
        {
            var formCreateLicenceRequest = new FormCreateLicenceRequest();
            formCreateLicenceRequest.ShowDialog(this);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void licenceInfoControl1_Load(object sender, EventArgs e)
        {
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    var titleAttribute = (AssemblyTitleAttribute) attributes[0];
                    if (titleAttribute.Title != "")
                        return titleAttribute.Title;
                }
                return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
        }

        public string AssemblyDescription
        {
            get
            {
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                    return "";
                return ((AssemblyDescriptionAttribute) attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                    return "";
                return ((AssemblyProductAttribute) attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                    return "";
                return ((AssemblyCopyrightAttribute) attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof (AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                    return "";
                return ((AssemblyCompanyAttribute) attributes[0]).Company;
            }
        }

        #endregion
    }
}