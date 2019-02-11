using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace WebMail
{
	/// <summary>
	/// Summary description for integr.
	/// </summary>
	public partial class integr : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			string hash = Request.QueryString["hash"] as string;
			string screen = Request.QueryString["scr"] as string;
			string to = Request.QueryString["to"] as string;

			if ((hash != null) && (hash.Length >= 0))
			{
				WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
				TempRow row = null;
				DbStorage storage = DbStorageCreator.CreateDatabaseStorage(null);
				try
				{
					storage.Connect();
					row = storage.GetTempRow(-1, string.Format(@"sessionHash_{0}", hash));
					if (row != null)
					{
						storage.DeleteTempRow(row.ID);
					}
					else
					{
						throw new WebMailException("Temp Row Is NULL");
					}
				}
				catch (Exception ex)
				{
					Log.WriteException(ex);
					Response.Redirect(@"../default.aspx", true);
				}
				finally
				{
					storage.Disconnect();
				}

				if (row != null)
				{
					Account acct = Account.LoadFromDb(row.IDAcct, -1, false);
					if (Session[Constants.sessionAccount] == null)
					{
						Session.Add(Constants.sessionAccount, acct);
					}
					else
					{
						Session[Constants.sessionAccount] = acct;
					}
					if (Session[Constants.sessionUserID] == null)
					{
						Session.Add(Constants.sessionUserID, acct.IDUser);
					}
					else
					{
						Session[Constants.sessionUserID] = acct.IDUser;
					}

					WMStartPage page = WMStartPage.Mailbox;
					try
					{
						short scrNum = short.Parse(screen);
						page = (WMStartPage)scrNum;
					} catch {}
					switch (page)
					{
						case WMStartPage.NewMessage:
							if (to != null) to = string.Format(@"&to={0}", to);
							goto default;
						case WMStartPage.Settings:
							goto default;
						case WMStartPage.Contacts:
							goto default;
						case WMStartPage.Mailbox:
							goto default;
						case WMStartPage.Calendar:
							goto default;
						default:
                            string check = string.Empty;
                            if (acct.GetMailAtLogin)
                            {
                                check = "check=1&";
                            }
                            HttpContext.Current.Response.Redirect(string.Format(@"../webmail.aspx?" + check + @"start={0}{1}", (int)page, to));
							break;
					}
				}
			}

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
}
