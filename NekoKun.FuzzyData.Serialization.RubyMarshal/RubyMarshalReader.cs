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
        private Dictionary<int, object> m_objects;
        private Dictionary<int, FuzzySymbol> m_symbols;
        private Dictionary<object, object> m_compat_tbl;
        private Converter<object, object> m_proc;

        public RubyMarshalReader(Stream input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("instance of IO needed");
            }
            if (!input.CanRead)
            {
                throw new ArgumentException("instance of IO needed");
            }
            this.m_stream = input;
            this.m_objects = new Dictionary<int, object>();
            this.m_symbols = new Dictionary<int, FuzzySymbol>();
            this.m_proc = null;
            this.m_compat_tbl = new Dictionary<object, object>();
            this.m_reader = new BinaryReader(m_stream, Encoding.ASCII);
        }

        public object Load()
        {
            int major = ReadByte();
            int minor = ReadByte();
            if (major != RubyMarshal.MarshalMajor || minor > RubyMarshal.MarshalMinor) {
                throw new InvalidDataException(string.Format("incompatible marshal file format (can't be read)\n\tformat version {0}.{1} required; {2}.{3} given", RubyMarshal.MarshalMajor, RubyMarshal.MarshalMinor, major, minor));
            }
            return ReadObject();
        }

        /// <summary>
        /// static int r_byte(struct load_arg *arg)
        /// </summary>
        /// <returns></returns>
        public int ReadByte()
        {
            return this.m_stream.ReadByte();
        }

        /// <summary>
        /// static long r_long(struct load_arg *arg)
        /// </summary>
        /// <returns></returns>
        public int ReadLong()
        {
            sbyte num = m_reader.ReadSByte();
            if (num <= -5)
                return num + 5;
            if (num < 0)
            {
                int output = 0;
                for (int i = 0; i < -num; i++)
                {
                    output += (0xff - m_reader.ReadByte()) << (8 * i);
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
                    output += m_reader.ReadByte() << (8 * i);
                }
                return output;
            }
            return (num - 5);
        }

        /// <summary>
        /// static VALUE r_bytes0(long len, struct load_arg *arg)
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public byte[] ReadBytes0(int len)
        {
            return this.m_reader.ReadBytes(len);
        }

        /// <summary>
        /// #define r_bytes(arg) r_bytes0(r_long(arg), (arg))
        /// </summary>
        /// <returns></returns>
        public byte[] ReadBytes()
        {
            return ReadBytes0(ReadLong());
        }

        /// <summary>
        /// static ID r_symlink(struct load_arg *arg)
        /// </summary>
        /// <returns></returns>
        public FuzzySymbol ReadSymbolLink()
        {
            int num = ReadLong();
            if (num >= this.m_symbols.Count)
                throw new InvalidDataException("bad symbol");
            return this.m_symbols[num];
        }

        /// <summary>
        /// static ID r_symreal(struct load_arg *arg, int ivar)
        /// </summary>
        /// <param name="ivar"></param>
        /// <returns></returns>
        public FuzzySymbol ReadSymbolReal(bool ivar)
        {
            byte[] s = ReadBytes();
            int n = m_symbols.Count;
            FuzzySymbol id;
            Encoding idx = Encoding.UTF8;
            m_symbols.Add(n, null);
            if (ivar)
            {
                int num = ReadLong();
                while (num-- > 0)
                {
                    id = ReadSymbol();
                    idx = GetEncoding(id, ReadObject());
                }
            }
            FuzzyString str = new FuzzyString(s, idx);
            id = FuzzySymbol.GetSymbol(str);
            m_symbols[n] = id;
            return id;
        }
        /// <summary>
        /// static int id2encidx(ID id, VALUE val)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public Encoding GetEncoding(FuzzySymbol id, object val)
        {
            if (id == RubyMarshal.IDs.encoding)
            {
                return Encoding.GetEncoding(((FuzzyString)val).Text);
            }
            else if (id == RubyMarshal.IDs.E)
            {
                if ((val is bool) && ((bool)val == false))
                    return null;
                if ((val is bool) && ((bool)val == true))
                    return Encoding.UTF8;
            }
            return null;
        }

        /// <summary>
        /// static ID r_symbol(struct load_arg *arg)
        /// </summary>
        /// <returns></returns>
        public FuzzySymbol ReadSymbol()
        {
            int type;
            bool ivar = false;
            again:
            switch (type = ReadByte())
            {
                case RubyMarshal.Types.InstanceVariable:
                    ivar = true;
                    goto again;
                case RubyMarshal.Types.Symbol:
                    return ReadSymbolReal(ivar);
                case RubyMarshal.Types.SymbolLink:
                    if (ivar)
                        throw new InvalidDataException("dump format error (symlink with encoding)");
                    return ReadSymbolLink();
                default:
                    throw new InvalidDataException(String.Format("dump format error for symbol(0x{0:X2})", type));
            }
        }
        /// <summary>
        /// static VALUE r_unique(struct load_arg *arg)
        /// </summary>
        /// <returns></returns>
        public FuzzySymbol ReadUnique()
        {
            return ReadSymbol();
        }

        /// <summary>
        /// static VALUE r_string(struct load_arg *arg)
        /// </summary>
        /// <returns></returns>
        public FuzzyString ReadString()
        {
            return new FuzzyString(ReadBytes());
        }

        /// <summary>
        /// static VALUE r_entry0(VALUE v, st_index_t num, struct load_arg *arg)
        /// </summary>
        /// <param name="v"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public object Entry0(object v, int num)
        {
            object real_obj = null;
            if (this.m_compat_tbl.TryGetValue(v, out real_obj))
            {
                if (this.m_objects.ContainsKey(num))
                    this.m_objects[num] = real_obj;
                else
                    this.m_objects.Add(num, real_obj);
            }
            else
            {
                if (this.m_objects.ContainsKey(num))
                    this.m_objects[num] = v;
                else
                    this.m_objects.Add(num, v);
            }
            return v;
        }

        /// <summary>
        /// #define r_entry(v, arg) r_entry0((v), (arg)->data->num_entries, (arg))
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public object Entry(object v)
        {
            return Entry0(v, m_objects.Count);
        }

        /// <summary>
        /// static VALUE r_leave(VALUE v, struct load_arg *arg)
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public object Leave(object v)
        {
            object data;
            if (this.m_compat_tbl.TryGetValue(v, out data))
            {
                object real_obj = data;
                object key = v;
                // TODO: 实现 MarshalCompat
                // if (st_lookup(compat_allocator_tbl, (st_data_t)allocator, &data)) {
                //   marshal_compat_t *compat = (marshal_compat_t*)data;
                //   compat->loader(real_obj, v);
                // }
                this.m_compat_tbl.Remove(key);
                v = real_obj;
            }
            if (this.m_proc != null)
            {
                v = this.m_proc(v);
            }
            return v;
        }

        /// <summary>
        /// static void r_ivar(VALUE obj, int *has_encoding, struct load_arg *arg)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="has_encoding"></param>
        public void ReadInstanceVariable(object obj, ref bool has_encoding)
        {
            int len = ReadLong();
            FuzzyObject fobj = obj as FuzzyObject;
            if (len > 0)
            {
                do
                {
                    FuzzySymbol id = ReadSymbol();
                    object val = ReadObject();
                    Encoding idx = GetEncoding(id, val);
                    if (idx != null)
                    {
                        if (fobj != null)
                            fobj.Encoding = idx;
                        has_encoding = true;
                    }
                    else
                    {
                        if (fobj != null)
                            fobj.InstanceVariable[id] = val;
                    }
                } while (--len > 0);
            }
        }

        /// <summary>
        /// static VALUE append_extmod(VALUE obj, VALUE extmod)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="extmod"></param>
        /// <returns></returns>
        public object AppendExtendedModule(object obj, List<FuzzyModule> extmod)
        {
            FuzzyObject fobj = obj as FuzzyObject;
            if (fobj != null)
                fobj.ExtendModules.AddRange(extmod);
            return obj;
        }

        /// <summary>
        /// static VALUE r_object(struct load_arg *arg)
        /// </summary>
        /// <returns></returns>
        public object ReadObject()
        {
            bool ivp = false;
            return ReadObject0(false, ref ivp, null);
        }

        public object ReadObject0(ref bool ivp, List<FuzzyModule> extmod)
        {
            return ReadObject0(true, ref ivp, extmod);
        }

        public object ReadObject0(List<FuzzyModule> extmod)
        {
            bool ivp = false;
            return ReadObject0(false, ref ivp, extmod);
        }

        /// <summary>
        /// static VALUE r_object0(struct load_arg *arg, int *ivp, VALUE extmod)
        /// </summary>
        /// <param name="hasivp"></param>
        /// <param name="ivp"></param>
        /// <param name="extmod"></param>
        /// <returns></returns>
        public object ReadObject0(bool hasivp, ref bool ivp, List<FuzzyModule> extmod)
        {
            object v = null;
            int type = ReadByte();
            int id;
            object link;
            switch (type)
            {
                case RubyMarshal.Types.Link:
                    id = ReadLong();
                    if (!this.m_objects.TryGetValue(id, out link))
                    {
                        throw new InvalidDataException("dump format error (unlinked)");
                    }
                    v = link;
                    if (this.m_proc != null)
                        v = this.m_proc(v);
                    break;
                case RubyMarshal.Types.InstanceVariable:
                    {
                        bool ivar = true;
                        v = ReadObject0(ref ivar, extmod);
                        bool hasenc = false;
                        if (ivar) ReadInstanceVariable(v, ref hasenc);
                    }
                    break;
                case RubyMarshal.Types.Extended:
                    {
                        FuzzyModule m = FuzzyModule.GetModule(ReadUnique());
                        if (extmod == null)
                            extmod = new List<FuzzyModule>();
                        extmod.Add(m);
                        v = ReadObject0(extmod);
                        FuzzyObject fobj = v as FuzzyObject;
                        if (fobj != null)
                        {
                            fobj.ExtendModules.AddRange(extmod);
                        }
                    }
                    break;
                default:
                    throw new InvalidDataException(string.Format("dump format error(0x{0:X2})", type));
            }
            return v;
        }

        /*
static VALUE r_object(struct load_arg *arg)
{
    return r_object0(arg, 0, Qnil);
}
*/


        /*
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
            
        }
        */
    }
}
