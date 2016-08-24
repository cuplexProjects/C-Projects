using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLinq = CuplexLib.Linq;

namespace CuplexLib
{
    public class EventLog
    {
        public static void SaveToEventLog(string eventText, EventLogType eventLogType, int? userRef)
        {
            try
            {
                using (CLinq.DataContext db = CLinq.DataContext.Create())
                {
                    CLinq.EventLog eventLog = new CuplexLib.Linq.EventLog();
                    eventLog.EventLogDate = DateTime.Now;
                    eventLog.EventText = eventText;
                    eventLog.EventType = (int)eventLogType;
                    eventLog.UserRef = userRef;

                    db.EventLogs.InsertOnSubmit(eventLog);
                    db.SubmitChanges();
                }
            }
            catch { }
        }

    }

    public enum EventLogType
    {
        Information = 1,
        Warning = 2,
        Error = 3
    }
}
