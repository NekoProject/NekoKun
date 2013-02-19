using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace NekoKun.Serialization.RubyMarshal
{
    class RubyMarshalReader
    {
        private Stream m_stream;
        private BinaryReader m_reader;
        private Dictionary<int, object> m_objects;
        private Dictionary<int, RubySymbol> m_symbols;
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
            this.m_symbols = new Dictionary<int, RubySymbol>();
            this.m_proc = null;
            this.m_compat_tbl = new Dictionary<object, object>();
            this.m_reader = new BinaryReader(m_stream, Encoding.ASCII);
        }

        public object Load()
        {
            int major = ReadByte();
            int minor = ReadByte();
            if (major != RubyMarshal.MarshalMajor || minor > RubyMarshal.MarshalMinor)
            {
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
        public RubySymbol ReadSymbolLink()
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
        public RubySymbol ReadSymbolReal(bool ivar)
        {
            byte[] s = ReadBytes();
            int n = m_symbols.Count;
            RubySymbol id;
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
            RubyString str = new RubyString(s, idx);
            id = RubySymbol.GetSymbol(str);
            m_symbols[n] = id;
            return id;
        }
        /// <summary>
        /// static int id2encidx(ID id, VALUE val)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public Encoding GetEncoding(RubySymbol id, object val)
        {
            if (id == RubyMarshal.IDs.encoding)
            {
                return Encoding.GetEncoding(((RubyString)val).Text);
            }
            else if (id == RubyMarshal.IDs.E)
            {
                if ((val is bool) && ((bool)val == false))
                    return Encoding.Default;
                if ((val is bool) && ((bool)val == true))
                    return Encoding.UTF8;
            }
            return null;
        }

        /// <summary>
        /// static ID r_symbol(struct load_arg *arg)
        /// </summary>
        /// <returns></returns>
        public RubySymbol ReadSymbol()
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
        public RubySymbol ReadUnique()
        {
            return ReadSymbol();
        }

        /// <summary>
        /// static VALUE r_string(struct load_arg *arg)
        /// </summary>
        /// <returns></returns>
        public RubyString ReadString()
        {
            byte[] raw = ReadBytes();
            RubyString v = new RubyString(raw);

            var decoder = Encoding.UTF8.GetDecoder();
            
            decoder.Fallback = System.Text.DecoderFallback.ExceptionFallback;
            try
            {
                decoder.GetCharCount(raw, 0, raw.Length);
                v.Encoding = Encoding.UTF8;
            }
            catch
            {
                v.Encoding = Encoding.Default;
            }

            return v;
        }

        /// <summary>
        /// static st_index_t r_prepare(struct load_arg *arg)
        /// </summary>
        /// <returns></returns>
        public int Prepare()
        {
            int idx = this.m_objects.Count;
            this.m_objects.Add(idx, null);
            return idx;
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
            RubyObject fobj = obj as RubyObject;
            if (len > 0)
            {
                do
                {
                    RubySymbol id = ReadSymbol();
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
        public void ReadInstanceVariable(object obj)
        {
            bool e = false;
            ReadInstanceVariable(obj, ref e);
        }

        /// <summary>
        /// static VALUE append_extmod(VALUE obj, VALUE extmod)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="extmod"></param>
        /// <returns></returns>
        public object AppendExtendedModule(object obj, List<RubyModule> extmod)
        {
            RubyObject fobj = obj as RubyObject;
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

        public object ReadObject0(ref bool ivp, List<RubyModule> extmod)
        {
            return ReadObject0(true, ref ivp, extmod);
        }

        public object ReadObject0(List<RubyModule> extmod)
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
        public object ReadObject0(bool hasivp, ref bool ivp, List<RubyModule> extmod)
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
                        RubyModule m = RubyModule.GetModule(ReadUnique());
                        if (extmod == null)
                            extmod = new List<RubyModule>();
                        extmod.Add(m);
                        v = ReadObject0(extmod);
                        RubyObject fobj = v as RubyObject;
                        if (fobj != null)
                        {
                            fobj.ExtendModules.AddRange(extmod);
                        }
                    }
                    break;
                case RubyMarshal.Types.UserClass:
                    {
                        RubyClass c = RubyClass.GetClass(ReadUnique());

                        v = ReadObject0(extmod);
                        if (v is RubyObject)
                            (v as RubyObject).ClassName = c.Symbol;
                    }
                    break;
                case RubyMarshal.Types.Nil:
                    v = RubyNil.Instance;
                    v = Leave(v);
                    break;
                case RubyMarshal.Types.True:
                    v = true;
                    v = Leave(v);
                    break;
                case RubyMarshal.Types.False:
                    v = false;
                    v = Leave(v);
                    break;
                case RubyMarshal.Types.Fixnum:
                    v = ReadLong();
                    v = Leave(v);
                    break;
                case RubyMarshal.Types.Float:
                    {
                        double d;
                        RubyString fstr = ReadString();
                        string str = fstr.Text;

                        if (str == "inf")
                            d = double.PositiveInfinity;
                        else if (str == "-inf")
                            d = double.NegativeInfinity;
                        else if (str == "nan")
                            d = double.NaN;
                        else
                        {
                            if (str.Contains("\0"))
                            {
                                str = str.Remove(str.IndexOf("\0"));
                            }
                            d = Convert.ToDouble(str);
                        }
                        v = new RubyFloat(d);
                        v = Entry(v);
                        v = Leave(v);
                    }
                    break;
                case RubyMarshal.Types.Bignum:
                    {
                        int sign = 0;
                        switch (ReadByte())
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
                        int num3 = ReadLong();
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
                        v = new RubyBignum(sign, data);
                        v = Entry(v);
                        v = Leave(v);
                    }
                    break;
                case RubyMarshal.Types.String:
                    v = Entry(ReadString());
                    v = Leave(v);
                    break;
                case RubyMarshal.Types.Regexp:
                    {
                        RubyString str = ReadString();
                        int options = ReadByte();
                        bool has_encoding = false;
                        int idx = Prepare();
                        if (hasivp)
                        {
                            ReadInstanceVariable(str, ref has_encoding);
                            ivp = false;
                        }
                        if (!has_encoding)
                        {
                            // TODO: 1.8 compatibility; remove escapes undefined in 1.8
                            /*
                            char *ptr = RSTRING_PTR(str), *dst = ptr, *src = ptr;
                            long len = RSTRING_LEN(str);
                            long bs = 0;
                            for (; len-- > 0; *dst++ = *src++) {
                                switch (*src) {
                                    case '\\': bs++; break;
                                    case 'g': case 'h': case 'i': case 'j': case 'k': case 'l':
                                    case 'm': case 'o': case 'p': case 'q': case 'u': case 'y':
                                    case 'E': case 'F': case 'H': case 'I': case 'J': case 'K':
                                    case 'L': case 'N': case 'O': case 'P': case 'Q': case 'R':
                                    case 'S': case 'T': case 'U': case 'V': case 'X': case 'Y':
                                    if (bs & 1) --dst;
                                    default: bs = 0; break;
                                }
                            }
                            rb_str_set_len(str, dst - ptr);
                            */
                        }
                        v = Entry0(new RubyRegexp(str, (RubyRegexpOptions)options), idx);
                        v = Leave(v);
                    }
                    break;
                case RubyMarshal.Types.Array:
                    {
                        int len = ReadLong();
                        RubyArray ary = new RubyArray();
                        v = ary;
                        v = Entry(v);
                        while (len-- > 0)
                        {
                            ary.Push(ReadObject());
                        }
                        v = Leave(v);
                    }
                    break;
                case RubyMarshal.Types.Hash:
                case RubyMarshal.Types.HashWithDefault:
                    {
                        int len = ReadLong();
                        RubyHash hash = new RubyHash();
                        v = hash;
                        v = Entry(v);
                        while (len-- > 0)
                        {
                            object key = ReadObject();
                            object value = ReadObject();
                            hash.Add(key, value);
                        }
                        if (type == RubyMarshal.Types.HashWithDefault)
                        {
                            hash.DefaultValue = ReadObject();
                        }
                        v = Leave(v);
                    }
                    break;
                case RubyMarshal.Types.Struct:
                    {
                        int idx = Prepare();
                        RubyStruct obj = new RubyStruct();
                        RubySymbol klass = ReadUnique();
                        obj.ClassName = klass;
                        int len = ReadLong();
                        v = obj;
                        v = Entry0(v, idx);
                        while (len-- > 0)
                        {
                            RubySymbol key = ReadSymbol();
                            object value = ReadObject();
                            obj.InstanceVariable[key] = value;
                        }
                        v = Leave(v);
                    }
                    break;
                case RubyMarshal.Types.UserDefined:
                    {
                        RubySymbol klass = ReadUnique();
                        RubyString data = ReadString();
                        if (hasivp)
                        {
                            ReadInstanceVariable(data);
                            ivp = false;
                        }
                        RubyUserdefinedDumpObject obj = new RubyUserdefinedDumpObject();
                        obj.ClassName = klass;
                        obj.DumpedObject = data.Raw;
                        v = obj;
                        v = Entry(v);
                        v = Leave(v);
                    }
                    break;
                case RubyMarshal.Types.UserMarshal:
                    {
                        RubySymbol klass = ReadUnique();
                        FuzzyUserdefinedMarshalDumpObject obj = new FuzzyUserdefinedMarshalDumpObject();
                        v = obj;
                        if (extmod != null)
                            AppendExtendedModule(obj, extmod);
                        v = Entry(v);
                        object data = ReadObject();
                        obj.ClassName = klass;
                        obj.DumpedObject = data;
                        v = Leave(v);
                        if (extmod != null)
                        {
                            extmod.Clear();
                        }
                    }
                    break;
                case RubyMarshal.Types.Object:
                    {
                        int idx = Prepare();
                        RubyObject obj = new RubyObject();
                        RubySymbol klass = ReadUnique();
                        obj.ClassName = klass;
                        v = obj;
                        v = Entry0(v, idx);
                        ReadInstanceVariable(v);
                        v = Leave(v);
                    }
                    break;
                case RubyMarshal.Types.Class:
                    {
                        RubyString str = ReadString();
                        v = RubyClass.GetClass(RubySymbol.GetSymbol(str));
                        v = Entry(v);
                        v = Leave(v);
                    }
                    break;
                case RubyMarshal.Types.Module:
                    {
                        RubyString str = ReadString();
                        v = RubyModule.GetModule(RubySymbol.GetSymbol(str));
                        v = Entry(v);
                        v = Leave(v);
                    }
                    break;
                case RubyMarshal.Types.Symbol:
                    if (hasivp)
                    {
                        v = ReadSymbolReal(ivp);
                        ivp = false;
                    }
                    else
                    {
                        v = ReadSymbolReal(false);
                    }
                    v = Leave(v);
                    break;
                case RubyMarshal.Types.SymbolLink:
                    v = ReadSymbolLink();
                    break;
                case RubyMarshal.Types.Data:
                /*  TODO: Data Support
                    {
                        VALUE klass = path2class(r_unique(arg));
                        VALUE oldclass = 0;

                        v = obj_alloc_by_klass(klass, arg, &oldclass);
                        if (!RB_TYPE_P(v, T_DATA)) {
                            rb_raise(rb_eArgError, "dump format error");
                        }
                        v = r_entry(v, arg);
                        if (!rb_respond_to(v, s_load_data)) {
                            rb_raise(rb_eTypeError, "class %s needs to have instance method `_load_data'", rb_class2name(klass));
                        }
                        rb_funcall(v, s_load_data, 1, r_object0(arg, 0, extmod));
                        check_load_arg(arg, s_load_data);
                        v = r_leave(v, arg);
                    }
                 */
                case RubyMarshal.Types.ModuleOld:
                /*
                    TODO: ModuleOld Support
                    {
                        volatile VALUE str = r_bytes(arg);
                        v = rb_path_to_class(str);
                        v = r_entry(v, arg);
                        v = r_leave(v, arg);
                    }
                 */
                default:
                    throw new InvalidDataException(string.Format("dump format error(0x{0:X2})", type));
            }
            return v;
        }
    }
}
