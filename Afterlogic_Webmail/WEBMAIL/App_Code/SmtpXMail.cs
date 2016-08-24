using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using MailBee;
using MailBee.Mime;
using MailBee.Security;
using MailBee.SmtpMail;
using System.IO;
using System.Text;

namespace WebMail
{
    /// <summary>
    /// This class implements an API for sending mails
    /// through filesystem (AfterLogic XMail Server spool).
    /// </summary>
    public class SmtpXMail
    {
        public static string WmServerRootPath
        {
            get
            {
                WebmailSettings settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
                return settings.WmServerRootPath;
            }
        }

        public static void SendMail(Account account, WebMailMessage message)
        {
            Random rnd = new Random();
            int pid = rnd.Next(1000, 9999);
            string MessageFileName = Utils.GetUnixTimeStamp().ToString() + "-001." + pid.ToString() + ".LOCALXMAIL";
            string MessageTempFileNamePath = Path.Combine(Path.Combine(WmServerRootPath, @"spool\temp\"), MessageFileName);
            string MessageFileNamePath = Path.Combine(Path.Combine(WmServerRootPath, @"spool\local\"), MessageFileName);

            /*for demo*/
            const string demoDomain = "afterlogic.com";
            const string demoErrorMessage = "For security reasons, sending e-mail from this account to external " +
                                            "addresses is disabled. Please send to demopro@afterlogic.com or relogin in Advanced Login mode using your " +
                                            "mail account on another mail server.";

            if (account.IsDemo)
            {
                for (int i = 0; i < message.MailBeeMessage.To.Count; i++)
                {
                    int dogPos = message.MailBeeMessage.To[i].Email.IndexOf("@");
                    string domain = message.MailBeeMessage.To[i].Email.Substring(dogPos + 1);
                    if (domain != demoDomain)
                    {
                        throw new WebMailException(demoErrorMessage);
                    }
                }
                for (int i = 0; i < message.MailBeeMessage.Cc.Count; i++)
                {
                    int dogPos = message.MailBeeMessage.Cc[i].Email.IndexOf("@");
                    string domain = message.MailBeeMessage.Cc[i].Email.Substring(dogPos + 1);
                    if (domain != demoDomain)
                    {
                        throw new WebMailException(demoErrorMessage);
                    }
                }
                for (int i = 0; i < message.MailBeeMessage.Bcc.Count; i++)
                {
                    int dogPos = message.MailBeeMessage.Bcc[i].Email.IndexOf("@");
                    string domain = message.MailBeeMessage.Bcc[i].Email.Substring(dogPos + 1);
                    if (domain != demoDomain)
                    {
                        throw new WebMailException(demoErrorMessage);
                    }
                }
            }

            StreamWriter sw = File.CreateText(MessageTempFileNamePath);
            sw.WriteLine(string.Format(@"mail from:<{0}>", message.MailBeeMessage.From.Email));

            sw.WriteLine(string.Format(@"mail from:<{0}>", account.Email));

            foreach (EmailAddress eAddress in message.MailBeeMessage.To)
            {
                sw.WriteLine(string.Format(@"rcpt to:<{0}>", eAddress.Email));
            }

            foreach (EmailAddress eAddressCc in message.MailBeeMessage.Cc)
            {
                sw.WriteLine(string.Format(@"rcpt to:<{0}>", eAddressCc.Email));
            }

            foreach (EmailAddress eAddressBcc in message.MailBeeMessage.Bcc)
            {
                sw.WriteLine(string.Format(@"rcpt to:<{0}>", eAddressBcc.Email));
            }

            message.MailBeeMessage.Bcc.Clear();

            sw.WriteLine();
            sw.Write(Encoding.UTF8.GetString(message.MailBeeMessage.GetMessageRawData()));
            sw.WriteLine();
            sw.Close();

            File.Move(MessageTempFileNamePath, MessageFileNamePath);
        }
    }
}
