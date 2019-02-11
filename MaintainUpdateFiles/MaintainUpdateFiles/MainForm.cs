using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MaintainUpdateFiles
{
    public partial class MainForm : Form
    {
        private readonly Regex _installFileRegex;
        public MainForm()
        {
            _installFileRegex = new Regex(@"^([a-zA-Z]{1,64}).*(\d\.\d\.\d\.\d)(\.msi)$");
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            UpdatesTextFileLink.Visible = false;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
            {
                txtFolderPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (!ValidatePath())
            {
                MessageBox.Show("Invalid path selected or no msi files found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            FileStream fileStream = null;
            try
            {
                var files = Directory.GetFiles(txtFolderPath.Text).ToList();
                files.Sort();

                fileStream = File.Create(Path.Combine(txtFolderPath.Text, "updates.txt"));
                var sw = new StreamWriter(fileStream);
                sw.WriteLine(";aiu;");
                sw.WriteLine("");

                foreach (string filename in files)
                {
                    WriteInstallFileBlock(filename, sw);
                }

                sw.Flush();
                fileStream.Flush();
                
                UpdatesTextFileLink.Tag = Path.Combine(txtFolderPath.Text,"Updates.txt");
                UpdatesTextFileLink.Visible = true;
                UpdatesTextFileLink.Click += UpdatesTextFileLink_Click;

                MessageBox.Show("Successfully created a new Updates.txt file");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error creating updates.txt file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                fileStream?.Close();
            }
        }

        private void UpdatesTextFileLink_Click(object sender, EventArgs e)
        {
            string updatesFilePath = UpdatesTextFileLink.Tag as string;
            if (!string.IsNullOrEmpty(updatesFilePath))
            {
                System.Diagnostics.Process.Start(updatesFilePath);
            }
        }

        private void WriteInstallFileBlock(string filename, StreamWriter streamWriter)
        {
            const string basePath = "https://apps.cuplex.se/Downloads/";
            Regex fileNameComponentsRegex = new Regex(@"^(\w.*)(\d\.\d\.\d\.\d)(\.msi)$");

            string shortName = filename.Split("\\".ToCharArray()).Last();
            var matches = _installFileRegex.Match(shortName).Groups;

            if (matches.Count != 4)
            {
                return;
            }

            string fullMatch = matches[0].Value;
            string name = matches[1].Value;
            string version = matches[2].Value;
            string fileSufix = matches[3].Value;

            FileInfo fileInfo = new FileInfo(filename);

            streamWriter.WriteLine($"[{name}{version.Substring(0, 3)}]");
            streamWriter.WriteLine($"Name = {name}{version}");
            streamWriter.WriteLine($"Description = New features");
            streamWriter.WriteLine($"URL = {basePath}{name}/{fullMatch}");
            streamWriter.WriteLine($"Size = {fileInfo.Length}");
            streamWriter.WriteLine($"ServerFileName = {shortName}");
            streamWriter.WriteLine($"FilePath = [APPDIR]{name}.exe");
            streamWriter.WriteLine($"Version = {version}");
            streamWriter.WriteLine("");

            /*
               [ImageViewer2.0]
                Name = ImageViewer2.0.3.1
                Description = New features
                URL = https://apps.cuplex.se/Downloads/Imageviewer/ImageViewer-2.0.3.1.msi
                Size = 2376704  
                ServerFileName = ImageViewer-2.0.3.1.msi
                FilePath = [APPDIR]ImageViewer.exe
                Version = 2.0.3.1             
             */
        }

        private bool ValidatePath()
        {
            if (Directory.Exists(txtFolderPath.Text))
            {
                var files = Directory.GetFiles(txtFolderPath.Text);
                foreach (string fileName in files)
                {
                    string shortName = fileName.Split("\\".ToCharArray()).LastOrDefault();
                    if (IsInstallFile(shortName))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsInstallFile(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return false;
            }

            return _installFileRegex.IsMatch(filename);
        }

        
    }
}
