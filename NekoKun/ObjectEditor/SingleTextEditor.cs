using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.ObjectEditor
{
    public class SingleTextEditor: UI.LynnTextbox, IObjectEditor
    {
        protected bool supply;

        public SingleTextEditor(Dictionary<string, object> Params)
        {
            this.TextChanged += new EventHandler(SingleTextEditor_TextChanged);
            if (DirtyChanged != null) DirtyChanged.ToString();
        }

        void SingleTextEditor_TextChanged(object sender, EventArgs e)
        {
            if (!supply) this.Commit();
        }

        public void Commit()
        {
            if (this.RequestCommit != null)
                this.RequestCommit(this, null);
        }

        public object SelectedItem
        {
            get
            {
                return this.Text;
            }
            set
            {
                supply = true;
                this.Text = value as string;
                supply = false;
            }
        }

        public event EventHandler RequestCommit;

        #region IObjectEditor 成员


        public event EventHandler DirtyChanged;

        #endregion
    }
}
