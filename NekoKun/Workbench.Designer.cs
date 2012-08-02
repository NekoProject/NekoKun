namespace NekoKun
{
    partial class Workbench
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStripPanelTop = new System.Windows.Forms.ToolStripPanel();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuEditSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuEditReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditFindAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuView = new System.Windows.Forms.ToolStripMenuItem();
            this.menuViewScriptList = new System.Windows.Forms.ToolStripMenuItem();
            this.menuViewLog = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDebug = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRunGame = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripPanelBottom = new System.Windows.Forms.ToolStripPanel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusPendingChanges = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripPanelLeft = new System.Windows.Forms.ToolStripPanel();
            this.toolStripPanelRight = new System.Windows.Forms.ToolStripPanel();
            this.menuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditCut = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditFind = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDebugGame = new System.Windows.Forms.ToolStripMenuItem();
            this.toolSaveAll = new System.Windows.Forms.ToolStripButton();
            this.toolRun = new System.Windows.Forms.ToolStripButton();
            this.toolStripPanelTop.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.toolStripMain.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripPanelTop
            // 
            this.toolStripPanelTop.Controls.Add(this.menuStrip);
            this.toolStripPanelTop.Controls.Add(this.toolStripMain);
            this.toolStripPanelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolStripPanelTop.Location = new System.Drawing.Point(0, 0);
            this.toolStripPanelTop.Name = "toolStripPanelTop";
            this.toolStripPanelTop.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.toolStripPanelTop.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.toolStripPanelTop.Size = new System.Drawing.Size(803, 25);
            // 
            // menuStrip
            // 
            this.menuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.menuStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuEdit,
            this.menuView,
            this.menuDebug});
            this.menuStrip.Location = new System.Drawing.Point(3, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(243, 24);
            this.menuStrip.Stretch = false;
            this.menuStrip.TabIndex = 9;
            this.menuStrip.Text = "menuStrip";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSave,
            this.toolStripSeparator1,
            this.menuExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(57, 20);
            this.menuFile.Text = "文件(&F)";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(173, 6);
            // 
            // menuEdit
            // 
            this.menuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuEditUndo,
            this.menuEditRedo,
            this.toolStripSeparator2,
            this.menuEditCut,
            this.menuEditCopy,
            this.menuEditPaste,
            this.menuEditDelete,
            this.toolStripSeparator3,
            this.menuEditSelectAll,
            this.toolStripSeparator4,
            this.menuEditFind,
            this.menuEditReplace,
            this.menuEditFindAll});
            this.menuEdit.Name = "menuEdit";
            this.menuEdit.Size = new System.Drawing.Size(57, 20);
            this.menuEdit.Text = "编辑(&E)";
            this.menuEdit.DropDownOpening += new System.EventHandler(this.menuEdit_DropDownOpening);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(202, 6);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(202, 6);
            // 
            // menuEditSelectAll
            // 
            this.menuEditSelectAll.Name = "menuEditSelectAll";
            this.menuEditSelectAll.ShortcutKeyDisplayString = "Ctrl+A";
            this.menuEditSelectAll.Size = new System.Drawing.Size(205, 22);
            this.menuEditSelectAll.Text = "全选(&A)";
            this.menuEditSelectAll.Click += new System.EventHandler(this.menuEditSelectAll_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(202, 6);
            // 
            // menuEditReplace
            // 
            this.menuEditReplace.Name = "menuEditReplace";
            this.menuEditReplace.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.menuEditReplace.Size = new System.Drawing.Size(205, 22);
            this.menuEditReplace.Text = "替换(&R)";
            this.menuEditReplace.Click += new System.EventHandler(this.menuEditReplace_Click);
            // 
            // menuEditFindAll
            // 
            this.menuEditFindAll.Name = "menuEditFindAll";
            this.menuEditFindAll.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.F)));
            this.menuEditFindAll.Size = new System.Drawing.Size(205, 22);
            this.menuEditFindAll.Text = "查找全部(&I)";
            this.menuEditFindAll.Click += new System.EventHandler(this.menuEditFindAll_Click);
            // 
            // menuView
            // 
            this.menuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuViewScriptList,
            this.menuViewLog});
            this.menuView.Name = "menuView";
            this.menuView.Size = new System.Drawing.Size(58, 20);
            this.menuView.Text = "视图(&V)";
            // 
            // menuViewScriptList
            // 
            this.menuViewScriptList.Name = "menuViewScriptList";
            this.menuViewScriptList.ShortcutKeyDisplayString = "";
            this.menuViewScriptList.Size = new System.Drawing.Size(136, 22);
            this.menuViewScriptList.Text = "脚本列表(&S)";
            this.menuViewScriptList.Click += new System.EventHandler(this.menuViewScriptList_Click);
            // 
            // menuViewLog
            // 
            this.menuViewLog.Name = "menuViewLog";
            this.menuViewLog.ShortcutKeyDisplayString = "";
            this.menuViewLog.Size = new System.Drawing.Size(136, 22);
            this.menuViewLog.Text = "日志(&L)";
            this.menuViewLog.Click += new System.EventHandler(this.menuViewLog_Click);
            // 
            // menuDebug
            // 
            this.menuDebug.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuDebugGame,
            this.menuRunGame});
            this.menuDebug.Name = "menuDebug";
            this.menuDebug.Size = new System.Drawing.Size(59, 20);
            this.menuDebug.Text = "调试(&D)";
            // 
            // menuRunGame
            // 
            this.menuRunGame.Name = "menuRunGame";
            this.menuRunGame.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F5)));
            this.menuRunGame.Size = new System.Drawing.Size(229, 22);
            this.menuRunGame.Text = "开始执行(不调试)(&H)";
            this.menuRunGame.Click += new System.EventHandler(this.menuRunGame_Click);
            // 
            // toolStripMain
            // 
            this.toolStripMain.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripMain.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolSaveAll,
            this.toolStripSeparator5,
            this.toolRun});
            this.toolStripMain.Location = new System.Drawing.Point(289, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(64, 25);
            this.toolStripMain.TabIndex = 7;
            this.toolStripMain.Text = "toolStrip1";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripPanelBottom
            // 
            this.toolStripPanelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripPanelBottom.Location = new System.Drawing.Point(0, 504);
            this.toolStripPanelBottom.Name = "toolStripPanelBottom";
            this.toolStripPanelBottom.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.toolStripPanelBottom.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.toolStripPanelBottom.Size = new System.Drawing.Size(803, 0);
            // 
            // statusStrip
            // 
            this.statusStrip.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusPendingChanges});
            this.statusStrip.Location = new System.Drawing.Point(0, 482);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.statusStrip.Size = new System.Drawing.Size(803, 22);
            this.statusStrip.TabIndex = 5;
            this.statusStrip.Text = "statusStrip1";
            // 
            // statusPendingChanges
            // 
            this.statusPendingChanges.IsLink = true;
            this.statusPendingChanges.Name = "statusPendingChanges";
            this.statusPendingChanges.Size = new System.Drawing.Size(91, 17);
            this.statusPendingChanges.Text = "尚未保存的更改";
            this.statusPendingChanges.Click += new System.EventHandler(this.statusPendingChanges_Click);
            // 
            // toolStripPanelLeft
            // 
            this.toolStripPanelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStripPanelLeft.Location = new System.Drawing.Point(0, 25);
            this.toolStripPanelLeft.Name = "toolStripPanelLeft";
            this.toolStripPanelLeft.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.toolStripPanelLeft.RowMargin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.toolStripPanelLeft.Size = new System.Drawing.Size(0, 457);
            // 
            // toolStripPanelRight
            // 
            this.toolStripPanelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolStripPanelRight.Location = new System.Drawing.Point(803, 25);
            this.toolStripPanelRight.Name = "toolStripPanelRight";
            this.toolStripPanelRight.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.toolStripPanelRight.RowMargin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.toolStripPanelRight.Size = new System.Drawing.Size(0, 457);
            // 
            // menuSave
            // 
            this.menuSave.Image = global::NekoKun.Properties.Resources.SaveAll;
            this.menuSave.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.menuSave.Name = "menuSave";
            this.menuSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuSave.Size = new System.Drawing.Size(176, 22);
            this.menuSave.Text = "保存工程(&S)";
            this.menuSave.Click += new System.EventHandler(this.menuSave_Click);
            // 
            // menuExit
            // 
            this.menuExit.Image = global::NekoKun.Properties.Resources.ClosePreviewHS;
            this.menuExit.ImageTransparentColor = System.Drawing.Color.Black;
            this.menuExit.Name = "menuExit";
            this.menuExit.ShortcutKeyDisplayString = "Alt+F4";
            this.menuExit.Size = new System.Drawing.Size(176, 22);
            this.menuExit.Text = "退出(&X)";
            this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // menuEditUndo
            // 
            this.menuEditUndo.Image = global::NekoKun.Properties.Resources.Edit_Undo;
            this.menuEditUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuEditUndo.Name = "menuEditUndo";
            this.menuEditUndo.ShortcutKeyDisplayString = "Ctrl+Z";
            this.menuEditUndo.Size = new System.Drawing.Size(205, 22);
            this.menuEditUndo.Text = "撤销(&U)";
            this.menuEditUndo.Click += new System.EventHandler(this.menuEditUndo_Click);
            // 
            // menuEditRedo
            // 
            this.menuEditRedo.Image = global::NekoKun.Properties.Resources.Edit_Redo;
            this.menuEditRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuEditRedo.Name = "menuEditRedo";
            this.menuEditRedo.ShortcutKeyDisplayString = "Ctrl+Y";
            this.menuEditRedo.Size = new System.Drawing.Size(205, 22);
            this.menuEditRedo.Text = "重做(&R)";
            this.menuEditRedo.Click += new System.EventHandler(this.menuEditRedo_Click);
            // 
            // menuEditCut
            // 
            this.menuEditCut.Image = global::NekoKun.Properties.Resources.Cut;
            this.menuEditCut.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.menuEditCut.Name = "menuEditCut";
            this.menuEditCut.ShortcutKeyDisplayString = "Ctrl+X";
            this.menuEditCut.Size = new System.Drawing.Size(205, 22);
            this.menuEditCut.Text = "剪切(&T)";
            this.menuEditCut.Click += new System.EventHandler(this.menuEditCut_Click);
            // 
            // menuEditCopy
            // 
            this.menuEditCopy.Image = global::NekoKun.Properties.Resources.Copy;
            this.menuEditCopy.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.menuEditCopy.Name = "menuEditCopy";
            this.menuEditCopy.ShortcutKeyDisplayString = "Ctrl+C";
            this.menuEditCopy.Size = new System.Drawing.Size(205, 22);
            this.menuEditCopy.Text = "复制(&C)";
            this.menuEditCopy.Click += new System.EventHandler(this.menuEditCopy_Click);
            // 
            // menuEditPaste
            // 
            this.menuEditPaste.Image = global::NekoKun.Properties.Resources.Paste;
            this.menuEditPaste.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.menuEditPaste.Name = "menuEditPaste";
            this.menuEditPaste.ShortcutKeyDisplayString = "Ctrl+V";
            this.menuEditPaste.Size = new System.Drawing.Size(205, 22);
            this.menuEditPaste.Text = "粘贴(&P)";
            this.menuEditPaste.Click += new System.EventHandler(this.menuEditPaste_Click);
            // 
            // menuEditDelete
            // 
            this.menuEditDelete.Image = global::NekoKun.Properties.Resources.Delete;
            this.menuEditDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuEditDelete.Name = "menuEditDelete";
            this.menuEditDelete.ShortcutKeyDisplayString = "Delete";
            this.menuEditDelete.Size = new System.Drawing.Size(205, 22);
            this.menuEditDelete.Text = "删除(&D)";
            this.menuEditDelete.Click += new System.EventHandler(this.menuEditDelete_Click);
            // 
            // menuEditFind
            // 
            this.menuEditFind.Image = global::NekoKun.Properties.Resources.Find;
            this.menuEditFind.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuEditFind.Name = "menuEditFind";
            this.menuEditFind.ShortcutKeyDisplayString = "";
            this.menuEditFind.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.menuEditFind.Size = new System.Drawing.Size(205, 22);
            this.menuEditFind.Text = "查找(&F)";
            this.menuEditFind.Click += new System.EventHandler(this.menuEditFind_Click);
            // 
            // menuDebugGame
            // 
            this.menuDebugGame.Image = global::NekoKun.Properties.Resources.Play;
            this.menuDebugGame.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            this.menuDebugGame.Name = "menuDebugGame";
            this.menuDebugGame.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.menuDebugGame.Size = new System.Drawing.Size(229, 22);
            this.menuDebugGame.Text = "启动调试(&S)";
            this.menuDebugGame.Click += new System.EventHandler(this.menuDebugGame_Click);
            // 
            // toolSaveAll
            // 
            this.toolSaveAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolSaveAll.Image = global::NekoKun.Properties.Resources.SaveAll;
            this.toolSaveAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolSaveAll.Name = "toolSaveAll";
            this.toolSaveAll.Size = new System.Drawing.Size(23, 22);
            this.toolSaveAll.Text = "保存工程(Ctrl+S)";
            this.toolSaveAll.Click += new System.EventHandler(this.menuSave_Click);
            // 
            // toolRun
            // 
            this.toolRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolRun.Image = global::NekoKun.Properties.Resources.Play;
            this.toolRun.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolRun.Name = "toolRun";
            this.toolRun.Size = new System.Drawing.Size(23, 22);
            this.toolRun.Text = "启动调试(F5)";
            this.toolRun.Click += new System.EventHandler(this.menuDebugGame_Click);
            // 
            // Workbench
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 504);
            this.Controls.Add(this.toolStripPanelRight);
            this.Controls.Add(this.toolStripPanelLeft);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStripPanelBottom);
            this.Controls.Add(this.toolStripPanelTop);
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.Name = "Workbench";
            this.Text = "NekoKun";
            this.toolStripPanelTop.ResumeLayout(false);
            this.toolStripPanelTop.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripPanel toolStripPanelTop;
        private System.Windows.Forms.ToolStripPanel toolStripPanelBottom;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripStatusLabel statusPendingChanges;
        private System.Windows.Forms.ToolStripPanel toolStripPanelLeft;
        private System.Windows.Forms.ToolStripPanel toolStripPanelRight;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuExit;
        private System.Windows.Forms.ToolStripMenuItem menuEdit;
        private System.Windows.Forms.ToolStripMenuItem menuEditUndo;
        private System.Windows.Forms.ToolStripMenuItem menuEditRedo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuEditCut;
        private System.Windows.Forms.ToolStripMenuItem menuEditCopy;
        private System.Windows.Forms.ToolStripMenuItem menuEditPaste;
        private System.Windows.Forms.ToolStripMenuItem menuEditDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuEditSelectAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem menuEditFind;
        private System.Windows.Forms.ToolStripMenuItem menuEditReplace;
        private System.Windows.Forms.ToolStripMenuItem menuView;
        private System.Windows.Forms.ToolStripMenuItem menuViewScriptList;
        private System.Windows.Forms.ToolStripMenuItem menuViewLog;
        private System.Windows.Forms.ToolStripMenuItem menuDebug;
        private System.Windows.Forms.ToolStripMenuItem menuDebugGame;
        private System.Windows.Forms.ToolStripMenuItem menuRunGame;
        private System.Windows.Forms.ToolStripButton toolSaveAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton toolRun;
        private System.Windows.Forms.ToolStripMenuItem menuEditFindAll;
    }
}

