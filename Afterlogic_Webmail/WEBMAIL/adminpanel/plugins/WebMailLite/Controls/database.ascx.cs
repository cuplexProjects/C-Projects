using System.Globalization;
using System;
using WebMail;

/// <summary>
///		Summary description for mailadm_database_settings.
/// </summary>
public partial class database_settingsLite : System.Web.UI.UserControl
{
    protected string controlPrefix = string.Empty;
    
    protected void Page_Load(object sender, System.EventArgs e)
	{
        controlPrefix = this.ClientID;

        intDbTypeMsSql_label.Attributes["for"] = intDbTypeMsSql.ClientID;
        intDbTypeMySql_label.Attributes["for"] = intDbTypeMySql.ClientID;
        intDbTypeMsAccess_label.Attributes["for"] = intDbTypeMsAccess.ClientID;

        if (!Page.IsPostBack) InitControl();
	}

    protected void InitControl()
    {
        WebmailSettings settings = new WebMailSettingsCreator().CreateWebMailSettings(AdminPanelUtils.GetWebMailDataFolder());
        if (Session["TrySqlPassword"] == null)
        {
            Session["TrySqlPassword"] = settings.DbPassword;
        }

        txtSqlPassword.Attributes.Add("Value", Session["TrySqlPassword"].ToString());

        switch (settings.DbType)
        {
            case SupportedDatabase.MsAccess:
                intDbTypeMsAccess.Checked = true;
                break;
            case SupportedDatabase.MsSqlServer:
                intDbTypeMsSql.Checked = true;
                break;
            case SupportedDatabase.MySql:
                intDbTypeMySql.Checked = true;
                break;
        }
        txtSqlLogin.Value = settings.DbLogin;
        txtSqlName.Value = settings.DbName;
        txtSqlDsn.Value = settings.DbDsn;
        txtSqlSrc.Value = settings.DbHost;
        txtAccessFile.Value = settings.DbPathToMdb;
        odbcConnectionString.Value = settings.DbCustomConnectionString;
        useCS.Checked = settings.UseCustomConnectionString;
        useDSN.Checked = settings.UseDSN;
    }

	private void Save()
	{
		Session["TrySqlPassword"] = null;
		try
		{
            WebmailSettings settings = new WebMailSettingsCreator().CreateWebMailSettings(AdminPanelUtils.GetWebMailDataFolder());
            
            if (intDbTypeMsAccess.Checked)
                settings.DbType = SupportedDatabase.MsAccess;
			else if (intDbTypeMySql.Checked)
                settings.DbType = SupportedDatabase.MySql;
			else
                settings.DbType = SupportedDatabase.MsSqlServer;

            string rrr = Request[txtSqlLogin.ClientID];

            if (useDSN.Checked)
            {
                settings.DbDsn = txtSqlDsn.Value;
            }
            else if (useCS.Checked)
            {
                settings.DbCustomConnectionString = odbcConnectionString.Value;
            }
            else
            {
                if (settings.DbType == SupportedDatabase.MsAccess)
                {
                    settings.DbPathToMdb = txtAccessFile.Value;
                }
                else
                {
                    settings.DbLogin = txtSqlLogin.Value;
                    settings.DbPassword = txtSqlPassword.Text;
                    settings.DbName = txtSqlName.Value;
                    settings.DbHost = txtSqlSrc.Value;
                }
            }
			settings.UseCustomConnectionString = useCS.Checked;
            settings.UseDSN = useDSN.Checked;

            settings.SaveWebmailSettings(AdminPanelUtils.GetWebMailDataFolder());
            this.Session.Remove(Constants.sessionDbManager);
            this.Context.Application.Remove(Constants.sessionSettings);

		}
		catch (Exception error)
		{
            Log.WriteException(error);
            AdminPanelUtils.SetPageErrorMessage(this.Page, Constants.mailAdmSaveUnsuccess);
		}
	}

	protected void test_connection_Click(object sender, System.EventArgs e)
	{
        Utils.SettingsPath = AdminPanelUtils.GetWebMailFolder();

        DbManager dbMan = null;
        string connectionString = string.Empty;
        SupportedDatabase dbType = SupportedDatabase.MsSqlServer;
        if (intDbTypeMsAccess.Checked)
        {
            dbMan = new MsAccessDbManager();
            dbType = SupportedDatabase.MsAccess;
        }
        else if (intDbTypeMySql.Checked)
        {
            dbMan = new MySqlDbManager();
            dbType = SupportedDatabase.MySql;
        }
        else
        {
            dbMan = new MsSqlDbManager();
            dbType = SupportedDatabase.MsSqlServer;
        }

        WebmailSettings settings = new WebMailSettingsCreator().CreateWebMailSettings(AdminPanelUtils.GetWebMailDataFolder());

        txtSqlPassword.Text = Request[txtSqlPassword.UniqueID] ?? settings.DbPassword;
        txtSqlLogin.Value = Request[txtSqlLogin.UniqueID] ?? settings.DbLogin;
        txtSqlName.Value = Request[txtSqlName.UniqueID] ?? settings.DbName;
        txtSqlDsn.Value = Request[txtSqlDsn.UniqueID] ?? settings.DbDsn;
        txtSqlSrc.Value = Request[txtSqlSrc.UniqueID] ?? settings.DbHost;
        txtAccessFile.Value = Request[txtAccessFile.UniqueID] ?? settings.DbPathToMdb;
        odbcConnectionString.Value = Request[odbcConnectionString.UniqueID] ?? settings.DbCustomConnectionString;
        useCS.Checked = (Request[useCS.UniqueID] != null) ? true : settings.UseCustomConnectionString;
        useDSN.Checked = (Request[useDSN.UniqueID] != null) ? true : settings.UseDSN;        
        
        Session["TrySqlPassword"] = txtSqlPassword.Text;
		txtSqlPassword.Attributes.Add("Value", txtSqlPassword.Text);

        string dsn = txtSqlDsn.Value;
        if (!useDSN.Checked) dsn = string.Empty;
        
        connectionString = DbManager.CreateConnectionString(useCS.Checked, odbcConnectionString.Value,
            dsn, dbType, txtAccessFile.Value, txtSqlLogin.Value,
			txtSqlPassword.Text, txtSqlName.Value, txtSqlSrc.Value);
		try
		{
			dbMan.Connect(connectionString);

            AdminPanelUtils.SetPageReportMessage(this.Page, Constants.mailAdmConnectSuccess);
		}
        catch (WebMailDatabaseException error)
		{
            Log.WriteException(error);
            AdminPanelUtils.SetPageErrorMessage(this.Page, Constants.mailAdmConnectUnsuccess);
		}
		finally
		{
			dbMan.Disconnect();
		}
	}

	protected void create_tables_Click(object sender, System.EventArgs e)
	{
        bool errorFlag = false;
        Save();

		Session["TrySqlPassword"] = txtSqlPassword.Text;
		txtSqlPassword.Attributes.Add("Value", txtSqlPassword.Text);

        WebmailSettings settings = new WebMailSettingsCreator().CreateWebMailSettings(AdminPanelUtils.GetWebMailDataFolder());
        
        string prefix = settings.DbPrefix;
		string[] dbTablesName = new string[] { };

		string[] tablesNames = new string[] 
            { 
                /* WebMail Tables */
                Constants.TablesNames.a_users, 
                Constants.TablesNames.awm_accounts, 
                Constants.TablesNames.awm_addr_book, 
                Constants.TablesNames.awm_addr_groups, 
                Constants.TablesNames.awm_addr_groups_contacts, 
                Constants.TablesNames.awm_columns, 
                Constants.TablesNames.awm_filters, 
                Constants.TablesNames.awm_folders, 
                Constants.TablesNames.awm_folders_tree, 
                Constants.TablesNames.awm_messages, 
                Constants.TablesNames.awm_messages_body, 
                Constants.TablesNames.awm_reads, 
                Constants.TablesNames.awm_senders, 
                Constants.TablesNames.awm_settings, 
                Constants.TablesNames.awm_domains, 
                Constants.TablesNames.awm_temp  
            };
		bool isTableExist = false;

        Utils.SettingsPath = AdminPanelUtils.GetWebMailFolder();

        DbStorage storage = DbStorageCreator.CreateDatabaseStorage(null, AdminPanelUtils.GetWebMailDataFolder());
		try
		{
			storage.Connect();
            dbTablesName = storage.GetTablesNames();
		}
        catch (WebMailDatabaseException error)
		{
            Log.WriteException(error);
            AdminPanelUtils.SetPageErrorMessage(this.Page, Constants.mailAdmTablesNotCreated);
            errorFlag = true;
		}
		finally
		{
			storage.Disconnect();
		}
		for (int i = 0; i < dbTablesName.Length; i++)
		{
			string dbTable = dbTablesName[i];
			foreach (string name in tablesNames)
			{
                if (string.Compare(dbTable, string.Format("{0}{1}", prefix, name), true, CultureInfo.InvariantCulture) == 0)
                {
                    isTableExist = true;
                    break;
                }
			}
            if (isTableExist) break;
		}
        if (isTableExist)
        {
            AdminPanelUtils.SetPageErrorMessage(this.Page, Constants.mailAdmTablesExists);
            errorFlag = true;
        }
        else
        {
            try
            {
                storage.Connect();
                foreach (string name in tablesNames)
                {
                    try
                    {
                        storage.CreateTable(name, prefix);
                    }
                    catch (WebMailDatabaseException error)
                    {
                        Log.WriteException(error);
                        AdminPanelUtils.SetPageErrorMessage(this.Page, Constants.mailAdmTablesNotCreated);
                        errorFlag = true;
                    }
                }
            }
            catch (WebMailDatabaseException error)
            {
                Log.WriteException(error);
                AdminPanelUtils.SetPageErrorMessage(this.Page, Constants.mailAdmTablesNotCreated);
                errorFlag = true;
            }
            finally
            {
                storage.Disconnect();
            }
            if (!errorFlag) AdminPanelUtils.SetPageReportMessage(this.Page, Constants.mailAdmTablesCreated);
        }
        InitControl();
    }

    protected void submit_btn_Click(object sender, EventArgs e)
    {
        Save();
        InitControl();
    }
}
