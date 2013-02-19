using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.Serialization.RubyMarshal
{
    public class RubyStruct : RubyObject
    {
        public RubyStruct()
            : base()
        {
            this.ClassName = RubySymbol.GetSymbol("Struct");
        }
    }
}
