using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace orzTech.NekoKun.ProjectEngines.RGSS
{
    class RubyMarshalReader
    {
        private Stream m_stream;
        private BinaryReader m_reader;
        private List<object> m_objects;
        private List<RubySymbol> m_symbols;
        private bool treatStringAsBytes = false;

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
            this.m_symbols = new List<RubySymbol>();
            this.m_reader = new BinaryReader(m_stream);
        }

        public bool TreatStringAsBytes
        {
            get { return treatStringAsBytes; }
            set { treatStringAsBytes = value; }
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
            byte id = m_reader.ReadByte();
            switch (id)
            {
                case 0x40: // @ Object Reference
                    return m_objects[ReadInt32()];

                case 0x3b: // ; Symbol Reference
                    return m_symbols[ReadInt32()];

                case 0x30: // 0 NilClass
                    return RubyNil.Instance;

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
            RubyUserdefinedDumpObject obj = new RubyUserdefinedDumpObject();
            AddObject(obj);
            obj.ClassName = (RubySymbol)ReadAnObject();
            obj.DumpedObject = ReadStringValueAsBytes();
            return obj;
        }

        private object ReadUsingMarshalLoad()
        {
            RubyUserdefinedMarshalDumpObject obj = new RubyUserdefinedMarshalDumpObject();
            AddObject(obj);
            obj.ClassName = (RubySymbol)ReadAnObject();
            obj.DumpedObject = ReadAnObject();
            return obj;
        }

        private RubyClass ReadClass()
        {
            RubyClass obj = RubyClass.GetClass(ReadStringValue());
            AddObject(obj);
            return obj;
        }

        private RubyModule ReadModule()
        {
            RubyModule module = RubyModule.GetModule(ReadStringValue());
            AddObject(module);
            return module;
        }

        private RubyExtendedObject ReadExtendedObject()
        {
            RubyExtendedObject extObj = new RubyExtendedObject();
            AddObject(extObj);
            extObj.ExtendedModule = RubyModule.GetModule(((RubySymbol)ReadAnObject()).GetString());
            extObj.BaseObject = ReadAnObject();
            return extObj;
        }

        private RubyStruct ReadStruct()
        {
            RubyStruct sobj = new RubyStruct();
            AddObject(sobj);
            sobj.ClassName = (RubySymbol)ReadAnObject();
            int sobjcount = ReadInt32();
            for (int i = 0; i < sobjcount; i++)
            {
                sobj[(RubySymbol)ReadAnObject()] = ReadAnObject();
            }
            return sobj;
        }

        private RubyBignum ReadBignum()
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
            RubyBignum bignum = new RubyBignum(sign, data);
            this.AddObject(bignum);
            return bignum;
        }

        private RubyExpendObject ReadExpendObjectBase()
        {
            RubyExpendObject expendobject = new RubyExpendObject();
            expendobject.ClassName = ReadSymbol();
            expendobject.BaseObject = ReadAnObject();
            return expendobject;
        }

        private RubyExpendObject ReadExpendObject()
        {
            RubyExpendObject expendobject = new RubyExpendObject();
            AddObject(expendobject);
            expendobject.BaseObject = ReadAnObject();
            if (expendobject.BaseObject is RubyExpendObject)
            {
                expendobject.ClassName = (expendobject.BaseObject as RubyExpendObject).ClassName;
                expendobject.BaseObject = (expendobject.BaseObject as RubyExpendObject).BaseObject;
            }
            int expendobjectcount = ReadInt32();
            for (int i = 0; i < expendobjectcount; i++)
            {
                expendobject[(RubySymbol)ReadAnObject()] = ReadAnObject();
            }
            return expendobject;
        }

        private RubyObject ReadObject()
        {
            RubyObject robj = new RubyObject();
            AddObject(robj);
            robj.ClassName = (RubySymbol)ReadAnObject();
            int robjcount = ReadInt32();
            for (int i = 0; i < robjcount; i++)
            {
                robj[(RubySymbol)ReadAnObject()] = ReadAnObject();
            }
            return robj;
        }

        private Regex ReadRegex()
        {
            string regexPattern = ReadStringValue();
            int regexOptionsRuby = m_reader.ReadByte();
            RegexOptions regexOptions = RegexOptions.None;
            if (regexOptionsRuby >= 4)
            {
                regexOptions |= RegexOptions.Multiline;
                regexOptionsRuby -= 4;
            }
            if (regexOptionsRuby >= 2)
            {
                regexOptions |= RegexOptions.IgnorePatternWhitespace;
                regexOptionsRuby -= 2;
            }
            if (regexOptionsRuby >= 1)
            {
                regexOptions |= RegexOptions.IgnoreCase;
                regexOptionsRuby -= 1;
            }
            Regex regex = new Regex(regexPattern, regexOptions);
            AddObject(regex);
            return regex;
        }

        private RubyHash ReadHash(bool hasDefaultValue)
        {
            RubyHash hash = new RubyHash();
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

        private RubySymbol ReadSymbol()
        {
            RubySymbol symbol = RubySymbol.GetSymbol(ReadStringValue());
            if (!m_symbols.Contains(symbol))
                m_symbols.Add(symbol);
            return symbol;
        }

        private double ReadFloat()
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
            AddObject(floatobj);
            return floatobj;
        }

        private object ReadString()
        {
            object str;
            if (!TreatStringAsBytes)
                str = ReadStringValue();
            else
                str = ReadStringValueAsBytes();
            AddObject(str);
            return str;
        }

        public string ReadStringValue()
        {
            int count = ReadInt32();
            byte[] bytes = m_reader.ReadBytes(count);
            byte[] buffer = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, bytes);
            return Encoding.Unicode.GetString(buffer);
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
