using GeneralToolkitLib.Encryption.Licence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class SystemInfoTest
    {
        [TestMethod]
        public void GetSystemInfo()
        {
            var sysInfo = SysInfoManager.GetComputerId();
        }
    }
}