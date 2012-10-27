using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.FuzzyData
{ 
    [System.Diagnostics.DebuggerTypeProxy(typeof(FuzzyObjectDebugView))]
    public class FuzzyObject
    {
        internal class FuzzyObjectDebugView
        {
            internal FuzzyObject obj;

            public FuzzyObjectDebugView(FuzzyObject obj)
            {
                this.obj = obj;
            }

            public FuzzySymbol ClassName
            {
                get { return obj.ClassName; }
            }

            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)]
            public KeyValuePair<FuzzySymbol, object>[] Keys
            {
                get
                {
                    KeyValuePair<FuzzySymbol, object>[] keys = new KeyValuePair<FuzzySymbol, object>[obj.InstanceVariables.Count];

                    int i = 0;
                    foreach (KeyValuePair<FuzzySymbol, object> key in obj.InstanceVariables)
                    {
                        keys[i] = key;
                        i++;
                    }
                    return keys;
                }
            }
        }
        private FuzzySymbol className;
        private Dictionary<FuzzySymbol, object> variables;
        private FuzzyObjectInstanceVariableProxy variableproxy;
        private List<FuzzyModule> extmodules;

        public override string ToString()
        {
            return ("#<" + this.ClassName + ">");
        }

        public virtual FuzzySymbol ClassName
        {
            get { return this.className ?? (this.className = FuzzySymbol.GetSymbol("Object")); }
            set { this.className = value; }
        }

        public FuzzyClass Class
        {
            get { return FuzzyClass.GetClass(this.ClassName); }
        }

        public Dictionary<FuzzySymbol, object> InstanceVariables
        {
            get { return variables ?? (variables = new Dictionary<FuzzySymbol, object>()); }
        }

        public FuzzyObjectInstanceVariableProxy InstanceVariable
        {
            get { return variableproxy ?? (variableproxy = new FuzzyObjectInstanceVariableProxy(this)); }
        }

        public List<FuzzyModule> ExtendModules
        {
            get { return extmodules ?? (extmodules = new List<FuzzyModule>()); }
        }

        public virtual Encoding Encoding
        {
            get;
            set;
        }

        public class FuzzyObjectInstanceVariableProxy
        {
            FuzzyObject obj;
            internal FuzzyObjectInstanceVariableProxy(FuzzyObject obj)
            {
                this.obj = obj;
            }

            public object this[FuzzySymbol key]
            {
                get
                {
                    return obj.InstanceVariables.ContainsKey(key) ?
                        obj.InstanceVariables[key] is FuzzyNil ?
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
                get { return this[FuzzySymbol.GetSymbol(key)]; }
                set { this[FuzzySymbol.GetSymbol(key)] = value; }
            }

            public object this[FuzzyString key]
            {
                get { return this[FuzzySymbol.GetSymbol(key)]; }
                set { this[FuzzySymbol.GetSymbol(key)] = value; }
            }
        }
    }
}
