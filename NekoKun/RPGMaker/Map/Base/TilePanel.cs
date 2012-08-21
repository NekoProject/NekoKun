using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public class TilePanel : MapPanelBase
    {
        public TilePanel(TilesetInfo tileset)
            : base(new List<MapLayer>(new MapLayer[] { tileset.TilePanelData }), tileset)
        {
        }
    }
}
