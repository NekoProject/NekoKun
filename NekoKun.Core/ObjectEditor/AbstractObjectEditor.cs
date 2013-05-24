using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun
{
    public abstract class AbstractObjectEditor
    {
        protected Object selectedItem;

        public AbstractObjectEditor(Dictionary<string, object> Params)
        {
        }

        public object SelectedItem
        {
            get { 
                Commit();
                return selectedItem; 
            }
            set { 
                selectedItem = value;
                InitControl();
            }
        }

        protected void MakeDirty()
        {
            if (DirtyChanged != null)
                DirtyChanged(this, EventArgs.Empty);
        }

        public virtual void Commit() { }

        protected abstract void InitControl();

        public abstract System.Windows.Forms.Control Control
        {
            get;
        }

        public event EventHandler DirtyChanged;
    }
}
