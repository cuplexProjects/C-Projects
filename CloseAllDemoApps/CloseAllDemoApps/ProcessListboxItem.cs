using System;

namespace CloseAllDemoApps
{
    public class ProcessListboxItem
    {
        public int PID { get; set; }
        public string ProcessName { get; set; }
        public string MainWindowTitle { get; set; }

        public override string ToString()
        {
            return String.Format("Pid:{0} - ProcessName: {1}, MainWindowTitle: {2}", PID, ProcessName, MainWindowTitle);
        }
    }
}
