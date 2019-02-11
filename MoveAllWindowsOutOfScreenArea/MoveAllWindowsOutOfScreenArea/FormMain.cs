using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace MoveAllWindowsOutOfScreenArea
{
    public partial class FormMain : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string strClassName, string strWindowName);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        public class SearchData
        {
            // You can put any dicks or Doms in here...
            public string Wndclass;
            public string Title;
            public IntPtr hWnd;
        }

        private delegate bool EnumWindowsProc(IntPtr hWnd, ref SearchData data);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, ref SearchData data);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        



        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumChildWindows(IntPtr hwndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam);

        private List<IntPtr> GetChildWindows(IntPtr parent)
        {
            List<IntPtr> result = new List<IntPtr>();
            GCHandle listHandle = GCHandle.Alloc(result);
            try
            {
                EnumWindowsProc childProc = new EnumWindowsProc(EnumProc);
                EnumChildWindows(parent, childProc, GCHandle.ToIntPtr(listHandle));
            }
            finally
            {
                if (listHandle.IsAllocated)
                    listHandle.Free();
            }
            return result;
        }

        public static bool EnumProc(IntPtr hWnd, ref SearchData data)
        {
            // Check classname and title 
            // This is different from FindWindow() in that the code below allows partial matches
            StringBuilder sb = new StringBuilder(1024);
            GetClassName(hWnd, sb, sb.Capacity);
            if (sb.ToString().StartsWith(data.Wndclass))
            {
                sb = new StringBuilder(1024);
                GetWindowText(hWnd, sb, sb.Capacity);
                if (sb.ToString().StartsWith(data.Title))
                {
                    data.hWnd = hWnd;
                    return false;    // Found the wnd, halt enumeration
                }
            }
            return true;
        }

        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }

        public FormMain()
        {
            InitializeComponent();
        }

        private void btnFindWindows_Click(object sender, EventArgs e)
        {
            UpdateProcessList();

        }

        private void UpdateProcessList()
        {
            try
            {
                listBoxWindows.Items.Clear();
                Process[] processes = Process.GetProcesses();
                foreach (Process process in processes)
                {
                    try
                    {
                        if (process.MainWindowHandle != IntPtr.Zero)
                        {
                            List<IntPtr> childWindows = GetChildWindows(process.MainWindowHandle);

                            ListBoxProcessItem mainItem = GetListBoxItemForWindowOutOfBounds(process.MainWindowHandle);
                            if (mainItem != null)
                                listBoxWindows.Items.Add(mainItem);

                            foreach (IntPtr childWindowRef in childWindows)
                            {
                                ListBoxProcessItem listBoxProcessItem = GetListBoxItemForWindowOutOfBounds(childWindowRef);
                                if (listBoxProcessItem != null)
                                    listBoxWindows.Items.Add(listBoxProcessItem);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private ListBoxProcessItem GetListBoxItemForWindowOutOfBounds(IntPtr winHandlePtr)
        {
            ListBoxProcessItem listBoxProcessItem = null;

            Rect windowPosition = new Rect();
            GetWindowRect(winHandlePtr, ref windowPosition);
            int winHeight = Math.Max(windowPosition.Bottom - windowPosition.Top, 25);
            int winWidth = Math.Max(windowPosition.Left - windowPosition.Right, 25);

            if (windowPosition.Top < 0 || windowPosition.Bottom > Screen.PrimaryScreen.Bounds.Height || (windowPosition.Right < winHeight) ||
                (windowPosition.Left + winWidth) > Screen.PrimaryScreen.Bounds.Width)
            {

                StringBuilder windowTitle =new StringBuilder();
                GetWindowText(winHandlePtr, windowTitle, 1024);
                listBoxProcessItem = new ListBoxProcessItem
                {
                    Name = windowTitle.ToString(),
                    WindowHandle = winHandlePtr,
                    Value = winHandlePtr.ToInt64().ToString(CultureInfo.InvariantCulture)
                };
            }

            return listBoxProcessItem;
        }

        private void btnMoveWindows_Click(object sender, EventArgs e)
        {
            foreach (var listItem in listBoxWindows.SelectedItems)
            {
                ListBoxProcessItem listBoxProcessItem = listItem as ListBoxProcessItem;
                
                if (listBoxProcessItem != null)
                {
                    IntPtr mainWindowHandle = listBoxProcessItem.WindowHandle;

                    Rect windowPosition = new Rect();
                    GetWindowRect(mainWindowHandle, ref windowPosition);
                    int winHeight = Math.Max(Math.Abs(windowPosition.Bottom - windowPosition.Top), 25);
                    int winWidth = Math.Max(Math.Abs(windowPosition.Left - windowPosition.Right), 25);

                    winWidth = Math.Min(winWidth, Screen.PrimaryScreen.Bounds.Width / 2);
                    winHeight = Math.Min(winHeight, Screen.PrimaryScreen.Bounds.Height / 2);

                    MoveWindow(mainWindowHandle, 25, 25, winWidth, winHeight, true);
                }
            }

            UpdateProcessList();
        }


        public class ListBoxProcessItem
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public IntPtr WindowHandle { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }
    }
}
