using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun
{
    public class Enum
    {
        public string ID;
        public string Name;
        protected Dictionary<string, string> KTV;
        protected Dictionary<string, string> VTK;

        public Enum(System.Xml.XmlNode node)
        {
            KTV = new Dictionary<string, string>();
            VTK = new Dictionary<string, string>();
            ID = node.Attributes["ID"].Value;
            Name = node.Attributes["Name"].Value;

            foreach (System.Xml.XmlNode cmd in node.ChildNodes)
            {
                if (cmd.Name != "EnumItem") continue;

                string k = cmd.Attributes["Value"].Value;
                string v = cmd.FirstChild.Value;

                KTV.Add(k, v);
                VTK.Add(v, k);
            }
        }

        public string this[string key]
        {
            get { return this.KTV[key]; }
        }

        public string GetKey(string value)
        {
            return this.VTK[value];
        }

        public string[] GetValues()
        {
            string[] res = new string[this.KTV.Count];
            this.KTV.Values.CopyTo(res, 0);
            return res;
        }

        public string[] GetKeys()
        {
            string[] res = new string[this.KTV.Count];
            this.KTV.Keys.CopyTo(res, 0);
            return res;
        }
    }
}
