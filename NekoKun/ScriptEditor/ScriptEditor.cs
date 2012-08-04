using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using ScintillaNET;
using WeifenLuo.WinFormsUI.Docking;

namespace NekoKun
{
    public class ScriptEditor : AbstractEditor, IClipboardHandler, IUndoHandler, IDeleteHandler, ISelectAllHandler, IFindReplaceHandler
    {
        public Scintilla editor;

        public ScriptEditor(ScriptFile item) : base(item)
        {
            editor = new UI.RubyScintilla();
            editor.Dock = DockStyle.Fill;
            this.Controls.Add(editor);

            editor.Text = (this.File as ScriptFile).Code;
            editor.UndoRedo.EmptyUndoBuffer();
            editor.TextDeleted += new EventHandler<TextModifiedEventArgs>(editor_TextDeleted);
            editor.TextInserted += new EventHandler<TextModifiedEventArgs>(editor_TextInserted);
            editor.Scrolling.HorizontalWidth = 1;

            this.FormClosing += new FormClosingEventHandler(ScriptEditor_FormClosing);

            editor.ContextMenuStrip = new EditContextMenuStrip(this);
        }

        public override void Commit()
        {
            (File as ScriptFile).Code = this.editor.Text;
        }

        void ScriptEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Commit();
        }

        void editor_TextInserted(object sender, TextModifiedEventArgs e)
        {
            this.File.MakeDirty();
        }

        void editor_TextDeleted(object sender, TextModifiedEventArgs e)
        {
            this.File.MakeDirty();
        }



        public bool CanCut
        {
            get { return this.editor.Selection.Length != 0 && !this.editor.IsReadOnly; }
        }

        public bool CanCopy
        {
            get { return this.editor.Selection.Length != 0; }
        }

        public bool CanPaste
        {
            get { return this.editor.Clipboard.CanPaste; }
        }

        public void Cut()
        {
            this.editor.Clipboard.Cut();
        }

        public void Copy()
        {
            this.editor.Clipboard.Copy();
        }

        public void Paste()
        {
            this.editor.Clipboard.Paste();
        }

        public bool CanUndo
        {
            get { return this.editor.UndoRedo.CanUndo; }
        }

        public bool CanRedo
        {
            get { return this.editor.UndoRedo.CanRedo; }
        }

        public void Undo()
        {
            this.editor.UndoRedo.Undo();
        }

        public void Redo()
        {
            this.editor.UndoRedo.Redo();
        }

        public bool CanDelete
        {
            get { return this.editor.Selection.Length != 0 && !this.editor.IsReadOnly; }
        }

        public void Delete()
        {
            this.editor.Selection.Clear();
        }

        public bool CanSelectAll
        {
            get { return this.editor.TextLength > 0; }
        }

        public void SelectAll()
        {
            this.editor.Selection.SelectAll();
        }

        public bool CanShowFindDialog
        {
            get { return true; }
        }

        public bool CanShowReplaceDialog
        {
            get { return true; }
        }

        public void ShowFindDialog()
        {
            this.editor.FindReplace.ShowFind();
        }

        public void ShowReplaceDialog()
        {
            this.editor.FindReplace.ShowReplace();
        }
    }
}
