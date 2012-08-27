using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.Core
{
    public class CommandLineParser
    {
        protected System.Type cmdClass;
        public Dictionary<string, System.Reflection.MethodInfo> methods;
        public List<System.Reflection.MethodInfo> defaults;

        public CommandLineParser(System.Type CommandClass)
        {
            cmdClass = CommandClass;
            defaults = new List<System.Reflection.MethodInfo>();
            methods = new Dictionary<string,System.Reflection.MethodInfo>();
            foreach (var item in cmdClass.GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.FlattenHierarchy))
            {
                object[] attr = item.GetCustomAttributes(typeof(CommandLineEntry), true);
                if (attr.Length == 1 && attr[0] is CommandLineEntry)
                {
                    CommandLineEntry entry = attr[0] as CommandLineEntry;
                    if (entry.IsDefault)
                    {
                        defaults.Add(item);
                    }
                    methods.Add(item.Name, item);
                }
            }
        }

        public void Parse(string[] args)
        {
            
        }
    }
    
    public class CommandLineEntry : System.Attribute {
        bool isDefault;
        public CommandLineEntry()
            : this(false)
        {
        }

        public CommandLineEntry(bool isDefault)
        {
            this.isDefault = isDefault;
        }

        public bool IsDefault
        {
            get { return this.isDefault; }
        }
    }

    public class CommandLineException : System.Exception
    {
        public System.Type CommandClass;
        public string Entry;
        public string CommandLine;

        public CommandLineException(string Message, System.Type CommandClass, string Entry, string CommandLine)
            : base(Message)
        { 
            this.CommandClass = CommandClass;
            this.Entry = Entry;
            this.CommandLine = CommandLine;
        }

        public CommandLineException(string Message, System.Type CommandClass, string Entry, string CommandLine, Exception InnerException)
            : base(Message, InnerException)
        {
            this.CommandClass = CommandClass;
            this.Entry = Entry;
            this.CommandLine = CommandLine;
        }
    }
}
