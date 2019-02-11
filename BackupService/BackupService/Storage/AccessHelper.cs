using BackupService.Utils;
using System;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;

namespace BackupService.Storage
{
    public class AccessHelper
    {
        private readonly WindowsIdentity _winId;
        public AccessHelper()
        {
            _winId = WindowsIdentity.GetCurrent();
        }

        public bool UserHasReadAccessToDirectory(DirectoryInfo directoryInfo)
        {
            try
            {
                DirectorySecurity dSecurity = directoryInfo.GetAccessControl();
                var authorizarionRuleCollecion = dSecurity.GetAccessRules(true, true, typeof(SecurityIdentifier));

                foreach (FileSystemAccessRule fsAccessRules in authorizarionRuleCollecion)
                {
                    if (_winId.UserClaims.Any(c => c.Value == fsAccessRules.IdentityReference.Value) &&
                        fsAccessRules.FileSystemRights.HasFlag(FileSystemRights.ReadData) && fsAccessRules.AccessControlType == AccessControlType.Allow)
                        return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                LogWriter.WriteLog("Errorr in DirectoryData.UserHasReadAccessToDirectory - " + ex.Message);
            }
            return false;
        }
    }
}
