using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.Core
{
    public interface IOutputProvider
    {
        System.Windows.Forms.Control OutputViewContent {
            get;
        }
    }
}
