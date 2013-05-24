using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun
{
    public class EnumProvider
    {
        public Dictionary<String, Enum> Enums;

        public EnumProvider(Dictionary<string, object> node)
        {
            Enums = new Dictionary<string, Enum>();

            var groups = node["Enums"] as System.Xml.XmlNodeList;
            foreach (System.Xml.XmlNode group in groups)
            {
                if (group.Name != "Enum") continue;

                Enum item = new Enum(group);
                Enums.Add(item.ID, item);
            }
        }
    }
}
