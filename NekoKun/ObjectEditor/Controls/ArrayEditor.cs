using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace NekoKun.ObjectEditor
{
    public class ArrayEditor : System.Windows.Forms.SplitContainer, IObjectEditor, IClipboardHandler, IDeleteHandler
    {
        object orig;
        System.Windows.Forms.ListBox list;
        System.Windows.Forms.Control con;
        System.Windows.Forms.ToolStripMenuItem menuRelength;
        public ArrayEditor(IObjectEditor Con)
        {
            if(this.RequestCommit != null) this.RequestCommit.ToString();

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

            this.list.ContextMenuStrip = new EditContextMenuStrip(this);
            this.list.ContextMenuStrip.Items.Add(new System.Windows.Forms.ToolStripSeparator());
            this.list.ContextMenuStrip.Items.Add(menuRelength = new System.Windows.Forms.ToolStripMenuItem("更改最大值(&M)...", null, delegate
            {
                this.Relength(10);
            }));
            this.list.ContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(ContextMenuStrip_Opening);
        }

        void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            menuRelength.Enabled = (this.CreateDefaultObject != null);
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
                if (this.DirtyChanged != null)
                    this.DirtyChanged(this, null);
            }
            else if (newsize < this.list.Items.Count)
            {
                int count = this.list.Items.Count;
                for (int i = newsize; i < count; i++)
                {
                    this.list.Items.RemoveAt(newsize);
                }
                if (this.DirtyChanged != null)
                    this.DirtyChanged(this, null);
            }
        }

        //Still loving my tree, but losing you.

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
            if (this.DirtyChanged != null)
                this.DirtyChanged(this, null);
        }

        public void Copy()
        {
            System.Windows.Forms.DataObject db = new System.Windows.Forms.DataObject();
            db.SetData(this.ClipboardFormat, this.DumpObject(this.list.SelectedItem));
            System.Windows.Forms.Clipboard.SetDataObject(db, true, 5, 100);
            if (this.DirtyChanged != null)
                this.DirtyChanged(this, null);
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
            this.list.Items[this.list.SelectedIndex] = this.LoadObject(buffer);
            (con as IObjectEditor).SelectedItem = this.list.Items[this.list.SelectedIndex];
            if (this.DirtyChanged != null)
                this.DirtyChanged(this, null);
        }

        public bool CanDelete
        {
            get { return this.list.SelectedItem != null && this.CreateDefaultObject != null; }
        }

        public void Delete()
        {
            this.list.Items[this.list.SelectedIndex] = this.CreateDefaultObject();
            (con as IObjectEditor).SelectedItem = this.list.Items[this.list.SelectedIndex];
            if (this.DirtyChanged != null)
                this.DirtyChanged(this, null);
        }

        public CreateDefaultObjectDelegate CreateDefaultObject;
        public DumpObjectDelegate DumpObject;
        public LoadObjectDelegate LoadObject;
        public string ClipboardFormat;

        public delegate object CreateDefaultObjectDelegate();
        public delegate byte[] DumpObjectDelegate(object obj);
        public delegate object LoadObjectDelegate(byte[] obj);
    }
}
