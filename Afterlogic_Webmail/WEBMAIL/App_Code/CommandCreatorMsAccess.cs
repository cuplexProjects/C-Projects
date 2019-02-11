using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Text;
using MailBee.ImapMail;

namespace WebMail
{
	public class MsAccessCommandCreator : CommandCreator
	{
		public MsAccessCommandCreator(OleDbConnection connection, OleDbCommand command)
			: base(connection, command) {}

		public MsAccessCommandCreator(OleDbConnection connection, OleDbCommand command, string dataFolder)
			: base(connection, command, dataFolder) {}

		protected override IDataParameter CreateParameter(string paramName, object paramValue)
		{
			Log.WriteLine("CreateParameter", string.Format("{0}='{1}'", paramName, paramValue));
			if (paramValue.GetType() == typeof(DateTime))
			{
				paramValue = ((DateTime)paramValue).ToString();
			}
			return new OleDbParameter(paramName, paramValue);
		}

		public override string SelectIdentity()
		{
			return @"SELECT @@IDENTITY;";
		}

        public override IDbCommand CreateTable(string name, string prefix)
        {
        	prefix = EncodeQuotes(prefix);

            string commandText = string.Empty;
            switch (name)
            {
                case Constants.TablesNames.awm_subadmins:
                    commandText = string.Format(@"
CREATE TABLE [{0}awm_subadmins] (
	[id_admin] AUTOINCREMENT,
	[login] varchar (255) NULL,
	[password] varchar (255) NULL,
	[description] varchar (255) NULL,
PRIMARY KEY  ([id_admin]))", prefix); 
                    break;
                case Constants.TablesNames.awm_subadmin_domains:
                    commandText = string.Format(@"
CREATE TABLE [{0}awm_subadmin_domains] (
	[id] AUTOINCREMENT,
	[id_admin] int NOT NULL default 0,
	[id_domain] int NOT NULL default 0,
PRIMARY KEY  ([id]))", prefix);
                    break;
#region CreateCalendarTablesCommands
                case Constants.TablesNames.acal_calendars:
                    commandText = string.Format(@"
CREATE TABLE [{0}{1}] (
    [calendar_id] AUTOINCREMENT,
    [user_id] int NOT NULL default 0,
    [calendar_name] varchar (100) NOT NULL,
    [calendar_description] varchar default NULL,
    [calendar_color] int NOT NULL default 0,
    [calendar_active] tinyint NOT NULL default 0,
    [calendar_str_id] varchar (100) NOT NULL,
PRIMARY KEY  ([calendar_id]))", prefix, Constants.TablesNames.acal_calendars);

                    break;
                case Constants.TablesNames.acal_events:
                    commandText = string.Format(@"
CREATE TABLE [{0}{1}] (
    [event_id] AUTOINCREMENT,
    [calendar_id] int NOT NULL default 0,
    [event_timefrom] datetime default NULL,
    [event_timetill] datetime default NULL, 
    [event_allday] tinyint NOT NULL default 0,
    [event_name] varchar (100) NOT NULL,
    [event_text] varchar default NULL,
    [event_priority] tinyint NULL DEFAULT 0,
    [event_str_id] varchar (200) NOT NULL,
    [event_last_modified] datetime DEFAULT NULL, 
PRIMARY KEY  ([event_id]))", prefix, Constants.TablesNames.acal_events);
                    break;
                case Constants.TablesNames.acal_users_data:
                    commandText = string.Format(@"
CREATE TABLE [{0}{1}] (
    [settings_id] AUTOINCREMENT,
    [user_id] int NOT NULL default 0,
    [timeformat] tinyint NOT NULL default 1,
    [dateformat] tinyint NOT NULL default 1,
    [showweekends] tinyint NOT NULL default 0,
    [workdaystarts] tinyint NOT NULL default 0,
    [workdayends] tinyint NOT NULL default 1,
    [showworkday] tinyint NOT NULL default 0,
    [weekstartson] tinyint NOT NULL default 0,
    [defaulttab] tinyint NOT NULL default 1,
    [country] varchar (2) default NULL,
    [timezone] smallint NULL,
    [alltimezones] tinyint NOT NULL default 0,
	[reminders_web_url] varchar (255) NULL,
	[autoaddinvitation] tinyint NOT NULL DEFAULT 0,
PRIMARY KEY ([settings_id]))", prefix, Constants.TablesNames.acal_users_data);
                    break;
                case Constants.TablesNames.acal_sharing:
                    commandText = string.Format(@"
CREATE TABLE [{0}acal_sharing] (
     [id_share] AUTOINCREMENT,
     [id_user] int NOT NULL,
     [id_calendar] int NOT NULL,
     [id_to_user] int NOT NULL,
     [str_to_email] varchar (255) NOT NULL DEFAULT '',
     [int_access_level] tinyint NOT NULL DEFAULT 2,
     [calendar_active] tinyint NOT NULL DEFAULT 1,
PRIMARY KEY ([id_share]))", prefix);
                    break;
                case Constants.TablesNames.acal_publications:
                    commandText = string.Format(@"
CREATE TABLE [{0}acal_publications] (
     [id_publication] AUTOINCREMENT,
     [id_user] int NOT NULL,
     [id_calendar] int NOT NULL,
     [str_md5] varchar (32) NOT NULL,
     [int_access_level] tinyint NOT NULL DEFAULT 1,
     [access_type] smallint NOT NULL DEFAULT 0,
PRIMARY KEY ([id_publication]))", prefix);
                    break;
                case Constants.TablesNames.acal_eventrepeats:
                    commandText = string.Format(@"
CREATE TABLE  [{0}acal_eventrepeats] (
  [id_repeat] AUTOINCREMENT,
  [id_event] int NOT NULL,
  [repeat_period] tinyint NOT NULL default 0,
  [repeat_order] tinyint NOT NULL default 1,
  [repeat_num] int NOT NULL default 0,
  [repeat_until] datetime default NULL,
  [week_number] tinyint default NULL,
  [repeat_end] tinyint NOT NULL default 0,
  [excluded] tinyint NOT NULL default 0,
  [sun] tinyint NOT NULL default 0,
  [mon] tinyint NOT NULL default 0,
  [tue] tinyint NOT NULL default 0,
  [wed] tinyint NOT NULL default 0,
  [thu] tinyint NOT NULL default 0,
  [fri] tinyint NOT NULL default 0,
  [sat] tinyint NOT NULL default 0,
PRIMARY KEY ([id_repeat]))", prefix);
                    break;
                case Constants.TablesNames.acal_exclusions:
                    commandText = string.Format(@"
CREATE TABLE  [{0}{1}] (
  [id_exclusion] AUTOINCREMENT,
  [id_event] int NOT NULL default 0,
  [id_calendar] int NOT NULL default 0,
  [id_repeat] int NOT NULL default 0,
  [event_timefrom] datetime NOT NULL,
  [event_timetill] datetime NOT NULL,
  [event_name] varchar(100) NOT NULL,
  [event_text] text,
  [event_allday] tinyint NOT NULL default 0,
  [is_deleted] tinyint NOT NULL default 0,
  [id_recurrence_date] datetime DEFAULT NULL,
  [event_last_modified] datetime DEFAULT NULL,
  PRIMARY KEY  ([id_exclusion]))", prefix, Constants.TablesNames.acal_exclusions);
                    break;
                case Constants.TablesNames.acal_reminders:
                    commandText = string.Format(@"
CREATE TABLE [{0}acal_reminders] (
	[id_reminder] AUTOINCREMENT,
	[id_event] int NOT NULL,
	[id_user] int NULL,
	[notice_type] tinyint NOT NULL default 0,
	[remind_offset] int NOT NULL default 0,
	[sent] int NOT NULL default 0,
  PRIMARY KEY  ([id_reminder]))", prefix);
                    break;
                case Constants.TablesNames.acal_cron_runs:
                    commandText = string.Format(@"
CREATE TABLE [{0}acal_cron_runs] (
	[id_run] AUTOINCREMENT,
	[run_date] datetime NOT NULL,
	[latest_date] datetime NOT NULL,
  PRIMARY KEY  ([id_run]))", prefix);
                    break;
                case Constants.TablesNames.acal_appointments:
                    commandText = string.Format(@"
CREATE TABLE [{0}acal_appointments] (
	[id_appointment] AUTOINCREMENT,
	[id_event] int NOT NULL,
	[id_user] int NOT NULL default 0,
	[email] varchar (255) NOT NULL,
	[access_type] tinyint NOT NULL default 0,
	[status] tinyint NOT NULL default 0,
	[hash] varchar (32) NOT NULL,
  PRIMARY KEY  ([id_appointment]))", prefix);
                    break;

                #endregion
            }

            return PrepareCommand(commandText, null);
        }

        public override IDbCommand AlterTable(string tableName, string field, string tablePrefix)
        {
            string commandText = string.Empty;
            switch (tableName)
            {
                case Constants.TablesNames.awm_addr_book:
                    switch (field)
                    {
                        case "use_frequency":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}awm_addr_book] 
                            ADD use_frequency INT DEFAULT 0 NOT NULL", tablePrefix);
                            break;
                        case "auto_create":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}awm_addr_book] 
                            ADD auto_create BIT DEFAULT 0 NOT NULL", tablePrefix);
                            break;
                        // 4.9.x
                        case "str_id":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}awm_addr_book] 
                            ADD str_id VARCHAR(100) NULL", tablePrefix);
                            break;
                        case "deleted":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}awm_addr_book] 
                            ADD deleted bit NOT NULL DEFAULT 0", tablePrefix);
                            break;
                        case "date_created":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}awm_addr_book] 
                            ADD date_created datetime NULL", tablePrefix);
                            break;
                        case "date_modified":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}awm_addr_book] 
                            ADD date_modified datetime NULL", tablePrefix);
                            break;
                    }
                    break;
                case Constants.TablesNames.awm_addr_groups:
                    switch (field)
                    {
                        case "email":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}awm_addr_groups] 
                            ADD email VARCHAR(255) DEFAULT '' NOT NULL", tablePrefix);
                            break;
                        case "company":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}awm_addr_groups] 
                            ADD company VARCHAR(200) DEFAULT '' NOT NULL", tablePrefix);
                            break;
                        case "street":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}awm_addr_groups] 
                            ADD street VARCHAR(255) DEFAULT '' NOT NULL", tablePrefix);
                            break;
                        case "city":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}awm_addr_groups] 
                            ADD city VARCHAR(200) DEFAULT '' NOT NULL", tablePrefix);
                            break;
                        case "state":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}awm_addr_groups] 
                            ADD state VARCHAR(200) DEFAULT '' NOT NULL", tablePrefix);
                            break;
                        case "zip":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}awm_addr_groups] 
                            ADD zip VARCHAR(10) DEFAULT '' NOT NULL", tablePrefix);
                            break;
                        case "country":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}awm_addr_groups] 
                            ADD country VARCHAR(200) DEFAULT '' NOT NULL", tablePrefix);
                            break;
                        case "phone":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}awm_addr_groups] 
                            ADD phone VARCHAR(50) DEFAULT '' NOT NULL", tablePrefix);
                            break;
                        case "fax":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}awm_addr_groups] 
                            ADD fax VARCHAR(50) DEFAULT '' NOT NULL", tablePrefix);
                            break;
                        case "web":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}awm_addr_groups] 
                            ADD web VARCHAR(255) DEFAULT '' NOT NULL", tablePrefix);
                            break;
                        case "organization":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}awm_addr_groups] 
                            ADD organization BIT DEFAULT 0 NOT NULL", tablePrefix);
                            break;
                        case "use_frequency":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}awm_addr_groups] 
                            ADD use_frequency INT DEFAULT 0 NOT NULL", tablePrefix);
                            break;
                        // 4.9.x
                        case "str_id":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}awm_addr_groups] 
                            ADD str_id  VARCHAR(100) NULL", tablePrefix);
                            break;
                    }
                    break;
                case Constants.TablesNames.awm_accounts:
                    switch (field)
                    {
                        case "imap_quota":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}awm_accounts] 
                            ADD imap_quota smallint DEFAULT -1 NOT NULL", tablePrefix);
                            break;
                        case "mailing_list":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}awm_accounts] 
                            ADD mailing_list BIT DEFAULT 0 NOT NULL", tablePrefix);
                            break;
                        case "id_domain":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}awm_accounts] 
                            ADD id_domain INT DEFAULT 0 NOT NULL", tablePrefix);
                            break;
                        case "namespace":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}awm_accounts] 
                            ADD namespace varchar(50) NOT NULL DEFAULT ''", tablePrefix);
                            break;
                    }
                    break;
                case Constants.TablesNames.awm_domains:
                    switch (field)
                    {
                        case "mail_protocol":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}awm_domains] 
                            ALTER COLUMN mail_protocol SMALLINT", tablePrefix);
                            break;
                        case "url":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}awm_domains]
                            ADD url VARCHAR(255) NULL", tablePrefix);
                            break;
                        case "site_name":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}awm_domains]
                            ADD site_name VARCHAR(255) NULL", tablePrefix);
                            break;
                        case "settings_mail_protocol":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD settings_mail_protocol INT NULL", tablePrefix);
                            break;
                        case "settings_mail_inc_host":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD settings_mail_inc_host VARCHAR(255) NULL", tablePrefix);
                            break;
                        case "settings_mail_inc_port":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD settings_mail_inc_port INT NULL", tablePrefix);
                            break;
                        case "settings_mail_out_host":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD settings_mail_out_host VARCHAR(255) NULL", tablePrefix);
                            break;
                        case "settings_mail_out_port":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD settings_mail_out_port INT NULL", tablePrefix);
                            break;
                        case "settings_mail_out_auth":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD settings_mail_out_auth BIT NULL", tablePrefix);
                            break;
                        case "allow_direct_mode":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD allow_direct_mode BIT NULL", tablePrefix);
                            break;
                        case "direct_mode_id_def":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD direct_mode_id_def BIT NULL", tablePrefix);
                            break;
                        case "attachment_size_limit":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD attachment_size_limit LONG NULL", tablePrefix);
                            break;
                        case "allow_attachment_limit":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD allow_attachment_limit BIT NULL", tablePrefix);
                            break;
                        case "mailbox_size_limit":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD mailbox_size_limit LONG NULL", tablePrefix);
                            break;
                        case "allow_mailbox_limit":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD allow_mailbox_limit BIT NULL", tablePrefix);
                            break;
                        case "take_quota":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD take_quota BIT NULL", tablePrefix);
                            break;
                        case "allow_new_users_change_settings":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD allow_new_users_change_settings BIT NULL", tablePrefix);
                            break;
                        case "allow_auto_reg_on_login":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD allow_auto_reg_on_login BIT NULL", tablePrefix);
                            break;
                        case "allow_users_add_accounts":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD allow_users_add_accounts BIT NULL", tablePrefix);
                            break;
                        case "allow_users_change_account_def":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD allow_users_change_account_def BIT NULL", tablePrefix);
                            break;
                        case "def_user_charset":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD def_user_charset INT NULL", tablePrefix);
                            break;
                        case "allow_users_change_charset":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD allow_users_change_charset BIT NULL", tablePrefix);
                            break;
                        case "def_user_timezone":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD def_user_timezone INT NULL", tablePrefix);
                            break;
                        case "allow_users_change_timezone":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD allow_users_change_timezone BIT NULL", tablePrefix);
                            break;
                        case "msgs_per_page":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD msgs_per_page SMALLINT NULL", tablePrefix);
                            break;
                        case "skin":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD skin VARCHAR(50) NULL", tablePrefix);
                            break;
                        case "allow_users_change_skin":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD allow_users_change_skin BIT NULL", tablePrefix);
                            break;
                        case "lang":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD lang VARCHAR(50) NULL", tablePrefix);
                            break;
                        case "allow_users_change_lang":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD allow_users_change_lang BIT NULL", tablePrefix);
                            break;
                        case "show_text_labels":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD show_text_labels BIT NULL", tablePrefix);
                            break;
                        case "allow_ajax":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD allow_ajax BIT NULL", tablePrefix);
                            break;
                        case "allow_editor":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD allow_editor BIT NULL", tablePrefix);
                            break;
                        case "allow_contacts":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD allow_contacts BIT NULL", tablePrefix);
                            break;
                        case "allow_calendar":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD allow_calendar BIT NULL", tablePrefix);
                            break;
                        case "hide_login_mode":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD hide_login_mode INT NULL", tablePrefix);
                            break;
                        case "domain_to_use":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD domain_to_use VARCHAR(255) NULL", tablePrefix);
                            break;
                        case "allow_choosing_lang":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD allow_choosing_lang BIT NULL", tablePrefix);
                            break;
                        case "allow_advanced_login":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD allow_advanced_login BIT NULL", tablePrefix);
                            break;
                        case "allow_auto_detect_and_correct":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD allow_auto_detect_and_correct BIT NULL", tablePrefix);
                            break;
                        case "global_addr_book":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD global_addr_book BIT DEFAULT 0 NOT NULL", tablePrefix);
                            break;
                        case "view_mode":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD view_mode tinyint DEFAULT 1 NOT NULL", tablePrefix);
                            break;
                        case "save_mail":
                            commandText = string.Format(@"
							ALTER TABLE [{0}awm_domains]
							ADD save_mail tinyint DEFAULT 0 NOT NULL", tablePrefix);
                            break;
                    }
                    break;
                case Constants.TablesNames.acal_calendars:
                    switch (field)
                    {
                        case "calendar_description":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}acal_calendars] 
                            ALTER COLUMN calendar_description text NULL", tablePrefix);
                            break;
                        case "calendar_str_id":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}acal_calendars] 
                            ADD calendar_str_id varchar(100) NOT NULL", tablePrefix);
                            break;
                    }
                    break;
                case Constants.TablesNames.acal_sharing:
                    switch (field)
                    {
                        case "calendar_active":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}acal_sharing] 
                            ADD calendar_active bit NOT NULL DEFAULT (1)", tablePrefix);
                            break;
                    }
                    break;
                case Constants.TablesNames.acal_events:
                    switch (field)
                    {
                        case "event_repeats":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}acal_events]
                            ADD event_repeats TINYINT DEFAULT 0 NOT NULL", tablePrefix);
                            break;
                        case "event_text":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}acal_events]
                            ALTER COLUMN event_text text NULL", tablePrefix);
                            break;
                        case "event_str_id":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}acal_events]
                            ADD event_str_id varchar(200) NOT NULL", tablePrefix);
                            break;
                        case "event_last_modified":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}acal_events]
                            ADD event_last_modified datetime DEFAULT NULL", tablePrefix);
                            break;
                        // 4.9.x
                        case "event_owner_email":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}acal_events] 
                            ADD event_owner_email VARCHAR(255) NOT NULL DEFAULT ''", tablePrefix);
                            break;
                        case "event_appointment_access":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}acal_events] 
                            ADD event_appointment_access TINYINT DEFAULT 0 NOT NULL", tablePrefix);
                            break;
                    }
                    break;
                case Constants.TablesNames.acal_publications:
                    switch (field)
                    {
                        case "access_type":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}acal_publications]
                            ADD access_type TINYINT DEFAULT 0 NOT NULL", tablePrefix);
                            break;
                    }
                    break;
                case Constants.TablesNames.awm_messages:
                    switch (field)
                    {
                        case "flags":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}awm_messages]
                            ALTER COLUMN flags int", tablePrefix);
                            break;
                        // 4.9.x
                        case "sensitivity":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}awm_messages]
                            ADD sensitivity TINYINT DEFAULT 0 NOT NULL", tablePrefix);
                            break;
                    }
                    break;
                case Constants.TablesNames.acal_exclusions:
                    switch (field)
                    {
                        case "id_recurrence_date":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}acal_exclusions]
                            ADD id_recurrence_date datetime DEFAULT NULL", tablePrefix);
                            break;
                        case "event_last_modified":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}acal_exclusions]
                            ADD event_last_modified datetime DEFAULT NULL", tablePrefix);
                            break;
                    }
                    break;
                // 4.9.x
                case Constants.TablesNames.acal_reminders:
                    switch (field)
                    {
                        case "id_user":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}acal_reminders]
                            ADD id_user INT NULL", tablePrefix);
                            break;
                    }
                    break;
                // 4.9.x
                case Constants.TablesNames.acal_users_data:
                    switch (field)
                    {
                        case "autoaddinvitation":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}acal_users_data]
                            ADD autoaddinvitation TINYINT DEFAULT 0 NOT NULL", tablePrefix);
                            break;
                        case "reminders_web_url":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}acal_users_data]
                            ADD reminders_web_url VARCHAR(255) NULL", tablePrefix);
                            break;
                    }
                    break;
                // 4.9.x
                case Constants.TablesNames.awm_filters:
                    switch (field)
                    {
                        case "applied":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}awm_filters]
                            ADD applied TINYINT DEFAULT 1 NOT NULL", tablePrefix);
                            break;
                    }
                    break;
                case Constants.TablesNames.awm_settings:
                    switch (field)
                    {
                        case "auto_checkmail_interval":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}awm_settings]
                            ADD auto_checkmail_interval INT DEFAULT 0 NOT NULL", tablePrefix);
                            break;
                    }
                    break;
            }

            return PrepareCommand(commandText, null);
        }

        public override IDbCommand SelectAwmAccountsForAdmin(int page, int pageSize, string orderBy, bool asc, string searchCondition)
        {
            return SelectAwmAccountsForAdmin(page, pageSize, orderBy, asc, searchCondition, 0);
        }

        public override IDbCommand SelectAwmAccountsForAdmin(int page, int pageSize, string orderBy, bool asc, string searchCondition, int id_domain)
        {
            if (page > 1)
            {
                return base.SelectAwmAccountsForAdmin(page, pageSize, orderBy, asc, searchCondition, id_domain);
            }
            string whereCondition;
            if (!string.IsNullOrEmpty(searchCondition))
            {
                whereCondition = string.Format(@"WHERE [email] LIKE ('%{0}%') AND id_domain={1}", EncodeQuotes(searchCondition), id_domain);
            }
            else
            {
                whereCondition = string.Format(@"WHERE [id_domain] = {0}", id_domain);
            }

            string commandText = string.Format(@"SELECT TOP {2} [{0}].[id_user], [id_acct], [email], [last_login], [logins_count], [mailbox_size], [mailbox_limit], [mailing_list], [imap_quota], [deleted] FROM [{0}]
INNER JOIN [{1}] ON [{0}].[id_user] = [{1}].[id_user] {5}
ORDER BY [{3}] {4};",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_accounts),
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_settings),
                pageSize,
                orderBy,
                (asc) ? "ASC" : "DESC",
                whereCondition);

            return PrepareCommand(commandText, null);
        }

        public override IDbCommand SelectAwmSubadmins(int page, int pageSize, string orderBy, bool asc, string searchCondition)
        {
            if (page > 1)
            {
                return base.SelectAwmSubadmins(page, pageSize, orderBy, asc, searchCondition);
            }

            string whereConditionInternal = string.Empty;
            if (!string.IsNullOrEmpty(searchCondition))
            {
                whereConditionInternal = string.Format(@"WHERE login LIKE ('{0}')", "%" + EscapeWildcardCharacters(EncodeQuotes(searchCondition)) + "%");
            }

            string commandText = string.Format(@"SELECT TOP {1} id_admin, login, password, description FROM {0}
{4} ORDER BY {2} {3}",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_subadmins),
                pageSize,
                orderBy,
                (asc) ? "ASC" : "DESC",
                whereConditionInternal);
            return PrepareCommand(commandText, null);
        }

		public override IDbCommand SelectAwmMessages(int id_acct, long id_folder_db, int pageNumber, int msgsOnPage, string order, bool asc)
		{
			if (pageNumber > 1)
			{
				return base.SelectAwmMessages (id_acct, id_folder_db, pageNumber, msgsOnPage, order, asc);
			}

			string commandText = string.Format(@"SELECT TOP {1} * FROM {0}
WHERE [id_acct]=@id_acct AND [id_folder_db]=@id_folder_db 
ORDER BY {2} {3};",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
				msgsOnPage, // 1
				order,
				(asc) ? "ASC" : "DESC");

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder_db", id_folder_db));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand InsertIntoAwmColumns(int id_column, int id_user, int value) {
			string commandText = string.Format(@"INSERT INTO {0} (id_user, id_column, column_value)
VALUES (?, ?, ?);",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_columns));


			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));
			parameters.Add(CreateParameter("@id_column", id_column));
			parameters.Add(CreateParameter("@value", value));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectAwmMessages(int id_acct, int pageNumber, int msgsOnPage, string condition, FolderCollection folders, bool inHeadersOnly, string order, bool asc)
		{
			if (pageNumber > 1)
			{
				return base.SelectAwmMessages (id_acct, pageNumber, msgsOnPage, condition, folders, inHeadersOnly, order, asc);
			}
			StringBuilder folder_ids = new StringBuilder();
			if (folders != null)
			{
				foreach (Folder fld in folders)
				{
					folder_ids.AppendFormat("{0},", fld.ID);
				}
				if (folder_ids.Length > 0)
				{
					folder_ids.Remove(folder_ids.Length - 1, 1);
				}
			}
			string bodyLike = string.Empty;
			if (!inHeadersOnly)
			{
				bodyLike = string.Format(" OR [body_text] LIKE '%{0}%'", condition);
			}

			string commandText = string.Format(@"SELECT TOP {1} * FROM [{0}] INNER JOIN [{6}] ON [{0}].id_folder_db = [{6}].id_folder 
WHERE [{0}].[id_acct]=@id_acct AND [id_folder_db] IN ({4}) AND 
([from_msg] LIKE '%{5}%' OR [to_msg] LIKE '%{5}%' OR [cc_msg] LIKE '%{5}%' OR [bcc_msg]
LIKE '%{5}%' OR [subject] LIKE '%{5}%'{7})
ORDER BY {2} {3};",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
				msgsOnPage,
				order,
				(asc) ? "ASC" : "DESC",
				folder_ids,
				EncodeQuotes(condition),
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders),
				bodyLike);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand DeleteFromAwmFolders(int id_acct)
		{
			string commandText = string.Format(@"DELETE FROM {0} WHERE [id_acct]=@id_acct;", 
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand DeleteFromAwmFolders(int id_acct, long id_folder)
		{
			string commandText = string.Format(@"
DELETE FROM {0} WHERE [id_folder]=@id_folder AND [id_acct]=@id_acct
",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders));


			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_folder", id_folder));
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand InsertIntoAUsers(bool deleted)
		{
			string commandText = string.Format(@"INSERT INTO {0} (deleted) VALUES(@deleted);",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.a_users));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@deleted", deleted));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand InsertIntoAwmFilters(int id_acct, byte field, byte condition, string filter, 
			byte action, long id_folder, bool applied)
		{
			string commandText = string.Format(@"
INSERT INTO {0} (id_acct, [field], [condition], [filter], [action], id_folder, applied)
VALUES (@id_acct, @field, @condition, @filter, @action, @id_folder, @applied)",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_filters));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@field", field));
			parameters.Add(CreateParameter("@condition", condition));
			parameters.Add(CreateParameter("@filter", (filter.Length > 255) ? filter.Substring(0, 255) : filter));
			parameters.Add(CreateParameter("@action", action));
			parameters.Add(CreateParameter("@id_folder", id_folder));
			parameters.Add(CreateParameter("@applied", applied));

			return PrepareCommand(commandText, parameters);
		}

        public override IDbCommand InsertIntoAwmAccounts(int id_user, bool def_acct, bool deleted, string email, IncomingMailProtocol mail_protocol, string mail_inc_host, string mail_inc_login, string mail_inc_pass, int mail_inc_port, string mail_out_host, string mail_out_login, string mail_out_pass, int mail_out_port, bool mail_out_auth, string friendly_nm, bool use_friendly_nm, DefaultOrder def_order, bool getmail_at_login, MailMode mail_mode, short mails_on_server_days, string signature, SignatureType signature_type, SignatureOptions signature_opt, string delimiter, long mailbox_size, string Namespace)
        {
            return InsertIntoAwmAccounts(id_user, def_acct, deleted, email, mail_protocol, mail_inc_host, mail_inc_login, mail_inc_pass, mail_inc_port, mail_out_host, mail_out_login, mail_out_pass, mail_out_port, mail_out_auth, friendly_nm, use_friendly_nm, def_order, getmail_at_login, mail_mode, mails_on_server_days, signature, signature_type, signature_opt, delimiter, mailbox_size, 0, false, -1, Namespace);
        }

        public override IDbCommand InsertIntoAwmAccounts(int id_user, bool def_acct, bool deleted, string email, IncomingMailProtocol mail_protocol, string mail_inc_host, string mail_inc_login, string mail_inc_pass, int mail_inc_port, string mail_out_host, string mail_out_login, string mail_out_pass, int mail_out_port, bool mail_out_auth, string friendly_nm, bool use_friendly_nm, DefaultOrder def_order, bool getmail_at_login, MailMode mail_mode, short mails_on_server_days, string signature, SignatureType signature_type, SignatureOptions signature_opt, string delimiter, long mailbox_size, int id_domain, bool mailing_list, int imap_quota, string Namespace)
        {
            string commandText = string.Format(@"INSERT INTO {0}
(id_user, def_acct, deleted, email, mail_protocol, mail_inc_host, mail_inc_login, mail_inc_pass,
 mail_inc_port, mail_out_host, mail_out_login, mail_out_pass, mail_out_port, mail_out_auth, friendly_nm,
 use_friendly_nm, def_order, getmail_at_login, mail_mode, mails_on_server_days, signature, signature_type,
 signature_opt, delimiter, mailbox_size, id_domain, mailing_list, imap_quota, namespace)
VALUES
(@id_user, @def_acct, @deleted, @email, @mail_protocol, @mail_inc_host, @mail_inc_login, @mail_inc_pass, 
@mail_inc_port, @mail_out_host, @mail_out_login, @mail_out_pass, @mail_out_port, @mail_out_auth, @friendly_nm,
@use_friendly_nm, @def_order, @getmail_at_login, @mail_mode, @mails_on_server_days, @signature, @signature_type,
@signature_opt, @delimiter, @mailbox_size, @id_domain, @mailing_list, @imap_quota, @namespace);",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_accounts));

            ArrayList parameters = new ArrayList();
            parameters.Add(CreateParameter("@id_user", id_user));
            parameters.Add(CreateParameter("@def_acct", def_acct));
            parameters.Add(CreateParameter("@deleted", deleted));
            parameters.Add(CreateParameter("@email", (email.Length > 255) ? email.Substring(0, 255) : email));
            parameters.Add(CreateParameter("@mail_protocol", mail_protocol));
            parameters.Add(CreateParameter("@mail_inc_host", (mail_inc_host.Length > 255) ? mail_inc_host.Substring(0, 255) : mail_inc_host));
            parameters.Add(CreateParameter("@mail_inc_login", (mail_inc_login.Length > 255) ? mail_inc_login.Substring(0, 255) : mail_inc_login));
            parameters.Add(CreateParameter("@mail_inc_pass", (mail_inc_pass.Length > 255) ? mail_inc_pass.Substring(0, 255) : mail_inc_pass));
            parameters.Add(CreateParameter("@mail_inc_port", mail_inc_port));
            parameters.Add(CreateParameter("@mail_out_host", (mail_out_host.Length > 255) ? mail_out_host.Substring(0, 255) : mail_out_host));
            parameters.Add(CreateParameter("@mail_out_login", (mail_out_login.Length > 255) ? mail_out_login.Substring(0, 255) : mail_out_login));
            parameters.Add(CreateParameter("@mail_out_pass", (mail_out_pass.Length > 255) ? mail_out_pass.Substring(0, 255) : mail_out_pass));
            parameters.Add(CreateParameter("@mail_out_port", mail_out_port));
            parameters.Add(CreateParameter("@mail_out_auth", mail_out_auth));
            parameters.Add(CreateParameter("@friendly_nm", (friendly_nm.Length > 255) ? friendly_nm.Substring(0, 255) : friendly_nm));
            parameters.Add(CreateParameter("@use_friendly_nm", use_friendly_nm));
            parameters.Add(CreateParameter("@def_order", def_order));
            parameters.Add(CreateParameter("@getmail_at_login", getmail_at_login));
            parameters.Add(CreateParameter("@mail_mode", mail_mode));
            parameters.Add(CreateParameter("@mails_on_server_days", mails_on_server_days));
            parameters.Add(CreateParameter("@signature", signature));
            parameters.Add(CreateParameter("@signature_type", signature_type));
            parameters.Add(CreateParameter("@signature_opt", signature_opt));
            parameters.Add(CreateParameter("@delimiter", delimiter));
            parameters.Add(CreateParameter("@mailbox_size", mailbox_size));
            parameters.Add(CreateParameter("@id_domain", id_domain));
            parameters.Add(CreateParameter("@mailing_list", mailing_list));
            parameters.Add(CreateParameter("@imap_quota", imap_quota));
            parameters.Add(CreateParameter("@namespace", Namespace));
            return PrepareCommand(commandText, parameters);
        }

		public override IDbCommand InsertIntoAwmAddrBook(int id_user, string h_email, string fullname, string notes, bool use_friendly_nm, string h_street, string h_city, string h_state, string h_zip, string h_country, string h_phone, string h_fax, string h_mobile, string h_web, string b_email, string b_company, string b_street, string b_city, string b_state, string b_zip, string b_country, string b_job_title, string b_department, string b_office, string b_phone, string b_fax, string b_web, byte birthday_day, byte birthday_month, short birthday_year, string other_email, short primary_email, long id_addr_prev, bool tmp, int use_frequency, bool auto_create, string str_id, DateTime date_modified)
		{
			string commandText = string.Format(@"INSERT INTO [{0}]([id_user], [h_email], [fullname], [notes],
 [use_friendly_nm], [h_street], [h_city], [h_state], [h_zip], [h_country], [h_phone], [h_fax], [h_mobile],
 [h_web], [b_email], [b_company], [b_street], [b_city], [b_state], [b_zip], [b_country], [b_job_title],
 [b_department], [b_office], [b_phone], [b_fax], [b_web], [birthday_day], [birthday_month], [birthday_year],
 [other_email], [primary_email], [id_addr_prev], [tmp], [use_frequency], [auto_create], [str_id], [date_modified])
VALUES(@id_user, @h_email, @fullname, @notes,
 @use_friendly_nm, @h_street, @h_city, @h_state, @h_zip, @h_country, @h_phone, @h_fax, @h_mobile,
 @h_web, @b_email, @b_company, @b_street, @b_city, @b_state, @b_zip, @b_country, @b_job_title,
 @b_department, @b_office, @b_phone, @b_fax, @b_web, @birthday_day, @birthday_month, @birthday_year,
 @other_email, @primary_email, @id_addr_prev, @tmp, @use_frequency, @auto_create, @str_id, @date_modified);",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_book));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));
			parameters.Add(CreateParameter("@h_email", (h_email.Length > 255) ? h_email.Substring(0, 255) : h_email));
			parameters.Add(CreateParameter("@fullname", (fullname.Length > 255) ? fullname.Substring(0, 255) : fullname));
			parameters.Add(CreateParameter("@notes", (notes.Length > 255) ? notes.Substring(0, 255) : notes));
			parameters.Add(CreateParameter("@use_friendly_nm", use_friendly_nm));
			parameters.Add(CreateParameter("@h_street", (h_street.Length > 255) ? h_street.Substring(0, 255) : h_street));
			parameters.Add(CreateParameter("@h_city", (h_city.Length > 200) ? h_city.Substring(0, 200) : h_city));
			parameters.Add(CreateParameter("@h_state", (h_state.Length > 200) ? h_state.Substring(0, 200) : h_state));
			parameters.Add(CreateParameter("@h_zip", (h_zip.Length > 10) ? h_zip.Substring(0, 10) : h_zip));
			parameters.Add(CreateParameter("@h_country", (h_country.Length > 200) ? h_country.Substring(0, 200) : h_country));
			parameters.Add(CreateParameter("@h_phone", (h_phone.Length > 50) ? h_phone.Substring(0, 50) : h_phone));
			parameters.Add(CreateParameter("@h_fax", (h_fax.Length > 50) ? h_fax.Substring(0, 50) : h_fax));
			parameters.Add(CreateParameter("@h_mobile", (h_mobile.Length > 50) ? h_mobile.Substring(0, 50) : h_mobile));
			parameters.Add(CreateParameter("@h_web", (h_web.Length > 255) ? h_web.Substring(0, 255): h_web));
			parameters.Add(CreateParameter("@b_email", (b_email.Length > 255) ? b_email.Substring(0, 255) : b_email));
			parameters.Add(CreateParameter("@b_company", (b_company.Length > 200) ? b_company.Substring(0, 200) : b_company));
			parameters.Add(CreateParameter("@b_street", (b_street.Length > 255) ? b_street.Substring(0, 255) : b_street));
			parameters.Add(CreateParameter("@b_city", (b_city.Length > 200) ? b_city.Substring(0, 200) : b_city));
			parameters.Add(CreateParameter("@b_state", (b_state.Length > 200) ? b_state.Substring(0, 255) : b_state));
			parameters.Add(CreateParameter("@b_zip", (b_zip.Length > 10) ? b_zip.Substring(0, 10) : b_zip));
			parameters.Add(CreateParameter("@b_country", (b_country.Length > 200) ? b_country.Substring(0, 200) : b_country));
			parameters.Add(CreateParameter("@b_job_title", (b_job_title.Length > 100) ? b_job_title.Substring(0, 100) : b_job_title));
			parameters.Add(CreateParameter("@b_department", (b_department.Length > 200) ? b_department.Substring(0, 200) : b_department));
			parameters.Add(CreateParameter("@b_office", (b_office.Length > 200) ? b_office.Substring(0, 200) : b_office));
			parameters.Add(CreateParameter("@b_phone", (b_phone.Length > 50) ? b_phone.Substring(0, 50) : b_phone));
			parameters.Add(CreateParameter("@b_fax", (b_fax.Length > 50) ? b_fax.Substring(0, 50) : b_fax));
			parameters.Add(CreateParameter("@b_web", (b_web.Length > 255) ? b_web.Substring(0, 255) : b_web));
			parameters.Add(CreateParameter("@birthday_day", birthday_day));
			parameters.Add(CreateParameter("@birthday_month", birthday_month));
			parameters.Add(CreateParameter("@birthday_year", birthday_year));
			parameters.Add(CreateParameter("@other_email", (other_email.Length > 255) ? other_email.Substring(0, 255) : other_email));
			parameters.Add(CreateParameter("@primary_email", primary_email));
			parameters.Add(CreateParameter("@id_addr_prev", id_addr_prev));
			parameters.Add(CreateParameter("@tmp", tmp));
			parameters.Add(CreateParameter("@use_frequency", use_frequency));
			parameters.Add(CreateParameter("@auto_create", auto_create));
			parameters.Add(CreateParameter("@str_id", str_id));
			parameters.Add(CreateParameter("@date_modified", date_modified));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand InsertIntoAwmAddrGroups(int id_user, string group_nm, string phone, string fax, string web, bool organization, int use_frequency, string email, string company, string street, string city, string state, string zip, string country, string str_id)
		{
			string commandText = string.Format(@"INSERT INTO {0}([id_user], [group_nm], [phone], [fax], [web], [organization], [use_frequency], [email], [company], [street], [city], [state], [zip], [country], [str_id])
VALUES(@id_user, @group_nm, @phone, @fax, @web, @organization, @use_frequency, @email, @company, @street, @city, @state, @zip, @country, @str_id);
",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));
			parameters.Add(CreateParameter("@group_nm", (group_nm.Length > 255) ? group_nm.Substring(0, 255) : group_nm));
			parameters.Add(CreateParameter("@phone", (phone.Length > 50) ? phone.Substring(0, 50) : phone));
			parameters.Add(CreateParameter("@fax", (fax.Length > 50) ? fax.Substring(0, 50) : fax));
			parameters.Add(CreateParameter("@web", (web.Length > 255) ? web.Substring(0, 255) : web));
			parameters.Add(CreateParameter("@organization", organization));
			parameters.Add(CreateParameter("@use_frequency", use_frequency));
			parameters.Add(CreateParameter("@email", (email.Length > 255) ? email.Substring(0, 255) : email));
			parameters.Add(CreateParameter("@company", (company.Length > 200) ? company.Substring(0, 200) : company));
			parameters.Add(CreateParameter("@street", (street.Length > 255) ? street.Substring(0, 255) : street));
			parameters.Add(CreateParameter("@city", (city.Length > 200) ? city.Substring(0, 200) : city));
			parameters.Add(CreateParameter("@state", (state.Length > 200) ? state.Substring(0, 200) : state));
			parameters.Add(CreateParameter("@zip", (zip.Length > 10) ? zip.Substring(0, 10) : zip));
			parameters.Add(CreateParameter("@country", (country.Length > 200) ? country.Substring(0, 200) : country));
			parameters.Add(CreateParameter("@str_id", (str_id.Length > 100) ? str_id.Substring(0, 100) : str_id));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand InsertIntoAwmFolders(int id_acct, long id_parent, FolderType type, string name, string full_path, FolderSyncType sync_type, bool hide, short fld_order)
		{
			string commandText = string.Format(@"
INSERT INTO {0} ([id_acct] ,[id_parent] ,[type] ,[name] ,[full_path] ,[sync_type] ,[hide] ,[fld_order])
	VALUES (@id_acct, @id_parent, @type, @name, @full_path, @sync_type, @hide, @fld_order);",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_parent", id_parent));
			parameters.Add(CreateParameter("@type", type));
			parameters.Add(CreateParameter("@name", (name.Length > 100) ? name.Substring(0, 100) : name));
			parameters.Add(CreateParameter("@full_path", (full_path.Length > 255) ? full_path.Substring(0, 255) : full_path));
			parameters.Add(CreateParameter("@sync_type", sync_type));
			parameters.Add(CreateParameter("@hide", hide));
			parameters.Add(CreateParameter("@fld_order", fld_order));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand InsertIntoAwmSettings(int id_user, short msgs_per_page, bool white_listing, bool x_spam, DateTime last_login, int logins_count, string def_skin, string def_lang, int def_charset_inc, short def_timezone, string def_date_fmt, bool hide_folders, long mailbox_limit, bool allow_change_settings, bool allow_dhtml_editor, bool allow_direct_mode, bool hide_contacts, int db_charset, short horiz_resizer, short vert_resizer, byte mark, byte reply, short contacts_per_page, int def_charset_out, byte view_mode)
		{
			string commandText = string.Format(@"INSERT INTO {0} (id_user, msgs_per_page, white_listing, x_spam, 
 last_login, logins_count, def_skin, def_lang, def_charset_inc, def_charset_out, def_timezone, def_date_fmt,
 hide_folders, mailbox_limit, allow_change_settings, allow_dhtml_editor, allow_direct_mode, hide_contacts, 
db_charset, horiz_resizer, vert_resizer, mark, reply, contacts_per_page, view_mode)
VALUES(@id_user, @msgs_per_page, @white_listing, @x_spam,
 @last_login, @logins_count, @def_skin, @def_lang, @def_charset_inc, @def_charset_out, @def_timezone, @def_date_fmt,
 @hide_folders, @mailbox_limit, @allow_change_settings, @allow_dhtml_editor, @allow_direct_mode, @hide_contacts,
 @db_charset, @horiz_resizer, @vert_resizer, @mark, @reply, @contacts_per_page, @view_mode);",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_settings));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));
			parameters.Add(CreateParameter("@msgs_per_page", msgs_per_page));
			parameters.Add(CreateParameter("@white_listing", white_listing));
			parameters.Add(CreateParameter("@x_spam", x_spam));
			parameters.Add(CreateParameter("@last_login", last_login));
			parameters.Add(CreateParameter("@logins_count", logins_count));
			parameters.Add(CreateParameter("@def_skin", (def_skin.Length > 255) ? def_skin.Substring(0, 255) : def_skin));
			parameters.Add(CreateParameter("@def_lang", (def_lang.Length > 50) ? def_lang.Substring(0, 50) : def_lang));
			parameters.Add(CreateParameter("@def_charset_inc", def_charset_inc));
			parameters.Add(CreateParameter("@def_charset_out", def_charset_out));
			parameters.Add(CreateParameter("@def_timezone", def_timezone));
			parameters.Add(CreateParameter("@def_date_fmt", (def_date_fmt.Length > 20) ? def_date_fmt.Substring(0, 20) : def_date_fmt));
			parameters.Add(CreateParameter("@hide_folders", hide_folders));
			parameters.Add(CreateParameter("@mailbox_limit", mailbox_limit));
			parameters.Add(CreateParameter("@allow_change_settings", allow_change_settings));
			parameters.Add(CreateParameter("@allow_dhtml_editor", allow_dhtml_editor));
			parameters.Add(CreateParameter("@allow_direct_mode", allow_direct_mode));
			parameters.Add(CreateParameter("@hide_contacts", hide_contacts));
			parameters.Add(CreateParameter("@db_charset", db_charset));
			parameters.Add(CreateParameter("@horiz_resizer", horiz_resizer));
			parameters.Add(CreateParameter("@vert_resizer", vert_resizer));
			parameters.Add(CreateParameter("@mark", mark));
			parameters.Add(CreateParameter("@reply", reply));
			parameters.Add(CreateParameter("@contacts_per_page", contacts_per_page));
			parameters.Add(CreateParameter("@view_mode", view_mode));

			return PrepareCommand(commandText, parameters);
		}

        public override IDbCommand InsertIntoAwmTemp(int id_acct, string data_val)
        {
            string commandText = string.Format(@"INSERT INTO {0}(id_acct, data_val)
VALUES(?, ?)",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_temp));

            ArrayList parameters = new ArrayList();
            parameters.Add(CreateParameter("@id_acct", id_acct));
            parameters.Add(CreateParameter("@data_val", data_val));

            return PrepareCommand(commandText, parameters);
        }

		public override IDbCommand UpdateAwmMessagesFlags(int id_acct, long id_folder_db, bool allMessages, int[] ids, SystemMessageFlags flags, MessageFlagAction flagsAction)
		{
			string flagsField = string.Empty;
			ArrayList flagsStrs = new ArrayList();
			string uidsStr = string.Empty;
			if (!allMessages)
			{
				string strIn = NumberArrayToString(ids);
				uidsStr = string.Format(" AND [id_msg] IN ({0})", strIn);
			}

			switch (flagsAction)
			{
				case MessageFlagAction.Add:
				{
					if ((flags & SystemMessageFlags.Seen) > 0) flagsStrs.Add("[seen]=1");
					if ((flags & SystemMessageFlags.Flagged) > 0) flagsStrs.Add("[flagged]=1");
					if ((flags & SystemMessageFlags.Deleted) > 0) flagsStrs.Add("[deleted]=1");
					if ((flags & SystemMessageFlags.Answered) > 0) flagsStrs.Add("[replied]=1");
					flagsField = "([flags] BOR @flags)";
					break;
				}
				case MessageFlagAction.Remove:
				{
					if ((flags & SystemMessageFlags.Seen) > 0) flagsStrs.Add("[seen]=0");
					if ((flags & SystemMessageFlags.Flagged) > 0) flagsStrs.Add("[flagged]=0");
					if ((flags & SystemMessageFlags.Deleted) > 0) flagsStrs.Add("[deleted]=0");
					if ((flags & SystemMessageFlags.Answered) > 0) flagsStrs.Add("[replied]=0");
					flagsField = "([flags] BAND BNOT(@flags))";
					break;
				}
				case MessageFlagAction.Replace:
				{
					flagsStrs.Add(string.Format("[seen]={0}", ((flags & SystemMessageFlags.Seen) > 0) ? "1" : "0"));
					flagsStrs.Add(string.Format("[flagged]={0}", ((flags & SystemMessageFlags.Flagged) > 0) ? "1" : "0"));
					flagsStrs.Add(string.Format("[deleted]={0}", ((flags & SystemMessageFlags.Deleted) > 0) ? "1" : "0"));
					flagsStrs.Add(string.Format("[replied]={0}", ((flags & SystemMessageFlags.Answered) > 0) ? "1" : "0"));
					flagsField = "@flags";
					break;
				}
			}

			string commandText = string.Format(@"UPDATE {0}
SET {2},[flags]={3}
WHERE [id_acct]=@id_acct AND [id_folder_db]=@id_folder_db {1}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
				uidsStr, // 1
				string.Join(",", (string[])flagsStrs.ToArray(typeof(string))), // 2
				flagsField); // 3

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@flags", (int)flags));
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder_db", id_folder_db));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand UpdateAwmFilters(int id_filter, int id_acct, byte field, byte condition, string filter,
			byte action, long id_folder, bool applied)
		{
			string commandText = string.Format(@"UPDATE {0}
SET [id_acct]=@id_acct, [field]=@field, [condition]=@condition, [filter]=@filter, [action]=@action,
 [id_folder]=@id_folder, [applied]=@applied
WHERE [id_filter]=@id_filter
",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_filters)); // 7

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@field", field));
			parameters.Add(CreateParameter("@condition", condition));
			parameters.Add(CreateParameter("@filter", (filter.Length > 255) ? filter.Substring(0, 255) : filter));
			parameters.Add(CreateParameter("@action", action));
			parameters.Add(CreateParameter("@id_folder", id_folder));
			parameters.Add(CreateParameter("@applied", applied));
			parameters.Add(CreateParameter("@id_filter", id_filter));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectAwmMessagesOlderThanXDays(int id_acct, long id_folder, int daysCount)
		{
			string commandText = string.Format(@"SELECT * FROM [{0}]
WHERE [id_acct]=@id_acct AND [id_folder_db]=@id_folder_db AND DATEDIFF('d', msg_date, DATE()) > {1}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
				daysCount.ToString(CultureInfo.InvariantCulture));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder_db", id_folder));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectAwmAddrBookGroups(int id_user, int contactsOnPage, int page, int sort_field, int sort_order)
		{
            string filter;
			switch (sort_field)
			{
				case 0:
					filter = "is_group";
					break;
				case 1:
					filter = "name";
					break;
				default:
					filter = "email";
					break;
				case 3:
					filter = "use_frequency";
					break;
			}

			string commandText = string.Format(@"SELECT TOP {2} * FROM (SELECT use_frequency, id_addr AS [uniq_id], id_addr AS [id], fullname AS [name],
Choose ([primary_email] + 1, [h_email], [b_email], [other_email]) AS email, 0 AS is_group FROM {0}
WHERE id_user=@id_user
UNION
SELECT use_frequency, -id_group AS [uniq_id], id_group AS [id], group_nm AS [name], '' AS email, 1 AS is_group FROM {1}
WHERE id_user=@id_user ORDER BY {3} {4}) AS union_table
ORDER BY {3} {4}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_book),
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups),
				contactsOnPage,
				filter,
				(sort_order == 0) ? "ASC" : "DESC");

			if (page > 1)
			{
				commandText = string.Format(@"SELECT TOP {2} * FROM (SELECT id_addr AS [uniq_id], id_addr AS [id], fullname AS [name],
Choose ([primary_email] + 1, [h_email], [b_email], [other_email]) AS email, 0 AS is_group FROM {0}
WHERE id_user=@id_user
UNION
SELECT -id_group AS [uniq_id], id_group AS [id], group_nm AS [name], '' AS email, 1 AS is_group FROM {1}
WHERE id_user=@id_user) as union_table 
WHERE [uniq_id] NOT IN (SELECT TOP {3} [uniq_id] FROM (SELECT id_addr AS [uniq_id], id_addr AS [id], fullname AS [name],
Choose ([primary_email], [h_email], [b_email], [other_email]) AS email, 0 AS is_group FROM {0}
WHERE id_user=@id_user
UNION
SELECT -id_group AS [uniq_id], id_group AS [id], group_nm AS [name], '' AS email, 1 AS is_group FROM {1}
WHERE id_user=@id_user) as union_table2
ORDER BY [{4}] {5})
ORDER BY [{4}] {5}",
					EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_book),
					EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups),
					contactsOnPage,
					(page > 0) ? (page - 1) * contactsOnPage : 0,
					filter,
					(sort_order == 0) ? "ASC" : "DESC");
			}

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SearchAwmAddrBookGroups(int id_user, int contactsOnPage, int page, string sort_field, int sort_order, int id_group, string look_for, int look_for_type, int id_domain)
		{
			string lookForSearchCondition = string.Empty;
			if (!string.IsNullOrEmpty(look_for))
			{
				lookForSearchCondition = EscapeWildcardCharacters(EncodeQuotes(look_for));
			}

			string gabSearchCondition = string.Empty;
			if (look_for_type == 1)
			{
				switch (_settings.GlobalAddressBook)
				{
					case GlobalAddressBookEnum.SystemWide:
						gabSearchCondition = string.Format(@"
UNION
SELECT id_acct AS uniq_id, id_acct AS id, friendly_nm AS name, email, 0 AS is_group, 0 AS frequency
FROM awm_accounts
WHERE (email LIKE '{0}%' OR friendly_nm LIKE '{0}%')
", lookForSearchCondition);
						break;
					case GlobalAddressBookEnum.DomainWide:
						if (id_domain > 0)
						{
							gabSearchCondition = string.Format(@"
UNION
SELECT id_acct AS uniq_id, id_acct AS id, friendly_nm AS name, email, 0 AS is_group, 0 AS frequency
FROM awm_accounts
WHERE id_domain={0} AND (email LIKE '{1}%' OR friendly_nm LIKE '{1}%')
", id_domain, lookForSearchCondition);
						}
						break;
					case GlobalAddressBookEnum.Off:
						break;
				}
			}

			string groupSearchCondition;
			if (id_group >= 0)
			{
				groupSearchCondition = string.Format(@"(SELECT [{0}].* FROM {0} INNER JOIN {1} ON {0}.id_addr={1}.id_addr WHERE {1}.id_group={2}) AS table_join",
					EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_book),
					EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups_contacts),
					id_group);
			}
			else
			{
				groupSearchCondition = EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_book);
			}

			string commandText = string.Format(@"SELECT TOP {1} * FROM (SELECT id_addr AS [uniq_id], id_addr AS [id], fullname AS [name],
Choose ([primary_email] + 1, [h_email], [b_email], [other_email]) AS email, 0 AS is_group, use_frequency AS frequency FROM {5}
WHERE id_user=@id_user AND (fullname LIKE '{4}' OR h_email LIKE '{4}' OR b_email LIKE '{4}' OR other_email LIKE '{4}')
UNION
SELECT -id_group AS [uniq_id], id_group AS [id], group_nm AS [name], '' AS email, 1 AS is_group, use_frequency AS frequency FROM {0}
WHERE id_user=@id_user AND (group_nm LIKE '{4}' OR email LIKE '{4}') ORDER BY {2} {3}) AS union_table{6}
ORDER BY {2} {3}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups),
				contactsOnPage,
				sort_field,
				(sort_order == 0) ? "ASC" : "DESC",
				(look_for_type == 0) ? "%" + lookForSearchCondition + "%" : lookForSearchCondition + "%",
				groupSearchCondition,
				gabSearchCondition); //6

			if (page > 1)
			{
				commandText = string.Format(@"SELECT TOP {1} * FROM (SELECT id_addr AS [uniq_id], id_addr AS [id], fullname AS [name],
Choose ([primary_email] + 1, [h_email], [b_email], [other_email]) AS email, 0 AS is_group, use_frequency AS frequency FROM {6}
WHERE id_user=@id_user AND (fullname LIKE '{5}' OR h_email LIKE '{5}' OR b_email LIKE '{5}' OR other_email LIKE '{5}')
UNION
SELECT -id_group AS [uniq_id], id_group AS [id], group_nm AS [name], '' AS email, 1 AS is_group, use_frequency AS frequency FROM {0}
WHERE id_user=@id_user AND (group_nm LIKE '{5}' OR email LIKE '{5}')) as union_table 
WHERE [uniq_id] NOT IN (SELECT TOP {2} [uniq_id] FROM (SELECT id_addr AS [uniq_id], id_addr AS [id], fullname AS [name],
Choose ([primary_email], [h_email], [b_email], [other_email]) AS email, 0 AS is_group, use_frequency AS frequency FROM {6}
WHERE id_user=@id_user AND (fullname LIKE '{5}' OR h_email LIKE '{5}' OR b_email LIKE '{5}' OR other_email LIKE '{5}')
UNION
SELECT -id_group AS [uniq_id], id_group AS [id], group_nm AS [name], '' AS email, 1 AS is_group, use_frequency AS frequency FROM {0}
WHERE id_user=@id_user AND (group_nm LIKE '{5}' OR email LIKE '{5}')) as union_table2
ORDER BY [{3}] {4}){7}
ORDER BY [{3}] {4}",
					EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups),
					contactsOnPage,
					(page > 0) ? (page - 1) * contactsOnPage : 0,
					sort_field,
					(sort_order == 0) ? "ASC" : "DESC",
					(look_for_type == 0) ? "%" + lookForSearchCondition + "%" : lookForSearchCondition + "%",
					groupSearchCondition,
					gabSearchCondition); // 7
			}

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));
			//parameters.Add(CreateParameter("@id_group", id_group));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectAwmMessagesMarkAsDelete(int id_acct, long id_folder)
		{
			string commandText = string.Format(@"SELECT * FROM {0}
WHERE id_acct=@id_acct AND id_folder_db=@id_folder AND deleted=-1",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder", id_folder));

			return PrepareCommand(commandText, parameters);
		}
        
#region Domains

        public override IDbCommand DeleteFromAwmDomains(int id)
        {
            string commandText = string.Format(@"DELETE FROM {0} WHERE id_domain=@id_domain",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_domains));

            ArrayList parameters = new ArrayList();
            parameters.Add(CreateParameter("@id_domain", id));
            return PrepareCommand(commandText, parameters);
        }

        public override IDbCommand InsertIntoAwmDomains(string name, short mail_protocol,
                      string mail_inc_host, int mail_inc_port, string mail_out_host,
                      int mail_out_port, bool mail_out_auth, string url, string site_name,
                      short settings_mail_protocol, string settings_mail_inc_host,
                      int settings_mail_inc_port, string settings_mail_out_host,
                      int settings_mail_out_port, bool settings_mail_out_auth,
                      bool allow_direct_mode, bool direct_mode_id_def, long attachment_size_limit,
                      bool allow_attachment_limit, long mailbox_size_limit, bool allow_mailbox_limit,
                      bool take_quota, bool allow_new_users_change_settings, bool allow_auto_reg_on_login,
                      bool allow_users_add_accounts, bool allow_users_change_account_def,
                      int def_user_charset, bool allow_users_change_charset, int def_user_timezone,
                      bool allow_users_change_timezone, int msgs_per_page, string skin,
                      bool allow_users_change_skin, string lang, bool allow_users_change_lang,
                      bool show_text_labels, bool allow_ajax, bool allow_editor, bool allow_contacts,
                      bool allow_calendar, short hide_login_mode,
                      string domain_to_use, bool allow_choosing_lang, bool allow_advanced_login,
                      bool allow_auto_detect_and_correct, bool global_addr_book, short viewmode, short save_mail)
        {
            string commandText = string.Format(@"
            INSERT INTO {0} 
            (
                name, 
                mail_protocol, 
                mail_inc_host, 
                mail_inc_port, 
                mail_out_host, 
                mail_out_port, 
                mail_out_auth,
                url,
                site_name,
                settings_mail_protocol,
                settings_mail_inc_host,
                settings_mail_inc_port,
                settings_mail_out_host,
                settings_mail_out_port,
                settings_mail_out_auth,
                allow_direct_mode,
                direct_mode_id_def,
                attachment_size_limit,
                allow_attachment_limit,
                mailbox_size_limit,
                allow_mailbox_limit,
                take_quota,
                allow_new_users_change_settings,
                allow_auto_reg_on_login,
                allow_users_add_accounts,
                allow_users_change_account_def,
                def_user_charset,
                allow_users_change_charset,
                def_user_timezone,
                allow_users_change_timezone,
                msgs_per_page,
                skin,
                allow_users_change_skin,
                lang,
                allow_users_change_lang,
                show_text_labels,
                allow_ajax,
                allow_editor,
                allow_contacts,
                allow_calendar,
                hide_login_mode,
                domain_to_use,
                allow_choosing_lang,
                allow_advanced_login,
                allow_auto_detect_and_correct,
                global_addr_book,
                view_mode,
                save_mail
            )
            VALUES 
            (
                @name, 
                @mail_protocol, 
                @mail_inc_host, 
                @mail_inc_port, 
                @mail_out_host, 
                @mail_out_port, 
                @mail_out_auth,
                @url,
                @site_name,
                @settings_mail_protocol,
                @settings_mail_inc_host,
                @settings_mail_inc_port,
                @settings_mail_out_host,
                @settings_mail_out_port,
                @settings_mail_out_auth,
                @allow_direct_mode,
                @direct_mode_id_def,
                @attachment_size_limit,
                @allow_attachment_limit,
                @mailbox_size_limit,
                @allow_mailbox_limit,
                @take_quota,
                @allow_new_users_change_settings,
                @allow_auto_reg_on_login,
                @allow_users_add_accounts,
                @allow_users_change_account_def,
                @def_user_charset,
                @allow_users_change_charset,
                @def_user_timezone,
                @allow_users_change_timezone,
                @msgs_per_page,
                @skin,
                @allow_users_change_skin,
                @lang,
                @allow_users_change_lang,
                @show_text_labels,
                @allow_ajax,
                @allow_editor,
                @allow_contacts,
                @allow_calendar,
                @hide_login_mode,
                @domain_to_use,
                @allow_choosing_lang,
                @allow_advanced_login,
                @allow_auto_detect_and_correct,
                @global_addr_book,
                @viewmode,
                @save_mail
            )",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_domains));


            ArrayList parameters = new ArrayList();
            parameters.Add(CreateParameter("@name", name));
            parameters.Add(CreateParameter("@mail_protocol", mail_protocol));
            parameters.Add(CreateParameter("@mail_inc_host", mail_inc_host));
            parameters.Add(CreateParameter("@mail_inc_port", mail_inc_port));
            parameters.Add(CreateParameter("@mail_out_host", mail_out_host));
            parameters.Add(CreateParameter("@mail_out_port", mail_out_port));
            parameters.Add(CreateParameter("@mail_out_auth", mail_out_auth));

            parameters.Add(CreateParameter("@url", url));
            parameters.Add(CreateParameter("@site_name", site_name));
            parameters.Add(CreateParameter("@settings_mail_protocol", settings_mail_protocol));
            parameters.Add(CreateParameter("@settings_mail_inc_host", settings_mail_inc_host));
            parameters.Add(CreateParameter("@settings_mail_inc_port", settings_mail_inc_port));
            parameters.Add(CreateParameter("@settings_mail_out_host", settings_mail_out_host));
            parameters.Add(CreateParameter("@settings_mail_out_port", settings_mail_out_port));
            parameters.Add(CreateParameter("@settings_mail_out_auth", settings_mail_out_auth));
            parameters.Add(CreateParameter("@allow_direct_mode", allow_direct_mode));
            parameters.Add(CreateParameter("@direct_mode_id_def", direct_mode_id_def));
            parameters.Add(CreateParameter("@attachment_size_limit", attachment_size_limit));
            parameters.Add(CreateParameter("@allow_attachment_limit", allow_attachment_limit));
            parameters.Add(CreateParameter("@mailbox_size_limit", mailbox_size_limit));
            parameters.Add(CreateParameter("@allow_mailbox_limit", allow_mailbox_limit));
            parameters.Add(CreateParameter("@take_quota", take_quota));
            parameters.Add(CreateParameter("@allow_new_users_change_settings", allow_new_users_change_settings));
            parameters.Add(CreateParameter("@allow_auto_reg_on_login", allow_auto_reg_on_login));
            parameters.Add(CreateParameter("@allow_users_add_accounts", allow_users_add_accounts));
            parameters.Add(CreateParameter("@allow_users_change_account_def", allow_users_change_account_def));
            parameters.Add(CreateParameter("@def_user_charset", def_user_charset));
            parameters.Add(CreateParameter("@allow_users_change_charset", allow_users_change_charset));
            parameters.Add(CreateParameter("@def_user_timezone", def_user_timezone));
            parameters.Add(CreateParameter("@allow_users_change_timezone", allow_users_change_timezone));
            parameters.Add(CreateParameter("@msgs_per_page", msgs_per_page));
            parameters.Add(CreateParameter("@skin", skin));
            parameters.Add(CreateParameter("@allow_users_change_skin", allow_users_change_skin));
            parameters.Add(CreateParameter("@lang", lang));
            parameters.Add(CreateParameter("@allow_users_change_lang", allow_users_change_lang));
            parameters.Add(CreateParameter("@show_text_labels", show_text_labels));
            parameters.Add(CreateParameter("@allow_ajax", allow_ajax));
            parameters.Add(CreateParameter("@allow_editor", allow_editor));
            parameters.Add(CreateParameter("@allow_contacts", allow_contacts));
            parameters.Add(CreateParameter("@allow_calendar", allow_calendar));
            parameters.Add(CreateParameter("@hide_login_mode", hide_login_mode));
            parameters.Add(CreateParameter("@domain_to_use", domain_to_use));
            parameters.Add(CreateParameter("@allow_choosing_lang", allow_choosing_lang));
            parameters.Add(CreateParameter("@allow_advanced_login", allow_advanced_login));
            parameters.Add(CreateParameter("@allow_auto_detect_and_correct", allow_auto_detect_and_correct));
            parameters.Add(CreateParameter("@global_addr_book", global_addr_book));
            parameters.Add(CreateParameter("@viewmode", viewmode));
            parameters.Add(CreateParameter("@save_mail", save_mail));

            return PrepareCommand(commandText, parameters);
        }

        public override IDbCommand SelectAwmDomain(int id_domain)
        {
            string commandText = string.Format(@"SELECT * FROM {0} WHERE id_domain=@id_domain",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_domains));

            ArrayList parameters = new ArrayList();
            parameters.Add(CreateParameter("@id_domain", id_domain));
            return PrepareCommand(commandText, parameters);
        }

        public override IDbCommand SelectAwmDomain(string domain)
        {
            string commandText = string.Format(@"SELECT * FROM {0} WHERE name=@name",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_domains));

            ArrayList parameters = new ArrayList();
            parameters.Add(CreateParameter("@name", domain));

            return PrepareCommand(commandText, parameters);
        }

        public override IDbCommand SelectAwmDomainUrl(string url)
        {
            string commandText = string.Format(@"SELECT * FROM {0} WHERE url=@url",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_domains));

            ArrayList parameters = new ArrayList();
            parameters.Add(CreateParameter("@url", url));

            return PrepareCommand(commandText, parameters);
        }

        public override IDbCommand SelectAwmDomains(int page, int pageSize, string orderBy, bool asc, string searchCondition)
        {
            string whereConditionInternal = string.Empty;
            string whereConditionExternal = string.Empty;
            if (!string.IsNullOrEmpty(searchCondition))
            {
                whereConditionInternal = string.Format(@"WHERE name LIKE ('%{0}%') OR mail_inc_host LIKE ('%{0}%') OR mail_out_host LIKE ('%{0}%')", EscapeWildcardCharacters(EncodeQuotes(searchCondition)));
                whereConditionExternal = string.Format(@"AND (name LIKE ('%{0}%') OR mail_inc_host LIKE ('%{0}%') OR mail_out_host LIKE ('%{0}%'))", EscapeWildcardCharacters(EncodeQuotes(searchCondition)));
            }

            string commandText = string.Format(@"SELECT TOP {1} * FROM {0}
WHERE id_domain NOT IN
(SELECT TOP {2} id_domain FROM {0} {5} ORDER BY {3} {4}) {6}
ORDER BY {3} {4}",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_domains),
                pageSize,
                (page > 0) ? (page - 1) * pageSize : 0,
                orderBy,
                (asc) ? "ASC" : "DESC",
                whereConditionInternal,
                whereConditionExternal);

            return PrepareCommand(commandText, null);
        }

        public override IDbCommand SelectAwmDomainsCount(string searchCondition)
        {
            string whereCondition = string.Empty;
            if (!string.IsNullOrEmpty(searchCondition))
            {
                whereCondition = string.Format(@"WHERE name LIKE ('%{0}%') OR mail_inc_host LIKE ('%{0}%') OR mail_out_host LIKE ('%{0}%')", EscapeWildcardCharacters(EncodeQuotes(searchCondition)));
            }

            string commandText = string.Format(@"SELECT COUNT(*) FROM {0} {1}",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_domains),
                whereCondition);

            return PrepareCommand(commandText, null);
        }
#endregion

        public override IDbCommand InsertIntoAwmSubadmins(string login, string password, string description)
        {
            string commandText = string.Format(@"INSERT INTO {0}(login, [password], description)
VALUES(@login, @password, @description);",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_subadmins));

            ArrayList parameters = new ArrayList();
            parameters.Add(CreateParameter("@login", login));
            parameters.Add(CreateParameter("@password", password));
            parameters.Add(CreateParameter("@description", description));

            return PrepareCommand(commandText, parameters);
        }
	}

}
