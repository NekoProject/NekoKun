using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace NekoKun.RubyBindings
{
    class RubyMarshalWriter
    {
        private Stream m_stream;
        private BinaryWriter m_writer;
        private List<object> m_objects;
        private List<RubySymbol> m_symbols;

        public RubyMarshalWriter(Stream output)
        {
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }
            if (!output.CanWrite)
            {
                throw new ArgumentException("stream cannot write");
            }
            this.m_stream = output;
            this.m_objects = new List<object>();
            this.m_symbols = new List<RubySymbol>();
            this.m_writer = new BinaryWriter(m_stream);
        }

        public void Dump(object obj)
        {
            this.m_writer.Write((byte)4);
            this.m_writer.Write((byte)8);
            WriteAnObject(obj);
            this.m_stream.Flush();
        }

        public void WriteAnObject(object obj)
        {
            if (obj is int)
            {
                int num = (int)obj;
                if ((num < -1073741824) || (num >= 0x40000000))
                {
                    obj = num;
                }
            }

            if (obj == null || obj is RubyNil)
            {
                this.m_writer.Write((byte)0x30);
            }
            else if (obj is bool)
            {
                this.m_writer.Write(((bool)obj) ? ((byte)0x54) : ((byte)70));
            }
            else if (obj is int)
            {
                this.WriteFixnum((int)obj);
            }
            else if (obj is RubySymbol)
            {
                if (this.m_symbols.Contains((RubySymbol)obj))
                {
                    this.m_writer.Write((byte)0x3b);
                    this.WriteInt32(this.m_symbols.IndexOf((RubySymbol)obj));
                }
                else
                {
                    this.m_symbols.Add((RubySymbol)obj);
                    this.WriteSymbol((RubySymbol)obj);
                }
            }
            else if (obj is byte[])
            {
                this.m_objects.Add(obj);
                this.WriteString((byte[])obj);
            }
            else if (obj is string)
            {
                this.m_objects.Add(obj);
                this.WriteString((string)obj);
            }
            else if (obj is double)
            {
                this.m_objects.Add(obj);
                this.WriteFloat((double)obj);
            }
            else if (obj is float)
            {
                this.m_objects.Add(obj);
                this.WriteFloat((double)obj);
            }
            else if (this.m_objects.Contains(obj))
            {
                this.m_writer.Write((byte)0x40);
                this.WriteInt32(this.m_objects.IndexOf(obj));
            }
            else
            {
                this.m_objects.Add(obj);
                if (obj is List<object>)
                {
                    this.WriteArray((List<object>)obj);
                }
                else if (obj is RubyHash)
                {
                    this.WriteHash((RubyHash)obj);
                }
                else if (obj is Regex)
                {
                    this.WriteRegex((Regex)obj);
                }
                else if (obj is RubyBignum)
                {
                    this.WriteBignum((RubyBignum)obj);
                }
                else if (obj is RubyClass)
                {
                    this.WriteClass((RubyClass)obj);
                }
                else if (obj is RubyModule)
                {
                    this.WriteModule((RubyModule)obj);
                }
                else if (obj is IRubyUserdefinedDumpObject)
                {
                    this.WriteUsingDump((IRubyUserdefinedDumpObject)obj);
                }
                else if (obj is RubyUserdefinedDumpObject)
                {
                    this.WriteUsingDump((RubyUserdefinedDumpObject)obj);
                }
                else if (obj is RubyUserdefinedMarshalDumpObject)
                {
                    this.WriteUsingMarshalDump((RubyUserdefinedMarshalDumpObject)obj);
                }
                else if (obj is RubyExtendedObject)
                {
                    this.WriteExtendedObject((RubyExtendedObject)obj);
                }
                else if (obj is RubyExpendObject)
                {
                    this.WriteExpendObject((RubyExpendObject)obj);
                }
                else if (obj is RubyStruct)
                {
                    this.WriteStruct((RubyStruct)obj);
                }
                else if (obj is RubyObject)
                {
                    this.WriteObject((RubyObject)obj);
                }
                else
                {
                    throw new ArgumentException("i don't know how to marshal.dump this type: " + obj.GetType().FullName);
                }
            }
        }

        private void WriteUsingDump(IRubyUserdefinedDumpObject iRubyUserdefinedDumpObject)
        {
            this.m_writer.Write((byte)0x75);
            this.WriteSymbol(RubySymbol.GetSymbol(iRubyUserdefinedDumpObject.ClassName));
            this.WriteStringValue(iRubyUserdefinedDumpObject.Dump());
        }

        private void WriteUsingMarshalDump(RubyUserdefinedMarshalDumpObject obj)
        {
            this.m_writer.Write((byte)0x55);
            this.WriteSymbol(obj.ClassName);
            this.WriteAnObject(obj.DumpedObject);
        }

        private void WriteUsingDump(RubyUserdefinedDumpObject obj)
        {
            this.m_writer.Write((byte)0x75);
            this.WriteSymbol(obj.ClassName);
            this.WriteStringValue(obj.DumpedObject as byte[]);
        }

        private void WriteExpendObject(RubyExpendObject obj)
        {
            if (obj.Variables.Count == 0)
            {
                this.m_writer.Write((byte)0x43);
                this.WriteSymbol(obj.ClassName);
                this.WriteAnObject(obj.BaseObject);
            }
            else
            {
                this.m_writer.Write((byte)0x49);
                if (obj.ClassName == null)
                {
                    this.WriteAnObject(obj.BaseObject);
                }
                else
                {
                    this.m_writer.Write((byte)0x43);
                    this.WriteSymbol(obj.ClassName);
                    this.WriteAnObject(obj.BaseObject);
                }
                this.WriteInt32(obj.Variables.Count);
                foreach (KeyValuePair<RubySymbol, object> item in obj.Variables)
                {
                    this.WriteSymbol(item.Key);
                    this.WriteAnObject(item.Value);
                }
            }
        }

        private void WriteObject(RubyObject obj)
        {
            this.m_writer.Write((byte)0x6f);
            this.WriteSymbol(obj.ClassName);
            this.WriteInt32(obj.Variables.Count);
            foreach (KeyValuePair<RubySymbol, object> item in obj.Variables)
            {
                this.WriteSymbol(item.Key);
                this.WriteAnObject(item.Value);
            }
        }

        private void WriteStruct(RubyStruct obj)
        {
            this.m_writer.Write((byte)0x53);
            this.WriteSymbol(obj.ClassName);
            this.WriteInt32(obj.Variables.Count);
            foreach (KeyValuePair<RubySymbol, object> item in obj.Variables)
            {
                this.WriteSymbol(item.Key);
                this.WriteAnObject(item.Value);
            }
        }

        private void WriteExtendedObject(RubyExtendedObject obj)
        {
            this.m_writer.Write((byte)0x65);
            this.WriteSymbol(obj.ExtendedModule.Symbol);
            this.WriteAnObject(obj.BaseObject);
        }

        private void WriteModule(RubyModule obj)
        {
            this.m_writer.Write((byte)0x6d);
            this.WriteStringValue(obj.ToString());
        }

        private void WriteClass(RubyClass obj)
        {
            this.m_writer.Write((byte)0x63);
            this.WriteStringValue(obj.ToString());
        }

        private void WriteBignum(RubyBignum value)
        {
            this.m_writer.Write((byte)0x6c);
            this.WriteBignumValue(value);
        }

        private void WriteBignumValue(RubyBignum value)
        {
            char ch;
            if (value.Sign > 0)
                ch = '+';
            else if (value.Sign < 0)
                ch = '-';
            else
                ch = '0';
            this.m_writer.Write((byte)ch);
            uint[] words = value.GetWords();
            int num = words.Length * 2;
            int index = words.Length - 1;
            bool flag = false;
            if ((words.Length > 0) && ((words[index] >> 0x10) == 0))
            {
                num--;
                flag = true;
            }
            this.WriteInt32(num);
            for (int i = 0; i < words.Length; i++)
            {
                if (flag && (i == index))
                {
                    this.m_writer.Write((ushort)words[i]);
                }
                else
                {
                    this.m_writer.Write(words[i]);
                }
            }
        }

        private void WriteRegex(Regex value)
        {
            this.m_writer.Write((byte)0x2f);
            this.WriteStringValue(value.ToString());
            byte options = 0;
            if ((value.Options & RegexOptions.IgnoreCase) != RegexOptions.None) options += 1;
            if ((value.Options & RegexOptions.IgnorePatternWhitespace) != RegexOptions.None) options += 2;
            if ((value.Options & RegexOptions.Multiline) != RegexOptions.None) options += 4;
            this.m_writer.Write(options);
        }

        private void WriteFloat(double value)
        {
            this.m_writer.Write((byte)0x66);
            if (double.IsInfinity(value))
            {
                if (double.IsPositiveInfinity(value))
                {
                    this.WriteStringValue("inf");
                }
                else
                {
                    this.WriteStringValue("-inf");
                }
            }
            else if (double.IsNaN(value))
            {
                this.WriteStringValue("nan");
            }
            else
            {
                this.WriteStringValue(string.Format("{0:g}", value));
            }
        }

        private void WriteHash(RubyHash value)
        {
            char ch = (value.DefaultValue != null) ? '}' : '{';
            this.m_writer.Write((byte)ch);
            this.WriteInt32(value.Count);
            foreach (KeyValuePair<object, object> pair in value)
            {
                this.WriteAnObject(pair.Key);
                this.WriteAnObject(pair.Value);
            }
            if (value.DefaultValue != null)
            {
                this.WriteAnObject(value.DefaultValue);
            }
        }

        private void WriteArray(List<object> value)
        {
            this.m_writer.Write((byte)0x5b);
            this.WriteInt32(value.Count);
            foreach (object obj2 in value)
            {
                this.WriteAnObject(obj2);
            }
        }

        private void WriteString(byte[] bytes)
        {
            this.m_writer.Write((byte)0x22);
            this.WriteStringValue(bytes);
        }

        private void WriteString(string value)
        {
            this.m_writer.Write((byte)0x22);
            this.WriteStringValue(value);
        }

        private void WriteStringValue(string value)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(value);
            byte[] buffer = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, bytes);
            this.WriteInt32(buffer.Length);
            this.m_writer.Write(buffer);
        }

        private void WriteStringValue(byte[] value)
        {
            this.WriteInt32(value.Length);
            this.m_writer.Write(value);
        }

        private void WriteSymbol(RubySymbol value)
        {
            if (this.m_symbols.Contains(value))
            {
                this.m_writer.Write((byte)0x3b);
                this.WriteInt32(this.m_symbols.IndexOf(value));
            }
            else
            {
                this.m_symbols.Add(value);
                this.m_writer.Write((byte)0x3a);
                this.WriteStringValue(value.GetString());
            }
        }

        private void WriteFixnum(int value)
        {
            this.m_writer.Write((byte)0x69);
            this.WriteInt32(value);
        }

        private void WriteInt32(int value)
        {
            if (value == 0)
            {
                this.m_writer.Write((byte)0);
            }
            else if ((value > 0) && (value < 0x7b))
            {
                this.m_writer.Write((byte)(value + 5));
            }
            else if ((value < 0) && (value > -124))
            {
                this.m_writer.Write((sbyte)(value - 5));
            }
            else
            {
                sbyte num2;
                byte[] buffer = new byte[5];
                buffer[1] = (byte)(value & 0xff);
                buffer[2] = (byte)((value >> 8) & 0xff);
                buffer[3] = (byte)((value >> 0x10) & 0xff);
                buffer[4] = (byte)((value >> 0x18) & 0xff);
                int index = 4;
                if (value >= 0)
                {
                    while (buffer[index] == 0)
                    {
                        index--;
                    }
                    num2 = (sbyte)index;
                }
                else
                {
                    while (buffer[index] == 0xff)
                    {
                        index--;
                    }
                    num2 = (sbyte)-index;
                }
                buffer[0] = (byte)num2;
                this.m_writer.Write(buffer, 0, index + 1);
            }
        }
    }
}
