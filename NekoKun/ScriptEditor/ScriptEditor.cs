using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using WeifenLuo.WinFormsUI.Docking;

namespace NekoKun
{
    public class ScriptEditor : AbstractEditor, IClipboardHandler, IUndoHandler, IDeleteHandler, ISelectAllHandler, IFindReplaceHandler
    {
        public NekoKun.UI.Scintilla editor;

        public ScriptEditor(ScriptFile item) : base(item)
        {
            editor = new UI.RubyScintilla();
            editor.Dock = DockStyle.Fill;
            this.Controls.Add(editor);

            editor.Text = (this.File as ScriptFile).Code;
            editor.UndoRedo.EmptyUndoBuffer();
            editor.TextDeleted += new EventHandler<ScintillaNET.TextModifiedEventArgs>(editor_TextDeleted);
            editor.TextInserted += new EventHandler<ScintillaNET.TextModifiedEventArgs>(editor_TextInserted);
            editor.Scrolling.HorizontalWidth = 1;

            this.FormClosing += new FormClosingEventHandler(ScriptEditor_FormClosing);
        }

        public override void Commit()
        {
            (File as ScriptFile).Code = this.editor.Text;
        }

        void ScriptEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Commit();
        }

        void editor_TextInserted(object sender, ScintillaNET.TextModifiedEventArgs e)
        {
            this.File.MakeDirty();
        }

        void editor_TextDeleted(object sender, ScintillaNET.TextModifiedEventArgs e)
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
            this.editor.Cut();
        }

        public void Copy()
        {
            this.editor.Copy();
        }

        public void Paste()
        {
            this.editor.Paste();
        }

        public bool CanUndo
        {
            get { return this.editor.CanUndo; }
        }

        public bool CanRedo
        {
            get { return this.editor.CanRedo; }
        }

        public void Undo()
        {
            this.editor.Undo();
        }

        public void Redo()
        {
            this.editor.Redo();
        }

        public bool CanDelete
        {
            get { return this.editor.CanDelete; }
        }

        public void Delete()
        {
            this.editor.Delete();
        }

        public bool CanSelectAll
        {
            get { return this.editor.CanSelectAll; }
        }

        public void SelectAll()
        {
            this.editor.SelectAll();
        }

        public bool CanShowFindDialog
        {
            get { return this.editor.CanShowFindDialog; }
        }

        public bool CanShowReplaceDialog
        {
            get { return this.editor.CanShowReplaceDialog; }
        }

        public void ShowFindDialog()
        {
            this.editor.ShowFindDialog();
        }

        public void ShowReplaceDialog()
        {
            this.editor.ShowReplaceDialog();
        }
    }
}
