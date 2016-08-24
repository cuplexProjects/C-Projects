public class AdminPanelConstants
{
	protected AdminPanelConstants() { }

	public const string serverTab = "server.tab";
	public const string serverSettings = "XMServerSettings";
	public const string WebMailFolderPath = "WebMailFolderPath";
    public const string DummyPassword = "*******";
    public const int PageSize = 20;
    public const int SessionTimeout = 120;

    public struct UserType
    {
        public const string xm = "xm";
        public const string xma = "xma";
        public const string wm = "wm";
        public const string xmc = "xmc";
    }

    public struct PluginName
    {
        public const string Domains = "Domains";
        public const string Users = "Users";
        public const string Server = "Server";
        public const string WebMail = "WebMail";
        public const string WebMailLite = "WebMailLite";
        public const string Security = "Security";
        public const string LicenseKey = "LicenseKey";
        public const string Upgrade = "Upgrade";
        public const string Archiving = "Archiving";
        public const string Subadmins = "Subadmins";
    }
}
