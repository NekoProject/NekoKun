using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.ObjectEditor
{
    public class UnknownEditor : AbstractObjectEditor
    {
        System.Windows.Forms.Label control;

        public UnknownEditor(Dictionary<string, object> Params)
            : base(Params)
        {
            control = new System.Windows.Forms.Label();
        }

        protected override void InitControl()
        {
            control.Text = selectedItem.ToString();
        }

        public override System.Windows.Forms.Control Control
        {
            get { return control; }
        }
    }
}
