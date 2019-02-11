using System;
using System.IO;
using System.Text;
using System.Web;
using MailBee;
using MailBee.Mime;
using MailBee.Security;
using MailBee.SmtpMail;

namespace WebMail
{
	/// <summary>
    /// This class provide method for sending messages using the SMTP protocol. 
	/// </summary>
	public class Smtp
	{
		// true if successfull, otherwise false
		public static void SendMail(Account account, WebMailMessage message)
		{
			SendMail(account, message, string.Empty);
		}

		public static void SendMail(Account account, WebMailMessage message, string dataFolder)
		{
			WebmailResourceManager resMan;
			if (dataFolder != string.Empty)
			{
				resMan = new WebmailResourceManager(dataFolder);
			}
			else
			{
				resMan = (new WebmailResourceManagerCreator()).CreateResourceManager();
			}

			try
			{
				MailMessage messageToSend = new MailMessage();
				messageToSend.Priority = message.MailBeeMessage.Priority;
                messageToSend.Sensitivity = message.MailBeeMessage.Sensitivity;

				messageToSend.From = message.MailBeeMessage.From;
				messageToSend.To = message.MailBeeMessage.To;
				messageToSend.Cc = message.MailBeeMessage.Cc;
				messageToSend.Bcc = message.MailBeeMessage.Bcc;
				messageToSend.Subject = message.MailBeeMessage.Subject;
				messageToSend.ConfirmRead = message.MailBeeMessage.ConfirmRead;
				messageToSend.Date = DateTime.Now;
				messageToSend.BodyHtmlText = message.MailBeeMessage.BodyHtmlText;
				messageToSend.BodyPlainText = message.MailBeeMessage.BodyPlainText;

				

				if (HttpContext.Current != null)
				{
					string str = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
					messageToSend.Headers.Add("X-Originating-IP", str, false);
				}

				foreach (Attachment attach in message.MailBeeMessage.Attachments)
				{
					messageToSend.Attachments.Add(attach);
				}

				WebmailSettings settings;
				if (dataFolder != string.Empty)
				{
					settings = (new WebMailSettingsCreator()).CreateWebMailSettings(dataFolder);
				}
				else
				{
					settings = (new WebMailSettingsCreator()).CreateWebMailSettings();
				}
                
                
                MailBee.SmtpMail.Smtp.LicenseKey  = settings.LicenseKey;
                

                MailBee.SmtpMail.Smtp objSmtp = new MailBee.SmtpMail.Smtp();

				if (settings.EnableLogging)
				{
					objSmtp.Log.Enabled = true;

					if (dataFolder != string.Empty)
					{
						objSmtp.Log.Filename = Path.Combine(dataFolder, Constants.logFilename);
					}
					else
					{
						string dataFolderPath = Utils.GetDataFolderPath();
						if (dataFolderPath != null)
						{
							objSmtp.Log.Filename = Path.Combine(dataFolderPath, Constants.logFilename);
						}
					}
				}

				SmtpServer server = new SmtpServer();
//                server.SmtpOptions = ExtendedSmtpOptions.NoChunking; 
                server.Name = !string.IsNullOrEmpty(account.MailOutgoingHost) ? account.MailOutgoingHost : account.MailIncomingHost;
				server.Port = account.MailOutgoingPort;
				server.AccountName = !string.IsNullOrEmpty(account.MailOutgoingLogin) ? account.MailOutgoingLogin : account.MailIncomingLogin;
				server.Password = (!string.IsNullOrEmpty(account.MailOutgoingPassword)) ? account.MailOutgoingPassword : account.MailIncomingPassword;
				
                if (account.MailOutgoingAuthentication)
				{
					server.AuthMethods = AuthenticationMethods.Auto;
					server.AuthOptions = AuthenticationOptions.PreferSimpleMethods;
				}
				
                if (server.Port == 465)
				{
					server.SslMode = SslStartupMode.OnConnect;
					server.SslProtocol = SecurityProtocol.Auto;
					server.SslCertificates.AutoValidation = CertificateValidationFlags.None;
				}
                
                if (server.Port == 587 && Constants.UseStartTLS)
                {
                    server.SslMode = SslStartupMode.UseStartTlsIfSupported;
                }

				objSmtp.SmtpServers.Add(server);

                Encoding outgoingCharset = Utils.GetOutCharset(account.UserOfAccount.Settings);
				messageToSend.Charset = outgoingCharset.HeaderName;

				objSmtp.Message = messageToSend;

				try
				{
					objSmtp.Connect();

					objSmtp.Send();
				}
				catch (MailBeeGetHostNameException ex)
				{
                    Log.WriteException(ex);
                    throw new WebMailException(resMan.GetString("ErrorSMTPConnect"));
				}
				catch (MailBeeConnectionException ex)
				{
                    Log.WriteException(ex);
                    throw new WebMailException(resMan.GetString("ErrorSMTPConnect"));
				}
				catch (MailBeeSmtpLoginBadCredentialsException ex)
				{
                    Log.WriteException(ex);
                    throw new WebMailException(resMan.GetString("ErrorSMTPAuth"));
				}
				catch (MailBeeException ex)
				{
                    Log.WriteException(ex);
                    throw new WebMailMailBeeException(ex);
				}
				finally
				{
					if (objSmtp.IsConnected) objSmtp.Disconnect();
				}
			}
			catch (MailBeeException ex)
			{
                Log.WriteException(ex);
                throw new WebMailMailBeeException(ex);
			}
		}

	}// END CLASS DEFINITION Smtp
}
