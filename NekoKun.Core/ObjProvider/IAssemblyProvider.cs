using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun
{
    public interface IAssemblyProvider
    {
        System.Reflection.Assembly GetAssembly();
    }
}
