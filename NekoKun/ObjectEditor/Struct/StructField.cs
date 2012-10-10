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
        protected object defaultValue;
        protected DefaultValueDelegate defaultProc;

        /// <summary>
        /// 根据 XmlNode 创建字段。
        /// </summary>
        /// <param name="field">描述此 field 的 XmlNode</param>
        public StructField(System.Xml.XmlNode field)
        {
            ID = field.Attributes["ID"].Value;
            Name = field.Attributes["Name"].Value;
            if (field.Attributes["Type"] != null)
            {
                if (field.Attributes["Type"].Value == "RubyMarshal")
                {
                    byte[] bytes = Convert.FromBase64String(field.InnerText);
                    this.defaultProc = delegate()
                    {
                        System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
                        return NekoKun.FuzzyData.Serialization.RubyMarshal.RubyMarshal.Load(ms);
                    };
                }
            }
            else
            {
                int def;
                if (Int32.TryParse(field.InnerText, out def))
                    this.defaultValue = def;
                else
                    this.defaultValue = field.InnerText;
            }
        }

        /// <summary>
        /// 根据 ID 创建 Unknown 字段。
        /// </summary>
        /// <param name="id">ID</param>
        public StructField(string id, DefaultValueDelegate defaultValue)
        {
            ID = id;
            Name = id;
            this.defaultProc = defaultValue;
        }

        public object DefaultValue
        {
            get{
                if (this.defaultProc != null)
                    return this.defaultProc();
                return this.defaultValue;
            }
        }

        public delegate object DefaultValueDelegate();
    }
}
