using System;
using System.Collections.Generic;
using System.Text;
using NekoKun.Serialization.RubyMarshal;

namespace NekoKun.ObjectEditor
{
    public class SingleTextEditor : AbstractObjectEditor
    {
        System.Windows.Forms.TextBox control;

        public SingleTextEditor(Dictionary<string, object> Params)
            : base(Params)
        {
            control = new NekoKun.UI.LynnTextbox();
            control.ModifiedChanged += new EventHandler(control_ModifiedChanged);
        }

        void control_ModifiedChanged(object sender, EventArgs e)
        {
            MakeDirty();
        }

        public override void Commit()
        {
            if (selectedItem is RubyString)
                this.selectedItem = new RubyString(control.Text);
            else if (selectedItem is string)
                this.selectedItem = control.Text;
        }

        protected override void InitControl()
        {
            if (selectedItem is RubyString)
                control.Text = (selectedItem as RubyString).Text;
            else if (selectedItem is string)
                control.Text = selectedItem as string;
        }

        public override System.Windows.Forms.Control Control
        {
            get { return control; }
        }
    }
}
