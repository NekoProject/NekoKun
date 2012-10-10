using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.FuzzyData
{
    public class FuzzyUserdefinedMarshalDumpObject : FuzzyObject
    {
        private object dumpedObject;

        public override string ToString()
        {
            return "#<" + this.ClassName.ToString() + ", dumped object: " + this.dumpedObject.ToString() + ">";
        }

        public object DumpedObject
        {
            get { return dumpedObject; }
            set { dumpedObject = value; }
        }
    }
}
