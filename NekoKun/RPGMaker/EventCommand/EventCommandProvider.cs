using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public class EventCommandProvider
    {
        public Dictionary<String, EventCommandGroup> Groups;
        public Dictionary<String, EventCommand> Commands;
        public FormatProvider Format;

        public EventCommandProvider(Dictionary<string, object> node)
        {
            Format = ProjectManager.Components[node["Format"] as string] as FormatProvider;

            Groups = new Dictionary<string,EventCommandGroup>();
            Commands = new Dictionary<string,EventCommand>();
            
            var groups = node["EventCommands"] as System.Xml.XmlNodeList;
            foreach (System.Xml.XmlNode group in groups)
            {
                if (group.Name != "Group") continue;

                EventCommandGroup item = new EventCommandGroup(group, this);
                Groups.Add(item.ID, item);

                foreach (var cmd in item.Commands)
                {
                    Commands.Add(cmd.Key, cmd.Value);
                }
            }
        }
    }
}
