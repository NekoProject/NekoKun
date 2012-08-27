using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun
{
    public static partial class Program
    {
        public static class CommandLineEntries
        {
            [Core.CommandLineEntry(true)]
            public static void Editor(string[] argf)
            {
            }

            [Core.CommandLineEntry()]
            public static void ReMarshal(string[] argf)
            {
            }
        }
    }
}
