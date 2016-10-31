using System.Diagnostics.CodeAnalysis;
using ImageView.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageView.UnitTest
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ThumbnailTest
    {
        [TestMethod]
        public void TestThumbnailDirectoryScan()
        {
            ThumbnailService thumbnailService=new ThumbnailService("f:\\");
            thumbnailService.Init();
        }

    }
}
