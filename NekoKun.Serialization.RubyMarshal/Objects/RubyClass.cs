using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.Serialization.RubyMarshal
{
    public class RubyClass: RubyObject
    {
        private string name;
        private RubySymbol symbol;
        private static Dictionary<string, RubyClass> classes = new Dictionary<string, RubyClass>();

        protected RubyClass(string s)
        {
            this.name = s;
            this.symbol = RubySymbol.GetSymbol(s);
            this.ClassName = RubySymbol.GetSymbol("Class");
            classes.Add(s, this);
        }

        public static Dictionary<string, RubyClass> GetClasses()
        {
            return classes;
        }

        public static RubyClass GetClass(RubySymbol s)
        {
            return GetClass(s.GetString());
        }

        public static RubyClass GetClass(string s)
        {
            if (classes.ContainsKey(s)) return classes[s];
            return new RubyClass(s);
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
