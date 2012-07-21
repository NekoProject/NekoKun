using System;
using System.Collections.Generic;
using System.Text;

namespace orzTech.NekoKun.ProjectEngines.RGSS
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
    }

}
