using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CLinq = CuplexLib.Linq;

namespace CuplexLib
{
    public class IpLookup
    {
        public string Registry { get; set; }
        public long Assigned { get; set; }
        public string Ctry { get; set; }
        public string Cntry { get; set; }
        public string Country { get; set; }

        public static IpLookup LookupIp(string ip)
        {
            IpLookup ipLookup = null;
            if (!VerifyIpNumber(ip))
                return null;
            else
            {
                long ipLng = ConvertIpToLong(ip);
                using (var db = CLinq.DataContext.Create())
                {
                    var dbQuery =
                    from ipl in db.IpLookups
                    where ipl.IPFrom <= ipLng && ipl.IPTo >= ipLng
                    select new IpLookup
                    {
                        Assigned = ipl.Assigned,
                        Cntry = ipl.Cntry,
                        Country = ipl.Country,
                        Ctry = ipl.Ctry,
                        Registry = ipl.Registry
                    };

                    ipLookup = dbQuery.Take(1).SingleOrDefault();
                }
            }
            return ipLookup;
        }
        public static long ConvertIpToLong(string ip)
        {
            long ipLng = 0;
            try
            {
                string[] ipArr = ip.Split(".".ToCharArray());
                if (ipArr.Length == 4)
                {
                    ipLng = long.Parse(ipArr[3]);
                    ipLng += long.Parse(ipArr[2])*256;
                    ipLng += long.Parse(ipArr[1]) * 65536;
                    ipLng += long.Parse(ipArr[0]) * 16777216;
                }
            }
            catch { return 0; }
            return ipLng;
        }
        public static bool VerifyIpNumber(string ip)
        {
            Regex ipMatch = new Regex(@"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b");
            return ipMatch.IsMatch(ip);
        }
    }
}