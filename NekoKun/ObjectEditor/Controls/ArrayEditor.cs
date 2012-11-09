using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace NekoKun.ObjectEditor
{
    public class ArrayEditor : AbstractObjectEditor, IClipboardHandler, IDeleteHandler
    {
        System.Windows.Forms.SplitContainer split = new System.Windows.Forms.SplitContainer();
        System.Windows.Forms.ListBox list;
        AbstractObjectEditor con;
        System.Windows.Forms.NumericUpDown menuResizeTextbox;
        System.Windows.Forms.ToolStripMenuItem menuResize;

        public ArrayEditor(AbstractObjectEditor Con)
            : base(null)
        {
            con = Con;
            split.Dock = System.Windows.Forms.DockStyle.Fill;
            split.Panel1MinSize = 100;
            split.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            split.SplitterDistance = 150;
            list = new ArrayListbox();
            list.Dock = System.Windows.Forms.DockStyle.Fill;
            list.SelectedIndexChanged += new EventHandler(list_SelectedIndexChanged);
            split.Panel1.Controls.Add(list);
            con.Control.Dock = System.Windows.Forms.DockStyle.Fill;
            split.Panel2.Controls.Add(con.Control);

            Con.DirtyChanged += new EventHandler(Con_DirtyChanged);
            menuResizeTextbox = new System.Windows.Forms.NumericUpDown();
            menuResize = new System.Windows.Forms.ToolStripMenuItem();
            menuResize.Text = "更改最大值(&M)";
            menuResizeTextbox.Font = split.Font;
            menuResizeTextbox.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            menuResizeTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            menuResizeTextbox.Cursor = System.Windows.Forms.Cursors.Default;
            var host = new System.Windows.Forms.ToolStripControlHost(menuResizeTextbox);
            menuResize.DropDownItems.Add(host);
            var dd = (menuResize.DropDown as System.Windows.Forms.ToolStripDropDownMenu);
            dd.ShowImageMargin = false;
            dd.BackColor = System.Drawing.SystemColors.Window;

            this.list.ContextMenuStrip = new EditContextMenuStrip(this);
            this.list.ContextMenuStrip.Items.Add(new System.Windows.Forms.ToolStripSeparator());
            this.list.ContextMenuStrip.Items.Add(menuResize);
            this.list.ContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(ContextMenuStrip_Opening);

            menuResizeTextbox.ValueChanged += new EventHandler(menuResizeTextbox_ValueChanged);
        }

        void menuResizeTextbox_ValueChanged(object sender, EventArgs e)
        {
            this.Relength((int)menuResizeTextbox.Value);
        }

        void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            menuResize.Enabled = (this.CreateDefaultObject != null);
            menuResizeTextbox.Value = this.list.Items.Count;
        }

        void Relength(int newsize)
        {
            if (newsize == this.list.Items.Count)
                return;
            else if (newsize > this.list.Items.Count)
            {
                int count = newsize - this.list.Items.Count;
                for (int i = 0; i < count; i++)
                {
                    this.list.Items.Add(this.CreateDefaultObject());
                }
                MakeDirty();
            }
            else if (newsize < this.list.Items.Count)
            {
                int count = this.list.Items.Count;
                for (int i = newsize; i < count; i++)
                {
                    this.list.Items.RemoveAt(newsize);
                }
                MakeDirty();
            }
        }

        //Still loving my tree, but losing you.

        void Con_DirtyChanged(object sender, EventArgs e)
        {
            MakeDirty();
        }

        int lastIndex = -1;

        void list_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lastIndex >= 0)
            {
                int id = lastIndex;
                lastIndex = -1;
                try
                {
                    list.Items[id] = con.SelectedItem;
                }
                catch { }
            }

            lastIndex = list.SelectedIndex;
            if (list.SelectedItem != null)
                con.SelectedItem = list.SelectedItem;
        }

        public override void Commit()
        {
            object[] ret = new object[this.list.Items.Count];
            this.list.Items.CopyTo(ret, 0);
            if (selectedItem is List<object>)
                selectedItem = new List<object>(ret);
            else
                selectedItem = ret;
        }

        public class ArrayListbox : UI.LynnListbox
        {
            protected override string GetString(int id)
            {
                return String.Format("{0:D3}: {1}", id + 1, this.Items[id].ToString());
            }
        }

        public bool CanCut
        {
            get { return this.CanCopy && this.CanDelete; }
        }

        public bool CanCopy
        {
            get { return this.list.SelectedItem != null && this.ClipboardFormat != null; }
        }

        public bool CanPaste
        {
            get {
                if (this.ClipboardFormat == null) return false;
                return System.Windows.Forms.Clipboard.ContainsData(this.ClipboardFormat);
            }
        }

        public void Cut()
        {
            Copy(); Delete();
            MakeDirty();
        }

        public void Copy()
        {
            System.Windows.Forms.DataObject db = new System.Windows.Forms.DataObject();
            db.SetData(this.ClipboardFormat, this.DumpObject(this.list.SelectedItem));
            System.Windows.Forms.Clipboard.SetDataObject(db, true, 5, 100);
        }

        public void Paste()
        {
            object data = System.Windows.Forms.Clipboard.GetData(this.ClipboardFormat);
            byte[] buffer;
            if (data is System.IO.MemoryStream)
            {
                System.IO.MemoryStream ms = data as System.IO.MemoryStream;
                buffer = new byte[ms.Length];
                ms.Read(buffer, 0, (int)ms.Length);
                ms.Dispose();
            }
            else if (data is byte[])
            {
                buffer = data as byte[];
            }
            else { return; }
            this.lastIndex = -1;
            this.list.Items[this.list.SelectedIndex] = this.LoadObject(buffer);
            con.SelectedItem = this.list.Items[this.list.SelectedIndex];
            MakeDirty();
        }

        public bool CanDelete
        {
            get { return this.list.SelectedItem != null && this.CreateDefaultObject != null; }
        }

        public void Delete()
        {
            this.lastIndex = -1;
            this.list.Items[this.list.SelectedIndex] = this.CreateDefaultObject();
            con.SelectedItem = this.list.Items[this.list.SelectedIndex];
            MakeDirty();
        }

        public CreateDefaultObjectDelegate CreateDefaultObject;
        public DumpObjectDelegate DumpObject;
        public LoadObjectDelegate LoadObject;
        public string ClipboardFormat;

        public delegate object CreateDefaultObjectDelegate();
        public delegate byte[] DumpObjectDelegate(object obj);
        public delegate object LoadObjectDelegate(byte[] obj);

        protected override void InitControl()
        {
            this.list.Items.Clear();
            if (selectedItem is List<object>)
                foreach (var item in (selectedItem as System.Collections.IEnumerable))
                {
                    this.list.Items.Add(item);
                }
            else
                this.list.Items.AddRange(selectedItem as object[]);

            lastIndex = -1;
            this.list.SelectedIndex = 0;
        }

        public override System.Windows.Forms.Control Control
        {
            get { return this.split; }
        }
    }
}
