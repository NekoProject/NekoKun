using System;
using System.Collections.Generic;
using System.Text;
using orzTech.NekoKun.Base;
using System.Windows.Forms;

namespace orzTech.NekoKun.ObjectEditor
{
    public interface IObjectViewContent : ICanBeDirty, IDisposable
    {
        Object Object
        {
            get;
        }

        string Title
        {
            get;
        }

        Control Control
        {
            get;
        }

        int DefaultHeight
        {
            get;
        }
    }
}
