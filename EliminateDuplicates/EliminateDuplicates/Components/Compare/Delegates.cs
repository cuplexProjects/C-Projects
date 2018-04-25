using System;
using System.Collections.Generic;
using System.Linq;
using DeleteDuplicateFiles.DataModels;
using DeleteDuplicateFiles.Managers;

namespace DeleteDuplicateFiles.Models
{
    public delegate void SearchProgressEventHandler(object sender, SearchProgressEventArgs e);

    public delegate void RemoveDeletedHashDbItemsEventHandler(object sender, FileHashRemovalEventArgs e);

    public class DuplicateFileComparer : IComparer<DuplicateFileModel>
    {
        private readonly SearchProfileManager _searchProfileManager;
        private readonly ApplicationSettingsModel.MasterFileSelectionMethods _selection;

        public DuplicateFileComparer(SearchProfileManager searchProfileManager, ApplicationSettingsModel.MasterFileSelectionMethods selection)
        {
            _searchProfileManager = searchProfileManager;
            _selection = selection;
        }

        public int Compare(DuplicateFileModel x, DuplicateFileModel y)
        {
            int compareVal = x.CompareTo(y);
            if (x.CompareTo(y) == 0)
            {
                var activeProfile = _searchProfileManager.SearchProfile;
                if (activeProfile == null)
                    return 0;

                var preferredDirectories = _searchProfileManager.SearchProfile.PreferredDirecoryList;
                

                if (preferredDirectories.Any(pd => x.GetDirectory().StartsWith(pd.Path)))
                {
                    PreferredDirectoryDataModel preferredDirectoryDataModelX =preferredDirectories.First(pd => x.GetDirectory().StartsWith(pd.Path));
                    PreferredDirectoryDataModel preferredDirectoryDataModelY =preferredDirectories.FirstOrDefault(pd => y.GetDirectory().StartsWith(pd.Path));
                    return preferredDirectoryDataModelY == null
                        ? -1
                        : preferredDirectoryDataModelX.SortOrder.CompareTo(preferredDirectoryDataModelY.SortOrder);
                }

                if (_selection == ApplicationSettingsModel.MasterFileSelectionMethods.OldestModifiedDate)
                    return x.LastWriteTime.CompareTo(y.LastWriteTime);

                return x.LastWriteTime.CompareTo(y.LastWriteTime)*-1;
            }
            return compareVal;
        }
    }

    public class SearchProgressEventArgs : EventArgs
    {
        public int PercentComplete { get; set; }
        public string ProgressMessage { get; set; }
    }

    public class FileHashRemovalEventArgs : EventArgs
    {
        public int ItemsRemoved { get; set; }
    }
}