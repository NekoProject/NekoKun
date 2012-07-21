using System;
using System.Collections;
using ICSharpCode.Core;
using orzTech.NekoKun.ObjectEditor;

namespace orzTech.NekoKun.ProjectEngines.RGSS
{
    public class RubyClassObjectDisplayBinding : IObjectDisplayBinding
    {
        public IObjectViewContent OpenObject(object Object)
        {
            if (!(Object is RubyObject))
                return null;
            return new RubyClassObjectViewContent(Object as RubyObject);
        }
    }
}
