using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public class MapEditor : AbstractEditor
    {
        MapFile map;
        TilesetInfo tileset;
        System.Windows.Forms.Panel panel;
        public MapEditor(MapFile file)
            : base(file)
        {
            map = file;
            this.tileset = map.TilesetFile[map.TilesetID];

            this.panel = new DoubleBufferedPanel();
            this.panel.AutoScroll = false;
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Paint += new System.Windows.Forms.PaintEventHandler(box_Paint);
            this.panel.Scroll += new System.Windows.Forms.ScrollEventHandler(panel_Scroll);
            this.Controls.Add(this.panel);
            this.Resize += new EventHandler(MapEditor_Resize);
            this.panel.Resize += new EventHandler(panel_Resize);
            UpdateScrollbars();
        }

        void panel_Resize(object sender, EventArgs e)
        {
            UpdateScrollbars();
        }

        void MapEditor_Resize(object sender, EventArgs e)
        {
            UpdateScrollbars();
        }

        void panel_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
        {
            if (e.ScrollOrientation == System.Windows.Forms.ScrollOrientation.HorizontalScroll)
                this.panel.HorizontalScroll.Value = e.NewValue;
            if (e.ScrollOrientation == System.Windows.Forms.ScrollOrientation.VerticalScroll)
                this.panel.VerticalScroll.Value = e.NewValue;
        }

        private void UpdateScrollbars()
        {
            this.panel.HorizontalScroll.Enabled = true;
            this.panel.HorizontalScroll.SmallChange = 128;
            this.panel.HorizontalScroll.LargeChange = this.panel.ClientSize.Width;
            this.panel.HorizontalScroll.Minimum = 0;
            this.panel.HorizontalScroll.Maximum = this.tileset.TileSize.Width * this.map.Size.Width;
            this.panel.HorizontalScroll.Visible = true;

            this.panel.VerticalScroll.Enabled = true;
            this.panel.VerticalScroll.SmallChange = 128;
            this.panel.VerticalScroll.LargeChange = this.panel.ClientSize.Height;
            this.panel.VerticalScroll.Minimum = 0;
            this.panel.VerticalScroll.Maximum = this.tileset.TileSize.Height * this.map.Size.Height;
            this.panel.VerticalScroll.Visible = true;
        }

        protected override void Dispose(bool disposing)
        {
            this.tileset.Dispose();
            base.Dispose(disposing);
        }

        void box_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            int left = this.panel.HorizontalScroll.Value, top = this.panel.VerticalScroll.Value;
            int x0 = Math.Max(left + e.ClipRectangle.Left / this.tileset.TileSize.Width - 1, 0);
            int x1 = Math.Min(left + e.ClipRectangle.Right / this.tileset.TileSize.Width + 1, map.Size.Width);
            int y0 = Math.Max(top + e.ClipRectangle.Top / this.tileset.TileSize.Height - 1, 0);
            int y1 = Math.Min(top + e.ClipRectangle.Bottom / this.tileset.TileSize.Height + 1, map.Size.Height);
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
