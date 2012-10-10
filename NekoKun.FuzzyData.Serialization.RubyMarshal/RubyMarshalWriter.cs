using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace NekoKun.FuzzyData.Serialization.RubyMarshal
{
    class RubyMarshalWriter
    {
        private Stream m_stream;
        private BinaryWriter m_writer;
        private List<object> m_objects;
        private List<FuzzySymbol> m_symbols;

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
            this.m_symbols = new List<FuzzySymbol>();
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
            WriteAnObject(obj, true);
        }

        public void WriteAnObject(object obj, bool count)
        {
            if (obj is int)
            {
                int num = (int)obj;
                if ((num < -1073741824) || (num >= 0x40000000))
                {
                    obj = num;
                }
            }

            if (obj == null || obj is FuzzyNil)
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
            else if (obj is FuzzySymbol)
            {
                this.WriteSymbol((FuzzySymbol)obj);
            }
            else if (this.m_objects.Contains(obj))
            {
                this.m_writer.Write((byte)0x40);
                this.WriteInt32(this.m_objects.IndexOf(obj));
            }   
            else if (obj is string)
            {
                FuzzyString str = new FuzzyString(obj as string);
                if (count) this.m_objects.Add(str);
                this.WriteString(str);
            }
            else
            {
                if (count) this.m_objects.Add(obj);
                if (obj is FuzzyString)
                {
                    if ((obj as FuzzyString).Encoding != null)
                    {
                        FuzzyExpendObject ooo = new FuzzyExpendObject();
                        ooo.BaseObject = obj;
                        this.WriteExpendObject(ooo);
                    }
                    else
                    {
                        this.WriteString((FuzzyString)obj);
                    }
                }
                else if (obj is FuzzyFloat)
                {
                    this.WriteFloat((FuzzyFloat)obj);
                }
                else if (obj is double)
                {
                    this.WriteFloat(new FuzzyFloat((double)obj));
                }
                else if (obj is float)
                {
                    this.WriteFloat(new FuzzyFloat((double)obj));
                }
                else if (obj is List<object>)
                {
                    this.WriteArray((List<object>)obj);
                }
                else if (obj is FuzzyHash)
                {
                    this.WriteHash((FuzzyHash)obj);
                }
                else if (obj is FuzzyRegexp)
                {
                    if ((obj as FuzzyRegexp).Pattern.Encoding != null)
                    {
                        FuzzyExpendObject ooo = new FuzzyExpendObject();
                        ooo.BaseObject = obj;
                        this.WriteExpendObject(ooo);
                    }
                    else
                    {
                        this.WriteRegex((FuzzyRegexp)obj);
                    }
                }
                else if (obj is FuzzyBignum)
                {
                    this.WriteBignum((FuzzyBignum)obj);
                }
                else if (obj is FuzzyClass)
                {
                    this.WriteClass((FuzzyClass)obj);
                }
                else if (obj is FuzzyModule)
                {
                    this.WriteModule((FuzzyModule)obj);
                }
                else if (obj is IRubyUserdefinedDumpObject)
                {
                    this.WriteUsingDump((IRubyUserdefinedDumpObject)obj);
                }
                else if (obj is FuzzyUserdefinedDumpObject)
                {
                    this.WriteUsingDump((FuzzyUserdefinedDumpObject)obj);
                }
                else if (obj is FuzzyUserdefinedMarshalDumpObject)
                {
                    this.WriteUsingMarshalDump((FuzzyUserdefinedMarshalDumpObject)obj);
                }
                else if (obj is FuzzyExtendedObject)
                {
                    this.WriteExtendedObject((FuzzyExtendedObject)obj);
                }
                else if (obj is FuzzyExpendObject)
                {
                    this.WriteExpendObject((FuzzyExpendObject)obj);
                }
                else if (obj is FuzzyStruct)
                {
                    this.WriteStruct((FuzzyStruct)obj);
                }
                else if (obj is FuzzyObject)
                {
                    this.WriteObject((FuzzyObject)obj);
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
            this.WriteSymbol(FuzzySymbol.GetSymbol(iRubyUserdefinedDumpObject.ClassName));
            this.WriteStringValue(iRubyUserdefinedDumpObject.Dump());
        }

        private void WriteUsingMarshalDump(FuzzyUserdefinedMarshalDumpObject obj)
        {
            this.m_writer.Write((byte)0x55);
            this.WriteSymbol(obj.ClassName);
            this.WriteAnObject(obj.DumpedObject);
        }

        private void WriteUsingDump(FuzzyUserdefinedDumpObject obj)
        {
            this.m_writer.Write((byte)0x75);
            this.WriteSymbol(obj.ClassName);
            this.WriteStringValue(obj.DumpedObject as byte[]);
        }

        private void WriteExpendObject(FuzzyExpendObject obj)
        {
            if (obj.BaseObject is FuzzySymbol || obj.BaseObject is FuzzyString || obj.BaseObject is FuzzyRegexp)
            {
                Encoding e = null;
                if (obj.BaseObject is FuzzySymbol)
                    e = (obj.BaseObject as FuzzySymbol).GetRubyString().Encoding;
                if (obj.BaseObject is FuzzyString)
                    e = (obj.BaseObject as FuzzyString).Encoding;
                if (obj.BaseObject is FuzzyRegexp)
                    e = (obj.BaseObject as FuzzyRegexp).Pattern.Encoding;

                if (e == Encoding.UTF8)
                    obj.InstanceVariable["E"] = true;
                else if (e == null)
                    obj.InstanceVariable["E"] = false;
                else
                    obj.InstanceVariable["encoding"] = new FuzzyString(System.Text.Encoding.ASCII.GetBytes(e.WebName));
            }

            if (obj.InstanceVariables.Count == 0)
            {
                this.m_writer.Write((byte)0x43);
                this.WriteSymbol(obj.ClassName);
                this.WriteAnObject(obj.BaseObject, false);
            }
            else
            {
                this.m_writer.Write((byte)0x49);
                if (obj.ClassName != null)
                {
                    this.m_writer.Write((byte)0x43);
                    this.WriteSymbol(obj.ClassName);
                }
                if (obj.BaseObject is FuzzyString)
                {
                    this.WriteString(obj.BaseObject as FuzzyString);
                }
                else if (obj.BaseObject is FuzzySymbol)
                {
                    this.m_writer.Write((byte)0x3a);
                    this.WriteStringValue((obj.BaseObject as FuzzySymbol).GetRubyString().Raw);
                }
                else if (obj.BaseObject is FuzzyRegexp)
                {
                    this.WriteRegex(obj.BaseObject as FuzzyRegexp);
                }
                else
                {
                    this.WriteAnObject(obj.BaseObject, false);
                }
                this.WriteInt32(obj.InstanceVariables.Count);
                foreach (KeyValuePair<FuzzySymbol, object> item in obj.InstanceVariables)
                {
                    this.WriteSymbol(item.Key);
                    this.WriteAnObject(item.Value);
                }
            }
        }

        private void WriteObject(FuzzyObject obj)
        {
            this.m_writer.Write((byte)0x6f);
            this.WriteSymbol(obj.ClassName);
            this.WriteInt32(obj.InstanceVariables.Count);
            foreach (KeyValuePair<FuzzySymbol, object> item in obj.InstanceVariables)
            {
                this.WriteSymbol(item.Key);
                this.WriteAnObject(item.Value);
            }
        }

        private void WriteStruct(FuzzyStruct obj)
        {
            this.m_writer.Write((byte)0x53);
            this.WriteSymbol(obj.ClassName);
            this.WriteInt32(obj.InstanceVariables.Count);
            foreach (KeyValuePair<FuzzySymbol, object> item in obj.InstanceVariables)
            {
                this.WriteSymbol(item.Key);
                this.WriteAnObject(item.Value);
            }
        }

        private void WriteExtendedObject(FuzzyExtendedObject obj)
        {
            this.m_writer.Write((byte)0x65);
            this.WriteSymbol(obj.ExtendedModule.Symbol);
            this.WriteAnObject(obj.BaseObject);
        }

        private void WriteModule(FuzzyModule obj)
        {
            this.m_writer.Write((byte)0x6d);
            this.WriteStringValue(obj.ToString());
        }

        private void WriteClass(FuzzyClass obj)
        {
            this.m_writer.Write((byte)0x63);
            this.WriteStringValue(obj.ToString());
        }

        private void WriteBignum(FuzzyBignum value)
        {
            this.m_writer.Write((byte)0x6c);
            this.WriteBignumValue(value);
        }

        private void WriteBignumValue(FuzzyBignum value)
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

        private void WriteRegex(FuzzyRegexp value)
        {
            this.m_writer.Write((byte)0x2f);
            this.WriteStringValue(value.Pattern.Raw);
            this.m_writer.Write((byte)value.Options);
        }

        private void WriteFloat(FuzzyFloat v)
        {
            double value = v.Value;
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

        private void WriteHash(FuzzyHash value)
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

        private void WriteString(FuzzyString value)
        {
            this.m_writer.Write((byte)0x22);
            this.WriteStringValue(value.Raw);
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

        private void WriteSymbol(FuzzySymbol value)
        {
            if (this.m_symbols.Contains(value))
            {
                this.m_writer.Write((byte)0x3b);
                this.WriteInt32(this.m_symbols.IndexOf(value));
            }
            else
            {
                this.m_symbols.Add(value);
                if (value.GetRubyString() != null)
                {
                    FuzzyExpendObject ooo = new FuzzyExpendObject();
                    ooo.BaseObject = value;
                    this.WriteExpendObject(ooo);
                }
                else
                {
                    this.m_writer.Write((byte)0x3a);
                    this.WriteStringValue(value.GetString());
                }
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
