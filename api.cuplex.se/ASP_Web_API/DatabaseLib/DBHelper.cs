using System;
using System.Configuration;
using System.Globalization;
using DatabaseLib.Linq;

namespace DatabaseLib
{
    public static class DBHelper
    {
        private static CultureInfo defaultCulture = new CultureInfo("sv-SE");
        private static string _connectionString;
        public static string GetConnectionString()
        {
            if (_connectionString != null)
                return _connectionString;

            var settings = ConfigurationManager.ConnectionStrings["ConnectionString1"];
            if (settings != null)
            {
                _connectionString = settings.ConnectionString;
                return settings.ConnectionString;
            }

            return "";
        }

        public static void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static bool CanConnectToDatabase()
        {
            try
            {
                using (var db = ApiDataContext.Create())
                {
                    return db.DatabaseExists();
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
