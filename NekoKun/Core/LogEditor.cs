using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using ScintillaNet;
using WeifenLuo.WinFormsUI.Docking;
namespace NekoKun
{
    public class LogEditor : AbstractEditor, IClipboardHandler, IUndoHandler, IDeleteHandler, ISelectAllHandler
    {
        public Scintilla Editor;

        public LogEditor(LogFile log) : base(log)
        {
            this.DockAreas |= DockAreas.DockBottom | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.Float;

            Editor = new UI.RubyScintilla();
            Editor.Dock = DockStyle.Fill;
            this.Controls.Add(Editor);

            Editor.Text = log.LogText;
            Editor.IsReadOnly = true;

            Editor.ContextMenuStrip = new EditContextMenuStrip(this);
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
            this.Editor.Clipboard.Cut();
        }

        public void Copy()
        {
            this.Editor.Clipboard.Copy();
        }

        public void Paste()
        {
            this.Editor.Clipboard.Paste();
        }

        public bool CanUndo
        {
            get { return this.Editor.UndoRedo.CanUndo; }
        }

        public bool CanRedo
        {
            get { return this.Editor.UndoRedo.CanRedo; }
        }

        public void Undo()
        {
            this.Editor.UndoRedo.Undo();
        }

        public void Redo()
        {
            this.Editor.UndoRedo.Redo();
        }

        public bool CanDelete
        {
            get { return this.Editor.Selection.Length != 0 && !this.Editor.IsReadOnly; }
        }

        public void Delete()
        {
            this.Editor.Selection.Clear();
        }

        public bool CanSelectAll
        {
            get { return this.Editor.TextLength > 0; }
        }

        public void SelectAll()
        {
            this.Editor.Selection.SelectAll();
        }
    }
}
