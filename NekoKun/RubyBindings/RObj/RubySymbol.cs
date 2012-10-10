using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RubyBindings
{
    public class RubySymbol
    {
        private string name;
        private static Dictionary<string, RubySymbol> symbols = new Dictionary<string, RubySymbol>();
        internal RubyString rubyString;

        protected RubySymbol(string s)
        {
            this.name = s;
            symbols.Add(s, this);
        }

        public static Dictionary<string, RubySymbol> GetSymbols()
        {
            return symbols;
        }

        public static RubySymbol GetSymbol(string s)
        {
            if (symbols.ContainsKey(s)) return symbols[s];
            return new RubySymbol(s);
        }

        public static RubySymbol GetSymbol(RubyString str)
        {
            string s = str.Text;
            if (symbols.ContainsKey(s)) return symbols[s];
            RubySymbol sym = new RubySymbol(s);
            sym.rubyString = str;
            return sym;
        }

        public RubyString GetRubyString()
        {
            return this.rubyString;
        }

        public string GetString()
        {
            return this.name;
        }

        public override string ToString()
        {
            return (":" + this.name);
        }

        public string Name
        {
            get { return this.name; }
        }

        public RubyClass GetClass()
        {
            return RubyClass.GetClass(this);
        }

        public RubyModule GetModule()
        {
            return RubyModule.GetModule(this);
        }
    }
}
