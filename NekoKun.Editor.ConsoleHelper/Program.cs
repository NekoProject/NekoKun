using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun
{
    class ConsoleProgram
    {
        [STAThread]
        static void Main(string[] args)
        {
            NekoKun.Program.UseStandardError = true;
            NekoKun.Program.Main(args);
        }
    }
}
