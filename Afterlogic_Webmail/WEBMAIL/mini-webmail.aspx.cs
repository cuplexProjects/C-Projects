using System;
using System.Web.UI;
using System.IO;
using MailBee.Mime;
using System.Globalization;

namespace WebMail
{
	public partial class mini_webmail : System.Web.UI.Page
	{
		protected Account acct = null;
		protected string acctFriendlyNm = string.Empty;
		protected string acctEmail = string.Empty;
		protected string acctSignature = string.Empty;

		protected WebmailResourceManager _resMan = null;

		protected string WmVersion = "500";
		protected string defaultTitle = "AfterLogic WebMail Pro 5.0";
		protected string defaultSkin = Constants.defaultSkinName;
		protected string defaultLang = Constants.defaultLang;

		protected string jsClearDefaultTitle = string.Empty;
		protected string jsClearToAddr = string.Empty;
		protected bool _stylesRtl = false;
		protected string _rtl = "false";

		protected string openMode = string.Empty;
		protected int msgId = -1;
		protected string msgUid = string.Empty;
		protected long folderId = -1;
		protected string folderFullName = string.Empty;
		protected int charset = 0;
		protected int size = 0;
		protected int mode = 0;

		protected string jsMessageInit = string.Empty;
		protected string jsClearMessageSubject = string.Empty;

		protected string allowDhtmlEditor = "true";
		protected string showTextLabels = "true";

		protected void Page_Load(object sender, EventArgs e)
		{
			acct = (Account)Session[Constants.sessionAccount];
			if (acct == null)
			{
				Response.Redirect("default.aspx?error=1", true);
			}
			else
			{
				acctFriendlyNm = Utils.EncodeJsSaveString(acct.FriendlyName);
				acctEmail = Utils.EncodeJsSaveString(acct.Email);
				acctSignature = Utils.EncodeJsSaveString(acct.Signature);

				ReadRequestValues();
				ReadSettingsValues();
				PrepareViewMessage();
				_resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
			}
		}

		protected void ReadSettingsValues()
		{
			WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();

			defaultTitle = settings.SiteName;
			jsClearDefaultTitle = Utils.EncodeJsSaveString(defaultTitle);
			showTextLabels = settings.ShowTextLabels ? "true" : "false";

			if (acct == null)
			{
				defaultSkin = settings.DefaultSkin;
				defaultLang = settings.DefaultLanguage;
				allowDhtmlEditor = settings.AllowDhtmlEditor ? "true" : "false";
			}
			else
			{
				if ((acct.UserOfAccount != null) && (acct.UserOfAccount.Settings != null))
				{
					if (acct.UserOfAccount.Settings.RTL)
					{
						_stylesRtl = true;
						_rtl = "true";
					}

					defaultSkin = acct.UserOfAccount.Settings.DefaultSkin;
					allowDhtmlEditor = acct.UserOfAccount.Settings.AllowDhtmlEditor ? "true" : "false";
					string[] supportedSkins = Utils.GetSupportedSkins((Page != null) ? Page.MapPath("skins") : string.Empty);
					if (Utils.GetCurrentSkinIndex(supportedSkins, defaultSkin) < 0)
					{
						if (supportedSkins.Length > 0)
						{
							defaultSkin = supportedSkins[0];
						}
					}

					defaultLang = acct.UserOfAccount.Settings.DefaultLanguage;
				}
			}
		}

		protected void ReadRequestValues()
		{
			openMode = (Request.QueryString.Get("open_mode") != null) ? Request.QueryString.Get("open_mode") : "";
			switch (openMode) {
				case "view":
					msgId = (Request.QueryString.Get("msg_id") != null) 
						? Convert.ToInt32(Request.QueryString.Get("msg_id")) : -1;
					msgUid = (Request.QueryString.Get("msg_uid") != null) 
						? Request.QueryString.Get("msg_uid") : "";
					folderId = (Request.QueryString.Get("folder_id") != null) 
						? Convert.ToInt64(Request.QueryString.Get("folder_id")) : -1;
					folderFullName = (Request.QueryString.Get("folder_full_name") != null) 
						? Request.QueryString.Get("folder_full_name") : "";
					charset = (Request.QueryString.Get("charset") != null) 
						? Convert.ToInt32(Request.QueryString.Get("charset")) : -1;
					size = (Request.QueryString.Get("size") != null) 
						? Convert.ToInt32(Request.QueryString.Get("size")) : 0;
					mode = (Request.QueryString.Get("mode") != null) 
						? Convert.ToInt32(Request.QueryString.Get("mode")) : 0;
					break;
				case "new":
					string toAddr = (Request.QueryString.Get("to") != null) ? Request.QueryString.Get("to") : "";
					jsClearToAddr = Utils.EncodeJsSaveString(toAddr);
					break;
			}
		}

		protected void PrepareViewMessage()
		{
			if (openMode != "view") return;

			BaseWebMailActions bwml = new BaseWebMailActions(acct, Page);
			byte safety = 0;
			bool showPicturesSettings = ((acct.UserOfAccount.Settings.ViewMode & ViewMode.AlwaysShowPictures) > 0) ? true : false;
			if (showPicturesSettings) safety = 1;
			WebMailMessage msg = bwml.GetMessage(msgId, msgUid, folderId, charset, safety, true, false, false, MessageMode.None);
			MailMessage outputMsg = msg.MailBeeMessage;

			jsClearMessageSubject = Utils.EncodeJsSaveString(outputMsg.Subject) + " - ";

			jsMessageInit = "ViewMessage = new CMessage();\n";
			jsMessageInit += "ViewMessage.FolderId = " + msg.IDFolderDB.ToString() + ";\n";
			jsMessageInit += "ViewMessage.FolderFullName = '" + msg.FolderFullName + "';\n";
			jsMessageInit += "ViewMessage.Size = " + msg.Size.ToString() + ";\n";
			jsMessageInit += "ViewMessage.Id = " + msg.IDMsg.ToString() + ";\n";
			jsMessageInit += "ViewMessage.Uid = '" + (string.IsNullOrEmpty(msg.StrUid)
				? msg.IntUid.ToString() : msg.StrUid) + "';\n";
			jsMessageInit += "ViewMessage.HasHtml = " + (string.IsNullOrEmpty(outputMsg.BodyHtmlText)
				? "false" : "true") + ";\n";
			jsMessageInit += "ViewMessage.HasPlain = " + (string.IsNullOrEmpty(outputMsg.BodyPlainText)
				? "false" : "true") + ";\n";
			jsMessageInit += "ViewMessage.Importance = " + Utils.ToStringPriority(msg.Priority) + ";\n";
			jsMessageInit += "ViewMessage.Sensivity = " + Utils.ToStringSensitivity(msg.Sensitivity) + ";\n";
			jsMessageInit += "ViewMessage.Charset = " + msg.OverrideCharset.ToString(CultureInfo.InvariantCulture) + ";\n";
			jsMessageInit += "ViewMessage.HasCharset = " + (string.IsNullOrEmpty(outputMsg.Charset)
				? "false" : "true") + ";\n";
			string strCharset = !string.IsNullOrEmpty(outputMsg.Charset)
				? outputMsg.Charset
				: Utils.GetCodePageName(msg.OverrideCharset);
			string rtl = (Utils.IsRtlCharset(strCharset, acct.UserOfAccount.Settings.RTL)) ? "true" : "false";
			jsMessageInit += "ViewMessage.RTL = " + rtl + ";\n";
			jsMessageInit += "ViewMessage.Safety = " + msg.Safety.ToString(CultureInfo.InvariantCulture) + ";\n";
			jsMessageInit += "ViewMessage.Downloaded = " + ((msg.Downloaded) ? "true" : "false") + ";\n";

			jsMessageInit += "ViewMessage.FromAddr = '" + Utils.EncodeJsSaveString(outputMsg.From.ToString()) + "';\n";
			jsMessageInit += "ViewMessage.FromDisplayName = '" + Utils.EncodeJsSaveString(outputMsg.From.DisplayName) + "';\n";
			jsMessageInit += "ViewMessage.ToAddr = '" + Utils.EncodeJsSaveString(outputMsg.To.ToString()) + "';\n";
			string shortTo = string.Empty;
			if ((outputMsg.To.ToString().IndexOf(acct.Email) == -1) && (outputMsg.To.Count > 0))
			{
				shortTo = (outputMsg.To[0].DisplayName.Length > 0) ? outputMsg.To[0].DisplayName : outputMsg.To[0].Email;
			}
			jsMessageInit += "ViewMessage.ShortToAddr = '" + Utils.EncodeJsSaveString(shortTo) + "';\n";
			jsMessageInit += "ViewMessage.CCAddr = '" + Utils.EncodeJsSaveString(outputMsg.Cc.ToString()) + "';\n";
			jsMessageInit += "ViewMessage.BCCAddr = '" + Utils.EncodeJsSaveString(outputMsg.Bcc.ToString()) + "';\n";
			jsMessageInit += "ViewMessage.ReplyToAddr = '" + Utils.EncodeJsSaveString(outputMsg.ReplyTo.ToString()) + "';\n";
			jsMessageInit += "ViewMessage.Subject = '" + Utils.EncodeJsSaveString(outputMsg.Subject) + "';\n";
			DateFormatting df = new DateFormatting(acct, msg.MsgDate);
			jsMessageInit += "ViewMessage.Date = '" + Utils.EncodeJsSaveString(df.ShortDate) + "';\n";
			jsMessageInit += "ViewMessage.FullDate = '" + Utils.EncodeJsSaveString(df.FullDate) + "';\n";
			jsMessageInit += "ViewMessage.Time = '" + Utils.EncodeJsSaveString(df.Time) + "';\n";

			jsMessageInit += "ViewMessage.HtmlBody = '" + Utils.EncodeJsSaveString(outputMsg.BodyHtmlText) + "';\n";
			jsMessageInit += "ViewMessage.PlainBody = '" + Utils.EncodeJsSaveString(Utils.MakeHtmlBodyFromPlainBody(outputMsg.BodyPlainText, true, "")) + "';\n";

			string replyHtml = Utils.GetMessageHtmlReplyToBody(acct, outputMsg);
			jsMessageInit += "ViewMessage.ReplyHtml = '" + Utils.EncodeJsSaveString(replyHtml) + "';\n";
			jsMessageInit += "ViewMessage.IsReplyHtml = true;\n";
			jsMessageInit += "ViewMessage.ForwardHtml = ViewMessage.ReplyHtml;\n";
			jsMessageInit += "ViewMessage.IsForwardHtml = true;\n";
			string replyPlain = Utils.GetMessagePlainReplyToBody(acct, outputMsg);
			jsMessageInit += "ViewMessage.ReplyPlain = '" + Utils.EncodeJsSaveString(replyPlain) + "';\n";
			jsMessageInit += "ViewMessage.IsReplyPlain = true;\n";
			jsMessageInit += "ViewMessage.ForwardPlain = ViewMessage.ReplyPlain;\n";
			jsMessageInit += "ViewMessage.IsForwardPlain = true;\n";

			if (outputMsg.Attachments.Count > 0)
			{
				jsMessageInit += "ViewMessage.Attachments = [];\n";
				foreach (Attachment attach in outputMsg.Attachments)
				{
					string _downloadlink = string.Empty;
					if (string.IsNullOrEmpty(attach.SavedAs))
					{
						_downloadlink = Utils.GetAttachmentDownloadLink(outputMsg, attach, folderFullName, false, Page.Session);
					}
					else
					{
						_downloadlink = Utils.GetAttachmentDownloadLink(attach, false);
					}
					string _viewlink = string.Empty;
					if (string.IsNullOrEmpty(attach.SavedAs))
					{
						_viewlink = Utils.GetAttachmentDownloadLink(outputMsg, attach, folderFullName, true, Page.Session);
					}
					else
					{
						_viewlink = Utils.GetAttachmentDownloadLink(attach, true);
					}
					string filename = Path.GetFileName(attach.SavedAs);
					jsMessageInit += "ViewMessage.Attachments.push({" + String.Format(@"Id: {0}, Inline: {1}, FileName: '{2}', Size: {3}, Download: '{4}', View: '{5}', TempName: '{6}', MimeType: '{7}'",
						"-1",
						(attach.IsInline) ? "true" : "false",
						(attach.Filename.Length > 0) ? attach.Filename : attach.Name,
						attach.Size.ToString(),
						_downloadlink,
						_viewlink,
						filename,
						Utils.GetAttachmentMimeTypeFromFileExtension(Path.GetExtension(filename))) + "});\n";
				}
			}

			jsMessageInit += "ViewMessage.SaveLink = '" + Utils.GetMessageDownloadLink(msg, folderId, folderFullName) + "';\n";
			jsMessageInit += "ViewMessage.PrintLink = '" + Utils.GetMessagePrintLink(msg, folderId, folderFullName) + "';\n";
		}
	}
}
