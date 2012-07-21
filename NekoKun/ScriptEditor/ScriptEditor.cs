using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using ScintillaNet;
using WeifenLuo.WinFormsUI;

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

            editor.CharAdded += new EventHandler<CharAddedEventArgs>(editor_CharAdded);

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

        void editor_CharAdded(object sender, CharAddedEventArgs e)
        {
            if ((e.Ch == 10 || e.Ch == 13) && editor.Lines.Current.Number > 0)
            {
                int ind = editor.Lines.Current.Indentation - 1;
                if ((editor.Lines.Current.Number == 1 &&
                     editor.Lines[editor.Lines.Current.Number - 1].FoldLevel > 1024) ||
                    (editor.Lines.Current.Number > 1 &&
                     editor.Lines[editor.Lines.Current.Number - 1].FoldLevel >
                     editor.Lines[editor.Lines.Current.Number - 2].FoldLevel))
                {
                    ind += editor.Indentation.IndentWidth;
                }
                if (ind > 0)
                {
                    editor.Lines.Current.Indentation = ind;
                    editor.GoTo.Position(editor.Lines.Current.StartPosition + ind);
                }
            }
            /*
              def on_editor_char_added chr
                curr_line = @editor.get_current_line
                if [10, 13].include? chr and curr_line > 0
                  line_ind = @editor.get_line_indentation curr_line - 1
                  if (curr_line == 1 and @editor.get_fold_level(curr_line - 1) > 1024) or 
                    (curr_line > 1 and @editor.get_fold_level(curr_line - 1) >
                    @editor.get_fold_level(curr_line - 2))
                    line_ind += @editor.get_indent
                  # FIXME: auto "unindent"
                  #elsif get_line(curr_line - 1).strip == 'end'
                  #  line_ind -= get_indent
                  #  line_ind = 0 if line_ind < 0
                  #  set_line_indentation curr_line - 1, line_ind
                  #elsif get_line(curr_line - 1).strip == 'else'
                  #  set_line_indentation curr_line - 1, line_ind - get_indent
                  end
                  if line_ind > 0
                    @editor.set_line_indentation curr_line, line_ind
                    @editor.goto_pos @editor.position_from_line(curr_line) + line_ind
                  end
                end
              end
            */
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
