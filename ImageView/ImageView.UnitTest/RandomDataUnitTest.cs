using System;
using GeneralToolkitLib.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageView.UnitTest
{
    [TestClass]
    public class RandomDataUnitTest
    {
        [TestMethod]
        public void TestGetRandomHexValue()
        {
            string data = GeneralConverters.GetRandomHexValue(128);


            Assert.IsTrue(data.Length == 128, "Data was not in expected length");

        }
    }
}
