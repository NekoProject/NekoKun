using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.Serialization.RubyMarshal
{
    public class RubyFloat: RubyObject
    {
        public double Value;

        public RubyFloat(double value)
        {
            Value = value;
            this.ClassName = RubySymbol.GetSymbol("Float");
        }

        public RubyFloat(float value)
        {
            Value = value;
            this.ClassName = RubySymbol.GetSymbol("Float");
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
}
