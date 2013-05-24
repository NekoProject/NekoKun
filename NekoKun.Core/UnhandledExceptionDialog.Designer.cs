namespace NekoKun.Core
{
    partial class UnhandledExceptionDialog
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
            this.txtSummary = new NekoKun.UI.LynnTextbox();
            this.tab = new NekoKun.UI.LynnTabControl();
            this.pageSummary = new System.Windows.Forms.TabPage();
            this.pageInspect = new System.Windows.Forms.TabPage();
            this.panelInspect = new System.Windows.Forms.SplitContainer();
            this.treeObjects = new NekoKun.UI.LynnTreeView();
            this.listObjects = new System.Windows.Forms.PropertyGrid();
            this.tab.SuspendLayout();
            this.pageSummary.SuspendLayout();
            this.pageInspect.SuspendLayout();
            this.panelInspect.Panel1.SuspendLayout();
            this.panelInspect.Panel2.SuspendLayout();
            this.panelInspect.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSummary
            // 
            this.txtSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSummary.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.txtSummary.ImeMode = System.Windows.Forms.ImeMode.On;
            this.txtSummary.Location = new System.Drawing.Point(3, 3);
            this.txtSummary.Multiline = true;
            this.txtSummary.Name = "txtSummary";
            this.txtSummary.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSummary.Size = new System.Drawing.Size(529, 317);
            this.txtSummary.TabIndex = 0;
            // 
            // tab
            // 
            this.tab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tab.Controls.Add(this.pageSummary);
            this.tab.Controls.Add(this.pageInspect);
            this.tab.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.tab.Location = new System.Drawing.Point(12, 12);
            this.tab.Name = "tab";
            this.tab.SelectedIndex = 0;
            this.tab.Size = new System.Drawing.Size(543, 349);
            this.tab.TabIndex = 1;
            // 
            // pageSummary
            // 
            this.pageSummary.BackColor = System.Drawing.Color.Transparent;
            this.pageSummary.Controls.Add(this.txtSummary);
            this.pageSummary.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.pageSummary.Location = new System.Drawing.Point(4, 22);
            this.pageSummary.Name = "pageSummary";
            this.pageSummary.Padding = new System.Windows.Forms.Padding(3);
            this.pageSummary.Size = new System.Drawing.Size(535, 323);
            this.pageSummary.TabIndex = 0;
            this.pageSummary.Text = "概况";
            // 
            // pageInspect
            // 
            this.pageInspect.BackColor = System.Drawing.Color.Transparent;
            this.pageInspect.Controls.Add(this.panelInspect);
            this.pageInspect.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.pageInspect.Location = new System.Drawing.Point(4, 22);
            this.pageInspect.Name = "pageInspect";
            this.pageInspect.Padding = new System.Windows.Forms.Padding(3);
            this.pageInspect.Size = new System.Drawing.Size(535, 323);
            this.pageInspect.TabIndex = 1;
            this.pageInspect.Text = "审查";
            // 
            // panelInspect
            // 
            this.panelInspect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelInspect.Location = new System.Drawing.Point(3, 3);
            this.panelInspect.Name = "panelInspect";
            // 
            // panelInspect.Panel1
            // 
            this.panelInspect.Panel1.Controls.Add(this.treeObjects);
            // 
            // panelInspect.Panel2
            // 
            this.panelInspect.Panel2.Controls.Add(this.listObjects);
            this.panelInspect.Size = new System.Drawing.Size(529, 317);
            this.panelInspect.SplitterDistance = 176;
            this.panelInspect.TabIndex = 0;
            // 
            // treeObjects
            // 
            this.treeObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeObjects.FullRowSelect = true;
            this.treeObjects.Location = new System.Drawing.Point(0, 0);
            this.treeObjects.Name = "treeObjects";
            this.treeObjects.ShowLines = false;
            this.treeObjects.Size = new System.Drawing.Size(176, 317);
            this.treeObjects.TabIndex = 0;
            // 
            // listObjects
            // 
            this.listObjects.CommandsVisibleIfAvailable = false;
            this.listObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listObjects.HelpVisible = false;
            this.listObjects.Location = new System.Drawing.Point(0, 0);
            this.listObjects.Name = "listObjects";
            this.listObjects.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.listObjects.Size = new System.Drawing.Size(349, 317);
            this.listObjects.TabIndex = 0;
            this.listObjects.ToolbarVisible = false;
            // 
            // UnhandledExceptionDialog
            // 
            this.ClientSize = new System.Drawing.Size(567, 373);
            this.Controls.Add(this.tab);
            this.Name = "UnhandledExceptionDialog";
            this.tab.ResumeLayout(false);
            this.pageSummary.ResumeLayout(false);
            this.pageSummary.PerformLayout();
            this.pageInspect.ResumeLayout(false);
            this.panelInspect.Panel1.ResumeLayout(false);
            this.panelInspect.Panel2.ResumeLayout(false);
            this.panelInspect.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private NekoKun.UI.LynnTextbox txtSummary;
        private NekoKun.UI.LynnTabControl tab;
        private System.Windows.Forms.TabPage pageSummary;
        private System.Windows.Forms.TabPage pageInspect;
        private System.Windows.Forms.SplitContainer panelInspect;
        private NekoKun.UI.LynnTreeView treeObjects;
        private System.Windows.Forms.PropertyGrid listObjects;
    }
}
