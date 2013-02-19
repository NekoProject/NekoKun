using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.Serialization.RubyMarshal
{
    public class RubyNil : RubyObject
    {
        private static RubyNil instance;

        private RubyNil() { this.ClassName = RubySymbol.GetSymbol("NilClass"); }

        public override string ToString()
        {
            return "Ruby::Nil";
        }

        public static RubyNil Instance {
            get {
                if (instance == null)
                    instance = new RubyNil();
                return instance; 
            }
        }
    }
}
