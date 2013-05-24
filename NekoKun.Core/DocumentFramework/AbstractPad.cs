using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun
{
    public abstract class AbstractPad : UI.LynnDockContent
    {
        public AbstractPad()
        {
            this.DockAreas = 
                WeifenLuo.WinFormsUI.Docking.DockAreas.Document | 
                WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom | 
                WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft | 
                WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight | 
                WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop | 
                WeifenLuo.WinFormsUI.Docking.DockAreas.Float;
        }
    }
}
