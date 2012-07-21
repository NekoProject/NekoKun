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
                MessageBox.Show("找不到工程目录。请将本程序放置于 Project7 或其任意层子目录内。", "NekoKun", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Logger.Log("工程路径: {0}", ProjectPath);

            ToolStripManager.Renderer = new Office2007Renderer();

            Application.Run(Workbench.Instance);
        }

        public static LogFile Logger = new LogFile();
    }
}
