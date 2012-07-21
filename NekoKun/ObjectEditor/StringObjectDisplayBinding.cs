using System;
using System.Collections;
using ICSharpCode.Core;

namespace orzTech.NekoKun.ObjectEditor
{
    class StringObjectDisplayBinding : IObjectDisplayBinding
    {
        public IObjectViewContent OpenObject(object Object)
        {
            if (
                !(Object is System.String)
               )
                return null;
            return new StringObjectViewContent(Object);
        }
    }
}
