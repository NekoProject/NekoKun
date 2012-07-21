using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using System.Runtime.InteropServices;

namespace orzTech.NekoKun.ObjectEditor
{
    public class ObjectEditor : Panel
    {
        private object mObject;
        private IObjectViewContent vc;
        public object Object
        {
            get 
            {
                return mObject;
            }
            set 
            {
                if (mObject != null)
                {
                    if (vc != null)
                    {
                        try
                        {
                            vc.Dispose();
                        }
                        catch { }
                    }
                }
                mObject = value;
                if (mObject != null)
                {
                    vc = ObjectDisplayBindingManager.CreateViewContent(mObject);
                    if (vc != null)
                    {
                        vc.Control.Dock = DockStyle.Fill;
                        this.Controls.Add(vc.Control);
                    }
                }
            }
        }

        public ObjectEditor()
        {
            
        }

        public ObjectEditor(object Object)
            : this()
        {
            this.Object = Object;
        }

        public int DefaultHeight
        {
            get { return vc.DefaultHeight; }
        }
    }
}
