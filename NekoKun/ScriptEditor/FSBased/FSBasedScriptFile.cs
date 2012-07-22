using System;
using System.Collections.Generic;

using System.Text;

namespace NekoKun
{
    public class FSBasedScriptFile : ScriptFile
    {
        public FSBasedScriptFile(string filename) : base(filename)
        {
            this.Code = System.IO.File.ReadAllText(filename, Encoding.UTF8);
        }

        public FSBasedScriptFile(string filename, string code) : base(filename, code) { }

        protected override void Save()
        {
            System.IO.File.WriteAllText(this.filename, this.Code, Encoding.UTF8);
        }
    }
}
