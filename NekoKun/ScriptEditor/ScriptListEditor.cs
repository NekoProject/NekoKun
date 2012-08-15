using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace NekoKun
{
    public class ScriptListEditor : AbstractEditor, IDeleteHandler  
    {
        public UI.LynnListbox list = new UI.LynnListbox();
        private ScriptListFile scriptList;

        private ToolStripMenuItem menuOpen;
        private ToolStripMenuItem menuInsert;
        private ToolStripMenuItem menuDelete;
        private ToolStripMenuItem menuRename;

        public ScriptListEditor(ScriptListFile item) : base(item)
        {
            list.Dock = DockStyle.Fill;
            this.Controls.Add(list);

            this.Text = "脚本列表";
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop | WeifenLuo.WinFormsUI.Docking.DockAreas.Document | WeifenLuo.WinFormsUI.Docking.DockAreas.Float;

            scriptList = item;

            foreach (ScriptFile file in scriptList.scripts)
            {
                this.list.Items.Add(file);
            }

            ContextMenuStrip menu = new ContextMenuStrip();
            (menuOpen = menu.Items.Add("打开(&O)", null, delegate {
                ActionOpenScript();
            }) as ToolStripMenuItem).ShortcutKeyDisplayString = "Enter";
            menu.Items.Add(new ToolStripSeparator());
            (menuInsert = menu.Items.Add("插入(&I)...", null, delegate
            {
                ActionInsertScript();
            }) as ToolStripMenuItem).ShortcutKeyDisplayString = "Insert";
            (menuDelete = menu.Items.Add("删除(&D)", null, delegate
            {
                ActionDeleteScript();
            }) as ToolStripMenuItem).ShortcutKeyDisplayString = "Delete";
            (menuRename = menu.Items.Add("重命名(&R)...", null, delegate
            {
                ActionRenameScript();
            }) as ToolStripMenuItem).ShortcutKeyDisplayString = "F2";

            menu.Opening += delegate(object sender, System.ComponentModel.CancelEventArgs args)
            {
                this.menuOpen.Enabled = this.list.SelectedItem != null;
                this.menuDelete.Enabled = this.list.SelectedItem != null;
                this.menuRename.Enabled = this.list.SelectedItem != null;
            };

            this.list.ContextMenuStrip = menu;
            this.list.AllowDrop = true;
            this.list.MouseMove += new MouseEventHandler(list_MouseMove);
            this.list.DragEnter += new DragEventHandler(list_DragEnter);
            this.list.DragDrop += new DragEventHandler(list_DragDrop);
            this.list.KeyDown += new KeyEventHandler(list_KeyDown);
            this.list.DoubleClick += new EventHandler(list_DoubleClick);
        }

        void list_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.list.SelectedItem != null && e.Button == MouseButtons.Right)
            {
                DataObject obj = new DataObject(this.File.filename, this.list.SelectedItem);
                this.list.DoDragDrop(obj, DragDropEffects.Move);
            }
        }

        void list_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(this.File.filename, false) && sender == this.list)
            {
                e.Effect = System.Windows.Forms.DragDropEffects.Move;
            }
        }

        void list_DragDrop(object sender, DragEventArgs e)
        {
            object newItem;

            if (e.Data.GetDataPresent(this.File.filename, false))
            {
                Point pt = ((System.Windows.Forms.ListBox)sender).PointToClient(new Point(e.X, e.Y));
                if (sender != this.list)
                    return;

                try
                {
                    int id = this.list.IndexFromPoint(pt);
                    object destItem = this.list.Items[id];
                    if (destItem != null)
                    {
                        newItem = e.Data.GetData(this.File.filename);
                        if (newItem != destItem)
                        {
                            this.list.Items.Remove(newItem);
                            this.list.Items.Insert(id, newItem);
                            this.scriptList.scripts.Remove(newItem as ScriptFile);
                            this.scriptList.scripts.Insert(id, newItem as ScriptFile);
                            this.File.MakeDirty();
                        }
                    }
                }
                catch { }
            }
        }

        void list_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    ActionOpenScript();
                    break;
                case Keys.Delete:
                    ActionDeleteScript();
                    break;
                case Keys.Insert:
                    ActionInsertScript();
                    break;
                case Keys.F2:
                    ActionRenameScript();
                    break;
                default:
                    break;
            }
        }

        void list_DoubleClick(object sender, EventArgs e)
        {
            ActionOpenScript();
        }

        private void ActionRenameScript()
        {
            ScriptFile item = this.list.SelectedItem as ScriptFile;
            int index = this.list.SelectedIndex;

            if (item == null)
                return;

            ScriptListEditorRenameDialog dialog = new ScriptListEditorRenameDialog(this.scriptList, item);
            DialogResult result = dialog.ShowDialog(this);

            if (result != System.Windows.Forms.DialogResult.OK)
                return;

            this.scriptList.DeleteFile(item);
            this.list.Items.Remove(item);

            ScriptFile file = this.scriptList.InsertFile(dialog.result, index);
            file.Code = item.Code;

            this.list.Items.Insert(index, file);
            this.list.SelectedItem = file;
            file.ShowEditor();
        }

        private void ActionOpenScript()
        {
            ScriptFile item = this.list.SelectedItem as ScriptFile;

            if (item == null)
                return;

            item.ShowEditor();
        }

        private void ActionDeleteScript()
        {
            ScriptFile item = this.list.SelectedItem as ScriptFile;

            if (item == null)
                return;

            this.scriptList.DeleteFile(item);
            this.list.Items.Remove(item);
        }

        private void ActionInsertScript()
        {
            ScriptFile item = this.list.SelectedItem as ScriptFile;
            int index = this.list.SelectedIndex;

            if (item == null)
            {
                index = 0;
            }

            ScriptListEditorInsertDialog dialog = new ScriptListEditorInsertDialog(this.scriptList);
            DialogResult result = dialog.ShowDialog(this);

            if (result != System.Windows.Forms.DialogResult.OK)
                return;

            ScriptFile file = this.scriptList.InsertFile(dialog.result, index);
            this.list.Items.Insert(index, file);

            this.list.SelectedItem = file;
            file.ShowEditor();
        }



        public bool CanDelete
        {
            get { return (this.list.SelectedItem != null); }
        }

        public void Delete()
        {
            ActionDeleteScript();
        }
    }
}
