using System.Configuration;
using OrganaizeTV_Series.Models;

namespace OrganaizeTV_Series.Configuration
{
    public class AppSettingsManager
    {
        public AppSettingsManager()
        {
            Init();
        }
        private const string FolderNameForUnmanagedItems = "ITEMS_WHICH_COULD_NOT_BE_PROCESSED";

        public SettingsModel Settings { get; private set; }

        private void Init()
        {
            //Set Default config
            var config = new SettingsModel {UnmanagedItemsFolderName = FolderNameForUnmanagedItems};

            string folderNameFromConfig = ConfigurationManager.AppSettings.Get("UnprocessedContentDirName");
            if (!string.IsNullOrEmpty(folderNameFromConfig) && ValidateFolderNameSyntax(folderNameFromConfig))
            {
                config.UnmanagedItemsFolderName = folderNameFromConfig;
            }

            
            Settings = config;
        }

        private bool ValidateFolderNameSyntax(string name)
        {
            return false;
        }

    }
}
