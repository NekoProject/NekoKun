using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public class MapLayer
    {
        public MapLayerType Type;
        public int[,] Data;
    }

    public enum MapLayerType
    {
        Tile,
        HalfBlockShadow,
    }
}
