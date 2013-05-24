using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NekoKun.Core
{
    public partial class UnhandledExceptionDialog : NekoKun.UI.LynnForm
    {
        public Exception e;
        public UnhandledExceptionDialog(Exception e)
        {
            this.e = e;

            InitializeComponent();

            this.Text = "Unhandled Exception occured in NekoKun";
            this.StartPosition = FormStartPosition.CenterParent;
            this.txtSummary.Font = UI.UIManager.GetMonospaceFont();
            this.txtSummary.Text = ExceptionHelper.ExceptionMessage(e).Replace("\r", "").Replace("\n", System.Environment.NewLine);
            this.txtSummary.ReadOnly = true;

            AddException(e);
            try
            {
                this.treeObjects.SelectedNode = this.treeObjects.Nodes[0];
            }
            catch { }
            this.treeObjects.MouseClick += new MouseEventHandler(treeObjects_SelectedItemChanged);
            this.treeObjects.KeyPress += new KeyPressEventHandler(treeObjects_SelectedItemChanged);
        }

        void treeObjects_SelectedItemChanged(object sender, EventArgs e)
        {
            this.listObjects.SelectedObject = this.treeObjects.SelectedNode.Tag;
        }

        public void AddException(object obj)
        {
            AddException(obj, this.treeObjects.Nodes);
        }

        public void AddException(object obj, TreeNodeCollection parent)
        {
            TreeNode node = parent.Add(obj.GetType().FullName);
            node.Tag = obj;

            if (obj is Exception)
            {
                Exception e = (Exception)obj;
                if (e.InnerException != null)
                    AddException(e.InnerException, node.Nodes);
            }
        }
    }
}
