using System;
using System.Collections.Generic;
using System.Text;

namespace orzTech.NekoKun.ProjectEngines.RGSS
{
    public class RubyModule
    {
        private string name;
        private RubySymbol symbol;
        private static Dictionary<string, RubyModule> modules = new Dictionary<string, RubyModule>();

        protected RubyModule(string s)
        {
            this.name = s;
            this.symbol = RubySymbol.GetSymbol(s);
            modules.Add(s, this);
        }

        public static Dictionary<string, RubyModule> GetModules()
        {
            return modules;
        }

        public static RubyModule GetModule(RubySymbol s)
        {
            return GetModule(s.GetString());
        }

        public static RubyModule GetModule(string s)
        {
            if (modules.ContainsKey(s)) return modules[s];
            return new RubyModule(s);
        }

        public override string ToString()
        {
            return (this.name);
        }

        public string Name
        {
            get { return this.name; }
        }

        public RubySymbol Symbol
        {
            get { return this.symbol; }
        }
    }
}
