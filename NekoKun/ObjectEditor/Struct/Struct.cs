using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.ObjectEditor
{
    [Serializable]
    public class Struct : Dictionary<StructField, object>
    {
        private DictionaryWithDefaultProc<string, object> inner;

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

        public object this[string key]
        {
            get
            {
                foreach (var item in this.Keys)
                {
                    if (item.ID == key)
                        return this[item];
                }
                return null;
            }
            set
            {
                foreach (var item in this.Keys)
                {
                    if (item.ID == key)
                    {
                        this[item] = value;
                        return;
                    }
                }
                throw new ArgumentException();
            }
        }

        public DictionaryWithDefaultProc<string, object> Runtime
        {
            get
            {
                if (this.inner == null)
                    this.inner = new DictionaryWithDefaultProc<string, object>((string s) => null);
                return this.inner;
            }
        }
    }
}
