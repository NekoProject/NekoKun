using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.FuzzyData
{
    public class FuzzyExpendObject : FuzzyObject
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

        public static implicit operator string(FuzzyExpendObject obj)
        {
            return obj.BaseObject as string;
        }
    }

}
