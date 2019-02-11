using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PriceCalculator.Settings;

namespace PriceCalculator
{
    public partial class frmMain : Form
    {
        private readonly SettingsService _settingsService;
        private const string VALID_DECIMAL_INPUTS = "0123456789,\b";
        private const string VALID_INTEGER_INPUTS = "0123456789,\b";
        private string clipboardStrData;
        private int numberOfItems = 1;

        public frmMain()
        {
            InitializeComponent();
            _settingsService = SettingsService.Instance;
            _settingsService.LoadSettings();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            txtBitcoinPrice.Text = _settingsService.Settings.BTC_Price.ToString();
            txtSekUsdRate.Text = _settingsService.Settings.SEK_USD_Rate.ToString();
            txtCommision.Text = _settingsService.Settings.Commision.ToString();
            chkTopmost.Checked = _settingsService.Settings.Topmost;
            chkAutofocusPrice.Checked = _settingsService.Settings.AutofocusPrice;

            txtBitcoinPrice.KeyPress += DecimalTextbox_KeyPress;
            txtBitcoinPrice.Validating += DecimalTextbox_Validating;
            txtBitcoinPrice.Validated += txtBitcoinPrice_Validated;

            txtSekUsdRate.KeyPress += DecimalTextbox_KeyPress;
            txtSekUsdRate.Validating += DecimalTextbox_Validating;
            txtSekUsdRate.Validated += txtSekUsdRate_Validated;

            txtCommision.KeyPress += DecimalTextbox_KeyPress;
            txtCommision.Validating += DecimalTextbox_Validating;
            txtCommision.Validated += txtCommision_Validated;
            txtCommision.TextChanged += txtCommision_TextChanged;

            txtPriceInBTC.KeyPress += DecimalTextbox_KeyPress;
            txtPriceInBTC.Validating += DecimalTextbox_Validating;
            txtPriceInBTC.TextChanged += txtPriceInBTC_TextChanged;

            txtNumberOfItems.KeyPress += IntegerTextbox_KeyPress;
            txtNumberOfItems.Validating += IntegerTextbox_Validating;
            txtNumberOfItems.TextChanged += txtNumberOfItems_TextChanged;

            txtPriceInBTC.Select();
            if(_settingsService.Settings.StartPosition != null)
            {
                try
                {
                    VerifyStartPositionSettings();
                    this.Location = new Point(_settingsService.Settings.StartPosition.X, _settingsService.Settings.StartPosition.Y);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        void txtNumberOfItems_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(txtNumberOfItems.Text, out numberOfItems);

            if(numberOfItems <= 0)
                numberOfItems = 1;

            UpdatePrice();
        }

        private void VerifyStartPositionSettings()
        {
            var firstScreen = Screen.PrimaryScreen.Bounds;
            //if (_settingsService.Settings.StartPosition.X < firstScreen.Bounds.Left)
            //    _settingsService.Settings.StartPosition.X = firstScreen.Bounds.Left;

            //if (_settingsService.Settings.StartPosition.Y < 0)
            //    _settingsService.Settings.StartPosition.Y = 0;

            //var lastScreen = Screen.AllScreens.OrderBy(s => s.Bounds.X).Last();
            //int maxX = lastScreen.Bounds.X + lastScreen.Bounds.Width;

            //if(_settingsService.Settings.StartPosition.X + this.Width > maxX)
            //    _settingsService.Settings.StartPosition.X = maxX - this.Width;

            //var curentScreen = Screen.AllScreens.FirstOrDefault(s => _settingsService.Settings.StartPosition.X >= s.Bounds.X && _settingsService.Settings.StartPosition.X <= s.Bounds.X + s.Bounds.Width);
            //if(curentScreen == null) return;
            //if(this._settingsService.Settings.StartPosition.Y + this.Height > curentScreen.Bounds.Height)
            //    this._settingsService.Settings.StartPosition.Y = curentScreen.Bounds.Height - this.Height;

            //if (this._settingsService.Settings.StartPosition.X + this.Width > curentScreen.Bounds.Right)
            //    this._settingsService.Settings.StartPosition.X = curentScreen.Bounds.Width - this.Width;

            if (_settingsService.Settings.StartPosition.X < 0)
                _settingsService.Settings.StartPosition.X = 0;

            if (_settingsService.Settings.StartPosition.X+this.Width > firstScreen.Right)
                _settingsService.Settings.StartPosition.X = firstScreen.Right-this.Width;


            if (_settingsService.Settings.StartPosition.Y < 0)
                _settingsService.Settings.StartPosition.Y = 0;

            if (_settingsService.Settings.StartPosition.Y + this.Height > firstScreen.Bottom)
                _settingsService.Settings.StartPosition.Y = firstScreen.Bottom - this.Height;

        }

        void txtCommision_TextChanged(object sender, EventArgs e)
        {
            UpdatePrice();
        }

        void txtPriceInBTC_TextChanged(object sender, EventArgs e)
        {
            UpdatePrice();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdatePrice();
        }

        void txtCommision_Validated(object sender, EventArgs e)
        {
            double commision;
            if (double.TryParse(txtCommision.Text, out commision))
                _settingsService.Settings.Commision = commision;
        }

        void txtSekUsdRate_Validated(object sender, EventArgs e)
        {
            double price;
            if (double.TryParse(txtSekUsdRate.Text, out price))
                _settingsService.Settings.SEK_USD_Rate = price;
        }

        void txtBitcoinPrice_Validated(object sender, EventArgs e)
        {
            double price;
            if(double.TryParse(txtBitcoinPrice.Text, out price))
                _settingsService.Settings.BTC_Price = price;
        }

        void DecimalTextbox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TextBox textBoxValidating = sender as TextBox;
            if(textBoxValidating != null)
            {
                double testVal;
                if(double.TryParse(textBoxValidating.Text, out testVal))
                    textBoxValidating.Text = testVal.ToString();
                else
                    e.Cancel = true;
            }
        }

        void IntegerTextbox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TextBox textBoxValidating = sender as TextBox;
            if (textBoxValidating != null)
            {
                int testVal;
                if (int.TryParse(textBoxValidating.Text, out testVal))
                    textBoxValidating.Text = testVal.ToString();
                else
                    e.Cancel = true;
            }
        }

        void DecimalTextbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!VALID_DECIMAL_INPUTS.Contains(e.KeyChar.ToString()))
            {
                e.KeyChar = (char)0;
                e.Handled = true;
            }
        }

        void IntegerTextbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!VALID_INTEGER_INPUTS.Contains(e.KeyChar.ToString()))
            {
                e.KeyChar = (char)0;
                e.Handled = true;
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            this._settingsService.Settings.StartPosition = new Coordinate(this.Location.X, this.Location.Y);
            _settingsService.SaveSettings();
        }
        
        private void chkTopmost_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = chkTopmost.Checked;
            _settingsService.Settings.Topmost = chkTopmost.Checked;
        }

        private void UpdatePrice()
        {
            if(string.IsNullOrEmpty(txtPriceInBTC.Text))
                return;

            double commision = 1;
            double price;
            double itemMultiplier = 1;

            if(numberOfItems != 0)
                itemMultiplier = 1 / (double)numberOfItems;

            if(double.TryParse(txtCommision.Text, out commision))
            {
                commision = (commision / 100) + 1;
            }

            if(double.TryParse(txtPriceInBTC.Text, out price))
            {
                txtPriceInUSD.Text = (_settingsService.Settings.BTC_Price * price * commision * itemMultiplier).ToString("N");
                txtPriceInSEK.Text = (_settingsService.Settings.BTC_Price * price * _settingsService.Settings.SEK_USD_Rate * commision * itemMultiplier).ToString("N");
            }
        }

        private void frmMain_Activated(object sender, EventArgs e)
        {
            if(_settingsService.Settings.AutofocusPrice)
            {
                txtPriceInBTC.Select();
                txtPriceInBTC.Focus();
            }

            if(!Clipboard.ContainsText(TextDataFormat.Text)) return;
            string clipboardText = Clipboard.GetText(TextDataFormat.Text);
            if (clipboardText != this.clipboardStrData)
            {
                clipboardText = clipboardText.Replace('.', ',');
                double btcPrice;
                if(double.TryParse(clipboardText, out btcPrice))
                {
                    txtPriceInBTC.Text = btcPrice.ToString();
                    if(!_settingsService.Settings.AutofocusPrice)
                    {
                        grpResult.Select();
                        grpResult.Focus();    
                    }
                }
            }
        }

        private void frmMain_Deactivate(object sender, EventArgs e)
        {
            clipboardStrData = Clipboard.GetText(TextDataFormat.Text);
        }

        private void chkAutofocusPrice_CheckedChanged(object sender, EventArgs e)
        {
            _settingsService.Settings.AutofocusPrice = chkAutofocusPrice.Checked;
        }
    }
}