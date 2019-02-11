using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace RegexpTester
{
    public partial class frmMain : Form
    {
        public readonly Color[] cMatchArr = { Color.Blue, Color.Yellow, Color.Green, Color.Red, Color.Orange, Color.Plum, Color.DarkRed, Color.Gray, Color.Magenta, Color.Lime };
        private Random _random = null;
        private int _randomSeed = 0;
        private System.Collections.Hashtable _dataStorage = new System.Collections.Hashtable();
        private bool _autoMatch;

        public frmMain()
        {            
            InitializeComponent();
            _randomSeed = Convert.ToInt32(DateTime.Now.Ticks % int.MaxValue);
            lblSilentError.Text = "";
        }

        private void chkOntop_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = chkOntop.Checked;
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            if (txtRegularExpression.Text.Length == 0)
            {
                lblMatchCount.Text = "0";
                return;
            }

            try
            {
                RegexOptions options = GetRegexOptions();
                Regex r = new Regex(txtRegularExpression.Text, options);

                txtResult.Rtf = "";
                txtResult.Text = r.Replace(txtSubjectString.Text, txtReplacement.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnMatch_Click(object sender, EventArgs e)
        {
            performMatch();
        }

        private void performMatch()
        {
            if (txtRegularExpression.Text.Length == 0)
            {
                lblMatchCount.Text = "0";
                return;
            }

            try
            {
                RegexOptions options = GetRegexOptions();
                Regex r = new Regex(txtRegularExpression.Text, options);

                int matchCnt = 0;
                _random = new Random(_randomSeed);
                MatchCollection matches = r.Matches(txtSubjectString.Text, 0);

                lblMatchCount.Text = matches.Count.ToString();
                txtResult.Rtf = "";
                txtResult.Text = txtSubjectString.Text;

                foreach (Match m in matches)
                {
                    int offset = Regex.Matches(txtSubjectString.Text.Substring(0, m.Index), "\r\n").Count;
                    txtResult.Select(m.Index - offset, m.Length);
                    if (matchCnt < cMatchArr.Length)
                        txtResult.SelectionBackColor = cMatchArr[matchCnt++];
                    else
                        txtResult.SelectionBackColor = GetRandomColor();

                    txtResult.SelectionColor = GetTextColor(txtResult.SelectionBackColor);
                    txtResult.SelectionFont = new Font(txtResult.SelectionFont.FontFamily, txtResult.SelectionFont.Size, FontStyle.Bold);
                }
            }
            catch (Exception ex)
            {
                if (_autoMatch)
                    throw ex;
                else
                    MessageBox.Show(ex.Message);
            }
        }

        private RegexOptions GetRegexOptions()
        {
            RegexOptions o = RegexOptions.None;
            if (!chkGlobal.Checked)
                o = o | RegexOptions.Singleline;
            if (chkIgnoreCase.Checked)
                o = o | RegexOptions.IgnoreCase;
            if (chkMultiline.Checked)
                o = o | RegexOptions.Multiline;
            return o;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtRegularExpression.Text = "";
            txtReplacement.Text = "";
            txtResult.Text = "";
            txtSubjectString.Text = "";
            lblSilentError.Text = "";
        }

        //Returns White or black depending on Color background so that the text will be readable
        private Color GetTextColor(Color back)
        {
            if (Brightness(back) > 128)
                return Color.Black;
            else
                return Color.White;
        }

        private static int Brightness(Color c)
        {
            return (int)Math.Sqrt(
               c.R * c.R * .241 +
               c.G * c.G * .691 +
               c.B * c.B * .068);
        }

        private Color GetRandomColor()
        {
            return System.Drawing.ColorTranslator.FromWin32(_random.Next(0xFFFFFF) | 0x7F000000);
        }

        private void autoMatch()
        {
            if (_autoMatch)
            {
                if (txtRegularExpression.Text.Length == 0)
                    txtResult.Rtf = "";
                else
                {
                    try
                    {
                        performMatch();
                        lblSilentError.Text = "";
                    }
                    catch (Exception ex)
                    {
                        lblSilentError.Text = ex.Message;
                    }
                }
            }
        }

        private void txtBox_KeyDown(object sender, KeyEventArgs e)
        {
            KeyMaping mapping = _dataStorage[(sender as Control).Name] as KeyMaping;
            if (mapping == null)
            {
                mapping = new KeyMaping();
                _dataStorage[(sender as Control).Name] = mapping;
            }

            mapping.AltPressed = e.Alt;
            mapping.CtrlPressed = e.Control;
            mapping.ShiftPressed = e.Shift;
            mapping.KeyValue = e.KeyValue;

            e.Handled = handleKeyCommand(sender, mapping);
        }

        private void txtBox_KeyUp(object sender, KeyEventArgs e)
        {
            KeyMaping mapping = _dataStorage[(sender as Control).Name] as KeyMaping;
            if (mapping == null)
            {
                mapping = new KeyMaping();
                _dataStorage[(sender as Control).Name] = mapping;
            }

            mapping.AltPressed = e.Alt;
            mapping.CtrlPressed = e.Control;
            mapping.ShiftPressed = e.Shift;
            mapping.KeyValue = e.KeyValue;
        }

        private bool handleKeyCommand(object sender, KeyMaping keyMaping)
        {
            TextBox textBox = sender as TextBox;

            //Ctrl + A = select all
            if (keyMaping.CtrlPressed && keyMaping.KeyValue == (int)'A')
            {                
                if (textBox != null)
                {
                    textBox.SelectAll();
                    return true;
                }                
            }
            return false;
        }

        private void chkAutoMatch_CheckedChanged(object sender, EventArgs e)
        {
            _autoMatch = chkAutoMatch.Checked;            
        }

        private void txtRegularExpression_TextChanged(object sender, EventArgs e)
        {
            autoMatch();
        }        

        private void txtSubjectString_TextChanged(object sender, EventArgs e)
        {
            autoMatch();
        }
    }
}