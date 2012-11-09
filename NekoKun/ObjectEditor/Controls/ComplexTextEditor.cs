using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.ObjectEditor
{
    public class ComplexTextEditor : AbstractObjectEditor
    {
        UI.Scintilla control;

        public ComplexTextEditor(Dictionary<string, object> Params)
            : base(Params)
        {
            control = new UI.Scintilla();
            control.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            control.ModifiedChanged += new EventHandler(control_ModifiedChanged);
        }

        void control_ModifiedChanged(object sender, EventArgs e)
        {
            MakeDirty();
        }

        public override void Commit()
        {
            this.selectedItem = new FuzzyData.FuzzyString(control.Text);
        }

        protected override void InitControl()
        {
            control.Text = (selectedItem as FuzzyData.FuzzyString).Text;
        }

        public override System.Windows.Forms.Control Control
        {
            get { return control; }
        }
    }
}
