using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using System.Runtime.InteropServices;

namespace orzTech.NekoKun.ObjectEditor
{
    public abstract class AbstractObjectViewContent : IObjectViewContent
    {
        private bool isDirty = false;
        protected object mObject;

        public virtual object Object
        {
            get { return mObject; }
        }

        public virtual string Title
        {
            get { return Object.ToString(); }
        }

        public abstract Control Control
        {
            get;
        }

        public virtual bool IsDirty
        {
            get
            {
                return isDirty;
            }
            set
            {
                isDirty = value;
            }
        }

        public event EventHandler DirtyChanged;

        protected virtual void OnDirtyChanged(EventArgs e)
        {
            if (DirtyChanged != null)
            {
                DirtyChanged(this, e);
            }
        }

        public virtual void Dispose()
        {
            Control.Dispose();
        }

        public virtual int DefaultHeight
        {
            get { return Control.Height; }
        }
    }
}
