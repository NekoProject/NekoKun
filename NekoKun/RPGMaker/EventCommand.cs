using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public class EventCommand
    {
        public string ID;
        public string Name;

        public EventCommand(System.Xml.XmlNode node)
        {
            ID = node.Attributes["ID"].Value;
            Name = node.Attributes["Name"].Value;
        }
    }
}
