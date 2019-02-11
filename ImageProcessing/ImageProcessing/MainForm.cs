using System;
using System.Drawing;
using System.Windows.Forms;
using ImageProcessing.DataModels;
using ImageProcessing.ImageFilterImplementations;
using ImageProcessing.Services;

namespace ImageProcessing
{
    public partial class MainForm : Form
    {
        private readonly ImageProcessService _imageProcessService;
        private readonly SelectedFilterValues _selectedFilterValues;
        private IImageFilter _selectedImageFilter;
        private string selectedImagePath = "";

        public MainForm()
        {
            InitializeComponent();
            _imageProcessService = ImageProcessService.Instance;
            _imageProcessService.OnProgressUpdate += imageProcessService_OnProgressUpdate;
            _imageProcessService.OnProcessComplete += imageProcessService_OnProcessComplete;
            _selectedFilterValues = new SelectedFilterValues();
            _selectedImageFilter = new GammaFilter(_selectedFilterValues.Gamma);
        }

        private void imageProcessService_OnProcessComplete(object sender, EventArgs e)
        {
            Invoke(new EventHandler(imageProcessService_OnProcessCompleteNativeThread));
        }

        private void imageProcessService_OnProgressUpdate(object sender, ApplyFilterEventArgs e)
        {
            Invoke(new ImageFilterEventHandler(imageProcessService_OnProgressUpdateNativeThread), sender, e);
        }

        private void imageProcessService_OnProgressUpdateNativeThread(object sender, ApplyFilterEventArgs e)
        {
            progressBarImageProcess.Value = e.PercentComplete;
        }

        private void imageProcessService_OnProcessCompleteNativeThread(object sender, EventArgs e)
        {
            progressBarImageProcess.Value = 0;
            pictureBoxOutput.Image = _imageProcessService.GetOutputBitmap();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "JPG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png|JPEG Files (*.jpeg)|*.jpeg|GIF Files (*.gif)|*.gif";
            openFileDialog1.FileName = "";

            saveFileDialog1.Filter = "PNG Files (*.png)|*.png";
            saveFileDialog1.FileName = "";

            lblInputImageHeight.Text = "0 px";
            lblInputImageWidth.Text = "0 px";
        }

        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                selectedImagePath = openFileDialog1.FileName;
                try
                {
                    _imageProcessService.LoadImage(openFileDialog1.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Error opening Image: " + ex.Message, "Could not open Image", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    selectedImagePath = "";
                }
                UpdateGUI();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            selectedImagePath = "";
            _imageProcessService.ClearImage();
            UpdateGUI();
        }

        private void UpdateGUI()
        {
            txtInputImagePath.Text = selectedImagePath;
            Size imageSize = _imageProcessService.GetOriginalImageSize();
            lblInputImageWidth.Text = imageSize.Width + " px";
            lblInputImageHeight.Text = imageSize.Height + " px";
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _imageProcessService.Dispose();
        }

        private async void btnApplyFilter_Click(object sender, EventArgs e)
        {
            if (!_imageProcessService.ImageLoaded)
            {
                MessageBox.Show("Please select an image before applying a filter", "No Image Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            if (!_imageProcessService.IsRunning)
            {
                SetFilterParameters();
                EdgeHandling edgeHandling = rbExtend.Checked ? EdgeHandling.Extend : EdgeHandling.Wrap;
                btnApplyFilter.Text = "Cancel";
                await _imageProcessService.ApplyFilter(_selectedImageFilter, edgeHandling);
                btnApplyFilter.Text = "Apply Filter";
            }
            else
            {
                await _imageProcessService.CancelApplyFilter();
                btnApplyFilter.Text = "Apply Filter";
            }
        }

        private void btnSaveImageToFile_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                if (!_imageProcessService.SaveImage(saveFileDialog1.FileName))
                    MessageBox.Show(this, "Error saving Image: ", "Could not save file", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void sliderGamma_Scroll(object sender, EventArgs e)
        {
            double gammaValue = 100;
            double sliderPercentOfMax = (sliderGamma.Value / (double) sliderGamma.Maximum);


            if (sliderPercentOfMax >= 0.5d)
            {
                gammaValue = (sliderGamma.Maximum - sliderGamma.Value) * 2;
                if (gammaValue <= 0.0)
                    gammaValue = 1;
            }
            else
            {
                gammaValue = ((sliderGamma.Maximum) / ((double)sliderGamma.Value))*0.5;
                gammaValue = gammaValue * 100;
            }


            gammaValue = Math.Round(gammaValue / 100d, 2);
            txtGammaCorrection.Text = gammaValue.ToString();
            _selectedFilterValues.Gamma = gammaValue;
        }

        private void sliderSmooth_Scroll(object sender, EventArgs e)
        {
            _selectedFilterValues.Smooth = Math.Max(Math.Round(sliderSmooth.Value / 10d, 2), 0.1d);
            txtSmoothAmount.Text = _selectedFilterValues.Smooth.ToString();
        }

        private void sliderSharpen_Scroll(object sender, EventArgs e)
        {
            _selectedFilterValues.Sharpen = Math.Max(Math.Round(sliderSharpen.Value / 10d, 2), 0.1d);
            txtSharpenAmount.Text = _selectedFilterValues.Sharpen.ToString();
        }

        private void sliderGaussianBlur_Scroll(object sender, EventArgs e)
        {
            _selectedFilterValues.Gaussian = Math.Max(Math.Round(sliderGaussianBlur.Value / 10d, 2), 0.1d);
            txtGaussianBlurPixels.Text = _selectedFilterValues.Gaussian.ToString();
        }

        private void SetFilterParameters()
        {
            foreach (var control in flowLayoutPanel1.Controls)
            {
                var radioButton = control as RadioButton;

                if (radioButton != null && radioButton.Checked)
                {
                    int tagId = int.Parse(radioButton.Tag.ToString());
                    switch (tagId)
                    {
                        case 1:
                            _selectedImageFilter = new GammaFilter(_selectedFilterValues.Gamma);
                            break;
                        case 2:
                            _selectedImageFilter = new BlurFilter(_selectedFilterValues.Smooth);
                            break;
                        case 3:
                            _selectedImageFilter = new SharpenFilter(_selectedFilterValues.Sharpen);
                            break;
                        case 4:
                            _selectedImageFilter = new EdgeDetectionFilter();
                            break;
                        case 5:
                            _selectedImageFilter = new GaussianBlurFilter(_selectedFilterValues.Gaussian);
                            break;
                        case 6:
                            _selectedImageFilter = new EmbossingFilter();
                            break;
                        case 7:
                            _selectedImageFilter = new MosaicFilter();
                            break;
                        default:
                            break;
                    }
                    return;
                }
            }
            
        }

        private void filterRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton != null)
            {
                int tagId = int.Parse(radioButton.Tag.ToString());
                switch (tagId)
                {
                    case 1:
                        _selectedImageFilter = new GammaFilter(_selectedFilterValues.Gamma);
                        tabControlFilterSettings.SelectedIndex = 0;
                        break;
                    case 2:
                        _selectedImageFilter = new BlurFilter(_selectedFilterValues.Smooth);
                        tabControlFilterSettings.SelectedIndex = 1;
                        break;
                    case 3:
                        _selectedImageFilter = new SharpenFilter(_selectedFilterValues.Sharpen);
                        tabControlFilterSettings.SelectedIndex = 2;
                        break;
                    case 4:
                        _selectedImageFilter = new EdgeDetectionFilter();
                        break;
                    case 5:
                        _selectedImageFilter = new GaussianBlurFilter(_selectedFilterValues.Gaussian);
                        tabControlFilterSettings.SelectedIndex = 3;
                        break;
                    case 6:
                        _selectedImageFilter = new EmbossingFilter();
                        break;
                    case 7:
                        _selectedImageFilter = new MosaicFilter();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}