using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI;

namespace NekoKun
{
    public partial class Workbench : Form
    {
        private List<AbstractFile> pendingChanges = new List<AbstractFile>();

        private static Workbench instance;
        public static Workbench Instance
        {
            get {
                if (instance == null)
                    instance = new Workbench();

                return instance;
            }
        }

        public DockPanel DockPanel = new DockPanel();
        public ScriptListFile ScriptList;
        public static string scriptDir = System.IO.Path.GetFullPath(System.IO.Path.Combine(Program.ProjectPath, @"Data\\Scripts\\source\\"));
        public static string scriptListFile = System.IO.Path.Combine(scriptDir, "rakefile.info");

        private Workbench()
        {
            DockPanel.Dock = DockStyle.Fill;
            this.IsMdiContainer = true;
            DockPanel.DocumentStyle = DocumentStyles.DockingWindow;
            this.Controls.Add(DockPanel);
            InitializeComponent();

            this.Load += new EventHandler(Workbench_Load);
            this.FormClosing += new FormClosingEventHandler(Workbench_FormClosing);
        }

        void Workbench_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.pendingChanges.Count >= 1)
            {
                DialogResult result = MessageBox.Show(this, "您已经修改了本工程。在关闭编辑器前是否需要保存呢？", this.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                switch (result)
                {
                    case DialogResult.Cancel:
                        break;
                    case DialogResult.No:
                        return;
                    case DialogResult.Yes:
                        this.SaveProject();
                        if (this.pendingChanges.Count == 0)
                            return;
                        break;
                }
                e.Cancel = true;
            }
        }

        void Workbench_Load(object sender, EventArgs e)
        {
            Program.Logger.ShowEditor();
            Program.Logger.Editor.DockState = DockState.DockBottom;

            ScriptList = new ScriptListFile(scriptListFile);
            ScriptList.ShowEditor();
            ScriptList.Editor.DockState = DockState.DockLeft;

            UpdatePendingChanges();
        }

        public void AddPendingChange(AbstractFile file)
        {
            pendingChanges.Add(file);
            UpdatePendingChanges();
        }

        void UpdatePendingChanges()
        {
            statusPendingChanges.Visible = pendingChanges.Count != 0;
        }

        private void statusPendingChanges_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("当前共有 {0} 个文件尚未保存：", pendingChanges.Count));
            foreach (AbstractFile file in pendingChanges)
            {
                sb.AppendLine(string.Format("  {0}: {1}；", file.GetType().Name, file.filename));
            }
            Program.Logger.Log(sb.ToString());
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuSave_Click(object sender, EventArgs e)
        {
            SaveProject();
        }

        public void SaveProject()
        {
            List<AbstractFile> changes = new List<AbstractFile>(pendingChanges);

            foreach (AbstractFile file in changes)
            {
                file.Commit();
                pendingChanges.Remove(file);
            }

            UpdatePendingChanges();
        }

        public void RunNormal()
        {
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(System.IO.Path.Combine(Program.ProjectPath, "Game.exe"));
            System.Diagnostics.Process.Start(info);
        }

        public void RunDebug()
        {
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(System.IO.Path.Combine(Program.ProjectPath, "Game.exe"));
            //System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo("cmd.exe");
            info.UseShellExecute = false;
            info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            info.StandardOutputEncoding = Encoding.UTF8;
            info.StandardErrorEncoding = Encoding.UTF8;
            info.RedirectStandardError = true;
            info.RedirectStandardInput = true;
            info.RedirectStandardOutput = true;
            System.Diagnostics.Process proc = System.Diagnostics.Process.Start(info);

            AbstractFile file = (new Debugger.ProcessStandardStreamFile(proc));
            file.ShowEditor();
            try
            {
                file.Editor.Show(Program.Logger.Editor.Pane, Program.Logger.Editor);
            }
            catch { }
        }

        private void menuDebugGame_Click(object sender, EventArgs e)
        {
            RunDebug();
        }

        private void menuRunGame_Click(object sender, EventArgs e)
        {
            RunNormal();
        }

        private void menuViewScriptList_Click(object sender, EventArgs e)
        {
            ScriptList.ShowEditor();
        }

        private void menuViewLog_Click(object sender, EventArgs e)
        {
            Program.Logger.ShowEditor();
        }

        private void menuEdit_DropDownOpening(object sender, EventArgs e)
        {
            IUndoHandler hUndo = this.DockPanel.ActiveContent as IUndoHandler;
            IClipboardHandler hClip = this.DockPanel.ActiveContent as IClipboardHandler;
            IDeleteHandler hDelete = this.DockPanel.ActiveContent as IDeleteHandler;
            ISelectAllHandler hSelectAll = this.DockPanel.ActiveContent as ISelectAllHandler;
            IFindReplaceHandler hFind = this.DockPanel.ActiveContent as IFindReplaceHandler;

            if (hUndo != null)
            {
                this.menuEditUndo.Enabled = hUndo.CanUndo;
                this.menuEditRedo.Enabled = hUndo.CanRedo;
            }
            else
            {
                this.menuEditUndo.Enabled = false;
                this.menuEditRedo.Enabled = false;
            }

            if (hClip != null)
            {
                this.menuEditCut.Enabled = hClip.CanCut;
                this.menuEditCopy.Enabled = hClip.CanCopy;
                this.menuEditPaste.Enabled = hClip.CanPaste;
            }
            else
            {
                this.menuEditCut.Enabled = false;
                this.menuEditCopy.Enabled = false;
                this.menuEditPaste.Enabled = false;
            }

            if (hDelete != null)
            {
                this.menuEditDelete.Enabled = hDelete.CanDelete;
            }
            else
            {
                this.menuEditDelete.Enabled = false;
            }

            if (hSelectAll != null)
            {
                this.menuEditSelectAll.Enabled = hSelectAll.CanSelectAll;
            }
            else
            {
                this.menuEditSelectAll.Enabled = false;
            }

            if (hFind != null)
            {
                this.menuEditFind.Enabled = hFind.CanShowFindDialog;
                this.menuEditReplace.Enabled = hFind.CanShowReplaceDialog;
            }
            else
            {
                this.menuEditFind.Enabled = false;
                this.menuEditReplace.Enabled = false;
            }
        }

        private void menuEditUndo_Click(object sender, EventArgs e)
        {
            IUndoHandler h = this.DockPanel.ActiveContent as IUndoHandler;
            if (h != null && h.CanUndo) h.Undo();
        }

        private void menuEditRedo_Click(object sender, EventArgs e)
        {
            IUndoHandler h = this.DockPanel.ActiveContent as IUndoHandler;
            if (h != null && h.CanRedo) h.Redo();
        }

        private void menuEditCut_Click(object sender, EventArgs e)
        {
            IClipboardHandler h = this.DockPanel.ActiveContent as IClipboardHandler;
            if (h != null && h.CanCut) h.Cut();
        }

        private void menuEditCopy_Click(object sender, EventArgs e)
        {
            IClipboardHandler h = this.DockPanel.ActiveContent as IClipboardHandler;
            if (h != null && h.CanCopy) h.Copy();
        }

        private void menuEditPaste_Click(object sender, EventArgs e)
        {
            IClipboardHandler h = this.DockPanel.ActiveContent as IClipboardHandler;
            if (h != null && h.CanPaste) h.Paste();
        }

        private void menuEditDelete_Click(object sender, EventArgs e)
        {
            IDeleteHandler h = this.DockPanel.ActiveContent as IDeleteHandler;
            if (h != null && h.CanDelete) h.Delete();
        }

        private void menuEditSelectAll_Click(object sender, EventArgs e)
        {
            ISelectAllHandler h = this.DockPanel.ActiveContent as ISelectAllHandler;
            if (h != null && h.CanSelectAll) h.SelectAll();
        }

        private void menuEditFind_Click(object sender, EventArgs e)
        {
            IFindReplaceHandler h = this.DockPanel.ActiveContent as IFindReplaceHandler;
            if (h != null && h.CanShowFindDialog) h.ShowFindDialog();
        }

        private void menuEditReplace_Click(object sender, EventArgs e)
        {
            IFindReplaceHandler h = this.DockPanel.ActiveContent as IFindReplaceHandler;
            if (h != null && h.CanShowReplaceDialog) h.ShowReplaceDialog();
        }

        private void menuEditFindAll_Click(object sender, EventArgs e)
        {
            var dialog = new FindAllDialog();
            dialog.ShowDialog();

            if (dialog.DialogResult == DialogResult.OK)
            {
                var keyword = dialog.Keyword;
                var result = FileManager.FindAll(keyword);
                var file = new FindAllResultFile(keyword, result);
                file.ShowEditor();
                try
                {
                    file.Editor.Show(Program.Logger.Editor.Pane, Program.Logger.Editor);
                }
                catch { }
            }
        }
    }
}
