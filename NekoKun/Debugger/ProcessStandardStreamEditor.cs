using System;
using System.Collections.Generic;
using System.Text;
using ScintillaNet;
using System.Drawing;
using WeifenLuo.WinFormsUI;
using System.Windows.Forms;

namespace NekoKun.Debugger
{
    public class ProcessStandardStreamEditor: AbstractEditor , IClipboardHandler, ISelectAllHandler
    {
        public Scintilla Editor;
        public int InputStart;

        public ProcessStandardStreamEditor(ProcessStandardStreamFile file)
            : base(file)
        {
            this.DockableAreas |= DockAreas.DockBottom | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.Float;

            Editor = new UI.RubyScintilla();
            Editor.Dock = DockStyle.Fill;
            this.Controls.Add(Editor);

            Editor.UndoRedo.IsUndoEnabled = false;
            Editor.Text = file.Buffer;
            InputStart = Editor.TextLength;

            Editor.KeyPress += new KeyPressEventHandler(Editor_KeyPress);
            Editor.KeyDown += new KeyEventHandler(Editor_KeyDown);
            Editor.MouseDown += new MouseEventHandler(Editor_MouseDown);
            Editor.MouseUp += new MouseEventHandler(Editor_MouseUp);
            this.FormClosing += new FormClosingEventHandler(ProcessStandardStreamEditor_FormClosing);

            Editor.ContextMenuStrip = new EditContextMenuStrip(this);
        }

        void Editor_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.Editor.IsReadOnly = false;
            }
        }

        void Editor_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.Editor.IsReadOnly = true;
            }
        }

        void Editor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.Home:
                        this.Editor.Caret.Goto(0);
                        e.Handled = true;
                        return;
                    case Keys.End:
                        this.Editor.Caret.Goto(this.Editor.TextLength);
                        e.Handled = true;
                        return;
                    case Keys.A:
                        this.SelectAll();
                        e.Handled = true;
                        return;
                    case Keys.C:
                    case Keys.Insert:
                        e.Handled = true;
                        if (this.CanCopy)
                            this.Copy();
                        return;
                    case Keys.X:
                        e.Handled = true;
                        if (this.CanCut)
                            this.Cut();
                        return;
                    case Keys.V:
                        e.Handled = true;
                        if (this.CanPaste)
                            this.Paste();
                        return;
                }
            }
            else if (e.Modifiers == Keys.Shift)
            {
                switch (e.KeyCode)
                {
                    case Keys.Insert: // paste
                        e.Handled = true;
                        if (this.CanPaste)
                            this.Paste();
                        return;
                    case Keys.Delete: // cut
                        e.Handled = true;
                        if (this.CanCut)
                            this.Cut();
                        return;
                }
            }

            if ((this.File as ProcessStandardStreamFile).Active == false) { e.SuppressKeyPress = true; return; }

            if ((this.Editor.Selection.Start <= InputStart) && e.KeyCode == Keys.Back) e.SuppressKeyPress = true;

            if ((this.Editor.Selection.Start < this.InputStart) &&
              !(e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down))
                e.SuppressKeyPress = true;

            switch (e.KeyCode)
            {
                case Keys.Enter:

                    if (!(this.File as ProcessStandardStreamFile).Active) return;

                    string input = this.Editor.GetRange(InputStart, this.Editor.TextLength).Text;
                    (this.File as ProcessStandardStreamFile).WriteToSTDIN(input);

                    break;
            }
        }

        void Editor_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        void ProcessStandardStreamEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                (this.File as ProcessStandardStreamFile).Process.Kill();
            }
            catch { }
        }

        public bool CanCut
        {
            get { return this.Editor.Selection.Length != 0 && !this.Editor.IsReadOnly && this.Editor.Selection.Start >= this.InputStart; }
        }

        public bool CanCopy
        {
            get { return this.Editor.Selection.Length != 0; }
        }

        public bool CanPaste
        {
            get { return this.Editor.Clipboard.CanPaste && this.Editor.Selection.Start >= this.InputStart; }
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

        public bool CanSelectAll
        {
            get { return this.Editor.TextLength > 0; }
        }

        public void SelectAll()
        {
            this.Editor.Selection.SelectAll();
        }

        delegate void AppendCallback(string msg);
        public void Append(string msg)
        {
            if (msg.Length == 0) return;

			if (this.Editor.InvokeRequired)
			{	
				AppendCallback d = new AppendCallback(Append);
				this.Invoke(d, new object[] { msg });
			}
			else
			{
                this.Editor.AppendText(msg);
                UpdateCaret();

                InputStart = this.Editor.TextLength;
			}
		}

        public void UpdateCaret()
        {
            this.Editor.Caret.Goto(this.Editor.TextLength);
        }
    }
}
