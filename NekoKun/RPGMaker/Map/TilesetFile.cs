using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public abstract class TilesetFile : DatabaseFile
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

        protected abstract TilesetInfo Build(ObjectEditor.Struct item);
    }
}
