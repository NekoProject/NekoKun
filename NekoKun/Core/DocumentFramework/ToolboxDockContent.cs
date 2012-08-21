using System;
using System.Collections.Generic;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;

namespace NekoKun
{
    public class ToolboxDockContent : UI.LynnDockContent
    {
        protected System.Windows.Forms.Control control;

        public ToolboxDockContent() : base()
        {
            this.DockAreas = DockAreas.Document | DockAreas.DockBottom | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.Float;
            this.Text = "工具箱";

            SetContent(null);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            SetContent(null);
            base.OnClosing(e);
        }

        public void SetContent(AbstractEditor editor)
        {
            if (editor != null && editor is IToolboxProvider)
            {
                System.Windows.Forms.Control con = (editor as IToolboxProvider).ToolboxControl;
                if (con != control)
                {
                    if (control != null)
                        this.Controls.Remove(control);

                    control = con;
                    control.Dock = System.Windows.Forms.DockStyle.Fill;
                    this.Controls.Add(control);
                    this.SetFont(control);
                }
                if (control != null)
                {
                    control.Tag = editor;
                }
            }
            else if (control != null)
            {
                this.Controls.Remove(control);
                control = null;
            }
        }
    }
}
