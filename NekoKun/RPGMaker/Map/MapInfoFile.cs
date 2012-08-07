using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public class MapInfoFile : AbstractFile
    {
        public Dictionary<string, MapFile> maps;

        public MapInfoFile(Dictionary<string, object> node)
            : base(
                System.IO.Path.Combine(
                    ProjectManager.ProjectDir,
                    node["FileName"].ToString()
                )
              )
        {
            this.maps = new Dictionary<string, MapFile>();
            RubyBindings.RubyHash mapHash = RubyBindings.RubyMarshal.Load(
                new System.IO.FileStream(this.filename,  System.IO.FileMode.Open, System.IO.FileAccess.Read)
            ) as RubyBindings.RubyHash;
            foreach (var item in mapHash)
	        {
                string key = null;
                string filename;
                MapFile map = null;
                if (item.Key is int)
                {
                    key = item.Key.ToString();
                    filename = String.Format(node["FileNameFormat"].ToString(), (int)item.Key);
                    map = new MapFile(System.IO.Path.Combine(ProjectManager.ProjectDir, filename));
                }
                else
                {
                    this.MakeDirty();
                    continue;
                }
                this.maps.Add(key, map);

                RubyBindings.RubyObject info = item.Value as RubyBindings.RubyObject;
                map.Title = info["name"] as string;

                /*
                    parent_id 
                    The parent map ID.

                    order 
                    The map tree display order, which is used internally.

                    expanded 
                    The map tree expansion flag, which is used internally.

                    scroll_x 
                    The x-axis scroll position, which is used internally.

                    scroll_y 
                    The y-axis scroll position, which is used internally.
                */
	        }
            this.maps.ToString();
        }

        protected override void Save()
        {
            throw new NotImplementedException();
        }

        public override AbstractEditor CreateEditor()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "地图索引";
        }
    }
}
