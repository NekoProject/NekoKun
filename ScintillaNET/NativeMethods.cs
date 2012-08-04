#region Using Directives

using System;
using System.Runtime.InteropServices;
using System.Text;

#endregion Using Directives


namespace ScintillaNET
{
    internal static partial class NativeMethods
    {
        #region Constants

        private const string
            DLL_NAME_KERNEL32 = "kernel32.dll",
            DLL_NAME_USER32 = "user32.dll";

        public const int
            WM_COMMAND = 0x0111;

        public const int ERROR_MOD_NOT_FOUND = 126;
        public const int WM_DESTROY = 0x02;
        public const int WM_DROPFILES = 0x0233;
        public const int WM_GETTEXT = 0x000D;
        public const int WM_GETTEXTLENGTH = 0x000E;
        public const int WM_HSCROLL = 0x114;
        public const int WM_NOTIFY = 0x004e;
        public const int WM_PAINT = 0x000F;
        public const int WM_REFLECT = WM_USER + 0x1C00;
        public const int WM_SETCURSOR = 0x0020;
        public const int WM_USER = 0x0400;
        public const int WM_VSCROLL = 0x115;

        public const int WS_BORDER = 0x00800000;
        public const int WS_EX_CLIENTEDGE = 0x00000200;

        #endregion Constants


        #region Fields

        internal static readonly IntPtr HWND_MESSAGE = new IntPtr(-3);

        #endregion Fields


        #region Functions

        [DllImport("shell32.dll")]
        public static extern void DragAcceptFiles(
            IntPtr hwnd,
            bool accept);

        [DllImport("shell32.dll")]
        public static extern int DragFinish(
            IntPtr hDrop);

        [DllImport("shell32.dll", CharSet=CharSet.Auto)]
        public static extern uint DragQueryFile(
            IntPtr hDrop,
            uint iFile,
            [Out] StringBuilder lpszFile,
            uint cch);

        [DllImport(DLL_NAME_KERNEL32, ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern IntPtr GetProcAddress(
            IntPtr hModule,
            string procName);

        [DllImport(DLL_NAME_USER32)]
        public static extern bool GetUpdateRect(
            IntPtr hWnd,
            out RECT lpRect,
            bool bErase);

        [DllImport(DLL_NAME_KERNEL32, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr LoadLibrary(
            string lpLibFileName);

        [DllImport(DLL_NAME_USER32, CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(
            IntPtr hWnd,
            int Msg,
            IntPtr wParam,
            IntPtr lParam);

        [DllImport(DLL_NAME_USER32)]
        public static extern IntPtr SetParent(
            IntPtr hWndChild,
            IntPtr hWndNewParent);

        #endregion Functions
    }
}
