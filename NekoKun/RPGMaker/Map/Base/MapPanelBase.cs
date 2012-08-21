using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public class MapPanelBase : UI.OwnerPaintPanel
    {
        protected System.Drawing.Size size;
        protected System.Drawing.Size mapSize;
        protected List<MapLayer> layers;
        protected TilesetInfo tileset;
        protected System.Drawing.SolidBrush shadowBrush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(128, 0, 0, 0));

        public MapPanelBase(List<MapLayer> layers, TilesetInfo tileset)
        {
            this.layers = layers;
            this.tileset = tileset;
            this.mapSize = new System.Drawing.Size(layers[0].Data.GetLength(0), layers[0].Data.GetLength(1));
            this.size = this.tileset.TileSize;

            this.ContentWidth = this.tileset.TileSize.Width * this.mapSize.Width;
            this.ContentHeight = this.tileset.TileSize.Height * this.mapSize.Height;

            this.Paint += new System.Windows.Forms.PaintEventHandler(this_Paint);
        }

        void PaintTile(int x, int y, int offx, int offy, System.Drawing.Graphics g)
        {
            System.Drawing.Rectangle dest = new System.Drawing.Rectangle();
            dest.Size = size;
            dest.X = x * size.Width - offx;
            dest.Y = y * size.Height - offy;
            g.SetClip(dest);
            g.Clear(System.Drawing.Color.SteelBlue);
            foreach (var item in this.layers)
            {
                if (item.Type == MapLayerType.Tile)
                {
                    g.DrawImage(
                        this.tileset[item.Data[x, y]],
                        dest
                    );

                }
                else if (item.Type == MapLayerType.HalfBlockShadow)
                {
                    int shadowData = item.Data[x, y];
                    for (int shadow = 0; shadow < 4; shadow++)
                    {
                        if ((shadowData & (1 << shadow)) != 0)
                        {
                            g.FillRectangle(
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

        void this_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            int left = this.ScrollOffsetX;
            int top = this.ScrollOffsetY;
            int x0 = Math.Max((left + e.ClipRectangle.Left) / size.Width - 1, 0);
            int x1 = Math.Min((left + e.ClipRectangle.Right) / size.Width + 1, mapSize.Width);
            int y0 = Math.Max((top + e.ClipRectangle.Top) / size.Height - 1, 0);
            int y1 = Math.Min((top + e.ClipRectangle.Bottom) / size.Height + 1, mapSize.Height);
            
            for (int x = x0; x < x1; x++)
            {
                for (int y = y0; y < y1; y++)
                {
                    PaintTile(x, y, left, top, e.Graphics);
                }
            }
        }
    }
}
