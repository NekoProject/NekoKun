using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public abstract class TilesetInfo : IDisposable
    {
        public System.Drawing.Size TileSize;
        protected Dictionary<int, System.Drawing.Image> tiles;
        protected List<System.Drawing.Image> images;
        protected List<System.Drawing.Rectangle?> imagesBounds;
        private MapLayer tilePanelData;

        public TilesetInfo(List<System.Drawing.Image> images)
        {
            tiles = new Dictionary<int, System.Drawing.Image>();

            this.images = images;
            this.imagesBounds = new List<System.Drawing.Rectangle?>();
            foreach (var item in images)
            {
                if (item != null)
                {
                    imagesBounds.Add(new System.Drawing.Rectangle(System.Drawing.Point.Empty, item.Size));
                }
                else
                    imagesBounds.Add(null);
            }
        }

        public List<System.Drawing.Image> Images
        {
            get { return this.images; }
        }

        public abstract System.Drawing.Image this[int id]
        {
            get;
        }

        public void Dispose()
        {
            foreach (var item in this.images)
            {
                if (item != null) item.Dispose();
            }

            foreach (var item in this.tiles)
            {
                if (item.Value != null) item.Value.Dispose();
            }
        }

        public MapLayer TilePanelData
        {
            get {
                if (tilePanelData != null)
                    return tilePanelData;
                return tilePanelData = BuildTilePanelData();
            }
        }

        protected abstract MapLayer BuildTilePanelData();
    }
}
