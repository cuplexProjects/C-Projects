using System;
using System.IO;

namespace WebMail
{
	/// <summary>
	/// Summary description for get_attachment_binary.
	/// </summary>
	public partial class get_attachment_binary : System.Web.UI.Page
	{
		Account acct = null;

		protected void Page_Load(object sender, EventArgs e)
		{
			acct = Session[Constants.sessionAccount] as Account;
			if (acct == null) 
			{
				Response.Redirect("default.aspx", true);
			}

			string filename = Request.QueryString["filename"];
			if (!string.IsNullOrEmpty(filename))
			{
				string safe_file_name = Path.GetFileName(filename);
				object tempFolder = Utils.GetTempFolderName(Session);
				if (tempFolder != null)
				{
					string fullPath = Path.Combine(tempFolder.ToString(), safe_file_name);
					if (File.Exists(fullPath))
					{
						using(FileStream fs = File.OpenRead(fullPath))
						{
							byte[] buffer = new byte[fs.Length];
							fs.Read(buffer, 0, buffer.Length);
							Response.BinaryWrite(buffer);
						}
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
