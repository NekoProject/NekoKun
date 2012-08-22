using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public class MapLayer
    {
        public MapLayerType Type;
        public short[,] Data;

        private DictionaryWithDefaultProc<string, object> inner;
        public DictionaryWithDefaultProc<string, object> Storage
        {
            get
            {
                if (this.inner == null)
                    this.inner = new DictionaryWithDefaultProc<string, object>((string s) => null);
                return this.inner;
            }
        }
    }

    public enum MapLayerType
    {
        Tile,
        HalfBlockShadow,
        TilesetLabel,
    }
}
