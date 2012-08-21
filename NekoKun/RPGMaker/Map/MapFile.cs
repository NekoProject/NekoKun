using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public class MapFile : AbstractFile
    {
        public string Title;
        private bool infoLoaded = false;
        public System.Drawing.Size Size;
        public TilesetInfo tileset;
        public List<MapLayer> Layers;
        public int TilesetID;
        public TilesetFile TilesetFile;
        public string ParentID;
        public int Order;

        public MapFile(string filename, TilesetFile tilesetFile)
            : base(filename)
        {
            this.TilesetFile = tilesetFile;
        }

        public override string ToString()
        {
            return this.Title + " (" + base.ToString() + ")";
        }

        protected override void Save()
        {
            throw new NotImplementedException();
        }

        public override AbstractEditor CreateEditor()
        {
            if (!infoLoaded)
                LoadInfo();
            return new MapEditor(this);
        }

        private void LoadInfo()
        {
            RubyBindings.RGSSTable data;
            infoLoaded = true;

            RubyBindings.RubyObject raw = RubyBindings.RubyMarshal.Load(new System.IO.FileStream(this.filename, System.IO.FileMode.Open, System.IO.FileAccess.Read)) as RubyBindings.RubyObject;
            this.TilesetID = (int)raw["@tileset_id"] - 1;
            data = raw["@data"] as RubyBindings.RGSSTable;
            this.Size = new System.Drawing.Size((int)raw["@width"], (int)raw["@height"]);

            this.Layers = new List<MapLayer>();
            for (int z = 0; z < data.ZSize; z++)
            {
                MapLayer layer = new MapLayer();
                layer.Data = new int[this.Size.Width, this.Size.Height];
                for (int x = 0; x < this.Size.Width; x++)
                {
                    for (int y = 0; y < this.Size.Height; y++)
                    {
                        layer.Data[x, y] = data[x, y, z];
                    }
                }
                layer.Type = MapLayerType.Tile;

                this.Layers.Add(layer);
            }

            if (this.Layers.Count == 4)
            {
                MapLayer layer = this.Layers[3];
                this.Layers.Remove(layer);
                this.Layers.Insert(2, layer);

                layer.Type = MapLayerType.HalfBlockShadow;
            }
        }
        /*
          @tileset_id = 1
          @width = width
          @height = height
          @autoplay_bgm = false
          @bgm = RPG::AudioFile.new
          @autoplay_bgs = false
          @bgs = RPG::AudioFile.new("", 80)
          @encounter_list = []
          @encounter_step = 30
          @data = Table.new(width, height, 3)
          @events = {}  
        */
    }
}
