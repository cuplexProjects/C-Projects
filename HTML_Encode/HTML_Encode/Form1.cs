using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace HTML_Encode
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void txtPasteBin_TextChanged(object sender, EventArgs e)
        {
            if(this.txtPasteBin.Text.Length == 0)
                return;

            try
            {
                string encodedText = this.radioSingleExpression.Checked ? this.HtmlEncodeString(this.txtPasteBin.Text) : this.HtmlEncodeDocument(this.txtPasteBin.Text,'\'');
                
                if(chkUseClipboard.Checked)
                {
                    //IntPtr hwnd = WindowsAPI.GetOpenClipboardWindow();
                    //if(hwnd != IntPtr.Zero)
                    //{
                    //    int processId;
                    //    WindowsAPI.GetWindowThreadProcessId(hwnd, out processId);
                    //    Process p = Process.GetProcessById(processId);
                    //    Console.WriteLine(p.Modules[0].FileName);
                    //}

                    string unicodeText = Utf8ToUtf16(encodedText + " ");
                    WindowsAPI.EmptyClipboard();
                    IntPtr strPointer = Marshal.StringToHGlobalUni(unicodeText);
                    WindowsAPI.SetClipboardData((int)WindowsAPI.uFormat.CF_TEXT, strPointer);
                    Marshal.FreeHGlobal(strPointer);
                }

                txtHtmlEncodedResult.Text = encodedText;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string HtmlEncodeDocument(string text, char delimiter)
        {
            int startPosition = 1;
            if(string.IsNullOrEmpty(text))
                return "";

            while (startPosition > 0)
            {
                startPosition = text.IndexOf(delimiter, startPosition);

                if(startPosition == text.Length - 1 || startPosition < 0)
                    break;
                else
                    startPosition++;

                int endPosition = text.IndexOf(delimiter, startPosition);

                if(endPosition < 0)
                    break;
                else
                    endPosition++;

                string textToEncode = text.Substring(startPosition, endPosition - startPosition-1);
                

                if(startPosition > 1 && text[startPosition - 1] == '[')
                    continue;

                if (endPosition + 1 < text.Length && text[endPosition] == ']')
                    continue;

                text = text.Replace(textToEncode, WebUtility.HtmlEncode(textToEncode));
            }
            return text;
        }

        private string HtmlEncodeString(string input)
        {
            return WebUtility.HtmlEncode(input);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtPasteBin.Text = "";
            txtHtmlEncodedResult.Text = "";
        }

        private static string Utf8ToUtf16(string utf8String)
        {
            // Get UTF8 bytes by reading each byte with ANSI encoding
            byte[] buffer = Encoding.UTF8.GetBytes(utf8String);
            string utf16Str = Encoding.Unicode.GetString(buffer, 0, buffer.Length);

            return utf16Str;
        }
        private string Utf16ToUtf8(string unicodeEncodedString)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(unicodeEncodedString);

            string utf8Str = Encoding.UTF8.GetString(buffer);
            return utf8Str;
        }
    }
}
