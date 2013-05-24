using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using WeifenLuo.WinFormsUI.Docking;

namespace NekoKun.Core
{
    [PadDefaultLocation(WeifenLuo.WinFormsUI.Docking.DockState.DockBottom)]
    public class OutputPad : AbstractPad, IClipboardHandler, IUndoHandler, IDeleteHandler, ISelectAllHandler
    {
        #region Static Members
        private static List<IOutputProvider> providers = new List<IOutputProvider>();

        public static event EventHandler ProviderAttached;
        private static void OnProviderAttached(IOutputProvider sender)
        {
            if (ProviderAttached != null)
                ProviderAttached(sender, EventArgs.Empty);
        }
        public static void Attach(IOutputProvider output)
        {
            providers.Add(output);
            OnProviderAttached(output);
        }

        public static event EventHandler ProviderDetached;
        private static void OnProviderDetatched(IOutputProvider sender)
        {
            if (ProviderDetached != null)
                ProviderDetached(sender, EventArgs.Empty);
        }
        public static void Detach(IOutputProvider output)
        {
            if (providers.Contains(output))
            {
                providers.Remove(output);
                OnProviderDetatched(output);
            }
        }

        public static IOutputProvider[] Providers
        {
            get
            {
                return providers.ToArray();
            }
        }
        #endregion

        protected ToolStrip Toolbar = new ToolStrip();
        protected ToolStripComboBox ItemBox = new ToolStripComboBox();
        protected Panel ViewPanel = new Panel();
        protected IOutputProvider CurrentProvider;
        protected Control CurrentControl;

        public OutputPad()
        {
            var iconO = global::NekoKun.UI.Properties.Resources.OutputWindow.Clone() as Bitmap;
            iconO.MakeTransparent(System.Drawing.Color.Fuchsia);
            this.Icon = Icon.FromHandle(iconO.GetHicon());

            ViewPanel.Dock = DockStyle.Fill;

            ItemBox.AutoSize = false;
            ItemBox.ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ItemBox.Width = 300;
            ItemBox.SelectedIndexChanged += new EventHandler(ItemBox_SelectedIndexChanged);
            ItemBox.Items.AddRange(Providers);
            ItemBox.SelectedIndex = this.ItemBox.Items.Count - 1;
            OutputPad.ProviderAttached += new EventHandler(OutputPad_ProviderAttached);
            OutputPad.ProviderDetached += new EventHandler(OutputPad_ProviderDetached);

            Toolbar.Items.Add(new ToolStripLabel("显示输出来源(&S):"));
            Toolbar.Items.Add(ItemBox);
            Toolbar.GripStyle = ToolStripGripStyle.Hidden;
            Toolbar.Dock = DockStyle.Top;

            this.Text = "输出";
            this.Controls.Add(ViewPanel);
            this.Controls.Add(Toolbar);
        }

        protected override void Dispose(bool disposing)
        {
            OutputPad.ProviderAttached -= new EventHandler(OutputPad_ProviderAttached);
            OutputPad.ProviderDetached -= new EventHandler(OutputPad_ProviderDetached);
            base.Dispose(disposing);
        }

        void OutputPad_ProviderDetached(object sender, EventArgs e)
        {
            IOutputProvider p = sender as IOutputProvider;
            if (p != null)
            {
                this.ItemBox.Items.Remove(p);
                if (CurrentProvider == p)
                {
                    this.ItemBox.SelectedItem = this.ItemBox.Items.Count > 0 ? this.ItemBox.Items[this.ItemBox.Items.Count - 1] : null;
                }
            }
        }

        void OutputPad_ProviderAttached(object sender, EventArgs e)
        {
            IOutputProvider p = sender as IOutputProvider;
            if (p != null)
            {
                this.ItemBox.Items.Add(p);
                this.ItemBox.SelectedIndex = this.ItemBox.Items.Count - 1;
            }
        }

        void ItemBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrentControl != null)
            {
                ViewPanel.Controls.Remove(CurrentControl);
                CurrentControl = null;
            }

            if (CurrentProvider != null)
            {
                CurrentProvider = null;
            }

            if (this.ItemBox.SelectedItem != null)
            {
                CurrentProvider = this.ItemBox.SelectedItem as IOutputProvider;
                CurrentControl = CurrentProvider.OutputViewContent;

                CurrentControl.Dock = DockStyle.Fill;
                ViewPanel.Controls.Add(CurrentControl);
            }
        }

        #region IClipboardHandler, IUndoHandler, IDeleteHandler, ISelectAllHandler 成员
        public bool CanCut
        {
            get { 
                var i = CurrentControl as IClipboardHandler;
                return i == null ? i.CanCut : false;
            }
        }

        public bool CanCopy
        {
            get
            {
                var i = CurrentControl as IClipboardHandler;
                return i == null ? i.CanCopy : false;
            }
        }

        public bool CanPaste
        {
            get
            {
                var i = CurrentControl as IClipboardHandler;
                return i == null ? i.CanPaste : false;
            }
        }

        public void Cut()
        {
            var i = CurrentControl as IClipboardHandler;
            if (i == null) 
                i.Cut();
        }

        public void Copy()
        {
            var i = CurrentControl as IClipboardHandler;
            if (i == null)
                i.Copy();
        }

        public void Paste()
        {
            var i = CurrentControl as IClipboardHandler;
            if (i == null)
                i.Paste();
        }

        public bool CanUndo
        {
            get
            {
                var i = CurrentControl as IUndoHandler;
                return i == null ? i.CanUndo : false;
            }
        }

        public bool CanRedo
        {
            get
            {
                var i = CurrentControl as IUndoHandler;
                return i == null ? i.CanRedo : false;
            }
        }

        public void Undo()
        {
            var i = CurrentControl as IUndoHandler;
            if (i == null)
                i.Undo();
        }

        public void Redo()
        {
            var i = CurrentControl as IUndoHandler;
            if (i == null)
                i.Redo();
        }

        public bool CanDelete
        {
            get
            {
                var i = CurrentControl as IDeleteHandler;
                return i == null ? i.CanDelete : false;
            }
        }

        public void Delete()
        {
            var i = CurrentControl as IDeleteHandler;
            if (i == null)
                i.Delete();
        }

        public bool CanSelectAll
        {
            get
            {
                var i = CurrentControl as ISelectAllHandler;
                return i == null ? i.CanSelectAll : false;
            }
        }

        public void SelectAll()
        {
            var i = CurrentControl as ISelectAllHandler;
            if (i == null)
                i.SelectAll();
        }

        public bool CanShowFindDialog
        {
            get
            {
                var i = CurrentControl as IFindReplaceHandler;
                return i == null ? i.CanShowFindDialog : false;
            }
        }

        public bool CanShowReplaceDialog
        {
            get
            {
                var i = CurrentControl as IFindReplaceHandler;
                return i == null ? i.CanShowReplaceDialog : false;
            }
        }

        public void ShowFindDialog()
        {
            var i = CurrentControl as IFindReplaceHandler;
            if (i == null)
                i.ShowFindDialog();
        }

        public void ShowReplaceDialog()
        {
            var i = CurrentControl as IFindReplaceHandler;
            if (i == null)
                i.ShowReplaceDialog();
        }
        #endregion
    }
}
