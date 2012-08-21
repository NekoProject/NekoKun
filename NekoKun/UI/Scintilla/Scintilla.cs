using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.UI
{
    public class Scintilla : ScintillaNET.Scintilla, IClipboardHandler, IUndoHandler, IDeleteHandler, ISelectAllHandler, IFindReplaceHandler
    {
    	protected System.Drawing.Color back = System.Drawing.Color.FromArgb(191, 219, 255);
    	
        public Scintilla()
        {

            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Font = Program.GetMonospaceFont();
            this.Styles.Default.Font = this.Font;
            for (int i = 0; i < 200; i++)
            {
                this.Styles[i].Font = this.Font;
            }
            // left margin backcolor
            if (UIManager.Enabled)
            {
                this.Styles[33].BackColor = back;
                this.Margins.FoldMarginColor = back;
            }
            this.Margins[0].Width = 39;
            this.FoldChanged += new EventHandler<ScintillaNET.FoldChangedEventArgs>(Scintilla_FoldChanged);
            this.ContextMenuStrip = new EditContextMenuStrip(this);
        }

        void Scintilla_FoldChanged(object sender, ScintillaNET.FoldChangedEventArgs e)
        {
            if ((e.NewFoldLevel & 0x2000 /*SC_FOLDLEVELHEADERFLAG*/) != 0)
            {
                if ((e.PreviousFoldLevel & 0x2000 /*SC_FOLDLEVELHEADERFLAG*/) == 0)
                {
                    // Adding a fold point.
                    this.Lines[e.Line].FoldExpanded = true;
                    if (this.NativeInterface.SendMessageDirect(2236 /*SCI_GETALLLINESVISIBLE*/) == 0)
                        Expand(e.Line, true, false, 0, e.PreviousFoldLevel);
                }
            }
            else if ((e.PreviousFoldLevel & 0x2000 /*SC_FOLDLEVELHEADERFLAG*/) != 0)
            {
                if (!this.Lines[e.Line].FoldExpanded)
                {
                    // Removing the fold from one that has been contracted so should expand
                    // otherwise lines are left invisible with no way to make them visible
                    this.Lines[e.Line].FoldExpanded = true;
                    if (this.NativeInterface.SendMessageDirect(2236 /*SCI_GETALLLINESVISIBLE*/) == 0)
                        Expand(e.Line, true, false, 0, e.PreviousFoldLevel);
                }
            }
            if ((e.NewFoldLevel & 0x1000/*SC_FOLDLEVELWHITEFLAG*/) == 0 &&
                    ((e.PreviousFoldLevel & 0x0FFF /*SC_FOLDLEVELNUMBERMASK*/) > (e.NewFoldLevel & 0x0FFF /*SC_FOLDLEVELNUMBERMASK*/)))
            {
                if (this.NativeInterface.SendMessageDirect(2236 /*SCI_GETALLLINESVISIBLE*/) == 0)
                {
                    // See if should still be hidden
                    ScintillaNET.Line parentLine = this.Lines[e.Line].FoldParent;
                    if (parentLine == null)
                    {
                        this.Lines[e.Line].IsVisible = true;
                    }
                    else if (parentLine.FoldExpanded && parentLine.IsVisible)
                    {
                        this.Lines[e.Line].IsVisible = true;
                    }
                }
            }
        }
        private void Expand(int line, bool doExpand, bool force, int visLevels, int level) {
            int lineMaxSubord = this.NativeInterface.SendMessageDirect(2224 /*SCI_GETLASTCHILD*/, line, level & 0x0FFF /*SC_FOLDLEVELNUMBERMASK*/);
	        line++;
	        while (line <= lineMaxSubord) {
		        if (force) {
                    this.Lines[line].IsVisible = visLevels > 0;
		        } else {
			        if (doExpand)
                        this.Lines[line].IsVisible = true;
		        }
		        int levelLine = level;
                if (levelLine == -1)
                    levelLine = (int) this.NativeInterface.GetFoldLevel(line);
                if ((levelLine & 0x2000 /*SC_FOLDLEVELHEADERFLAG*/) == 0)
                {
			        if (force) {
					    this.Lines[line].FoldExpanded = visLevels > 1;
				        Expand(line, doExpand, force, visLevels - 1, -1);
			        } else {
				        if (doExpand) {
                            if (!this.Lines[line].FoldExpanded)
                                this.Lines[line].FoldExpanded = true;
					        Expand(line, true, force, visLevels - 1, -1);
				        } else {
					        Expand(line, false, force, visLevels - 1, -1);
				        }
			        }
		        } else {
			        line++;
		        }
	        }
        }

        public bool CanCut
        {
            get { return this.Selection.Length != 0 && !this.IsReadOnly; }
        }

        public bool CanCopy
        {
            get { return this.Selection.Length != 0; }
        }

        public bool CanPaste
        {
            get { return this.Clipboard.CanPaste; }
        }

        public void Cut()
        {
            this.Clipboard.Cut();
        }

        public void Copy()
        {
            this.Clipboard.Copy();
        }

        public void Paste()
        {
            this.Clipboard.Paste();
        }

        public bool CanUndo
        {
            get { return this.UndoRedo.CanUndo; }
        }

        public bool CanRedo
        {
            get { return this.UndoRedo.CanRedo; }
        }

        public void Undo()
        {
            this.UndoRedo.Undo();
        }

        public void Redo()
        {
            this.UndoRedo.Redo();
        }

        public bool CanDelete
        {
            get { return this.Selection.Length != 0 && !this.IsReadOnly; }
        }

        public void Delete()
        {
            this.Selection.Clear();
        }

        public bool CanSelectAll
        {
            get { return this.TextLength > 0; }
        }

        public void SelectAll()
        {
            this.Selection.SelectAll();
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
            this.FindReplace.ShowFind();
        }

        public void ShowReplaceDialog()
        {
            this.FindReplace.ShowReplace();
        }
    }
}
