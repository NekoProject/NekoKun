using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun
{
    public interface IDeleteHandler
    {
        bool CanDelete { get; }
        void Delete();
    }
}
