using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.Core
{
    public static class XMLHelper
    {

        public static Dictionary<string, object> BuildParameterDictionary(System.Xml.XmlNode field)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            foreach (System.Xml.XmlNode property in field.ChildNodes)
            {
                if (property.HasChildNodes && property.ChildNodes.Count == 1 && (property.FirstChild is System.Xml.XmlText))
                    dict.Add(
                        property.Attributes["Name"].Value,
                        property.FirstChild.Value
                    );
                else
                    dict.Add(
                        property.Attributes["Name"].Value,
                        property.ChildNodes
                    );

            }

            return dict;
        }
    }
}
