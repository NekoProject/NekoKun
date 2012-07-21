using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.Core;

namespace orzTech.NekoKun.ObjectEditor
{
    public static class ObjectDisplayBindingManager
    {
        static ArrayList items;

        public static IObjectViewContent CreateViewContent(object Object)
        {
            
            if (items == null)
            {
                items = AddInTree.BuildItems("/NekoKun/ObjectEditor/DisplayBindings", null, true);
            }
            foreach (ObjectDisplayBindingDescriptor desc in items)
            {
                if (desc.CanHandle(Object.GetType()))
                {
                    IObjectViewContent content = desc.GetDisplayBinding().OpenObject(Object);
                    if (content != null)
                    {
                        return content;
                    }
                }
            }
            return new orzTech.NekoKun.ObjectEditor.StringObjectViewContent(Object.ToString());
            //return new GeneralObjectViewContent(Object);
        }
    }
}
