using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public class MapPanel : MapPanelBase
    {
        public MapPanel(List<MapLayer> layers, TilesetInfo tileset)
            : base(layers, tileset)
        {
        }
    }
}
