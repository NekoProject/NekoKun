using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public class MapFile : AbstractFile
    {
        public string Title;

        public MapFile(string filename)
            : base(filename)
        {
            
        }

        protected override void Save()
        {
            throw new NotImplementedException();
        }

        public override AbstractEditor CreateEditor()
        {
            throw new NotImplementedException();
        }
    }
}
