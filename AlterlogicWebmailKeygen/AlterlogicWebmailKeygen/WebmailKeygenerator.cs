using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlterlogicWebmailKeygen
{
    public enum LicenseTypes
    {
        Unlimited=1,
        UserLimited=2,
    }

    public class WebmailKeygenerator
    {
        private readonly LicenseTypes _licenseType;
        private DateTime _expireDate;
        private int _userLimit = 100;

        public WebmailKeygenerator(LicenseTypes licenseType, DateTime expireDate, int userLimit = 0)
        {
            _licenseType = licenseType;
            _expireDate = expireDate;
            _userLimit = userLimit;
        }

        public string GenerateLicenseKey()
        {
            string licenseKey = "";

            if (_licenseType == LicenseTypes.Unlimited)
            {
                licenseKey = "WM510-";
            }
            else
            {
                licenseKey = "WM500-";
            }

            return licenseKey;
        }
    }
}
