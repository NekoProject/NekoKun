namespace NekoKun
{
    partial class CreateProjectWizard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.bigPanel = new System.Windows.Forms.SplitContainer();
            this.fieldName = new NekoKun.UI.LynnTextbox();
            this.cueName = new NekoKun.UI.LynnLabel();
            this.cueLocation = new NekoKun.UI.LynnLabel();
            this.fieldLocation = new NekoKun.UI.LynnTextbox();
            this.browseFolderButton = new NekoKun.UI.LynnButton();
            this.templateCategory = new System.Windows.Forms.TreeView();
            this.templateList = new System.Windows.Forms.ListView();
            this.accpetButton = new NekoKun.UI.LynnButton();
            this.cancelButton = new NekoKun.UI.LynnButton();
            this.bigPanel.Panel1.SuspendLayout();
            this.bigPanel.Panel2.SuspendLayout();
            this.bigPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // bigPanel
            // 
            this.bigPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.bigPanel.Location = new System.Drawing.Point(12, 12);
            this.bigPanel.Name = "bigPanel";
            // 
            // bigPanel.Panel1
            // 
            this.bigPanel.Panel1.Controls.Add(this.templateCategory);
            // 
            // bigPanel.Panel2
            // 
            this.bigPanel.Panel2.Controls.Add(this.templateList);
            this.bigPanel.Size = new System.Drawing.Size(500, 244);
            this.bigPanel.SplitterDistance = 166;
            this.bigPanel.TabIndex = 0;
            // 
            // fieldName
            // 
            this.fieldName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fieldName.Location = new System.Drawing.Point(74, 262);
            this.fieldName.Name = "fieldName";
            this.fieldName.Size = new System.Drawing.Size(438, 25);
            this.fieldName.TabIndex = 1;
            // 
            // cueName
            // 
            this.cueName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cueName.AutoSize = true;
            this.cueName.Location = new System.Drawing.Point(12, 265);
            this.cueName.Name = "cueName";
            this.cueName.Size = new System.Drawing.Size(56, 18);
            this.cueName.TabIndex = 2;
            this.cueName.Text = "名称(&N):";
            // 
            // cueLocation
            // 
            this.cueLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cueLocation.AutoSize = true;
            this.cueLocation.Location = new System.Drawing.Point(12, 296);
            this.cueLocation.Name = "cueLocation";
            this.cueLocation.Size = new System.Drawing.Size(54, 18);
            this.cueLocation.TabIndex = 3;
            this.cueLocation.Text = "位置(&L):";
            // 
            // fieldLocation
            // 
            this.fieldLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fieldLocation.Location = new System.Drawing.Point(74, 293);
            this.fieldLocation.Name = "fieldLocation";
            this.fieldLocation.Size = new System.Drawing.Size(357, 25);
            this.fieldLocation.TabIndex = 4;
            // 
            // browseFolderButton
            // 
            this.browseFolderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.browseFolderButton.Location = new System.Drawing.Point(437, 293);
            this.browseFolderButton.Name = "browseFolderButton";
            this.browseFolderButton.Size = new System.Drawing.Size(75, 26);
            this.browseFolderButton.TabIndex = 5;
            this.browseFolderButton.Text = "浏览(&B)...";
            // 
            // templateCategory
            // 
            this.templateCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.templateCategory.Location = new System.Drawing.Point(0, 0);
            this.templateCategory.Name = "templateCategory";
            this.templateCategory.Size = new System.Drawing.Size(166, 244);
            this.templateCategory.TabIndex = 0;
            // 
            // templateList
            // 
            this.templateList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.templateList.Location = new System.Drawing.Point(0, 0);
            this.templateList.Name = "templateList";
            this.templateList.Size = new System.Drawing.Size(330, 244);
            this.templateList.TabIndex = 0;
            this.templateList.UseCompatibleStateImageBehavior = false;
            // 
            // accpetButton
            // 
            this.accpetButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.accpetButton.Location = new System.Drawing.Point(356, 325);
            this.accpetButton.Name = "accpetButton";
            this.accpetButton.Size = new System.Drawing.Size(75, 23);
            this.accpetButton.TabIndex = 6;
            this.accpetButton.Text = "确定";
            this.accpetButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(437, 325);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "取消";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // CreateProjectWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 359);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.accpetButton);
            this.Controls.Add(this.browseFolderButton);
            this.Controls.Add(this.fieldLocation);
            this.Controls.Add(this.cueLocation);
            this.Controls.Add(this.cueName);
            this.Controls.Add(this.fieldName);
            this.Controls.Add(this.bigPanel);
            this.AcceptButton = this.accpetButton;
            this.CancelButton = this.cancelButton;
            this.MinimumSize = new System.Drawing.Size(300, 200);
            this.Name = "CreateProjectWizard";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "新建工程";
            this.bigPanel.Panel1.ResumeLayout(false);
            this.bigPanel.Panel2.ResumeLayout(false);
            this.bigPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer bigPanel;
        private NekoKun.UI.LynnTextbox fieldName;
        private System.Windows.Forms.TreeView templateCategory;
        private System.Windows.Forms.ListView templateList;
        private NekoKun.UI.LynnLabel cueName;
        private NekoKun.UI.LynnLabel cueLocation;
        private NekoKun.UI.LynnTextbox fieldLocation;
        private NekoKun.UI.LynnButton browseFolderButton;
        private NekoKun.UI.LynnButton accpetButton;
        private NekoKun.UI.LynnButton cancelButton;

    }
}