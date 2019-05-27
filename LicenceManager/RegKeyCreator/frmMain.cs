using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Windows.Forms;
using GeneralToolkitLib.Converters;
using GeneralToolkitLib.Encryption;
using GeneralToolkitLib.Encryption.Licence.DataModels;
using GeneralToolkitLib.Encryption.Licence.StaticData;
using LicenceManagerLib.License;
using RegKeyCreator.ApplicationKeys;

namespace RegKeyCreator
{
    public partial class frmMain : Form
    {
        private readonly SerialNumberGenerator serialNumberGenerator;
        private readonly RsaKeySetIdentity rsaPrivateKeyIdentity;
        private readonly string destinationFolder;
        private RegistrationDataModel registrationDataModel;
        private const int DEFAULT_VALID_YEARS = 1;

        public frmMain()
        {
            InitializeComponent();
            RsaAsymetricEncryption rsaAsymetricEncryption = new RsaAsymetricEncryption();
            rsaPrivateKeyIdentity = new RsaKeySetIdentity(RSAKeys.PrivateKeys.GetBase64Key(), RSAKeys.PublicKeys.GetBase64Key());
            RSAParameters rsaPrivateKeyParameters = rsaAsymetricEncryption.ParseRsaKeyInfo(rsaPrivateKeyIdentity);

            serialNumberGenerator = new SerialNumberGenerator(rsaPrivateKeyParameters, SerialNumbersSettings.ProtectedApp.SecureMemo);
            int nextMonth = DateTime.Today.AddMonths(1).Month;
            int year = DateTime.Today.AddMonths(1).AddYears(DEFAULT_VALID_YEARS).Year;

            dateTimePicker.Value = new DateTime(year, nextMonth, 1);
            destinationFolder = GetOutputPath(Assembly.GetExecutingAssembly().Location);
            this.CreateFolder(destinationFolder);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            List<string> versionList = LicenseGeneratorStaticData.SecureMemo.Versions.Select(x => x.ToString()).ToList();

            foreach (string version in versionList)
            {
                comboVersion.Items.Add(version);
            }

            if(comboVersion.Items.Count > 0)
                comboVersion.SelectedIndex = comboVersion.Items.Count - 1;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtSerial_Click(object sender, EventArgs e)
        {
            txtSerial.SelectAll();
            txtSerial.Select();
        }

        private void btnGenerateSalt_Click(object sender, EventArgs e)
        {
            ComputeAndSetSaltValue();
        }

        private void ComputeAndSetSaltValue()
        {
            txtSalt.Text = GeneralConverters.GetRandomHexValue(256);
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (txtSalt.Text.Length == 0)
            {
                MessageBox.Show(this, "Salt can not be empty, please generate a new value", "Missing salt", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (registrationDataModel == null)
                registrationDataModel = new RegistrationDataModel();

            registrationDataModel.Company = txtCompany.Text;
            registrationDataModel.FirstName = txtFirstName.Text;
            registrationDataModel.LastName = txtLastName.Text;
            registrationDataModel.Salt = txtSalt.Text;
            registrationDataModel.ValidTo = dateTimePicker.Value;
            registrationDataModel.VersionName = comboVersion.SelectedText;

            serialNumberGenerator.LicenseData.RegistrationData = registrationDataModel;
            serialNumberGenerator.GenerateLicenseData(rsaPrivateKeyIdentity);

            txtSerial.Text = serialNumberGenerator.LicenseData.RegistrationKey;
            ComputeAndSetSaltValue();

            string path = destinationFolder + "\\" + serialNumberGenerator.LicenseData.RegistrationKey;
            CreateFolder(path);
            serialNumberGenerator.CreateLicenseFile(path + "\\license.txt");
        }

        private void btnBrowseFolder_Click(object sender, EventArgs e)
        {
            Process.Start(destinationFolder);
        }

        private void CreateFolder(string path)
        {
            if(!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        private static string GetOutputPath(string fullAssemblyPath)
        {
            if (fullAssemblyPath != null)
            {
                int lastSlash = fullAssemblyPath.LastIndexOf('\\');
                if (lastSlash > 0)
                    return fullAssemblyPath.Substring(0, lastSlash + 1) + "\\LicenseData";
            }
            return null;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "License files|*.txt";
            openFileDialog1.FileName = "";

            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;

                try
                {
                    RegistrationDataModel registrationData = serialNumberGenerator.ReadRegistrationDataFromFile(fileName);
                    registrationDataModel = registrationData;

                    if (registrationDataModel != null)
                    {
                        txtCompany.Text = registrationDataModel.Company;
                        txtFirstName.Text = registrationDataModel.FirstName;
                        txtLastName.Text = registrationDataModel.LastName;
                        txtComputerId.Text = registrationData.ComputerId.ComputerId;
                        ComputeAndSetSaltValue();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
