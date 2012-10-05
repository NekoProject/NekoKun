using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RubyBindings
{
    public class RubyObject
    {
        private RubySymbol className;
        private Dictionary<RubySymbol, object> variables = new Dictionary<RubySymbol, object>();

        public override string ToString()
        {
            return ("#<" + this.ClassName + ">");
        }

        public RubySymbol ClassName
        {
            get { return this.className; }
            set { this.className = value; }
        }

        public Dictionary<RubySymbol, object> Variables
        {
            get { return variables; }
        }

        public object this[RubySymbol key]
        {
            get {
                return variables.ContainsKey(key) ?
                    variables[key] is RubyNil ?
                        null :
                        variables[key] :
                    null;
            }
            set {
                if (variables.ContainsKey(key))
                    variables[key] = value;
                else
                    variables.Add(key, value);
            }
        }

        public object this[string key]
        {
            get { return variables[RubySymbol.GetSymbol(key)]; }
            set { variables[RubySymbol.GetSymbol(key)] = value; }
        }
    }
}
