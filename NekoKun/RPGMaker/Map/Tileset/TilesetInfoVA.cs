using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker

{
    public class TilesetInfoVA : TilesetInfo
    {
        private static byte[][][] autotilesInfo = new byte[][][] {
            new byte[][] { new byte[] { 19, 18, 15, 14 }, new byte[] { 3, 18, 15, 14 }, new byte[] { 19, 4, 15, 14 }, new byte[] { 3, 4, 15, 14 }, new byte[] { 19, 18, 15, 8 }, new byte[] { 3, 18, 15, 8 }, new byte[] { 19, 4, 15, 8 }, new byte[] { 3, 4, 15, 8 }, new byte[] { 19, 18, 7, 14 }, new byte[] { 3, 18, 7, 14 }, new byte[] { 19, 4, 7, 14 }, new byte[] { 3, 4, 7, 14 }, new byte[] { 19, 18, 7, 8 }, new byte[] { 3, 18, 7, 8 }, new byte[] { 19, 4, 7, 8 }, new byte[] { 3, 4, 7, 8 }, new byte[] { 17, 18, 13, 14 }, new byte[] { 17, 4, 13, 14 }, new byte[] { 17, 18, 13, 8 }, new byte[] { 17, 4, 13, 8 }, new byte[] { 11, 10, 15, 14 }, new byte[] { 11, 10, 15, 8 }, new byte[] { 11, 10, 7, 14 }, new byte[] { 11, 10, 7, 8 }, new byte[] { 19, 20, 15, 16 }, new byte[] { 19, 20, 7, 16 }, new byte[] { 3, 20, 15, 16 }, new byte[] { 3, 20, 7, 16 }, new byte[] { 19, 18, 23, 22 }, new byte[] { 3, 18, 23, 22 }, new byte[] { 19, 4, 23, 22 }, new byte[] { 3, 4, 23, 22 }, new byte[] { 17, 20, 13, 16 }, new byte[] { 11, 10, 23, 22 }, new byte[] { 9, 10, 13, 14 }, new byte[] { 9, 10, 13, 8 }, new byte[] { 11, 12, 15, 16 }, new byte[] { 11, 12, 7, 16 }, new byte[] { 19, 20, 23, 24 }, new byte[] { 3, 20, 23, 24 }, new byte[] { 17, 18, 21, 22 }, new byte[] { 17, 4, 21, 22 }, new byte[] { 9, 12, 13, 16 }, new byte[] { 9, 10, 21, 22 }, new byte[] { 17, 20, 21, 24 }, new byte[] { 11, 12, 23, 24 }, new byte[] { 9, 12, 21, 24 }, new byte[] { 1, 2, 5, 6 } }, 
            new byte[][] { new byte[] { 11, 10, 7, 6 }, new byte[] { 9, 10, 5, 6 }, new byte[] { 3, 2, 7, 6 }, new byte[] { 1, 2, 5, 6 }, new byte[] { 11, 12, 7, 8 }, new byte[] { 9, 12, 5, 8 }, new byte[] { 3, 4, 7, 8 }, new byte[] { 1, 4, 5, 8 }, new byte[] { 11, 10, 15, 14 }, new byte[] { 9, 10, 13, 14 }, new byte[] { 3, 2, 15, 14 }, new byte[] { 1, 2, 13, 14 }, new byte[] { 11, 12, 15, 16 }, new byte[] { 9, 12, 13, 16 }, new byte[] { 3, 4, 15, 16 }, new byte[] { 1, 4, 13, 16 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 } }, 
            new byte[][] { new byte[] { 3, 2, 7, 6 }, new byte[] { 1, 2, 5, 6 }, new byte[] { 3, 4, 7, 8 }, new byte[] { 1, 4, 5, 8 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 }, new byte[] { 1, 1, 1, 1 } } 
        };

        private static Int16[][] tileA1Offset = new Int16[][]
        {
            new Int16[] {0, 0}, new Int16[] {0, 96}, new Int16[] {192, 0}, new Int16[] {192, 96}, 
            new Int16[] {256, 0}, new Int16[] {448, 0}, new Int16[] {256, 96}, new Int16[] {448, 96}, 
            new Int16[] {0, 192}, new Int16[] {192, 192}, new Int16[] {0, 288}, new Int16[] {192, 288}, 
            new Int16[] {256, 192}, new Int16[] {448, 192}, new Int16[] {256, 288}, new Int16[] {448, 288}
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

        private void DrawAutotile(int id, int typeid, int imgid, int offx, int offy, System.Drawing.Graphics g)
        {
            int subid = id % 48;
            byte[] info = autotilesInfo[typeid][subid];
            for (int i = 0; i < 4; i++)
            {
                int pos = info[i] - 1;
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(
                    offx + pos % 4 * (this.TileSize.Width / 2),
                    offy + pos / 4 * (this.TileSize.Height / 2),
                    this.TileSize.Width / 2,
                    this.TileSize.Height / 2
                );
                if (rect.IntersectsWith(this.imagesBounds[imgid].Value))
                {
                    System.Drawing.Rectangle rect2 = new System.Drawing.Rectangle(
                        i % 2 * (this.TileSize.Width / 2),
                        i / 2 * (this.TileSize.Height / 2),
                        this.TileSize.Width / 2,
                        this.TileSize.Height / 2
                    );
                    g.DrawImage(
                        this.images[imgid],
                        rect2,
                        rect,
                        System.Drawing.GraphicsUnit.Pixel
                    );
                }
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
                    if (id == 0) { }
                    else if (id < 256) // B
                        DrawTile(id, 5, g);
                    else if (id < 512) // C
                        DrawTile(id - 256, 6, g);
                    else if (id < 768) // D
                        DrawTile(id - 512, 7, g);
                    else if (id < 1024) // E
                        DrawTile(id - 768, 8, g);
                    else if (id >= 1536 && id < 1664) // A5
                        DrawTile(id - 1536, 4, g);
                    else if (id >= 2048 && id < 2816) // A1
                    {
                        int subid = (id - 2048) / 48;
                        if (subid >= 5 && subid % 2 == 1)
                            DrawAutotile((id - 2048) % 48, 2, 0, tileA1Offset[subid][0], tileA1Offset[subid][1], g);
                        else
                            DrawAutotile((id - 2048) % 48, 0, 0, tileA1Offset[subid][0], tileA1Offset[subid][1], g);
                    }
                    else if (id >= 2816 && id < 4352) // A2
                    {
                        int subid = (id - 2816) / 48;
                        DrawAutotile((id - 2816) % 48, 0, 1, (subid % 8) * 64, (subid / 8) * 96, g);
                    }
                    else if (id >= 4352 && id < 5888) // A3
                    {
                        int subid = (id - 4352) / 48;
                        DrawAutotile((id - 4352) % 48, 1, 2, (subid % 8) * 64, (subid / 8) * 64, g);
                    }
                    else if (id >= 5888 && id < 8192) // A4
                    {
                        int subid = (id - 5888) / 48;
                        if (subid / 8 % 2 == 1)
                            DrawAutotile((id - 5888) % 48, 1, 3, (subid % 8) * 64, (subid / 16) * (96 + 64) + 96, g);
                        else
                            DrawAutotile((id - 5888) % 48, 0, 3, (subid % 8) * 64, (subid / 16) * (96 + 64), g);
                    }
                    else
                    {
                        g.DrawString(id.ToString(), System.Drawing.SystemFonts.DefaultFont, System.Drawing.Brushes.White, 0, 0);
                    }
                    g.Dispose();
                    this.tiles.Add(id, tile);
                    return tile;
                }
            }
        }
        /*
         * 突然好想你 你会在哪里
         * 过得快乐和委屈
         */
        protected override MapLayer BuildTilePanelData()
        {
            List<int> data = new List<int>();
            if (this.images[0] != null)

                for (int i = 0; i < 16; i++)
                    data.Add(2048 + i * 48 + ((i >= 5 && i % 2 == 1) ? 3 : 47));

            if (this.images[1] != null)
                for (int i = 0; i < 32; i++)
                    data.Add(2816 + i * 48 + 47);

            if (this.images[2] != null)
                for (int i = 0; i < 16; i++)
                    data.Add(4352 + i * 48 + 15);

            if (this.images[3] != null)
                for (int i = 0; i < 48; i++)
                    data.Add(5888 + i * 48 + ((i / 8 % 2 == 1) ? 15 : 47));

            if (this.images[4] != null)
                for (int i = 0; i < 128; i++)
                    data.Add(1536 + i);

            if (this.images[5] != null)
                for (int i = 0; i < 256; i++)
                    data.Add(0 + i);

            if (this.images[6] != null)
                for (int i = 0; i < 256; i++)
                    data.Add(256 + i);

            if (this.images[7] != null)
                for (int i = 0; i < 256; i++)
                    data.Add(512 + i);

            if (this.images[8] != null)
                for (int i = 0; i < 256; i++)
                    data.Add(768 + i);

            MapLayer layer = new MapLayer();
            layer.Type = MapLayerType.Tile;
            layer.Data = new int[8, data.Count / 8];
            for (int i = 0; i < data.Count; i++)
            {
                layer.Data[i % 8, i / 8] = data[i];
            }

            return layer;
        }
    }
}