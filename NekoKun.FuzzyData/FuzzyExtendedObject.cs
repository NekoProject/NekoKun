using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.FuzzyData
{
    public class FuzzyExtendedObject
    {
        private object baseObject;
        private FuzzyModule extendedModule;

        public override string ToString()
        {
            return this.baseObject.ToString() + "extended " + this.extendedModule.ToString();
        }

        public object BaseObject
        {
            get { return baseObject; }
            set { baseObject = value; }
        }

        public FuzzyModule ExtendedModule
        {
            get { return extendedModule; }
            set { this.extendedModule = value; }
        }
    }
}
