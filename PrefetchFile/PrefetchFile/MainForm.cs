using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrefetchFile
{
    public partial class MainForm : Form
    {
        private const long MaxBufferSize = 33554432;
        private bool _cancelFileRead;
        private bool _fileSelected;
        private bool _readingFromFile;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            UpdateFileRangeLabel();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) != DialogResult.OK) return;
            txtFilename.Text = openFileDialog.FileName;
            _fileSelected = true;
            UpdateButtonState();
        }

        private void btnReadFile_Click(object sender, EventArgs e)
        {
            if (_readingFromFile)
            {
                _cancelFileRead = true;
                return;
            }

            _readingFromFile = true;
            IProgress<FileReadProgress> progress = new Progress<FileReadProgress>(Handler);
            long startPosition = 0;

            var fileInfo = new FileInfo(txtFilename.Text);
            long endPosition = fileInfo.Length;

            if (fileInfo.Length == 0)
            {
                _readingFromFile = false;
                MessageBox.Show(this, @"Cant read from zero length file", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (barSelector.SelectionStart > barSelector.MinValue)
            {
                double skip = (double)barSelector.SelectionStart/barSelector.MaxValue;
                startPosition = Convert.ToInt64(endPosition * skip);
            }

            if (barSelector.SelectionEnd < barSelector.MaxValue)
            {
                double skip = (double)(barSelector.MaxValue - barSelector.SelectionEnd)/barSelector.MaxValue;
                if (skip > 0)
                {
                    endPosition -= Convert.ToInt64(endPosition*skip);
                }
            }

            UpdateButtonState();
            progressBar.Value = 0;
            lblFileReadRange.Text = $"Reading file... Data to read: {FileSizeToStringFormater.ConvertFileSizeToString(endPosition-startPosition,2)}";
            Task.Run(() => { ReadFromFile(txtFilename.Text, startPosition, endPosition, progress); });
        }

        private void Handler(FileReadProgress fileReadProgress)
        {
            Invoke(new ProgressEventHandler(SetProgress), this, new ProgressEventArgs(fileReadProgress));
        }

        private void SetProgress(object sender, ProgressEventArgs e)
        {
            if (e.Completed)
            {
                progressBar.Value = 100;
                _cancelFileRead = false;
                _readingFromFile = false;
                UpdateButtonState();
            }
            else
            {
                progressBar.Value = e.PercentComplete;
            }
        }

        private void ReadFromFile(string filename, long startPosition, long endPosition, IProgress<FileReadProgress> progress)
        {
            long bufferSize = Math.Min(endPosition - startPosition, MaxBufferSize);
            long bytesRead = 0;
            long totalBytes = endPosition - startPosition;

            var buffer = new byte[bufferSize];
            FileStream fs = null;

            try
            {
                fs = File.OpenRead(filename);
                fs.Position = startPosition;

                while (bytesRead < totalBytes)
                {
                    int bytesToRead = Convert.ToInt32(Math.Min(totalBytes - bytesRead, MaxBufferSize));
                    bytesRead += fs.Read(buffer, 0, bytesToRead);

                    progress?.Report(new FileReadProgress {BytesRead = bytesRead, TotalBytes = totalBytes});

                    if (_cancelFileRead)
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                fs?.Close();
                progress?.Report(new FileReadProgress {BytesRead = bytesRead, TotalBytes = totalBytes, Completed = true});
            }
        }

        private void UpdateButtonState()
        {
            btnReadFile.Enabled = _fileSelected;
            btnBrowse.Enabled = !_readingFromFile;
            barSelector.Enabled = !_readingFromFile;

            btnReadFile.Text = _readingFromFile ? "Cancel" : "Read File";
        }

        private delegate void ProgressEventHandler(object sender, ProgressEventArgs e);

        private void barSelector_SelectionChanged(object sender, EventArgs e)
        {
            UpdateFileRangeLabel();
        }

        private void UpdateFileRangeLabel()
        {
            lblFileReadRange.Text = $"{barSelector.SelectionStart}% to {barSelector.SelectionEnd}%";
        }
    }

    public class ProgressEventArgs : FileReadProgress
    {
        public ProgressEventArgs()
        {
            
        }

        public ProgressEventArgs(FileReadProgress fileReadProgress)
        {
            Completed = fileReadProgress.Completed;
            BytesRead = fileReadProgress.BytesRead;
            TotalBytes = fileReadProgress.TotalBytes;
        }
    }
}