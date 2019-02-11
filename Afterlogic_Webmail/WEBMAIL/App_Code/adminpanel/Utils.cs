using System;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Globalization;
using XMailAdminProxy;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.SessionState;
using MailBee.Pop3Mail;
using MailBee;
using System.Web.Configuration;

using WebMail;

/// <summary>
/// Summary description for Utils
/// </summary>
public struct SelectedValueStruct
{
    public string Type;
    public string Name;
}

[Flags]
public enum OutputDomains
{
	None = 0,
	XMailDomains = 1,
	WebMailDomains = 2
}

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

    public DateFormatting()
    {
        _short = "";
        _full = "";
        _time = "";
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
        if (timeOffsetInMinutes != 0 && msgDate >= WebMail.Constants.MinDateWithMaxZoneTimeOffset)
        {
            TimeZone localZone = TimeZone.CurrentTimeZone;
            if (localZone.IsDaylightSavingTime(DateTime.Now))
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

public class AdminPanelUtils
{
    public static bool ValidationLicenseKey(string licenseKey)
    {
        bool flag = false;
        try
        {
            
            
            Pop3.LicenseKey = licenseKey;
            
            Pop3 pop = new Pop3();
            flag = true;
        }
        catch (Exception error)
        {
            Log.WriteException(error);
            flag = false;
        }
        return flag;
    }

	public static string GetWebMailFolder()
	{
	    string strDataFolderPath = string.Empty;

        strDataFolderPath = ConfigurationManager.AppSettings["WebMailFolderPath"];	        
        if (HttpContext.Current.Request.PhysicalApplicationPath != null)
        {
            Uri appUri = new Uri(HttpContext.Current.Request.PhysicalApplicationPath);
            Uri newUri = new Uri(appUri, ConfigurationManager.AppSettings["WebMailFolderPath"]);
            strDataFolderPath = newUri.LocalPath;
        }

        return strDataFolderPath;
    }

    public static string GetAdminPanelDataFolderPath()
    {
        string result = string.Empty;
        if (ConfigurationManager.AppSettings["AdminPanelDataFolderPath"] != null)
            result = ConfigurationManager.AppSettings["AdminPanelDataFolderPath"];
        else
        {
            try
            {
                result = Path.Combine(GetWebMailDataFolder(), "settings");
            }
            catch (Exception error)
            {
                Log.WriteException(error);
                result = string.Empty;
            }
        }
        return result;
    }

    public static string GetWebMailDataFolder()
	{
        string strDataFolderPath;
        if (HttpContext.Current.Application[WebMail.Constants.appSettingsDataFolderPath] != null)
        {
            strDataFolderPath = (string)HttpContext.Current.Application[WebMail.Constants.appSettingsDataFolderPath];
        }
        else
        {
            strDataFolderPath = ConfigurationManager.AppSettings[WebMail.Constants.appSettingsDataFolderPath];
        }

        /*converting relative path in absolute*/
        Uri appUri = new Uri(HttpContext.Current.Request.PhysicalApplicationPath);
        Uri newUri = new Uri(appUri, strDataFolderPath);
        strDataFolderPath = newUri.LocalPath;

        return strDataFolderPath;
	}

	public static void SaveState(string key, object value, HttpSessionState session)
	{
        session[key] = value;
	}

    public static object LoadState(string key, HttpSessionState session)
	{
		return session[key];
	}

    public static string[] GetSupportedSkins(string webmailFolderPath)
    {
        if (webmailFolderPath == null) return null;
        string skinsFolder = Path.Combine(webmailFolderPath, "skins");
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
        return (string[])skinsArr.ToArray(typeof(string));
    }
    
    public static string[] GetSupportedLangs(string dataFolderPath)
	{
		ArrayList arr = new ArrayList();
		string langsXml = Path.Combine(dataFolderPath, @"langs\langs.xml");
		if (File.Exists(langsXml))
		{
			XPathDocument xpathDoc = new XPathDocument(langsXml);
			XPathNavigator nav = xpathDoc.CreateNavigator();
			XPathNodeIterator langIter = nav.Select(string.Format("langs/lang/Name"));
			while (langIter.MoveNext())
			{
				if (langIter.Current.Value == "English")
                    arr.Insert(0, langIter.Current.Value);
                else
                    arr.Add(langIter.Current.Value);
			}
		}

		return (arr.Count > 0) ? (string[])arr.ToArray(typeof(string)) : new string[] { WebMail.Constants.defaultLang };
	}

    public static string EncryptPassword(string password)
    {
        StringBuilder sb = new StringBuilder();
        byte[] bytes = Encoding.UTF8.GetBytes(password);
        for (int i = 0; i < bytes.Length; i++)
        {
            byte b = (byte)((bytes[i] ^ 101) & 0xFF);
            sb.Append(b.ToString("X2"));
        }
        return sb.ToString();
    }

    public static string DecryptPassword(string password)
    {
        if (password == null) return string.Empty;
        string result = string.Empty;
        if ((password.Length > 0) && (password.Length % 2 == 0))
        {
            byte[] decryptedBytes = new byte[password.Length / 2];
            int startIndex = 0;
            int index = 0;
            while (startIndex < password.Length)
            {
                string strByte = password.Substring(startIndex, 2);
                int b = 0;
                if (int.TryParse(strByte, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out b))
                {
                    b = ((b & 0xFF) ^ 101);
                    decryptedBytes[index] = (byte)b;
                }
                startIndex += 2;
                index++;
            }
            result = Encoding.UTF8.GetString(decryptedBytes);
        }
        return result;
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

    public static void SetPageErrorMessage(Page page, string err)
    {
        HiddenField hfErrorMessage = (HiddenField)page.FindControl("errorMessage");
        hfErrorMessage.Value += err;
    }
    public static void SetPageReportMessage(Page page, string err)
    {
        HiddenField hfReportMessage = (HiddenField)page.FindControl("reportMessage");
        hfReportMessage.Value += err;
    }

    public static SelectedValueStruct ParseSelectedValue(string Domain)
    {
        SelectedValueStruct sds = new SelectedValueStruct();
        if (Domain.Trim() != string.Empty)
        {
            string[] domains = Domain.Split(new Char[] {'_'}, 2);
            if (domains.Length > 1)
            {
                sds.Type = domains[0];
                sds.Name = domains[1];
            }
        }
        return sds;
    }

    public static void ShowReportAndReportMessages(Page page)
    {
        string _PageReportMessage = (AdminPanelUtils.LoadState("SessPageReportMessage", page.Session) != null) ? AdminPanelUtils.LoadState("SessPageReportMessage", page.Session).ToString() : null;
        string _PageErrorMessage = (AdminPanelUtils.LoadState("SessPageErrorMessage", page.Session) != null) ? AdminPanelUtils.LoadState("SessPageErrorMessage", page.Session).ToString() : null;

        if (_PageReportMessage != null)
        {
            AdminPanelUtils.SetPageReportMessage(page, _PageReportMessage);
            AdminPanelUtils.SaveState("SessPageReportMessage", null, page.Session);
        }

        if (_PageErrorMessage != null)
        {
            AdminPanelUtils.SetPageErrorMessage(page, _PageErrorMessage);
            AdminPanelUtils.SaveState("SessPageErrorMessage", null, page.Session);
        }
    }

    

    public static bool IsSuperAdmin(HttpSessionState session, AdminPanelSettings apSettings)
    {
        object Admin = LoadState("Admin", session);

        if (Admin != null && Admin.ToString() == apSettings.User)
        {
            return true;
        }
        return false;
    }
}
