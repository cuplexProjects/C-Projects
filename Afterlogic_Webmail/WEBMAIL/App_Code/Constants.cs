using System;
using System.Collections;

namespace WebMail
{
	/// <summary>
	/// Summary description for Constants.
	/// </summary>
	public class Constants
	{
		protected Constants() {}

		//add constants to lang file-------
		public const string ntwStreamNull = "Network stream is null.";
		public const string wmServCmdNull = "WMServer command is null.";
		//---------------------------------------

        public const string WMVERSION = "5";

        public const bool IMAP_OPTIMIZATION = false;
        public const int IMAP_OPT_MAIL_SIZE = 20 * 1024;		
        
        public static DateTime MinDate = new DateTime(1970, 1, 1);
		public static DateTime MinDateWithMaxZoneTimeOffset = new DateTime(1970, 1, 1, 23, 0, 0);

        public const string LiteLicenseKey = "MNWL";
        
        public const string mailadmLogin = "mailadm";
		public const string appSettingsDataFolderPath = "dataFolderPath";
		public const string mailFolderName = "Mail";
		public const string tempFolderName = "Temp";
		public const string logFilename = "log.txt";
		public const string defaultSkinName = "AfterLogic";
		public const string nonChangedPassword = "77u/ZG9t";
		public const int PathMaxLength = 248;
		public const int DownloadChunk = 50;
		public const string defaultLang = "English";
        public const string webmailTab = "wm.tab";
        public const string ctrlAccountsTab = "ctrlaccounts.tab";
        public const int BodyMaxLength = 153600;
        public const bool UseStartTLS = true;


		public const string sessionUserID = "AUserId";
		public const string sessionAccount = "account";
		public const string sessionTempFolder = "temp_folder";
		public const string sessionSettings = "webmail_settings";
		public const string sessionAccountEdit = "account_edit";
		public const string sessionReportText = "report_text";
		public const string sessionErrorText = "error_text";
        public const string sessionDbManager = "db_manager";

		public const string mailAdmSaveSuccess = "Save successful!";
		public const string mailAdmSaveUnsuccess = "Save unsuccessful!";
		public const string mailAdmConnectSuccess = "Connect successful!";
		public const string mailAdmConnectUnsuccess = "Connect unsuccessful!";
		public const string mailAdmTablesExists = "Some tables exist. You must rename or remove it.";
		public const string mailAdmTablesCreated = "Tables created!";
		public const string mailAdmTablesNotCreated = "Creating table error!";
		public const string mailAdmLogClearSuccess = "Log clear successful!";
		public const string mailAdmLogClearUnsuccess = "Log clear error!";
		public const string mailAdmUpdateAccountSuccess = "Update successful!";
		public const string mailAdmUpdateAccountUnsuccess = "Update unsuccessful!";
		public const string mailAdmCreateAccountSuccess = "Create successful!";
		public const string mailAdmCreateAccountUnsuccess = "Create unsuccessful!";

        public const bool synchronizeImapFoldersAtLogin = true;

		public const string AddressGroupIDPreffix = "5765624D61696C50726F"; // "WebMailPro" hex string in ASCII codepage
		public const string ContactIDPreffix = "040000008200E00074C5B7101A82E008"; // iCal standart preffix

		public struct DateFormats
		{
			public const string Default = "default";
			public const string DDMMYY = "dd/mm/yy";
			public const string MMDDYY = "mm/dd/yy";
			public const string DDMonth = "dd month";
		}

        public const string timeFormat = "|#";

		public struct SupportedLangs
		{
			public const string English = "English";
			public const string French = "French";
			public const string Catala = "Catala";
			public const string Espanyol = "Espanyol";
			public const string Nederlands = "Nederlands";
			public const string Swedish = "Swedish";
			public const string Turkish = "Turkish";
			public const string German = "German";
			public const string Portuguese = "Portuguese";
			public const string Italiano = "Italiano";
		}

		public struct FolderNames
		{
			public const string Inbox = "Inbox";
			public const string InboxLower = "inbox";
			public const string SentItems = "Sent Items";
			public const string SentItemsLower = "sent items";
			public const string Sent = "Sent";
			public const string SentLower = "sent";
			public const string Drafts = "Drafts";
			public const string DraftsLower = "drafts";
			public const string Trash = "Trash";
			public const string TrashLower = "trash";
            public const string Spam = "Spam";
            public const string SpamLower = "spam";
            public const string Quarantine = "Quarantine";
            public const string QuarantineLower = "quarantine";
        }

        public struct MailDirStruct
        {
            public const string Cur = "cur";
            public const string New = "new";
            public const string Tmp = "tmp";
        }

        public const string FLAG_DELIMETER = "_2,";
        public const string SIZE_DELIMETER = ",S=";

        public struct TablesNames
        {
            /* WebMail Tables */
            public const string a_users = "a_users";
            public const string awm_accounts = "awm_accounts";
            public const string awm_addr_book = "awm_addr_book";
            public const string awm_addr_groups = "awm_addr_groups";
            public const string awm_addr_groups_contacts = "awm_addr_groups_contacts";
            public const string awm_columns = "awm_columns";
            public const string awm_filters = "awm_filters";
            public const string awm_folders = "awm_folders";
            public const string awm_folders_tree = "awm_folders_tree";
            public const string awm_messages = "awm_messages";
            public const string awm_messages_body = "awm_messages_body";
            public const string awm_reads = "awm_reads";
            public const string awm_senders = "awm_senders";
            public const string awm_settings = "awm_settings";
            public const string awm_temp = "awm_temp";
            public const string awm_subadmins = "awm_subadmins";
            public const string awm_subadmin_domains = "awm_subadmin_domains";

            /* Calendar Tables */
			public const string acal_appointments = "acal_appointments";
            public const string acal_calendars = "acal_calendars";
            public const string acal_events = "acal_events";
            public const string acal_users_data = "acal_users_data";
            public const string acal_sharing = "acal_sharing";
            public const string acal_publications = "acal_publications";
            public const string acal_eventrepeats = "acal_eventrepeats";
            public const string acal_exclusions = "acal_exclusions";
            public const string acal_reminders = "acal_reminders";
            public const string acal_cron_runs = "acal_cron_runs";

            /* Other Tables */
            public const string awm_domains = "awm_domains";
        }

		public struct TablesIndexes
		{
			public const string awm_messages_index = "awm_messages_index";
			public const string awm_messages_body_index = "awm_messages_body_index";
		}

		public struct StaticScreenNames
		{
			public const string new_message = "new_message";
			public const string settings_common = "settings_common";
			public const string settings_accounts_properties = "settings_accounts_properties";
			public const string settings_accounts_filters = "settings_accounts_filters";
            public const string settings_accounts_autoresponder = "settings_accounts_autoresponder";
            public const string settings_accounts_signature = "settings_accounts_signature";
            public const string settings_accounts_folders = "settings_accounts_folders";
			public const string settings_contacts = "settings_contacts";
			public const string contacts= "contacts";
			public const string contacts_view = "contacts_view";
			public const string contacts_add = "contacts_add";
			public const string _default = "default";
			public const string mail_list = "mail_list";
			public const string message = "message";
		}

		public struct GroupOperationsRequests
		{
			public const string Delete = "delete";
            public const string NoMoveDelete = "no_move_delete";
			public const string Flag = "flag";
			public const string MarkAllRead = "mark_all_read";
			public const string MarkAllUnread = "mark_all_unread";
			public const string MarkRead = "mark_read";
			public const string MarkUnread = "mark_unread";
			public const string MoveToFolder = "move_to_folder";
			public const string Purge = "purge";
			public const string Undelete = "undelete";
			public const string Unflag = "unflag";
			public const string Spam = "spam";
			public const string NotSpam = "not_spam";
            public const string ClearSpam = "clear_spam";
        }

		public static string[] Charsets = {
												   "CharsetDefault",
												   "CharsetArabicAlphabetISO",
												   "CharsetArabicAlphabet",
												   "CharsetBalticAlphabetISO",
												   "CharsetBalticAlphabet",
												   "CharsetCentralEuropeanAlphabetISO",
												   "CharsetCentralEuropeanAlphabet",
												   "CharsetChineseTraditional",
												   "CharsetCyrillicAlphabetISO",
												   "CharsetCyrillicAlphabetKOI8R",
												   "CharsetCyrillicAlphabet",
												   "CharsetGreekAlphabetISO",
												   "CharsetGreekAlphabet",
												   "CharsetHebrewAlphabetISO",
												   "CharsetHebrewAlphabet",
												   "CharsetJapanese",
												   "CharsetJapaneseShiftJIS",
												   "CharsetKoreanEUC",
												   "CharsetKoreanISO",
												   "CharsetLatin3AlphabetISO",
												   "CharsetTurkishAlphabet",
												   "CharsetUniversalAlphabetUTF7",
												   "CharsetUniversalAlphabetUTF8",
												   "CharsetVietnameseAlphabet",
												   "CharsetWesternAlphabetISO",
												   "CharsetWesternAlphabet"
											   };

		public static string[] PageName = {
											  "-1",
											  "iso-8859-6",
											  "windows-1256",
											  "iso-8859-4",
											  "windows-1257",
											  "iso-8859-2",
											  "windows-1250",
											  "big5",
											  "iso-8859-5",
											  "koi8-r",
											  "windows-1251",
											  "iso-8859-7",
											  "windows-1253",
											  "iso-8859-8",
											  "windows-1255",
											  "iso-2022-jp",
											  "shift-jis",
											  "euc-kr",
											  "iso-2022-kr",
											  "iso-8859-3",
											  "windows-1254",
											  "utf-7",
											  "utf-8",
											  "windows-1258",
											  "iso-8859-1",
											  "windows-1252"
										  };

		public static int[] PageNumber = {
											  -1,
											  28596,
											  1256,
											  28594,
											  1257,
											  28592,
											  1250,
											  950,
											  28595,
											  20866,
											  1251,
											  28597,
											  1253,
											  28598,
											  1255,
											  50220,
											  932,
											  946,
											  50225,
											  28593,
											  1254,
											  65000,
											  65001,
											  1258,
											  28591,
											  1252
										  };

        public struct WebmailTabKeys
        {
            public const string ControlPort = "CtrlPort";
            public const string SmtpPort = "SmtpPort";
            public const string Login = "Login";
            public const string Pass = "Password";
        }

		public struct FilterStatus
		{
			public const string New = "new";
			public const string Unchanged = "unchanged";
			public const string Updated = "updated";
			public const string Removed = "removed";
		}
		
		public class TimeZones
        {
			public static Hashtable tz = new Hashtable();

			public string[] this[int index]
			{
				get
				{
					if (index < 0 || index > tz.Count)
					{
						return null;
					}

					if (index == 0)
					{
						index = 120; // London, GMT+0
					}

					return (string[]) tz[index.ToString()];
				}
			}

			public TimeZones()
			{
				if (tz.Count > 0) return;

				tz.Add("1", new string[] {"-11:00","Apia"});
				tz.Add("2", new string[] {"-11:00","Midway"});
				tz.Add("3", new string[] {"-11:00","Niue"});
				tz.Add("4", new string[] {"-11:00","Pago Pago"});
				tz.Add("5", new string[] {"-10:00","Fakaofo"});
				tz.Add("6", new string[] {"-10:00","Hawaii Time"});
				tz.Add("7", new string[] {"-10:00","Johnston"});
				tz.Add("8", new string[] {"-10:00","Rarotonga"});
				tz.Add("9", new string[] {"-10:00","Tahiti"});
				tz.Add("10", new string[] {"-09:30","Marquesas"});
				tz.Add("11", new string[] {"-09:00","Alaska Time"});
				tz.Add("12", new string[] {"-09:00","Gambier"});
				tz.Add("13", new string[] {"-08:00","Pacific Time"});
				tz.Add("14", new string[] {"-08:00","Pacific Time - Tijuana"});
				tz.Add("15", new string[] {"-08:00","Pacific Time - Vancouver"});
				tz.Add("16", new string[] {"-08:00","Pacific Time - Whitehorse"});
				tz.Add("17", new string[] {"-08:00","Pitcairn"});
				tz.Add("18", new string[] {"-07:00","Mountain Time"});
				tz.Add("19", new string[] {"-07:00","Mountain Time - Arizona"});
				tz.Add("20", new string[] {"-07:00","Mountain Time - Chihuahua, Mazatlan"});
				tz.Add("21", new string[] {"-07:00","Mountain Time - Dawson Creek"});
				tz.Add("22", new string[] {"-07:00","Mountain Time - Edmonton"});
				tz.Add("23", new string[] {"-07:00","Mountain Time - Hermosillo"});
				tz.Add("24", new string[] {"-07:00","Mountain Time - Yellowknife"});
				tz.Add("25", new string[] {"-06:00","Belize"});
				tz.Add("26", new string[] {"-06:00","Central Time"});
				tz.Add("27", new string[] {"-06:00","Central Time - Mexico City"});
				tz.Add("28", new string[] {"-06:00","Central Time - Regina"});
				tz.Add("29", new string[] {"-06:00","Central Time - Winnipeg"});
				tz.Add("30", new string[] {"-06:00","Costa Rica"});
				tz.Add("31", new string[] {"-06:00","Easter Island"});
				tz.Add("32", new string[] {"-06:00","El Salvador"});
				tz.Add("33", new string[] {"-06:00","Galapagos"});
				tz.Add("34", new string[] {"-06:00","Guatemala"});
				tz.Add("35", new string[] {"-06:00","Managua"});
				tz.Add("36", new string[] {"-05:00","Bogota"});
				tz.Add("37", new string[] {"-05:00","Cayman"});
				tz.Add("38", new string[] {"-05:00","Eastern Time"});
				tz.Add("39", new string[] {"-05:00","Eastern Time - Iqaluit"});
				tz.Add("40", new string[] {"-05:00","Eastern Time - Montreal"});
				tz.Add("41", new string[] {"-05:00","Eastern Time - Toronto"});
				tz.Add("42", new string[] {"-05:00","Grand Turk"});
				tz.Add("43", new string[] {"-05:00","Guayaquil"});
				tz.Add("44", new string[] {"-05:00","Havana"});
				tz.Add("45", new string[] {"-05:00","Jamaica"});
				tz.Add("46", new string[] {"-05:00","Lima"});
				tz.Add("47", new string[] {"-05:00","Nassau"});
				tz.Add("48", new string[] {"-05:00","Panama"});
				tz.Add("49", new string[] {"-05:00","Port-au-Prince"});
				tz.Add("50", new string[] {"-05:00","Rio Branco"});
				tz.Add("51", new string[] {"-04:00","Anguilla"});
				tz.Add("52", new string[] {"-04:00","Antigua"});
				tz.Add("53", new string[] {"-04:00","Aruba"});
				tz.Add("54", new string[] {"-04:00","Asuncion"});
				tz.Add("55", new string[] {"-04:00","Atlantic Time - Halifax"});
				tz.Add("56", new string[] {"-04:00","Barbados"});
				tz.Add("57", new string[] {"-04:00","Bermuda"});
				tz.Add("58", new string[] {"-04:00","Boa Vista"});
				tz.Add("59", new string[] {"-04:00","Campo Grande"});
				tz.Add("60", new string[] {"-04:00","Caracas"});
				tz.Add("61", new string[] {"-04:00","Cuiaba"});
				tz.Add("62", new string[] {"-04:00","Curacao"});
				tz.Add("63", new string[] {"-04:00","Dominica"});
				tz.Add("64", new string[] {"-04:00","Grenada"});
				tz.Add("65", new string[] {"-04:00","Guadeloupe"});
				tz.Add("66", new string[] {"-04:00","Guyana"});
				tz.Add("67", new string[] {"-04:00","La Paz"});
				tz.Add("68", new string[] {"-04:00","Manaus"});
				tz.Add("69", new string[] {"-04:00","Martinique"});
				tz.Add("70", new string[] {"-04:00","Montserrat"});
				tz.Add("71", new string[] {"-04:00","Palmer"});
				tz.Add("72", new string[] {"-04:00","Port of Spain"});
				tz.Add("73", new string[] {"-04:00","Porto Velho"});
				tz.Add("74", new string[] {"-04:00","Puerto Rico"});
				tz.Add("75", new string[] {"-04:00","Santiago"});
				tz.Add("76", new string[] {"-04:00","Santo Domingo"});
				tz.Add("77", new string[] {"-04:00","St. Kitts"});
				tz.Add("78", new string[] {"-04:00","St. Lucia"});
				tz.Add("79", new string[] {"-04:00","St. Thomas"});
				tz.Add("80", new string[] {"-04:00","St. Vincent"});
				tz.Add("81", new string[] {"-04:00","Stanley"});
				tz.Add("82", new string[] {"-04:00","Thule"});
				tz.Add("83", new string[] {"-04:00","Tortola"});
				tz.Add("84", new string[] {"-03:30","Newfoundland Time - St. Johns"});
				tz.Add("85", new string[] {"-03:00","Araguaina"});
				tz.Add("86", new string[] {"-03:00","Belem"});
				tz.Add("87", new string[] {"-03:00","Buenos Aires"});
				tz.Add("88", new string[] {"-03:00","Cayenne"});
				tz.Add("89", new string[] {"-03:00","Fortaleza"});
				tz.Add("90", new string[] {"-03:00","Godthab"});
				tz.Add("91", new string[] {"-03:00","Maceio"});
				tz.Add("92", new string[] {"-03:00","Miquelon"});
				tz.Add("93", new string[] {"-03:00","Montevideo"});
				tz.Add("94", new string[] {"-03:00","Paramaribo"});
				tz.Add("95", new string[] {"-03:00","Recife"});
				tz.Add("96", new string[] {"-03:00","Rothera"});
				tz.Add("97", new string[] {"-03:00","Salvador"});
				tz.Add("98", new string[] {"-03:00","Sao Paulo"});
				tz.Add("99", new string[] {"-02:00","Noronha"});
				tz.Add("100", new string[] {"-02:00","South Georgia"});
				tz.Add("101", new string[] {"-01:00","Azores"});
				tz.Add("102", new string[] {"-01:00","Cape Verde"});
				tz.Add("103", new string[] {"-01:00","Scoresbysund"});
				tz.Add("104", new string[] {"+00:00","Abidjan"});
				tz.Add("105", new string[] {"+00:00","Accra"});
				tz.Add("106", new string[] {"+00:00","Atlantic/Faeroe"});
				tz.Add("107", new string[] {"+00:00","Bamako"});
				tz.Add("108", new string[] {"+00:00","Banjul"});
				tz.Add("109", new string[] {"+00:00","Bissau"});
				tz.Add("110", new string[] {"+00:00","Canary Islands"});
				tz.Add("111", new string[] {"+00:00","Casablanca"});
				tz.Add("112", new string[] {"+00:00","Conakry"});
				tz.Add("113", new string[] {"+00:00","Dakar"});
				tz.Add("114", new string[] {"+00:00","Danmarkshavn"});
				tz.Add("115", new string[] {"+00:00","Dublin"});
				tz.Add("116", new string[] {"+00:00","El Aaiun"});
				tz.Add("117", new string[] {"+00:00","Freetown"});
				tz.Add("118", new string[] {"+00:00","Lisbon"});
				tz.Add("119", new string[] {"+00:00","Lome"});
				tz.Add("120", new string[] {"+00:00","London"});
				tz.Add("121", new string[] {"+00:00","Monrovia"});
				tz.Add("122", new string[] {"+00:00","Nouakchott"});
				tz.Add("123", new string[] {"+00:00","Ouagadougou"});
				tz.Add("124", new string[] {"+00:00","Reykjavik"});
				tz.Add("125", new string[] {"+00:00","Sao Tome"});
				tz.Add("126", new string[] {"+00:00","St Helena"});
				tz.Add("127", new string[] {"+01:00","Algiers"});
				tz.Add("128", new string[] {"+01:00","Amsterdam"});
				tz.Add("129", new string[] {"+01:00","Andorra"});
				tz.Add("130", new string[] {"+01:00","Bangui"});
				tz.Add("131", new string[] {"+01:00","Berlin"});
				tz.Add("132", new string[] {"+01:00","Brazzaville"});
				tz.Add("133", new string[] {"+01:00","Brussels"});
				tz.Add("134", new string[] {"+01:00","Budapest"});
				tz.Add("135", new string[] {"+01:00","Central European Time"});
				tz.Add("136", new string[] {"+01:00","Ceuta"});
				tz.Add("137", new string[] {"+01:00","Copenhagen"});
				tz.Add("138", new string[] {"+01:00","Douala"});
				tz.Add("139", new string[] {"+01:00","Gibraltar"});
				tz.Add("140", new string[] {"+01:00","Kinshasa"});
				tz.Add("141", new string[] {"+01:00","Lagos"});
				tz.Add("142", new string[] {"+01:00","Libreville"});
				tz.Add("143", new string[] {"+01:00","Luanda"});
				tz.Add("144", new string[] {"+01:00","Luxembourg"});
				tz.Add("145", new string[] {"+01:00","Madrid"});
				tz.Add("146", new string[] {"+01:00","Malabo"});
				tz.Add("147", new string[] {"+01:00","Malta"});
				tz.Add("148", new string[] {"+01:00","Monaco"});
				tz.Add("149", new string[] {"+01:00","Ndjamena"});
				tz.Add("150", new string[] {"+01:00","Niamey"});
				tz.Add("151", new string[] {"+01:00","Oslo"});
				tz.Add("152", new string[] {"+01:00","Paris"});
				tz.Add("153", new string[] {"+01:00","Porto-Novo"});
				tz.Add("154", new string[] {"+01:00","Rome"});
				tz.Add("155", new string[] {"+01:00","Stockholm"});
				tz.Add("156", new string[] {"+01:00","Tirane"});
				tz.Add("157", new string[] {"+01:00","Tunis"});
				tz.Add("158", new string[] {"+01:00","Vaduz"});
				tz.Add("159", new string[] {"+01:00","Vienna"});
				tz.Add("160", new string[] {"+01:00","Warsaw"});
				tz.Add("161", new string[] {"+01:00","Windhoek"});
				tz.Add("162", new string[] {"+01:00","Zurich"});
				tz.Add("163", new string[] {"+02:00","Amman"});
				tz.Add("164", new string[] {"+02:00","Athens"});
				tz.Add("165", new string[] {"+02:00","Beirut"});
				tz.Add("166", new string[] {"+02:00","Blantyre"});
				tz.Add("167", new string[] {"+02:00","Bucharest"});
				tz.Add("168", new string[] {"+02:00","Bujumbura"});
				tz.Add("169", new string[] {"+02:00","Cairo"});
				tz.Add("170", new string[] {"+02:00","Chisinau"});
				tz.Add("171", new string[] {"+02:00","Damascus"});
				tz.Add("172", new string[] {"+02:00","Gaborone"});
				tz.Add("173", new string[] {"+02:00","Gaza"});
				tz.Add("174", new string[] {"+02:00","Harare"});
				tz.Add("175", new string[] {"+02:00","Helsinki"});
				tz.Add("176", new string[] {"+02:00","Istanbul"});
				tz.Add("177", new string[] {"+02:00","Johannesburg"});
				tz.Add("178", new string[] {"+02:00","Kiev"});
				tz.Add("179", new string[] {"+02:00","Kigali"});
				tz.Add("180", new string[] {"+02:00","Lubumbashi"});
				tz.Add("181", new string[] {"+02:00","Lusaka"});
				tz.Add("182", new string[] {"+02:00","Maputo"});
				tz.Add("183", new string[] {"+02:00","Maseru"});
				tz.Add("184", new string[] {"+02:00","Mbabane"});
				tz.Add("185", new string[] {"+02:00","Minsk"});
				tz.Add("186", new string[] {"+02:00","Kaliningrad"});
				tz.Add("187", new string[] {"+02:00","Nicosia"});
				tz.Add("188", new string[] {"+02:00","Riga"});
				tz.Add("189", new string[] {"+02:00","Sofia"});
				tz.Add("190", new string[] {"+02:00","Tallinn"});
				tz.Add("191", new string[] {"+02:00","Tel Aviv"});
				tz.Add("192", new string[] {"+02:00","Tripoli"});
				tz.Add("193", new string[] {"+02:00","Vilnius"});
				tz.Add("194", new string[] {"+03:00","Addis Ababa"});
				tz.Add("195", new string[] {"+03:00","Aden"});
				tz.Add("196", new string[] {"+03:00","Africa/Asmera"});
				tz.Add("197", new string[] {"+03:00","Antananarivo"});
				tz.Add("198", new string[] {"+03:00","Baghdad"});
				tz.Add("199", new string[] {"+03:00","Bahrain"});
				tz.Add("200", new string[] {"+03:00","Comoro"});
				tz.Add("201", new string[] {"+03:00","Dar es Salaam"});
				tz.Add("202", new string[] {"+03:00","Djibouti"});
				tz.Add("203", new string[] {"+03:00","Kampala"});
				tz.Add("204", new string[] {"+03:00","Khartoum"});
				tz.Add("205", new string[] {"+03:00","Kuwait"});
				tz.Add("206", new string[] {"+03:00","Mayotte"});
				tz.Add("207", new string[] {"+03:00","Mogadishu"});
				tz.Add("208", new string[] {"+03:00","Moscow"});
				tz.Add("209", new string[] {"+03:00","Nairobi"});
				tz.Add("210", new string[] {"+03:00","Qatar"});
				tz.Add("211", new string[] {"+03:00","Riyadh"});
				tz.Add("212", new string[] {"+03:00","Syowa"});
				tz.Add("213", new string[] {"+03:30","Tehran"});
				tz.Add("214", new string[] {"+04:00","Baku"});
				tz.Add("215", new string[] {"+04:00","Dubai"});
				tz.Add("216", new string[] {"+04:00","Mahe"});
				tz.Add("217", new string[] {"+04:00","Mauritius"});
				tz.Add("218", new string[] {"+04:00","Samara"});
				tz.Add("219", new string[] {"+04:00","Muscat"});
				tz.Add("220", new string[] {"+04:00","Reunion"});
				tz.Add("221", new string[] {"+04:00","Tbilisi"});
				tz.Add("222", new string[] {"+04:00","Yerevan"});
				tz.Add("223", new string[] {"+04:30","Kabul"});
				tz.Add("224", new string[] {"+05:00","Aqtau"});
				tz.Add("225", new string[] {"+05:00","Aqtobe"});
				tz.Add("226", new string[] {"+05:00","Ashgabat"});
				tz.Add("227", new string[] {"+05:00","Dushanbe"});
				tz.Add("228", new string[] {"+05:00","Karachi"});
				tz.Add("229", new string[] {"+05:00","Kerguelen"});
				tz.Add("230", new string[] {"+05:00","Maldives"});
				tz.Add("231", new string[] {"+05:00","Yekaterinburg"});
				tz.Add("232", new string[] {"+05:00","Tashkent"});
				tz.Add("233", new string[] {"+05:30","Colombo"});
				tz.Add("234", new string[] {"+05:30","India Standard Time"});
				tz.Add("235", new string[] {"+06:00","Almaty"});
				tz.Add("236", new string[] {"+06:00","Bishkek"});
				tz.Add("237", new string[] {"+06:00","Chagos"});
				tz.Add("238", new string[] {"+06:00","Dhaka"});
				tz.Add("239", new string[] {"+06:00","Mawson"});
				tz.Add("240", new string[] {"+06:00","Omsk, Novosibirsk"});
				tz.Add("241", new string[] {"+06:00","Thimphu"});
				tz.Add("242", new string[] {"+06:00","Vostok"});
				tz.Add("243", new string[] {"+06:30","Cocos"});
				tz.Add("244", new string[] {"+06:30","Rangoon"});
				tz.Add("245", new string[] {"+07:00","Bangkok"});
				tz.Add("246", new string[] {"+07:00","Christmas"});
				tz.Add("247", new string[] {"+07:00","Davis"});
				tz.Add("248", new string[] {"+07:00","Hanoi"});
				tz.Add("249", new string[] {"+07:00","Hovd"});
				tz.Add("250", new string[] {"+07:00","Jakarta"});
				tz.Add("251", new string[] {"+07:00","Krasnoyarsk"});
				tz.Add("252", new string[] {"+07:00","Phnom Penh"});
				tz.Add("253", new string[] {"+07:00","Vientiane"});
				tz.Add("254", new string[] {"+08:00","Brunei"});
				tz.Add("255", new string[] {"+08:00","Casey"});
				tz.Add("256", new string[] {"+08:00","China Time - Beijing"});
				tz.Add("257", new string[] {"+08:00","Hong Kong"});
				tz.Add("258", new string[] {"+08:00","Kuala Lumpur"});
				tz.Add("259", new string[] {"+08:00","Macau"});
				tz.Add("260", new string[] {"+08:00","Makassar"});
				tz.Add("261", new string[] {"+08:00","Manila"});
				tz.Add("262", new string[] {"+08:00","Irkutsk"});
				tz.Add("263", new string[] {"+08:00","Singapore"});
				tz.Add("264", new string[] {"+08:00","Taipei"});
				tz.Add("265", new string[] {"+08:00","Ulaanbaatar"});
				tz.Add("266", new string[] {"+08:00","Western Time - Perth"});
				tz.Add("267", new string[] {"+09:00","Choibalsan"});
				tz.Add("268", new string[] {"+09:00","Dili"});
				tz.Add("269", new string[] {"+09:00","Jayapura"});
				tz.Add("270", new string[] {"+09:00","Yakutsk"});
				tz.Add("271", new string[] {"+09:00","Palau"});
				tz.Add("272", new string[] {"+09:00","Pyongyang"});
				tz.Add("273", new string[] {"+09:00","Seoul"});
				tz.Add("274", new string[] {"+09:00","Tokyo"});
				tz.Add("275", new string[] {"+09:30","Central Time - Adelaide"});
				tz.Add("276", new string[] {"+09:30","Central Time - Darwin"});
				tz.Add("277", new string[] {"+10:00","Dumont D&"});
				tz.Add("278", new string[] {"+10:00","Eastern Time - Brisbane"});
				tz.Add("279", new string[] {"+10:00","Eastern Time - Hobart"});
				tz.Add("280", new string[] {"+10:00","Eastern Time - Melbourne, Sydney"});
				tz.Add("281", new string[] {"+10:00","Guam"});
				tz.Add("282", new string[] {"+10:00","Yuzhno-Sakhalinsk"});
				tz.Add("283", new string[] {"+10:00","Port Moresby"});
				tz.Add("284", new string[] {"+10:00","Saipan"});
				tz.Add("285", new string[] {"+10:00","Truk"});
				tz.Add("286", new string[] {"+11:00","Efate"});
				tz.Add("287", new string[] {"+11:00","Guadalcanal"});
				tz.Add("288", new string[] {"+11:00","Kosrae"});
				tz.Add("289", new string[] {"+11:00","Magadan"});
				tz.Add("290", new string[] {"+11:00","Noumea"});
				tz.Add("291", new string[] {"+11:00","Ponape"});
				tz.Add("292", new string[] {"+11:30","Norfolk"});
				tz.Add("293", new string[] {"+12:00","Antarctica/McMurdo"});
				tz.Add("294", new string[] {"+12:00","Antarctica/South_Pole"});
				tz.Add("295", new string[] {"+12:00","Auckland"});
				tz.Add("296", new string[] {"+12:00","Fiji"});
				tz.Add("297", new string[] {"+12:00","Funafuti"});
				tz.Add("298", new string[] {"+12:00","Kwajalein"});
				tz.Add("299", new string[] {"+12:00","Majuro"});
				tz.Add("300", new string[] {"+12:00","Petropavlovsk-Kamchatskiy"});
				tz.Add("301", new string[] {"+12:00","Nauru"});
				tz.Add("302", new string[] {"+12:00","Tarawa"});
				tz.Add("303", new string[] {"+12:00","Wake"});
				tz.Add("304", new string[] {"+12:00","Wallis"});
				tz.Add("305", new string[] {"+13:00","Enderbury"});
				tz.Add("306", new string[] {"+13:00","Tongatapu"});
				tz.Add("307", new string[] {"+14:00", "Kiritimati"});
			}
		}
	}
}

