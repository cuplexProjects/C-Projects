using System;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Web.UI;
using System.Xml.XPath;
using System.Web;

namespace WebMail
{
	public class WebmailResourceManager
	{
		protected CultureInfo _cultureInfo;
		protected ResourceManager _resManager;

		public CultureInfo Culture
		{
			get { return _cultureInfo; }
			set { _cultureInfo = value; }
		}

		public ResourceManager Manager
		{
			get { return _resManager; }
			set { _resManager = value; }
		}

		public WebmailResourceManager(string resourceDir)
		{
			_cultureInfo = CultureInfo.InvariantCulture;
			_resManager = ResourceManager.CreateFileBasedResourceManager("webmail", resourceDir, null);
		}

		public string GetString(string name)
		{
			try
			{
				string result = _resManager.GetString(name, _cultureInfo) ?? name;
				return result;
			}
			catch (Exception)
			{
				return name;
			}
		}
	}

	/// <summary>
    /// This class provides methods for retrieving language-dependent keys
	/// </summary>
	public class WebmailResourceManagerCreator : Control
	{
        public WebmailResourceManager CreateResourceManager()
        {
            return CreateResourceManager("");
        }

		public WebmailResourceManager CreateResourceManager(string lang)
		{
			if (lang.Length < 1)
			{
				WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
				lang = settings.DefaultLanguage;
			}

			if (/*this.Context*/HttpContext.Current.Session[Constants.sessionAccount] != null)
			{
				Account acct = /*this.Context*/HttpContext.Current.Session[Constants.sessionAccount] as Account;
				if (acct != null)
				{
					if ((acct.UserOfAccount != null) && (acct.UserOfAccount.Settings != null))
					{
						lang = acct.UserOfAccount.Settings.DefaultLanguage;
					}
				}
			}

			string culture = "";
			string langsXml = Path.Combine(Utils.GetDataFolderPath(), @"langs\langs.xml");
			if (File.Exists(langsXml))
			{
				XPathDocument xpathDoc = new XPathDocument(langsXml);
				XPathNavigator nav = xpathDoc.CreateNavigator();
				XPathNodeIterator langIter = nav.Select(string.Format("langs/lang[Name='{0}']/CultureName", lang));
				if (langIter.MoveNext())
				{
					culture = langIter.Current.Value;
				}
				else
				{
					culture = "";
				}
			}
			WebmailResourceManager newManager = new WebmailResourceManager(Path.Combine(Utils.GetDataFolderPath(), @"langs"));
			newManager.Culture = new CultureInfo(culture);
			
			return newManager;
		}
	}
}
