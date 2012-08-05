using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.ObjectEditor
{
    public class Struct : Dictionary<StructField, object>
    {
        public override string ToString()
        {
            foreach (var item in this)
            {
                if (item.Key.ID == "@name")
                    return (item.Value as string) ?? "";
            }
            return base.ToString();
        }

        public new object this[StructField key]
        {
            get
            {
                if (this.ContainsKey(key))
                {
                    return base[key];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (this.ContainsKey(key))
                {
                    base[key] = value;
                }
                else
                {
                    this.Add(key, value);
                }
            }
        }
    }
}
