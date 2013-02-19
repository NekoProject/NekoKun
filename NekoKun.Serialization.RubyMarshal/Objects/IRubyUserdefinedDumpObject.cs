using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.Serialization.RubyMarshal
{
    public interface IRubyUserdefinedDumpObject
    {
        byte[] Dump();
    }
}
