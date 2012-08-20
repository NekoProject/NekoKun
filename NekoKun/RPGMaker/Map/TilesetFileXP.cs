using System;
using System.Collections.Generic;
using System.Text;

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
                images.Add(ResourceManager.Caches["Tilesets"][item["@tileset_name"] as string] as System.Drawing.Image);
            else
                images.Add(null);

            if (item["@autotile_names"] is List<object>)
                foreach (object name in item["@autotile_names"] as List<object>)
                {
                    if (name is string)
                    {
                        images.Add(ResourceManager.Caches["Autotiles"][name as string] as System.Drawing.Image);
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
