using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.ObjectEditor
{
    public class DecimalEditor : AbstractObjectEditor
    {
        System.Windows.Forms.NumericUpDown control;

        public DecimalEditor(Dictionary<string, object> Params)
            : base(Params)
        {
            control = new NekoKun.UI.LynnNumericUpDown();
            control.ValueChanged += new EventHandler(control_ModifiedChanged);
        }

        void control_ModifiedChanged(object sender, EventArgs e)
        {
            MakeDirty();
        }

        public override void Commit()
        {
            this.selectedItem = Convert.ChangeType(control.Value, selectedItem.GetType());
        }

        protected override void InitControl()
        {
            control.ValueChanged -= new EventHandler(control_ModifiedChanged);
            control.Value = Convert.ToDecimal(selectedItem);
            control.ValueChanged += new EventHandler(control_ModifiedChanged);
        }

        public override System.Windows.Forms.Control Control
        {
            get { return control; }
        }
    }
}
