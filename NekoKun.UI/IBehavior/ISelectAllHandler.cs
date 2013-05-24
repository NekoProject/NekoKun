using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun
{
    public interface ISelectAllHandler
    {
        bool CanSelectAll { get; }
        void SelectAll();
    }
}
