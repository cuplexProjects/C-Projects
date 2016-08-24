using System;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using System.Xml.XPath;
using MailBee.Html;
using MailBee.ImapMail;
using MailBee.Mime;
using Microsoft.Win32;
using System.Reflection;
using System.Collections.Generic;
using MailBee.Pop3Mail;
using MailBee;


namespace WebMail
{
    public class Validation
	{
		private static string _errorMessage = null;
		private static string _corrected = null;
		private static string _testData = null;
		private static object _additionalInfo = null;
			
		public static string ErrorMessage
		{
			get
			{
				return _errorMessage;
			}
		}
		public static string Corrected
		{
			get
			{
				return _corrected;
			}
		}
		//Enum			
		public enum ValidationTask
		{
			/// <summary>
			/// Input: Not empty email string, like account@domen. Output: Trimmed string.
			/// </summary>
			Email,
			/// <summary>
			/// Input: Not empty login string, without special chars. Output: Trimmed string. Additional info: advanced login flag as string: "0" || "1"
			/// </summary>
			Login,
			/// <summary>
			/// Input: Not empty string. Output: Input.
			/// </summary>
			Password,
			/// <summary>
			/// Input: Incoming email server name, not empty. Output: Trimmed string.
			/// </summary>
			INServer,
			/// <summary>
			/// Input: Positive integer number for incoming port, not empty. Output: Trimmed string.
			/// </summary>
			INPort,
			/// <summary>
			/// Input: Outcoming email server name, not empty. Output: Trimmed string.
			/// </summary>
			OUTServer,
			/// <summary>
			/// Input: Positive integer number for outcoming port, not empty. Output: Trimmed string.
			/// </summary>
			OUTPort,
			/// <summary>
			/// Input: Messages per page value as positive number. Output: Trimmed string.
			/// </summary>
			MPP,
			/// <summary>
			/// Input: (???) Advanced, not empty. Output: Trimmed string.
			/// </summary>
			Advanced,
			/// <summary>
			/// Input: SMTP Server login. Output: Trimmed string.
			/// </summary>
			SMTPLogin,
			/// <summary>
			/// Input: Keep messages on server for days, positive number, not empty. Output: Trimmed string.
			/// </summary>
			KeepMessages,
			/// <summary>
			/// Input: Contacts per page, positive number, not empty. Output: Trimmed string.
			/// </summary>
			CPP,
			/// <summary>
			/// Input: Filter substring, not empty. Output: Trimmed string.
			/// </summary>
			Substring,
			/// <summary>
			/// Input: Valid folder name, not empty, excluding CON, AUX, COM1, COM2, COM3, COM4, LPT1, LPT2, LPT3, PRN, NUL. Output: Trimmed string.
			/// </summary>
			FolderName,
			/// <summary>
			/// Input: Email address for contact. Output: Trimmed string.
			/// </summary>
			ContactsEMail,
			/// <summary>
			/// Input: Name for contact. Output: Trimmed string.
			/// </summary>
			ContactsName,
			/// <summary>
			/// Input: Street for contact. Output: Trimmed string.
			/// </summary>
			ContactsStreet,
			/// <summary>
			/// Input: City for contact. Output: Trimmed string.
			/// </summary>
			ContactsCity,
			/// <summary>
			/// Input: State/province for contact. Output: Trimmed string.
			/// </summary>
			ContactsState,
			/// <summary>
			/// Input: Zip code for contact. Output: Trimmed string.
			/// </summary>
			ContactsZipCode,
			/// <summary>
			/// Input: Country/region for contact. Output: Trimmed string.
			/// </summary>
			ContactsCountry,
			/// <summary>
			/// Input: Fax number for contact. Output: Trimmed string.
			/// </summary>
			ContactsFax,
			/// <summary>
			/// Input: Phone number for contact. Output: Trimmed string.
			/// </summary>
			ContactsPhone,
			/// <summary>
			/// Input: Web page for contact. Output: Trimmed string.
			/// </summary>
			ContactsWebPage,
			/// <summary>
			/// Input: Cell phone number for contact. Output: Trimmed string.
			/// </summary>
			ContactsMobile,
			/// <summary>
			/// Input: Company name for contact. Output: Trimmed string.
			/// </summary>
			ContactsCompany,
			/// <summary>
			/// Input: Company department for contact. Output: Trimmed string.
			/// </summary>
			ContactsDepartment,
			/// <summary>
			/// Input: Job title for contact. Output: Trimmed string.
			/// </summary>
			ContactsJobTitle,
			/// <summary>
			/// Input: Office (???) for contact. Output: Trimmed string.
			/// </summary>
			ContactsOffice,
			/// <summary>
			/// Input: Notes for contact. Output: Trimmed string.
			/// </summary>
			ContactsNotes,
			/// <summary>
			/// Input: Group name, not empty, not existing. Output: Trimmed string.
			/// </summary>
			GroupName,
			/// <summary>
			/// Input: AddContact (???), not empty, not existing. Output: Trimmed string.
			/// </summary>
			AddContacts
		}
		//Ctor
		public Validation()
		{
		}
		//Public methods
		public static bool CheckIt(ValidationTask checkFor, string testData)
		{
			return CheckIt(checkFor, testData, null);
		}
		public static bool CheckIt(ValidationTask checkFor, string testData, object additionalInfo)
		{
			if (testData==null)
			{
				_errorMessage = "Null!";
				return false;
			}
			_testData = testData;
			if (additionalInfo!=null)
				{
					_additionalInfo = additionalInfo;
				}
			switch(checkFor)
			{
				case ValidationTask.Email:
					return emailCheck();
				case ValidationTask.Login:
					return loginCheck();
				case ValidationTask.Password:
					return passwordCheck();
				case ValidationTask.INServer:
					return inServerCheck();
				case ValidationTask.INPort:
					return inPortCheck();
				case ValidationTask.OUTServer:
					return outServerCheck();
				case ValidationTask.OUTPort:
					return outPortCheck();
				case ValidationTask.MPP:
					return mppCheck();
				case ValidationTask.Advanced:
					return advancedCheck();
				case ValidationTask.SMTPLogin:
					return smtpLogin();
				case ValidationTask.KeepMessages:
					return keepMessagesCheck();
				case ValidationTask.CPP:
					return cppCheck();
				case ValidationTask.Substring:
					return substringCheck();
				case ValidationTask.FolderName:
					return folderNameCheck();
				case ValidationTask.ContactsEMail:
					return contactsEmailCheck();
				case ValidationTask.ContactsName:
					return contactsNameCheck();
				case ValidationTask.ContactsStreet:
					return contactsStreetCheck();
				case ValidationTask.ContactsCity:
					return contactsCityCheck();
				case ValidationTask.ContactsState:
					return contactsStateCheck();
				case ValidationTask.ContactsZipCode:
					return contactsZipCodeCheck();
				case ValidationTask.ContactsCountry:
					return contactsCountryCheck();
				case ValidationTask.ContactsFax:
					return contactsFaxCheck();
				case ValidationTask.ContactsPhone:
					return contactsPhoneCheck();
				case ValidationTask.ContactsWebPage:
					return contactsWebPageCheck();
				case ValidationTask.ContactsMobile:
					return contactsMobileCheck();
				case ValidationTask.ContactsCompany:
					return contactsCompanyCheck();
				case ValidationTask.ContactsDepartment:
					return contactsDepartmentCheck();
				case ValidationTask.ContactsJobTitle:
					return contactsJobTitleCheck();
				case ValidationTask.ContactsOffice:
					return contactsOfficeCheck();
				case ValidationTask.ContactsNotes:
					return contactsNotesCheck();
				case ValidationTask.GroupName:
					return groupNameCheck();
				case ValidationTask.AddContacts:
					return addContactsCheck();
				default:
					return true;
			}
		}
		//Checks
		/// <summary>
		/// Check for valid email. Additional info required:
		/// </summary>
		/// <returns>True, if valid. False, if invalid.</returns>
		private static bool emailCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length==0)
			{
				_errorMessage = "WarningEmailFieldBlank";
				return false;
			}
			if (!Regex.IsMatch(_corrected,@"^([A-Za-z_0-9!#\$%\^\{\}`~&'\+\-=_.])+@([A-Za-z_0-9-.])+$"))
			{
				_errorMessage = "WarningCorrectEmail";
				return false;
			}
			return true;
		}
		/// <summary>
		/// Check for valid login. Additional info required: advanced login flag as string: "0" || "1"
		/// </summary>
		/// <returns>True, if valid. False, if invalid.</returns>
		private static bool loginCheck()
		{
			_corrected = _testData.Trim();
			if ((_corrected.Length==0)&&((_additionalInfo as string)=="1"))
			{
				_errorMessage = "WarningLoginFieldBlank";
				return false;
			}
			return true;
		}
		/// <summary>
		/// Check for valid password. Additional info required:
		/// </summary>
		/// <returns></returns>
		private static bool passwordCheck()
		{
			if (_testData.Length==0)
			{
				_errorMessage = "WarningPassBlank";
				return false;
			}
			_corrected = _testData;
			return true;
		}
		/// <summary>
		/// Check for vadid incoming mail server. Additional info required:
		/// </summary>
		/// <returns></returns>
		private static bool inServerCheck()
		{
			_corrected = _testData.Trim();
			if ((_corrected.Length == 0)&&(_additionalInfo as string)=="1")
			{
				_errorMessage = "WarningIncServerBlank";
				return false;
			}
			if (!Regex.IsMatch(_corrected,@"^([A-Za-z0-9-.])+$"))
			{
				_errorMessage = "WarningCorrectIncServer";
				return false;
			}
			return true;
		}
		/// <summary>
		/// Check for valid port number. Additional info required:
		/// </summary>
		/// <returns></returns>
		private static bool inPortCheck()
		{
			_corrected = _testData.Trim();
			if ((_corrected.Length == 0)&&((_additionalInfo as string)=="1"))
			{
				_errorMessage = "WarningIncPortBlank";
				return false;
			}
			int iPort;
			try
			{
				iPort = int.Parse(_corrected);
			}
			catch
			{
				_errorMessage = "WarningIncPortNumber";
				return false;
			}
			if ((iPort<0)||(iPort>65535))
			{
				_errorMessage = "WarningIncPortNumber";
				return false;
			}
			return true;
		}
		/// <summary>
		/// Check for valid SMTP Server string. Additional info required:
		/// </summary>
		/// <returns></returns>
		private static bool outServerCheck()
		{
			_corrected = _testData.Trim();
			if ((_corrected.Length == 0)&&((_additionalInfo as string)=="1"))
			{
				_errorMessage = "WarningOutPortBlank";
				return false;
			}
			if (!Regex.IsMatch(_corrected,@"^([A-Za-z0-9-.])+$"))
			{
				_errorMessage = "WarningCorrectSMTPServer";
				return false;
			}
			return true;
		}
		/// <summary>
		/// Check for valid out port. Additional info required:
		/// </summary>
		/// <returns></returns>
		private static bool outPortCheck()
		{
			_corrected = _testData.Trim();
			if ((_corrected.Length == 0)&&((_additionalInfo as string)=="1"))
			{
				_errorMessage = "WarningOutPortBlank";
				return false;
			}
			int iPort;
			try
			{
				iPort = int.Parse(_corrected);
			}
			catch
			{
				_errorMessage = "WarningOutPortNumber";
				return false;
			}
			if ((iPort<0)||(iPort>65535))
			{
				_errorMessage = "WarningOutPortNumber";
				return false;
			}
			return true;
		}
		/// <summary>
		/// Check for valid message per page string. Additional info required:
		/// </summary>
		/// <returns></returns>
		private static bool mppCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			short mPerPage;
			try
			{
				mPerPage = short.Parse(_corrected);
			}
			catch
			{
				_errorMessage = "<undefined>";
				return false;
			}
			if (mPerPage<0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		/// <summary>
		/// Check for valid Advanced Login field.
		/// </summary>
		/// <returns></returns>
		private static bool advancedCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		/// <summary>
		/// Check for valid smtpLogin. Additional info not required.
		/// </summary>
		/// <returns></returns>
		private static bool smtpLogin()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		/// <summary>
		/// Check for valid "keep messages on server for days" value. Additional info not required.
		/// </summary>
		/// <returns></returns>
		private static bool keepMessagesCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			if (!Regex.IsMatch(_corrected,@"^([\d])+$"))
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		/// <summary>
		/// Check for valid "Contacts per page" value. Additional info not required.
		/// </summary>
		/// <returns></returns>
		private static bool cppCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			if (!Regex.IsMatch(_corrected,@"^([\d])+$"))
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		/// <summary>
		/// Check for valid "Substring" value in filters. Additional info not required.
		/// </summary>
		/// <returns></returns>
		private static bool substringCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "WarningEmptyFilter";
				return false;
			}
			return true;
		}
		/// <summary>
		/// Check for valid folder name. Additional info not required.
		/// </summary>
		/// <returns></returns>
		private static bool folderNameCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "WarningEmptyFolderName";
				return false;
			}
			if (Regex.IsMatch(_corrected,@"[""<>/\\\*\?\|:]+"))
			{
				_errorMessage = "WarningCorrectFolderName";
				return false;
			}
			if (Regex.IsMatch(_corrected,@"^(|CON|AUX|COM1|COM2|COM3|COM4|LPT1|LPT2|LPT3|PRN|NUL)+$",RegexOptions.IgnoreCase))
			{
				_errorMessage = "WarningCorrectFolderName";
				return false;
			}
			return true;
		}
		/// <summary>
		/// Check for valid email in contacts. Additional info not required.
		/// </summary>
		/// <returns></returns>
		private static bool contactsEmailCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				return false;
			}
			return true;
		}
		/// <summary>
		/// Check for valid name in contacts. Additional info not required.
		/// </summary>
		/// <returns></returns>
		private static bool contactsNameCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				return false;
			}
			return true;
		}
		/// <summary>
		/// Checks for valid contacts values. Additional info not required.
		/// </summary>
		/// <returns></returns>
		private static bool contactsStreetCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		private static bool contactsCityCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		private static bool contactsStateCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		private static bool contactsZipCodeCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		private static bool contactsCountryCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		private static bool contactsFaxCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		private static bool contactsPhoneCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		private static bool contactsWebPageCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			Regex re = new Regex(@"^[/\;\<\=\>\[\\#\?\]]+");
			_corrected = re.Replace(_corrected, "");
			return true;
		}
		private static bool contactsMobileCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		private static bool contactsCompanyCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		private static bool contactsDepartmentCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		private static bool contactsJobTitleCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		private static bool contactsOfficeCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		private static bool contactsNotesCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "<undefined>";
				return false;
			}
			return true;
		}
		/// <summary>
		/// Check for valid group name. Additional info required:
		/// </summary>
		/// <returns></returns>
		private static bool groupNameCheck()
		{
			_corrected = _testData.Trim();
			if (_corrected.Length == 0)
			{
				_errorMessage = "WarningGroupNotComplete";
				return false;
			}
			return true;
		}
		/// <summary>
		/// Check for valid add contacts. Additional info required:
		/// </summary>
		/// <returns></returns>
		private static bool addContactsCheck()
		{
			return true;
		}
	}
	//*****************************************************************

	public class DateFormatting
	{
		private string _short;
		private string _full;
		private string _time;

		public string ShortDate
		{
			get { return _short; }
		}

		public string FullDate
		{
			get { return _full; }
		}

		public string Time
		{
			get { return _time; }
		}

		public DateFormatting(Account acct, DateTime date)
		{
			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
			int timeOffset = acct.UserOfAccount.Settings.DefaultTimeZone;
			if (timeOffset == 0 || !settings.AllowUsersChangeTimeZone)
			{
				timeOffset = settings.DefaultTimeZone;
			}
			int minutesTimesOffset = GetMinutesTimesOffset(timeOffset);
			DateTime dt = GetMessageDateWithOffset(date, minutesTimesOffset);
            if (acct.UserOfAccount.Settings.DefaultTimeFormat == TimeFormats.F12)
            {
                _time = dt.ToString("hh:mm tt", CultureInfo.InvariantCulture);
            }
            else
            {
                _time = dt.ToString("t", CultureInfo.InvariantCulture);
            }
			_full = GetWeekday(dt) + ", " + GetMonth(dt) +
				dt.ToString(" dd, yyyy", CultureInfo.InvariantCulture) + ", " + _time;

			DateTime Today = GetMessageDateWithOffset(DateTime.Now.ToUniversalTime(), minutesTimesOffset);
			if (Today.Year == dt.Year && Today.Month == dt.Month && Today.Day == dt.Day)
			{
				_short = _time;
				return;
			}

			DateTime Yesterday = Today.AddDays(-1);
			if (Yesterday.Year == dt.Year && Yesterday.Month == dt.Month && Yesterday.Day == dt.Day)
			{
				WebmailResourceManager resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
				_short = resMan.GetString("DateYesterday");
				return;
			}

			DateTime YearAgo = Today.AddMonths(-11);
			DateTime LastThisYearDay = new DateTime(YearAgo.Year, YearAgo.Month, 1);
			if (dt >= LastThisYearDay)
			{
				_short = GetMonth(dt) + dt.ToString(" dd", CultureInfo.InvariantCulture);
				return;
			}
			_short = GetMonth(dt) + dt.ToString(" dd, yyyy", CultureInfo.InvariantCulture);
		}

		private static string GetWeekday(DateTime dt)
		{
			WebmailResourceManager resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
			switch (dt.DayOfWeek)
			{
				case DayOfWeek.Monday:
					return resMan.GetString("DayToolMonday");
				case DayOfWeek.Tuesday:
					return resMan.GetString("DayToolTuesday");
				case DayOfWeek.Wednesday:
					return resMan.GetString("DayToolWednesday");
				case DayOfWeek.Thursday:
					return resMan.GetString("DayToolThursday");
				case DayOfWeek.Friday:
					return resMan.GetString("DayToolFriday");
				case DayOfWeek.Saturday:
					return resMan.GetString("DayToolSaturday");
				default:
					return resMan.GetString("DayToolSunday");
			}
		}

		private static string GetMonth(DateTime dt)
		{
			WebmailResourceManager resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
			switch (dt.Month)
			{
				case 2:
					return resMan.GetString("ShortMonthFebruary");
				case 3:
					return resMan.GetString("ShortMonthMarch");
				case 4:
					return resMan.GetString("ShortMonthApril");
				case 5:
					return resMan.GetString("ShortMonthMay");
				case 6:
					return resMan.GetString("ShortMonthJune");
				case 7:
					return resMan.GetString("ShortMonthJuly");
				case 8:
					return resMan.GetString("ShortMonthAugust");
				case 9:
					return resMan.GetString("ShortMonthSeptember");
				case 10:
					return resMan.GetString("ShortMonthOctober");
				case 11:
					return resMan.GetString("ShortMonthNovember");
				case 12:
					return resMan.GetString("ShortMonthDecember");
				default:
					return resMan.GetString("ShortMonthJanuary");
			}
		}

		private static DateTime GetMessageDateWithOffset(DateTime msgDateReceivedOriginal, int timeOffsetInMinutes)
		{
			DateTime msgDate;

			msgDate = msgDateReceivedOriginal;
			if (timeOffsetInMinutes != 0 && msgDate >= Constants.MinDateWithMaxZoneTimeOffset)
			{
				TimeZone localZone = TimeZone.CurrentTimeZone;
				if (DateTime.Now.IsDaylightSavingTime())
				{
					msgDate = msgDate.AddMinutes(timeOffsetInMinutes + 60);
				}
				else
				{
					msgDate = msgDate.AddMinutes(timeOffsetInMinutes);
				}
			}
			return msgDate;
		}

		private static int GetMinutesTimesOffset(int timeZone)
		{
			int timeOffset = 0;
			switch (timeZone)
			{
				case 0:
					TimeZone tz = TimeZone.CurrentTimeZone;
					TimeSpan ts = tz.GetUtcOffset(DateTime.Now);
					timeOffset = ((tz.IsDaylightSavingTime(DateTime.Now)) ? ts.Hours - 1 : ts.Hours) * 60;
					timeOffset += ts.Minutes;
					break;
				case 1:
					timeOffset = -12 * 60;
					break;
				case 2:
					timeOffset = -11 * 60;
					break;
				case 3:
					timeOffset = -10 * 60;
					break;
				case 4:
					timeOffset = -9 * 60;
					break;
				case 5:
					timeOffset = -8 * 60;
					break;
				case 6:
				case 7:
					timeOffset = -7 * 60;
					break;
				case 8:
				case 9:
				case 10:
				case 11:
					timeOffset = -6 * 60;
					break;
				case 12:
				case 13:
				case 14:
					timeOffset = -5 * 60;
					break;
				case 15:
				case 16:
				case 17:
					timeOffset = -4 * 60;
					break;
				case 18:
					timeOffset = (int)(-3.5 * 60);
					break;
				case 19:
				case 20:
				case 21:
					timeOffset = -3 * 60;
					break;
				case 22:
					timeOffset = -2 * 60;
					break;
				case 23:
				case 24:
					timeOffset = -60;
					break;
				case 25:
				case 26:
					timeOffset = 0;
					break;
				case 27:
				case 28:
				case 29:
				case 30:
				case 31:
					timeOffset = 60;
					break;
				case 32:
				case 33:
				case 34:
				case 35:
				case 36:
				case 37:
					timeOffset = 2 * 60;
					break;
				case 38:
				case 39:
				case 40:
				case 41:
					timeOffset = 3 * 60;
					break;
				case 42:
					timeOffset = (int)(3.5 * 60);
					break;
				case 43:
				case 44:
					timeOffset = 4 * 60;
					break;
				case 45:
					timeOffset = (int)(4.5 * 60);
					break;
				case 46:
				case 47:
					timeOffset = 5 * 60;
					break;
				case 48:
					timeOffset = (int)(5.5 * 60);
					break;
				case 49:
					timeOffset = 5 * 60 + 45;
					break;
				case 50:
				case 51:
				case 52:
					timeOffset = 6 * 60;
					break;
				case 53:
					timeOffset = (int)(6.5 * 60);
					break;
				case 54:
				case 55:
					timeOffset = 7 * 60;
					break;
				case 56:
				case 57:
				case 58:
				case 59:
				case 60:
					timeOffset = 8 * 60;
					break;
				case 61:
				case 62:
				case 63:
					timeOffset = 9 * 60;
					break;
				case 64:
				case 65:
					timeOffset = (int)(9.5 * 60);
					break;
				case 66:
				case 67:
				case 68:
				case 69:
				case 70:
					timeOffset = 10 * 60;
					break;
				case 71:
					timeOffset = 11 * 60;
					break;
				case 72:
				case 73:
					timeOffset = 12 * 60;
					break;
				case 74:
					timeOffset = 13 * 60;
					break;
			}
			return timeOffset;
		}
	}

	public class Utils
	{
        private static string _SettingsPath = string.Empty;
        public static string SettingsPath
        {
            get
            {
                return _SettingsPath;
            }
            set
            {
                _SettingsPath = value;
            }
        }
        private Utils() { }

        public static int GetLicensesNum(string licenseKey)
        {
             int licences_num = 0;
            

            return licences_num;
        }

        public static int GetTrialDaysLeft(string licenseKey)
        {
            int daysLeft = 0;
            try
            {
                
                
                Pop3.LicenseKey = WebMail.Constants.LiteLicenseKey;
                Pop3 pop = new Pop3();
                daysLeft = pop.TrialDaysLeft;
                
            }
            catch (MailBeeException error)
            {
                Log.WriteException(error);
                daysLeft = -2;
            }
            catch (Exception error)
            {
                Log.WriteException(error);
                daysLeft = -2;
            }
            return daysLeft;
        }	
        
        public static string ConvertToDBString(Account acct, string messageString)
		{
			Encoding dbEncoding = Encoding.UTF8;
			if ((acct != null) && (acct.UserOfAccount != null) && (acct.UserOfAccount.Settings != null))
			{
                try
				{
                    dbEncoding = Encoding.GetEncoding(acct.UserOfAccount.Settings.DbCharset);
				}
				catch { }
			}
			byte[] bytes = dbEncoding.GetBytes(messageString);
			messageString = Encoding.Default.GetString(bytes);
			return messageString;
		}

        public static string ConvertFromDBString(Account acct, string dbString)
        {
            Encoding dbEncoding = Encoding.UTF8;
            if ((acct != null) && (acct.UserOfAccount != null) && (acct.UserOfAccount.Settings != null))
            {
                try
                {
                    dbEncoding = Encoding.GetEncoding(acct.UserOfAccount.Settings.DbCharset);
                }
                catch { }
            }
            byte[] bytes = Encoding.Default.GetBytes(dbString);
            dbString = dbEncoding.GetString(bytes);

            return dbString;
        }

		public static string[] GetSupportedSkins(string skinsFolder)
		{
			ArrayList skinsArr = new ArrayList();
			if (Directory.Exists(skinsFolder))
			{
				string[] skins = Directory.GetDirectories(skinsFolder);
				foreach (string skin in skins)
				{
                    if (File.Exists(Path.Combine(skin, "styles.css")))
                    {
                        skinsArr.Add(Path.GetFileName(skin));
                    }
				}
			}
			return (string[])skinsArr.ToArray(typeof (string));
		}

		public static int GetCurrentSkinIndex(string[] supportedSkins, string skinName)
		{
			for (int i = 0; i < supportedSkins.Length; i++)
			{
				if (string.Compare(skinName, supportedSkins[i], true, CultureInfo.InvariantCulture) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		public static Dictionary<string, string> GetSupportedLangs()
		{
			Dictionary<string, string> langs = new Dictionary<string, string>();
			string langsXml = Path.Combine(GetDataFolderPath(), @"langs\langs.xml");
			if (File.Exists(langsXml))
			{
				XPathDocument xpathDoc = new XPathDocument(langsXml);
				XPathNavigator nav = xpathDoc.CreateNavigator();
				XPathNodeIterator langIter = nav.Select(string.Format("langs/lang"));
                langs.Add("English", "English");
                while (langIter.MoveNext())
				{
					XPathNavigator currNav = langIter.Current;
					XPathNodeIterator currIter = currNav.Select("Name");
					string name = currIter.MoveNext() ? currIter.Current.Value : "";
					currIter = currNav.Select("FriendlyName");
					string frName = currIter.MoveNext() ? currIter.Current.Value : "";
                    if (name != "English")
                        langs.Add(name, frName);
				}
			}
			return langs;
		}

		public static string EncodeHtml(string s)
		{
			Regex rChar = new Regex("[\x0-\x8\xB-\xC\xE-\x1F]+");
			s = rChar.Replace(s, " ");

			StringBuilder sb = new StringBuilder(s);
			sb.Replace("&", "&amp;");
			sb.Replace("<", "&lt;");
			sb.Replace(">", "&gt;");
			return sb.ToString();
		}

        public static string EncodeHtmlSimple(string s)
        {
            StringBuilder sb = new StringBuilder(s);
            sb.Replace("<", "&lt;");
            sb.Replace(">", "&gt;");
            return sb.ToString();
        }

		public static string DecodeHtml(string s)
		{
			StringBuilder sb = new StringBuilder(s);
			sb.Replace("&lt;", "<");
			sb.Replace("&gt;", ">");
			sb.Replace("&amp;", "&");
			return sb.ToString();
		}

		public static string AttributeQuote(string s)
		{
			s = s.Replace("\"", "&quot;");
			return s;
		}

		public static string EncodeCDATABody(string s)
		{
			Regex rChar = new Regex("[\x0-\x8\xB-\xC\xE-\x1F]+");
			s = rChar.Replace(s, " ");

			return s.Replace(@"]]>", @"&#93;&#93;&gt;");
		}

		public static string DecodeHtmlBody(string s)
		{
			return s.Replace(@"&#93;&#93;&gt;", @"]]>");
		}

		public static string EncodeJsSaveString(string s)
		{
			StringBuilder sb = new StringBuilder(s);
			sb.Replace("\\", @"\\");
			sb.Replace("'", @"\'");
			sb.Replace("\"", @"\""");
			sb.Replace("\t", @"\t");
			sb.Replace("\r", @"\r");
			sb.Replace("\n", @"\n");
			return sb.ToString();
		}

		public static string GetMD5DigestHexString(string str)
		{
			byte[] bytes = Encoding.Default.GetBytes(str);
			MD5 md5 = new MD5CryptoServiceProvider();
			bytes = md5.ComputeHash(bytes);
			return BytesToHexString(bytes);
		}

		public static string BytesToHexString(byte[] bytes)
		{
			StringBuilder result = new StringBuilder();
			foreach(byte b in bytes)
			{
				result.Append(b.ToString("x2"));
			}
			return result.ToString();
		}

		public static string GetAttachmentMimeTypeFromFileExtension(string fileExtension)
		{
			string result = "application/octet-stream";

			if (string.IsNullOrEmpty(fileExtension))
			{
				return result;
			}

			switch (fileExtension.ToLower(CultureInfo.InvariantCulture))
			{
				case "gif": return "image/gif";
				case "png": return "image/png";
				case "jpe":
				case "jpg":
				case "jpeg": return "image/jpeg";
				case "tif":
				case "tiff": return "image/tiff";
				case "bin":
				case "dms":
				case "lha":
				case "lzh":
				case "exe":
				case "class":
				case "dll": return "application/octet-stream";
				case "js": return "application/x-javascript";
				case "swf": return "application/x-shockwave-flash";
				case "doc": return "application/msword";
				case "zip": return "application/zip";
				case "ai":
				case "eps":
				case "ps": return "application/postscript";
				case "pdf": return "application/pdf";
				case "rtf": return "application/rtf";
				case "ppt": return "application/vnd.ms-powerpoint";
				case "htm":
				case "html": return "text/html";
				case "css": return "text/css";
				case "rtx": return "text/richtext";
				case "txt":
				case "asc": return "text/plain";
				case "xml": return "text/xml";
				case "wav": return "audio/x-wav";
				case "mid":
				case "midi": return "audio/midi";
				case "mpga":
				case "mp2":
				case "mp3": return "audio/mpeg";
				case "aif":
				case "aiff": return "audio/x-aiff";
				case "ra": return "audio/x-realaudio";
				case "mpeg":
				case "mpg":
				case "mpe": return "video/mpeg";
				case "qt":
				case "mov": return "video/quicktime";
				case "avi": return "video/x-msvideo";
			}


			RegistryKey regKey = null;
			try
			{
				if (fileExtension[0] == '.')
				{
					fileExtension = fileExtension.Remove(0, 1);
				}

				regKey = Registry.ClassesRoot;
				if (regKey != null)
				{
					regKey = regKey.OpenSubKey(string.Format(CultureInfo.InvariantCulture, ".{0}", fileExtension));
				}
				if (regKey != null)
				{
					result = (string)regKey.GetValue("Content Type");
				}
			}
			catch
			{
				return result;
			}
			finally
			{
				if (regKey != null) regKey.Close();
			}
			return result ?? "application/octet-stream";
		}

		public static string GetTempFolderName(HttpSessionState s)
		{
			string tempFolder = string.Empty;
			if (s != null)
			{
				Account acct = s[Constants.sessionAccount] as Account;
				if (acct != null)
				{
					tempFolder = GetMD5DigestHexString(s.SessionID);
					FileSystem fs = new FileSystem(acct.Email, acct.ID, false);
					tempFolder = fs.CreateFolder(tempFolder);
				}
				else
				{
					Log.WriteLine("GetTempFolderName", "account is null");
				}
				if (s[Constants.sessionTempFolder] != null)
				{
					if (string.Compare(s[Constants.sessionTempFolder].ToString(), tempFolder, true, CultureInfo.InvariantCulture) != 0)
					{
						if (Directory.Exists(s[Constants.sessionTempFolder].ToString()))
						{
							Directory.Delete(s[Constants.sessionTempFolder].ToString(), true);
						}
					}
					s[Constants.sessionTempFolder] = tempFolder;
				}
				else
				{
					s.Add(Constants.sessionTempFolder, tempFolder);
				}
			}
			else
			{
				Log.WriteLine("GetTempFolderName", "session is null");
			}
			return tempFolder;
		}

		public static string ConvertToUtf7Modified(string src)
		{
			return ImapUtils.ToUtf7String(src);
		}

		public static string ConvertFromUtf7Modified(string src)
		{
			return ImapUtils.FromUtf7String(src);
		}

        public static string CreateTempFilePath(string tempFolderName, string filename)
        {
            return CreateTempFilePath(tempFolderName, filename, false);
        }
		
        public static string CreateTempFilePath(string tempFolderName, string filename, bool overwrite)
		{
			string tempFilePath = string.Format("{0}{1}", GetMD5DigestHexString(filename), Path.GetExtension(filename));
			tempFilePath = Path.Combine(tempFolderName, tempFilePath);
            if (File.Exists(tempFilePath) && !overwrite)
			{
				int i = 1;
				while (File.Exists(tempFilePath))
				{
					tempFilePath = string.Format("{0}_{2}{1}", GetMD5DigestHexString(filename), Path.GetExtension(filename), i);
					tempFilePath = Path.Combine(tempFolderName, tempFilePath);
					i++;
				}
			}
			if (tempFilePath.Length > Constants.PathMaxLength)
			{
				System.Diagnostics.Debug.Assert(false);
			}
			return tempFilePath;
		}

		public static string EncodePassword(string password)
		{
			if (password == null) return password;

			StringBuilder sb = new StringBuilder();
			byte[] plainBytes = Encoding.UTF8.GetBytes(password);
			if (plainBytes.Length > 0)
			{
				byte encodeByte = plainBytes[0];
				sb.Append(encodeByte.ToString("x2"));
				for (int i = 1; i < plainBytes.Length; i++)
				{
					plainBytes[i] = (byte)(plainBytes[i] ^ encodeByte);
					sb.Append(plainBytes[i].ToString("x2"));
				}
			}
			return sb.ToString();
		}

		public static string DecodePassword(string password)
		{
			string result = string.Empty;
			if ((password.Length > 0) && (password.Length % 2 == 0))
			{
				try
				{
					byte decodeByte = byte.Parse(password.Substring(0, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
					byte[] plainBytes = new byte[password.Length / 2];
					plainBytes[0] = decodeByte;
					if (password.Length > 2)
					{
						int startIndex = 2;
						int currentByte = 1;
						do
						{
							string hexByte = password.Substring(startIndex, 2);
							plainBytes[currentByte] = (byte)(byte.Parse(hexByte, NumberStyles.HexNumber, CultureInfo.InvariantCulture) ^ decodeByte);
							startIndex += 2;
							currentByte++;
						}
						while (startIndex < password.Length);
					}
					result = Encoding.UTF8.GetString(plainBytes);
				}
				catch
				{
					// can't parse hexByte
				}
			}
			return result;
		}

		public static string GetLogFilePath()
		{
			string logPath = string.Empty;
			string dataFolder = GetDataFolderPath();
			if (dataFolder != null)
			{
				logPath = Path.Combine(dataFolder, Constants.logFilename);
			}
			return logPath;
		}

		public static string LoadFromFile(string filename, Encoding enc, int index, int count)
		{
			FileInfo fi = new FileInfo(filename);
			if (fi.Exists)
			{
				if (count < 0) count = (int)fi.Length;
				if ((index + count) <= fi.Length)
				{
                    using (FileStream fs = File.OpenRead(filename))
                    {
                        byte[] b = new byte[count];
                        UTF8Encoding temp = new UTF8Encoding(true);
                        fs.Position = index;
                        while (fs.Read(b, 0, b.Length) > 0)
                        {
                            return temp.GetString(b);
                        }
                    }
				}
			}
			return null;
		}

		public static string LoadFromFile(string filename, Encoding enc)
		{
			return LoadFromFile(filename, enc, 0, -1);
		}

		public static void ClearLog()
		{
			string logFile = GetLogFilePath();
			FileInfo fi = new FileInfo(logFile);
			if (fi.Exists)
			{
				try
				{
					using (FileStream fs = fi.Open(FileMode.Create)){}
				}
				catch {}
			}
		}

		public static Encoding GetIncCharset(UserSettings userSettings)
		{
			return GetIncCharset(userSettings, string.Empty);
		}

        public static Encoding GetIncCharset(UserSettings userSettings, string dataFolder)
        {
            Encoding enc = Encoding.GetEncoding("iso-8859-1");
            int codePage = userSettings.DefaultCharsetInc;
            if (codePage == 0 || codePage == -1)
            {
				WebmailSettings settings;
				if (dataFolder != string.Empty)
				{
					settings = (new WebMailSettingsCreator()).CreateWebMailSettings(dataFolder);
				}
				else
				{
					settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
				}
                codePage = settings.DefaultUserCharset;
            }
            return GetEncodingByCodePage(codePage, enc);
        }

        public static Encoding GetOutCharset(UserSettings userSettings)
        {
            Encoding enc = Encoding.UTF8;
            int codePage = userSettings.DefaultCharsetOut;
            if (codePage == 0 || codePage == -1)
            {
                WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
                codePage = settings.DefaultUserCharset;
            }
            return GetEncodingByCodePage(codePage, enc);
        }

        public static Encoding GetEncodingByCodePage(int codePage)
        {
            return GetEncodingByCodePage(codePage, Encoding.Default);
        }

        public static Encoding GetEncodingByCodePage(int codePage, Encoding defaultEnc)
        {
            Encoding enc = defaultEnc;
            if (codePage != 0 && codePage != -1)
            {
                try
                {
                    enc = Encoding.GetEncoding(codePage);
                }
                catch { }
            }
            return enc;
        }

		public static void ReverseCollection(ref CollectionBase collection)
		{
			Stack s = new Stack(collection);
			object[] objArr = s.ToArray();

			collection.Clear();
			foreach (object obj in objArr)
			{
				((IList)collection).Add(obj);
			}
		}

		public static int RandMsgID(int id_msg)
		{
			Random rnd = new Random();
			return id_msg + rnd.Next(1, 10);
		}

		public static string GetMessagePlainReplyToBody(Account acct, MailMessage mailMessage)
		{
			string signature = string.Empty;
			if ((acct.SignatureOptions & SignatureOptions.AddSignatureToAllOutgoingMessages) > 0)
			{
				if (acct.SignatureType == SignatureType.Html)
				{
					MailMessage tempMsg = new MailMessage();
					tempMsg.BodyHtmlText = acct.Signature;
					tempMsg.MakePlainBodyFromHtmlBody();
					signature = tempMsg.BodyPlainText;
				}
				else if (acct.SignatureType == SignatureType.Plain)
				{
					signature = acct.Signature;
				}
			}

			string plainBody;
			if (string.IsNullOrEmpty(mailMessage.BodyPlainText))
			{
				// clone because we need Attach.SavedAs
				MailMessage cloneMsg = mailMessage.Clone();
				cloneMsg.Parser.HtmlToPlainMode = HtmlToPlainAutoConvert.IfHtml;
				cloneMsg.Parser.HtmlToPlainOptions = HtmlToPlainConvertOptions.AddImgAltText | HtmlToPlainConvertOptions.AddUriForAHRef;
				cloneMsg.Parser.Apply();
				plainBody = cloneMsg.BodyPlainText;
			}
			else
			{
				plainBody = mailMessage.BodyPlainText;
			}
			if (signature.Length > 0) signature = signature + "\n";
            mailMessage.Parser.DatesAsUtc = true;
            DateFormatting df = new DateFormatting(acct, (mailMessage.DateReceived != DateTime.MinValue) ? mailMessage.DateReceived : mailMessage.Date);
			string mess = "---- " + "OriginalMessage" + " ----\n";
			mess += "From" + ": " + EncodeHtml(mailMessage.From.ToString()) + "\n";
			mess += "To" + ": " + EncodeHtml(mailMessage.To.ToString()) + "\n";
			if (mailMessage.Cc.ToString().Length > 0)
				mess += "CC" + ": " + EncodeHtml(mailMessage.Cc.ToString()) + "\n";
			mess += "Sent" + ": " + DecodeHtml(df.FullDate) + "\n";
			mess += "Subject" + ": " + EncodeHtml(mailMessage.Subject) + "\n\n";
			mess += DecodeHtml(plainBody);
			mess = mess.Replace("\n", "\n>");
			mess = "\n\n" + signature + mess;

			return mess;
		}

		public static string GetMessageHtmlReplyToBody(Account acct, MailMessage mailMessage)
		{
			string signature = string.Empty;
			if ((acct.SignatureOptions & SignatureOptions.AddSignatureToAllOutgoingMessages) > 0)
			{
				signature = acct.Signature;
			}

			string htmlBody;
			if (string.IsNullOrEmpty(mailMessage.BodyHtmlText))
			{
                htmlBody = MakeHtmlBodyFromPlainBody(mailMessage.BodyPlainText, true, "");
			}
			else
			{
				htmlBody = mailMessage.BodyHtmlText;
			}

			if (signature.Length > 0) signature = signature + "<br />";
            mailMessage.Parser.DatesAsUtc = true;
            DateFormatting df = new DateFormatting(acct, (mailMessage.DateReceived != DateTime.MinValue) ? mailMessage.DateReceived : mailMessage.Date);
			string mess = @"<br/><br/>" + signature + @"<blockquote style=""border-left: solid 2px #000000; margin-left: 5px; padding-left: 5px;"">";
			mess += "---- " + "OriginalMessage" + " ----<br/>";
			mess += "<b>" + "From" + "</b>: " + EncodeHtml(mailMessage.From.ToString()) + "<br/>";
			mess += "<b>" + "To" + "</b>: " + EncodeHtml(mailMessage.To.ToString()) + "<br/>";
			if (mailMessage.Cc.ToString().Length > 0)
				mess += "<b>" + "CC" + "</b>: " + EncodeHtml(mailMessage.Cc.ToString()) + "<br/>";
			mess += "<b>" + "Sent" + "</b>: " + EncodeHtml(df.FullDate) + "<br/>";
			mess += "<b>" + "Subject" + "</b>: " + EncodeHtml(mailMessage.Subject) + "<br/><br/>";
			mess += htmlBody + "</blockquote>";

			return mess;
		}

		public static string GetAttachmentDownloadLink(Attachment attach, bool isViewLink)
		{
			string attachmentName = (attach.Filename.Length > 0) ? attach.Filename : attach.Name;

			string downloadStr = "download-view-attachment.aspx?download={0}&filename={1}&temp_filename={2}";
			string filename = Path.GetFileName(attach.SavedAs);
			downloadStr = string.Format(downloadStr, (isViewLink) ? "0" : "1", HttpUtility.UrlEncode(attachmentName), HttpUtility.UrlEncode(filename));
			
			return downloadStr;
		}

        public static string GetAttachmentDownloadLink(MailMessage msg, Attachment attach, string full_name_folder, bool isViewLink, HttpSessionState s)
        {
            string attachmentName = (attach.Filename.Length > 0) ? attach.Filename : attach.Name;

            string partID = attach.Headers["X-PartID"];

            string tmp_filename = Utils.CreateTempFilePath(Utils.GetTempFolderName(s), attachmentName, true);

            string downloadStr = "download-view-attachment.aspx?download={0}&filename={1}&uid={2}&full_folder_name={3}&partID={4}&tmp_filename={5}";

            downloadStr = string.Format(downloadStr, (isViewLink) ? "0" : "1", HttpUtility.UrlEncode(attachmentName), msg.UidOnServer, full_name_folder, partID, HttpUtility.UrlEncode(Path.GetFileName(tmp_filename)));

            return downloadStr;
        }

        public static string GetEmbeddedAttachmentDownloadLink(Attachment attach)
        {
            string downloadStr = "view-embedded-msg.aspx?filename={0}";
            if (attach.IsTnef)
                downloadStr += "&tnef=1";
            string filename = Path.GetFileName(attach.SavedAs);
            downloadStr = string.Format(downloadStr, HttpUtility.UrlEncode(filename));
            return downloadStr;
        }
        
        public static string GetMessageDownloadLink(WebMailMessage msg, long id_folder, string folder_full_path)
		{
			if ((msg != null))
			{
				return string.Format(@"download-view-attachment.aspx?id_msg={0}&uid={1}&id_folder={2}&folder_path={3}",
					msg.IDMsg,
					HttpUtility.UrlEncode((!string.IsNullOrEmpty(msg.StrUid)) ? msg.StrUid : msg.IntUid.ToString(CultureInfo.InvariantCulture)),
					id_folder,
					HttpUtility.UrlEncode(folder_full_path));
			}
			return string.Empty;
		}

		public static string GetMessagePrintLink(WebMailMessage msg, long id_folder, string folder_full_path)
		{
			if ((msg != null))
			{
				return string.Format(@"print-msg.aspx?id={0}&uid={1}&id_folder={2}&full_name_folder={3}&charset={4}",
					msg.IDMsg,
					HttpUtility.UrlEncode((!string.IsNullOrEmpty(msg.StrUid)) ? msg.StrUid : msg.IntUid.ToString(CultureInfo.InvariantCulture)),
					id_folder,
					HttpUtility.UrlEncode(folder_full_path),
					GetCodePageNumber(msg.MailBeeMessage.Charset));
			}
			return string.Empty;
		}

		public static string GetAttachmentIconBackgroundPosition(Attachment attach)
		{
			switch (attach.ContentType.ToLower(CultureInfo.InvariantCulture))
			{
				case "application/asp":
					return "-480px -80px";
				case "application/css":
					return "-440px -80px";
				case "application/doc":
					return "-400px -80px";
				case "application/html":
					return "-360px -80px";
				case "application/pdf":
					return "-320px -80px";
				case "application/xls":
					return "-280px -80px";
				case "image/bmp":
					return "-160px -80px";
				case "image/gif":
					return "-120px -80px";
				case "image/jpg":
				case "image/jpeg":
                case "image/png":
					return "-80px -80px";
				case "image/tif":
				case "image/tiff":
					return "-40px -80px";
				case "text/plain":
					return "0 -80px";
				default:
					return "-240px -80px";
			}
		}

		public static string GetFriendlySize(long byteSize)
		{
			WebmailResourceManager man = (new WebmailResourceManagerCreator()).CreateResourceManager();
			double size = Math.Ceiling((double)byteSize / 1024);
			double mbSize = size / 1024;
			const string str = "{0}{1}";
			return (mbSize > 1) ? string.Format(str, Math.Ceiling(mbSize * 10)/10, man.GetString("Mb")) : string.Format(str, size, man.GetString("Kb"));
		}

		public static string GetShortFilename(string filename)
		{
			if (filename.Length > 15)
			{
				return string.Format(@"{0}...", filename.Substring(0, 12));
			}
			return filename;
		}

		public static int GetCodePageNumber(string codePageNum)
		{
			int result = -1;

			for(int i = 0; Constants.PageName.Length > i; i++)
			{
				if(Constants.PageName[i] == codePageNum)
				{
					result = Constants.PageNumber[i];
				}
			}

			return result;
		}

		public static string GetCodePageName(int codePageNum)
		{
			string result = "-1";

			for(int i = 0; Constants.PageNumber.Length > i; i++)
			{
				if(Constants.PageNumber[i] == codePageNum)
				{
					result = Constants.PageName[i];
				}
			}

			return result;
		}

		public static string ConvertHtmlToPlain(string htmlText)
		{
			MailMessage msg = new MailMessage();
			msg.BodyHtmlText = htmlText;
			msg.MakePlainBodyFromHtmlBody();
			return msg.BodyPlainText;
		}

		public static string GetTrimMessage(long id, string uid, long id_folder, string full_name_folder, 
			int charset, int body_type)
		{
			WebmailResourceManager resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
			return string.Format(@"
				<div class=""wm_safety_info"">
					<span>
						<span>{0} {1} </span>
						<a href=""view-full-msg.aspx?bodytype={2}&id={3}&uid={4}&id_folder={5}&full_name_folder={6}&charset={7}"" target=""view-full-msg"">{8}</a>
					</span>
				</div>",
			   resMan.GetString("ReportMessagePartDisplayed"),
			   resMan.GetString("ReportViewEntireMessage"),
			   body_type.ToString(),
			   id.ToString(),
			   uid,
			   id_folder.ToString(),
			   full_name_folder,
			   charset.ToString(),
			   resMan.GetString("ReportClickHere"));
		}

		public static string MakeHtmlBodyFromPlainBody(string bodyPlain, bool needToTrim, string trimMsg)
		{
			WebmailResourceManager resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();

			bool isTrimmed = false;
			if (needToTrim && (bodyPlain.Length > Constants.BodyMaxLength))
			{
				bodyPlain = bodyPlain.Substring(0, Constants.BodyMaxLength)/* + trimMsg*/;
				isTrimmed = true;
			}

			StringBuilder sb = new StringBuilder(EncodeHtml(bodyPlain));
			//const string urlPattern = @"(http|https|ftp|nntp|file|telnet|gopher|news|wais|prospero)://[\w\d!\$&'\(\)\*\+-\./:\?@_~%=;,#]+";
			//from php-version
            const string urlPattern = @"(http|https|ftp|nntp|file|telnet|gopher|news|wais|prospero)://[a-z0-9+-=%&@:_\.~?]+[#a-z0-9+]*[^\.<>()\s""\']";
			Regex r = new Regex(urlPattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);//-_.!~*'();/?:@&=+,$%
			Match m = r.Match(sb.ToString(), 0);
			while (m.Success)
			{
				string link = string.Format(CultureInfo.InvariantCulture, @"<a href=""{0}"">{0}</a>", m.Value);

				sb.Replace(m.Value, link, m.Index, m.Length);
				m = r.Match(sb.ToString(), m.Index + link.Length);
			}

			string quotedBody = "";
			string[] splittedBody = sb.ToString().Split(new Char[] { '\n' });
			for (int i = 0; i < splittedBody.Length; i++ )
			{
				Regex lineRegex = new Regex("(\r){1}", RegexOptions.Singleline);
				string bodyLine = lineRegex.Replace(splittedBody[i], "");

				lineRegex = new Regex("^[^&;]{0,20}&gt;", RegexOptions.Multiline);
				if (lineRegex.IsMatch(bodyLine))
				{
					quotedBody += @"<font class=""wm_message_body_quotation"">" + bodyLine + "</font><br />";
				}
				else
				{
					quotedBody += bodyLine + "<br />";
				}
			}

			// MailBee.Html
            Processor pr = new Processor();
			pr.Dom.OuterHtml = quotedBody;
            RuleSet rs = new RuleSet();

            TagAttributeCollection attrsToAdd = new TagAttributeCollection();
            TagAttribute addAttr = new TagAttribute();
            addAttr.Name = "target";
            addAttr.Value = "\"_blank\"";
            attrsToAdd.Add(addAttr);
            rs.AddTagProcessingRule("a", null, attrsToAdd, null, false);

            pr.Dom.Process(rs, null);
			string result = pr.Dom.OuterHtml;
			if (isTrimmed)
			{
				result += trimMsg;
			}
			return result;
		}

		public static ArrayList Split(string src, string separator)
		{
			ArrayList result = new ArrayList();
			int startIndex = 0;
			while (startIndex < src.Length)
			{
				int index = src.IndexOf(separator, startIndex);
				if (index >= 0)
				{
					result.Add(src.Substring(startIndex, index - startIndex));
					startIndex = index + separator.Length;
				}
				else
				{
					result.Add(src.Substring(startIndex, src.Length - startIndex));
					break;
				}
			}
			return result;
		}

		public static string GetDelimeter()
		{
			string delimeter;

			if (Environment.Version.Major == 1)
			{
				delimeter = ":";
			}
			else
			{
				delimeter = "$";
			}

			return delimeter;
		}

		public static string GetAddressesFriendlyName(EmailAddressCollection coll)
		{
			if (coll == null) return string.Empty;

			StringBuilder sb = new StringBuilder();
			foreach (EmailAddress addr in coll)
			{
				sb.AppendFormat(@"{0}, ",
					(!string.IsNullOrEmpty(addr.DisplayName)) ? addr.DisplayName : addr.Email);
			}
			if (sb.Length > 2) sb.Remove(sb.Length - 2, 2);
			return sb.ToString();
		}
		
		public static string GetLocalizedFolderNameByType(Folder fld)
		{
			WebmailResourceManager resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
			switch (fld.Type)
			{
				case FolderType.Drafts:
					return resMan.GetString(@"FolderDrafts");
				case FolderType.Inbox:
					return resMan.GetString(@"FolderInbox");
				case FolderType.SentItems:
					return resMan.GetString(@"FolderSentItems");
				case FolderType.Trash:
					return resMan.GetString(@"FolderTrash");
			}
			return fld.Name;
		}

        public static string GetDataFolderPath()
        {
			if (HttpContext.Current == null)
			{
				return null;
			}

            string strDataFolderPath;
            if (HttpContext.Current.Application[Constants.appSettingsDataFolderPath] != null)
            {
                strDataFolderPath = (string)HttpContext.Current.Application[Constants.appSettingsDataFolderPath];
            }
            else
            {
                strDataFolderPath = ConfigurationManager.AppSettings[Constants.appSettingsDataFolderPath];
            }

            /*converting relative path in absolute*/

            if (SettingsPath == string.Empty)
                SettingsPath = HttpContext.Current.Request.PhysicalApplicationPath;
            Uri appUri = new Uri(SettingsPath);
            Uri newUri = new Uri(appUri, strDataFolderPath);
            strDataFolderPath = newUri.LocalPath;

            return strDataFolderPath;
        }

        public static Hashtable ReadWebmailTab(string xmailRootPath)
        {
            if (xmailRootPath == null) return null;
            Hashtable result = null;
            if (Directory.Exists(xmailRootPath))
            {
                string path = Path.Combine(xmailRootPath, Constants.webmailTab);
                if (File.Exists(path))
                {
                    result = new Hashtable();
                    try
                    {
                        using (StreamReader sr = new StreamReader(File.OpenRead(path)))
                        {
                            string str;
                            while ((str = sr.ReadLine()) != null)
                            {
                                str = str.Trim();
                                if (str.StartsWith("#")) continue;
                                string[] keyValue = str.Split(new char[] { '\t' });
                                if (keyValue.Length == 2)
                                {
                                    string key = keyValue[0].ToString().Trim(new char[] { '"' });
                                    string value = keyValue[1].ToString().Trim(new char[] { '"' });
                                    if (!result.ContainsKey(key))
                                    {
                                        result.Add(key, value);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.WriteException(ex);
                        throw new WebMailIOException(ex);
                    }
                }
            }
            return result;
        }

        public static Hashtable ReadCtrlAccountsTab(string xmailRootPath)
        {
            if (xmailRootPath == null) return null;
            Hashtable result = null;
            if (Directory.Exists(xmailRootPath))
            {
                string path = Path.Combine(xmailRootPath, Constants.ctrlAccountsTab);
                if (File.Exists(path))
                {
                    result = new Hashtable();
                    try
                    {
                        using (StreamReader sr = new StreamReader(File.OpenRead(path)))
                        {
                            string str;
                            while ((str = sr.ReadLine()) != null)
                            {
                                str = str.Trim();
                                if (str.StartsWith("#")) continue;
                                string[] keyValue = str.Split(new char[] { '\t' });
                                if (keyValue.Length == 2)
                                {
                                    string login = keyValue[0].ToString().Trim(new char[] { '"' });
                                    string password = keyValue[1].ToString().Trim(new char[] { '"' });
                                    result.Add("login", login);
                                    result.Add("password", password);
                                    break;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.WriteException(ex);
                        throw new WebMailIOException(ex);
                    }
                }
            }
            return result;
        }

        public static string SystemMessageFlagsToStr(SystemMessageFlags flags)
        {
            string result = string.Empty;

            if ((flags & SystemMessageFlags.Draft) > 0) result += "D";
            if ((flags & SystemMessageFlags.Flagged) > 0) result += "F";
            if ((flags & SystemMessageFlags.Recent) > 0) result += "P";
            if ((flags & SystemMessageFlags.Answered) > 0) result += "R";
            if ((flags & SystemMessageFlags.Seen) > 0) result += "S";
            if ((flags & SystemMessageFlags.Deleted) > 0) result += "T";

            return result;
        }

        public static SystemMessageFlags StrToSystemMessageFlags(string flags)
        {
            SystemMessageFlags result = SystemMessageFlags.None;

            foreach (char ch in flags.Trim())
            {
                switch (ch.ToString().ToLower())
                {
                    case "s":
                        result = (result | SystemMessageFlags.Seen);
                        break;
                    case "r":
                        result = (result | SystemMessageFlags.Answered);
                        break;
                    case "t":
                        result = (result | SystemMessageFlags.Deleted);
                        break;
                    case "d":
                        result = (result | SystemMessageFlags.Draft);
                        break;
                    case "p":
                        result = (result | SystemMessageFlags.Recent);
                        break;
                    case "f":
                        result = (result | SystemMessageFlags.Flagged);
                        break;
                    default:
                        result = (result | SystemMessageFlags.Other);
                        break;
                }
            }
            return result;
        }

        public static T CreateShallowCopy<T>(T o)
        {
            MethodInfo memberwiseClone = o.GetType().
            GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
            return (T)memberwiseClone.Invoke(o, null);
        }
        
        public static T CreateDeepCopy<T>(T o)
        {
            T copy = CreateShallowCopy(o);
            foreach (FieldInfo f in typeof(T).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                object original = f.GetValue(o);
                f.SetValue(copy, CreateDeepCopy(original));
            }
            return copy;
        }

        public static long GetUnixMicroTimeStamp()
        {
            return Convert.ToInt64(((TimeSpan)(DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0))).TotalMilliseconds);
        }

        public static long GetUnixTimeStamp()
        {
            return Convert.ToInt64(((TimeSpan)(DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0))).TotalSeconds);
        }

        public static WebMailSensitivity ToWebMailSensitivity(MailSensitivity sensitivity)
        {
            WebMailSensitivity result = WebMailSensitivity.None;
            switch (sensitivity)
            {
                case MailSensitivity.None:
                    result = WebMailSensitivity.None;
                    break;
                case MailSensitivity.Confidential:
                    result = WebMailSensitivity.Confidential;
                    break;
                case MailSensitivity.Private:
                    result = WebMailSensitivity.Private;
                    break;
                case MailSensitivity.Personal:
                    result = WebMailSensitivity.Personal;
                    break;
                default:
                    result = WebMailSensitivity.None;
                    break;
            }
            return result;
        }

        public static MailSensitivity ToMailSensitivity(int sensitivity)
        {
            MailSensitivity result = MailSensitivity.None;
            switch (sensitivity)
            {
                case 0:
                    result = MailSensitivity.None;
                    break;
                case 1:
                    result = MailSensitivity.Confidential;
                    break;
                case 2:
                    result = MailSensitivity.Private;
                    break;
                case 3:
                    result = MailSensitivity.Personal;
                    break;
                default:
                    result = MailSensitivity.None;
                    break;
            }
            return result;
        }

		public static String ToStringSensitivity(WebMailSensitivity sensitivity)
		{
			switch (sensitivity)
			{
				default:
				case WebMailSensitivity.None:
					return "0";
				case WebMailSensitivity.Confidential:
					return "1";
				case WebMailSensitivity.Private:
					return "2";
				case WebMailSensitivity.Personal:
					return "3";
			}
		}

		public static String ToStringPriority(MailPriority priority)
		{
			switch (priority)
			{
				case MailPriority.High:
				case MailPriority.Highest:
					return "1";
				case MailPriority.Low:
				case MailPriority.Lowest:
					return "5";
				default:
				case MailPriority.Normal:
					return "3";
			}
		}

		public static bool IsRtlCharset(string charset, bool rtlInterface)
		{
			switch (charset.ToLower())
			{
				case "iso-8859-6":
				case "windows-1256":
				case "iso-8859-8":
				case "iso-8859-8-i":
				case "windows-1255":
					return true;
				case "utf-8":
					return rtlInterface;
			}
			return false;
		}
    }
}
