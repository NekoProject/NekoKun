using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace NekoKun.UI
{
    internal static class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
    }
}
