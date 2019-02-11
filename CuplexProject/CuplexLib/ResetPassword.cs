using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLinq = CuplexLib.Linq;
using System.Net.Mail;
using System.Configuration;
using System.Security.Cryptography;

namespace CuplexLib
{
    public class PasswordReset
    {
        public static bool SendPasswordResetEmail(string emailAddress)
        {
            try
            {
                using (var db = CLinq.DataContext.Create())
                {
                    CLinq.User user = db.Users.Where(u => u.EmailAddress == emailAddress).Take(1).SingleOrDefault();

                    if (user == null)
                        return false;

                    //create reset id
                    string resetIdentyfier = Utils.GetMd5Hash(user.UserRef + user.UserName + emailAddress) + Utils.GetMd5Hash(DateTime.Now.Ticks.ToString() + GetRandomString(32));
                    CLinq.PasswordReset passwordReset = new CuplexLib.Linq.PasswordReset();
                    passwordReset.ResetIdentyfier = resetIdentyfier;
                    passwordReset.UserRef = user.UserRef;
                    db.PasswordResets.InsertOnSubmit(passwordReset);
                    db.SubmitChanges();

                    string messageBody = "Klicka på följande länk för att återställa ditt lösenord.&nbsp;<BR/><a href='" + ConfigurationSettings.AppSettings["DomainUrl"] + "/ResetPassword.aspx?resetId=" + resetIdentyfier + "'>Återställ lösenord »</a>";
                    MailMessage email = new MailMessage("noreply@cuplex.se", emailAddress, "Återställ ditt lösenord på Cuplex.se", messageBody);
                    email.IsBodyHtml = true;

                    SmtpClient smtpClient = new SmtpClient();
                    smtpClient.Host = ConfigurationSettings.AppSettings["SMTPServer"];

                    if (!string.IsNullOrEmpty(ConfigurationSettings.AppSettings["SMTPAccountName"]))
                    {
                        string accountName = ConfigurationSettings.AppSettings["SMTPAccountName"];
                        string accountPassword = ConfigurationSettings.AppSettings["SMTPAccountPassword"];

                        System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(accountName, accountPassword);
                        smtpClient.Credentials = SMTPUserInfo;
                    }

                    smtpClient.Send(email); 
                }
                return true;
            }
            catch (Exception ex)
            {
                EventLog.SaveToEventLog(ex.Message, EventLogType.Error, null);
                return false;
            }
        }
        public static string GetRandomString(uint length)
        {
            RNGCryptoServiceProvider _rng = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[length];

            _rng.GetBytes(buffer);
            return BitConverter.ToString(buffer);
        }
    }
}