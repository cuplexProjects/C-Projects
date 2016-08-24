using System;
using System.Text.RegularExpressions;

namespace WebMail
{
	/// <summary>
	/// Summary description for history_storage.
	/// </summary>
	public partial class history_storage : System.Web.UI.Page
	{
		protected string objectName = string.Empty;
		protected string historyKey = string.Empty;

		protected void Page_Load(object sender, EventArgs e)
		{
			Account acct = Session[Constants.sessionAccount] as Account;
            if (acct != null)
            {
                objectName = Request["HistoryStorageObjectName"] ?? string.Empty;
                historyKey = Request["HistoryKey"] ?? string.Empty;

                Regex r1 = new Regex(@"[a-zA-Z]");
                if (!r1.IsMatch(objectName))
                {
                    objectName = string.Empty;
                }

                historyKey = "()";
                Regex r2 = new Regex(@"[\(\)&<>]");
                if (r2.IsMatch(historyKey))
                {
                    historyKey = string.Empty;
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
