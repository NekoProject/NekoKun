using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public class MapEditor : AbstractEditor
    {
        MapFile map;
        TilesetInfo tileset;

        NekoKun.UI.OwnerPaintPanel mapPanel;
        System.Drawing.SolidBrush shadowBrush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(128, 0, 0, 0));
        System.Drawing.Size size;

        public MapEditor(MapFile file)
            : base(file)
        {
            map = file;
            this.tileset = map.TilesetFile[map.TilesetID];
            this.size = this.tileset.TileSize;

            mapPanel = new NekoKun.UI.OwnerPaintPanel();
            mapPanel.ContentWidth = this.tileset.TileSize.Width * this.map.Size.Width;
            mapPanel.ContentHeight = this.tileset.TileSize.Height * this.map.Size.Height;
            mapPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            mapPanel.Paint += new System.Windows.Forms.PaintEventHandler(box_Paint);
            this.Controls.Add(mapPanel);
        }


        protected override void Dispose(bool disposing)
        {
            this.tileset.Dispose();
            base.Dispose(disposing);
        }

        void box_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            int left = this.mapPanel.ScrollOffsetX;
            int top = this.mapPanel.ScrollOffsetY;
            int x0 = Math.Max((left + e.ClipRectangle.Left) / size.Width - 1, 0);
            int x1 = Math.Min((left + e.ClipRectangle.Right) / size.Width + 1, map.Size.Width);
            int y0 = Math.Max((top + e.ClipRectangle.Top) / size.Height - 1, 0);
            int y1 = Math.Min((top + e.ClipRectangle.Bottom) / size.Height + 1, map.Size.Height);
            System.Drawing.Rectangle dest = new System.Drawing.Rectangle();
            
            dest.Size = size;
            for (int x = x0; x < x1; x++)
            {
                for (int y = y0; y < y1; y++)
                {
                    dest.X = x * size.Width - left;
                    dest.Y = y * size.Height - top;
                    e.Graphics.SetClip(dest);
                    e.Graphics.Clear(System.Drawing.Color.SteelBlue);
                    for (int layer = 0; layer < 3; layer++)
                    {
                        e.Graphics.DrawImage(
                            this.tileset[this.map.data[x, y, layer]],
                            dest
                        );

                        if (this.map.data.ZSize == 4 && layer == 1)
                        {
                            int shadowData = this.map.data[x, y, 3];
                            for (int shadow = 0; shadow < 4; shadow++)
                            {
                                if ((shadowData & (1 << shadow)) != 0)
                                {
                                    e.Graphics.FillRectangle(
                                        shadowBrush,
                                        dest.X + (shadow % 2) * size.Width / 2,
                                        dest.Y + (shadow / 2) * size.Height / 2,
                                        size.Width / 2,
                                        size.Height / 2
                                    );
                                }
                            }
                        }
                    }
                }
            }
        }


    }
}
