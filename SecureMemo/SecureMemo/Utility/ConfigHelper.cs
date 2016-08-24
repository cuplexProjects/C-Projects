using System.Drawing;
using System.Security.Cryptography;
using GeneralToolkitLib.Converters;
using SecureMemo.DataModels;

namespace SecureMemo.Utility
{
    public static class ConfigHelper
    {
        private const int EMPTY_TAB_PAGES = 3;
        private const int WIN_WIDTH = 400;
        private const int WIN_HEIGHT = 300;
        private const bool ALLWAYS_ONTOP = false;

        private static string CreateApplicationSalt()
        {
            var randomBytes = new byte[512];
            using (RandomNumberGenerator random = RandomNumberGenerator.Create())
            {
                random.GetBytes(randomBytes);
                return GeneralConverters.ByteArrayToHexString(randomBytes);
            }
        }

        public static SecureMemoAppSettings GetDefaultSettings()
        {
            var appSettings = new SecureMemoAppSettings {AlwaysOntop = ALLWAYS_ONTOP, ApplicationSaltValue = CreateApplicationSalt(), DefaultEmptyTabPages = EMPTY_TAB_PAGES};
            var fontSettings = new SecureMemoFontSettings {FontFamily = new FontFamily("Courier New"), FontSize = 9.75f, Style = FontStyle.Regular};
            fontSettings.FontFamilyName = fontSettings.FontFamily.GetName(0);
            appSettings.FontSettings = fontSettings;
            appSettings.MainWindowHeight = WIN_HEIGHT;
            appSettings.MainWindowWith = WIN_WIDTH;
            appSettings.PasswordDerivedString = null;

            return appSettings;
        }
    }
}