using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeneralToolkitLib.Barcode;
using GeneralToolkitLib.Utility;
using GeneralToolkitLib.Utility.RandomGenerator;
using Serilog;
using WiFiPasswordGenerator.Properties;
using WiFiPasswordGenerator.Settings;

namespace WiFiPasswordGenerator
{
    /// <summary>
    ///     Main user form
    /// </summary>
    public partial class MainForm : Form
    {
        private const int MainPanelBorderWith = 1;
        private const int MaxPasswordLength = 500;

        private readonly ActiveSettings _activeSettings;
        private readonly Pen _innerPen;
        private readonly Pen _outerPen;
        private Size _qrOutputSize;

        /// <summary>
        ///     Constructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            var outerBorderColor = Color.FromArgb(0x80, 0x80, 0x80, 0x80);
            var innerBorderColor = Color.FromArgb(0x80, 0xB0, 0xC4, 0xDE);
            Brush innerBrush = new SolidBrush(innerBorderColor);
            Brush outerBrush = new SolidBrush(outerBorderColor);

            _innerPen = new Pen(innerBrush);
            _outerPen = new Pen(outerBrush);
            _activeSettings = new ActiveSettings();
        }

        /// <summary>
        ///     Releases unmanaged resources and performs other cleanup operations before the
        ///     <see cref="T:System.ComponentModel.Component" /> is reclaimed by garbage collection.
        /// </summary>
        private void MainForm_Load(object sender, EventArgs e)
        {
            linkLabelLastQRPath.Text = "";
            Text = Application.ProductName + Resources.Version_ + Application.ProductVersion;
            Log.Verbose("Main form loaded");
        }

        private void pnlMain_Paint(object sender, PaintEventArgs e)
        {
            using (var g = e.Graphics)
            {
                g.Clear(BackColor);
                g.InterpolationMode = InterpolationMode.Bilinear;
                var bordeRectangle = e.ClipRectangle;
                bordeRectangle.Width--;
                bordeRectangle.Height--;
                g.DrawRectangle(_outerPen, bordeRectangle);
                g.DrawRectangle(_innerPen,
                    Rectangle.FromLTRB(MainPanelBorderWith, MainPanelBorderWith, Width - MainPanelBorderWith * 2,
                        Height - MainPanelBorderWith * 2));
            }
        }

        private void UpdateActiveSettingsFromGuiUpdate()
        {
            // Update QR ECC Level
            foreach (Control control in flowLayoutQRSettings.Controls)
            {
                if (control is RadioButton radioButton && radioButton.Checked)
                {
                    switch (radioButton.Text.ToUpper()[0])
                    {
                        //L
                        case (char)76:
                            _activeSettings.QR_CodeLevel = QR_CodeLevels.L;
                            break;

                        //M
                        case (char)77:
                            _activeSettings.QR_CodeLevel = QR_CodeLevels.M;
                            break;

                        //Q
                        case (char)81:
                            _activeSettings.QR_CodeLevel = QR_CodeLevels.Q;
                            break;

                        //H
                        case (char)72:
                            _activeSettings.QR_CodeLevel = QR_CodeLevels.H;
                            break;
                    }

                    break;
                }
            }

            // Update Password Type
            foreach (Control control in flowLayoutOutputType.Controls)
            {
                if (!(control is RadioButton radioButton) || !radioButton.Checked) continue;
                if (!int.TryParse(radioButton.Tag.ToString(), out int checkBoxIndex))
                    break;

                switch (checkBoxIndex)
                {
                    case 0:
                        _activeSettings.PasswordType = PasswordTypes.StandardMixedChars;
                        break;
                    case 1:
                        _activeSettings.PasswordType = PasswordTypes.AlphaNumeric;
                        break;
                    case 2:
                        _activeSettings.PasswordType = PasswordTypes.Numeric;
                        break;
                    case 3:
                        _activeSettings.PasswordType = PasswordTypes.Base64;
                        break;
                    case 4:
                        _activeSettings.PasswordType = PasswordTypes.Hex;
                        break;
                    default:
                        Log.Error("checkBoxIndex unknown: " + checkBoxIndex);
                        break;
                }

                break;
            }

            UpdateActiveSettingsFromPasswordLength();
        }

        private void UpdateActiveSettingsFromPasswordLength()
        {
            int passwordLnegth = int.Parse(txtPasswordLength.Text);
            _activeSettings.PasswordLength = passwordLnegth;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            pnlMain.Invalidate();
        }

        private void MainForm_ResizeBegin(object sender, EventArgs e)
        {
            pnlMain.Invalidate();
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            pnlMain.Invalidate();
        }

        private void rbPasswordType_Click(object sender, EventArgs e)
        {
            UpdateActiveSettingsFromGuiUpdate();
        }

        private void rbQRCodeLevel_Click(object sender, EventArgs e)
        {
            UpdateActiveSettingsFromGuiUpdate();
        }

        private void txtPasswordLength_Validating(object sender, CancelEventArgs e)
        {
            if (!IsValidPasswordLength())
            {
                e.Cancel = true;
                toolTipPasswordLength.Active = true;
            }
            else
            {
                toolTipPasswordLength.Active = false;
            }
        }

        private bool IsValidPasswordLength()
        {
            int keyVal;
            var validData = false;
            if (int.TryParse(txtPasswordLength.Text, out keyVal))
                validData = keyVal > 0 && keyVal <= MaxPasswordLength;

            return validData;
        }

        private void txtPasswordLength_Validated(object sender, EventArgs e)
        {
            UpdateActiveSettingsFromPasswordLength();
        }

        private void txtPasswordLength_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!KeyValueNumericValidator.ValidateIntegerInput(e.KeyChar))
            {
                e.Handled = true;
                e.KeyChar = char.MaxValue;
            }
        }

        private void grpBoxQRCode_Resize(object sender, EventArgs e)
        {
            var margin = 15;
            int squareSize = Math.Min(grpBoxQRCode.Width - margin * 2, grpBoxQRCode.Height - margin * 2);

            PicBoxQRCode.SetBounds(margin, margin, squareSize, squareSize);

            PicBoxQRCode.Width = squareSize;
            PicBoxQRCode.Left = (grpBoxQRCode.Width - PicBoxQRCode.Width) / 2;
        }

        private void rbDefaultRes_CheckedChanged(object sender, EventArgs e)
        {
            pnlUserDefinedRes.Enabled = rbUserDefined.Checked;
            SetUSerDefinedQRSize(rbUserDefined.Checked);
        }

        private void SetUSerDefinedQRSize(bool userDinened)
        {
            try
            {
                if (userDinened)
                {
                    _qrOutputSize.Width = int.Parse(txtUserDefinedQRWidth.Text);
                    _qrOutputSize.Height = int.Parse(txtUserDefinedQRHeight.Text);
                }
                else
                {
                    _qrOutputSize = Size.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Parse error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtUserDefinedQRWidth_Validating(object sender, CancelEventArgs e)
        {
            var validData = false;
            if (int.TryParse(txtPasswordLength.Text, out int keyVal))
                validData = keyVal > 0 && keyVal <= 500;

            if (!validData)
                e.Cancel = true;
        }

        private void txtUserDefinedQRHeight_Validating(object sender, CancelEventArgs e)
        {
            var validData = false;
            if (int.TryParse(txtPasswordLength.Text, out int keyVal))
                validData = keyVal > 0 && keyVal <= 500;

            if (!validData)
                e.Cancel = true;
        }

        private void txtUserDefinedQRWidth_Validated(object sender, EventArgs e)
        {
            _qrOutputSize.Width = int.Parse(txtUserDefinedQRWidth.Text);
        }

        private void txtUserDefinedQRHeight_Validated(object sender, EventArgs e)
        {
            _qrOutputSize.Height = int.Parse(txtUserDefinedQRHeight.Text);
        }

        private void linkLabelLastQRPath_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (sender is LinkLabel linkLabel) Process.Start(linkLabel.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Open Link Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void contextMenuItemCopy_Click(object sender, EventArgs e)
        {
            if (PicBoxQRCode.Image == null) return;
            Clipboard.Clear();
            Clipboard.SetImage(PicBoxQRCode.Image);
        }

        private void toolStripMenuItemCopyImgInStringEncoding_Click(object sender, EventArgs e)
        {
            ExportQRCodeImageToBase64PngData();
        }

        private void ExportQRCodeImageToBase64PngData()
        {
            const int resolution = 500;
            if (PicBoxQRCode.Image != null)
            {
                Clipboard.Clear();
                var bitmap = new Bitmap(PicBoxQRCode.Image);
                if (bitmap.Width != resolution || bitmap.Height != resolution)
                    bitmap.SetResolution(resolution, resolution);

                var memoryStream = new MemoryStream();
                var encoderParameter = new EncoderParameter(Encoder.Quality, 100);
                bitmap.Save(memoryStream, GetEncoderInfo(ImageFormat.Png), new EncoderParameters(1) { Param = new[] { encoderParameter } });
                memoryStream.Position = 0;
                Clipboard.Clear();
                Clipboard.SetText(Convert.ToBase64String(memoryStream.ToArray(), 0, Convert.ToInt32(memoryStream.Length), Base64FormattingOptions.InsertLineBreaks), TextDataFormat.Text);
            }
        }

        private void copyToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(txtGeneratedPassword.Text);
        }

        private void txtPasswordLength_TextChanged(object sender, EventArgs e)
        {
            if (IsValidPasswordLength())
            {
                txtGeneratedPassword.Tag = txtGeneratedPassword.Text;
            }
            else
            {
                var previousText = txtGeneratedPassword.Tag as string;
                txtGeneratedPassword.Text = !string.IsNullOrWhiteSpace(previousText) ? previousText : "63";
            }
        }

        private void ImportPasswordFromClipboard()
        {
            if (Clipboard.ContainsText(TextDataFormat.Text))
            {
                string txtData = Clipboard.GetText(TextDataFormat.Text);
                if (!string.IsNullOrWhiteSpace(txtData) && txtData.Length > 0 && txtData.Length <= MaxPasswordLength)
                    txtGeneratedPassword.Text = txtData;
                else
                    MessageBox.Show("The clipboard data did not contain a string between 1 and 500 characters long", "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void setTextFromClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportPasswordFromClipboard();
        }

        private async void generateQRCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsValidPasswordLength() && IsValidSsid())
            {
                await GnerateQrCode();
            }
            else
            {
                MessageBox.Show(this, "Invalid password leangth or SSID", "Ivalid input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private bool IsValidSsid()
        {
            Regex ssidRegex = new Regex(@"^[\w-\.]{8,}$");
            return txtSSId.Text.Length >= 8 && ssidRegex.IsMatch(txtSSId.Text);
        }

        private void toolStripMenuItemImportPassword_Click(object sender, EventArgs e)
        {
            ImportPasswordFromClipboard();
        }

        private void toolStripMenuItemImportBase64ImgData_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText(TextDataFormat.Text))
            {
                string txtData = Clipboard.GetText(TextDataFormat.Text);
                try
                {
                    byte[] pngBytes = Convert.FromBase64String(txtData);
                    var memoryStream = new MemoryStream(pngBytes);
                    var bitmap = new Bitmap(memoryStream);
                    PicBoxQRCode.Image = bitmap;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Could not import Image", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void toolStripMenuItemExportQRImage_Click(object sender, EventArgs e)
        {
            ExportQRCodeImageToBase64PngData();
        }

        private void toolStripMenuItemExportPwdStr_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtGeneratedPassword.Text) && txtGeneratedPassword.Text.Length > 0 && txtGeneratedPassword.Text.Length <= MaxPasswordLength)
            {
                Clipboard.Clear();
                Clipboard.SetText(txtGeneratedPassword.Text);
            }
        }

        private void txtSSId_Validating(object sender, CancelEventArgs e)
        {
            if (txtSSId.Text.Length > 0 && !IsValidSsid())
            {
                e.Cancel = true;
            }
        }

        private void txtSSId_Validated(object sender, EventArgs e)
        {

        }

        #region GenerateOutput Methods

        private async void btnGenerate_Click(object sender, EventArgs e)
        {
            var secureRandomGenerator = new SecureRandomGenerator();
            txtGeneratedPassword.Text = await secureRandomGenerator.GetRandomStringFromPasswordType(_activeSettings.PasswordType, _activeSettings.PasswordLength);

            // Always generate QR even if the SSID is invalid, but inform instead.
            await GnerateQrCode();

            // Redundent code now that the text field validation functiction prevents entering invalid characters
            if (!IsValidSsid() && txtSSId.Text.Length > 0)
            {
                txtSSId.Text = "";
                MessageBox.Show(this, "QR Code was created with the password but The SSID is invalid", "Invalid caracters", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private async Task GnerateQrCode()
        {
            await Task.Run(() =>
            {
                var qrCodeGenerator = new QRCodeGenerator();
                string enValue = _activeSettings.QR_CodeLevel.ToString();
                var ecc = (QRCodeGenerator.ECCLevel)Enum.Parse(typeof(QRCodeGenerator.ECCLevel), enValue);
                string encoderContent = CreateWifimetadataFormatString(txtSSId.Text, rdWPA.Checked, txtGeneratedPassword.Text, rdSSIDVisibleFalse.Checked);

                var qrCode = qrCodeGenerator.CreateQrCode(encoderContent, ecc);

                int moduleCount = qrCode.ModuleMatrix.Count;
                if (_qrOutputSize == Size.Empty)
                {
                    PicBoxQRCode.Image = qrCode.GetGraphic(Math.Min(500, PicBoxQRCode.Height) / moduleCount);
                }
                else
                {
                    PicBoxQRCode.Image = qrCode.GetGraphic(_qrOutputSize.Height / moduleCount);
                    var b = new Bitmap(PicBoxQRCode.Image);
                    b.SetResolution(_qrOutputSize.Width, _qrOutputSize.Height);
                    PicBoxQRCode.Image = b;
                    PicBoxQRCode.Invalidate();
                }
            });
            PicBoxQRCode.Refresh();
            PicBoxQRCode.BorderStyle = BorderStyle.None;
            PicBoxQRCode.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void btnSaveQRCode_Click(object sender, EventArgs e)
        {
            if (PicBoxQRCode.Image != null)
            {
                saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
                if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    string fileName = saveFileDialog1.FileName;

                    try
                    {
                        ImageCodecInfo imageCodecInfo;
                        var encoderParameters = new EncoderParameters(1);

                        if (fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                        {
                            imageCodecInfo = GetEncoderInfo(ImageFormat.Png);
                            encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 100);
                        }
                        else if (fileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                 fileName.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                        {
                            imageCodecInfo = GetEncoderInfo(ImageFormat.Jpeg);
                            encoderParameters.Param[0] = new EncoderParameter(Encoder.Compression, 50);
                        }
                        else
                        {
                            throw new Exception("Unsupported image type");
                        }

                        var img = PicBoxQRCode.Image;
                        if (_qrOutputSize != Size.Empty)
                        {
                            var b = new Bitmap(img);
                            b.SetResolution(_qrOutputSize.Width, _qrOutputSize.Height);
                        }

                        img.Save(fileName, imageCodecInfo, encoderParameters);
                        linkLabelLastQRPath.Text = fileName;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Save error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                return;
            }

            MessageBox.Show("Please generate a password first", "Nothing to save", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Output format is: WIFI:S:<SSID>;T:<WPA|WEP|>;P:<password>;H:<true|false|>;
        private string CreateWifimetadataFormatString(string ssid, bool WPAEncryption, string password, bool ssidHidden)
        {
            // Replace special characters in ssid
            if (ssid.Contains(';'))
            {
                ssid = ssid.Replace(";", @"\;");
            }

            string tValue = WPAEncryption ? "WPA" : "WEP";
            string template = $"WIFI:S:{ssid};T:{tValue};P:{password};H:{ssidHidden.ToString()}";
            return template;
        }

        private ImageCodecInfo GetEncoderInfo(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
        }

        #endregion
    }
}