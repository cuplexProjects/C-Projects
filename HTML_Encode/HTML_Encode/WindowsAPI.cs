using System;
using System.Runtime.InteropServices;
using System.Text;

namespace HTML_Encode
{
    public static class WindowsAPI
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetOpenClipboardWindow();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("user32.dll")]
        public static extern bool EmptyClipboard();

        [DllImport("kernel32.dll")]
        public static extern Int32 MultiByteToWideChar(UInt32 CodePage, UInt32 dwFlags, [MarshalAs(UnmanagedType.LPStr)] String lpMultiByteStr, Int32 cbMultiByte, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpWideCharStr, Int32 cchWideChar);


        [DllImport("user32.dll", SetLastError = true)]
        public static extern int SetClipboardData(int uFormat, IntPtr hMem);

        public enum uFormat
        {
            CF_TEXT = 1,
            CF_BITMAP = 2,
            CF_SYLK = 4,
            CF_DIF = 5,
            CF_TIFF = 6,
            CF_OEMTEXT = 7,
            CF_DIB = 8,
            CF_PALETTE = 9,
            CF_PENDATA = 10,
            CF_RIFF = 11,
            CF_WAVE = 12,
            CF_UNICODETEXT = 13,
        }
    }
}
