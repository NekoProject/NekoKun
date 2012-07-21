using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace orzTech.NekoKun.ObjectEditor
{
    public class ListObjectViewContent : AbstractObjectViewContent
    {
        private SplitContainer con;
        private ListObjectViewContentListBox list;
        private ObjectEditor editor;
        public ListObjectViewContent(object Object)
        {
            this.mObject = Object;
            this.list = new ListObjectViewContentListBox(Object);
            this.list.SelectedIndexChanged += new EventHandler(SelectedIndexChanged);
            this.list.Dock = DockStyle.Fill;

            this.editor = new ObjectEditor();
            this.editor.Dock = DockStyle.Fill;

            this.con = new SplitContainer();
            this.con.Orientation = Orientation.Vertical;
            this.con.FixedPanel = FixedPanel.Panel1;
            this.con.Panel1MinSize = 100;
            this.con.Panel1.Controls.Add(this.list);
            this.con.Panel2.Controls.Add(this.editor);

            //if (this.list.Items.Count > 0)
                //this.list.SelectedIndex = 0;
        }

        private void SelectedIndexChanged(object sender, EventArgs e)
        {
            this.editor.Object = this.list.SelectedObject;
        }

        public override System.Windows.Forms.Control Control
        {
            get
            {
                return con;
            }
        }

        public override int DefaultHeight
        {
            get
            {
                return 500;
            }
        }
    }
}
