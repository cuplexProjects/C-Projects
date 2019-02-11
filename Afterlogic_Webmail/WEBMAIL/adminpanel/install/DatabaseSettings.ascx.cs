using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Globalization;
using System.Text.RegularExpressions;
using WebMail;

public partial class DatabaseSettings : System.Web.UI.UserControl
{
    protected string controlPrefix = string.Empty;
    protected string _errorMessage = string.Empty;
    protected string _errorMessageCreateDB = string.Empty;
    protected string _errorMessageConnection = string.Empty;

    protected int _web_step = 1;
    protected int _max_step = 7;

    protected void Page_Load(object sender, EventArgs e)
    {
        controlPrefix = this.ClientID;

        install _page = Page as install;

        _web_step = _page._web_step;
        _max_step = _page._max_step;

        intDbTypeMsSql_label.Attributes["for"] = intDbTypeMsSql.ClientID;
        intDbTypeMySql_label.Attributes["for"] = intDbTypeMySql.ClientID;
        intDbTypeMsAccess_label.Attributes["for"] = intDbTypeMsAccess.ClientID;

        WebmailSettings settings = new WebmailSettings().CreateInstance(AdminPanelUtils.GetWebMailDataFolder());
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
        DbPrefix.Value = settings.DbPrefix;
    }

    protected void SubmitButton_Click(object sender, System.EventArgs e)
    {
        if (Save())
        {
            if (chNotCreate.Checked)
            {
                if (!create_tables())
                {
                    Response.Redirect("install.aspx?mode=common", true);
                }
            }
            else
            {
                Response.Redirect("install.aspx?mode=common", true);
            }
        }
        else
        {
            WebmailSettings settings = new WebmailSettings().CreateInstance(AdminPanelUtils.GetWebMailDataFolder());
            DbPrefix.Value = settings.DbPrefix;
        }
    }

    private bool Save()
    {
        Session["TrySqlPassword"] = null;
        try
        {
            WebmailSettings settings = new WebmailSettings().CreateInstance(AdminPanelUtils.GetWebMailDataFolder());

    		Regex r = new Regex(@"[^a-zA-Z0-9_]");
            settings.DbPrefix = r.Replace(DbPrefix.Value, "_");

            bool res = true;
            if (settings.DbPrefix != DbPrefix.Value)
            {
                res = false;
            }
            
            if (intDbTypeMsAccess.Checked)
            {
                settings.DbType = SupportedDatabase.MsAccess;
                settings.DbPrefix = "";
            }
            else if (intDbTypeMySql.Checked)
            {
                settings.DbType = SupportedDatabase.MySql;
            }
            else
            {
                settings.DbType = SupportedDatabase.MsSqlServer;
            }
            settings.DbLogin = txtSqlLogin.Value;
            settings.DbPassword = txtSqlPassword.Text;
            settings.DbName = txtSqlName.Value;
            settings.DbDsn = txtSqlDsn.Value;
            settings.DbHost = txtSqlSrc.Value;
            settings.DbPathToMdb = txtAccessFile.Value;
            settings.DbCustomConnectionString = odbcConnectionString.Value;
            settings.UseCustomConnectionString = useCS.Checked;
            settings.UseDSN = useDSN.Checked;

            settings.SaveWebmailSettings(AdminPanelUtils.GetWebMailDataFolder());
            Session.Remove(Constants.sessionDbManager);
            Context.Application.Remove(Constants.sessionSettings);

            if (!res)
            {
                _errorMessage = @"<font color=""red"">Only letters, digits and underscore (""_"") allowed.</font>";
            }
            return res;
        }
        catch (Exception error)
        {
            Log.WriteException(error);
            _errorMessage = @"<font color=""red"">" + Constants.mailAdmSaveUnsuccess + ": " + error.Message + "</font>";
            return false;
        }
    }

    protected void test_connection_Click(object sender, System.EventArgs e)
    {
        Session["TrySqlPassword"] = txtSqlPassword.Text;
        txtSqlPassword.Attributes.Add("Value", txtSqlPassword.Text);

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

        string dsn = txtSqlDsn.Value;
        if (!useDSN.Checked)
        {
            dsn = string.Empty;
        }
        
        connectionString = DbManager.CreateConnectionString(useCS.Checked, odbcConnectionString.Value,
            dsn, dbType, txtAccessFile.Value, txtSqlLogin.Value,
            txtSqlPassword.Text, txtSqlName.Value, txtSqlSrc.Value);
        try
        {
            dbMan.Connect(connectionString);

            if (!intDbTypeMsAccess.Checked)
            {
                try
                {
                    dbMan.DropTable("a_test", DbPrefix.Value);
                }
                catch{ }

                dbMan.CreateTable("a_test", DbPrefix.Value);
                dbMan.DropTable("a_test", DbPrefix.Value);
            }

            _errorMessageConnection = @"<font color=""green"">" + Constants.mailAdmConnectSuccess + "</font>";
        }
        catch (WebMailDatabaseException error)
        {
            Log.WriteException(error);
            _errorMessageConnection = @"<font color=""red"">" + Constants.mailAdmConnectUnsuccess + ": " + error.Message + "</font>";
        }
        finally
        {
            dbMan.Disconnect();
        }
    }

    protected void create_database_Click(object sender, System.EventArgs e)
    {
        Session["TrySqlPassword"] = txtSqlPassword.Text;
        txtSqlPassword.Attributes.Add("Value", txtSqlPassword.Text);

        if (intDbTypeMsAccess.Checked)
        {
            return;
        }

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

        string dsn = txtSqlDsn.Value;
        if (!useDSN.Checked)
        {
            dsn = string.Empty;
        }

        connectionString = DbManager.CreateConnectionString(useCS.Checked, odbcConnectionString.Value,
            dsn, dbType, txtAccessFile.Value, txtSqlLogin.Value,
            txtSqlPassword.Text, null, txtSqlSrc.Value);
        try
        {
            dbMan.Connect(connectionString);
            dbMan.CreateDatabase(txtSqlName.Value);
            _errorMessageCreateDB = @"<br /><font color=""green"">Database create successfily.</font>";
        }
        catch (WebMailDatabaseException error)
        {
            Log.WriteException(error);
            _errorMessageCreateDB = @"<br /><font color=""red"">" + error.Message + "</font>";
        }
        finally
        {
            dbMan.Disconnect();
        }        
    }

    protected bool create_tables()
    {
        bool errorFlag = false;
        WebmailSettings settings = new WebmailSettings().CreateInstance(AdminPanelUtils.GetWebMailDataFolder());

        if (settings.DbType == SupportedDatabase.MsAccess)
        {
            return false;
        }

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
            Constants.TablesNames.awm_temp,
            Constants.TablesNames.awm_subadmin_domains,
            Constants.TablesNames.awm_subadmins,
            /* Calendar Tables */
            Constants.TablesNames.acal_calendars,
            Constants.TablesNames.acal_events,
            Constants.TablesNames.acal_users_data,
            Constants.TablesNames.acal_publications,
            Constants.TablesNames.acal_sharing,
            Constants.TablesNames.acal_eventrepeats,
            Constants.TablesNames.acal_exclusions,
            Constants.TablesNames.acal_appointments,
            Constants.TablesNames.acal_cron_runs,
            Constants.TablesNames.acal_reminders
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
            _errorMessage = @"<font color=""red"">" + Constants.mailAdmTablesNotCreated + ": " + error.Message + "</font>";
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
            if (isTableExist)
            {
                break;
            }
        }
        if (isTableExist)
        {
            _errorMessage = @"<font color=""red"">The data tables with """ + prefix + @""" prefix already exist. To proceed, specify another prefix or delete the existing tables.</font>";
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
                        _errorMessage = @"<font color=""red"">" + Constants.mailAdmTablesNotCreated + ": " + error.Message + "</font>";
                        errorFlag = true;
                    }
                }
            }
            catch (WebMailDatabaseException error)
            {
                Log.WriteException(error);
                _errorMessage = @"<font color=""red"">" + Constants.mailAdmTablesNotCreated + ": " + error.Message + "</font>";
                errorFlag = true;
            }
            finally
            {
                storage.Disconnect();
            }
            if (!errorFlag)
            {
                _errorMessage = @"<font color=""green"">" + Constants.mailAdmTablesCreated + "</font>";
            }
        }
        return errorFlag;
    }

}
