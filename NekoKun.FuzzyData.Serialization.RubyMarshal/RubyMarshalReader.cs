using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace NekoKun.FuzzyData.Serialization.RubyMarshal
{
    class RubyMarshalReader
    {
        private Stream m_stream;
        private BinaryReader m_reader;
        private List<object> m_objects;
        private List<FuzzySymbol> m_symbols;

        public RubyMarshalReader(Stream input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            if (!input.CanRead)
            {
                throw new ArgumentException("stream cannot read");
            }
            this.m_stream = input;
            this.m_objects = new List<object>();
            this.m_symbols = new List<FuzzySymbol>();
            this.m_reader = new BinaryReader(m_stream, Encoding.ASCII);
        }

        public object Load()
        {
            this.m_reader.Read();
            this.m_reader.Read();
            return ReadAnObject();
        }

        public void AddObject(object Object)
        {
            this.m_objects.Add(Object);
        }

        public object ReadAnObject()
        {
            bool p = false;
            return ReadAnObject(ref p);
        }

        public object ReadAnObject(ref bool hasiv)
        {
            byte id = m_reader.ReadByte();
            switch (id)
            {
                case 0x40: // @ Object Reference
                    return m_objects[ReadInt32()];

                case 0x3b: // ; Symbol Reference
                    return m_symbols[ReadInt32()];

                case 0x30: // 0 NilClass
                    return FuzzyNil.Instance;

                case 0x54: // T TrueClass
                    return true;

                case 0x46: // F FalseClass
                    return false;

                case 0x69: // i Fixnum
                    return ReadInt32();

                case 0x66: // f Float
                    return ReadFloat();

                case 0x22: // " String
                    return ReadString();

                case 0x3a: // : Symbol
                    return ReadSymbol();

                case 0x5b: // [ Array
                    return ReadArray();

                case 0x7b: // { Hash
                case 0x7d: // } Hash w/ default value
                    return ReadHash(id == 0x7d);

                case 0x2f: // / Regexp
                    return ReadRegex();

                case 0x6f: // o Object
                    return ReadObject();

                case 0x43: // C Expend Object w/o attributes
                    return ReadExpendObjectBase();

                case 0x49: // I Expend Object
                    return ReadExpendObject();

                case 0x6c: // l Bignum
                    return ReadBignum();

                case 0x53: // S Struct
                    return ReadStruct();

                case 0x65: // e Extended Object
                    return ReadExtendedObject();

                case 0x6d: // m Module
                    return ReadModule();

                case 0x63: // c Class
                    return ReadClass();

                case 0x55: // U
                    return ReadUsingMarshalLoad();

                case 0x75: // u
                    return ReadUsingLoad();

                default:
                    throw new NotImplementedException("not implemented type identifier: " + id.ToString());
            }
        }

        private object ReadUsingLoad()
        {
            FuzzySymbol symbol = (FuzzySymbol)ReadAnObject();
            byte[] raw = ReadStringValueAsBytes();
            object obj;
            switch (symbol.GetString())
            {
                //case "Table":
                //    obj = new RGSSTable(raw);
                //    break;
                default:
                    obj = new FuzzyUserdefinedDumpObject()
                    {
                        ClassName = symbol,
                        DumpedObject = raw
                    };
                    break;
            }
            AddObject(obj);
            return obj;
        }

        private object ReadUsingMarshalLoad()
        {
            FuzzyUserdefinedMarshalDumpObject obj = new FuzzyUserdefinedMarshalDumpObject();
            AddObject(obj);
            obj.ClassName = (FuzzySymbol)ReadAnObject();
            obj.DumpedObject = ReadAnObject();
            return obj;
        }

        private FuzzyClass ReadClass()
        {
            FuzzyClass obj = FuzzyClass.GetClass(ReadStringValue());
            AddObject(obj);
            return obj;
        }

        private FuzzyModule ReadModule()
        {
            FuzzyModule module = FuzzyModule.GetModule(ReadStringValue());
            AddObject(module);
            return module;
        }

        private FuzzyExtendedObject ReadExtendedObject()
        {
            FuzzyExtendedObject extObj = new FuzzyExtendedObject();
            AddObject(extObj);
            extObj.ExtendedModule = FuzzyModule.GetModule(((FuzzySymbol)ReadAnObject()).GetString());
            extObj.BaseObject = ReadAnObject();
            return extObj;
        }

        private FuzzyStruct ReadStruct()
        {
            FuzzyStruct sobj = new FuzzyStruct();
            AddObject(sobj);
            sobj.ClassName = (FuzzySymbol)ReadAnObject();
            int sobjcount = ReadInt32();
            for (int i = 0; i < sobjcount; i++)
            {
                sobj.InstanceVariable[(FuzzySymbol)ReadAnObject()] = ReadAnObject();
            }
            return sobj;
        }

        private FuzzyBignum ReadBignum()
        {
            int sign = 0;
            switch (m_reader.ReadByte())
            {
                case 0x2b:
                    sign = 1;
                    break;

                case 0x2d:
                    sign = -1;
                    break;

                default:
                    sign = 0;
                    break;
            }
            int num3 = ReadInt32();
            int index = num3 / 2;
            int num5 = (num3 + 1) / 2;
            uint[] data = new uint[num5];
            for (int i = 0; i < index; i++)
            {
                data[i] = m_reader.ReadUInt32();
            }
            if (index != num5)
            {
                data[index] = m_reader.ReadUInt16();
            }
            FuzzyBignum bignum = new FuzzyBignum(sign, data);
            this.AddObject(bignum);
            return bignum;
        }

        private FuzzyExpendObject ReadExpendObjectBase()
        {
            FuzzyExpendObject expendobject = new FuzzyExpendObject();
            expendobject.ClassName = (FuzzySymbol)ReadAnObject();
            expendobject.BaseObject = ReadAnObject();
            return expendobject;
        }

        private object ReadExpendObject()
        {
            FuzzyExpendObject expendobject = new FuzzyExpendObject();
            int id = m_objects.Count;
            AddObject(expendobject);
            int type = m_reader.PeekChar();
            switch (type)
            {
                case 0x22: // " String
                    m_reader.ReadByte();
                    FuzzyString str = ReadString(false);
                    expendobject.BaseObject = str;
                    break;

                case 0x3a: // : Symbol
                    m_reader.ReadByte();
                    FuzzySymbol symbol = FuzzySymbol.GetSymbol(ReadString(false));
                    if (!m_symbols.Contains(symbol))
                        m_symbols.Add(symbol);
                    expendobject.BaseObject = symbol;
                    break;

                case 0x2f: // / Regexp
                    m_reader.ReadByte();
                    expendobject.BaseObject = ReadRegex(false);
                    break;

                case 0x43: // C Expend Object w/o attributes
                    m_reader.ReadByte();
                    var bas = ReadExpendObjectBase();
                    expendobject.BaseObject = bas.BaseObject;
                    expendobject.ClassName = bas.ClassName;
                    break;

                default:
                    expendobject.BaseObject = ReadAnObject();
                    break;
            }
            int expendobjectcount = ReadInt32();
            for (int i = 0; i < expendobjectcount; i++)
            {
                expendobject.InstanceVariable[(FuzzySymbol)ReadAnObject()] = ReadAnObject();
            }

            Encoding e = null;
            if (expendobject.InstanceVariable["E"] is bool)
            {
                if ((bool)(expendobject.InstanceVariable["E"]) == true)
                    e = Encoding.UTF8;
                else
                    e = Encoding.Default;

                expendobject.InstanceVariables.Remove(FuzzySymbol.GetSymbol("E"));
            }
            if (expendobject.InstanceVariable["encoding"] != null && expendobject.InstanceVariable["encoding"] is FuzzyString)
            {
                e = Encoding.GetEncoding((expendobject.InstanceVariable["encoding"] as FuzzyString).Text);

                expendobject.InstanceVariables.Remove(FuzzySymbol.GetSymbol("encoding"));
            }
            if (e != null)
            {
                if (expendobject.BaseObject is FuzzyString)
                    (expendobject.BaseObject as FuzzyString).Encoding = e;
                else if (expendobject.BaseObject is FuzzyRegexp)
                    (expendobject.BaseObject as FuzzyRegexp).Pattern.Encoding = e;
                else if (expendobject.BaseObject is FuzzySymbol)
                    (expendobject.BaseObject as FuzzySymbol).GetRubyString().Encoding = e;
            }
            if (expendobject.InstanceVariables.Count == 0 && (expendobject.BaseObject is FuzzyString || expendobject.BaseObject is FuzzyRegexp || expendobject.BaseObject is FuzzySymbol))
            {
                m_objects[id] = expendobject.BaseObject;
                return expendobject.BaseObject;
            }
            return expendobject;
        }

        private FuzzyObject ReadObject()
        {
            FuzzyObject robj = new FuzzyObject();
            AddObject(robj);
            robj.ClassName = (FuzzySymbol)ReadAnObject();
            int robjcount = ReadInt32();
            for (int i = 0; i < robjcount; i++)
            {
                robj.InstanceVariable[(FuzzySymbol)ReadAnObject()] = ReadAnObject();
            }
            return robj;
        }

        private FuzzyRegexp ReadRegex()
        {
            return ReadRegex(true);
        }

        private FuzzyRegexp ReadRegex(bool count)
        {
            FuzzyString ptn = ReadString();
            int opt = m_reader.ReadByte();
            FuzzyRegexp exp = new FuzzyRegexp(ptn, (FuzzyRegexpOptions)opt);
            if (count) AddObject(exp);
            return exp;
        }

        private FuzzyHash ReadHash(bool hasDefaultValue)
        {
            FuzzyHash hash = new FuzzyHash();
            AddObject(hash);
            int hashcount = ReadInt32();
            for (int i = 0; i < hashcount; i++)
            {
                hash[ReadAnObject()] = ReadAnObject();
            }
            if (hasDefaultValue)
                hash.DefaultValue = ReadAnObject();
            return hash;
        }

        private List<object> ReadArray()
        {
            List<object> array = new List<object>();
            AddObject(array);
            int arraycount = ReadInt32();
            for (int i = 0; i < arraycount; i++)
            {
                array.Add(ReadAnObject());
            }
            return array;
        }

        private FuzzySymbol ReadSymbol()
        {
            FuzzySymbol symbol = FuzzySymbol.GetSymbol(ReadStringValue());
            if (!m_symbols.Contains(symbol))
                m_symbols.Add(symbol);
            return symbol;
        }

        private FuzzyFloat ReadFloat()
        {
            string floatstr = ReadStringValue();
            double floatobj;
            if (floatstr == "inf")
                floatobj = double.PositiveInfinity;
            else if (floatstr == "-inf")
                floatobj = double.NegativeInfinity;
            else if (floatstr == "nan")
                floatobj = double.NaN;
            else
            {
                if (floatstr.Contains("\0"))
                {
                    floatstr = floatstr.Remove(floatstr.IndexOf("\0"));
                }
                floatobj = Convert.ToDouble(floatstr);
            }
            var fobj = new FuzzyFloat(floatobj);
            AddObject(fobj);
            return fobj;
        }

        private FuzzyString ReadString()
        {
            return ReadString(true);
        }

        private FuzzyString ReadString(bool count)
        {
            FuzzyString str = new FuzzyString(ReadStringValueAsBytes());
            if (str.Raw.Length > 2 && str.Raw[0] == 120 && str.Raw[1] == 156)
                str.Encoding = null;
            else
                str.Encoding = Encoding.UTF8;

            if (count) AddObject(str);
            return str;
        }

        public string ReadStringValue()
        {
            int count = ReadInt32();
            return Encoding.UTF8.GetString(m_reader.ReadBytes(count));
        }

        public byte[] ReadStringValueAsBytes()
        {
            int count = ReadInt32();
            return m_reader.ReadBytes(count);
        }

        public int ReadInt32()
        {
            sbyte num = m_reader.ReadSByte();
            if (num <= -5)
                return num + 5;
            if (num < 0)
            {
                int output = 0;
                for (int i = 0; i < -num; i++)
                {
                    output += (0xff - m_reader.ReadByte()) << (8*i);
                }
                return (-output - 1);
            }
            if (num == 0)
                return 0;
            if (num <= 4)
            {
                int output = 0;
                for (int i = 0; i < num; i++)
                {
                    output += m_reader.ReadByte() << (8*i);
                }
                return output;
            }
            return (num - 5);
        }
    }
}
