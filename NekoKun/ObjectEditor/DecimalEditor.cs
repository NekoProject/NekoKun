using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.ObjectEditor
{
    public class DecimalEditor : System.Windows.Forms.NumericUpDown, IObjectEditor
    {
        protected decimal orig;
        protected object ori;

        public DecimalEditor(Dictionary<string, object> Params)
        {
            this.Maximum = int.MaxValue;
            this.Minimum = int.MinValue;
            this.ValueChanged += new EventHandler(DecimalEditor_ValueChanged);
        }

        void DecimalEditor_ValueChanged(object sender, EventArgs e)
        {
            this.Commit();
        }

        public void Commit()
        {
            if (this.RequestCommit != null && orig != this.Value)
                this.RequestCommit(this, null);
        }

        public object SelectedItem
        {
            get
            {
                return ori.GetType().InvokeMember("Parse", System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.InvokeMethod, null, null, new object[] { this.Value.ToString() });
            }
            set
            {
                this.orig = this.Value;
                this.ori = value;
                this.Value = Convert.ToDecimal(value ?? 0);
            }
        }

        public event EventHandler RequestCommit;
    }
}
