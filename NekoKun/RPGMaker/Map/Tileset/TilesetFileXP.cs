using System;
using System.Collections.Generic;
using System.Text;
using NekoKun.Serialization.RubyMarshal;

namespace NekoKun.RPGMaker
{
    public class TilesetFileXP : TilesetFile
    {
        public TilesetFileXP(Dictionary<string, object> node)
            : base(node)
        {
        }

        protected override TilesetInfo Build(ObjectEditor.Struct item)
        {
            List<System.Drawing.Image> images = new List<System.Drawing.Image>();

            if (item["@tileset_name"] is string)
                images.Add(ResourceManager.Caches["Graphics/Tilesets"][item["@tileset_name"] as string] as System.Drawing.Image);
            else if (item["@tileset_name"] is RubyString)
                images.Add(ResourceManager.Caches["Graphics/Tilesets"][(item["@tileset_name"] as RubyString).Text] as System.Drawing.Image);
            else
                images.Add(null);

            if (item["@autotile_names"] is RubyArray)
                foreach (object name in item["@autotile_names"] as RubyArray)
                {
                    if (name is string)
                    {
                        images.Add(ResourceManager.Caches["Graphics/Autotiles"][name as string] as System.Drawing.Image);
                    }
                    else if (name is RubyString)
                    {
                        images.Add(ResourceManager.Caches["Graphics/Autotiles"][(name as RubyString).Text] as System.Drawing.Image);
                    }
                    else
                    {
                        images.Add(null);
                    }
                }

            TilesetInfo info = new TilesetInfoXP(images);
            return info;
        }
    }
}
