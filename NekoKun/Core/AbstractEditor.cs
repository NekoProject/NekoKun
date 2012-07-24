using System;
using System.Collections.Generic;

using System.Text;
using WeifenLuo.WinFormsUI.Docking;

namespace NekoKun
{
    public abstract class AbstractEditor : UI.LynnDockContent
    {
        public AbstractFile File;
        public AbstractEditor(AbstractFile item)
        {
            this.File = item;
            this.Text = item.ToString();
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
        }

        public virtual void Commit()
        {
            
        }
    }
}
