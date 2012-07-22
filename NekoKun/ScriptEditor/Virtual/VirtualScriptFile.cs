using System;
using System.Collections.Generic;

using System.Text;

namespace NekoKun
{
    public class VirtualScriptFile : ScriptFile
    {
        public VirtualScriptFile(string filename) : base(filename)
        {
            this.Code = "";
        }

        public VirtualScriptFile(string filename, string code) : base(filename, code) { }

        protected override void Save()
        {
            
        }

        protected override void Delete()
        {
            
        }
    }
}
