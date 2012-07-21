using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun
{
    public interface IFindAllProvider
    {
        NavPoint[] FindAll(string Keyword);
    }
}
