using System;
using System.Collections.Generic;
using System.Text;

namespace orzTech.NekoKun.ProjectEngines.RGSS
{
    [Serializable, System.Diagnostics.DebuggerDisplay("{GetDebugView()}")]
    public class RubyHash : Dictionary<object, object>
    {
        private object defaultValue;

        public RubyHash()
            : this(null)
        { }

        public RubyHash(object DefaultValue)
            : base()
        {
            this.defaultValue = DefaultValue;
        }

        public object DefaultValue
        {
            get { return this.defaultValue; }
            set {
                this.defaultValue = value;
            }
        }

        public new object this[object key]
        {
            get {
                if (base.ContainsKey(key))
                {
                    return base[key];
                }
                return defaultValue;
            }
            set {
                base[key] = value;
            }
        }
    }
}
