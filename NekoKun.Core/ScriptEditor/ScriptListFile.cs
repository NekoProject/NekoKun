using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun
{
    public abstract class ScriptListFile : AbstractFile
    {
        public List<ScriptFile> scripts;

        public ScriptListFile(string filename)
            : base(filename)
        {
            this.scripts = new List<ScriptFile>();
        }

        public override AbstractEditor CreateEditor()
        {
            return new ScriptListEditor(this);
        }

        public abstract ScriptFile InsertFile(string pageName, int index);
        public abstract void DeleteFile(ScriptFile file);
        public abstract string GenerateFileName(string pageName);

        protected override void Save()
        {
            throw new NotImplementedException();
        }
    }
}
