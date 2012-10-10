using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.FuzzyData
{
    public class FuzzyObject
    {
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
            get { return this.className; }
            set { this.className = value; }
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
