namespace NekoKun
{
    partial class WelcomePage
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
            this.welcome = new NekoKun.UI.LynnLabel();
            this.listbox = new NekoKun.UI.LynnListbox();
            this.SuspendLayout();
            // 
            // welcome
            // 
            this.welcome.AutoSize = true;
            this.welcome.Location = new System.Drawing.Point(12, 9);
            this.welcome.Name = "welcome";
            this.welcome.Size = new System.Drawing.Size(328, 18);
            this.welcome.TabIndex = 0;
            this.welcome.Text = "感谢您选择出类拔萃的 NekoKun，现在阁下想做些什么呢？";
            // 
            // listbox
            // 
            this.listbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listbox.Location = new System.Drawing.Point(13, 31);
            this.listbox.Name = "listbox";
            this.listbox.Size = new System.Drawing.Size(484, 215);
            this.listbox.TabIndex = 1;
            // 
            // WelcomePage
            // 
            this.ClientSize = new System.Drawing.Size(509, 258);
            this.Controls.Add(this.listbox);
            this.Controls.Add(this.welcome);
            this.MinimumSize = new System.Drawing.Size(441, 178);
            this.Name = "WelcomePage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "欢迎来到 NekoKun";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NekoKun.UI.LynnLabel welcome;
        private NekoKun.UI.LynnListbox listbox;
    }
}
