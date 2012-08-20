using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public class MapInfoFile : AbstractFile
    {
        public Dictionary<string, MapFile> maps;
        public TilesetFile TilesetFile;

        public MapInfoFile(Dictionary<string, object> node)
            : base(
                System.IO.Path.Combine(
                    ProjectManager.ProjectDir,
                    node["FileName"].ToString()
                )
              )
        {
            this.maps = new Dictionary<string, MapFile>();

            this.TilesetFile = ProjectManager.Components[node["TilesetProvider"].ToString()] as TilesetFile;

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
                    map = new MapFile(System.IO.Path.Combine(ProjectManager.ProjectDir, filename), this.TilesetFile);
                }
                else
                {
                    this.MakeDirty();
                    continue;
                }
                this.maps.Add(key, map);

                RubyBindings.RubyObject info = item.Value as RubyBindings.RubyObject;
                map.Title = (info["@name"] is RubyBindings.RubyExpendObject) ? ((RubyBindings.RubyExpendObject)info["@name"]) : (info["@name"] as string);
                map.ParentID = info["@parent_id"].ToString();
                map.Order = (int) info["@order"];
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
            return new MapInfoEditor(this);
        }

        public override string ToString()
        {
            return "地图索引";
        }
    }
}
