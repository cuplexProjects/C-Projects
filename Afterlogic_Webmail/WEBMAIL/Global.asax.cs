using System;
using System.Configuration;
using System.ComponentModel;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Reflection;

namespace WebMail 
{
	/// <summary>
	/// Summary description for Global.
	/// </summary>
	public class Global : HttpApplication
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>

		public Global()
		{
			InitializeComponent();
		}	
		
		protected void Application_Start(Object sender, EventArgs e)
		{
			PropertyInfo p = typeof(System.Web.HttpRuntime).GetProperty("FileChangesMonitor", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
			object o = p.GetValue(null, null);
			FieldInfo f = o.GetType().GetField("_dirMonSubdirs", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
			object monitor = f.GetValue(o);
			MethodInfo m = monitor.GetType().GetMethod("StopMonitoring", BindingFlags.Instance | BindingFlags.NonPublic);
			m.Invoke(monitor, new object[] { }); 
		}
 
		protected void Session_Start(Object sender, EventArgs e)
		{

		}

		protected void Application_BeginRequest(Object sender, EventArgs e)
		{
			try
			{
				string session_param_name = "ASPSESSID";
				string session_cookie_name = "ASP.NET_SESSIONID";
				string session_cookie_value = HttpContext.Current.Request.Form[session_param_name];

				if (session_cookie_value != null)
				{
					UpdateCookie(session_cookie_name, session_cookie_value);
				}
			}
			catch (Exception)
			{
			}
		}

		void UpdateCookie(string cookie_name, string cookie_value)
		{
			HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(cookie_name);
			if (cookie == null)
			{
				cookie = new HttpCookie(cookie_name, cookie_value);
			}
			else
			{
				cookie.Value = cookie_value;
			}
			HttpContext.Current.Request.Cookies.Set(cookie);
		}

		protected void Application_EndRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_Error(Object sender, EventArgs e)
		{

		}

		protected void Session_End(Object sender, EventArgs e)
		{
			if (Session[Constants.sessionTempFolder] != null)
			{
				string tempFolder = Session[Constants.sessionTempFolder].ToString();
				if (Directory.Exists(tempFolder))
				{
					try
					{
						Directory.Delete(tempFolder, true);
					}
					catch {}
				}
			}
		}

		protected void Application_End(Object sender, EventArgs e)
		{
            try
            {
                string datapath = String.Empty;
                if (ConfigurationManager.AppSettings[Constants.appSettingsDataFolderPath] != null)
                {
                    datapath = ConfigurationManager.AppSettings[Constants.appSettingsDataFolderPath];
                }

                string fullpath = Path.Combine(datapath, Constants.tempFolderName);
                if (fullpath != string.Empty && Directory.Exists(fullpath))
                {
                    Directory.Delete(fullpath, true);
                }
            }
            catch (Exception ex)
            {
                Log.WriteLine("Application_End Directory.Delete", ex.Message);
            }
		}
			
		#region Web Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion
	}
}

