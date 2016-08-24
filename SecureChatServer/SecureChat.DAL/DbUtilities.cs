using System.Configuration;

namespace SecureChat.DAL
{
    public static class DbUtilities
    {
        public static string GetConnectionString()
        {
            var settings = ConfigurationManager.ConnectionStrings["SecureChat_Connection"];
            return settings != null ? settings.ConnectionString : "";
        }
    }
}
