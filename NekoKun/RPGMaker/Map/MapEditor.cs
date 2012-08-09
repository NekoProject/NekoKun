using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public class MapEditor : AbstractEditor
    {
        MapFile map;
        TilesetInfo tileset;
        System.Windows.Forms.VScrollBar barV;
        System.Windows.Forms.HScrollBar barH;

        System.Windows.Forms.Panel panel;
        public MapEditor(MapFile file)
            : base(file)
        {
            map = file;
            this.tileset = map.TilesetFile[map.TilesetID];

            this.panel = new DoubleBufferedPanel();
            this.panel.Paint += new System.Windows.Forms.PaintEventHandler(box_Paint);
            this.Controls.Add(this.panel);

            this.barV = new System.Windows.Forms.VScrollBar();
            this.Controls.Add(this.barV);
            this.barH = new System.Windows.Forms.HScrollBar();
            this.Controls.Add(this.barH);

            this.ClientSizeChanged += new EventHandler(MapEditor_Resize);
            this.Layout += MapEditor_Resize;
            this.SizeChanged += new EventHandler(MapEditor_Resize);
            this.Resize += new EventHandler(MapEditor_Resize);
            this.barH.Scroll += new System.Windows.Forms.ScrollEventHandler(bar_Scroll);
            this.barV.Scroll += new System.Windows.Forms.ScrollEventHandler(bar_Scroll);

            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this_MouseWheel);
            UpdateScrollbars();
        }

        void this_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
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
            int newH = Math.Max(this.tileset.TileSize.Width * this.map.Size.Width, 0);
            int newV = Math.Max(this.tileset.TileSize.Height * this.map.Size.Height, 0);
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

        protected override void Dispose(bool disposing)
        {
            this.tileset.Dispose();
            base.Dispose(disposing);
        }

        void box_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            int left = this.barH.Value;
            int top = this.barV.Value;
            int x0 = Math.Max((left + e.ClipRectangle.Left) / this.tileset.TileSize.Width - 1, 0);
            int x1 = Math.Min((left + e.ClipRectangle.Right) / this.tileset.TileSize.Width + 1, map.Size.Width);
            int y0 = Math.Max((top + e.ClipRectangle.Top) / this.tileset.TileSize.Height - 1, 0);
            int y1 = Math.Min((top + e.ClipRectangle.Bottom) / this.tileset.TileSize.Height + 1, map.Size.Height);
            System.Drawing.Rectangle dest = new System.Drawing.Rectangle();
            dest.Size = this.tileset.TileSize;
            for (int x = x0; x < x1; x++)
            {
                for (int y = y0; y < y1; y++)
                {
                    dest.X = x * this.tileset.TileSize.Width - left;
                    dest.Y = y * this.tileset.TileSize.Height - top;
                    e.Graphics.SetClip(dest);
                    e.Graphics.Clear(System.Drawing.Color.SteelBlue);
                    for (int layer = 0; layer < 3; layer++)
                    {
                        e.Graphics.DrawImage(
                            this.tileset[this.map.data[x, y, layer]],
                            dest.Location
                        );
                    }
                }
            }
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
