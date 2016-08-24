#region

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace DeleteDuplicateFiles.Models
{
    public delegate void SearchProgressEventHandler(object sender, SearchProgressEventArgs e);

    public delegate void RemoveDeletedHashDBItemsEventHandler(object sender, FileHashRemovalEventArgs e);

    public class DuplicateFileComparer : IComparer<DuplicateFile>
    {
        private readonly SearchProfileManager _searchProfileManager;
        private readonly ProgramSettings.MasterFileSelectionMethods _selection;

        public DuplicateFileComparer(SearchProfileManager searchProfileManager, ProgramSettings.MasterFileSelectionMethods selection)
        {
            _searchProfileManager = searchProfileManager;
            _selection = selection;
        }

        public int Compare(DuplicateFile x, DuplicateFile y)
        {
            int compareVal = x.CompareTo(y);
            if (x.CompareTo(y) == 0)
            {
                var activeProfile = _searchProfileManager.CurrentProfile;
                if (activeProfile == null)
                    return 0;

                var preferredDirectories = _searchProfileManager.CurrentProfile.PreferredDirecoryList;
                

                if (preferredDirectories.Any(pd => x.Dir.StartsWith(pd.Path)))
                {
                    PreferredDirectory preferredDirectoryX =preferredDirectories.First(pd => x.Dir.StartsWith(pd.Path));
                    PreferredDirectory preferredDirectoryY =preferredDirectories.FirstOrDefault(pd => y.Dir.StartsWith(pd.Path));
                    return preferredDirectoryY == null
                        ? -1
                        : preferredDirectoryX.SortOrder.CompareTo(preferredDirectoryY.SortOrder);
                }

                if (_selection == ProgramSettings.MasterFileSelectionMethods.OldestModifiedDate)
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