using System;
using System.Collections.Generic;
using System.Text;
using NekoKun.Serialization.RubyMarshal;

namespace NekoKun.RPGMaker
{
    public class TilesetFileVA: TilesetFile
    {
        public TilesetFileVA(Dictionary<string, object> node)
            : base(node)
        {
        }

        protected override TilesetInfo Build(ObjectEditor.Struct item)
        {
            List<System.Drawing.Image> images = new List<System.Drawing.Image>();

            if (item["@tileset_names"] is RubyArray)
                foreach (object name in item["@tileset_names"] as RubyArray)
                {
                    if (name is string)
                    {
                        images.Add(ResourceManager.Caches["Graphics/Tilesets"][name as string] as System.Drawing.Image);
                    }
                    else if (name is RubyString)
                    {
                        images.Add(ResourceManager.Caches["Graphics/Tilesets"][(name as RubyString).Text] as System.Drawing.Image);
                    }
                    else
                    {
                        images.Add(null);
                    }
                }

            TilesetInfo info = new TilesetInfoVA(images);
            return info;
        }
    }
}
