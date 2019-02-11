using System;
using MailBee.Mime;
using System.Text;

namespace WebMail {
	/// <summary>
	/// Summary description for print_msg.
	/// </summary>
	public partial class print_msg : System.Web.UI.Page
	{
		protected WebmailResourceManager _manager = null;
		protected WebMailMessage msg = null;
		Account acct = null;
		protected bool IsHTMLMsg = false;
		protected string msgPlain = null;
		protected string msgHTML = null;
		protected string CcAddr = string.Empty;
		protected string Attachments = string.Empty;
		protected bool _stylesRtl = false;
		protected string defaultSkin = string.Empty;

		protected void Page_Load(object sender, EventArgs e) {
			bool showPicturesSettings = false;
			acct = Session[Constants.sessionAccount] as Account;
			if (acct == null) {
				Response.Redirect("default.aspx", true);
			}
			else {
				if (acct.UserOfAccount.Settings.RTL)
				{
					_stylesRtl = true;
				}
				defaultSkin = Utils.EncodeJsSaveString(acct.UserOfAccount.Settings.DefaultSkin);
				_manager = (new WebmailResourceManagerCreator()).CreateResourceManager();
				if ((acct.UserOfAccount != null) && (acct.UserOfAccount.Settings != null)) {
					showPicturesSettings = ((acct.UserOfAccount.Settings.ViewMode & ViewMode.AlwaysShowPictures) > 0) ? true : false;
				}

				try {
					int charset;
					string full_name_folder;
					long id_folder;
					string uid;
					int id;
					if (Request.QueryString["id"] != null)
					{
						id = Convert.ToInt32(Request.QueryString["id"]);
						uid = Request.QueryString["uid"];
						id_folder = Convert.ToInt64(Request.QueryString["id_folder"]);
						full_name_folder = Request.QueryString["full_name_folder"];
						charset = Convert.ToInt32(Request.QueryString["charset"]);
					}
					else
					{
						id = (int)Session["id"];
						uid = (string)Session["uid"];
						id_folder = (long)Session["id_folder"];
						full_name_folder = (string)Session["full_name_folder"];
						charset = (int)Session["charset"];
					}

					IsHTMLMsg = false;
					msgPlain = null;
					msgHTML = null;
					BaseWebMailActions bwml = new BaseWebMailActions(acct, Page);
					if (showPicturesSettings)
						msg = bwml.GetMessage(id, uid, id_folder, charset, 1, true, false, false, MessageMode.None);
					else
                        msg = bwml.GetMessage(id, uid, id_folder, full_name_folder, charset, false, MessageMode.None);
					LabelFrom.Text = Server.HtmlEncode(msg.FromMsg.ToString());
					LabelTo.Text = Server.HtmlEncode(msg.ToMsg.ToString());
					CcAddr = msg.CcMsg.ToString();
					if (CcAddr != string.Empty)
					{
						LabelCc.Text = Server.HtmlEncode(msg.CcMsg.ToString());
					}
					DateFormatting df = new DateFormatting(acct, msg.MsgDate);
					LabelDate.Text = df.FullDate;
					LabelSubject.Text = Server.HtmlEncode(msg.Subject);

					AttachmentCollection AttachmentsColl = msg.MailBeeMessage.Attachments;

					StringBuilder sb = new StringBuilder();
					foreach (Attachment Attach in AttachmentsColl)
					{
						sb.AppendFormat(@"{0}, ", Attach.Filename);
					}
					if (sb.Length > 2) sb.Remove(sb.Length - 2, 2);
					Attachments = sb.ToString();
					if (Attachments.Length > 0) {
						LabelAttachments.Text = Attachments;
					}

					msgPlain = Utils.MakeHtmlBodyFromPlainBody(msg.MailBeeMessage.BodyPlainText, true, "");
					msgHTML = msg.MailBeeMessage.BodyHtmlText;
					if(msgPlain == "") {
						msgPlain = Utils.ConvertHtmlToPlain(msgHTML);
					}
					if(msgHTML != "") {
						IsHTMLMsg = true;
						//Message body
						MessageBody.Text= msgHTML;
					}
					else {
						IsHTMLMsg = false;
						MessageBody.Text = msgPlain;
					}
				}
				catch (Exception ex) {
					Log.WriteException(ex);
					Response.Write(ex.Message);
				}
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e) {
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
		private void InitializeComponent() {    
		}
		#endregion
	}
}
