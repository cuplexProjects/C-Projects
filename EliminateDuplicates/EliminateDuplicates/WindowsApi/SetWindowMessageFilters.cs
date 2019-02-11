using System.Runtime.InteropServices;

namespace DeleteDuplicateFiles.WindowsApi
{
    public static class SetWindowMessageFilters
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool ChangeWindowMessageFilter(uint message, uint dwFlag);

        private const uint WmDropfiles = 0x233;
        private const uint WmCopydata = 0x004A;
        private const uint WmCopyglobaldata = 0x0049;
        private const uint MsgfltAdd = 1;

        public static void AllowDragAndDropWhenExecutionLevelIsAdministrator()
        {
            ChangeWindowMessageFilter(WmDropfiles, MsgfltAdd);
            ChangeWindowMessageFilter(WmCopydata, MsgfltAdd);
            ChangeWindowMessageFilter(WmCopyglobaldata, MsgfltAdd);
        }
    }
}
