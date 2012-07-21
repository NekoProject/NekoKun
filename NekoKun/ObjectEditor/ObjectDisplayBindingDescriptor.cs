using System;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.Core;

namespace orzTech.NekoKun.ObjectEditor
{
    public class ObjectDisplayBindingDescriptor
    {
        AddIn addIn;
        string typeName;
        string className;

        public ObjectDisplayBindingDescriptor(AddIn addIn,
               string typeName, string className)
        {
            this.addIn = addIn;
            this.typeName = typeName;
            typeNotFound = (typeName == null);
            this.className = className;
        }

        IObjectDisplayBinding cachedDisplayBinding;

        public IObjectDisplayBinding GetDisplayBinding()
        {
            if (cachedDisplayBinding == null)
            {
                cachedDisplayBinding = (IObjectDisplayBinding)addIn.CreateObject(className);
            }
            return cachedDisplayBinding;
        }

        Type cachedType;
        bool typeNotFound = false;

        public bool CanHandle(Type type)
        {
            if (typeNotFound) return true;
            if (cachedType == null)
            {
                cachedType = Type.GetType(typeName);
                if (cachedType == null)
                {
                    foreach (System.Reflection.Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        cachedType = asm.GetType(typeName);
                        if (cachedType != null) break;
                    }
                    if (cachedType == null)
                        typeNotFound = true;
                }
            }
            return type.IsInstanceOfType(cachedType) || type == cachedType;
        }
    }
}
