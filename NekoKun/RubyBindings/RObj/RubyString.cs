using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RubyBindings
{
    public class RubyString
    {
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
        }

        public RubyString(byte[] raw)
        {
            this.raw = raw;
            this.encoding = null;
            this.setByRaw = true;
        }

        public RubyString(byte[] raw, Encoding encoding)
        {
            this.raw = raw;
            this.encoding = encoding;
            this.setByRaw = true;
        }

        public byte[] Raw
        {
            get {
                if (this.setByRaw)
                    return this.raw;
                else if (this.encoding != null)
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
                else if (this.encoding != null)
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

        public Encoding Encoding
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
    }
}
