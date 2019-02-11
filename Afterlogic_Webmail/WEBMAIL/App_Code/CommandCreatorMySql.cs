using System;
using System.Collections;
using System.Data;
using System.Data.Odbc;
using System.Globalization;
using System.Text;
using MailBee.ImapMail;

namespace WebMail
{
	public class MySqlCommandCreator : CommandCreator
	{
		public MySqlCommandCreator(OdbcConnection connection, OdbcCommand command)
			: base(connection, command) {}

		public MySqlCommandCreator(OdbcConnection connection, OdbcCommand command, string dataFolder)
			: base(connection, command, dataFolder) {}

		protected int GetStartIndexFromPage(int page, int pageSize)
		{
            int result = (page > 0) ? (page - 1) * pageSize : 0;
			return result;
		}

		protected override IDataParameter CreateParameter(string paramName, object paramValue)
		{
			Log.WriteLine("CreateParameter", string.Format("{0}='{1}'", paramName, paramValue));
			return new OdbcParameter(paramName, paramValue);
		}

        public override string SelectIdentity()
        {
            return @"SELECT LAST_INSERT_ID();";
        }

		public override IDbCommand SetNames()
		{
			return PrepareCommand("SET NAMES utf8", null);
		}

        public override IDbCommand SelectAwmAccountsForAdmin(int page, int pageSize, string orderBy, bool asc, string searchCondition)
        {
            return SelectAwmAccountsForAdmin(page, pageSize, orderBy, asc, searchCondition, 0);
        }

        public override IDbCommand SelectAwmAccountsForAdmin(int page, int pageSize, string orderBy, bool asc, string searchCondition, int id_domain)
        {
            char[] trimChars = { '[', ']' };
            orderBy = orderBy.Trim(trimChars);

            string whereCondition = string.Empty;
            if ((searchCondition != null) && (searchCondition.Length > 0))
            {
                whereCondition = string.Format(@"WHERE id_domain = {1} AND email LIKE ('{0}')", "%" + EncodeQuotes(searchCondition) + "%", id_domain);
            }
            else
            {
                whereCondition = string.Format(@"WHERE id_domain = {0}", id_domain);
            }

            string commandText = string.Format(@"SELECT {0}.id_user, id_acct, email, last_login, logins_count, mailbox_size, mailbox_limit, mailing_list, imap_quota, deleted FROM {0}
INNER JOIN {1} ON {0}.id_user = {1}.id_user {6} ORDER BY {4} {5} LIMIT {2},{3}",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_accounts),
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_settings),
                GetStartIndexFromPage(page, pageSize),
                pageSize,
                orderBy,
                (asc) ? "ASC" : "DESC",
                whereCondition);

            return PrepareCommand(commandText, null);
        }

		public override IDbCommand InsertIntoAUsers(bool deleted)
		{
			string commandText = string.Format(@"INSERT INTO {0} (deleted) VALUES(?);",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.a_users));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@deleted", deleted));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand InsertIntoAwmSenders(int id_user, string email, byte safety)
		{
			string commandText = string.Format(@"INSERT INTO {0} (id_user, email, safety)
VALUES (?, ?, ?)",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_senders));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));
			parameters.Add(CreateParameter("@email", email));
			parameters.Add(CreateParameter("@safety", safety));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand InsertIntoAwmSettings(int id_user, short msgs_per_page, bool white_listing, bool x_spam, DateTime last_login, int logins_count, string def_skin, string def_lang, int def_charset_inc, short def_timezone, string def_date_fmt, bool hide_folders, long mailbox_limit, bool allow_change_settings, bool allow_dhtml_editor, bool allow_direct_mode, bool hide_contacts, int db_charset, short horiz_resizer, short vert_resizer, byte mark, byte reply, short contacts_per_page, int def_charset_out, byte view_mode)
		{
			string commandText = string.Format(@"INSERT INTO {0} (id_user, msgs_per_page, white_listing, x_spam, 
			last_login, logins_count, def_skin, def_lang, def_charset_inc, def_charset_out, def_timezone, def_date_fmt,
			hide_folders, mailbox_limit, allow_change_settings, allow_dhtml_editor, allow_direct_mode, hide_contacts, 
			db_charset, horiz_resizer, vert_resizer, mark, reply, contacts_per_page, view_mode)
			VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?);",
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

        public override IDbCommand InsertIntoAwmAccounts(int id_user, bool def_acct, bool deleted, string email, IncomingMailProtocol mail_protocol, string mail_inc_host, string mail_inc_login, string mail_inc_pass, int mail_inc_port, string mail_out_host, string mail_out_login, string mail_out_pass, int mail_out_port, bool mail_out_auth, string friendly_nm, bool use_friendly_nm, DefaultOrder def_order, bool getmail_at_login, MailMode mail_mode, short mails_on_server_days, string signature, SignatureType signature_type, SignatureOptions signature_opt, string delimiter, long mailbox_size, string Namespace)
        {
            return InsertIntoAwmAccounts(id_user, def_acct, deleted, email, mail_protocol, mail_inc_host, mail_inc_login, mail_inc_pass, mail_inc_port, mail_out_host, mail_out_login, mail_out_pass, mail_out_port, mail_out_auth, friendly_nm, use_friendly_nm, def_order, getmail_at_login, mail_mode, mails_on_server_days, signature, signature_type, signature_opt, delimiter, mailbox_size, 0, false, -1, Namespace);
        }

        public override IDbCommand InsertIntoAwmAccounts(int id_user, bool def_acct, bool deleted, string email, IncomingMailProtocol mail_protocol, string mail_inc_host, string mail_inc_login, string mail_inc_pass, int mail_inc_port, string mail_out_host, string mail_out_login, string mail_out_pass, int mail_out_port, bool mail_out_auth, string friendly_nm, bool use_friendly_nm, DefaultOrder def_order, bool getmail_at_login, MailMode mail_mode, short mails_on_server_days, string signature, SignatureType signature_type, SignatureOptions signature_opt, string delimiter, long mailbox_size, int id_domain, bool mailing_list, int imap_quota, string Namespace)
        {
            string commandText = string.Format(@"INSERT INTO {0} (id_user, def_acct, deleted, email, mail_protocol, mail_inc_host, mail_inc_login, mail_inc_pass,
			mail_inc_port, mail_out_host, mail_out_login, mail_out_pass, mail_out_port, mail_out_auth, friendly_nm,
			use_friendly_nm, def_order, getmail_at_login, mail_mode, mails_on_server_days, signature, signature_type,
			signature_opt, delimiter, mailbox_size, id_domain, mailing_list, imap_quota, namespace)
			VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?);",
            EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_accounts));

            ArrayList parameters = new ArrayList();
            parameters.Add(CreateParameter("@id_user", id_user));
            parameters.Add(CreateParameter("@def_acct", def_acct));
            parameters.Add(CreateParameter("@deleted", deleted));
            parameters.Add(CreateParameter("@email", (email.Length > 255) ? email.Substring(0, 255) : email));
            parameters.Add(CreateParameter("@mail_protocol", Convert.ToInt32(mail_protocol)));
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
            parameters.Add(CreateParameter("@def_order", Convert.ToInt32(def_order)));
            parameters.Add(CreateParameter("@getmail_at_login", getmail_at_login));
            parameters.Add(CreateParameter("@mail_mode", Convert.ToInt32(mail_mode)));
            parameters.Add(CreateParameter("@mails_on_server_days", mails_on_server_days));
            parameters.Add(CreateParameter("@signature", signature));
            parameters.Add(CreateParameter("@signature_type", Convert.ToInt32(signature_type)));
            parameters.Add(CreateParameter("@signature_opt", Convert.ToInt32(signature_opt)));
            parameters.Add(CreateParameter("@delimiter", delimiter));
            parameters.Add(CreateParameter("@mailbox_size", mailbox_size));
            parameters.Add(CreateParameter("@id_domain", id_domain));
            parameters.Add(CreateParameter("@mailing_list", mailing_list));
            parameters.Add(CreateParameter("@imap_quota", imap_quota));
            parameters.Add(CreateParameter("@namespace", Namespace));
            return PrepareCommand(commandText, parameters);
        }

		public override IDbCommand InsertIntoAwmColumns(int id_column, int id_user, int value)
		{
			string commandText = string.Format(@"INSERT INTO {0} (id_user, id_column, column_value)
VALUES (?, ?, ?);",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_columns));


			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));
			parameters.Add(CreateParameter("@id_column", id_column));
			parameters.Add(CreateParameter("@value", value));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand InsertIntoAwmFilters(int id_acct, byte field, byte condition, string filter, 
			byte action, long id_folder, bool applied)
		{
			string commandText = string.Format(@"
INSERT INTO {0} (id_acct, `field`, `condition`, `filter`, `action`, id_folder, applied)
VALUES (?, ?, ?, ?, ?, ?, ?)",
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

		public override IDbCommand InsertIntoAwmFolders(int id_acct, long id_parent, FolderType type, string name, string full_path, FolderSyncType sync_type, bool hide, short fld_order)
		{
			string commandText = string.Format(@"INSERT INTO {0} (id_acct, id_parent, type, name, full_path, sync_type, hide, fld_order)
			VALUES (?, ?, ?, ?, ?, ?, ?, ?);",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders));


			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_parent", id_parent));
			parameters.Add(CreateParameter("@type", Convert.ToInt32(type)));
			parameters.Add(CreateParameter("@name", (name.Length > 100) ? name.Substring(0, 100) : name));
			parameters.Add(CreateParameter("@full_path", (full_path.Length > 255) ? full_path.Substring(0, 255) : full_path));
			parameters.Add(CreateParameter("@sync_type", Convert.ToInt32(sync_type)));
			parameters.Add(CreateParameter("@hide", hide));
			parameters.Add(CreateParameter("@fld_order", fld_order));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand InsertIntoAwmFoldersTree(long id_folder)
		{
			string commandText = string.Format(@"INSERT INTO {0} (id_folder, id_parent, folder_level) VALUES (?, ?, 0);",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders_tree));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_folder", id_folder));
			parameters.Add(CreateParameter("@id_folder", id_folder));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand InsertIntoAwmFoldersTree(long id_folder, long id_parent)
		{
			string commandText = string.Format(@"INSERT INTO {0} (id_folder, id_parent, folder_level)
			SELECT ?, id_parent, (folder_level + 1) AS folders_level
			FROM {0}
			WHERE id_folder=?;",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders_tree));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_folder", id_folder));
			parameters.Add(CreateParameter("@id_parent", id_parent));

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

		public override IDbCommand UpdateAwmAccounts(int id_acct, int id_user, bool def_acct, bool deleted, string email, IncomingMailProtocol mail_protocol, string mail_inc_host, string mail_inc_login, string mail_inc_pass, int mail_inc_port, string mail_out_host, string mail_out_login, string mail_out_pass, int mail_out_port, bool mail_out_auth, string friendly_nm, bool use_friendly_nm, DefaultOrder def_order, bool getmail_at_login, MailMode mail_mode, short mails_on_server_days, string signature, SignatureType signature_type, SignatureOptions signature_opt, string delimiter, long mailbox_size, int imap_quota, string Namespace)
		{
			string commandText = string.Format(@"UPDATE {0} SET id_user=?, def_acct=?, deleted=?, email=?, mail_protocol=?,
			mail_inc_host=?, mail_inc_login=?, mail_inc_pass=?,
			mail_inc_port=?, mail_out_host=?, mail_out_login=?,
			mail_out_pass=?, mail_out_port=?, mail_out_auth=?, friendly_nm=?,
			use_friendly_nm=?, def_order=?, getmail_at_login=?, mail_mode=?,
			mails_on_server_days=?, signature=?, signature_type=?,
			signature_opt=?, delimiter=?, mailbox_size=?, imap_quota=?, namespace=?
			WHERE id_acct=?",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_accounts));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));
			parameters.Add(CreateParameter("@def_acct", def_acct));
			parameters.Add(CreateParameter("@deleted", deleted));
			parameters.Add(CreateParameter("@email", (email.Length > 255) ? email.Substring(0, 255) : email));
			parameters.Add(CreateParameter("@mail_protocol", Convert.ToInt32(mail_protocol)));
			parameters.Add(CreateParameter("@mail_inc_host", (mail_inc_host.Length > 255) ? mail_inc_host.Substring(0, 255) : mail_inc_host));
			parameters.Add(CreateParameter("@mail_inc_login", (mail_inc_login.Length > 255) ? mail_inc_login.Substring(0, 255) : mail_inc_login));
			parameters.Add(CreateParameter("@mail_inc_pass", (mail_inc_pass.Length > 255) ? mail_inc_pass.Substring(0, 255) : mail_inc_pass));
			parameters.Add(CreateParameter("@mail_inc_port", mail_inc_port));
			parameters.Add(CreateParameter("@mail_out_host", (mail_out_host.Length > 255) ? mail_out_host.Substring(0, 255) : mail_out_host));
			parameters.Add(CreateParameter("@mail_out_login", (mail_out_login.Length > 255) ? mail_out_host.Substring(0, 255) : mail_out_login));
			parameters.Add(CreateParameter("@mail_out_pass", (mail_out_pass.Length > 255) ? mail_out_pass.Substring(0, 255) : mail_out_pass));
			parameters.Add(CreateParameter("@mail_out_port", mail_out_port));
			parameters.Add(CreateParameter("@mail_out_auth", mail_out_auth));
			parameters.Add(CreateParameter("@friendly_nm", (friendly_nm.Length > 200) ? friendly_nm.Substring(0, 200) : friendly_nm));
			parameters.Add(CreateParameter("@use_friendly_nm", use_friendly_nm));
			parameters.Add(CreateParameter("@def_order", Convert.ToInt32(def_order)));
			parameters.Add(CreateParameter("@getmail_at_login", getmail_at_login));
			parameters.Add(CreateParameter("@mail_mode", Convert.ToInt32(mail_mode)));
			parameters.Add(CreateParameter("@mails_on_server_days", mails_on_server_days));
			parameters.Add(CreateParameter("@signature", signature));
			parameters.Add(CreateParameter("@signature_type", Convert.ToInt32(signature_type)));
			parameters.Add(CreateParameter("@signature_opt", Convert.ToInt32(signature_opt)));
			parameters.Add(CreateParameter("@delimiter", delimiter));
			parameters.Add(CreateParameter("@mailbox_size", mailbox_size));
            parameters.Add(CreateParameter("@imap_quota", imap_quota));
            parameters.Add(CreateParameter("@namespace", Namespace));
            parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand UpdateAUser(int id_user, bool deleted)
		{
			string commandText = string.Format("UPDATE {0} SET deleted=? WHERE id_user=?",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.a_users));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@deleted", deleted));
			parameters.Add(CreateParameter("@id_user", id_user));

			return PrepareCommand(commandText, parameters);
		}

        public override IDbCommand UpdateAUsersByLicences(int licences_num)
        {
            string commandText = string.Format(@"UPDATE {0}a_users
SET deleted = 0
WHERE {0}a_users.id_user
IN (SELECT {0}awm_settings.id_user FROM {0}awm_settings)
    order by {0}a_users.id_user LIMIT ?",
                EncodeQuotes(_settings.DbPrefix));

            ArrayList parameters = new ArrayList();
            parameters.Add(CreateParameter("@licences_num", licences_num));

            return PrepareCommand(commandText, parameters);
        }

		public override IDbCommand UpdateAwmAccountsDefOrder(int id_acct, int def_order)
		{
			string commandText = string.Format("UPDATE {0} SET def_order=? WHERE id_acct=?",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_accounts));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@def_order", def_order));
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand UpdateAwmSenders(int id_user, string email, byte safety)
		{
			string commandText = string.Format(@"UPDATE {0} SET
email=?, safety=?
WHERE id_user=?",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_senders));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@email", email));
			parameters.Add(CreateParameter("@safety", safety));
			parameters.Add(CreateParameter("@id_user", id_user));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand UpdateAwmSettings(int id, int id_user, int msgs_per_page, bool white_listing, bool x_spam, DateTime last_login, int logins_count, string def_skin, string def_lang, int def_charset_inc, short def_timezone, string def_date_fmt, bool hide_folders, long mailbox_limit, bool allow_change_settings, bool allow_dhtml_editor, bool allow_direct_mode, bool hide_contacts, int db_charset, short horiz_resizer, short vert_resizer, byte mark, byte reply, short contacts_per_page, int def_charset_out, byte view_mode, TimeFormats time_fmt, int auto_checkmail_interval)
		{
			string commandText = string.Format(@"UPDATE {0} SET id_user=?, msgs_per_page=?, white_listing=?, x_spam=?, last_login=?,
			logins_count=?, def_skin=?, def_lang=?, def_charset_inc=?, def_timezone=?,
			def_date_fmt=?, hide_folders=?, mailbox_limit=?, allow_change_settings=?,
			allow_dhtml_editor=?, allow_direct_mode=?, hide_contacts=?, db_charset=?,
			horiz_resizer=?, vert_resizer=?, mark=?, reply=?, contacts_per_page=?,
			def_charset_out=?, view_mode=?, auto_checkmail_interval=?
			WHERE id_setting=?",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_settings));

			int maxDateFmtLength = 20 - Constants.timeFormat.Length;
			def_date_fmt = (def_date_fmt.Length > maxDateFmtLength) ? def_date_fmt.Substring(0, maxDateFmtLength) : def_date_fmt;
            def_date_fmt = (time_fmt == TimeFormats.F12) ? def_date_fmt + Constants.timeFormat : def_date_fmt;

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
			parameters.Add(CreateParameter("@def_timezone", def_timezone));
			parameters.Add(CreateParameter("@def_date_fmt", def_date_fmt));
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
			parameters.Add(CreateParameter("@def_charset_out", def_charset_out));
			parameters.Add(CreateParameter("@view_mode", view_mode));
            parameters.Add(CreateParameter("@auto_checkmail_interval", auto_checkmail_interval));
			parameters.Add(CreateParameter("@id_setting", id));

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
				groupSearchCondition = string.Format(@"(SELECT {0}.* FROM {0} INNER JOIN {1} ON {0}.id_addr={1}.id_addr WHERE {1}.id_group={2}) AS table_join",
					EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_book),
					EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups_contacts),
					id_group.ToString(CultureInfo.InvariantCulture));
			}
			else
			{
				groupSearchCondition = EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_book);
			}

			string commandText = string.Format(@"SELECT id_addr AS uniq_id, id_addr AS id, fullname AS name,
CASE primary_email
	WHEN 0 THEN h_email
	WHEN 1 THEN b_email
	WHEN 2 THEN other_email
END  AS email, 0 AS is_group, use_frequency AS frequency FROM {6}
WHERE id_user=? AND (fullname LIKE '{5}' OR h_email LIKE '{5}' OR b_email LIKE '{5}' OR other_email LIKE '{5}')
UNION
SELECT -id_group AS uniq_id, id_group AS id, group_nm AS name, '' AS email, 1 AS is_group, use_frequency AS frequency FROM {0}
WHERE id_user=?  AND (group_nm LIKE '{5}' OR email LIKE '{5}'){7}
ORDER BY {3} {4} LIMIT {2},{1}
",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups), // 0
				contactsOnPage, // 1
				(page > 0) ? (page - 1) * contactsOnPage : 0, // 2
				sort_field, // 3
				(sort_order == 0) ? "ASC" : "DESC", // 4
				(look_for_type == 0) ? "%" + lookForSearchCondition + "%" : lookForSearchCondition + "%", // 5
				groupSearchCondition,
				gabSearchCondition); // 6

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));
			parameters.Add(CreateParameter("@id_user", id_user));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectAUsersAndAwmSettings(int id_user)
		{
			string commandText = string.Format(@"SELECT * FROM {0} AS users INNER JOIN {1} AS settings
			ON users.id_user = settings.id_user
			WHERE users.id_user=?",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.a_users),
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_settings),
			id_user);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectAwmFoldersChilds(int id_acct, long id_parent)
		{
			string commandText = string.Format(@"SELECT p.* FROM {0} n, {1} t, {0} p
			WHERE n.id_parent = ?
			AND n.id_folder = t.id_parent
			AND t.id_folder = p.id_folder
			AND p.id_acct = ?
			ORDER BY t.folder_level",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders),
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders_tree));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_parent", id_parent));
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectAwmFolders(int id_acct)
		{
			string commandText = string.Format(@"SELECT p.* FROM {0} n, {1} t, {0} p
			WHERE n.id_parent = -1
			AND n.id_folder = t.id_parent
			AND t.id_folder = p.id_folder
			AND p.id_acct = ?
			ORDER BY t.folder_level",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders),
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders_tree));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectAwmFolders(int id_acct, long id_folder)
		{
			string commandText = string.Format(@"SELECT * FROM {0} 
			WHERE id_acct=? AND id_folder=?",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder", id_folder));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectAwmFolders(int id_acct, FolderType type)
		{
			string commandText = string.Format(@"SELECT * FROM {0} 
			WHERE id_acct=? AND type=?",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@type", Convert.ToInt16(type)));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectAwmFolders(int id_acct, string full_path)
		{
			string commandText = string.Format(@"SELECT * FROM {0} 
			WHERE id_acct=? AND full_path=? LIMIT 1",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@full_path", full_path));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand InsertIntoAwmMessages(int id_msg, int id_acct, long id_folder_srv, long id_folder_db, string str_uid, long int_uid, string from_msg, string to_msg, string cc_msg, string bcc_msg, string subject, DateTime msg_date, bool attachments, long size, bool seen, bool flagged, byte priority, byte sensitivity, bool downloaded, bool x_spam, bool rtl, bool deleted, bool is_full, bool replied, bool forwarded, int flags, string body_text, bool grayed, int charset)
		{
			if (str_uid != null)
			{
				if (str_uid.Length > 255)
				{
					str_uid = str_uid.Substring(0, 255);
					is_full = false;
				}
			}

			if (from_msg != null)
			{
				if (from_msg.Length > 255)
				{
					from_msg = from_msg.Substring(0, 255);
					is_full = false;
				}
			}

			if (to_msg != null)
			{
				if (to_msg.Length > 255)
				{
					to_msg = to_msg.Substring(0, 255);
					is_full = false;
				}
			}

			if (cc_msg != null)
			{
				if (cc_msg.Length > 255)
				{
					cc_msg = cc_msg.Substring(0, 255);
					is_full = false;
				}
			}

			if (bcc_msg != null)
			{
				if (bcc_msg.Length > 255)
				{
					bcc_msg = bcc_msg.Substring(0, 255);
					is_full = false;
				}
			}

			if (subject != null)
			{
				if (subject.Length > 255)
				{
					subject = subject.Substring(0, 255);
					is_full = false;
				}
			}

			if (body_text != null)
			{
				if (body_text.Length > 65535)
				{
					body_text = body_text.Substring(0, 65535);
					is_full = false;
				}
			}

			string commandText = string.Format(@"INSERT INTO {0} (id_msg, id_acct, id_folder_srv, id_folder_db,
			str_uid, int_uid, from_msg, to_msg, cc_msg, bcc_msg, subject, msg_date, attachments,
			size, seen, flagged, priority, downloaded, x_spam, rtl, deleted, is_full, replied,
			forwarded, flags, body_text, grayed, charset, sensitivity)
			VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_msg", id_msg));
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder_srv", id_folder_srv));
			parameters.Add(CreateParameter("@id_folder_db", id_folder_db));
			parameters.Add(CreateParameter("@str_uid", str_uid));
			parameters.Add(CreateParameter("@int_uid", int_uid));
			parameters.Add(CreateParameter("@from_msg", from_msg));
			parameters.Add(CreateParameter("@to_msg", to_msg));
			parameters.Add(CreateParameter("@cc_msg", cc_msg));
			parameters.Add(CreateParameter("@bcc_msg", bcc_msg));
			parameters.Add(CreateParameter("@subject", subject));
			parameters.Add(CreateParameter("@msg_date", msg_date));
			parameters.Add(CreateParameter("@attachments", attachments));
			parameters.Add(CreateParameter("@size", size));
			parameters.Add(CreateParameter("@seen", seen));
			parameters.Add(CreateParameter("@flagged", flagged));
			parameters.Add(CreateParameter("@priority", priority));
			parameters.Add(CreateParameter("@downloaded", downloaded));
			parameters.Add(CreateParameter("@x_spam", x_spam));
			parameters.Add(CreateParameter("@rtl", rtl));
			parameters.Add(CreateParameter("@deleted", deleted));
			parameters.Add(CreateParameter("@is_full", is_full));
			parameters.Add(CreateParameter("@replied", replied));
			parameters.Add(CreateParameter("@forwarded", forwarded));
			parameters.Add(CreateParameter("@flags", flags));
			parameters.Add(CreateParameter("@body_text", body_text));
			parameters.Add(CreateParameter("@grayed", grayed));
			parameters.Add(CreateParameter("@charset", charset));
            parameters.Add(CreateParameter("@sensitivity", sensitivity));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand InsertIntoAwmMessagesBody(int id_acct, int id_msg, byte[] msg)
		{
			string commandText = string.Format(@"INSERT INTO {0} (id_acct, id_msg, msg)
			VALUES(?, ?, ?)",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages_body));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_msg", id_msg));
			parameters.Add(CreateParameter("@msg", msg));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand UpdateAwmAddrGroups(int id_group, int id_user, string group_nm, string phone, string fax, string web, bool organization, int use_frequency, string email, string company, string street, string city, string state, string zip, string country, string str_id)
		{
			string commandText = string.Format(@"UPDATE {0}
			SET id_user=?, group_nm=?, phone=?, fax=?, web=?, 
organization=?, use_frequency=?, email=?, company=?, street=?, 
city=?, state=?, zip=?, country=?{1}
			WHERE id_group=?",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups),
			(!string.IsNullOrEmpty(str_id)) ? ", str_id=?" : string.Empty);

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
			if (!string.IsNullOrEmpty(str_id)) parameters.Add(CreateParameter("@str_id", (str_id.Length > 100) ? str_id.Substring(0, 100) : str_id));
			parameters.Add(CreateParameter("@id_group", id_group));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand UpdateAwmMessages(long id, int id_msg, int id_acct, long id_folder_srv, long id_folder_db, string str_uid, long int_uid, string from_msg, string to_msg, string cc_msg, string bcc_msg, string subject, DateTime msg_date, bool attachments, long size, bool seen, bool flagged, byte priority, byte sensitivity, bool downloaded, bool x_spam, bool rtl, bool deleted, bool is_full, bool replied, bool forwarded, int flags, string body_text, bool grayed, int charset)
		{
			if (str_uid != null)
			{
				if (str_uid.Length > 255)
				{
					str_uid = str_uid.Substring(0, 255);
					is_full = false;
				}
			}

			if (from_msg != null)
			{
				if (from_msg.Length > 255)
				{
					from_msg = from_msg.Substring(0, 255);
					is_full = false;
				}
			}

			if (to_msg != null)
			{
				if (to_msg.Length > 255)
				{
					to_msg = to_msg.Substring(0, 255);
					is_full = false;
				}
			}

			if (cc_msg != null)
			{
				if (cc_msg.Length > 255)
				{
					cc_msg = cc_msg.Substring(0, 255);
					is_full = false;
				}
			}

			if (bcc_msg != null)
			{
				if (bcc_msg.Length > 255)
				{
					bcc_msg = bcc_msg.Substring(0, 255);
					is_full = false;
				}
			}

			if (subject != null)
			{
				if (subject.Length > 255)
				{
					subject = subject.Substring(0, 255);
					is_full = false;
				}
			}

			if (body_text != null)
			{
				if (body_text.Length > 65535)
				{
					body_text = body_text.Substring(0, 65535);
					is_full = false;
				}
			}

			// downloaded must updated in 'SaveMessage'
			string commandText = string.Format(@"UPDATE {0}
			SET id_msg=?, id_acct=?, id_folder_srv=?, id_folder_db=?,
			str_uid=?, int_uid=?, from_msg=?, to_msg=?, cc_msg=?, bcc_msg=?,
			subject=?, msg_date=?, attachments=?, size=?, seen=?, flagged=?,
			priority=?, x_spam=?, rtl=?, deleted=?, is_full=?,
			replied=?, forwarded=?, flags=?, body_text=?, grayed=?, charset=?, sensitivity=?
			WHERE id=?",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_msg", id_msg));
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder_srv", id_folder_srv));
			parameters.Add(CreateParameter("@id_folder_db", id_folder_db));
			parameters.Add(CreateParameter("@str_uid", str_uid));
			parameters.Add(CreateParameter("@int_uid", int_uid));
			parameters.Add(CreateParameter("@from_msg", from_msg));
			parameters.Add(CreateParameter("@to_msg", to_msg));
			parameters.Add(CreateParameter("@cc_msg", cc_msg));
			parameters.Add(CreateParameter("@bcc_msg", bcc_msg));
			parameters.Add(CreateParameter("@subject", subject));
			parameters.Add(CreateParameter("@msg_date", msg_date));
			parameters.Add(CreateParameter("@attachments", attachments));
			parameters.Add(CreateParameter("@size", size));
			parameters.Add(CreateParameter("@seen", seen));
			parameters.Add(CreateParameter("@flagged", flagged));
			parameters.Add(CreateParameter("@priority", priority));
			//parameters.Add(CreateParameter("@downloaded", downloaded));
			parameters.Add(CreateParameter("@x_spam", x_spam));
			parameters.Add(CreateParameter("@rtl", rtl));
			parameters.Add(CreateParameter("@deleted", deleted));
			parameters.Add(CreateParameter("@is_full", is_full));
			parameters.Add(CreateParameter("@replied", replied));
			parameters.Add(CreateParameter("@forwarded", forwarded));
			parameters.Add(CreateParameter("@flags", flags));
			parameters.Add(CreateParameter("@body_text", body_text));
			parameters.Add(CreateParameter("@grayed", grayed));
			parameters.Add(CreateParameter("@charset", charset));
            parameters.Add(CreateParameter("@sensitivity", sensitivity));
			parameters.Add(CreateParameter("@id", id));

			return PrepareCommand(commandText, parameters);
		}

        public override IDbCommand UpdateAwmMessagesForwardedFlag(int id_acct, long id_folder_db, int[] ids)
        {
            string flagsField = string.Empty;
            ArrayList flagsStrs = new ArrayList();
            string uidsStr = string.Empty;

            string strIn = NumberArrayToString(ids);
            uidsStr = string.Format(" AND id_msg IN ({0})", strIn);

            string commandText = string.Format(@"UPDATE {0}
            SET forwarded=1
            WHERE id_acct=? AND id_folder_db=? {1}",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
                uidsStr); // 3

            ArrayList parameters = new ArrayList();
            parameters.Add(CreateParameter("@id_acct", id_acct));
            parameters.Add(CreateParameter("@id_folder_db", id_folder_db));

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
				uidsStr = string.Format(" AND id_msg IN ({0})", strIn);
			}

			switch (flagsAction)
			{
				case MessageFlagAction.Add:
				{
					if ((flags & SystemMessageFlags.Seen) > 0) flagsStrs.Add("seen=1");
					if ((flags & SystemMessageFlags.Flagged) > 0) flagsStrs.Add("flagged=1");
					if ((flags & SystemMessageFlags.Deleted) > 0) flagsStrs.Add("deleted=1");
					if ((flags & SystemMessageFlags.Answered) > 0) flagsStrs.Add("replied=1");
					flagsField = "(flags | ?)";
					break;
				}
				case MessageFlagAction.Remove:
				{
					if ((flags & SystemMessageFlags.Seen) > 0) flagsStrs.Add("seen=0");
					if ((flags & SystemMessageFlags.Flagged) > 0) flagsStrs.Add("flagged=0");
					if ((flags & SystemMessageFlags.Deleted) > 0) flagsStrs.Add("deleted=0");
					if ((flags & SystemMessageFlags.Answered) > 0) flagsStrs.Add("replied=0");
					flagsField = "(flags & ~?)";
					break;
				}
				case MessageFlagAction.Replace:
				{
					flagsStrs.Add(string.Format("seen={0}", ((flags & SystemMessageFlags.Seen) > 0) ? "1" : "0"));
					flagsStrs.Add(string.Format("flagged={0}", ((flags & SystemMessageFlags.Flagged) > 0) ? "1" : "0"));
					flagsStrs.Add(string.Format("deleted={0}", ((flags & SystemMessageFlags.Deleted) > 0) ? "1" : "0"));
					flagsStrs.Add(string.Format("replied={0}", ((flags & SystemMessageFlags.Answered) > 0) ? "1" : "0"));
					flagsField = "?";
					break;
				}
			}

			string commandText = string.Format(@"UPDATE {0}
			SET {2},flags={3}
			WHERE id_acct=? AND id_folder_db=? {1}",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
			uidsStr,
			string.Join(",", (string[])flagsStrs.ToArray(typeof(string))),
			flagsField); 

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@flags", (int)flags));
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder_db", id_folder_db));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand UpdateAwmMessagesFolders(int id_acct, int[] ids, long id_folder_db, long id_folder_db_new)
		{
			string strIn = NumberArrayToString(ids);
			
			string commandText = string.Format(@"UPDATE {0} SET id_folder_db=?
			WHERE id_acct=? AND id_folder_db=? AND id_msg IN ({1})
			", 
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
			strIn); // 4

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_folder_db_new", id_folder_db_new));
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder_db", id_folder_db));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectIDMsgFromAwmMessages(int id_acct)
		{
			string commandText = string.Format(@"SELECT MAX(id_msg) FROM {0} WHERE id_acct=?",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectFolderSize(long id_folder, int id_acct)
		{
			string commandText = string.Format(@"SELECT SUM(size) FROM {0}
			WHERE id_folder_db=? AND id_acct=?",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_folder", id_folder));
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectFolderMessageCount(long id_folder, int id_acct)
		{
			string commandText = string.Format(@"SELECT COUNT(*) FROM {0}
			WHERE id_folder_db=? AND id_acct=?",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_folder", id_folder));
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectFolderUnreadMessageCount(long id_folder, int id_acct)
		{
			string commandText = string.Format(@"SELECT COUNT(*) FROM {0}
			WHERE id_folder_db=? AND id_acct=? AND seen=0",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_folder", id_folder));
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectFolderStatistics(long id_folder, int id_acct)
		{
			string commandText = string.Format(@"
SELECT 0, SUM([size]) FROM awm_messages
WITH(NOLOCK) WHERE id_folder_db=? AND id_acct=?
UNION
SELECT 1, COUNT(*)
FROM awm_messages
WITH(NOLOCK) WHERE id_folder_db=? AND id_acct=?
UNION
SELECT 2, COUNT(*)
FROM awm_messages
WITH(NOLOCK) WHERE id_folder_db=? AND id_acct=? AND seen=0
",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
				_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_folder", id_folder));
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectTop1AwmMessages(int id_acct, long id_folder_db)
		{
			string commandText = string.Format(@"SELECT int_uid FROM {0}
			WHERE id_acct=? AND id_folder_db=? ORDER BY int_uid DESC LIMIT 1",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder_db", id_folder_db));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectAwmMessages(int id_acct, long id_folder_db, int pageNumber, int msgsOnPage, string order, bool asc)
		{
			char[] trimChars = {'[', ']'};
			order = order.Trim(trimChars);

			string commandText = string.Format(@"SELECT * FROM {0}
			WHERE id_acct=? AND id_folder_db=? AND id_msg 
			ORDER BY {3} {4} LIMIT {2},{1} ",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
			msgsOnPage, // 1
			(pageNumber > 0) ? (pageNumber - 1) * msgsOnPage : 0, // 2
			order, // 3
			(asc) ? "ASC" : "DESC" // 4
			);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder_db", id_folder_db));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectAwmMessages(int id_acct, int pageNumber, int msgsOnPage, string condition, FolderCollection folders, bool inHeadersOnly, string order, bool asc)
		{
			char[] trimChars = {'[', ']'};
			order = order.Trim(trimChars);

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
				bodyLike = string.Format(" OR body_text LIKE '%{0}%'", EscapeWildcardCharacters(EncodeQuotes(condition)));
			}
			string commandText = string.Format(@"SELECT messages.*, folders.name AS folder_name FROM (SELECT * FROM {0} 
			WHERE id_acct=? AND id_folder_db IN ({5}) AND (from_msg LIKE '%{6}%' OR to_msg LIKE '%{6}%' OR cc_msg LIKE '%{6}%' 
			OR bcc_msg LIKE '%{6}%' OR subject LIKE '%{6}%'{8}) AND id_msg ORDER BY {3} {4} LIMIT {2},{1}) 
			AS messages INNER JOIN {7} AS folders ON messages.id_folder_db = folders.id_folder",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),//0
			msgsOnPage,//1
			(pageNumber > 0) ? (pageNumber - 1) * msgsOnPage : 0,//2
			order,//3
			(asc) ? "ASC" : "DESC",//4
			folder_ids,//5
			EncodeQuotes(condition),//6
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders),//7
			bodyLike);//8

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectAwmMessagesCount(int id_acct, int pageNumber, int msgsOnPage, string condition, FolderCollection folders, bool inHeadersOnly, string order, bool asc)
		{
			char[] trimChars = {'[', ']'};

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
				bodyLike = string.Format(" OR body_text LIKE '%{0}%'", EscapeWildcardCharacters(EncodeQuotes(condition)));
			}
			string commandText = string.Format(@"SELECT COUNT(*) FROM {0}
WHERE id_acct=? AND id_folder_db IN ({1}) AND 
(from_msg LIKE '%{2}%' OR to_msg LIKE '%{2}%' OR cc_msg LIKE '%{2}%' OR bcc_msg
LIKE '%{2}%' OR subject LIKE '%{2}%'{3});",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),//0
				folder_ids,//1
				EncodeQuotes(condition),//2
				bodyLike);//3

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));

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
				case 2:
					filter = "email";
					break;
				case 3:
					filter = "use_frequency";
					break;
			}

			string commandText = string.Format(@"SELECT * FROM (SELECT use_frequency, id_addr AS uniq_id, id_addr AS id, fullname AS name, 
			CASE primary_email WHEN 0 THEN h_email WHEN 1 THEN b_email WHEN 2 THEN other_email END  AS email, 0 AS is_group FROM {0} 
			WHERE id_user=? UNION SELECT use_frequency, -id_group AS uniq_id, id_group AS id, group_nm AS name, '' AS email, 1 AS is_group FROM {1} 
			WHERE id_user=?) as union_table WHERE uniq_id ORDER BY {4} {5} LIMIT {3},{2}",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_book),//0
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups),//1
			contactsOnPage,//2
			(page > 0) ? (page - 1) * contactsOnPage : 0,//3
			filter,//4
			(sort_order == 0) ? "ASC" : "DESC");//5

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));
			parameters.Add(CreateParameter("@id_user", id_user));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand UpdateAwmFolders(int id_acct, long id_folder, short type, string name, string full_path, byte sync_type, bool hide, short fld_order)
		{
			string commandText = string.Format(@" UPDATE {0} SET type=?, name=?, full_path=?, sync_type=?, hide=?, fld_order=?
			WHERE id_acct=? AND id_folder=?",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders)); //8

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@type", type));
			parameters.Add(CreateParameter("@name", (name.Length > 100) ? name.Substring(0, 100) : name));
			parameters.Add(CreateParameter("@full_path", (full_path.Length > 255) ? full_path.Substring(0, 255) : full_path));
			parameters.Add(CreateParameter("@sync_type", sync_type));
			parameters.Add(CreateParameter("@hide", hide));
			parameters.Add(CreateParameter("@fld_order", fld_order));
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder", id_folder));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectMaxFolderOrder(int id_acct, long id_parent)
		{
			string commandText = string.Format(@"SELECT MAX(fld_order) FROM {0}
			WHERE id_parent=? AND id_acct=?",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders)); // 2

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_parent", id_parent));
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectAwmMessages(int id_acct, int[] id_msgs, long id_folder_db)
		{
			string strIn = NumberArrayToString(id_msgs);
			string commandText = string.Format(@"SELECT * FROM {0}
			WHERE id_acct=? AND id_folder_db=? AND id_msg IN ({1}) ORDER BY id_msg ASC",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
			strIn);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder_db", id_folder_db));

			return PrepareCommand(commandText, parameters);
		}

        public override IDbCommand SelectAwmMessagesWithBodies(int id_acct, int[] id_msgs, long id_folder_db)
		{
			string strIn = NumberArrayToString(id_msgs);
			string commandText = string.Format(@"SELECT message.id, message.id_msg, message.id_acct, 
message.id_folder_srv, message.id_folder_db, message.str_uid, message.int_uid, message.from_msg, message.to_msg, 
message.cc_msg, message.bcc_msg, message.subject, message.msg_date, message.attachments, message.size, 
message.seen, message.flagged, message.priority, message.downloaded, message.x_spam, message.rtl, 
message.deleted, message.is_full, message.replied, message.forwarded, message.flags, message.body_text, 
message.grayed, message.charset, message_body.msg AS body FROM {0} AS message 
LEFT JOIN {1} AS message_body ON (message.id_msg = message_body.id_msg)
WHERE message.id_acct=? AND message.id_folder_db=? AND message.id_msg IN ({2}) ORDER BY id_msg ASC",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages_body),
				strIn);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder_db", id_folder_db));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectAwmMessages(int id_acct, int id_msg, long id_folder_db)
		{
			string commandText = string.Format(@"SELECT * FROM {0}
			WHERE id_acct=? AND id_folder_db=? AND id_msg=? ORDER BY id_msg ASC",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder_db", id_folder_db));
			parameters.Add(CreateParameter("@id_msg", id_msg));

			return PrepareCommand(commandText, parameters);
		}

        public override IDbCommand SelectAwmMessagesIntUids(int id_acct, Folder folder, string order, bool asc)
        {
            string commandText = string.Format(@"SELECT int_uid FROM {0} 
WHERE id_acct=? AND id_folder_db=? ORDER BY {1} {2}",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
                order,
                (asc) ? "ASC" : "DESC");

            ArrayList parameters = new ArrayList();
            parameters.Add(CreateParameter("@id_acct", id_acct));
            parameters.Add(CreateParameter("@id_folder_db", folder.ID));

            return PrepareCommand(commandText, parameters);
        }

        public override IDbCommand SelectAwmMessagesBody(int id_acct, int id_msg)
		{
			string commandText = string.Format(@"SELECT msg
			FROM {0}
			WHERE id_acct=? AND id_msg=?",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages_body));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_msg", id_msg));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectAddressBookContactsCount(int id_user, string look_for, int look_for_type)
		{
			string lookForSearchCondition = string.Empty;
			if (!string.IsNullOrEmpty(look_for))
			{
				lookForSearchCondition = string.Format(@" AND (fullname LIKE '{0}' OR h_email LIKE '{0}' OR b_email LIKE '{0}' OR other_email LIKE '{0}')", (look_for_type == 0) ? "%" + EscapeWildcardCharacters(EncodeQuotes(look_for)) + "%" : EscapeWildcardCharacters(EncodeQuotes(look_for)) + "%");
			}

			string commandText = string.Format(@"
SELECT COUNT(*) FROM {0} WHERE id_user=?{1}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_book),
				lookForSearchCondition);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectAddressBookGroupsCount(int id_user, string look_for, int look_for_type)
		{
			string lookForSearchCondition = string.Empty;
			if (!string.IsNullOrEmpty(look_for))
			{
				lookForSearchCondition = string.Format(@" AND (group_nm LIKE '{0}' OR email LIKE '{0}')", (look_for_type == 0) ? "%" + EscapeWildcardCharacters(EncodeQuotes(look_for)) + "%" : EscapeWildcardCharacters(EncodeQuotes(look_for)) + "%");
			}

			string commandText = string.Format(@"
SELECT COUNT(*) FROM {0} WHERE id_user=?{1}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups),
				lookForSearchCondition);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand InsertIntoAwmAddrBook(int id_user, string h_email, string fullname, string notes, bool use_friendly_nm, string h_street, string h_city, string h_state, string h_zip, string h_country, string h_phone, string h_fax, string h_mobile, string h_web, string b_email, string b_company, string b_street, string b_city, string b_state, string b_zip, string b_country, string b_job_title, string b_department, string b_office, string b_phone, string b_fax, string b_web, byte birthday_day, byte birthday_month, short birthday_year, string other_email, short primary_email, long id_addr_prev, bool tmp, int use_frequency, bool auto_create, string str_id, DateTime date_modified)
		{
			string commandText = string.Format(@"INSERT INTO {0}(id_user, h_email, fullname, notes,
			use_friendly_nm, h_street, h_city, h_state, h_zip, h_country, h_phone, h_fax, h_mobile,
			h_web, b_email, b_company, b_street, b_city, b_state, b_zip, b_country, b_job_title,
			b_department, b_office, b_phone, b_fax, b_web, birthday_day, birthday_month, birthday_year,
			other_email, primary_email, id_addr_prev, tmp, use_frequency, auto_create, str_id, date_modified)
			VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?);",
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
			string commandText = string.Format(@"INSERT INTO {0}(id_user, group_nm, phone, fax, web, organization, use_frequency, email, company, street, city, state, zip, country, str_id)
			VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?);",
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

		public override IDbCommand SelectAwmAddrGroups(int id_user, int id_group)
		{
			string commandText = string.Format(@"SELECT * FROM {0}
			WHERE id_user=? AND id_group=?",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));
			parameters.Add(CreateParameter("@id_group", id_group));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectAwmAddrGroups(int id_user, long id_addr)
		{
			string commandText = string.Format(@"SELECT * FROM {0} AS groups_contacts
			INNER JOIN
			{1} AS groups
			ON groups_contacts.id_group=groups.id_group
			WHERE groups_contacts.id_addr=? AND groups.id_user=?",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups_contacts),
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_addr", id_addr));
			parameters.Add(CreateParameter("@id_user", id_user));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectAwmAddrGroups(int id_user)
		{
			string commandText = string.Format(@"SELECT * FROM {0}
			WHERE id_user=?",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand InsertIntoAwmAddrGroupsContacts(long id_addr, int id_group)
		{
			string commandText = string.Format(@"INSERT INTO {0}(id_addr, id_group)
			VALUES(?, ?)",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups_contacts));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_addr", id_addr));
			parameters.Add(CreateParameter("@id_group", id_group));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand DeleteFromAwmAccounts(int id_acct)
		{
			string commandText = string.Format(@"DELETE FROM {0}
			WHERE id_acct=?",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_accounts));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand DeleteFromAwmAddrBook(int id_user, long[] id_addrs)
		{
			string strIn = NumberArrayToString(id_addrs);
			string commandText = string.Format(@"DELETE FROM {0}
			WHERE id_user=? AND id_addr IN ({1})",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_book),
			strIn);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand DeleteFromAwmAddrGroupContacts(long[] id_addrs)
		{
			string strIn = NumberArrayToString(id_addrs);
			string commandText = string.Format(@"DELETE FROM {0}
			WHERE id_addr IN ({1})",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups_contacts),
			strIn);
			
			return PrepareCommand(commandText, null);
		}

		public override IDbCommand DeleteFromAwmAddrGroupContacts(int[] id_groups)
		{
			string strIn = NumberArrayToString(id_groups);
			string commandText = string.Format(@"DELETE FROM {0}
			WHERE id_group IN ({1})",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups_contacts),
			strIn);

			return PrepareCommand(commandText, null);
		}

		public override IDbCommand DeleteFromAwmAddrGroupContacts(long id_addr)
		{
			string commandText = string.Format(@"DELETE FROM {0}
			WHERE id_addr=?",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups_contacts));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_addr", id_addr));
			
			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand DeleteFromAwmAddrGroupContacts(int id_group)
		{
			string commandText = string.Format(@"DELETE FROM {0}
			WHERE id_group=?",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups_contacts));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_group", id_group));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand DeleteFromAwmAddrGroupContacts(int id_group, long[] id_addrs)
		{
			string strIn = NumberArrayToString(id_addrs);
			string commandText = string.Format(@"DELETE FROM {0}
			WHERE id_group=? AND id_addr IN ({1})",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups_contacts),
			strIn);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_group", id_group));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand DeleteFromAwmAddrBook(int id_user)
		{
			string commandText = string.Format(@"DELETE FROM {0}
			WHERE id_user=?",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_book));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));
			
			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand DeleteFromAwmAddrGroups(int id_user, int id_group)
		{
			string commandText = string.Format(@"DELETE FROM {0}
			WHERE id_user=? AND id_group=?",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));
			parameters.Add(CreateParameter("@id_group", id_group));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand DeleteFromAwmFilters(int id_acct, int id_filter, long id_folder)
		{
			string commandText = string.Format(@"DELETE FROM {0}
			WHERE id_acct=?{1}{2}",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_filters),
			(id_filter > 0) ? string.Format(" AND id_filter={0}", id_filter): string.Empty,
			(id_folder > 0) ? string.Format(" AND id_folder={0}", id_folder): string.Empty);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand DeleteFromAwmFolders(int id_acct)
		{
			string commandText = string.Format(@"DELETE FROM {0} WHERE id_acct=?",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand DeleteFromAwmFolders(int id_acct, long id_folder)
		{
			string commandText = string.Format(@"DELETE FROM {0} WHERE id_folder=? AND id_acct=?",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders),
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders_tree));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_folder", id_folder));
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder", id_folder));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand DeleteFromAwmFoldersTree(int id_acct)
		{
			string commandText = string.Format(@"DELETE FROM {0} WHERE id_folder
			IN (SELECT id_folder FROM {1} WHERE id_acct=?)",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders_tree),
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand DeleteFromAwmFoldersTree(long id_folder)
		{
			string commandText = string.Format(@"DELETE FROM {0} WHERE id_folder=?",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders_tree));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_folder", id_folder));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand DeleteFromAwmMessages(int id_acct)
		{
			string commandText = string.Format(@"DELETE FROM {0}
			WHERE id_acct=?",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			
			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand DeleteFromAwmMessages(int id_acct, long id_folder_db)
		{
			string commandText = string.Format(@"DELETE FROM {0}
			WHERE id_acct=? AND id_folder_db=?",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder_db", id_folder_db));
			
			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand DeleteFromAwmMessages(int id_acct, int[] id_msgs)
		{
			string strIn = NumberArrayToString(id_msgs);
			
			string commandText = string.Format(@"DELETE FROM {0}
			WHERE id_acct=? AND id_msg IN ({1})",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
			strIn);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectSumSizesOfRemainMessages(int id_acct)
		{
			string commandText = string.Format(@"SELECT SUM(size) FROM {0}
			WHERE id_acct=?",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand DeleteFromAwmMessagesBody(int id_acct)
		{
			string commandText = string.Format(@"DELETE FROM {0}
			WHERE id_acct=?",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages_body));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			
			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand DeleteFromAwmMessagesBody(int id_acct, int[] id_msg_array)
		{
			string strIn = NumberArrayToString(id_msg_array);
			
			string commandText = string.Format(@"DELETE FROM {0}
			WHERE id_acct=? AND id_msg IN ({1})",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages_body),
			strIn);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand DeleteFromAwmReads(int id_acct)
		{
			string commandText = string.Format(@"DELETE FROM {0}
			WHERE id_acct=?",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_reads));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			
			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectAwmReads(int id_acct)
		{
			string commandText = string.Format(@"SELECT str_uid FROM {0}
			WHERE id_acct=? ORDER BY id_read",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_reads));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectAwmColumns(int id_user) {
			string commandText = string.Format(@"SELECT * FROM {0} WHERE id_user={1}",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_columns),
				id_user);

			return PrepareCommand(commandText, null);
		}

		public override IDbCommand SelectAwmSendersSafety(int id_user, string email)
		{
			string commandText = string.Format(@"SELECT safety FROM {0}
WHERE id_user=? AND email LIKE '{1}'",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_senders),
				EncodeQuotes(email));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand InsertIntoAwmReads(int id_acct, string str_uid)
		{
			string commandText = string.Format(@"INSERT INTO {0} (id_acct, str_uid, tmp)
			VALUES (?, ?, 0)",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_reads));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@str_uid", str_uid));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectAwmAddrBook(int id_user, long id_addr)
		{
			string commandText = string.Format(@"SELECT * FROM {0}
			WHERE id_user=? AND id_addr=?",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_book));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));
			parameters.Add(CreateParameter("@id_addr", id_addr));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectAwmAddrBook(int id_user, int id_group)
		{
			string commandText = string.Format(@"SELECT * FROM {0} AS contacts
			INNER JOIN
			{1} AS groups_contacts
			ON groups_contacts.id_addr=contacts.id_addr
			WHERE groups_contacts.id_group=? AND contacts.id_user=?",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_book),
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_groups_contacts));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_group", id_group));
			parameters.Add(CreateParameter("@id_user", id_user));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectAwmAddrBookByEmail(int id_user, string email)
		{
			string commandText = string.Format(@"SELECT * FROM {0}
WHERE id_user=? AND (h_email LIKE '{1}' OR b_email LIKE '{1}' OR other_email LIKE '{1}')",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_book),
				EncodeQuotes(email));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));

			return PrepareCommand(commandText, parameters);
		}
		
		public override IDbCommand SelectAwmAddrBook(int id_user, string email)
		{
			string commandText = string.Format(@"SELECT * FROM {0}
WHERE id_user=? AND (h_email LIKE '{1}' OR b_email LIKE '{1}' OR other_email LIKE '{1}')",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_book),
				EncodeQuotes(email));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));

			return PrepareCommand(commandText, parameters);
		}
		
		public override IDbCommand UpdateAwmAddrBook(long id_addr, int id_user, string h_email, string fullname, string notes, bool use_friendly_nm, string h_street, string h_city, string h_state, string h_zip, string h_country, string h_phone, string h_fax, string h_mobile, string h_web, string b_email, string b_company, string b_street, string b_city, string b_state, string b_zip, string b_country, string b_job_title, string b_department, string b_office, string b_phone, string b_fax, string b_web, byte birthday_day, byte birthday_month, short birthday_year, string other_email, short primary_email, long id_addr_prev, bool tmp, int use_frequency, bool auto_create, string str_id, DateTime date_modified)
		{
			string commandText = string.Format(@"UPDATE {0}
			SET id_user=?, h_email=?, fullname=?, notes=?, use_friendly_nm=?,
			h_street=?, h_city=?, h_state=?, h_zip=?, h_country=?,
			h_phone=?, h_fax=?, h_mobile=?, h_web=?, b_email=?,
			b_company=?, b_street=?, b_city=?, b_state=?, b_zip=?,
			b_country=?, b_job_title=?, b_department=?, b_office=?,
			b_phone=?, b_fax=?, b_web=?, birthday_day=?,
			birthday_month=?, birthday_year=?, other_email=?,
			primary_email=?, id_addr_prev=?, tmp=?, use_frequency=?,
			auto_create=?,{1} date_modified=?
			WHERE id_addr=?",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_addr_book),
			(!string.IsNullOrEmpty(str_id)) ? " str_id=?," : string.Empty);

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
			if (!string.IsNullOrEmpty(str_id)) parameters.Add(CreateParameter("@str_id", (str_id.Length > 100) ? str_id.Substring(0, 100) : str_id));
			parameters.Add(CreateParameter("@date_modified", (date_modified > Constants.MinDate) ? date_modified : DateTime.Now.ToUniversalTime()));
            parameters.Add(CreateParameter("@id_addr", id_addr));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectAwmMessagesOlderThanXDays(int id_acct, long id_folder, int daysCount)
		{
			string commandText = string.Format(@"SELECT * FROM {0}
			WHERE id_acct=? AND id_folder_db=? AND DATEDIFF(CURDATE(), msg_date) > {1}",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
			daysCount.ToString(CultureInfo.InvariantCulture));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder", id_folder));

			return PrepareCommand(commandText, parameters);
		}

        public override IDbCommand CreateDatabase(string databaseName)
        {
            string commandText = string.Format(@"CREATE DATABASE {0} /*!40101 DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci */", databaseName);
            return PrepareCommand(commandText, null);
        }

        public override IDbCommand CreateTable(string name, string tablePrefix)
        {
            WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
            tablePrefix = EncodeQuotes(tablePrefix);

            string commandText = string.Empty;
            switch (name)
            {
                case "a_test":
                    commandText = string.Format(@"
CREATE TABLE `{0}a_test` (
   `id` int(11) NOT NULL auto_increment,
   PRIMARY KEY  (`id`)
)", tablePrefix);
                    break;
                case Constants.TablesNames.a_users:
                    commandText = string.Format(@"
CREATE TABLE `{0}{1}` (
  `id_user` int(11) NOT NULL auto_increment,
  `deleted` tinyint(1) NOT NULL default 0,
  PRIMARY KEY  (`id_user`)
) /*!40101 DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci */", tablePrefix, Constants.TablesNames.a_users);
                    break;
                case Constants.TablesNames.awm_accounts:
                    commandText = string.Format(@"
CREATE TABLE `{0}{1}` (
  `id_acct` int(11) NOT NULL auto_increment,
  `id_user` int(11) NOT NULL default 0,
  `id_domain` int(11) NOT NULL default 0,
  `def_acct` tinyint(1) NOT NULL default 0,
  `deleted` tinyint(1) NOT NULL default 0,
  `email` varchar(255) NOT NULL default '',
  `mail_protocol` tinyint(1) NOT NULL default 0,
  `mail_inc_host` varchar(255) default NULL,
  `mail_inc_login` varchar(255) default NULL,
  `mail_inc_pass` varchar(255) default NULL,
  `mail_inc_port` int(11) NOT NULL default 110,
  `mail_out_host` varchar(255) default NULL,
  `mail_out_login` varchar(255) default NULL,
  `mail_out_pass` varchar(255) default NULL,
  `mail_out_port` int(11) NOT NULL default 25,
  `mail_out_auth` tinyint(1) NOT NULL default 1,
  `friendly_nm` varchar(200) default NULL,
  `use_friendly_nm` tinyint(1) NOT NULL default 1,
  `def_order` tinyint(4) NOT NULL default 0,
  `getmail_at_login` tinyint(1) NOT NULL default 0,
  `mail_mode` tinyint(4) NOT NULL default 1,
  `mails_on_server_days` smallint(6) NOT NULL,
  `signature` text,
  `signature_type` tinyint(4) NOT NULL default 1,
  `signature_opt` tinyint(4) NOT NULL default 0,
  `delimiter` char(1) NOT NULL default '/',
  `mailbox_size` bigint(20) NOT NULL default 0,
  `mailing_list` tinyint(1) NOT NULL default 0,
  `imap_quota` tinyint(1) NOT NULL default -1,
  `namespace` varchar(50) character set utf8 NOT NULL default '',
  PRIMARY KEY  (`id_acct`)
) /*!40101 DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci */", tablePrefix, Constants.TablesNames.awm_accounts);
                    break;
                case Constants.TablesNames.awm_domains:
                    commandText = string.Format(@"
CREATE TABLE `{0}awm_domains` (
	`id_domain` int(11) NOT NULL auto_increment,
	`name` varchar(255) NOT NULL,
	`mail_protocol` tinyint(1) NOT NULL default 0,
	`mail_inc_host` varchar(255) NULL,
	`mail_inc_port` int(11) NOT NULL default 110,
	`mail_out_host` varchar(255) NULL,
	`mail_out_port` int(11) NOT NULL default 25,
	`mail_out_auth` tinyint(1) NOT NULL default 0,

	`url` varchar(255) NULL,
	`site_name` varchar(255) NULL,
	`settings_mail_protocol` tinyint(1) NULL,
	`settings_mail_inc_host` varchar(255) NULL,
	`settings_mail_inc_port` int(11) NULL,
	`settings_mail_out_host` varchar(255) NULL,
	`settings_mail_out_port` int(11) NULL,
	`settings_mail_out_auth` tinyint(1) NULL,
	`allow_direct_mode` tinyint(1) NULL,
	`direct_mode_id_def` tinyint(1) NULL,
	`attachment_size_limit` bigint(20) NULL,
	`allow_attachment_limit` tinyint(1) NULL,
	`mailbox_size_limit` bigint(20) NULL,
	`allow_mailbox_limit` tinyint(1) NULL,
	`take_quota` tinyint(1) NULL,
	`allow_new_users_change_settings` tinyint(1) NULL,
	`allow_auto_reg_on_login` tinyint(1) NULL,
	`allow_users_add_accounts` tinyint(1) NULL,
	`allow_users_change_account_def` tinyint(1) NULL,
	`def_user_charset` int(11) NULL,
	`allow_users_change_charset` tinyint(1) NULL,
	`def_user_timezone` int(11) NULL,
	`allow_users_change_timezone` tinyint(1) NULL,

	`msgs_per_page` smallint(6) NULL,
	`skin` varchar(50) NULL,
	`allow_users_change_skin` tinyint(1) NULL,
	`lang` varchar(50) NULL,
	`allow_users_change_lang` tinyint(1) NULL,
	`show_text_labels` tinyint(1) NULL,
	`allow_ajax` tinyint(1) NULL,
	`allow_editor` tinyint(1) NULL,
	`allow_contacts` tinyint(1) NULL,
	`allow_calendar` tinyint(1) NULL,

	`hide_login_mode` tinyint(1) NULL,
	`domain_to_use` varchar(255) NULL,
	`allow_choosing_lang` tinyint(1) NULL,
	`allow_advanced_login` tinyint(1) NULL,
	`allow_auto_detect_and_correct` tinyint(1) NULL,
    `global_addr_book` tinyint(1) NOT NULL default 0,
    `view_mode` tinyint(4) NOT NULL default 1,
    `save_mail` tinyint(4) NOT NULL default 0,
	PRIMARY KEY  (`id_domain`)
) /*!40101 DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci */", tablePrefix);
                    break;
                case Constants.TablesNames.awm_addr_book:
                    commandText = string.Format(@"
CREATE TABLE `{0}{1}` (
  `id_addr` bigint(20) NOT NULL auto_increment,
  `id_user` int(11) NOT NULL default 0,
  `deleted` tinyint(1) NOT NULL default 0,
  `date_created` datetime NULL,
  `date_modified` datetime NULL,
  `h_email` varchar(255) default NULL,
  `fullname` varchar(255) default NULL,
  `notes` varchar(255) default NULL,
  `use_friendly_nm` tinyint(1) NOT NULL default 1,
  `h_street` varchar(255) default NULL,
  `h_city` varchar(200) default NULL,
  `h_state` varchar(200) default NULL,
  `h_zip` varchar(10) default NULL,
  `h_country` varchar(200) default NULL,
  `h_phone` varchar(50) default NULL,
  `h_fax` varchar(50) default NULL,
  `h_mobile` varchar(50) default NULL,
  `h_web` varchar(255) default NULL,
  `b_email` varchar(255) default NULL,
  `b_company` varchar(200) default NULL,
  `b_street` varchar(255) default NULL,
  `b_city` varchar(200) default NULL,
  `b_state` varchar(200) default NULL,
  `b_zip` varchar(10) default NULL,
  `b_country` varchar(200) default NULL,
  `b_job_title` varchar(100) default NULL,
  `b_department` varchar(200) default NULL,
  `b_office` varchar(200) default NULL,
  `b_phone` varchar(50) default NULL,
  `b_fax` varchar(50) default NULL,
  `b_web` varchar(255) default NULL,
  `other_email` varchar(255) default NULL,
  `primary_email` tinyint(4) default NULL,
  `id_addr_prev` bigint(20) NOT NULL default 0,
  `tmp` tinyint(1) NOT NULL default 0,
  `use_frequency` int(11) NOT NULL default 0,
  `auto_create` tinyint(1) NOT NULL default 0,
  `birthday_day` tinyint(4) NOT NULL default 0,
  `birthday_month` tinyint(4) NOT NULL default 0,
  `birthday_year` smallint(6) NOT NULL default 0,
  `str_id` varchar(255) default NULL,
  PRIMARY KEY  (`id_addr`)
) /*!40101 DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci */", tablePrefix, Constants.TablesNames.awm_addr_book);
                    break;
                case Constants.TablesNames.awm_addr_groups:
                    commandText = string.Format(@"
CREATE TABLE `{0}{1}` (
  `id_group` int(11) NOT NULL auto_increment,
  `id_user` int(11) NOT NULL default 0,
  `group_nm` varchar(255) default NULL,
  `use_frequency` int(11) NOT NULL default 0,
  `email` varchar(255) default NULL,
  `company` varchar(200) default NULL,
  `street` varchar(255) default NULL,
  `city` varchar(200) default NULL,
  `state` varchar(200) default NULL,
  `zip` varchar(10) default NULL,
  `country` varchar(200) default NULL,
  `phone` varchar(50) default NULL,
  `fax` varchar(50) default NULL,
  `web` varchar(255) default NULL,
  `organization` tinyint(1) NOT NULL default 0,
  `str_id` varchar(255) default NULL,
  PRIMARY KEY  (`id_group`)
) /*!40101 DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci */", tablePrefix, Constants.TablesNames.awm_addr_groups);
                    break;
                case Constants.TablesNames.awm_addr_groups_contacts:
                    commandText = string.Format(@"
CREATE TABLE `{0}{1}` (
  `id_addr` bigint(20) NOT NULL default 0,
  `id_group` int(11) NOT NULL default 0
) /*!40101 DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci */", tablePrefix, Constants.TablesNames.awm_addr_groups_contacts);
                    break;
                case Constants.TablesNames.awm_columns:
                    commandText = string.Format(@"
CREATE TABLE `{0}{1}` (
  `id` int(11) NOT NULL auto_increment,
  `id_column` int(11) NOT NULL default 0,
  `id_user` int(11) NOT NULL default 0,
  `column_value` int(11) NOT NULL default 0,
  PRIMARY KEY  (`id`)
) /*!40101 DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci */", tablePrefix, Constants.TablesNames.awm_columns);
                    break;
                case Constants.TablesNames.awm_filters:
                    commandText = string.Format(@"
CREATE TABLE `{0}{1}` (
  `id_filter` int(11) NOT NULL auto_increment,
  `id_acct` int(11) NOT NULL default 0,
  `field` tinyint(4) NOT NULL default 0,
  `condition` tinyint(4) NOT NULL default 0,
  `filter` varchar(255) default NULL,
  `action` tinyint(4) NOT NULL default 0,
  `id_folder` bigint(20) NOT NULL default 0,
  `applied` tinyint(1) NOT NULL default 1,
  PRIMARY KEY  (`id_filter`)
) /*!40101 DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci */", tablePrefix, Constants.TablesNames.awm_filters);
                    break;
                case Constants.TablesNames.awm_folders:
                    commandText = string.Format(@"
CREATE TABLE `{0}{1}` (
  `id_folder` bigint(20) NOT NULL auto_increment,
  `id_acct` int(11) NOT NULL default 0,
  `id_parent` bigint(20) NOT NULL default 0,
  `type` smallint(6) NOT NULL default 0,
  `name` varchar(100) default NULL,
  `full_path` varchar(255) default NULL,
  `sync_type` tinyint(4) NOT NULL default 0,
  `hide` tinyint(1) NOT NULL default 0,
  `fld_order` smallint(6) NOT NULL default 1,
  PRIMARY KEY  (`id_folder`)
) /*!40101 DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci */", tablePrefix, Constants.TablesNames.awm_folders);
                    break;
                case Constants.TablesNames.awm_folders_tree:
                    commandText = string.Format(@"
CREATE TABLE `{0}{1}` (
  `id` int(11) NOT NULL auto_increment,
  `id_folder` bigint(20) NOT NULL default 0,
  `id_parent` bigint(20) NOT NULL default 0,
  `folder_level` tinyint(4) NOT NULL default 0,
  PRIMARY KEY  (`id`)
) /*!40101 DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci */", tablePrefix, Constants.TablesNames.awm_folders_tree);
                    break;
                case Constants.TablesNames.awm_messages:
                    commandText = string.Format(@"
CREATE TABLE `{0}{1}` (
  `id` bigint(20) NOT NULL auto_increment PRIMARY KEY,
  `id_msg` int(11) NOT NULL default 0,
  `id_acct` int(11) NOT NULL default 0,
  `id_folder_srv` bigint(20) NOT NULL,
  `id_folder_db` bigint(20) NOT NULL,
  `str_uid` varchar(255) default NULL,
  `int_uid` bigint(20) NOT NULL default 0,
  `from_msg` varchar(255) default NULL,
  `to_msg` varchar(255) default NULL,
  `cc_msg` varchar(255) default NULL,
  `bcc_msg` varchar(255) default NULL,
  `subject` varchar(255) default NULL,
  `msg_date` datetime default NULL,
  `attachments` tinyint(1) NOT NULL default 0,
  `size` bigint(20) NOT NULL,
  `seen` tinyint(1) NOT NULL default 1,
  `flagged` tinyint(1) NOT NULL default 0,
  `priority` tinyint(4) NOT NULL default 3,
  `sensitivity` tinyint(4) NOT NULL default 0,
  `downloaded` tinyint(1) NOT NULL default 1,
  `x_spam` tinyint(1) NOT NULL default 0,
  `rtl` tinyint(1) NOT NULL default 0,
  `deleted` tinyint(1) NOT NULL default 0,
  `is_full` tinyint(1) default 1,
  `replied` tinyint(1) default NULL,
  `forwarded` tinyint(1) default NULL,
  `flags` int(11) default NULL,
  `body_text` mediumtext,
  `grayed` tinyint(1) default 0 NOT NULL,
  `charset` int(11) NOT NULL default -1
) /*!40101 DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci */", tablePrefix, Constants.TablesNames.awm_messages);
                    break;
                case Constants.TablesIndexes.awm_messages_index:
                    commandText = string.Format(@"
					CREATE INDEX {0}{1} ON
					 {0}awm_messages(id_acct, id_msg
					)", tablePrefix, Constants.TablesIndexes.awm_messages_index);
                    break;
                case Constants.TablesNames.awm_messages_body:
                    commandText = string.Format(@"
CREATE TABLE `{0}{1}` (
  `id` bigint(20) NOT NULL auto_increment PRIMARY KEY,
  `id_msg` bigint(20) NOT NULL default 0,
  `id_acct` int(11) NOT NULL default 0,
  `msg` longblob
) /*!40101 DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci */", tablePrefix, Constants.TablesNames.awm_messages_body);
                    break;
                case Constants.TablesIndexes.awm_messages_body_index:
                    commandText = string.Format(@"
					CREATE INDEX {0}{1} ON
					 {0}awm_messages_body(id_acct, id_msg
					)", tablePrefix, Constants.TablesIndexes.awm_messages_body_index);
                    break;
                case Constants.TablesNames.awm_reads:
                    commandText = string.Format(@"
CREATE TABLE `{0}{1}` (
  `id_read` bigint(20) NOT NULL auto_increment,
  `id_acct` int(11) NOT NULL default 0,
  `str_uid` varchar(255) default NULL,
  `tmp` tinyint(1) NOT NULL default 0,
  PRIMARY KEY  (`id_read`)
) /*!40101 DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci */", tablePrefix, Constants.TablesNames.awm_reads);
                    break;
                case Constants.TablesNames.awm_senders:
                    commandText = string.Format(@"
CREATE TABLE `{0}{1}` (
  `id` int(11) NOT NULL auto_increment,
  `id_user` int(11) NOT NULL default 0,
  `email` varchar(255) NOT NULL,
  `safety`  tinyint(4) NOT NULL default 0,
  PRIMARY KEY  (`id`)
) /*!40101 DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci */", tablePrefix, Constants.TablesNames.awm_senders);
                    break;
                case Constants.TablesNames.awm_settings:
                    commandText = string.Format(@"
CREATE TABLE `{0}{3}` (
  `id_setting` int(11) NOT NULL auto_increment,
  `id_user` int(11) NOT NULL default 0,
  `msgs_per_page` smallint(6) NOT NULL default 20,
  `white_listing` tinyint(1) NOT NULL default 0,
  `x_spam` tinyint(1) NOT NULL default 0,
  `last_login` datetime default NULL,
  `logins_count` int(11) NOT NULL default 0,
  `def_skin` varchar(255) NOT NULL default '{1}',
  `def_lang` varchar(50) default NULL,
  `def_charset_inc` int(11) NOT NULL default 1250,
  `def_charset_out` int(11) NOT NULL default 1250,
  `def_timezone` smallint(6) NOT NULL default 0,
  `def_date_fmt` varchar(20) NOT NULL default '{2}',
  `hide_folders` tinyint(1) NOT NULL default 0,
  `mailbox_limit` bigint(20) NOT NULL default 1000000000,
  `allow_change_settings` tinyint(1) NOT NULL default 1,
  `allow_dhtml_editor` tinyint(1) NOT NULL default 1,
  `allow_direct_mode` tinyint(1) NOT NULL default 1,
  `hide_contacts` tinyint(1) NOT NULL default 0,
  `db_charset` int(11) NOT NULL default 65001,
  `horiz_resizer` smallint(6) NOT NULL default 150,
  `vert_resizer` smallint(6) NOT NULL default 115,
  `mark` tinyint(4) NOT NULL default 0,
  `reply` tinyint(4) NOT NULL default 0,
  `contacts_per_page` smallint(6) NOT NULL default 20,
  `view_mode` tinyint(4) NOT NULL default 1,
  `auto_checkmail_interval` int(11) NOT NULL default 0, 
  PRIMARY KEY  (`id_setting`),
  UNIQUE KEY `id_user` (`id_user`)
) /*!40101 DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci */",
                        tablePrefix,
                        EncodeQuotes(settings.DefaultSkin),
                        EncodeQuotes(Constants.DateFormats.Default),
                        Constants.TablesNames.awm_settings);
                    break;
                case Constants.TablesNames.awm_temp:
                    commandText = string.Format(@"
CREATE TABLE `{0}{1}` (
  `id_temp` bigint(20) NOT NULL auto_increment,
  `id_acct` int(11) NOT NULL default 0,
  `data_val` text,
  PRIMARY KEY  (`id_temp`)
) /*!40101 DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci */", tablePrefix, Constants.TablesNames.awm_temp);
                    break;
                case Constants.TablesNames.awm_subadmins:
                    commandText = string.Format(@"
CREATE TABLE `{0}awm_subadmins` (
	`id_admin` int(11) NOT NULL auto_increment,
	`login` varchar(255) default NULL,
	`password` varchar(255) default NULL,
	`description` varchar(255) default NULL,
  PRIMARY KEY  (`id_admin`)
) /*!40101 DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci */", tablePrefix);
                    break;
                case Constants.TablesNames.awm_subadmin_domains:
                    commandText = string.Format(@"
CREATE TABLE `{0}awm_subadmin_domains` (
	`id` int(11) NOT NULL auto_increment,
	`id_admin` int(11) NOT NULL default 0,
	`id_domain` int(11) NOT NULL default 0,
  PRIMARY KEY  (`id`)
) /*!40101 DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci */", tablePrefix); break;

                #region CreateCalendarTablesCommands
                case Constants.TablesNames.acal_calendars:
                    commandText = string.Format(@"
CREATE TABLE `{0}{1}` (
    `calendar_id` int(11) NOT NULL auto_increment,
    `user_id` int(11) NOT NULL default 0,
    `calendar_name` varchar(100) character set utf8 NOT NULL default '',
    `calendar_description` text character set utf8 default NULL,
    `calendar_color` int(11) NOT NULL default 0,
    `calendar_active` tinyint(1) NOT NULL default 0,
    `calendar_str_id` varchar(100) character set utf8 NOT NULL default '',
PRIMARY KEY  (`calendar_id`)) /*!40101 DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci */", tablePrefix, Constants.TablesNames.acal_calendars);
                    break;
                case Constants.TablesNames.acal_events:
                    commandText = string.Format(@"
CREATE TABLE `{0}{1}` (
    `event_id` int(11) NOT NULL auto_increment,
    `calendar_id` int(11) NOT NULL default 0,
    `event_timefrom` datetime default NULL,
    `event_timetill` datetime default NULL, 
    `event_allday` tinyint(1) NOT NULL default 0,
    `event_name` varchar(100) character set utf8 NOT NULL default '',
    `event_text` text,
    `event_priority` tinyint(4) NULL DEFAULT 0,
    `event_repeats` tinyint(1) NOT NULL DEFAULT 0,
    `event_str_id` varchar(200) character set utf8 NOT NULL default '',
    `event_last_modified` datetime default NULL,
	`event_owner_email` varchar(255) NOT NULL default '',
	`event_appointment_access` tinyint(4) NOT NULL default 0,
PRIMARY KEY  (`event_id`)) /*!40101 DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci */", tablePrefix, Constants.TablesNames.acal_events);
                    break;
                case Constants.TablesNames.acal_users_data:
                    commandText = string.Format(@"
CREATE TABLE `{0}{1}` (
    `settings_id` int(11) NOT NULL auto_increment,
    `user_id` int(11) NOT NULL default 0,
    `timeformat` tinyint(1) NOT NULL default 1,
    `dateformat` tinyint(1) NOT NULL default 1,
    `showweekends` tinyint(1) NOT NULL default 0,
    `workdaystarts` tinyint(2) NOT NULL default 0,
    `workdayends` tinyint(2) NOT NULL default 1,
    `showworkday` tinyint(1) NOT NULL default 0,
    `weekstartson` tinyint(1) NOT NULL default '0',
    `defaulttab` tinyint(1) NOT NULL default 1,
    `country` varchar(2) default NULL,
    `timezone` smallint(3) NULL,
    `alltimezones` tinyint(1) NOT NULL default 0,
	`reminders_web_url` varchar(255) NULL,
	`autoaddinvitation` tinyint(1) NOT NULL default 0,
PRIMARY KEY (`settings_id`)) /*!40101 DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci */", tablePrefix, Constants.TablesNames.acal_users_data);
                    break;
                case Constants.TablesNames.acal_sharing:
                    commandText = string.Format(@"
CREATE TABLE `{0}acal_sharing` (
     `id_share` int(11) NOT NULL auto_increment,
     `id_user` int(11) NOT NULL,
     `id_calendar` int(11) NOT NULL,
     `id_to_user` int(11) NOT NULL,
     `str_to_email` varchar(255) NOT NULL default '',
     `int_access_level` tinyint(4) NOT NULL default 2,
     `calendar_active` tinyint(1) NOT NULL DEFAULT 1,
     PRIMARY KEY  (`id_share`)) /*!40101 DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci */", tablePrefix);
                    break;
                case Constants.TablesNames.acal_publications:
                    commandText = string.Format(@"
CREATE TABLE `{0}acal_publications` (
     `id_publication` int(11) NOT NULL auto_increment,
     `id_user` int(11) NOT NULL,
     `id_calendar` int(11) NOT NULL,
     `str_md5` varchar(32) NOT NULL,
     `int_access_level` tinyint(4) NOT NULL default 1,
     `access_type` tinyint(4) NOT NULL default 0,
     PRIMARY KEY  (`id_publication`)) /*!40101 DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci */", tablePrefix);
                    break;
                case Constants.TablesNames.acal_eventrepeats:
                    commandText = string.Format(@"
CREATE TABLE  `{0}acal_eventrepeats` (
  `id_repeat` int(11) NOT NULL auto_increment,
  `id_event` int(11) NOT NULL,
  `repeat_period` tinyint(1) NOT NULL default 0,
  `repeat_order` tinyint(1) NOT NULL default 1,
  `repeat_num` int(11) NOT NULL default 0,
  `repeat_until` date default NULL,
  `week_number` tinyint(1) default NULL,
  `repeat_end` tinyint(1) NOT NULL default 0,
  `excluded` tinyint(1) NOT NULL default 0,
  `sun` tinyint(1) NOT NULL default 0,
  `mon` tinyint(1) NOT NULL default 0,
  `tue` tinyint(1) NOT NULL default 0,
  `wed` tinyint(1) NOT NULL default 0,
  `thu` tinyint(1) NOT NULL default 0,
  `fri` tinyint(1) NOT NULL default 0,
  `sat` tinyint(1) NOT NULL default 0,
  PRIMARY KEY  (`id_repeat`)) /*!40101 DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci */", tablePrefix);
                    break;
                case Constants.TablesNames.acal_exclusions:
                    commandText = string.Format(@"
CREATE TABLE  `{0}acal_exclusions` (
  `id_exclusion` int(11) NOT NULL auto_increment,
  `id_event` int(11) NOT NULL,
  `id_calendar` int(11) NOT NULL,
  `id_repeat` int(11) NOT NULL,
  `event_timefrom` datetime NOT NULL,
  `event_timetill` datetime NOT NULL,
  `event_name` varchar(100) NOT NULL,
  `event_text` text,
  `event_allday` tinyint(1) NOT NULL default 0,
  `is_deleted` tinyint(1) NOT NULL default 0,
  `id_recurrence_date` datetime default NULL,
  `event_last_modified` datetime default NULL,
  PRIMARY KEY  (`id_exclusion`)) /*!40101 DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci */", tablePrefix);
                    break;
                case Constants.TablesNames.acal_reminders:
                    commandText = string.Format(@"
CREATE TABLE `{0}acal_reminders` (
	`id_reminder` int(11) NOT NULL auto_increment,
	`id_event` int(11) NOT NULL,
	`id_user` int(11) NULL,
	`notice_type` tinyint(4) NOT NULL default 0,
	`remind_offset` int(11) NOT NULL default 0,
	`sent` int(11) NOT NULL default 0,
	PRIMARY KEY  (`id_reminder`)) /*!40101 DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci */", tablePrefix);
                    break;
                case Constants.TablesNames.acal_cron_runs:
                    commandText = string.Format(@"
CREATE TABLE `{0}acal_cron_runs` (
	`id_run` int(11) NOT NULL auto_increment,
	`run_date` datetime NOT NULL,
	`latest_date` datetime NOT NULL,
	PRIMARY KEY  (`id_run`)) /*!40101 DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci */", tablePrefix);
                    break;
                case Constants.TablesNames.acal_appointments:
                    commandText = string.Format(@"
CREATE TABLE `{0}acal_appointments` (
	`id_appointment` int(11) NOT NULL auto_increment,
	`id_event` int(11) NOT NULL,
	`id_user` int(11) NOT NULL default 0,
	`email` varchar(255) NOT NULL,
	`access_type` tinyint(4) NOT NULL default 0,
	`status` tinyint(4) NOT NULL default 0,
	`hash` varchar(32) NOT NULL,
	PRIMARY KEY  (`id_appointment`)) /*!40101 DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci */", tablePrefix);
                    break;
                #endregion
            }

            return PrepareCommand(commandText, null);
        }

        public override IDbCommand AlterTable(string name, string field, string tablePrefix)
        {
            string commandText = string.Empty;
            switch (name)
            {
                case Constants.TablesNames.awm_addr_book:
                    switch (field)
                    {
                        case "use_frequency":
                            commandText = string.Format(@"
                            ALTER TABLE {0}awm_addr_book 
                            ADD use_frequency INT(11) NOT NULL DEFAULT 0", tablePrefix);
                            break;
                        case "auto_create":
                            commandText = string.Format(@"
                            ALTER TABLE {0}awm_addr_book 
                            ADD auto_create TINYINT(1) NOT NULL DEFAULT 0", tablePrefix);
                            break;
                        // 4.9.x
                        case "str_id":
                            commandText = string.Format(@"
                            ALTER TABLE {0}awm_addr_book 
                            ADD str_id  VARCHAR(100) NULL", tablePrefix);
                            break;
                        case "deleted":
                            commandText = string.Format(@"
                            ALTER TABLE {0}awm_addr_book 
                            ADD deleted TINYINT(1) NOT NULL DEFAULT 0", tablePrefix);
                            break;
                        case "date_created":
                            commandText = string.Format(@"
                            ALTER TABLE {0}awm_addr_book 
                            ADD date_created datetime NULL", tablePrefix);
                            break;
                        case "date_modified":
                            commandText = string.Format(@"
                            ALTER TABLE {0}awm_addr_book 
                            ADD date_modified datetime NULL", tablePrefix);
                            break;
                    }
                    break;
                case Constants.TablesNames.awm_addr_groups:
                    switch (field)
                    {
                        case "email":
                            commandText = string.Format(@"
                            ALTER TABLE {0}awm_addr_groups 
                            ADD email VARCHAR(255) DEFAULT '' NOT NULL", tablePrefix);
                            break;
                        case "company":
                            commandText = string.Format(@"
                            ALTER TABLE {0}awm_addr_groups 
                            ADD company VARCHAR(200) DEFAULT '' NOT NULL", tablePrefix);
                            break;
                        case "street":
                            commandText = string.Format(@"
                            ALTER TABLE {0}awm_addr_groups 
                            ADD street VARCHAR(255) DEFAULT '' NOT NULL", tablePrefix);
                            break;
                        case "city":
                            commandText = string.Format(@"
                            ALTER TABLE {0}awm_addr_groups 
                            ADD city VARCHAR(200) DEFAULT '' NOT NULL", tablePrefix);
                            break;
                        case "state":
                            commandText = string.Format(@"
                            ALTER TABLE {0}awm_addr_groups 
                            ADD state VARCHAR(200) DEFAULT '' NOT NULL", tablePrefix);
                            break;
                        case "zip":
                            commandText = string.Format(@"
                            ALTER TABLE {0}awm_addr_groups 
                            ADD zip VARCHAR(10) DEFAULT '' NOT NULL", tablePrefix);
                            break;
                        case "country":
                            commandText = string.Format(@"
                            ALTER TABLE {0}awm_addr_groups 
                            ADD country VARCHAR(200) DEFAULT '' NOT NULL", tablePrefix);
                            break;
                        case "phone":
                            commandText = string.Format(@"
                            ALTER TABLE {0}awm_addr_groups 
                            ADD phone VARCHAR(50) DEFAULT '' NOT NULL", tablePrefix);
                            break;
                        case "fax":
                            commandText = string.Format(@"
                            ALTER TABLE {0}awm_addr_groups 
                            ADD fax VARCHAR(50) DEFAULT '' NOT NULL", tablePrefix);
                            break;
                        case "web":
                            commandText = string.Format(@"
                            ALTER TABLE {0}awm_addr_groups 
                            ADD web VARCHAR(255) DEFAULT '' NOT NULL", tablePrefix);
                            break;
                        case "organization":
                            commandText = string.Format(@"
                            ALTER TABLE {0}awm_addr_groups 
                            ADD organization tinyint(1) DEFAULT 0 NOT NULL", tablePrefix);
                            break;
                        case "use_frequency":
                            commandText = string.Format(@"
                            ALTER TABLE {0}awm_addr_groups 
                            ADD use_frequency int(11) DEFAULT 0 NOT NULL", tablePrefix);
                            break;
                        // 4.9.x
                        case "str_id":
                            commandText = string.Format(@"
                            ALTER TABLE {0}awm_addr_groups 
                            ADD str_id  VARCHAR(100) NULL", tablePrefix);
                            break;
                    }
                    break;
                case Constants.TablesNames.awm_accounts:
                    switch (field)
                    {
                        case "imap_quota":
                            commandText = string.Format(@"
                            ALTER TABLE {0}awm_accounts 
                            ADD imap_quota tinyint(1) DEFAULT -1 NOT NULL", tablePrefix);
                            break;
                        case "mailing_list":
                            commandText = string.Format(@"
                            ALTER TABLE {0}awm_accounts 
                            ADD mailing_list tinyint(1) DEFAULT 0 NOT NULL", tablePrefix);
                            break;
                        case "id_domain":
                            commandText = string.Format(@"
                            ALTER TABLE {0}awm_accounts 
                            ADD id_domain INT(11) DEFAULT 0 NOT NULL", tablePrefix);
                            break;
                        case "namespace":
                            commandText = string.Format(@"
                            ALTER TABLE {0}awm_accounts 
                            ADD namespace varchar(50) DEFAULT '' NOT NULL", tablePrefix);
                            break;
                    }
                    break;
				case Constants.TablesNames.awm_domains:
                    switch (field)
                    {
                        case "url":
                            commandText = string.Format(@"
                            ALTER TABLE {0}awm_domains 
                            ADD url varchar(255) NULL", tablePrefix);
                            break;
                        case "site_name":
                            commandText = string.Format(@"
                            ALTER TABLE {0}awm_domains 
                            ADD site_name varchar(255) NULL", tablePrefix);
                            break;
						case "settings_mail_protocol":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD settings_mail_protocol tinyint(1) NULL", tablePrefix);
							break;
						case "settings_mail_inc_host":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD settings_mail_inc_host varchar(255) NULL", tablePrefix);
							break;
						case "settings_mail_inc_port":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD settings_mail_inc_port int(11) NULL", tablePrefix);
							break;
						case "settings_mail_out_host":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD settings_mail_out_host varchar(255) NULL", tablePrefix);
							break;
						case "settings_mail_out_port":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD settings_mail_out_port int(11) NULL", tablePrefix);
							break;
						case "settings_mail_out_auth":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD settings_mail_out_auth tinyint(1) NULL", tablePrefix);
							break;
						case "allow_direct_mode":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD allow_direct_mode tinyint(1) NULL", tablePrefix);
							break;
						case "direct_mode_id_def":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD direct_mode_id_def tinyint(1) NULL", tablePrefix);
							break;
						case "attachment_size_limit":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD attachment_size_limit bigint(20) NULL", tablePrefix);
							break;
						case "allow_attachment_limit":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD allow_attachment_limit tinyint(1) NULL", tablePrefix);
							break;
						case "mailbox_size_limit":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD mailbox_size_limit bigint(20) NULL", tablePrefix);
							break;
						case "allow_mailbox_limit":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD allow_mailbox_limit tinyint(1) NULL", tablePrefix);
							break;
						case "take_quota":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD take_quota tinyint(1) NULL", tablePrefix);
							break;
						case "allow_new_users_change_settings":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD allow_new_users_change_settings tinyint(1) NULL", tablePrefix);
							break;
						case "allow_auto_reg_on_login":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD allow_auto_reg_on_login tinyint(1) NULL", tablePrefix);
							break;
						case "allow_users_add_accounts":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD allow_users_add_accounts tinyint(1) NULL", tablePrefix);
							break;
						case "allow_users_change_account_def":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD allow_users_change_account_def tinyint(1) NULL", tablePrefix);
							break;
						case "def_user_charset":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD def_user_charset int(11) NULL", tablePrefix);
							break;
						case "allow_users_change_charset":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD allow_users_change_charset tinyint(1) NULL", tablePrefix);
							break;
						case "def_user_timezone":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD def_user_timezone int(11) NULL", tablePrefix);
							break;
						case "allow_users_change_timezone":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD allow_users_change_timezone tinyint(1) NULL", tablePrefix);
							break;
						case "msgs_per_page":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD msgs_per_page smallint(6) NULL", tablePrefix);
							break;
						case "skin":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD skin varchar(50) NULL", tablePrefix);
							break;
						case "allow_users_change_skin":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD allow_users_change_skin tinyint(1) NULL", tablePrefix);
							break;
						case "lang":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD lang varchar(50) NULL", tablePrefix);
							break;
						case "allow_users_change_lang":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD allow_users_change_lang tinyint(1) NULL", tablePrefix);
							break;
						case "show_text_labels":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD show_text_labels tinyint(1) NULL", tablePrefix);
							break;
						case "allow_ajax":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD allow_ajax tinyint(1) NULL", tablePrefix);
							break;
						case "allow_editor":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD allow_editor tinyint(1) NULL", tablePrefix);
							break;
						case "allow_contacts":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD allow_contacts tinyint(1) NULL", tablePrefix);
							break;
						case "allow_calendar":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD allow_calendar tinyint(1) NULL", tablePrefix);
							break;
						case "hide_login_mode":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD hide_login_mode tinyint(1) NULL", tablePrefix);
							break;
						case "domain_to_use":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD domain_to_use varchar(255) NULL", tablePrefix);
							break;
						case "allow_choosing_lang":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD allow_choosing_lang tinyint(1) NULL", tablePrefix);
							break;
						case "allow_advanced_login":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD allow_advanced_login tinyint(1) NULL", tablePrefix);
							break;
						case "allow_auto_detect_and_correct":
							commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD allow_auto_detect_and_correct tinyint(1) NULL", tablePrefix);
							break;
                        case "global_addr_book":
                            commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD global_addr_book  tinyint(1) DEFAULT 0 NOT NULL", tablePrefix);
                            break;
                        case "view_mode":
                            commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD view_mode  tinyint(4) DEFAULT 1 NOT NULL", tablePrefix);
                            break;
                        case "save_mail":
                            commandText = string.Format(@"
							ALTER TABLE {0}awm_domains
							ADD save_mail  tinyint(4) DEFAULT 0 NOT NULL", tablePrefix);
                            break;
                    }
                    break;
                case Constants.TablesNames.acal_calendars:
                    switch (field)
                    {
                        case "calendar_description":
                            commandText = string.Format(@"
                            ALTER TABLE {0}acal_calendars 
                            MODIFY COLUMN calendar_description text", tablePrefix);
                            break;
                        case "calendar_str_id":
                            commandText = string.Format(@"
                            ALTER TABLE {0}acal_calendars 
                            ADD calendar_str_id varchar(100) character set utf8 NOT NULL default ''", tablePrefix);
                            break;
                    }
                    break;
                case Constants.TablesNames.acal_sharing:
                    switch (field)
                    {
                        case "calendar_active":
                            commandText = string.Format(@"
                            ALTER TABLE [{0}acal_sharing] 
                            ADD calendar_active tinyint(1) NOT NULL default 0", tablePrefix);
                            break;
                    }
                    break;
                case Constants.TablesNames.acal_events:
                    switch (field)
                    {
                        case "event_repeats":
                            commandText = string.Format(@"
                            ALTER TABLE {0}acal_events
                            ADD event_repeats tinyint(1) DEFAULT 0 NOT NULL", tablePrefix);
                            break;
                        case "event_text":
                            commandText = string.Format(@"
                            ALTER TABLE {0}acal_events
                            MODIFY COLUMN event_text text", tablePrefix);
                            break;
                        case "event_str_id":
                            commandText = string.Format(@"
                            ALTER TABLE {0}acal_events
                            ADD event_str_id varchar(200) character set utf8 NOT NULL DEFAULT ''", tablePrefix);
                            break;
                        case "event_last_modified":
                            commandText = string.Format(@"
                            ALTER TABLE {0}acal_events
                            ADD event_last_modified datetime DEFAULT NULL", tablePrefix);
                            break;
                        // 4.9.x
                        case "event_owner_email":
                            commandText = string.Format(@"
                            ALTER TABLE {0}acal_events 
                            ADD event_owner_email VARCHAR(255) character set utf8 NOT NULL DEFAULT ''", tablePrefix);
                            break;
                        case "event_appointment_access":
                            commandText = string.Format(@"
                            ALTER TABLE {0}acal_events 
                            ADD event_appointment_access tinyint(1) DEFAULT 0 NOT NULL", tablePrefix);
                            break;
                    }
                    break;
                case Constants.TablesNames.acal_publications:
                    switch (field)
                    {
                        case "access_type":
                            commandText = string.Format(@"
                            ALTER TABLE {0}acal_publications
                            ADD access_type tinyint(4) DEFAULT 0 NOT NULL", tablePrefix);
                            break;
                    }
                    break;
                case Constants.TablesNames.awm_messages:
                    switch (field)
                    {
                        case "flags":
                            commandText = string.Format(@"
                            ALTER TABLE {0}awm_messages
                            MODIFY COLUMN flags int(11)", tablePrefix);
                            break;
                        case "body_text":
                            commandText = string.Format(@"
                            ALTER TABLE {0}awm_messages
                            MODIFY COLUMN body_text mediumtext", tablePrefix);
                            break;
                        // 4.9.x
                        case "sensitivity":
                            commandText = string.Format(@"
                            ALTER TABLE {0}awm_messages
                            ADD sensitivity TINYINT(4) DEFAULT 0 NOT NULL", tablePrefix);
                            break;
                    }
                    break;
                case Constants.TablesNames.acal_exclusions:
                    switch (field)
                    {
                        case "id_recurrence_date": 
                            commandText = string.Format(@"
                            ALTER TABLE {0}acal_exclusions
                            ADD id_recurrence_date datetime DEFAULT NULL", tablePrefix);
                            break;
                        case "event_last_modified": 
                            commandText = string.Format(@"
                            ALTER TABLE {0}acal_exclusions
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
                            ALTER TABLE {0}acal_reminders
                            ADD id_user INT(11) NULL", tablePrefix);
                            break;
                    }
                    break;
                // 4.9.x
                case Constants.TablesNames.acal_users_data:
                    switch (field)
                    {
                        case "autoaddinvitation":
                            commandText = string.Format(@"
                            ALTER TABLE {0}acal_users_data
                            ADD autoaddinvitation TINYINT(1) DEFAULT 0 NOT NULL", tablePrefix);
                            break;
                        case "reminders_web_url":
                            commandText = string.Format(@"
                            ALTER TABLE {0}acal_users_data
                            ADD reminders_web_url VARCHAR(255) character set utf8 NULL", tablePrefix);
                            break;
                    }
                    break;
                // 4.9.x
                case Constants.TablesNames.awm_filters:
                    switch (field)
                    {
                        case "applied":
                            commandText = string.Format(@"
                            ALTER TABLE {0}awm_filters
                            ADD applied TINYINT(1) DEFAULT 1 NOT NULL", tablePrefix);
                            break;
                    }
                    break;
                case Constants.TablesNames.awm_settings:
                    switch (field)
                    {
                        case "auto_checkmail_interval":
                            commandText = string.Format(@"
                            ALTER TABLE {0}awm_settings
                            ADD auto_checkmail_interval INT(11) DEFAULT 0 NOT NULL", tablePrefix);
                            break;
                    }
                    break;
            }
            return PrepareCommand(commandText, null);
        }
		
		public override IDbCommand CreateIndex(string tablePrefix, string sufix, string column)
		{
			string commandText = string.Format(@"CREATE INDEX {0}{1} ON {0} ({2});", tablePrefix, sufix, column);
			return PrepareCommand(commandText, null);
		}

		//************************************************************************

		public override IDbCommand SelectTablesNames()
		{
			const string commandText = @"show tables";
			return PrepareCommand(commandText, null);
		}

		public override IDbCommand SelectTableFields(string tableName, string tablePrefix)
		{
			string commandText = string.Format(@"SHOW COLUMNS FROM `{0}{1}`", tablePrefix, tableName);
			return PrepareCommand(commandText, null);
		}

		public override IDbCommand SelectTableIndexes(string tableName, string tablePrefix)
		{
			string commandText = string.Format(@"SHOW INDEX FROM `{0}{1}`", tablePrefix, tableName);
			return PrepareCommand(commandText, null);
		}


		public override IDbCommand SelectMailboxsSize(int id_user)
		{
			string commandText = string.Format(@"SELECT SUM(mailbox_size) FROM {0} WHERE id_user=?",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_accounts));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_user", id_user));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectAwmAccountsNonDefaultCount(string email, string login, string password)
		{
			string commandText = string.Format(@"
			SELECT COUNT(*)	FROM {0}
			WHERE email LIKE '{1}' AND mail_inc_login LIKE '{2}' AND mail_inc_pass LIKE '{3}' AND def_acct=0
			",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_accounts),
				EncodeQuotes(email),
				EncodeQuotes(login),
				EncodeQuotes(password));

			return PrepareCommand(commandText, null);
		}

        public override IDbCommand SelectAwmAccountsCount(string searchCondition)
        {
            return SelectAwmAccountsCount(searchCondition, 0);
        }

        public override IDbCommand SelectAwmAccountsCount(string searchCondition, int id_domain)
        {
            string whereCondition;
            if (!string.IsNullOrEmpty(searchCondition))
            {
                whereCondition = string.Format(@"WHERE (email LIKE ('%{0}%') OR last_login LIKE ('%{0}%') OR logins_count LIKE ('%{0}%') OR mail_inc_host LIKE ('%{0}%') OR mail_out_host LIKE ('%{0}%')) AND id_domain = {1}", EncodeQuotes(searchCondition), id_domain);
            }
            else
            {
                whereCondition = string.Format(@"WHERE id_domain = {0}", id_domain);
            }

            string commandText = string.Format(@"SELECT COUNT(*) FROM {0} INNER JOIN {1} ON {0}.id_user = {1}.id_user {2}",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_accounts),
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_settings),
                whereCondition);

            return PrepareCommand(commandText, null);
        }

		public override IDbCommand SelectAwmFilters(int id_acct, int id_filter)
		{
			string commandText = string.Format(@"
			SELECT * FROM {0}
			WHERE id_acct=?{1}",
			EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_filters),
			(id_filter > 0) ? string.Format(" AND id_filter={0}", id_filter) : string.Empty);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

        public override IDbCommand SelectAwmMessagesUids(int id_acct, long id_folder_db)
        {
            string commandText = string.Format(@"SELECT str_uid FROM {0}
WHERE id_acct=? AND id_folder_db=?",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages));

            ArrayList parameters = new ArrayList();
            parameters.Add(CreateParameter("@id_acct", id_acct));
            parameters.Add(CreateParameter("@id_folder_db", id_folder_db));

            return PrepareCommand(commandText, parameters);
        }

		public override IDbCommand SelectAwmMessagesIntUids(int id_acct, long id_folder_db, bool msgsCompletely)
		{
			string commandText = string.Format(@"SELECT {0} FROM {1}
{2}WHERE id_acct=? AND id_folder_db=? ORDER BY int_uid ASC",
				msgsCompletely ? "*" : "int_uid",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
				_nolock);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder_db", id_folder_db));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectAwmMessages(int id_acct, long id_folder_db)
		{
			string commandText = string.Format(@"SELECT * FROM {0}
WHERE id_acct=? AND id_folder_db=? ORDER BY id_msg ASC",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder_db", id_folder_db));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectAwmMessages(int id_acct, long id_folder_db, long last_int_uid)
		{
			string commandText = string.Format(@"SELECT * FROM {0}
WHERE id_acct=? AND id_folder_db=? AND int_uid<=? ORDER BY id_msg ASC",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder_db", id_folder_db));
			parameters.Add(CreateParameter("@last_int_uid", last_int_uid));

			return PrepareCommand(commandText, parameters);
		}

        public override IDbCommand SelectAwmMessages(int id_acct, long id_folder_db, string[] uids, bool isImap, string order)
		{
			StringBuilder sb = new StringBuilder();
            foreach (string uid in uids)
            {
                if (isImap)
                {
                    sb.AppendFormat("{0},", EncodeQuotes(uid));
                }
                else
                {
                    sb.AppendFormat("'{0}',", EncodeQuotes(uid));
                }
            }
            if (uids.Length == 0)
            {
                sb.Append("0");
            }
            else
            {
                sb.Remove(sb.Length - 1, 1);
            }
			
			string commandText = string.Format(@" SELECT * FROM {0}
WHERE id_acct=? AND id_folder_db=? AND {2} IN ({1}) ORDER BY {3}", 
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
				sb.ToString(),
                isImap ? "int_uid" : "str_uid",
                order);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder_db", id_folder_db));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand SelectAwmMessages(int id_acct, string condition, FolderCollection folders, bool inHeadersOnly)
		{
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
				bodyLike = string.Format(" OR body_text LIKE '%{0}%'", EscapeWildcardCharacters(EncodeQuotes(condition)));
			}
			string commandText = string.Format(@"SELECT messages.*, folders.name AS folder_name FROM 
(SELECT * FROM {0} WHERE id_acct=? AND id_folder_db IN ({3}) AND
(from_msg LIKE '%{1}%' OR to_msg LIKE '%{1}%' OR cc_msg LIKE '%{1}%' OR bcc_msg
LIKE '%{1}%' OR subject LIKE '%{1}%'{4})) AS messages
INNER JOIN {2} AS folders
ON messages.id_folder_db = folders.id_folder",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages),
				EscapeWildcardCharacters(EncodeQuotes(condition)),
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_folders),
				folder_ids.ToString(),
				bodyLike);

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand UpdateAwmColumns(int id_column, int id_user, int value)
		{
            string commandText = string.Format(@"UPDATE {0}
SET column_value=? WHERE id_column=? AND id_user=?
",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_columns));

			ArrayList parameters = new ArrayList();
            parameters.Add(CreateParameter("@value", value));
			parameters.Add(CreateParameter("@id_column", id_column));
			parameters.Add(CreateParameter("@id_user", id_user));

			return PrepareCommand(commandText, parameters);
		}

		public override IDbCommand UpdateAwmFilters(int id_filter, int id_acct, byte field, byte condition, string filter,
			byte action, long id_folder, bool applied)
		{
			string commandText = string.Format(@"UPDATE {0}
SET id_acct=?, `field`=?, `condition`=?, `filter`=?, `action`=?,
 id_folder=?, applied=?
WHERE id_filter=?
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

		public override IDbCommand SelectAwmMessagesMarkAsDelete(int id_acct, long id_folder)
		{
			string commandText = string.Format(@"SELECT * FROM {0}
WHERE id_acct=? AND id_folder_db=? AND deleted=1",
				EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_messages));

			ArrayList parameters = new ArrayList();
			parameters.Add(CreateParameter("@id_acct", id_acct));
			parameters.Add(CreateParameter("@id_folder", id_folder));

			return PrepareCommand(commandText, parameters);
		}

#region Domains
        public override IDbCommand DeleteFromAwmDomains(int id)
        {
            string commandText = string.Format(@"DELETE FROM {0} WHERE id_domain=?",
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
            VALUES ( ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)",
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

        public override IDbCommand UpdateAwmDomains(int id, string name, short mail_protocol,
                      string mail_inc_host, int mail_inc_port, string mail_out_host,
                      int mail_out_port, bool mail_out_auth)
        {
            string commandText = string.Format(@"
            UPDATE {0} 
            SET 
                name=?, 
                mail_protocol=?, 
                mail_inc_host=?, 
                mail_inc_port=?, 
                mail_out_host=?, 
                mail_out_port=?, 
                mail_out_auth=?
            WHERE id_domain=?",
            EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_domains));

            ArrayList parameters = new ArrayList();
            parameters.Add(CreateParameter("@name", name));
            parameters.Add(CreateParameter("@mail_protocol", mail_protocol));
            parameters.Add(CreateParameter("@mail_inc_host", mail_inc_host));
            parameters.Add(CreateParameter("@mail_inc_port", mail_inc_port));
            parameters.Add(CreateParameter("@mail_out_host", mail_out_host));
            parameters.Add(CreateParameter("@mail_out_port", mail_out_port));
            parameters.Add(CreateParameter("@mail_out_auth", mail_out_auth));

            parameters.Add(CreateParameter("@id_domain", id));

            return PrepareCommand(commandText, parameters);
        }

        public override IDbCommand UpdateAwmDomains(int id, string name, short mail_protocol,
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
            UPDATE {0} 
            SET 
                name=?, 
                mail_protocol=?, 
                mail_inc_host=?, 
                mail_inc_port=?, 
                mail_out_host=?, 
                mail_out_port=?, 
                mail_out_auth=?,
                url=?,
                site_name=?,
                settings_mail_protocol=?,
                settings_mail_inc_host=?,
                settings_mail_inc_port=?,
                settings_mail_out_host=?,
                settings_mail_out_port=?,
                settings_mail_out_auth=?,
                allow_direct_mode=?,
                direct_mode_id_def=?,
                attachment_size_limit=?,
                allow_attachment_limit=?,
                mailbox_size_limit=?,
                allow_mailbox_limit=?,
                take_quota=?,
                allow_new_users_change_settings=?,
                allow_auto_reg_on_login=?,
                allow_users_add_accounts=?,
                allow_users_change_account_def=?,
                def_user_charset=?,
                allow_users_change_charset=?,
                def_user_timezone=?,
                allow_users_change_timezone=?,
                msgs_per_page=?,
                skin=?,
                allow_users_change_skin=?,
                lang=?,
                allow_users_change_lang=?,
                show_text_labels=?,
                allow_ajax=?,
                allow_editor=?,
                allow_contacts=?,
                allow_calendar=?,
                hide_login_mode=?,
                domain_to_use=?,
                allow_choosing_lang=?,
                allow_advanced_login=?,
                allow_auto_detect_and_correct=?,
                global_addr_book=?,
                view_mode=?,
                save_mail=?
            WHERE id_domain=?",
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

            parameters.Add(CreateParameter("@id_domain", id));

            return PrepareCommand(commandText, parameters);
        }

        public override IDbCommand SelectAwmDomain(int id_domain)
        {
            string commandText = string.Format(@"SELECT * FROM {0} WHERE id_domain=?",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_domains));

            ArrayList parameters = new ArrayList();
            parameters.Add(CreateParameter("@id_domain", id_domain));

            return PrepareCommand(commandText, parameters);
        }

        public override IDbCommand SelectAwmDomain(string domain)
        {
            string commandText = string.Format(@"SELECT * FROM {0} WHERE name=?",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_domains));

            ArrayList parameters = new ArrayList();
            parameters.Add(CreateParameter("@name", domain));

            return PrepareCommand(commandText, parameters);
        }

        public override IDbCommand SelectAwmDomainUrl(string url)
        {
            string commandText = string.Format(@"SELECT * FROM {0} WHERE url=?",
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

            string commandText = string.Format(@"SELECT * FROM {0}
            WHERE id_domain NOT IN
            (SELECT id_domain FROM {0} {5} ORDER BY {3} {4} LIMIT 0, {2}) {6}
            ORDER BY {3} {4} LIMIT 0, {1}",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_domains),
                pageSize,
                (page > 0) ? (page - 1) * pageSize : 0,
                orderBy,
                (asc) ? "ASC" : "DESC",
                whereConditionInternal,
                whereConditionExternal);

            return PrepareCommand(commandText, null);
        }

        public override IDbCommand SelectAwmDomains(int id_admin)
        {

            string commandText = string.Format(@"SELECT {0}.* FROM {0}, {1} WHERE {0}.id_domain={1}.id_domain AND {1}.id_admin=?",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_domains),
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_subadmin_domains));

            ArrayList parameters = new ArrayList();
            parameters.Add(CreateParameter("@id_admin", id_admin));

            return PrepareCommand(commandText, parameters);
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

        public override IDbCommand UpdateAccountsByDomain(string domain, int old_domain, int id_domain, short mail_protocol)
        {
            string commandText = string.Format(@"UPDATE {0}
SET id_domain=?
WHERE id_domain = {2} AND email LIKE '%@{1}'",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_accounts), domain, old_domain);

            if (mail_protocol != -1)
            {
                commandText += string.Format(@" AND mail_protocol = {0}", mail_protocol);
            }

            ArrayList parameters = new ArrayList();
            parameters.Add(CreateParameter("@id_domain", id_domain));

            return PrepareCommand(commandText, parameters);
        }

        #endregion

        public override IDbCommand SelectAwmSubadmins(int page, int pageSize, string orderBy, bool asc, string searchCondition)
        {
            string whereConditionInternal = string.Empty;
            string whereConditionExternal = string.Empty;
            if (!string.IsNullOrEmpty(searchCondition))
            {
                whereConditionInternal = string.Format(@"WHERE login LIKE ('{0}')", "%" + EscapeWildcardCharacters(EncodeQuotes(searchCondition)) + "%");
                whereConditionExternal = string.Format(@"AND (login LIKE ('{0}'))", "%" + EscapeWildcardCharacters(EncodeQuotes(searchCondition)) + "%");
            }

            string commandText = string.Format(@"SELECT * FROM {0}
WHERE id_admin {6}
            ORDER BY {3} {4} LIMIT {2}, {1}",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_subadmins),
                pageSize,
                GetStartIndexFromPage(page, pageSize),
                orderBy,
                (asc) ? "ASC" : "DESC",
                whereConditionInternal,
                whereConditionExternal);

            return PrepareCommand(commandText, null);
        }

        public override IDbCommand InsertIntoAwmSubadmins(string login, string password, string description)
        {
            string commandText = string.Format(@"INSERT INTO {0}(login, password, description)
VALUES(?, ?, ?)",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_subadmins));

            ArrayList parameters = new ArrayList();
            parameters.Add(CreateParameter("?", login));
            parameters.Add(CreateParameter("?", password));
            parameters.Add(CreateParameter("@?", description));

            return PrepareCommand(commandText, parameters);
        }

        public override IDbCommand InsertIntoAwmSubadminDomains(int id_admin, int id_domain)
        {
            string commandText = string.Format(@"INSERT INTO {0}(id_admin, id_domain)
VALUES(?, ?)",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_subadmin_domains));

            ArrayList parameters = new ArrayList();
            parameters.Add(CreateParameter("?", id_admin));
            parameters.Add(CreateParameter("?", id_domain));

            return PrepareCommand(commandText, parameters);
        }

        public override IDbCommand SelectAwmSubadminsCount(string condition)
        {
            string whereCondition = string.Empty;
            if (!string.IsNullOrEmpty(condition))
            {
                whereCondition = string.Format(@" WHERE login LIKE ('%{0}%')", EscapeWildcardCharacters(EncodeQuotes(condition)));
            }
            string commandText = string.Format(@"SELECT COUNT(id_admin) FROM {0}{1}",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_subadmins),
                whereCondition);

            return PrepareCommand(commandText, null);
        }

        public override IDbCommand SelectAwmSubadminsCountByLogin(string login)
        {
            string whereCondition = string.Empty;
            if (!string.IsNullOrEmpty(login))
            {
                whereCondition = string.Format(@" WHERE login = {0}", EscapeWildcardCharacters(EncodeQuotes(login)));
            }
            string commandText = string.Format(@"SELECT COUNT(id_admin) FROM {0}{1}",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_subadmins),
                whereCondition);

            return PrepareCommand(commandText, null);
        }


        public override IDbCommand SelectAwmSubadmins()
        {
            string commandText = string.Format(@"SELECT * FROM {0} 
{1}",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_subadmins),
                _nolock);

            return PrepareCommand(commandText, null);
        }

        public override IDbCommand SelectAwmSubadmins(int id_admin)
        {
            string commandText = string.Format(@"SELECT * FROM {0} WHERE id_admin=?",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_subadmins));

            ArrayList parameters = new ArrayList();
            parameters.Add(CreateParameter("?", id_admin));

            return PrepareCommand(commandText, parameters);
        }

        public override IDbCommand SelectAwmSubadmins(string login)
        {
            string commandText = string.Format(@"SELECT * FROM {0} WHERE login=?",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_subadmins));

            ArrayList parameters = new ArrayList();
            parameters.Add(CreateParameter("?", login));

            return PrepareCommand(commandText, parameters);
        }

        public override IDbCommand SelectAwmSubadminDomains(int id_admin)
        {
            string commandText = string.Format(@"SELECT id_domain FROM {0} WHERE id_admin=?",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_subadmin_domains));

            ArrayList parameters = new ArrayList();
            parameters.Add(CreateParameter("?", id_admin));

            return PrepareCommand(commandText, parameters);
        }

        public override IDbCommand SelectAwmSubadminDomains(string login)
        {
            string commandText = string.Format(@"SELECT {0}.id_domain FROM {0}, {1} WHERE {0}.id_domain={1}.id_admin AND {1}.login=?",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_subadmin_domains),
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_subadmins));

            ArrayList parameters = new ArrayList();
            parameters.Add(CreateParameter("?", login));

            return PrepareCommand(commandText, parameters);
        }

        public override IDbCommand UpdateAwmSubadmins(int id_admin, string login, string password, string description)
        {
            string commandText = string.Format(@"UPDATE {0} SET
login=?, 
password=?,
description=?
WHERE id_admin=?",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_subadmins));

            ArrayList parameters = new ArrayList();
            parameters.Add(CreateParameter("?", login));
            parameters.Add(CreateParameter("?", password));
            parameters.Add(CreateParameter("?", description));
            parameters.Add(CreateParameter("?", id_admin));

            return PrepareCommand(commandText, parameters);
        }

        public override IDbCommand DeleteFromAwmSubadmins(int id_admin)
        {
            string commandText = string.Format(@"DELETE FROM {0}
WHERE id_admin=?",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_subadmins));

            ArrayList parameters = new ArrayList();
            parameters.Add(CreateParameter("?", id_admin));

            return PrepareCommand(commandText, parameters);
        }

        public override IDbCommand DeleteFromAwmSubadminDomains(int id)
        {
            string commandText = string.Format(@"DELETE FROM {0}
WHERE id_admin=?",
                EncodeQuotes(_settings.DbPrefix + Constants.TablesNames.awm_subadmin_domains));

            ArrayList parameters = new ArrayList();
            parameters.Add(CreateParameter("?", id));

            return PrepareCommand(commandText, parameters);
        }

	}
}