using System;
using System.Collections.Generic;
using System.Text;
using NekoKun.Serialization.RubyMarshal;

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

            RubyHash mapHash = NekoKun.Serialization.RubyMarshal.RubyMarshal.Load(
                new System.IO.FileStream(this.filename,  System.IO.FileMode.Open, System.IO.FileAccess.Read)
            ) as RubyHash;
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

                RubyObject info = item.Value as RubyObject;
                map.Title = (info.InstanceVariable["@name"] as RubyString).Text;
                map.ParentID = info.InstanceVariable["@parent_id"].ToString();
                map.Order = (int)info.InstanceVariable["@order"];
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
