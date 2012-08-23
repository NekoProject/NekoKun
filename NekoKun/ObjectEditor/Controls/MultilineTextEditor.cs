using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.ObjectEditor
{
    public class MultilineTextEditor: UI.LynnTextbox, IObjectEditor, IUndoHandler, IClipboardHandler, ISelectAllHandler, IDeleteHandler
    {
        protected bool supply;

        public MultilineTextEditor(Dictionary<string, object> Params)
        {
            this.Multiline = true;
            this.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ContextMenuStrip = new EditContextMenuStrip(this);
            this.TextChanged += new EventHandler(MultilineTextEditor_TextChanged);
            if (DirtyChanged != null) DirtyChanged.ToString();
        }

        void MultilineTextEditor_TextChanged(object sender, EventArgs e)
        {
            if (!supply) this.Commit();
        }

        public void Commit()
        {
            if (this.RequestCommit != null)
                this.RequestCommit(this, null);
        }

        public object SelectedItem
        {
            get
            {
                return this.Text;
            }
            set
            {
                supply = true;
                this.Text = value as string;
                supply = false;
            }
        }

        public event EventHandler RequestCommit;

        #region IObjectEditor 成员


        public event EventHandler DirtyChanged;

        #endregion

        public bool CanRedo
        {
            get { return false; }
        }

        public void Redo()
        {
            throw new NotImplementedException();
        }

        public bool CanCut
        {
            get { return !this.ReadOnly && this.SelectionLength != 0; }
        }

        public bool CanCopy
        {
            get { return this.SelectionLength != 0; }
        }

        public bool CanPaste
        {
            get { return !this.ReadOnly && System.Windows.Forms.Clipboard.ContainsText(); }
        }

        public bool CanSelectAll
        {
            get { return !(this.SelectionStart == 0 && this.SelectionLength == this.Text.Length) && this.Text.Length > 0; }
        }

        public bool CanDelete
        {
            get { return !this.ReadOnly && this.Text.Length > 0; }
        }

        public void Delete()
        {
            this.Text = "";
        }

    }
}
