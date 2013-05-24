using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace NekoKun.Core
{
    public static class Application
    {
        public static bool UseStandardError = false;
        public static LogFile Logger = new LogFile();

        public static void ShowError(string message)
        {
            if (UseStandardError)
            {
                System.Console.Error.WriteLine();
                System.Console.Error.WriteLine(message);
            }
            else
                MessageBox.Show(message, System.Reflection.Assembly.GetCallingAssembly().FullName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
