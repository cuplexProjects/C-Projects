using System;
using System.Collections.Generic;

namespace CloseAllDemoApps
{
    public interface IEventLogSource
    {
        List<string> GetLogsList();
        event EventHandler OnEventLogUpdated;
    }
}
