using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace NekoKun.Core
{
    public static class ReflectionHelper
    {
        private static Dictionary<string, Type> typeNameToType = new Dictionary<string, Type>();

        public static object CreateInstanceFromTypeName(string typeName, params object[] param)
        {
            Type target;

            // Find in cached dictionary.
            if (typeNameToType.TryGetValue(typeName.ToLowerInvariant(), out target))
            {
                if (target != null)
                    return target.InvokeMember("", BindingFlags.CreateInstance, null, null, param);
            }

            // Find in popular assemblies
            List<Assembly> asses = new List<Assembly>();
            asses.Add(System.Reflection.Assembly.GetCallingAssembly());
            if (!asses.Contains(System.Reflection.Assembly.GetExecutingAssembly()))
                asses.Add(System.Reflection.Assembly.GetExecutingAssembly());
            if (!asses.Contains(System.Reflection.Assembly.GetEntryAssembly()))
                asses.Add(System.Reflection.Assembly.GetEntryAssembly());
            foreach (Assembly ass in asses)
            {
                target = ass.GetType(typeName, false, true);
                if (target != null)
                {
                    typeNameToType.Add(typeName.ToLowerInvariant(), target);
                    return target.InvokeMember("", BindingFlags.CreateInstance, null, null, param);
                }
            }

            // Find in all assemblies
            List<Assembly> assesFull = new List<Assembly>(System.AppDomain.CurrentDomain.GetAssemblies());
            assesFull.RemoveAll((o) => asses.Contains(o));
            asses = assesFull;
            foreach (Assembly ass in asses)
            {
                target = ass.GetType(typeName, false, true);
                if (target != null)
                {
                    typeNameToType.Add(typeName.ToLowerInvariant(), target);
                    return target.InvokeMember("", BindingFlags.CreateInstance, null, null, param);
                }
            }

            // 404 Not Found
            throw new System.TypeLoadException(String.Format("Cannot find type `{0}` in any loaded assemblies in current AppDomain. ", typeName));
        }
    }
}
