using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace TextSharpTestApp.Pdf
{
    public class PdfFactory
    {
        public static void CreatePdf(string filename, PdfContent pdfContent)
        {
            Document doc = new Document();
            var fs = File.OpenWrite(filename);
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);
            
            doc.Open();

            var paragraph = new Paragraph(pdfContent.Text1) {Font = pdfContent.PdfFont};
            doc.Add(paragraph);

            paragraph = new Paragraph(pdfContent.Text2) { Font = pdfContent.PdfFont };
            doc.Add(paragraph);

            paragraph = new Paragraph(pdfContent.Text3) { Font = pdfContent.PdfFont };
            doc.Add(paragraph);

            doc.Add(pdfContent.BackgroundImage);

            doc.AddTitle("Hello World example");
            doc.AddSubject("This is an Example 4 of Chapter 1 of Book 'iText in Action'");
            doc.AddKeywords("Metadata, iTextSharp 5.4.4, Chapter 1, Tutorial");
            doc.AddCreator("iTextSharp 5.4.4");
            doc.AddAuthor("Martin Dahl");
            doc.AddHeader("Nothing", "No Header");

            doc.Close();

        }
    }
}
