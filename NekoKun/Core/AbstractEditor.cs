using System;
using System.Collections.Generic;

using System.Text;
using WeifenLuo.WinFormsUI;

namespace NekoKun
{
    public abstract class AbstractEditor : WeifenLuo.WinFormsUI.DockContent
    {
        public AbstractFile File;
        public AbstractEditor(AbstractFile item)
        {
            this.File = item;
            this.Text = item.ToString();
            this.DockableAreas = DockAreas.Document;
        }

        public virtual void Commit()
        {
            
        }
    }
}
