using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun
{
    public static partial class Program
    {
        public static class CommandLineEntries
        {
            internal static string ParseOneFile(string[] argf)
            {
                if (argf == null)
                    return null;
                if (argf.Length == 1)
                {
                    try
                    {
                        if (System.IO.File.Exists(System.IO.Path.IsPathRooted(argf[0]) ? argf[0] : System.IO.Path.GetFullPath(argf[0])))
                        {
                            return argf[0];
                        }
                        else
                        {
                            throw new System.IO.FileNotFoundException("文件不存在。", argf[0]);
                        }
                    }
                    catch (Exception e)
                    {
                        ShowError(String.Format("查询文件 {0} 失败：{1}", argf[0], e.Message));
                        return null;
                    }
                }
                return null;
            }

            [Core.CommandLineEntry(true)]
            public static void Editor(string[] argf)
            {
                string result = ParseOneFile(argf);

                if (result == null)
                {
                    WelcomePage welcome = new WelcomePage();
                    welcome.ShowDialog();
                    if (welcome.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        result = welcome.Result;
                    }
                    else
                    {
                        return;
                    }
                }
                try
                {
                    ProjectManager.OpenProject(result);
                }
                catch (Exception e)
                {
                    Application_ThreadException(null, new System.Threading.ThreadExceptionEventArgs(e));
                    return;
                }
                System.Windows.Forms.Application.Run(Workbench.Instance);
            }

            [Core.CommandLineEntry()]
            public static void ReMarshal(string[] argf)
            {
                string result = ParseOneFile(argf);
                if (result != null)
                {
                    object mar;
                    using (System.IO.FileStream file = new System.IO.FileStream(result, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        mar = NekoKun.FuzzyData.Serialization.RubyMarshal.RubyMarshal.Load(file);
                    }
                    using (System.IO.FileStream file = new System.IO.FileStream(result + ".output", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))
                    {
                        NekoKun.FuzzyData.Serialization.RubyMarshal.RubyMarshal.Dump(file, mar);
                    }
                    Program.ShowError("成功处理文件：" + result);
                }
            }
        }
    }
}
