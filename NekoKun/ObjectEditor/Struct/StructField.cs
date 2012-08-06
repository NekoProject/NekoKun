using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace NekoKun.ObjectEditor
{
    [Serializable]
    public class StructField
    {
        public string ID;
        public string Name;
        /// <summary>
        /// 根据 XmlNode 创建字段。
        /// </summary>
        /// <param name="field">描述此 field 的 XmlNode</param>
        public StructField(System.Xml.XmlNode field)
        {
            ID = field.Attributes["ID"].Value;
            Name = field.Attributes["Name"].Value;
        }

        /// <summary>
        /// 根据 ID 创建 Unknown 字段。
        /// </summary>
        /// <param name="id">ID</param>
        public StructField(string id)
        {
            ID = id;
            Name = id;
        }
    }
}
