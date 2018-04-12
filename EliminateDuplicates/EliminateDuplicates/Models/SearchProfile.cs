using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using DeleteDuplicateFiles.Annotations;

namespace DeleteDuplicateFiles.Models
{
    [Serializable]
    [DataContract]
    public class SearchProfile : INotifyPropertyChanged, IDisposable
    {
        [DataMember(Order = 1, IsRequired = true, Name = "ScanFolderList")]
        private List<ScanFolderListItem> _scanFolderList;

        [DataMember(Order = 2, IsRequired = true, Name = "PreferredDirecoryList")]
        private List<PreferredDirectory> _preferredDirecoryList;

        [DataMember(Order = 3, IsRequired = true, Name = "FileNameFilter")]
        private string _fileNameFilter;

        [DataMember(Order = 4, IsRequired = true, Name = "ProfileName")]
        private string _profileName;

        protected SearchProfile()
        {

        }

        public List<ScanFolderListItem> ScanFolderList => _scanFolderList;

        public List<PreferredDirectory> PreferredDirecoryList
        {
            get => _preferredDirecoryList;
            set
            {
                _preferredDirecoryList = value;
                OnPropertyChanged(nameof(PreferredDirecoryList));
            }
        }

        public string FileNameFilter
        {
            get => _fileNameFilter;
            set
            {
                _fileNameFilter = value;
                OnPropertyChanged(nameof(FileNameFilter));
            }
        }

        public string ProfileName
        {
            get => _profileName;
            set
            {
                _profileName = value;
                OnPropertyChanged(nameof(ProfileName));
            }
        }

        public void ScanFolderListUpdated()
        {
            OnPropertyChanged(nameof(ScanFolderList));
        }

        //No need to save this property
        public string FullPath { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal static SearchProfile CreateDefaultProfile(string profileName)
        {
            var profile = new SearchProfile { ProfileName = profileName };
            profile.CreateNewScanFolderList();
            profile.PreferredDirecoryList = new List<PreferredDirectory>();

            return profile;
        }

        private void CreateNewScanFolderList()
        {
            _preferredDirecoryList = new List<PreferredDirectory>();
            _scanFolderList = new List<ScanFolderListItem>();
        }

        public void SetExistingScanFolderList(List<ScanFolderListItem> scanFolderList)
        {
            _scanFolderList = scanFolderList;
        }

        public void Dispose()
        {
            PropertyChanged = null;
        }
    }
}
