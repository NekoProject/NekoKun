using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public class DatabaseItemEditorSmartLayout : DatabaseItemEditor
    {
        protected System.Windows.Forms.FlowLayoutPanel panel;

        public DatabaseItemEditorSmartLayout(System.Xml.XmlNode param, DatabaseFile file)
            : base(param, file)
        {
            panel = new System.Windows.Forms.FlowLayoutPanel();
            panel.AutoScroll = true;
            panel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Controls.Add(panel);

            foreach (var item in this.editors)
            {
                System.Windows.Forms.Control q = new UI.LynnLabel();
                q.Text = item.Key.Name;
                q.Width = 200;
                this.panel.Controls.Add(q);
                this.panel.Controls.Add(q = item.Value as System.Windows.Forms.Control);
                q.Width = 200;
            }
        }

    }
}
