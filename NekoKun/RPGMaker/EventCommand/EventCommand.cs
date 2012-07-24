using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public class EventCommand
    {
        public string ID;
        public string Name;
        public string Format;
        public EventCommandGroup Group;
        public bool IsGenerated;
        public string GeneratedBy;

        public EventCommand(System.Xml.XmlNode node, EventCommandGroup group)
        {
            ID = node.Attributes["ID"].Value;
            Name = node.Attributes["Name"].Value;
            Group = group;
            if (node.Attributes["Format"] != null)
                Format = node.Attributes["Format"].Value;
            else
                Format = Name;

            if (node.Attributes["GeneratedBy"] != null)
            {
                IsGenerated = true;
                GeneratedBy = node.Attributes["GeneratedBy"].Value;
            }
        }
    }
}
