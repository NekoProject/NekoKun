using System;
using System.Collections.Generic;
using System.Text;
using NekoKun.Serialization.RubyMarshal;

namespace NekoKun.ObjectEditor
{
    public class MultilineTextEditor : AbstractObjectEditor
    {
        System.Windows.Forms.TextBox control;

        public MultilineTextEditor(Dictionary<string, object> Params)
            : base(Params)
        {
            control = new NekoKun.UI.LynnTextbox();
            control.Multiline = true;
            control.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            control.ModifiedChanged += new EventHandler(control_ModifiedChanged);
        }

        void control_ModifiedChanged(object sender, EventArgs e)
        {
            MakeDirty();
        }

        public override void Commit()
        {
            this.selectedItem = new RubyString(control.Text);
        }

        protected override void InitControl()
        {
            control.Text = (selectedItem as RubyString).Text;
        }

        public override System.Windows.Forms.Control Control
        {
            get { return control; }
        }
    }
}
