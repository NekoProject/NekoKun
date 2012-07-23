using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.ObjectEditor
{
    public class UnknownEditor : System.Windows.Forms.Label, IObjectEditor
    {
        private Object obj;
        public UnknownEditor(Dictionary<string, object> Params)
        {
            
        }

        public void Commit()
        {

        }

        public object SelectedItem
        {
            get
            {
                return this.obj;
            }
            set
            {
                this.obj = value;
                this.Text = value.ToString();
            }
        }

        public event EventHandler RequestCommit;
    }
}
