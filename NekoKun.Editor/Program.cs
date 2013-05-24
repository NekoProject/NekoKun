using System;
using System.Collections.Generic;

using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Text;

namespace NekoKun
{
    public static partial class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        ///         
        [STAThread]
        public static void Main(string[] args)
        {
            /*
            using (var f = new System.IO.FileStream(@"c:\\a.txt", FileMode.Create))
            {
                var sym = RubySymbol.GetSymbol("seiran7");
                FuzzyData.Serialization.RubyMarshal.RubyMarshal.Dump(f, sym);
            }
            return;
            */
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            System.AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            if (UI.UIManager.Enabled)
                ToolStripManager.Renderer = new Office2007Renderer();
            else
                ToolStripManager.RenderMode = ToolStripManagerRenderMode.System;

            Core.CommandLineParser parser = new NekoKun.Core.CommandLineParser(typeof(Program.CommandLineEntries));
            if (args.Length == 0)
            {
                CommandLineEntries.Editor(null);
                //CommandLineEntries.ReMarshal(new string[] { @"c:\users\Yichen\abc" });
            }
            else
                parser.ParseAndExecute(args);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Core.ExceptionHelper.UnhandledError(e.ExceptionObject);
#if !DEBUG
            if (e.IsTerminating)
                System.Diagnostics.Process.GetCurrentProcess().Kill();
#endif
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Core.ExceptionHelper.UnhandledError(e.Exception);
        }

        public static string BuildHexDump(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            int lines = (int) Math.Ceiling(bytes.Length / 16.0f);
            sb.AppendLine("#ADDRESS: 00 01 02 03 04 05 06 07   08 09 0A 0B 0C 0D 0E 0F : 0123456789ABCDEF");
            sb.AppendLine("==============================================================================");
            for (int i = 0; i < lines; i++)
            {
                sb.Append(string.Format("{0:X8}: ", i * 16));
                for (int j = 0; j < 8; j++)
                {
                    int l = i * 16 + j;
                    if (bytes.Length > l)
                        sb.Append(string.Format("{0:X2} ", bytes[l]));
                    else
                        sb.Append("   ");
                }
                sb.Append("  ");
                for (int j = 8; j < 16; j++)
                {
                    int l = i * 16 + j;
                    if (bytes.Length > l)
                        sb.Append(string.Format("{0:X2} ", bytes[l]));
                    else
                        sb.Append("   ");
                }

                sb.Append(": ");
                for (int j = 0; j < 16; j++)
                {
                    int l = i * 16 + j;
                    if (bytes.Length > l)
                        sb.Append(GetPrintableChar(bytes[l]));
                    else
                        sb.Append(" ");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private static string GetPrintableChar(byte p)
        {
            if (p >= 32 && p <= 126)
                return ((char)p).ToString();
            else
                return ".";
        }
    }
}
