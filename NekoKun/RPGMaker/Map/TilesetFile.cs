using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public class TilesetFile : DatabaseFile
    {
        List<object> content;
        public TilesetFile(Dictionary<string, object> node)
            : base(node)
        {
            content = base.contents as List<object>;
        }

        public TilesetInfo this[int id]
        {
            get
            {
                ObjectEditor.Struct item = content[id] as ObjectEditor.Struct;
                return Build(item);
            }
        }

        private TilesetInfo Build(ObjectEditor.Struct item)
        {
            System.Drawing.Image tileset = null;
            List<System.Drawing.Image> autotiles = new List<System.Drawing.Image>();

            if (item["@tileset_name"] is string)
                tileset = ResourceManager.Caches["Tilesets"][item["@tileset_name"] as string] as System.Drawing.Image;

            if (item["@autotile_names"] is List<object>)
                foreach (object name in item["@autotile_names"] as List<object>)
                {
                    if (name is string)
                    {
                        autotiles.Add(ResourceManager.Caches["Autotiles"][name as string] as System.Drawing.Image);
                    }
                    else
                    {
                        autotiles.Add(null);
                    }
                }

            TilesetInfo info = new TilesetInfo(tileset, autotiles);
            return info;
        }
    }
}
