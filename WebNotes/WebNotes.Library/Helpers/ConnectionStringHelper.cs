using System.Configuration;

namespace WebNotes.Library.Helpers
{
    public static class ConnectionStringHelper
    {
        public static string WebNotes => ConfigurationManager.ConnectionStrings["WebNotes"].ConnectionString;
    }
}
