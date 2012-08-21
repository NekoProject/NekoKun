using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public class MapEditor : AbstractEditor, IToolboxProvider
    {
        MapFile map;
        TilesetInfo tileset;

        MapPanel mapPanel;
        TilePanel tilePanel;

        System.Drawing.Size size;

        public MapEditor(MapFile file)
            : base(file)
        {
            map = file;
            this.tileset = map.TilesetFile[map.TilesetID];
            this.size = this.tileset.TileSize;

            mapPanel = new MapPanel(map.Layers, tileset);
            mapPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Controls.Add(mapPanel);

            tilePanel = new TilePanel(tileset);
        }

        protected override void Dispose(bool disposing)
        {
            this.tileset.Dispose();
            base.Dispose(disposing);
        }

        public System.Windows.Forms.Control ToolboxControl
        {
            get { return tilePanel; }
        }
    }
}
