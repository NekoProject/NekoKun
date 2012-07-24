using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public class EventCommandGroup
    {
        public Dictionary<String, EventCommand> Commands;
        public string ID;
        public string Name;
        public System.Drawing.Color ForeColor;

        public EventCommandGroup(System.Xml.XmlNode node)
        {
            Commands = new Dictionary<string, EventCommand>();
            ID = node.Attributes["ID"].Value;
            Name = node.Attributes["Name"].Value;
            ForeColor = (node.Attributes["ForeColor"] != null) ? 
                Program.ParseColor(node.Attributes["ForeColor"].Value) :
                System.Drawing.SystemColors.WindowText;

            foreach (System.Xml.XmlNode cmd in node.ChildNodes)
            {
                if (cmd.Name != "EventCommand") continue;

                EventCommand item = new EventCommand(cmd, this);

                this.Commands.Add(item.ID, item);
            }
        }
    }
}
