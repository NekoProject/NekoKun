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
        public FuzzyData.FuzzyObject raw;

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
            if (!this.infoLoaded)
                return;

            FuzzyData.RGSSTable table = new NekoKun.FuzzyData.RGSSTable(this.Size.Width, this.Size.Height, this.Layers.Count);
            List<MapLayer> truth = new List<MapLayer>(this.Layers);
            if (truth.Count == 4)
            {
                MapLayer layer = truth[2];
                truth.Remove(layer);
                truth.Add(layer);
            }
            for (int z = 0; z < truth.Count; z++)
            {
                for (int x = 0; x < this.Size.Width; x++)
                {
                    for (int y = 0; y < this.Size.Height; y++)
                    {
                        table[x, y, z] = truth[z].Data[x, y];
                    }
                }
            }
            raw.InstanceVariable["@data"] = table;

            using (var fs = new System.IO.FileStream(this.filename, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                NekoKun.FuzzyData.Serialization.RubyMarshal.RubyMarshal.Dump(fs, raw);
            }
        }

        public override AbstractEditor CreateEditor()
        {
            if (!infoLoaded)
                LoadInfo();

            return new MapEditor(this);
        }

        private void LoadInfo()
        {
            FuzzyData.RGSSTable data;
            infoLoaded = true;

            raw = NekoKun.FuzzyData.Serialization.RubyMarshal.RubyMarshal.Load(new System.IO.FileStream(this.filename, System.IO.FileMode.Open, System.IO.FileAccess.Read)) as FuzzyData.FuzzyObject;
            this.TilesetID = (int)raw.InstanceVariable["@tileset_id"] - 1;
            var j = raw.InstanceVariable["@data"] as NekoKun.FuzzyData.Serialization.RubyMarshal.FuzzyUserdefinedDumpObject;
            data = new FuzzyData.RGSSTable(j.DumpedObject as byte[]);
            this.Size = new System.Drawing.Size((int)raw.InstanceVariable["@width"], (int)raw.InstanceVariable["@height"]);

            this.Layers = new List<MapLayer>();
            for (int z = 0; z < data.ZSize; z++)
            {
                MapLayer layer = new MapLayer();
                layer.Data = new short[this.Size.Width, this.Size.Height];
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

            raw.InstanceVariable["@data"] = null;
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
