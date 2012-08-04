using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using ScintillaNET;
using WeifenLuo.WinFormsUI.Docking;
namespace NekoKun
{
    public class LogEditor : AbstractEditor, IClipboardHandler, IUndoHandler, IDeleteHandler, ISelectAllHandler
    {
        public UI.Scintilla Editor;

        public LogEditor(LogFile log) : base(log)
        {
            this.DockAreas |= DockAreas.DockBottom | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.Float;

            Editor = new UI.RubyScintilla();
            Editor.Dock = DockStyle.Fill;
            this.Controls.Add(Editor);

            Editor.Text = log.LogText;
            Editor.IsReadOnly = true;

        }

        public void Append(string str)
        {
            Editor.IsReadOnly = false;
            this.Editor.AppendText(str);
            Editor.IsReadOnly = true;
            this.Editor.Caret.Goto(this.Editor.TextLength);
        }
        public bool CanCut
        {
            get { return this.Editor.Selection.Length != 0 && !this.Editor.IsReadOnly; }
        }

        public bool CanCopy
        {
            get { return this.Editor.Selection.Length != 0; }
        }

        public bool CanPaste
        {
            get { return this.Editor.Clipboard.CanPaste; }
        }

        public void Cut()
        {
            this.Editor.Cut();
        }

        public void Copy()
        {
            this.Editor.Copy();
        }

        public void Paste()
        {
            this.Editor.Paste();
        }

        public bool CanUndo
        {
            get { return this.Editor.CanUndo; }
        }

        public bool CanRedo
        {
            get { return this.Editor.CanRedo; }
        }

        public void Undo()
        {
            this.Editor.Undo();
        }

        public void Redo()
        {
            this.Editor.Redo();
        }

        public bool CanDelete
        {
            get { return this.Editor.CanDelete; }
        }

        public void Delete()
        {
            this.Editor.Delete();
        }

        public bool CanSelectAll
        {
            get { return this.Editor.CanSelectAll; }
        }

        public void SelectAll()
        {
            this.Editor.SelectAll();
        }

        public bool CanShowFindDialog
        {
            get { return this.Editor.CanShowFindDialog; }
        }

        public bool CanShowReplaceDialog
        {
            get { return this.Editor.CanShowReplaceDialog; }
        }

        public void ShowFindDialog()
        {
            this.Editor.ShowFindDialog();
        }

        public void ShowReplaceDialog()
        {
            this.Editor.ShowReplaceDialog();
        }
    }
}
