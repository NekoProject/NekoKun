using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun
{
    public class DictionaryWithDefaultProc<TKey, TValue> : Dictionary<TKey, TValue>
    {
        protected DefaultProcDelegate load;

        public DictionaryWithDefaultProc(DefaultProcDelegate load)
        {
            this.load = load;
        }

        public delegate TValue DefaultProcDelegate(TKey key);

        public new TValue this[TKey key]
        {
            get {
                if (!this.ContainsKey(key))
                {
                    this.Add(key, load(key));
                }
                return base[key];
            }
            set {
                if (this.ContainsKey(key))
                    base[key] = value;
                else
                    this.Add(key, value);
            }
        }
    }
}
