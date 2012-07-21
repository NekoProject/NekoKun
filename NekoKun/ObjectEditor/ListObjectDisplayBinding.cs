using System;
using System.Collections;
using ICSharpCode.Core;

namespace orzTech.NekoKun.ObjectEditor
{
    class ListObjectDisplayBinding : IObjectDisplayBinding
    {
        public IObjectViewContent OpenObject(object Object)
        {
            if (
                !(Object is System.Collections.Generic.List<object>) &&
                !(Object is System.Collections.Generic.Dictionary<object, object>)
               )
                return null;
            return new ListObjectViewContent(Object);
        }
    }
}
