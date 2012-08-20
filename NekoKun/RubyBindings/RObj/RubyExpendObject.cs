using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RubyBindings
{
    public class RubyExpendObject : RubyObject
    {
        private object baseObject;

        public override string ToString()
        {
            return this.baseObject.ToString();
        }

        public object BaseObject
        {
            get { return baseObject; }
            set { baseObject = value; }
        }

        public static implicit operator string(RubyExpendObject obj)
        {
            return obj.BaseObject as string;
        }
    }

}
