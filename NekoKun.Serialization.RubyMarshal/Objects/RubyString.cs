using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.Serialization.RubyMarshal
{
    [System.Diagnostics.DebuggerTypeProxy(typeof(RubyStringDebugView))]
    public class RubyString : RubyObject
    {
        internal class RubyStringDebugView
        {
            internal RubyString str;

            public RubyStringDebugView(RubyString str)
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

            public RubySymbol ClassName
            {
                get { return str.ClassName; }
            }

            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)]
            public KeyValuePair<RubySymbol, object>[] Keys
            {
                get
                {
                    KeyValuePair<RubySymbol, object>[] keys = new KeyValuePair<RubySymbol, object>[str.InstanceVariables.Count];

                    int i = 0;
                    foreach (KeyValuePair<RubySymbol, object> key in str.InstanceVariables)
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

        public RubyString(string unicodeText)
        {
            this.encoding = Encoding.Unicode;
            this.str = unicodeText;
            this.setByText = true;
            this.ClassName = RubySymbol.GetSymbol("String");
            this.Encoding = Encoding.UTF8;
        }

        public RubyString(byte[] raw)
        {
            this.raw = raw;
            this.encoding = Encoding.Default;
            this.setByRaw = true;
            this.ClassName = RubySymbol.GetSymbol("String");
        }

        public RubyString(byte[] raw, Encoding encoding)
        {
            this.raw = raw;
            this.encoding = encoding;
            this.setByRaw = true;
            this.ClassName = RubySymbol.GetSymbol("String");
        }

        public RubyString ForceEncoding(Encoding encoding)
        {
            this.Encoding = encoding;
            return this;
        }

        public RubyString Encode(Encoding encoding)
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
                else if (this.encoding !=  null)
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
