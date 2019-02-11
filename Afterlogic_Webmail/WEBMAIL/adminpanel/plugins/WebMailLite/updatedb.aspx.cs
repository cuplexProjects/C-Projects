using System;
using System.Collections;
using System.Text;
using System.Drawing;
using WebMail;

public partial class PlugIns_WebMailLite_updatedb : System.Web.UI.Page
{
    protected StringBuilder sb = new StringBuilder();
    protected int _errorCounter = 0;
    protected int _allOps = 0;
    protected SupportedDatabase _dbType;


    protected void Page_Load(object sender, System.EventArgs e)
    {
        if (Session["AUTH"] == null) return;

        Utils.SettingsPath = AdminPanelUtils.GetWebMailFolder();
        #region Convert Settings in settings.xml

        WebmailSettings wmsNew = new WebmailSettings().CreateInstance(AdminPanelUtils.GetWebMailDataFolder());

        sb.AppendFormat("<font color='black' sise='3' face='verdana'><b>{1}</b>. Start convert settings.xml</font><BR />", "settings.xml", _allOps + 1);
        _allOps++;

        try
        {
            wmsNew.SaveWebmailSettings(AdminPanelUtils.GetWebMailDataFolder());
            sb.AppendFormat("<font color='grey' sise='3' face='verdana'>- Convert settings successful.</font><BR /><BR />");
        }
        catch (Exception ex)
        {
            sb.AppendFormat("<font color='grey' sise='3' face='verdana'>- {0}</font><BR /><BR />", ex.Message);
            _errorCounter++;
        }
        # endregion


        WebmailSettings settings = new WebMailSettingsCreator().CreateWebMailSettings(AdminPanelUtils.GetWebMailDataFolder());
        DbStorage storage = DbStorageCreator.CreateDatabaseStorage(null, AdminPanelUtils.GetWebMailDataFolder());
        _dbType = settings.DbType;
        string[] tablesToCreate;
        string[] tablesToAlter = new string[] { };

        Hashtable alter_fields = new Hashtable();

        alter_fields.Add(Constants.TablesNames.a_users, new string[] { "id_user" });
        alter_fields.Add(Constants.TablesNames.awm_accounts, new string[] { "id_acct", "imap_quota", "mailing_list", "id_domain", "mail_protocol", "namespace" });
        alter_fields.Add(Constants.TablesNames.awm_folders, new string[] { "id_folder" });
        alter_fields.Add(Constants.TablesNames.awm_folders_tree, new string[] { "id" });
        alter_fields.Add(Constants.TablesNames.awm_filters, new string[] { "id_filter", "applied" });
        alter_fields.Add(Constants.TablesNames.awm_senders, new string[] { "id" });
        alter_fields.Add(Constants.TablesNames.awm_columns, new string[] { "id" });
        alter_fields.Add(Constants.TablesNames.awm_temp, new string[] { "id_temp" });
        alter_fields.Add(Constants.TablesNames.awm_messages, new string[] { "id", "flags", "sensitivity" });
        alter_fields.Add(Constants.TablesNames.awm_messages_body, new string[] { "id" });
        alter_fields.Add(Constants.TablesNames.awm_reads, new string[] { "id_read" });
        alter_fields.Add(Constants.TablesNames.awm_settings, new string[] { "id_setting", "auto_checkmail_interval" });
        alter_fields.Add(Constants.TablesNames.awm_domains, new string[] { "id_domain", "mail_protocol", "url", "site_name", "settings_mail_protocol", "settings_mail_inc_host", "settings_mail_inc_port", "settings_mail_out_host", "settings_mail_out_port", "settings_mail_out_auth", "allow_direct_mode", "direct_mode_id_def", "attachment_size_limit", "allow_attachment_limit", "mailbox_size_limit", "allow_mailbox_limit", "take_quota", "allow_new_users_change_settings", "allow_auto_reg_on_login", "allow_users_add_accounts", "allow_users_change_account_def", "def_user_charset", "allow_users_change_charset", "def_user_timezone", "allow_users_change_timezone", "msgs_per_page", "skin", "allow_users_change_skin", "lang", "allow_users_change_lang", "show_text_labels", "allow_ajax", "allow_editor", "allow_contacts", "allow_calendar", "hide_login_mode", "domain_to_use", "allow_choosing_lang", "allow_advanced_login", "allow_auto_detect_and_correct", "global_addr_book", "view_mode" });
        alter_fields.Add(Constants.TablesNames.awm_addr_book, new string[] { "id_addr", "use_frequency", "auto_create", "str_id", "deleted", "date_created", "date_modified" });
        alter_fields.Add(Constants.TablesNames.awm_addr_groups, new string[] { "id_group", "email", "company", "street", "city", "state", "zip", "country", "phone", "fax", "web", "organization", "use_frequency", "str_id" });

        try
        {
            tablesToCreate = new string[] 
                { 
                    Constants.TablesNames.awm_columns, 
                    Constants.TablesNames.awm_senders,
                    Constants.TablesNames.awm_domains
                };
            tablesToAlter = new string[] 
                { 
                        Constants.TablesNames.a_users,
                        Constants.TablesNames.awm_accounts,
                        Constants.TablesNames.awm_folders,
                        Constants.TablesNames.awm_folders_tree,
                        Constants.TablesNames.awm_filters,
                        Constants.TablesNames.awm_senders,
                        Constants.TablesNames.awm_columns,
                        Constants.TablesNames.awm_temp,
                        Constants.TablesNames.awm_messages,
                        Constants.TablesNames.awm_messages_body,
                        Constants.TablesNames.awm_reads,
                        Constants.TablesNames.awm_settings,
                        Constants.TablesNames.awm_domains,
                        Constants.TablesNames.awm_addr_book,
                        Constants.TablesNames.awm_addr_groups
                };

            Hashtable indices = new Hashtable();
            indices.Add(Constants.TablesNames.awm_reads, new string[] { settings.DbPrefix + "awm_reads", "_id_acct_index", "id_acct" });
            indices.Add(Constants.TablesNames.awm_columns + "_0", new string[] { settings.DbPrefix + "awm_columns", "_id_user_index", "id_user" });
            indices.Add(Constants.TablesNames.awm_columns + "_1", new string[] { settings.DbPrefix + "awm_columns", "_id_column_index", "id_column" });
            indices.Add(Constants.TablesNames.awm_messages + "_0", new string[] { settings.DbPrefix + "awm_messages", "_id_folder_srv_index", "id_folder_srv" });
            indices.Add(Constants.TablesNames.awm_messages + "_1", new string[] { settings.DbPrefix + "awm_messages", "_id_folder_db_index", "id_folder_db" });
            indices.Add(Constants.TablesNames.awm_settings, new string[] { settings.DbPrefix + "awm_settings", "_id_user_index", "id_user" });
            indices.Add(Constants.TablesNames.awm_senders, new string[] { settings.DbPrefix + "awm_senders", "_id_user_index", "id_user" });
            indices.Add(Constants.TablesNames.awm_accounts, new string[] { settings.DbPrefix + "awm_accounts", "_id_user_index", "id_user" });
            indices.Add(Constants.TablesNames.awm_addr_groups, new string[] { settings.DbPrefix + "awm_addr_groups", "_id_user_index", "id_user" });
            indices.Add(Constants.TablesNames.awm_addr_book, new string[] { settings.DbPrefix + "awm_addr_book", "_id_user_index", "id_user" });
            indices.Add(Constants.TablesNames.awm_folders + "_0", new string[] { settings.DbPrefix + "awm_folders", "_id_acct_index", "id_acct" });
            indices.Add(Constants.TablesNames.awm_folders + "_1", new string[] { settings.DbPrefix + "awm_folders", "_id_parent_index", "id_parent" });
            indices.Add(Constants.TablesNames.awm_folders_tree + "_0", new string[] { settings.DbPrefix + "awm_folders_tree", "_id_folder_index", "id_folder" });
            indices.Add(Constants.TablesNames.awm_folders_tree + "_1", new string[] { settings.DbPrefix + "awm_folders_tree", "_id_parent_index", "id_parent" });
            indices.Add(Constants.TablesNames.awm_filters + "_0", new string[] { settings.DbPrefix + "awm_filters", "_id_acct_index", "id_acct" });
            indices.Add(Constants.TablesNames.awm_filters + "_1", new string[] { settings.DbPrefix + "awm_filters", "_id_folder_index", "id_folder" });
            storage.Connect();

            if (tablesToCreate.Length > 0)
            {
                foreach (string tableName in tablesToCreate)
                {
                    sb.AppendFormat("<font color='black' sise='3' face='verdana'><b>{1}</b>. Start create <b>{0}</b> table:</font><BR />", tableName, _allOps + 1);
                    _allOps++;
                    try
                    {
                        storage.CreateTable(tableName, settings.DbPrefix);
                        sb.AppendFormat("<font color='#808080' sise='3' face='verdana'>- {0} create successful</font><BR /><BR />", tableName);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteException(ex);
                        sb.AppendFormat("<font color='#808080' sise='3' face='verdana'>- {0}</font><BR /><BR />", ex.InnerException.Message);
                        _errorCounter++;
                    }
                }
            }

            sb.AppendFormat(@"<font color='black' sise='3' face='verdana'><b>{0}</b>. Start update tables:</font><BR />", _allOps + 1);
            _allOps++;

            if (tablesToAlter.Length > 0)
            {
                foreach (string tableName in tablesToAlter)
                {
                    try
                    {
                        sb.AppendFormat("<BR /><font color='black' sise='3' face='verdana'>- Update <b>{0}</b>:</font><BR /><BR />", tableName);

                        string[] fields_array = (string[])alter_fields[tableName];
                        foreach (string fieldName in fields_array)
                        {
                            try
                            {
                                sb.AppendFormat("<font color='black' sise='3' face='verdana'>- Update Field {0}:</font>", fieldName);
                                storage.AlterTable(tableName, fieldName, settings.DbPrefix);
                                sb.AppendFormat("<font color='#808080' sise='3' face='verdana'> Done</font><BR />");
                            }
                            catch (Exception ex)
                            {
                                Log.WriteException(ex);
                                sb.AppendFormat("<font color='#808080' sise='3' face='verdana'> {0}</font><BR />", ex.InnerException.Message);
                                _errorCounter++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.WriteException(ex);
                        sb.AppendFormat("<font color='#808080' sise='3' face='verdana'>- {0}</font><BR /><BR />", ex.InnerException.Message);
                        _errorCounter++;
                    }
                }
            }
            else
            {
                sb.AppendFormat("<font color='#808080' sise='3' face='verdana'>- There are no tables for updating</font><BR /><BR />");
            }

            sb.AppendFormat(@"<font color='black' sise='3' face='verdana'><b>{0}</b>. Start create new index:</font><BR /><BR />", _allOps + 1);
            _allOps++;
            foreach (DictionaryEntry de in indices)
            {
                string[] aPSC = (string[])de.Value;
                try
                {
                    sb.AppendFormat("<font color='black' sise='3' face='verdana'>- Create <b>{0}</b> with params <b>{1}</b>, <b>{2}</b>, <b>{3}</b>.</font><br />", de.Key, aPSC[0], aPSC[1], aPSC[2]);
                    storage.CreateIndex(aPSC[0], aPSC[1], aPSC[2]);
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex);
                    sb.AppendFormat("<font color='#808080' sise='3' face='verdana'>- {0}</font><BR /><BR />", ex.InnerException.Message);
                    _errorCounter++;
                }
            }

            outputLabel.Text = String.Format("Update is done!", _errorCounter);
            outputLabel.ForeColor = Color.Black;
            outputLabel.Font.Bold = true;
        }


        catch (Exception ex)
        {
            Log.WriteException(ex);
            OutputUnsuccess(ex);
        }
        finally
        {
            storage.Disconnect();
        }
    }

    private void OutputUnsuccess(Exception ex)
    {
        outputLabel.Text = "Update unsuccessful! Error: " + ex.Message;
        outputLabel.ForeColor = Color.Red;
        outputLabel.Font.Bold = true;
    }

    #region Web Form Designer generated code
    override protected void OnInit(EventArgs e)
    {
        //
        // CODEGEN: This call is required by the ASP.NET Web Form Designer.
        //
        InitializeComponent();
        base.OnInit(e);
    }

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {

    }
    #endregion
}
