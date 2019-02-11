using System;
using System.Runtime.Serialization;

namespace SecureMemo.DataModels
{
    [Serializable]
    [DataContract(Name = "SecureMemoAppSettings")]
    public class SecureMemoAppSettings
    {
        [DataMember(Name = "DefaultEmptyTabPages", Order = 1)]
        public int DefaultEmptyTabPages { get; set; }

        [DataMember(Name = "ApplicationSaltValue", Order = 2)]
        public string ApplicationSaltValue { get; set; }

        [DataMember(Name = "AlwaysOntop", Order = 3)]
        public bool AlwaysOntop { get; set; }

        [DataMember(Name = "PasswordDerivedString", Order = 4)]
        public string PasswordDerivedString { get; set; }

        [DataMember(Name = "MainWindowWith", Order = 5)]
        public int MainWindowWith { get; set; }

        [DataMember(Name = "MainWindowHeight", Order = 6)]
        public int MainWindowHeight { get; set; }

        [DataMember(Name = "FontSettings", Order = 7)]
        public SecureMemoFontSettings FontSettings { get; set; }

        [DataMember(Name = "UseSharedSyncFolder", Order = 8)]
        public bool UseSharedSyncFolder { get; set; }

        [DataMember(Name = "SyncFolderPath", Order = 9)]
        public string SyncFolderPath { get; set; }

        [DataMember(Name = "OTPEnabled", Order = 10)]
        public bool OTPEnabled { get; set; }

        [DataMember(Name = "OTPConfigFilename", Order = 11)]
        public string OTPConfigFilename { get; set; }

    }
}