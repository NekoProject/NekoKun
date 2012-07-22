using System;
using System.Collections.Generic;

using System.Text;

namespace NekoKun.RPGMaker
{
    public class ScriptFile : NekoKun.ScriptFile
    {
        public ScriptListFile parent;
        public string Title;
        public int ID;
        public ScriptFile(ScriptListFile file, string code, string title, int id)
            : base(String.Format("{0}:{1}", file.filename, id.ToString()))
        {
            this.parent = file;
            this.Code = code;
            this.Title = title;
            this.ID = id;
        }

        protected override void Delete()
        {
            return;
        }

        protected override void Save()
        {
            this.parent.Commit();
        }

        public override string ToString()
        {
            return this.Title;
        }
    }
}
