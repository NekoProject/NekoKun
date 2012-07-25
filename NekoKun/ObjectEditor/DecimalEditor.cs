using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.ObjectEditor
{
    public class DecimalEditor : System.Windows.Forms.NumericUpDown, IObjectEditor
    {
        protected decimal orig;
        protected object ori;
        internal System.Windows.Forms.NativeWindow native;

        public DecimalEditor(Dictionary<string, object> Params)
        {
            native = new UI.NativeBorder(this, 0xf /* WM_PAINT */, true, false);

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
                if (ori == null)
                    return (Int32)this.Value;
                return Convert.ChangeType(this.Value, ori.GetType());
            }
            set
            {
                this.orig = this.Value;
                this.ori = value;
                this.Value = Convert.ToDecimal(value ?? 0);
            }
        }

        public event EventHandler RequestCommit;

        #region IObjectEditor 成员


        public event EventHandler DirtyChanged;

        #endregion
    }
}
