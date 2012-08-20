using System;
using System.Collections.Generic;
using System.Text;

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

            if (item["@tileset_names"] is List<object>)
                foreach (object name in item["@tileset_names"] as List<object>)
                {
                    if (name is string)
                    {
                        images.Add(ResourceManager.Caches["Graphics/Tilesets"][name as string] as System.Drawing.Image);
                    }
                    else if (name is RubyBindings.RubyExpendObject)
                    {
                        images.Add(ResourceManager.Caches["Graphics/Tilesets"][(name as RubyBindings.RubyExpendObject)] as System.Drawing.Image);
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
