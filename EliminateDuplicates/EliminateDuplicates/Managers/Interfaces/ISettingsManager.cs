using DeleteDuplicateFiles.Models;

namespace DeleteDuplicateFiles.Managers.Interfaces
{
    interface ISettingsManager
    {
        void LoadSettings();

        void SaveSettings();

        ApplicationSettingsModel ApplicationSettings { get; }

    }
}
