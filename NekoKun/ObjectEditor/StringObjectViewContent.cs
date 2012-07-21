using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace orzTech.NekoKun.ObjectEditor
{
    public class StringObjectViewContent : AbstractObjectViewContent
    {
        private TextBox edit;
        public StringObjectViewContent(object Object)
        {
            this.mObject = Object;
            this.edit = new TextBox();
            this.edit.Text = this.mObject as string;
            this.edit.TextChanged += new EventHandler(edit_TextChanged);
            this.edit.Multiline = true;
        }

        public void edit_TextChanged(object sender, EventArgs e)
        {
            this.IsDirty = true;
        }

        public override int DefaultHeight
        {
            get
            {
                return ((int) edit.Font.GetHeight() * 2) + SystemInformation.BorderSize.Height * 2;
            }
        }

        public override System.Windows.Forms.Control Control
        {
            get
            {
                return edit;
            }
        }


    }
}
