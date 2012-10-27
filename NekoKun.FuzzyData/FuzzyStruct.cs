using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.FuzzyData
{
    public class FuzzyStruct : FuzzyObject
    {
        public FuzzyStruct()
            : base()
        {
            this.ClassName = FuzzySymbol.GetSymbol("Struct");
        }
    }
}
