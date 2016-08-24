using System;
using System.IO;
using System.Web;
using System.Text;
using System.Collections.Generic;

namespace WebMail
{
	public enum WMStartPage
	{
		Mailbox = 0,
		NewMessage = 1,
		Settings = 2,
		Contacts = 3,
		Calendar = 4
	}

	/// <summary>
	/// Summary description for Integration.
	/// </summary>
	public class Integration
	{
		protected WebmailResourceManager _resMan = null;
		protected string _wmRoot = string.Empty;

		public Integration(string wmDataPath, string wmRootPath)
		{
			HttpApplicationState app = HttpContext.Current.Application;
			if (app != null)
			{
				if (app[Constants.appSettingsDataFolderPath] == null)
				{
					app.Add(Constants.appSettingsDataFolderPath, wmDataPath);
				}
			}
			_resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
			_wmRoot = wmRootPath.TrimEnd(new char[2] { '/', '\\' }); ;
		}

        

		public void UserLoginByEmail(string email, string login, string password, WMStartPage startPage, string toEmail)
		{
			Account acct = Account.LoginAccount(email, login, password);
			if (acct != null)
			{
				string sessionHash = Utils.GetMD5DigestHexString(HttpContext.Current.Session.SessionID);
				DbStorage storage = DbStorageCreator.CreateDatabaseStorage(acct);
				try
				{
					storage.Connect();
					storage.CreateTempRow(acct.ID, string.Format(@"sessionHash_{0}", sessionHash));
				}
				catch (WebMailException)
				{
					throw;
				}
				catch (Exception ex)
				{
					throw new WebMailDatabaseException(ex);
				}
				finally
				{
					storage.Disconnect();
				}

                HttpContext.Current.Response.Redirect(_wmRoot + @"/" + string.Format(@"integration/integr.aspx?hash={0}&scr={1}&to={2}", sessionHash, (int)startPage, toEmail), false);
            }
		}

		public void UserLoginByEmail(string email, string login, string pass, WMStartPage startPage)
		{
			UserLoginByEmail(email, login, pass, startPage, null);
		}
        

    }
}
