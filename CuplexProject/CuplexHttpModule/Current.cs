using System.Web;
using System.Configuration;

namespace cms
{
    public class Current
    {
        public static string GetRootPath
        {
            get
            {
                HttpContext context = HttpContext.Current;
                if (context.Request.ApplicationPath.EndsWith("/"))
                    return context.Request.Url.Scheme + "://" + context.Request.Url.Authority + context.Request.ApplicationPath;
                else
                    return context.Request.Url.Scheme + "://" + context.Request.Url.Authority + context.Request.ApplicationPath + "/";
            }
        }
        public static string DomainUrl
        {
            get
            {
                return ConfigurationSettings.AppSettings["DomainUrl"];
            }
        }

        public static string HTTPS_DomainUrl
        {
            get
            {
                return ConfigurationSettings.AppSettings["HTTPS_DomainUrl"];
            }
        }

        public static string LocalFilePath
        {
            get 
            {
                HttpContext context = HttpContext.Current;
                return context.Request.PhysicalApplicationPath;
            }
        }
    }
}
