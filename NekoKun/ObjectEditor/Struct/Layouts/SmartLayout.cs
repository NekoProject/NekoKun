using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.ObjectEditor
{
    public class SmartLayout : System.Windows.Forms.FlowLayoutPanel, IStructEditorLayout
    {
        public SmartLayout(System.Xml.XmlNode param, CreateControlDelegate createControlDelegate)
        {
            this.AutoScroll = true;
            this.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.Dock = System.Windows.Forms.DockStyle.Fill;
        }

    }
}
