using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.ObjectEditor
{
    public abstract class ObjectFile : AbstractFile
    {
        public abstract object Contents { get; }
        protected abstract override void Save();
        public abstract override AbstractEditor CreateEditor();

        public ObjectFile(string filename) : base(filename) { }
    }
}
