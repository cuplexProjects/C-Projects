using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Forms;
using ImageView.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageView.UnitTest
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class FormAddBookmarkUnitTest
    {
        [TestMethod]
        public void ShowDialogTest()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Point startupPosition = new Point(500, 250);
            ImageReferenceElement imageReference= new ImageReferenceElement();
            
            Application.Run(new FormAddBookmark(startupPosition,imageReference));
        }
        
    }
}
