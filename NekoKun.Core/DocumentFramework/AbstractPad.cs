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

    [global::System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class PadDefaultLocationAttribute : Attribute
    {
        readonly WeifenLuo.WinFormsUI.Docking.DockState dockState;

        public PadDefaultLocationAttribute(WeifenLuo.WinFormsUI.Docking.DockState dockState)
        {
            this.dockState = dockState;
        }

        public WeifenLuo.WinFormsUI.Docking.DockState DockState {
            get { return this.dockState; }
        }
    }
}
