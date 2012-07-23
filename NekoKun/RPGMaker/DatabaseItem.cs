using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public class DatabaseItem
    {
        public Dictionary<DatabaseField, object> Items;

        public DatabaseItem(Dictionary<DatabaseField, object> items)
        {
            this.Items = items;
        }

        public override string ToString()
        {
            foreach (var item in Items)
            {
                if (item.Key.ID == "@name")
                    return item.Value as string;
            }
            return base.ToString();
        }

        public object this[DatabaseField key]
        {
            get { 
                if (this.Items.ContainsKey(key))
                {
                    return this.Items[key];
                }
                else
                {
                    return null;
                }
            }
            set {
                if (this.Items.ContainsKey(key))
                {
                    this.Items[key] = value;
                }
                else
                {
                    this.Items.Add(key, value);
                }
            }
        }
    }
}
