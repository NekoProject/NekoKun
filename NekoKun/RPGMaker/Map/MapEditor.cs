using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public class MapEditor : AbstractEditor
    {
        MapFile map;
        System.Drawing.Image[] layer;
        System.Windows.Forms.Panel box;

        public MapEditor(MapFile file)
            : base(file)
        {
            map = file;
            layer = new System.Drawing.Image[3];
            for (int i = 0; i < 3; i++)
            {
                layer[i] = DrawLayer(i);
            }

            box = new MapPanel();
            this.AutoScroll = true;
            box.Location = System.Drawing.Point.Empty;
            box.Size = new System.Drawing.Size(
                map.Size.Width * map.tileset.TileSize.Width,
                map.Size.Height * map.tileset.TileSize.Height
            );
            this.Controls.Add(box);

            box.Paint += new System.Windows.Forms.PaintEventHandler(box_Paint);
        }

        void box_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            e.Graphics.SetClip(e.ClipRectangle);
            e.Graphics.Clear(System.Drawing.Color.Transparent);
            for (int i = 0; i < 3; i++)
			{
                e.Graphics.DrawImage(this.layer[i], e.ClipRectangle, e.ClipRectangle, System.Drawing.GraphicsUnit.Pixel);
			}
        }

        protected System.Drawing.Image DrawLayer(int id)
        {
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(
                map.Size.Width * map.tileset.TileSize.Width,
                map.Size.Height * map.tileset.TileSize.Height,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb
            );
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            for (int x = 0; x < map.Size.Width; x++)
            {
                for (int y = 0; y < map.Size.Height; y++)
                {
                    g.DrawImage(
                        this.map.tileset[this.map.data[x, y, id]],
                        x * map.tileset.TileSize.Width,
                        y * map.tileset.TileSize.Height
                    );
                }
            }
            g.Dispose();

            return bitmap;
        }

        public class MapPanel : System.Windows.Forms.Panel
        {
            public MapPanel()
            {
                this.DoubleBuffered = true;
            }
        }
    }
}
