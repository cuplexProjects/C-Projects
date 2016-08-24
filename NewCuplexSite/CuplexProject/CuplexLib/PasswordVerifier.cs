using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CLinq = CuplexLib.Linq;

namespace CuplexLib
{
    public class PasswordVerifier
    {
        public int MinimumPasswordCapitalLetters { get; private set; }
        public int MinimumPasswordDigits { get; private set; }
        public int MinimumPasswordLength { get; private set; }

        public enum PasswordVerifierResult
        {
            PasswordIsNullOrEmpty,
            PasswordIsToShort,
            PasswordDigitCountToLow,
            PasswordCapitalLettersCountToLow,
            PasswordIsOk
        }

        public PasswordVerifier()
        {
            initSettings();
        }

        public PasswordVerifierResult VerifyPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return PasswordVerifierResult.PasswordIsNullOrEmpty;
            if (password.Length < MinimumPasswordLength)
                return PasswordVerifierResult.PasswordIsToShort;

            Regex digitVerifier = new Regex("[0-9]");
            if (digitVerifier.Matches(password).Count < MinimumPasswordDigits)
                return PasswordVerifierResult.PasswordDigitCountToLow;

            Regex capitalLettersVerifier = new Regex("[A-Z]|[Å-Ö]");
            if (capitalLettersVerifier.Matches(password).Count < MinimumPasswordCapitalLetters)
                return PasswordVerifierResult.PasswordCapitalLettersCountToLow;

            return PasswordVerifierResult.PasswordIsOk;
        }

        private void initSettings()
        {
            try
            {
                using (var db = CLinq.DataContext.Create())
                {
                    int tmp = 0;
                    CLinq.Setting setting = db.Settings.Where(s => s.KeyType == "MinimumPasswordCapitalLetters").SingleOrDefault();
                    if (setting != null)
                    {
                        int.TryParse(setting.Value, out tmp);
                        MinimumPasswordCapitalLetters = tmp;
                    }

                    setting = db.Settings.Where(s => s.KeyType == "MinimumPasswordDigits").SingleOrDefault();
                    if (setting != null)
                    {
                        int.TryParse(setting.Value, out tmp);
                        MinimumPasswordDigits = tmp;
                    }

                    setting = db.Settings.Where(s => s.KeyType == "MinimumPasswordLength").SingleOrDefault();
                    if (setting != null)
                    {
                        int.TryParse(setting.Value, out tmp);
                        MinimumPasswordLength = tmp;
                    }
                }
            }
            catch (Exception ex)
            {
                EventLog.SaveToEventLog(ex.Message, EventLogType.Error, null);
            }
        }
    }
    public class UserNameVerifier
    {
        public static bool VerifyUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return false;
            if (userName.Length > 50)
                return false;

            Regex userNameVerifier = new Regex("[^a-zA-Z0-9_-]");
            return userNameVerifier.Matches(userName).Count == 0;
        }
    }
}
