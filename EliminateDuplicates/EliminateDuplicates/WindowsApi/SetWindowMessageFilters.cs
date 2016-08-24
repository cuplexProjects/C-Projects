using System.Runtime.InteropServices;

namespace DeleteDuplicateFiles.WindowsApi
{
    public static class SetWindowMessageFilters
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool ChangeWindowMessageFilter(uint message, uint dwFlag);

        private const uint WM_DROPFILES = 0x233;
        private const uint WM_COPYDATA = 0x004A;
        private const uint WM_COPYGLOBALDATA = 0x0049;
        private const uint MSGFLT_ADD = 1;

        public static void AllowDragAndDropWhenExecutionLevelIsAdministrator()
        {
            ChangeWindowMessageFilter(WM_DROPFILES, MSGFLT_ADD);
            ChangeWindowMessageFilter(WM_COPYDATA, MSGFLT_ADD);
            ChangeWindowMessageFilter(WM_COPYGLOBALDATA, MSGFLT_ADD);
        }
    }
}
