using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.Core
{
    public class CommandLineParser
    {
        protected System.Type cmdClass;
        protected Dictionary<string, System.Reflection.MethodInfo> methods;
        protected List<System.Reflection.MethodInfo> defaults;

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
            methods.Add("?", this.GetType().GetMethod("ShowHelpEntry"));
            methods.Add("Help", this.GetType().GetMethod("ShowHelpEntry"));
        }

        public void ShowHelpEntry(string[] argf)
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine(String.Format("{0} [/command] [(/|-)key[=value]] [files]", System.Reflection.Assembly.GetEntryAssembly().CodeBase));
            str.AppendLine();

            string[] keysa = new string[this.methods.Count];
            this.methods.Keys.CopyTo(keysa, 0);
            List<string> keys = new List<string>(keysa);
            keys.Sort(StringComparer.OrdinalIgnoreCase);

            str.AppendLine("Available commands: ");
            foreach (var item in keys)
            {
                str.AppendLine(item);
                str.AppendLine("\t" + this.methods[item].ToString());
                str.AppendLine();
            }
            NekoKun.Core.Application.ShowError(str.ToString());
        }

        public void ParseAndExecute(string[] args)
        {
            List<ParsedArgument> arg;
            try
            {
                arg = Parse(args);
            }
            catch (Exception e)
            {
                NekoKun.Core.Application.ShowError(e.Message);
                return;
            }
            Execute(arg);
        }

        public void Execute(List<ParsedArgument> args)
        {
            if (args == null || args.Count == 0)
            {
                ExecuteInner(this.defaults.ToArray(), new ParsedArgument[] { }, new string[] { });
                return;
            }

            string entry = null;
            if (((args[0].Key!= null && args[0].Key.Length > 0) && (args[0].Value == null || args[0].Value.Length == 0)))
            {
                entry = args[0].Key;
                args.RemoveAt(0);
            }

            List<string> argfb = new List<string>();
            List<ParsedArgument> argvb = new List<ParsedArgument>();
            foreach (var item in args)
            {
                if (item.Key != null && item.Key.Length > 0)
                {
                    argvb.Add(item);
                }
                else
                {
                    argfb.Add(item.Value);
                }
            }
            string[] argf = argfb.ToArray();
            ParsedArgument[] argv = argvb.ToArray();

            if (entry != null)
            {
                List<System.Reflection.MethodInfo> methods = new List<System.Reflection.MethodInfo>();
                foreach (var item in this.methods)
                {
                    if (item.Key.Equals(entry, StringComparison.CurrentCultureIgnoreCase) )
                    {
                        methods.Add(item.Value);
                    }
                }
                if (methods.Count > 0)
                    ExecuteInner(methods.ToArray(), argv, argf);
                else
                    throw new CommandLineException("Cannot found entry for " + entry, this.cmdClass, entry, null);
            }
            else
            {
                ExecuteInner(this.defaults.ToArray(), argv, argf);
            }
        }

        internal void ExecuteInner(System.Reflection.MethodInfo[] methods, ParsedArgument[] param, string[] argf)
        {
            if (param.Length == 0)
            {
                foreach (var item in methods)
                {
                    System.Reflection.ParameterInfo[] info = item.GetParameters();
                    if (info.Length == 1)
                    {
                        if (info[0].ParameterType == typeof(string[]))
                        {
                            if (item.IsStatic)
                                item.Invoke(null, new object[] { argf });
                            else
                                item.Invoke(this, new object[] { argf });
                            return;
                        }
                    }
                }
            }
            throw new NotImplementedException();
        }
        
        public List<ParsedArgument> Parse(string[] args)
        {
            List<ParsedArgument> arg = new List<ParsedArgument>();

            if (args.Length == 0)
                return arg;

            bool parse = true;

            for (int i = 0; i < args.Length; i++)
            {
                if (parse)
                {
                    if (args[i] == "-")
                    {
                        parse = false;
                    }
                    else if (args[i].StartsWith("-") || args[i].StartsWith("/"))
                    {
                        ParsedArgument item = new ParsedArgument();
                        int pos = args[i].IndexOf('=');
                        if (pos >= 0)
                        {
                            item.Key = args[i].Substring(1, pos - 1);
                            item.Value = args[i].Substring(pos + 1);
                        }
                        else
                        {
                            item.Key = args[i].Substring(1);
                        }

                        if (item.Key != null)
                            arg.Add(item);
                        else
                            throw new CommandLineException(
                                String.Format("Invalid argument: {0}", args[i]),
                                this.cmdClass, null, args
                            );
                    }
                    else
                    {
                        parse = false;
                    }
                }
                if (!parse)
                {
                    ParsedArgument item = new ParsedArgument();
                    item.Value = args[i];
                    arg.Add(item);
                }
            }

            return arg;
        }

        public class ParsedArgument
        {
            public string Key;
            public string Value;
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
        public string[] CommandLine;

        public CommandLineException(string Message, System.Type CommandClass, string Entry, string[] CommandLine)
            : base(Message)
        { 
            this.CommandClass = CommandClass;
            this.Entry = Entry;
            this.CommandLine = CommandLine;
        }

        public CommandLineException(string Message, System.Type CommandClass, string Entry, string[] CommandLine, Exception InnerException)
            : base(Message, InnerException)
        {
            this.CommandClass = CommandClass;
            this.Entry = Entry;
            this.CommandLine = CommandLine;
        }
    }
}
