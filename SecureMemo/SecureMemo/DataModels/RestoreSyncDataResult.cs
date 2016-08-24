using System;

namespace SecureMemo.DataModels
{
    public class RestoreSyncDataResult
    {
        public bool Successful { get; set; }
        public RestoreSyncDataErrorCodes ErrorCode { get; set; }
        public string ErrorText { get; set; }
    }

    [Flags]
    public enum RestoreSyncDataErrorCodes
    {
        None = 0,
        ApplicationSettingsFileNotFound = 1,
        ApplicationSettingsFileParseError = 2,
        MemoDatabaseFileNotFound = 4,
        MemoDatabaseFileParseError = 8
    }
}