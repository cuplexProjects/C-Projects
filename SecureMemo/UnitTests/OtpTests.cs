using System;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecureMemo.Services;

namespace UnitTests
{
    [TestClass]
    public class OTPTests
    {
        private const string password = "8QIBwaFcwbrjcqFfbSDogrV6qhVfa6uJ";
        [TestMethod]
        public void TestCreateNewOtp()
        {
            string filename = Environment.CurrentDirectory + "\\" + "testOtpData.dat";
            OTPConfigService.Service.Create(password);
            OTPConfigService.Service.SaveSettings(filename, password);

            Assert.IsTrue(File.Exists(filename));

        }
    }
}
