using System;

namespace WebMail
{
	public partial class session_saver : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			Server.ScriptTimeout = 120;
			Response.Expires = -8000;
			if (Session["webmail_session"] != null)
			{
				Session["webmail_session"] = true;
			}
			else
			{
				Session.Add("webmail_session", true);
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
