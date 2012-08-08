using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RubyBindings
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
