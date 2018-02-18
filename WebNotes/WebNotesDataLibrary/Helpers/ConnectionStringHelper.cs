using System.Configuration;

namespace WebNotesDataLibrary.Helpers
{
    public static class ConnectionStringHelper
    {
        public static string GetWebNotesConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["Webnotes"].ConnectionString;
        }

        public static string GetErrorLogConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["ErrorLog"].ConnectionString;
        }
    }
}
