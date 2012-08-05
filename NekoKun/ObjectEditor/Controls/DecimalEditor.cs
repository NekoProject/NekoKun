using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.ObjectEditor
{
    public class DecimalEditor : UI.LynnNumericUpDown, IObjectEditor
    {
        protected decimal orig;
        protected object ori;

        public DecimalEditor(Dictionary<string, object> Params)
        {
            if (DirtyChanged != null) DirtyChanged.ToString();

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
