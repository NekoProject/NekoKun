using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.ObjectEditor
{
    public class ArrayEditor : System.Windows.Forms.SplitContainer, IObjectEditor
    {
        object orig;
        System.Windows.Forms.ListBox list;
        System.Windows.Forms.Control con;

        public ArrayEditor(IObjectEditor Con)
        {
            con = Con as System.Windows.Forms.Control;
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel1MinSize = 100;
            this.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.SplitterDistance = 150;
            list = new ArrayListbox();
            list.Dock = System.Windows.Forms.DockStyle.Fill;
            list.SelectedIndexChanged += new EventHandler(list_SelectedIndexChanged);
            this.Panel1.Controls.Add(list);
            con.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel2.Controls.Add(con);

            Con.DirtyChanged += new EventHandler(Con_DirtyChanged);
        }

        void Con_DirtyChanged(object sender, EventArgs e)
        {
            if (this.DirtyChanged != null)
                this.DirtyChanged(sender, e);
        }

        void list_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (con != null)
                (con as IObjectEditor).SelectedItem = list.SelectedItem;
        }

        public void Commit()
        {
            (con as IObjectEditor).Commit();
        }

        public object SelectedItem
        {
            get
            {
                object[] ret = new object[this.list.Items.Count];
                this.list.Items.CopyTo(ret, 0);
                if (orig is List<object>)
                    return new List<object>(ret);
                return ret;
            }
            set
            {
                orig = value;
                this.list.Items.Clear();
                if (orig is List<object>)
                    foreach (var item in (orig as System.Collections.IEnumerable))
	                {
                        this.list.Items.Add(item);
	                }
                else
                    this.list.Items.AddRange(orig as object[]);

                this.list.SelectedIndex = 0;
            }
        }

        public event EventHandler DirtyChanged;

        public event EventHandler RequestCommit;

        public class ArrayListbox : UI.LynnListbox
        {
            protected override string GetString(int id)
            {
                return String.Format("{0:D3}: {1}", id + 1, this.Items[id].ToString());
            }
        }
    }
}
