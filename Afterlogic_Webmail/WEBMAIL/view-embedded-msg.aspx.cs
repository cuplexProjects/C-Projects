using System;
using MailBee.Mime;
using System.Text;
using System.IO;

namespace WebMail
{
    public partial class view_embedded_msg : System.Web.UI.Page
    {
        protected WebmailResourceManager _manager = null;
        protected MailMessage msg = new MailMessage();
        Account acct = null;
        protected bool IsHTMLMsg = false;
        protected string msgPlain = null;
        protected string msgHTML = null;
        protected string CcAddr = string.Empty;
        protected string Attachments = string.Empty;
        protected string FileName = string.Empty;
		protected bool _stylesRtl = false;
		protected string defaultSkin = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
			acct = Session[Constants.sessionAccount] as Account;
            if (acct == null)
            {
                Response.Redirect("default.aspx", true);
            }
            else
            {
				if (acct.UserOfAccount.Settings.RTL)
				{
					_stylesRtl = true;
				}
				defaultSkin = Utils.EncodeJsSaveString(acct.UserOfAccount.Settings.DefaultSkin);
				_manager = (new WebmailResourceManagerCreator()).CreateResourceManager();

                if (Request.QueryString["filename"] != null)
                    FileName = Request.QueryString["filename"];

                try
                {
                    IsHTMLMsg = false;
                    msgPlain = null;
                    msgHTML = null;

                    string tempFolder = Utils.GetTempFolderName(Page.Session);
                    string FilePath = Path.Combine(tempFolder, FileName);
                    
                    msg.Parser.WorkingFolder = tempFolder;
                    msg.LoadMessage(FilePath);

                    LabelFrom.Text = Server.HtmlEncode(msg.From.ToString());
                    LabelTo.Text = Server.HtmlEncode(msg.To.ToString());
                    CcAddr = msg.Cc.ToString();
                    if (CcAddr != string.Empty)
                    {
                        LabelCc.Text = Server.HtmlEncode(msg.Cc.ToString());
                    }
					DateFormatting df = new DateFormatting(acct, msg.Date);
                    LabelDate.Text = df.FullDate;
                    LabelSubject.Text = Server.HtmlEncode(msg.Subject);

                    AttachmentCollection AttachmentsColl = msg.Attachments;

                    StringBuilder sb = new StringBuilder();
                    foreach (Attachment Attach in AttachmentsColl)
                    {
                        string filename = Utils.CreateTempFilePath(tempFolder,
                            (Attach.Filename.Length > 0) ? Attach.Filename : Attach.Name);
                        Attach.Save(filename, true);

                        FileInfo fi = new FileInfo(filename);

                        if (Attach.ContentType.ToLower().StartsWith("image"))
                        {
                            sb.AppendFormat(@"<a href=""{1}"">{0}</a>", Attach.Filename, Utils.GetAttachmentDownloadLink(Attach, false));
                            sb.AppendFormat(@" (<a href=""{0}"">View</a>), ", Utils.GetAttachmentDownloadLink(Attach, true));
                        }
                        else if (Attach.ContentType.ToLower().StartsWith("message"))
                        {
                            sb.AppendFormat(@"<a href=""{1}"">{0}</a>", Attach.Filename, Utils.GetAttachmentDownloadLink(Attach, false));
                            sb.AppendFormat(@" (<a href=""view-embedded-msg.aspx?filename={0}"">View</a>), ", fi.Name);
                        }
                        else
                        {
                            sb.AppendFormat(@"<a href=""{1}"">{0}</a>, ", Attach.Filename, Utils.GetAttachmentDownloadLink(Attach, false));
                        }
                    }
                    if (sb.Length > 2) sb.Remove(sb.Length - 2, 2);
                    Attachments = sb.ToString();
                    if (Attachments.Length > 0)
                    {
                        LabelAttachments.Text = Attachments;
                    }

                    msgPlain = Utils.MakeHtmlBodyFromPlainBody(msg.BodyPlainText, true, "");
                    msgHTML = msg.BodyHtmlText;
                    if (msgPlain == "")
                    {
                        msgPlain = Utils.ConvertHtmlToPlain(msgHTML);
                    }
                    if (msgHTML != "")
                    {
                        IsHTMLMsg = true;
                        //Message body
                        MessageBody.Text = msgHTML;
                    }
                    else
                    {
                        IsHTMLMsg = false;
                        MessageBody.Text = msgPlain;
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex);
                }            
            }

        }
    }
}
