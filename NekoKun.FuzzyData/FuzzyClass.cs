using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.FuzzyData
{
    public class FuzzyClass: FuzzyObject
    {
        private string name;
        private FuzzySymbol symbol;
        private static Dictionary<string, FuzzyClass> classes = new Dictionary<string, FuzzyClass>();

        protected FuzzyClass(string s)
        {
            this.name = s;
            this.symbol = FuzzySymbol.GetSymbol(s);
            this.ClassName = FuzzySymbol.GetSymbol("Class");
            classes.Add(s, this);
        }

        public static Dictionary<string, FuzzyClass> GetClasses()
        {
            return classes;
        }

        public static FuzzyClass GetClass(FuzzySymbol s)
        {
            return GetClass(s.GetString());
        }

        public static FuzzyClass GetClass(string s)
        {
            if (classes.ContainsKey(s)) return classes[s];
            return new FuzzyClass(s);
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
