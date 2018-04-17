using System;
using System.Runtime.Serialization;

namespace DeleteDuplicateFiles.DataModels
{
    [Serializable]
    [DataContract]
    public class ApplicationSettingsDataModel
    {
        [DataMember(Name = "HashAlgorithm", Order = 1)]
        public int HashAlgorithm { get; set; }

        [DataMember(Name = "MasterFileSelectionMethod", Order = 2)]
        public int MasterFileSelectionMethod { get; set; }

        [DataMember(Name = "DeletionMode", Order = 3)]
        public int DeletionMode { get; set; }


        [DataMember(Name = "MaximumNoOfHashingThreads", Order = 4)]
        public int MaximumNoOfHashingThreads { get; set; }


        [DataMember(Name = "IgnoreHiddenFilesAndDirectories", Order = 5)]
        public bool IgnoreHiddenFilesAndDirectories { get; set; }


        [DataMember(Name = "IgnoreSystemFilesAndDirectories", Order = 6)]
        public bool IgnoreSystemFilesAndDirectories { get; set; }


        [DataMember(Name = "LastProfileFilePath", Order = 7)]
        public string LastProfileFilePath { get; set; }
    }
}