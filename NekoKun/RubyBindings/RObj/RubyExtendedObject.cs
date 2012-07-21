using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RubyBindings
{
    public class RubyExtendedObject
    {
        private object baseObject;
        private RubyModule extendedModule;

        public override string ToString()
        {
            return this.baseObject.ToString() + "extended " + this.extendedModule.ToString();
        }

        public object BaseObject
        {
            get { return baseObject; }
            set { baseObject = value; }
        }

        public RubyModule ExtendedModule
        {
            get { return extendedModule; }
            set { this.extendedModule = value; }
        }
    }
}
