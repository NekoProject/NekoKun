using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.FuzzyData
{
    public class FuzzyFloat: FuzzyObject
    {
        public double Value;

        public FuzzyFloat(double value)
        {
            Value = value;
            this.ClassName = FuzzySymbol.GetSymbol("Float");
        }

        public FuzzyFloat(float value)
        {
            Value = value;
            this.ClassName = FuzzySymbol.GetSymbol("Float");
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
}
