using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.ObjectEditor
{
    public class RawLayout : System.Windows.Forms.FlowLayoutPanel, IStructEditorLayout
    {
        public RawLayout(StructField[] fields, CreateControlDelegate createControlDelegate)
        {
            this.AutoScroll = true;
            this.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.Dock = System.Windows.Forms.DockStyle.Fill;

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            foreach (StructField item in fields)
            {
                System.Windows.Forms.Label lbl = new UI.LynnLabel();
                lbl.Text = string.Format("{0} {1}", item.ID, item.Name);
                lbl.Width = 200;
                this.Controls.Add(lbl);

                System.Xml.XmlElement node = doc.CreateElement("Control");
                node.SetAttribute("ID", item.ID);
                node.SetAttribute("Editor", EditorFactory.CreateFromDefaultValue(item.DefaultValue));
                
                System.Windows.Forms.Control con = createControlDelegate(node);
                con.Width = 200;
                this.Controls.Add(con);
            }
        }

    }
}
