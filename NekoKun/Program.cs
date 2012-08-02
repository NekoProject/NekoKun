using System;
using System.Collections.Generic;

using System.Windows.Forms;

namespace NekoKun
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        /// 
        public static string ProjectPath;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            System.AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            /*
            try
            {
                ProjectPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
                while (true)
                {
                    string path = (System.IO.Path.Combine(System.IO.Path.Combine(ProjectPath, @"Game"), "Game.exe"));
                    if (System.IO.File.Exists(path))
                    {
                        ProjectPath = System.IO.Path.GetDirectoryName(System.IO.Path.GetFullPath(path));
                        break;
                    }
                    ProjectPath = System.IO.Directory.GetParent(ProjectPath).FullName;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("可以给我一个工程目录吃吗？", "NekoKun", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                ProjectManager.OpenProject(
                    System.IO.Path.Combine(
                        ProjectPath,
                        "Game.nkproj"
                    )
                );
            }
            catch (Exception e)
            {
                Application_ThreadException(null, new System.Threading.ThreadExceptionEventArgs(e));
            }
            */

            Logger.Log("工程路径: {0}", ProjectPath);

            ToolStripManager.Renderer = new Office2007Renderer();

            WelcomePage welcome = new WelcomePage();
            welcome.ShowDialog();
            if (welcome.DialogResult == DialogResult.OK)
            {
                string result = welcome.Result;
                try
                {
                    ProjectManager.OpenProject(result);
                }
                catch (Exception e)
                {
                    Application_ThreadException(null, new System.Threading.ThreadExceptionEventArgs(e));
                    return;
                }
                Application.Run(Workbench.Instance);
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Application_ThreadException(sender, new System.Threading.ThreadExceptionEventArgs(e.ExceptionObject as Exception));
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MessageBox.Show("休斯顿，我们遇到了一个问题……\n\n" + ExceptionMessage(e.Exception));
        }

        public static string ExceptionMessage(Exception e)
        {
            return (e.InnerException != null ? (ExceptionMessage(e.InnerException) + "\n\n") : "") + String.Format("{0}\n类型: {1}\n来源: {2}\n堆栈: \n {3}", e.Message, e.GetType().FullName, e.Source, e.StackTrace);
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

        private static System.Drawing.Font monospaceFont;

        public static System.Drawing.Font GetMonospaceFont()
        {
            if (monospaceFont != null)
                return monospaceFont;

            var fallback = new string[] { "Simsun_Yahei", "Consolas", "宋体", "SimSun", "Courier New", "Courier" };

            foreach (string name in fallback)
            {
                try
                {
                    monospaceFont = new System.Drawing.Font(new System.Drawing.FontFamily(name), 12);
                    return monospaceFont;
                }
                catch { }
            }

            monospaceFont = new System.Drawing.Font(System.Drawing.FontFamily.GenericMonospace, 12);
            return monospaceFont;
        }
    }
}
