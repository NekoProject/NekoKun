using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using WeifenLuo.WinFormsUI.Docking;

namespace NekoKun
{
    public class ScriptEditor : AbstractEditor, IClipboardHandler, IUndoHandler, IDeleteHandler, ISelectAllHandler, IFindReplaceHandler, IToolboxProvider
    {
        private static System.Windows.Forms.ListBox toolboxControl;
        public NekoKun.UI.Scintilla editor;

        static ScriptEditor()
        {
            toolboxControl = new UI.LynnListbox();
            toolboxControl.Items.AddRange(new string[] { 
                "class << self; self; end"
            });
            toolboxControl.AllowDrop = true;
            toolboxControl.DragEnter += new DragEventHandler(toolboxControl_DragEnter);
            toolboxControl.DragDrop += new DragEventHandler(toolboxControl_DragDrop);
            toolboxControl.MouseMove += new MouseEventHandler(toolboxControl_MouseMove);
        }

        static void toolboxControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left )
                if (toolboxControl.SelectedItem != null)
                {
                    System.Windows.Forms.DataObject obj = new DataObject();
                    obj.SetText(toolboxControl.SelectedItem.ToString(), TextDataFormat.UnicodeText);
                    obj.SetData(typeof(ScriptEditor).FullName + ".toolbox", toolboxControl);
                    toolboxControl.DoDragDrop(obj, DragDropEffects.Copy | DragDropEffects.Move);
                }
        }

        static void toolboxControl_DragDrop(object sender, DragEventArgs e)
        {
            string data = null;
            if (e.Data.GetDataPresent(System.Windows.Forms.DataFormats.UnicodeText))
                data = e.Data.GetData(System.Windows.Forms.DataFormats.UnicodeText) as string;

            if (e.Data.GetDataPresent(typeof(ScriptEditor).FullName + ".toolbox"))
            {
                object obj = e.Data.GetData(typeof(ScriptEditor).FullName + ".toolbox");
                if (obj == toolboxControl)
                {
                    if (data != null)
                    {
                        if (toolboxControl.Items.Contains(data))
                        {
                            int id = toolboxControl.IndexFromPoint(toolboxControl.PointToClient(new Point(e.X, e.Y)));
                            if (id < 0 || id >= toolboxControl.Items.Count)
                                id = toolboxControl.Items.Count - 1;

                            if ((string)toolboxControl.Items[id] == data)
                                return;

                            toolboxControl.Items.Remove(data);
                            toolboxControl.Items.Insert(id, data);
                            toolboxControl.SelectedItem = data;
                        }
                        else
                        {
                            toolboxControl.Items.Add(data);
                            toolboxControl.SelectedItem = data;
                        }
                    }
                }
                else
                {
                    if (data != null && !toolboxControl.Items.Contains(data))
                    {
                        toolboxControl.Items.Add(data);
                        toolboxControl.SelectedItem = data;
                    }
                }
            }
            else if (data != null && !toolboxControl.Items.Contains(data))
            {
                toolboxControl.Items.Add(data);
                toolboxControl.SelectedItem = data;
            }
        }

        static void toolboxControl_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ScriptEditor).FullName + ".toolbox"))
            {
                object obj = e.Data.GetData(typeof(ScriptEditor).FullName + ".toolbox");
                if (obj == toolboxControl)
                    e.Effect = DragDropEffects.Move;
                else
                    e.Effect = DragDropEffects.Copy;
            }
            else if (e.Data.GetDataPresent(System.Windows.Forms.DataFormats.UnicodeText))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

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

        public Control ToolboxControl
        {
            get { return toolboxControl; }
        }
    }
}
