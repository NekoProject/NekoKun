using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun
{
    public class SyntaxSugarIndexer<TKey, TValue>
    {
        protected IndexerDelegate load;
        private DictionaryWithDefaultProc<string, object> inner;

        public SyntaxSugarIndexer(IndexerDelegate load)
        {
            this.load = load;
        }

        public delegate TValue IndexerDelegate(TKey key);

        public TValue this[TKey key]
        {
            get {
                return load(key);
            }
        }

        public IndexerDelegate Indexer
        {
            get { return this.load; }
            set { this.load = value; }
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
