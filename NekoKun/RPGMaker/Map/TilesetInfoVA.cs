using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public class TilesetInfoVA : TilesetInfo
    {
        private static byte[][][] autotilesInfo = new byte[][][]
        {
            new byte[][]{ new byte[] {27, 28, 33, 34}, new byte[] { 5, 28, 33, 34}, new byte[] {27,  6, 33, 34}, new byte[] { 5,  6, 33, 34},new byte[] {27, 28, 33, 12}, new byte[] { 5, 28, 33, 12}, new byte[] {27,  6, 33, 12}, new byte[] { 5,  6, 33, 12}}, 
            new byte[][]{ new byte[] {27, 28, 11, 34}, new byte[] { 5, 28, 11, 34}, new byte[] {27,  6, 11, 34}, new byte[] { 5,  6, 11, 34},new byte[] {27, 28, 11, 12}, new byte[] { 5, 28, 11, 12}, new byte[] {27,  6, 11, 12}, new byte[] { 5,  6, 11, 12}}, 
            new byte[][]{ new byte[] {25, 26, 31, 32}, new byte[] {25,  6, 31, 32}, new byte[] {25, 26, 31, 12}, new byte[] {25,  6, 31, 12},new byte[] {15, 16, 21, 22}, new byte[] {15, 16, 21, 12}, new byte[] {15, 16, 11, 22}, new byte[] {15, 16, 11, 12}}, 
            new byte[][]{ new byte[] {29, 30, 35, 36}, new byte[] {29, 30, 11, 36}, new byte[] { 5, 30, 35, 36}, new byte[] { 5, 30, 11, 36},new byte[] {39, 40, 45, 46}, new byte[] { 5, 40, 45, 46}, new byte[] {39,  6, 45, 46}, new byte[] { 5,  6, 45, 46}}, 
            new byte[][]{ new byte[] {25, 30, 31, 36}, new byte[] {15, 16, 45, 46}, new byte[] {13, 14, 19, 20}, new byte[] {13, 14, 19, 12},new byte[] {17, 18, 23, 24}, new byte[] {17, 18, 11, 24}, new byte[] {41, 42, 47, 48}, new byte[] { 5, 42, 47, 48}}, 
            new byte[][]{ new byte[] {37, 38, 43, 44}, new byte[] {37,  6, 43, 44}, new byte[] {13, 18, 19, 24}, new byte[] {13, 14, 43, 44},new byte[] {37, 42, 43, 48}, new byte[] {17, 18, 47, 48}, new byte[] {13, 18, 43, 48}, new byte[] { 1,  2,  7,  8}}
        };

        public TilesetInfoVA(List<System.Drawing.Image> images)
            : base(images)
        {
            TileSize = new System.Drawing.Size(32, 32);
        }

        private void DrawTile(int id, int imgid, System.Drawing.Graphics g)
        {
            if (this.images[imgid] == null)
                return;
            System.Drawing.Rectangle rect;
            if (id < 128)
            {
                rect = new System.Drawing.Rectangle(
                    new System.Drawing.Point(
                        (id) % 8 * this.TileSize.Width,
                        (id) / 8 * this.TileSize.Height
                    ),
                    TileSize
                );
            }
            else
            {
                rect = new System.Drawing.Rectangle(
                    new System.Drawing.Point(
                        (id) % 8 * this.TileSize.Width + 256,
                        (id) / 8 * this.TileSize.Height - 512
                    ),
                    TileSize
                );
            }
            if (rect.IntersectsWith(this.imagesBounds[imgid].Value))
            {
                g.DrawImage(this.images[imgid], new System.Drawing.Rectangle(System.Drawing.Point.Empty, this.TileSize), rect, System.Drawing.GraphicsUnit.Pixel);
            }
        }

        public override System.Drawing.Image this[int id]
        {
            get
            {
                if (this.tiles.ContainsKey(id))
                    return this.tiles[id];
                else
                {
                    System.Drawing.Bitmap tile = new System.Drawing.Bitmap(this.TileSize.Width, this.TileSize.Height);
                    System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(tile);
                    if (1 == 1)
                    {
                        if (id == 0) { }
                        else if (id < 256)
                            DrawTile(id, 5, g);
                        else if (id < 512)
                            DrawTile(id - 256, 6, g);
                        else if (id < 768)
                            DrawTile(id - 512, 7, g);
                        else if (id < 1024)
                            DrawTile(id - 758, 8, g);
                        else
                        {
                            g.DrawString(id.ToString(), System.Drawing.SystemFonts.DefaultFont, System.Drawing.Brushes.White, 0, 0);
                        }
                    }
                    else
                    {
                        if (id >= 384 && this.images.Count > 1 && this.images[0] != null)
                        {
                            
                        }
                        else if (id >= 48 && id < 384)
                        {
                            int tileid = id / 48 - 1;
                            if (this.images[tileid + 1] != null)
                            {
                                int subid = id % 48;
                                byte[] info = autotilesInfo[subid / 8][subid % 8];
                                for (int i = 0; i < 4; i++)
                                {
                                    int pos = info[i] - 1;
                                    System.Drawing.Rectangle rect = new System.Drawing.Rectangle(
                                        pos % 6 * (this.TileSize.Width / 2),
                                        pos / 6 * (this.TileSize.Height / 2),
                                        this.TileSize.Width / 2,
                                        this.TileSize.Height / 2
                                    );
                                    if (rect.IntersectsWith(this.imagesBounds[tileid + 1].Value))
                                    {
                                        System.Drawing.Rectangle rect2 = new System.Drawing.Rectangle(
                                            i % 2 * (this.TileSize.Width / 2),
                                            i / 2 * (this.TileSize.Height / 2),
                                            this.TileSize.Width / 2,
                                            this.TileSize.Height / 2
                                        );
                                        g.DrawImage(
                                            this.images[tileid + 1],
                                            rect2,
                                            rect,
                                            System.Drawing.GraphicsUnit.Pixel
                                        );
                                    }
                                }
                            }
                        }
                    }
                    g.Dispose();
                    this.tiles.Add(id, tile);
                    return tile;
                }
            }
        }
    }
}