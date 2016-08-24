using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureChat.Common
{
    public static class Enums
    {

        public enum ServerStatusFlags
        {
            Initializing = 1,
            DbConnectionSuccessful = 2,
            Configured = 3,
            TryingToConnectToDatabase = 4,
            DbConnectionFailed = 255,
            NotConfigured = 256,
        }
    }
}
