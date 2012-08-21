using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.UI
{
    public class OwnerPaintPanel : System.Windows.Forms.Panel
    {
        public new event System.Windows.Forms.PaintEventHandler Paint;
        public new event System.Windows.Forms.MouseEventHandler MouseUp;
        public new event System.Windows.Forms.MouseEventHandler MouseDown;
        public new event System.Windows.Forms.MouseEventHandler MouseMove;
        public new event System.Windows.Forms.MouseEventHandler MouseClick;
        public new event System.Windows.Forms.MouseEventHandler MouseDoubleClick;
        public new event System.Windows.Forms.MouseEventHandler MouseWheel;
        public new event EventHandler MouseHover;
        public new event EventHandler MouseLeave;
        public new event EventHandler MouseEnter;

        protected System.Drawing.Size contentSize = System.Drawing.Size.Empty;

        System.Windows.Forms.VScrollBar barV;
        System.Windows.Forms.HScrollBar barH;
        System.Windows.Forms.Panel panel;

        public OwnerPaintPanel()
        {
            this.contentSize = new System.Drawing.Size();

            this.panel = new DoubleBufferedPanel();
            this.panel.Paint += new System.Windows.Forms.PaintEventHandler(panel_Paint);
            this.Controls.Add(this.panel);

            this.barV = new System.Windows.Forms.VScrollBar();
            this.Controls.Add(this.barV);
            this.barH = new System.Windows.Forms.HScrollBar();
            this.Controls.Add(this.barH);

            this.Layout += MapEditor_Resize;
            this.barH.Scroll += new System.Windows.Forms.ScrollEventHandler(bar_Scroll);
            this.barV.Scroll += new System.Windows.Forms.ScrollEventHandler(bar_Scroll);

            base.MouseWheel += new System.Windows.Forms.MouseEventHandler(barV_MouseWheel);
            this.panel.MouseWheel += new System.Windows.Forms.MouseEventHandler(barV_MouseWheel);
            this.barV.MouseWheel += new System.Windows.Forms.MouseEventHandler(barV_MouseWheel);
            this.barH.MouseWheel += new System.Windows.Forms.MouseEventHandler(barH_MouseWheel);

            this.panel.MouseUp+= new System.Windows.Forms.MouseEventHandler(panel_MouseUp);
            this.panel.MouseDown += new System.Windows.Forms.MouseEventHandler(panel_MouseDown);
            this.panel.MouseMove += new System.Windows.Forms.MouseEventHandler(panel_MouseMove);
            this.panel.MouseClick += new System.Windows.Forms.MouseEventHandler(panel_MouseClick);
            this.panel.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(panel_MouseDoubleClick);
            this.panel.MouseWheel += new System.Windows.Forms.MouseEventHandler(panel_MouseWheel);
            this.panel.MouseHover += new EventHandler(panel_MouseHover);
            this.panel.MouseLeave += new EventHandler(panel_MouseLeave);
            this.panel.MouseEnter += new EventHandler(panel_MouseEnter);

            UpdateScrollbars();
        }

        void panel_MouseEnter(object sender, EventArgs e)
        {
            if (this.MouseEnter != null)
                this.MouseEnter(sender, e);
        }

        void panel_MouseLeave(object sender, EventArgs e)
        {
            if (this.MouseLeave != null)
                this.MouseLeave(sender, e);
        }

        void panel_MouseHover(object sender, EventArgs e)
        {
            if (this.MouseHover != null)
                this.MouseHover(sender, e);
        }

        void panel_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.MouseWheel != null)
                this.MouseWheel(sender, new System.Windows.Forms.MouseEventArgs(e.Button, e.Clicks, this.ScrollOffsetX + e.X, this.ScrollOffsetY + e.Y, e.Delta));
        }

        void panel_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.MouseDoubleClick != null)
                this.MouseDoubleClick(sender, new System.Windows.Forms.MouseEventArgs(e.Button, e.Clicks, this.ScrollOffsetX + e.X, this.ScrollOffsetY + e.Y, e.Delta));
        }

        void panel_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.MouseClick != null)
                this.MouseClick(sender, new System.Windows.Forms.MouseEventArgs(e.Button, e.Clicks, this.ScrollOffsetX + e.X, this.ScrollOffsetY + e.Y, e.Delta));
        }

        void panel_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.MouseMove != null)
                this.MouseMove(sender, new System.Windows.Forms.MouseEventArgs(e.Button, e.Clicks, this.ScrollOffsetX + e.X, this.ScrollOffsetY + e.Y, e.Delta));
        }

        void panel_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.Focus();

            if (this.MouseDown != null)
                this.MouseDown(sender, new System.Windows.Forms.MouseEventArgs(e.Button, e.Clicks, this.ScrollOffsetX + e.X, this.ScrollOffsetY + e.Y, e.Delta));
        }

        void panel_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.MouseUp != null)
                this.MouseUp(sender, new System.Windows.Forms.MouseEventArgs(e.Button, e.Clicks, this.ScrollOffsetX + e.X, this.ScrollOffsetY + e.Y, e.Delta));
        }


        void barH_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.barH.Visible == false)
                return;

            int newValue = this.barH.Value - e.Delta;
            if (newValue < 0)
                newValue = 0;

            if (newValue > this.barH.Maximum - this.barH.LargeChange)
                newValue = this.barH.Maximum - this.barH.LargeChange;

            if (this.barH.Value != newValue)
            {
                this.barH.Value = newValue;
                this.panel.Invalidate();
            }
        }

        void barV_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.barV.Visible == false)
                return;

            int newValue = this.barV.Value - e.Delta;
            if (newValue < 0)
                newValue = 0;

            if (newValue > this.barV.Maximum - this.barV.LargeChange)
                newValue = this.barV.Maximum - this.barV.LargeChange;

            if (this.barV.Value != newValue)
            {
                this.barV.Value = newValue;
                this.panel.Invalidate();
            }
        }

        public System.Drawing.Size ContentSize
        {
            get { return this.contentSize; }
            set {
                this.contentSize = value;
                UpdateScrollbars();
            }
        }

        public int ContentWidth
        {
            get { return this.contentSize.Width; }
            set { this.contentSize.Width = value; UpdateScrollbars(); }
        }

        public int ContentHeight
        {
            get { return this.contentSize.Height; }
            set { this.contentSize.Height = value; UpdateScrollbars(); }
        }

        public int ScrollOffsetX
        {
            get { return this.barH.Value; }
        }

        public int ScrollOffsetY
        {
            get { return this.barV.Value; }
        }

        void panel_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (this.Paint != null)
            {
                this.Paint(sender, e);
            }
        }

        void bar_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
        {
            this.panel.Invalidate();
        }

        void MapEditor_Resize(object sender, EventArgs e)
        {
            for (int i = 0; i < 2; i++)
            {
                if (this.barV.Visible)
                    this.panel.Width = this.ClientSize.Width - this.barV.Width;
                else
                    this.panel.Width = this.ClientSize.Width;

                if (this.barH.Visible)
                    this.panel.Height = this.ClientSize.Height - this.barH.Height;
                else
                    this.panel.Height = this.ClientSize.Height;

                UpdateScrollbars();
            }

            this.barV.Left = this.panel.Width;
            this.barV.Top = 0;
            this.barV.Height = this.panel.Height;

            this.barH.Left = 0;
            this.barH.Top = this.panel.Height;
            this.barH.Width = this.panel.Width;

            this.panel.Invalidate();
        }

        private void UpdateScrollbars()
        {
            int newH = Math.Max(this.contentSize.Width, 0);
            int newV = Math.Max(this.contentSize.Height, 0);
            int test;
            this.barH.Enabled = true;
            this.barH.SmallChange = 128;
            if (this.barH.Value >= (test = Math.Max(newH - this.barH.LargeChange, 0)))
                this.barH.Value = test;
            this.barH.LargeChange = this.panel.Width;
            this.barH.Minimum = 0;
            this.barH.Maximum = newH;
            this.barH.Visible = this.barH.Maximum > this.barH.LargeChange;

            this.barV.Enabled = true;
            this.barV.SmallChange = 128;
            if (this.barV.Value >= (test = Math.Max(newV - this.barV.LargeChange, 0)))
                this.barV.Value = test;
            this.barV.LargeChange = this.panel.Height;
            this.barV.Minimum = 0;
            this.barV.Maximum = newV;
            this.barV.Visible = this.barV.Maximum > this.barV.LargeChange;
        }


        internal class DoubleBufferedPanel : System.Windows.Forms.Panel
        {
            public DoubleBufferedPanel()
            {
                this.DoubleBuffered = true;
                this.SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, true);
                this.SetStyle(System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer, true);
                this.SetStyle(System.Windows.Forms.ControlStyles.ResizeRedraw, true);
                this.SetStyle(System.Windows.Forms.ControlStyles.UserPaint, true);
            }
        }
    }
}
