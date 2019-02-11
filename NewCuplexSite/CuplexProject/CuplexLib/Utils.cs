using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Threading;
using System.Globalization;
using System.Web;

namespace CuplexLib
{
    public static class Utils
    {
        private static CultureInfo defaultCulture = new CultureInfo("sv-SE");
        public static string GetConnectionString()
        {
            System.Configuration.ConnectionStringSettings settings = null;

            if (System.Web.HttpContext.Current != null)
            {
                string host = System.Web.HttpContext.Current.Request.Headers["Host"];

                settings = System.Configuration.ConfigurationManager.ConnectionStrings[host];
                if (settings != null)
                    return settings.ConnectionString;
            }

            settings = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString1"];
            if (settings != null)
                return settings.ConnectionString;            

            return "";
        }
        public static string GetMd5Hash(string input)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
        public static bool ValidateEmail(string emailAddress)
        {
            string regExpPattern = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
            Regex regExpression = new Regex(regExpPattern);

            return regExpression.IsMatch(emailAddress);
        }
        public static string TruncateString(string inputString, int maxLength)
        {
            if (inputString.Length > maxLength)
                inputString = inputString.Substring(0, maxLength);
            return inputString;
        }
        public static string GetResourceText(string resourceKey)
        {
            //CultureInfo culture = Thread.CurrentThread.CurrentUICulture;
            return HttpContext.GetGlobalResourceObject("Resources", resourceKey, defaultCulture) as string;
        }
        public static void SetGlobalization()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("sv-SE");
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }
    }
}
