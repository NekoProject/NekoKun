using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.Core
{
    public static class ExceptionHelper
    {
        public static void UnhandledError(Object e)
        {
            string str = ExceptionMessage(e);

            if (Core.Application.UseStandardError)
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
                new Core.UnhandledExceptionDialog(Core.ExceptionHelper.ForceException(e)).ShowDialog();
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
            if (e is Exception) return (Exception)e;
            Exception myE = new Exception(e.ToString());
            myE.Data.Add("exception object", e);
            return myE;
        }
    }
}
