using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun
{
    public interface IObjectEditor
    {
        void Commit();
        object SelectedItem
        {
            get;
            set;
        }

        event EventHandler RequestCommit;
    }
}
