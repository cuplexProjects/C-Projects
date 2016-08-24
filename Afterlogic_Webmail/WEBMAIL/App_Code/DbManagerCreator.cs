using System.Web.UI;

namespace WebMail
{
	/// <summary>
	/// Summary description for DbManagerCreator.
	/// </summary>
	public class DbManagerCreator : Control
	{
        public DbManager CreateDbManager()
		{
            DbManager newManager = null;
            WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
			switch(settings.DbType)
			{
                case SupportedDatabase.PostgreSql:
                    newManager = new PostgreSqlDbManager();
                    break;
                case SupportedDatabase.MsAccess:
                    newManager = new MsAccessDbManager();
					break;
                case SupportedDatabase.MySql:
                    newManager = new MySqlDbManager();
					break;
				default:
                    newManager = new MsSqlDbManager();
					break;
			}
			return newManager;
		}

        public DbManager CreateDbManager(string dataPath)
        {
            DbManager newManager = null;
            WebmailSettings settings = new WebmailSettings().CreateInstance(dataPath);
            switch (settings.DbType)
            {
                case SupportedDatabase.MsAccess:
                    newManager = new MsAccessDbManager();
                    break;
                case SupportedDatabase.MySql:
                    newManager = new MySqlDbManager();
                    break;
                case SupportedDatabase.PostgreSql:
                    newManager = new PostgreSqlDbManager();
                    break;
                default:
                    newManager = new MsSqlDbManager();
                    break;
            }
            return newManager;
        }

        public DbManager CreateDbManager(Account acct)
		{
            DbManager result = CreateDbManager();
			result.DbAccount = acct;
			return result;
		}
    }
}
