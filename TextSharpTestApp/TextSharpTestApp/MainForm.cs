using System;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using iTextSharp.text;
using TextSharpTestApp.Pdf;
using Font = System.Drawing.Font;

namespace TextSharpTestApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "";
            saveFileDialog1.Filter = "(*.pdf) Pdf Files|*.pdf";
            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                txtFilename.Text = saveFileDialog1.FileName;
            }
        }

        private void btnGeneratePdf_Click(object sender, EventArgs e)
        {
            string fileName = txtFilename.Text;
            if (string.IsNullOrEmpty(fileName))
            {
                MessageBox.Show(this, "Filename cant be empty", "error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            try
            {

                PdfContent pdfContent = new PdfContent
                {
                    Text1 = txtText1.Text,
                    Text2 = txtText2.Text,
                    Text3 = txtText3.Text,
                    PdfFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, txtFont.Font.Size)
                };
                //var fontFactory = new FontFactoryImp();
                //fontFactory.RegisterFamily(txtFont.Font.FontFamily.Name, txtFont.Font.Name,);

                //pdfContent.PdfFont = new iTextSharp.text.Font(BaseFont.CreateFont(txtFont.Font.FontFamily.Name, iTextSharp.text.FontFactory.DefaultEncoding, true));
                //pdfContent.PdfFont.Size = txtFont.Font.Size;
                //pdfContent.PdfFont.SetStyle(txtFont.Font.Style.ToString());
                System.Drawing.Image img = pictureBoxBackground.Image;

                MemoryStream ms = new MemoryStream();
                img.Save(ms, ImageFormat.Jpeg);
                pdfContent.BackgroundImage = new Jpeg(ms.ToArray(), img.Width, img.Height);

                PdfFactory.CreatePdf(txtFilename.Text, pdfContent);
                MessageBox.Show(this, "Pdf Generated successfully", "Pdf created", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, exception.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void UpdateFont()
        {
            Font font = txtFont.Font;
            txtFont.Text = font.Name + ";" + font.Style + ";" + font.Size;
        }
        
        private void MainForm_Load(object sender, EventArgs e)
        {
            UpdateFont();
        }

        private void btnSetNewFont_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog(this) == DialogResult.OK)
            {
                txtFont.Font = fontDialog1.Font;
                UpdateFont();
            }
        }
    }
}
