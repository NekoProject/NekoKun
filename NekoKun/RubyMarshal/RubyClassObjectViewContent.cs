using System;
using System.Collections.Generic;
using orzTech.NekoKun.ObjectEditor;

namespace orzTech.NekoKun.ProjectEngines.RGSS
{
    public class RubyClassObjectViewContent : AbstractObjectViewContent
    {
        RubyObjectViewContentListBox list;

        public RubyClassObjectViewContent(RubyObject Object)
        {
            this.mObject = Object;
            this.list = new RubyObjectViewContentListBox(Object);
        }

        public override System.Windows.Forms.Control Control
        {
            get { return this.list; }
        }
    }
}
