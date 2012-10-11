using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.ObjectEditor
{
    public class SingleTextEditor: UI.LynnTextbox, IObjectEditor, IUndoHandler, IClipboardHandler, ISelectAllHandler, IDeleteHandler
    {
        protected bool supply;
        
        public SingleTextEditor(Dictionary<string, object> Params)
        {
            this.ContextMenuStrip = new EditContextMenuStrip(this);
            this.TextChanged += new EventHandler(SingleTextEditor_TextChanged);
            if (DirtyChanged != null) DirtyChanged.ToString();
        }

        void SingleTextEditor_TextChanged(object sender, EventArgs e)
        {
            if (!supply) this.Commit();
        }

        public void Commit()
        {
            if (this.RequestCommit != null)
                this.RequestCommit(this, null);
        }

        protected object supp;
        
        public object SelectedItem
        {
            get
            {
                if (supp is FuzzyData.FuzzyString)
                    return new FuzzyData.FuzzyString(this.Text).Encode((supp as FuzzyData.FuzzyString).Encoding);
                else
                    return this.Text;
            }
            set
            {
                supply = true;
                supp = value;
                if (supp is FuzzyData.FuzzyString)
                    this.Text = (value as FuzzyData.FuzzyString).Text;
                else
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
