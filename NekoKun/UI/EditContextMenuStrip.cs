using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace NekoKun
{
    public class EditContextMenuStrip : ContextMenuStrip
    {
        IUndoHandler hUndo;
        IClipboardHandler hClip;
        IDeleteHandler hDelete;
        ISelectAllHandler hSelectAll;
        IFindReplaceHandler hFind;

        ToolStripMenuItem menuUndo, menuRedo, menuCut, menuCopy, menuPaste, menuDelete, menuSelectAll, menuFind, menuReplace;

        public EditContextMenuStrip(Object handler)
            : base()
        {
            hUndo = handler as IUndoHandler;
            hClip = handler as IClipboardHandler;
            hDelete = handler as IDeleteHandler;
            hSelectAll = handler as ISelectAllHandler;
            hFind = handler as IFindReplaceHandler;

            this.menuUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCut = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFind = new System.Windows.Forms.ToolStripMenuItem();
            this.menuReplace = new System.Windows.Forms.ToolStripMenuItem();

            this.menuUndo.Image = global::NekoKun.Properties.Resources.Edit_Undo;
            this.menuUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuUndo.Name = "menuUndo";
            this.menuUndo.ShortcutKeyDisplayString = "Ctrl+Z";
            this.menuUndo.Text = "撤销(&U)";
            this.menuUndo.Click += new System.EventHandler(this.menuUndo_Click);

            this.menuRedo.Image = global::NekoKun.Properties.Resources.Edit_Redo;
            this.menuRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuRedo.Name = "menuRedo";
            this.menuRedo.ShortcutKeyDisplayString = "Ctrl+Y";
            this.menuRedo.Text = "重做(&R)";
            this.menuRedo.Click += new System.EventHandler(this.menuRedo_Click);

            this.menuCut.Image = global::NekoKun.Properties.Resources.Cut;
            this.menuCut.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.menuCut.Name = "menuCut";
            this.menuCut.ShortcutKeyDisplayString = "Ctrl+X";
            this.menuCut.Text = "剪切(&T)";
            this.menuCut.Click += new System.EventHandler(this.menuCut_Click);

            this.menuCopy.Image = global::NekoKun.Properties.Resources.Copy;
            this.menuCopy.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.menuCopy.Name = "menuCopy";
            this.menuCopy.ShortcutKeyDisplayString = "Ctrl+C";
            this.menuCopy.Text = "复制(&C)";
            this.menuCopy.Click += new System.EventHandler(this.menuCopy_Click);

            this.menuPaste.Image = global::NekoKun.Properties.Resources.Paste;
            this.menuPaste.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.menuPaste.Name = "menuPaste";
            this.menuPaste.ShortcutKeyDisplayString = "Ctrl+V";
            this.menuPaste.Text = "粘贴(&P)";
            this.menuPaste.Click += new System.EventHandler(this.menuPaste_Click);

            this.menuDelete.Image = global::NekoKun.Properties.Resources.Delete;
            this.menuDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuDelete.Name = "menuDelete";
            this.menuDelete.ShortcutKeyDisplayString = "Delete";
            this.menuDelete.Text = "删除(&D)";
            this.menuDelete.Click += new System.EventHandler(this.menuDelete_Click);

            this.menuSelectAll.Name = "menuSelectAll";
            this.menuSelectAll.ShortcutKeyDisplayString = "Ctrl+A";
            this.menuSelectAll.Text = "全选(&A)";
            this.menuSelectAll.Click += new System.EventHandler(this.menuSelectAll_Click);

            this.menuFind.Image = global::NekoKun.Properties.Resources.Find;
            this.menuFind.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuFind.Name = "menuFind";
            this.menuFind.ShortcutKeyDisplayString = "Ctrl+F";
            this.menuFind.Text = "查找(&F)";
            this.menuFind.Click += new System.EventHandler(this.menuFind_Click);

            this.menuReplace.Name = "menuReplace";
            this.menuReplace.ShortcutKeyDisplayString = "Ctrl+H";
            this.menuReplace.Text = "替换(&R)";
            this.menuReplace.Click += new System.EventHandler(this.menuReplace_Click);

            this.Items.AddRange(new ToolStripItem[] {
                this.menuUndo, this.menuRedo,
                new ToolStripSeparator(),
                this.menuCut, this.menuCopy, this.menuPaste, this.menuDelete,
                new ToolStripSeparator(),
                this.menuSelectAll,
                new ToolStripSeparator(),
                this.menuFind, this.menuReplace
            });

            this.Opening += new System.ComponentModel.CancelEventHandler(EditContextMenuStrip_Opening);
        }

        public void ResetHandler(object handler)
        {
            hUndo = handler as IUndoHandler;
            hClip = handler as IClipboardHandler;
            hDelete = handler as IDeleteHandler;
            hSelectAll = handler as ISelectAllHandler;
            hFind = handler as IFindReplaceHandler;
        }

        private void EditContextMenuStrip_Opening(object sender, EventArgs e)
        {
            if (hUndo != null)
            {
                this.menuUndo.Enabled = hUndo.CanUndo;
                this.menuRedo.Enabled = hUndo.CanRedo;
            }
            else
            {
                this.menuUndo.Enabled = false;
                this.menuRedo.Enabled = false;
            }

            if (hClip != null)
            {
                this.menuCut.Enabled = hClip.CanCut;
                this.menuCopy.Enabled = hClip.CanCopy;
                this.menuPaste.Enabled = hClip.CanPaste;
            }
            else
            {
                this.menuCut.Enabled = false;
                this.menuCopy.Enabled = false;
                this.menuPaste.Enabled = false;
            }

            if (hDelete != null)
            {
                this.menuDelete.Enabled = hDelete.CanDelete;
            }
            else
            {
                this.menuDelete.Enabled = false;
            }

            if (hSelectAll != null)
            {
                this.menuSelectAll.Enabled = hSelectAll.CanSelectAll;
            }
            else
            {
                this.menuSelectAll.Enabled = false;
            }

            if (hFind != null)
            {
                this.menuFind.Enabled = hFind.CanShowFindDialog;
                this.menuReplace.Enabled = hFind.CanShowReplaceDialog;
            }
            else
            {
                this.menuFind.Enabled = false;
                this.menuReplace.Enabled = false;
            }
        }

        private void menuUndo_Click(object sender, EventArgs e)
        {
            if (hUndo != null && hUndo.CanUndo) hUndo.Undo();
        }

        private void menuRedo_Click(object sender, EventArgs e)
        {
            if (hUndo != null && hUndo.CanRedo) hUndo.Redo();
        }

        private void menuCut_Click(object sender, EventArgs e)
        {
            if (hClip != null && hClip.CanCut) hClip.Cut();
        }

        private void menuCopy_Click(object sender, EventArgs e)
        {
            if (hClip != null && hClip.CanCopy) hClip.Copy();
        }

        private void menuPaste_Click(object sender, EventArgs e)
        {
            if (hClip != null && hClip.CanPaste) hClip.Paste();
        }

        private void menuDelete_Click(object sender, EventArgs e)
        {
            if (hDelete != null && hDelete.CanDelete) hDelete.Delete();
        }

        private void menuSelectAll_Click(object sender, EventArgs e)
        {
            if (hSelectAll != null && hSelectAll.CanSelectAll) hSelectAll.SelectAll();
        }

        private void menuFind_Click(object sender, EventArgs e)
        {
            if (hFind != null && hFind.CanShowFindDialog) hFind.ShowFindDialog();
        }

        private void menuReplace_Click(object sender, EventArgs e)
        {
            if (hFind != null && hFind.CanShowReplaceDialog) hFind.ShowReplaceDialog();
        }
    }
}
