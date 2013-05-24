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
        public static bool UseStandardError;
        
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
            UnhandledError(e.ExceptionObject);
            //if (e.IsTerminating)
            //    System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            UnhandledError(e.Exception);
        }

        public static void UnhandledError(Object e)
        {
            string str = ExceptionMessage(e);

            if (UseStandardError)
            {
                System.Console.Error.WriteLine();
                System.Console.Error.WriteLine(new string('=', System.Console.WindowWidth - 1));
                ConsoleColor orig = System.Console.ForegroundColor;
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.Error.WriteLine("Unhandled Exception occured in NekoKun");
                System.Console.ForegroundColor = orig;
                System.Console.Error.WriteLine(new string('=', System.Console.WindowWidth - 1));
                System.Console.Error.WriteLine(str);
                System.Console.Error.WriteLine();
                System.Console.Error.WriteLine(new string('=', System.Console.WindowWidth - 1));
            }
            else
                //MessageBox.Show("休斯顿，我们遇到了一个问题……\n\n" + str, "Unhandled Exception occured in NekoKun",  MessageBoxButtons.OK, MessageBoxIcon.Error);
                new Core.UnhandledExceptionDialog(ForceException(e)).ShowDialog();
        }

        public static void ShowError(string message)
        {
            if (UseStandardError)
            {
                System.Console.Error.WriteLine();
                System.Console.Error.WriteLine(message);
            }
            else
                MessageBox.Show(message, "NekoKun",  MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static string ExceptionMessage(Exception e)
        {
            return (e.InnerException != null ? (ExceptionMessage(e.InnerException) + "\n\n") : "") + String.Format("{0}\n类型: {1}\n来源: {2}\n堆栈: \n{3}", e.Message, e.GetType().FullName, e.Source, e.StackTrace);
        }

        public static string ExceptionMessage(Object e)
        {
            return ExceptionMessage(ForceException(e));
        }

        public static Exception ForceException(object e)
        {
            if (e is Exception) return (Exception) e;
            Exception myE = new Exception(e.ToString());
            myE.Data.Add("exception object", e);
            return myE;
        }

        public static LogFile Logger = new LogFile();

        public static object CreateInstanceFromTypeName(string typeName, params object[] param)
        {
            return System.AppDomain.CurrentDomain.CreateInstance(
                        System.Reflection.Assembly.GetExecutingAssembly().FullName,
                        typeName,
                        false,
                        System.Reflection.BindingFlags.CreateInstance,
                        null,
                        param,
                        null,
                        null,
                        null
                    ).Unwrap();
        }

        public static System.Drawing.Color ParseColor(string name)
        {
            System.Drawing.Color col = System.Drawing.Color.FromName(name);
            if (!col.IsKnownColor)
            {
                System.Text.RegularExpressions.Match match;
                match = System.Text.RegularExpressions.Regex.Match(name, @"#?([0-9a-fA-F]{2})([0-9a-fA-F]{2})([0-9a-fA-F]{2})#?");
                if (match.Success)
                {
                    return System.Drawing.Color.FromArgb(System.Convert.ToInt32(match.Groups[1].Value, 16), System.Convert.ToInt32(match.Groups[2].Value, 16), System.Convert.ToInt32(match.Groups[3].Value, 16));
                }
                match = System.Text.RegularExpressions.Regex.Match(name, @"#?([0-9a-fA-F]{2})([0-9a-fA-F]{2})([0-9a-fA-F]{2})([0-9a-fA-F]{2})#?");
                if (match.Success)
                {
                    return System.Drawing.Color.FromArgb(System.Convert.ToInt32(match.Groups[1].Value, 16), System.Convert.ToInt32(match.Groups[2].Value, 16), System.Convert.ToInt32(match.Groups[3].Value, 16), System.Convert.ToInt32(match.Groups[4].Value, 16));
                }
                return System.Drawing.Color.Empty;
            }
            else
                return System.Drawing.Color.FromName(name);
        }

        public static System.Drawing.Font GetMonospaceFont()
        {
            return UI.UIManager.GetMonospaceFont();
        }

        public static Dictionary<string, object> BuildParameterDictionary(System.Xml.XmlNode field)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            foreach (System.Xml.XmlNode property in field.ChildNodes)
            {
                if (property.HasChildNodes && property.ChildNodes.Count == 1 && (property.FirstChild is System.Xml.XmlText))
                    dict.Add(
                        property.Attributes["Name"].Value,
                        property.FirstChild.Value
                    );
                else
                    dict.Add(
                        property.Attributes["Name"].Value,
                        property.ChildNodes
                    );

            }

            return dict;
        }

        public static System.Drawing.Image DecodeBase64Image(string base64)
        {
            byte[] imageBytes = Convert.FromBase64String(base64);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
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
