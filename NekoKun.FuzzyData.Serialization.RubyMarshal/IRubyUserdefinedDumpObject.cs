using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.FuzzyData.Serialization.RubyMarshal
{
    public interface IRubyUserdefinedDumpObject
    {
        byte[] Dump();
        string ClassName
        {
            get;
        }
    }
}
