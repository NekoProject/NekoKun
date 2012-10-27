using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.FuzzyData
{
    [System.Diagnostics.DebuggerTypeProxy(typeof(FuzzyStringDebugView))]
    public class FuzzyString : FuzzyObject
    {
        internal class FuzzyStringDebugView
        {
            internal FuzzyString str;

            public FuzzyStringDebugView(FuzzyString str)
            {
                this.str = str;
            }

            public string Text
            {
                get { return str.Text; }
            }

            public Encoding Encoding
            {
                get { return str.Encoding; }
            }

            public FuzzySymbol ClassName
            {
                get { return str.ClassName; }
            }

            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)]
            public KeyValuePair<FuzzySymbol, object>[] Keys
            {
                get
                {
                    KeyValuePair<FuzzySymbol, object>[] keys = new KeyValuePair<FuzzySymbol, object>[str.InstanceVariables.Count];

                    int i = 0;
                    foreach (KeyValuePair<FuzzySymbol, object> key in str.InstanceVariables)
                    {
                        keys[i] = key;
                        i++;
                    }
                    return keys;
                }
            }
        }

        protected byte[] raw = null;
        protected System.Text.Encoding encoding = null;
        protected string str = null;
        protected bool setByText = false;
        protected bool setByRaw = false;

        public FuzzyString(string unicodeText)
        {
            this.encoding = Encoding.Unicode;
            this.str = unicodeText;
            this.setByText = true;
            this.ClassName = FuzzySymbol.GetSymbol("String");
        }

        public FuzzyString(byte[] raw)
        {
            this.raw = raw;
            this.encoding = Encoding.Default;
            this.setByRaw = true;
            this.ClassName = FuzzySymbol.GetSymbol("String");
        }

        public FuzzyString(byte[] raw, Encoding encoding)
        {
            this.raw = raw;
            this.encoding = encoding;
            this.setByRaw = true;
            this.ClassName = FuzzySymbol.GetSymbol("String");
        }

        public FuzzyString ForceEncoding(Encoding encoding)
        {
            this.Encoding = encoding;
            return this;
        }

        public FuzzyString Encode(Encoding encoding)
        {
            this.Text = this.Text;
            this.Encoding = encoding;
            return this;
        }

        public byte[] Raw
        {
            get {
                if (this.setByRaw)
                    return this.raw;
                else if (this.encoding != Encoding.Default)
                {
                    this.setByText = false;
                    this.setByRaw = true;
                    this.raw = this.encoding.GetBytes(this.str);
                    return this.raw;
                }
                else
                    throw new NotSupportedException();
            }
            set {
                this.raw = value;
                this.setByText = false;
                this.setByRaw = true;
            }
        }

        public string Text
        {
            get
            {
                if (this.setByText)
                    return this.str;
                else if (this.encoding != Encoding.Default)
                {
                    this.setByRaw = false;
                    this.setByText = true;
                    this.Text = this.encoding.GetString(this.raw);
                    return this.str;
                }
                else
                {
                    return Encoding.Default.GetString(this.raw);
                }
            }
            set
            {
                this.str = value;
                if (this.encoding == null)
                    this.encoding = Encoding.Unicode;
                this.setByText = true;
                this.setByRaw = false;
            }
        }

        public string RawText
        {
            get { return Encoding.Default.GetString(this.raw); }
        }

        public override Encoding Encoding
        {
            get
            {
                return this.encoding;
            }
            set
            {
                this.Text.ToString();
                this.encoding = value;
            }
        }

        public override string ToString()
        {
            return this.Text;
        }
    }
}
