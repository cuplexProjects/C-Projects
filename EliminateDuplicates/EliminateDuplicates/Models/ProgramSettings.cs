#region

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using DeleteDuplicateFiles.Annotations;

#endregion

namespace DeleteDuplicateFiles.Models
{
    [Serializable]
    [DataContract]
    public sealed class ProgramSettings : INotifyPropertyChanged
    {
        public enum DeletionModes
        {
            Permanent = 1,
            RecycleBin = 2
        }

        public enum HashAlgorithms
        {
            CRC32 = 1,
            MD5 = 2
        }

        public enum MasterFileSelectionMethods
        {
            OldestModifiedDate = 1,
            NewestModifiedDate = 2
        }

        private DeletionModes _deletionMode;
        private HashAlgorithms _hashAlgorithm;
        private bool _ignoreHiddenFilesAndDirectories;
        private bool _ignoreSystemFilesAndDirectories;
        private string _lastProfileFilePath;
        private MasterFileSelectionMethods _masterFileSelectionMethod;
        private int _maximumNoOfHashingThreads;

        public ProgramSettings()
        {
            //Default value
            MaximumNoOfHashingThreads = Environment.ProcessorCount;
            IgnoreSystemFilesAndDirectories = true;
            LastProfileFilePath = null;
        }

        [DataMember(Name = "HashAlgorithm", Order = 1)]
        public HashAlgorithms HashAlgorithm
        {
            get => _hashAlgorithm;
            set
            {
                if (_hashAlgorithm != value)
                    OnPropertyChanged(nameof(HashAlgorithm));

                _hashAlgorithm = value;
            }
        }

        [DataMember(Name = "MasterFileSelectionMethod", Order = 2)]
        public MasterFileSelectionMethods MasterFileSelectionMethod
        {
            get => _masterFileSelectionMethod;
            set
            {
                if (_masterFileSelectionMethod != value)
                    OnPropertyChanged(nameof(MasterFileSelectionMethod));

                _masterFileSelectionMethod = value;
            }
        }

        [DataMember(Name = "DeletionMode", Order = 3)]
        public DeletionModes DeletionMode
        {
            get => _deletionMode;
            set
            {
                if (_deletionMode != value)
                    OnPropertyChanged(nameof(DeletionMode));

                _deletionMode = value;
            }
        }

        [DataMember(Name = "MaximumNoOfHashingThreads", Order = 4)]
        public int MaximumNoOfHashingThreads
        {
            get => _maximumNoOfHashingThreads;
            set
            {
                if (_maximumNoOfHashingThreads != value)
                    OnPropertyChanged(nameof(MaximumNoOfHashingThreads));

                _maximumNoOfHashingThreads = value;
            }
        }

        [DataMember(Name = "IgnoreHiddenFilesAndDirectories", Order = 5)]
        public bool IgnoreHiddenFilesAndDirectories
        {
            get => _ignoreHiddenFilesAndDirectories;
            set
            {
                if (_ignoreHiddenFilesAndDirectories != value)
                    OnPropertyChanged(nameof(IgnoreHiddenFilesAndDirectories));

                _ignoreHiddenFilesAndDirectories = value;
            }
        }

        [DataMember(Name = "IgnoreSystemFilesAndDirectories", Order = 6)]
        public bool IgnoreSystemFilesAndDirectories
        {
            get => _ignoreSystemFilesAndDirectories;
            set
            {
                if (_ignoreSystemFilesAndDirectories != value)
                    OnPropertyChanged(nameof(IgnoreSystemFilesAndDirectories));
                _ignoreSystemFilesAndDirectories = value;
            }
        }

        [DataMember(Name = "LastProfileFilePath", Order = 7)]
        public string LastProfileFilePath
        {
            get => _lastProfileFilePath;
            set
            {
                if (_lastProfileFilePath != value)
                    OnPropertyChanged(nameof(LastProfileFilePath));

                _lastProfileFilePath = value;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}