using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace NekoKun.RPGMaker
{
    public class DatabaseField
    {
        public string ID;
        public string Name;
        public string EditorTypeName;
        public Dictionary<string, object> EditorParams;

        /// <summary>
        /// 根据 XmlNode 创建字段。
        /// </summary>
        /// <param name="field">描述此 field 的 XmlNode</param>
        public DatabaseField(System.Xml.XmlNode field)
        {
            ID = field.Attributes["ID"].Value;
            Name = field.Attributes["Name"].Value;
            EditorTypeName = field.Attributes["Editor"].Value;
            EditorParams = new Dictionary<string, object>();
            foreach (XmlNode property in field.ChildNodes)
            {
                if (property.HasChildNodes && property.ChildNodes.Count == 1 && (property.FirstChild is XmlText))
                    EditorParams.Add(
                        property.Attributes["Name"].Value,
                        property.FirstChild.Value
                    );
                else
                    EditorParams.Add(
                        property.Attributes["Name"].Value,
                        property.ChildNodes
                    );

            }
        }

        /// <summary>
        /// 根据 ID 创建 Unknown 字段。
        /// </summary>
        /// <param name="id">ID</param>
        public DatabaseField(string id)
        {
            ID = id;
            Name = id;
            EditorTypeName = (typeof (NekoKun.ObjectEditor.UnknownEditor)).FullName;
            EditorParams = new Dictionary<string, object>();
        }
    }
}
