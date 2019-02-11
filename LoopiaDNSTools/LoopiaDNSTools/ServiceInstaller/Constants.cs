namespace LoopiaDNSTools.ServiceInstaller
{
    public class Constants
    {
        public const string SERVICE_DEFAULT_NAME = "SecureChat Server";

        public enum ServerStatusFlags
        {
            Initializing = 1,
            DbConnectionSuccessful = 2,
            Configured = 3,
            TryingToConnectToDatabase = 4,
            DbConnectionFailed = 255,
            NotConfigured = 256
        }
    }
}