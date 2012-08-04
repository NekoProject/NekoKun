using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun
{
    public class FindAllResultEditor : AbstractEditor, IClipboardHandler, IUndoHandler, IDeleteHandler, ISelectAllHandler
    {
        UI.NavPointListView view;

        public FindAllResultEditor(FindAllResultFile file)
            : base(file)
        {
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop | WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.Document;

            this.view = new NekoKun.UI.NavPointListView();
            this.view.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Controls.Add(this.view);

            Array.ForEach<NavPoint>(file.Result, this.view.AddItem);
            this.view.SetKeyword(file.Keyword);
        }


        public bool CanCut
        {
            get { return this.view.Selection.Length != 0 && !this.view.IsReadOnly; }
        }

        public bool CanCopy
        {
            get { return this.view.Selection.Length != 0; }
        }

        public bool CanPaste
        {
            get { return this.view.Clipboard.CanPaste; }
        }

        public void Cut()
        {
            this.view.Cut();
        }

        public void Copy()
        {
            this.view.Copy();
        }

        public void Paste()
        {
            this.view.Paste();
        }

        public bool CanUndo
        {
            get { return this.view.CanUndo; }
        }

        public bool CanRedo
        {
            get { return this.view.CanRedo; }
        }

        public void Undo()
        {
            this.view.Undo();
        }

        public void Redo()
        {
            this.view.Redo();
        }

        public bool CanDelete
        {
            get { return this.view.CanDelete; }
        }

        public void Delete()
        {
            this.view.Delete();
        }

        public bool CanSelectAll
        {
            get { return this.view.CanSelectAll; }
        }

        public void SelectAll()
        {
            this.view.SelectAll();
        }

        public bool CanShowFindDialog
        {
            get { return this.view.CanShowFindDialog; }
        }

        public bool CanShowReplaceDialog
        {
            get { return this.view.CanShowReplaceDialog; }
        }

        public void ShowFindDialog()
        {
            this.view.ShowFindDialog();
        }

        public void ShowReplaceDialog()
        {
            this.view.ShowReplaceDialog();
        }
    }
}
