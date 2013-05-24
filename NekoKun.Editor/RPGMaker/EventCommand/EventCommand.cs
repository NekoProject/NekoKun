using System;
using System.Collections.Generic;
using System.Text;
using NekoKun.Serialization.RubyMarshal;

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
            {
                Format = group.Provider.Format.PrepareFormat(node.Attributes["Format"].Value);
            }
            else
                Format = Name;

            if (node.Attributes["GeneratedBy"] != null)
            {
                IsGenerated = true;
                GeneratedBy = node.Attributes["GeneratedBy"].Value;
            }
        }

        public string FormatParams(RubyObject ev)
        {
            object o = ev.InstanceVariable["@parameters"];
            if (o == null)
                return this.Group.Provider.Format.ParseFormat(this.Format, null);

            List<object> param = o as List<object>;
            if (param.Count == 0)
                return this.Group.Provider.Format.ParseFormat(this.Format, null);

            return this.Group.Provider.Format.ParseFormat(this.Format, param);
        }
    }
}
