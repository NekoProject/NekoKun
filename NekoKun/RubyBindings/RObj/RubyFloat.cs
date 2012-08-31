using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RubyBindings
{
    public class RubyFloat
    {
        public double Value;

        public RubyFloat(double value)
        {
            Value = value;
        }

        public RubyFloat(float value)
        {
            Value = value;
        }
    }
}
