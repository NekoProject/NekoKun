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
        protected List<bool> layersVisible;
        protected TilesetInfo tileset;
        protected System.Drawing.SolidBrush shadowBrush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(128, 0, 0, 0));
        protected float zoom;

        public MapPanelBase(List<MapLayer> layers, TilesetInfo tileset)
        {
            this.layers = layers;
            this.layersVisible = new List<bool>();
            foreach (var item in this.layers)
            {
                this.layersVisible.Add(true);
            }

            this.tileset = tileset;
            this.mapSize = new System.Drawing.Size(layers[0].Data.GetLength(0), layers[0].Data.GetLength(1));
            this.Zoom = 1.0f;
            this.Paint += new System.Windows.Forms.PaintEventHandler(this_Paint);
        }

        public List<bool> LayersVisible
        {
            get { return this.layersVisible; }
        }

        public float Zoom
        {
            get
            {
                return this.zoom;
            }
            set
            {
                if (value == 0) return;

                zoom = value;
                this.size.Width = Math.Max((int) (this.tileset.TileSize.Width * zoom), 1);
                this.size.Height = Math.Max((int) (this.tileset.TileSize.Height * zoom), 1);

                this.ContentWidth = this.size.Width * this.mapSize.Width;
                this.ContentHeight = this.size.Height * this.mapSize.Height;
                this.InvalidateContents();
            }
        }

        private void PaintTile(int x, int y, System.Drawing.Graphics g)
        {
            System.Drawing.Rectangle dest = new System.Drawing.Rectangle();
            dest.Size = size;
            dest.X = x * size.Width - this.ScrollOffsetX;
            dest.Y = y * size.Height - this.ScrollOffsetY;
            g.SetClip(dest);
            g.Clear(System.Drawing.Color.SteelBlue);
            int id = 0;
            foreach (var item in this.layers)
            {
                if (this.layersVisible[id])
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

                id += 1;
            }
        }

        void this_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            int x0 = Math.Max((ScrollOffsetX + e.ClipRectangle.Left) / size.Width - 1, 0);
            int x1 = Math.Min((ScrollOffsetX + e.ClipRectangle.Right) / size.Width + 1, mapSize.Width);
            int y0 = Math.Max((ScrollOffsetY + e.ClipRectangle.Top) / size.Height - 1, 0);
            int y1 = Math.Min((ScrollOffsetY + e.ClipRectangle.Bottom) / size.Height + 1, mapSize.Height);
            
            for (int x = x0; x < x1; x++)
            {
                for (int y = y0; y < y1; y++)
                {
                    PaintTile(x, y, e.Graphics);
                }
            }
        }

        public void DrawTile(int x, int y)
        {
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(
                x * size.Width - ScrollOffsetX,
                y * size.Height - ScrollOffsetY,
                size.Width,
                size.Height
            );
            if (rect.IntersectsWith(this.panel.ClientRectangle))
                this.panel.Invalidate(rect);
        }

        public System.Drawing.Point PointToMapPoint(int x, int y)
        {
            return new System.Drawing.Point(x / this.size.Width, y / this.size.Height);
        }

        public System.Drawing.Point PointToMapPoint(System.Drawing.Point pt)
        {
            return PointToMapPoint(pt.X, pt.Y);
        }

        public bool MapPointValid(System.Drawing.Point pt)
        {
            if (pt.X < 0) return false;
            if (pt.Y < 0) return false;
            if (pt.X >= this.mapSize.Width) return false;
            if (pt.Y >= this.mapSize.Height) return false;
            return true;
        }
    }
}
