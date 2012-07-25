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
        protected System.Reflection.MethodInfo formatMethod;

        public EventCommand(System.Xml.XmlNode node, EventCommandGroup group)
        {
            ID = node.Attributes["ID"].Value;
            Name = node.Attributes["Name"].Value;
            Group = group;
            if (node.Attributes["Format"] != null)
            {
                Format = node.Attributes["Format"].Value;
                ParseFormat();
            }
            else
                Format = Name;

            if (node.Attributes["GeneratedBy"] != null)
            {
                IsGenerated = true;
                GeneratedBy = node.Attributes["GeneratedBy"].Value;
            }
        }

        protected void ParseFormat()
        {
            //{assembly|Helper|NekoKun.RPGMaker.XPHelper|EventCommand111}
            System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(
                this.Format,
                @"^\{assembly\|(.*?)\|(.*?)\|(.*?)\}$",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
            );
            if (match.Success)
            {
                System.Reflection.Assembly assembly = (ProjectManager.Components[match.Groups[1].Value] as IAssemblyProvider).GetAssembly();
                formatMethod = assembly
                    .GetType(match.Groups[2].Value, true, true)
                    .GetMethod(match.Groups[3].Value);
                System.Diagnostics.Debug.Assert(formatMethod != null, "FormatMethod should be a valid method");
            }
        }

        public string FormatParams(NekoKun.RubyBindings.RubyObject ev)
        {
            object o = ev["@parameters"];
            if (o == null)
            {
                if (this.formatMethod != null)
                    return this.formatMethod.Invoke(null, new object[] { new List<object>() }) as string;
                else
                    return this.Format;
            }
            List<object> param = o as List<object>;
            if (param.Count == 0)
            {
                if (this.formatMethod != null)
                    return this.formatMethod.Invoke(null, new object[] { new List<object>() }) as string;
                else
                    return this.Format;
            }

            if (this.formatMethod != null)
                return this.formatMethod.Invoke(null, new object[] { param }) as string;
            else
                return System.Text.RegularExpressions.Regex.Replace(this.Format, @"\{([0-9]*)(.([@A-Za-z0-9_]))?\}",
                    delegate(System.Text.RegularExpressions.Match match)
                    {
                        try
                        {
                            int index = Int32.Parse(match.Groups[1].Value);
                            return param[index].ToString();
                        }
                        catch { return match.Value; }
                    }
                );
        }
    }
}
