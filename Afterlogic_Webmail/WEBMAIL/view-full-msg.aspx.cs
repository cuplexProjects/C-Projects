using System;

namespace WebMail
{
	public partial class view_full_msg : System.Web.UI.Page
	{
		protected WebmailResourceManager _manager = null;
		protected bool _stylesRtl = false;
		protected string defaultSkin = string.Empty;

		protected void Page_Load(object sender, EventArgs e)
		{
			bool showPicturesSettings = false;
			Account acct = Session[Constants.sessionAccount] as Account;
			if (acct == null)
			{
				Response.Redirect("default.aspx", true);
			}
			else
			{
				try
				{
					if (acct.UserOfAccount.Settings.RTL)
					{
						_stylesRtl = true;
					}
					defaultSkin = Utils.EncodeJsSaveString(acct.UserOfAccount.Settings.DefaultSkin);
					_manager = (new WebmailResourceManagerCreator()).CreateResourceManager();
					int id = Convert.ToInt32(Request.QueryString["id"]);
					string uid = Request.QueryString["uid"];
					long id_folder = Convert.ToInt64(Request.QueryString["id_folder"]);
					string full_name_folder = Request.QueryString["full_name_folder"];
					int charset = Convert.ToInt32(Request.QueryString["charset"]);
					if ((acct.UserOfAccount != null) && (acct.UserOfAccount.Settings != null))
					{
						showPicturesSettings = ((acct.UserOfAccount.Settings.ViewMode & ViewMode.AlwaysShowPictures) > 0) ? true : false;
					}

					WebMailMessage msg;
					BaseWebMailActions bwml = new BaseWebMailActions(acct, Page);
					if (showPicturesSettings)
						msg = bwml.GetMessage(id, uid, id_folder, charset, 1, false, false, false, MessageMode.None);
					else
                        msg = bwml.GetMessage(id, uid, id_folder, charset, 1, false, false, false, MessageMode.None);
					if (msg != null)
					{
						LabelFrom.Text = Server.HtmlEncode(msg.FromMsg.ToString());
						LabelTo.Text = Server.HtmlEncode(msg.ToMsg.ToString());
						DateFormatting df = new DateFormatting(acct, msg.MsgDate);
						LabelDate.Text = df.FullDate;
						LabelSubject.Text = Server.HtmlEncode(msg.Subject);
						string msgPlain = msg.MailBeeMessage.BodyPlainText;
						string msgHTML = msg.MailBeeMessage.BodyHtmlText;

						switch (Request.QueryString["bodytype"])
						{
							case "1":
								MessageBody.Text = msgHTML;
								break;
							default:
								MessageBody.Text = string.Format("<pre>{0}</pre>", msgPlain);
								break;
						}
					}
				}
				catch (Exception ex)
				{
					Log.WriteException(ex);
					Response.Write(ex.Message);
				}
			}
		}
	}
}
