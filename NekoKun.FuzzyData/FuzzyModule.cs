using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.FuzzyData
{
    public class FuzzyModule: FuzzyObject
    {
        private string name;
        private FuzzySymbol symbol;
        private static Dictionary<string, FuzzyModule> modules = new Dictionary<string, FuzzyModule>();

        protected FuzzyModule(string s)
        {
            this.name = s;
            this.symbol = FuzzySymbol.GetSymbol(s);
            this.ClassName = FuzzySymbol.GetSymbol("Module");
            modules.Add(s, this);
        }

        public static Dictionary<string, FuzzyModule> GetModules()
        {
            return modules;
        }

        public static FuzzyModule GetModule(FuzzySymbol s)
        {
            return GetModule(s.GetString());
        }

        public static FuzzyModule GetModule(string s)
        {
            if (modules.ContainsKey(s)) return modules[s];
            return new FuzzyModule(s);
        }

        public override string ToString()
        {
            return (this.name);
        }

        public string Name
        {
            get { return this.name; }
        }

        public FuzzySymbol Symbol
        {
            get { return this.symbol; }
        }
    }
}
