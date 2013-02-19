using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.Serialization.RubyMarshal
{ 
    [System.Diagnostics.DebuggerTypeProxy(typeof(RubyObjectDebugView))]
    public class RubyObject
    {
        internal class RubyObjectDebugView
        {
            internal RubyObject obj;

            public RubyObjectDebugView(RubyObject obj)
            {
                this.obj = obj;
            }

            public RubySymbol ClassName
            {
                get { return obj.ClassName; }
            }

            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)]
            public KeyValuePair<RubySymbol, object>[] Keys
            {
                get
                {
                    KeyValuePair<RubySymbol, object>[] keys = new KeyValuePair<RubySymbol, object>[obj.InstanceVariables.Count];

                    int i = 0;
                    foreach (KeyValuePair<RubySymbol, object> key in obj.InstanceVariables)
                    {
                        keys[i] = key;
                        i++;
                    }
                    return keys;
                }
            }
        }
        private RubySymbol className;
        private Dictionary<RubySymbol, object> variables;
        private RubyObjectInstanceVariableProxy variableproxy;
        private List<RubyModule> extmodules;

        public override string ToString()
        {
            return ("#<" + this.ClassName + ">");
        }

        public virtual RubySymbol ClassName
        {
            get { return this.className ?? (this.className = RubySymbol.GetSymbol("Object")); }
            set { this.className = value; }
        }

        public RubyClass Class
        {
            get { return RubyClass.GetClass(this.ClassName); }
        }

        public Dictionary<RubySymbol, object> InstanceVariables
        {
            get { return variables ?? (variables = new Dictionary<RubySymbol, object>()); }
        }

        public RubyObjectInstanceVariableProxy InstanceVariable
        {
            get { return variableproxy ?? (variableproxy = new RubyObjectInstanceVariableProxy(this)); }
        }

        public List<RubyModule> ExtendModules
        {
            get { return extmodules ?? (extmodules = new List<RubyModule>()); }
        }

        public virtual Encoding Encoding
        {
            get;
            set;
        }

        public class RubyObjectInstanceVariableProxy
        {
            RubyObject obj;
            internal RubyObjectInstanceVariableProxy(RubyObject obj)
            {
                this.obj = obj;
            }

            public object this[RubySymbol key]
            {
                get
                {
                    return obj.InstanceVariables.ContainsKey(key) ?
                        obj.InstanceVariables[key] is RubyNil ?
                            null :
                            obj.InstanceVariables[key] :
                        null;
                }
                set
                {
                    if (obj.InstanceVariables.ContainsKey(key))
                        obj.InstanceVariables[key] = value;
                    else
                        obj.InstanceVariables.Add(key, value);
                }
            }

            public object this[string key]
            {
                get { return this[RubySymbol.GetSymbol(key)]; }
                set { this[RubySymbol.GetSymbol(key)] = value; }
            }

            public object this[RubyString key]
            {
                get { return this[RubySymbol.GetSymbol(key)]; }
                set { this[RubySymbol.GetSymbol(key)] = value; }
            }
        }
    }
}
