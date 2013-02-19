using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace NekoKun.Serialization.RubyMarshal
{
    class RubyMarshalWriter
    {
        private Stream m_stream;
        private BinaryWriter m_writer;
        private Dictionary<object, int> m_objects;
        private Dictionary<RubySymbol, int> m_symbols;
        private Dictionary<object, object> m_compat_tbl;

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
            this.m_objects = new Dictionary<object, int>();
            this.m_symbols = new Dictionary<RubySymbol, int>();
            this.m_compat_tbl = new Dictionary<object, object>();
            this.m_writer = new BinaryWriter(m_stream);
        }

        /// <summary>
        /// static void w_nbyte(const char *s, long n, struct dump_arg *arg)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="n"></param>
        public void WriteNByte(byte[] s, int n)
        {
            this.m_writer.Write(s, 0, n);
        }

        /// <summary>
        /// static void w_byte(char c, struct dump_arg *arg)
        /// </summary>
        /// <param name="c"></param>
        public void WriteByte(byte c)
        {
            this.m_writer.Write(c);
        }

        /// <summary>
        /// static void w_long(long x, struct dump_arg *arg)
        /// </summary>
        /// <param name="value"></param>
        public void WriteLong(int value)
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

        /// <summary>
        /// static void w_bytes(const char *s, long n, struct dump_arg *arg)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="n"></param>
        public void WriteBytes(byte[] s, int n)
        {
            WriteLong(n);
            WriteNByte(s, n);
        }

        public void WriteBytes(byte[] s)
        {
            WriteBytes(s, s.Length);
        }

        /// <summary>
        /// #define w_cstr(s, arg) w_bytes((s), strlen(s), (arg))
        /// </summary>
        /// <param name="s"></param>
        public void WriteCString(string s)
        {
            WriteLong(s.Length);
            this.m_writer.Write(Encoding.Default.GetBytes(s));
        }

        /// <summary>
        /// static void w_float(double d, struct dump_arg *arg)
        /// </summary>
        /// <param name="value"></param>
        public void WriteFloat(double value)
        {
            if (double.IsInfinity(value))
            {
                if (double.IsPositiveInfinity(value))
                {
                    WriteCString("inf");
                }
                else
                {
                    WriteCString("-inf");
                }
            }
            else if (double.IsNaN(value))
            {
                WriteCString("nan");
            }
            else
            {
                WriteCString(string.Format("{0:g}", value));
            }
        }

        public void WriteFloat(float value)
        {
            WriteFloat((double)value);
        }

        public void WriteFloat(RubyFloat value)
        {
            WriteFloat(value.Value);
        }

        /// <summary>
        /// static void w_symbol(ID id, struct dump_arg *arg)
        /// </summary>
        /// <param name="id"></param>
        public void WriteSymbol(RubySymbol id)
        {
            string sym;
            int num;
            System.Text.Encoding encidx = null;

            if (this.m_symbols.TryGetValue(id, out num)) {
	            WriteByte(RubyMarshal.Types.SymbolLink);
	            WriteLong(num);
            }
            else {
	            sym = id.Name;
	            if (sym.Length == 0) {
    	            throw new InvalidDataException("can't dump anonymous ID");
	            }
	            encidx = id.Encoding;
	            if (encidx == Encoding.ASCII || encidx == Encoding.Default || encidx == Encoding.UTF8) {
	                encidx = null;
	            }
	            if (encidx != null) {
	                WriteByte(RubyMarshal.Types.InstanceVariable);
	            }
	            WriteByte(RubyMarshal.Types.Symbol);
	            WriteCString(sym);

                this.m_symbols.Add(id, this.m_symbols.Count);
	            if (encidx != null) {
    	            WriteEncoding(id, 0);
	            }
            }
        }

        /// <summary>
        /// static void w_unique(VALUE s, struct dump_arg *arg)
        /// </summary>
        /// <param name="s"></param>
        public void WriteUnique(RubySymbol s)
        {
            WriteSymbol(s);
        }

        /// <summary>
        /// static void w_extended(VALUE klass, struct dump_arg *arg, int check)
        /// </summary>
        /// <param name="klass"></param>
        /// <param name="check"></param>
        public void WriteExtended(object klass, bool check)
        {
            RubyObject fobj = klass as RubyObject;
            if (fobj != null)
            {
                foreach (RubyModule item in fobj.ExtendModules)
                {
                    WriteByte(RubyMarshal.Types.Extended);
                    WriteUnique(item.Symbol);
                }
            }
        } 

        /// <summary>
        /// static void w_class(char type, VALUE obj, struct dump_arg *arg, int check)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        /// <param name="check"></param>
        public void WriteClass(byte type, object obj, bool check)
        {
            object real_obj;
            if (this.m_compat_tbl.TryGetValue(obj, out real_obj))
            {
                obj = real_obj;
            }
            RubyObject fobj = obj as RubyObject;
            if (fobj != null)
            {
                RubyClass klass = RubyClass.GetClass(fobj.ClassName);
                WriteExtended(klass, check);
                WriteByte(type);
                WriteUnique(fobj.ClassName);
            }
        }

        /// <summary>
        /// static void w_uclass(VALUE obj, VALUE super, struct dump_arg *arg)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="super"></param>
        public void WriteUserClass(object obj, RubyClass super)
        {
            RubyObject fobj = obj as RubyObject;
            if (fobj != null)
            {
                RubyClass klass = fobj.Class;
                WriteExtended(klass, true);
                if (klass != super)
                {
                    WriteByte(RubyMarshal.Types.UserClass);
                    WriteUnique(klass.Symbol);
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// static void w_encoding(VALUE obj, long num, struct dump_call_arg *arg)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="num"></param>
        public void WriteEncoding(object obj, int num)
        {
            Encoding encidx =null;
            if (obj is RubyObject)
                encidx = (obj as RubyObject).Encoding;

            if (encidx == null)
            {
                WriteLong(num);
                return;
            }
            WriteLong(num + 1);

            if (encidx == Encoding.Default)
            {
                /* special treatment for US-ASCII and UTF-8 */
	            WriteSymbol(RubyMarshal.IDs.E);
	            WriteObject(false);
	            return;
            }
            else if (encidx == Encoding.UTF8) {
	            WriteSymbol(RubyMarshal.IDs.E);
	            WriteObject(true);
	            return;
            }

            WriteSymbol(RubyMarshal.IDs.encoding);
            WriteObject(RubySymbol.GetSymbol(encidx.BodyName));
            
        } 

        /// <summary>
        /// static void w_ivar(VALUE obj, st_table *tbl, struct dump_call_arg *arg)
        /// </summary>
        /// <param name="obj"></param>
        public void WriteInstanceVariable(RubyObject obj, Dictionary<RubySymbol, object> tbl)
        {
            int num = tbl != null ? tbl.Count : 0;

            WriteEncoding(obj, num);
            if (tbl != null) {
                foreach (KeyValuePair<RubySymbol, object> item in tbl)
                {
                    if (item.Key == RubyMarshal.IDs.encoding) continue;
                    if (item.Key == RubyMarshal.IDs.E) continue;
                    WriteSymbol(item.Key);
                    WriteObject(item.Value);
                }
            }
        }

        /// <summary>
        /// static void w_objivar(VALUE obj, struct dump_call_arg *arg)
        /// </summary>
        /// <param name="obj"></param>
        public void WriteObjectInstanceVariable(RubyObject obj)
        {
            WriteInstanceVariable(obj, obj.InstanceVariables);
        }  

        /// <summary>
        /// static void w_object(VALUE obj, struct dump_arg *arg, int limit)
        /// </summary>
        /// <param name="obj"></param>
        public void WriteObject(object obj)
        {
            int num;
            if (this.m_objects.TryGetValue(obj, out num))
            {
                WriteByte(RubyMarshal.Types.Link);
                WriteLong(num);
                return;
            }
            if (obj == null || obj == RubyNil.Instance)
            {
                WriteByte(RubyMarshal.Types.Nil);
            }
            else if (obj is bool && (bool)obj == true)
                WriteByte(RubyMarshal.Types.True);
            else if (obj is bool && (bool)obj == false)
                WriteByte(RubyMarshal.Types.False);
            else if (obj is int) {
                int v = (int) obj;
                // (2**30).class   => Bignum
                // (2**30-1).class => Fixnum
                // (-2**30-1).class=> Bignum
                // (-2**30).class  => Fixnum
                if (v <= 1073741823 && v >= -1073741824)
                {
                    WriteByte(RubyMarshal.Types.Fixnum);
                    WriteLong(v);
                }
                else
                {
                    WriteObject(RubyBignum.Create(v));
                }
            }
            else if (obj is RubySymbol) {
                WriteSymbol(obj as RubySymbol);
            }
            else {
                RubyObject fobj = obj as RubyObject;
                bool hasiv = false;
                if (fobj != null)
                    hasiv = (obj as RubyObject).InstanceVariables.Count > 0 || fobj.Encoding != null;

	            if (obj is IRubyUserdefinedMarshalDumpObject)
                {
                    this.m_objects.Add(obj, this.m_objects.Count);
                    object result = (obj as IRubyUserdefinedMarshalDumpObject).Dump();
                    if (hasiv)
                        WriteByte(RubyMarshal.Types.InstanceVariable);
                    WriteClass(RubyMarshal.Types.UserMarshal, obj, false);
                    WriteObject(result);
                    if (hasiv)
                        WriteObjectInstanceVariable(fobj);
                    return;
                }
                if (obj is IRubyUserdefinedDumpObject)
                {
                    byte[] result = (obj as IRubyUserdefinedDumpObject).Dump();
                    if (hasiv)
                        WriteByte(RubyMarshal.Types.InstanceVariable);
                    WriteClass(RubyMarshal.Types.UserDefined, obj, false);
                    WriteBytes(result, result.Length);
                    if (hasiv)
                        WriteObjectInstanceVariable(fobj);
                    this.m_objects.Add(obj, this.m_objects.Count);
                    return;
                }
                this.m_objects.Add(obj, this.m_objects.Count);

                /*
                {
                    st_data_t compat_data;
                    rb_alloc_func_t allocator = rb_get_alloc_func(RBASIC(obj)->klass);
                    if (st_lookup(compat_allocator_tbl,
                                  (st_data_t)allocator,
                                  &compat_data)) {
                        marshal_compat_t *compat = (marshal_compat_t*)compat_data;
                        VALUE real_obj = obj;
                        obj = compat->dumper(real_obj);
                        st_insert(arg->compat_tbl, (st_data_t)obj, (st_data_t)real_obj);
		        if (obj != real_obj && !ivtbl) hasiv = 0;
                    }
                }*/

                if (hasiv) 
                    WriteByte(RubyMarshal.Types.InstanceVariable);

                if (obj is RubyClass)
                {
                    WriteByte(RubyMarshal.Types.Class);
                    WriteCString((obj as RubyClass).Name);
                }
                else if (obj is RubyModule)
                {
                    WriteByte(RubyMarshal.Types.Module);
                    WriteCString((obj as RubyModule).Name);
                }
                else if (obj is float)
                {
                    WriteByte(RubyMarshal.Types.Float);
                    WriteFloat((float)obj);
                }
                else if (obj is double)
                {
                    WriteByte(RubyMarshal.Types.Float);
                    WriteFloat((double)obj);
                }
                else if (obj is RubyFloat)
                {
                    WriteByte(RubyMarshal.Types.Float);
                    WriteFloat((RubyFloat)obj);
                }
                else if (obj is RubyBignum)
                {
                    RubyBignum value = (RubyBignum) obj;
                    char ch;
                    if (value.Sign > 0)
                        ch = '+';
                    else if (value.Sign < 0)
                        ch = '-';
                    else
                        ch = '0';
                    this.m_writer.Write((byte)ch);
                    uint[] words = value.GetWords();
                    int num2 = words.Length * 2;
                    int index = words.Length - 1;
                    bool flag = false;
                    if ((words.Length > 0) && ((words[index] >> 0x10) == 0))
                    {
                        num--;
                        flag = true;
                    }
                    this.WriteLong(num2);
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
                else if (obj is RubyString || obj is string)
                {
                    RubyString v;
                    if (obj is string)
                        v = new RubyString(obj as string);
                    else
                        v = (RubyString)obj;
                    WriteUserClass(v, RubyClass.GetClass("String"));
                    WriteByte(RubyMarshal.Types.String);
                    WriteBytes(v.Raw);
                }
                else if (obj is RubyRegexp)
                {
                    RubyRegexp v = (RubyRegexp) obj;
                    WriteUserClass(obj, RubyClass.GetClass("Regexp"));
                    WriteByte(RubyMarshal.Types.Regexp);
                    WriteBytes(v.Pattern.Raw);
                    WriteByte((byte)v.Options);
                }
                else if (obj is RubyArray || obj is List<object>)
                {
                    RubyArray v;
                    if (obj is List<object>)
                        v = new RubyArray(obj as List<object>);
                    else
                        v = (RubyArray)obj;
                    WriteUserClass(v, RubyClass.GetClass("Array"));
                    WriteByte(RubyMarshal.Types.Array);
                    WriteLong(v.Length);
                    for (int i = 0; i < v.Count; i++)
    			        WriteObject(v[i]);
                }
                else if (obj is RubyHash)
                {
                    RubyHash v = (RubyHash) obj;
                    WriteUserClass(obj, RubyClass.GetClass("Hash"));
                    WriteByte(v.DefaultValue != null ? RubyMarshal.Types.HashWithDefault : RubyMarshal.Types.Hash);
                    WriteLong(v.Length);
                    foreach (KeyValuePair<object, object> item in v)
	                {
                        WriteObject(item.Key);
                        WriteObject(item.Value); 
	                }
                    if (v.DefaultValue != null) WriteObject(v.DefaultValue);
                }
                else if (obj is RubyStruct)
                {
                    RubyStruct v = (RubyStruct) obj;
                    WriteUserClass(obj, RubyClass.GetClass("Struct"));
                    WriteLong(v.InstanceVariables.Count);
                    foreach (KeyValuePair<RubySymbol, object> item in v.InstanceVariables)
	                {
                        WriteObject(item.Key);
                        WriteObject(item.Value); 
	                }
                }
                else if (obj is RubyObject)
                {
                    WriteClass(RubyMarshal.Types.Object, obj, true);
                    WriteObjectInstanceVariable((RubyObject) obj);
                }
                /* TODO: Data
	          case T_DATA:
	            {
		        VALUE v;

		        if (!rb_respond_to(obj, s_dump_data)) {
		            rb_raise(rb_eTypeError,
			             "no _dump_data is defined for class %s",
			             rb_obj_classname(obj));
		        }
		        v = rb_funcall(obj, s_dump_data, 0);
		        check_dump_arg(arg, s_dump_data);
		        w_class(TYPE_DATA, obj, arg, TRUE);
		        w_object(v, arg, limit);
	            }
	            break;*/
                else {
                    throw new InvalidDataException(string.Format("can't dump {0}", obj.GetType().FullName));
	            }
                if (hasiv)
                    WriteInstanceVariable(fobj, fobj.InstanceVariables);
            }
        }  

        public void Dump(object obj)
        {
            this.m_writer.Write((byte)4);
            this.m_writer.Write((byte)8);
            WriteObject(obj);
            this.m_stream.Flush();
        }

        /*
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
                else if (obj is RubyFloat)
                {
                    this.WriteFloat((RubyFloat)obj);
                }
                else if (obj is double)
                {
                    this.WriteFloat(new RubyFloat((double)obj));
                }
                else if (obj is float)
                {
                    this.WriteFloat(new RubyFloat((double)obj));
                }
                else if (obj is List<object>)
                {
                    this.WriteArray((List<object>)obj);
                }
                else if (obj is RubyHash)
                {
                    this.WriteHash((RubyHash)obj);
                }
                else if (obj is RubyRegexp)
                {
                    if ((obj as RubyRegexp).Pattern.Encoding != null)
                    {
                        FuzzyExpendObject ooo = new FuzzyExpendObject();
                        ooo.BaseObject = obj;
                        this.WriteExpendObject(ooo);
                    }
                    else
                    {
                        this.WriteRegex((RubyRegexp)obj);
                    }
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
                else if (obj is FuzzyUserdefinedDumpObject)
                {
                    this.WriteUsingDump((FuzzyUserdefinedDumpObject)obj);
                }
                else if (obj is RubyUserdefinedMarshalDumpObject)
                {
                    this.WriteUsingMarshalDump((RubyUserdefinedMarshalDumpObject)obj);
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

        private void WriteUsingMarshalDump(RubyUserdefinedMarshalDumpObject obj)
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
        class FuzzyExpendObject : FuzzyObject { public object BaseObject;}
        private void WriteExpendObject(FuzzyExpendObject obj)
        {
            if (obj.BaseObject is FuzzySymbol || obj.BaseObject is FuzzyString || obj.BaseObject is RubyRegexp)
            {
                Encoding e = null;
                if (obj.BaseObject is FuzzySymbol)
                    e = (obj.BaseObject as FuzzySymbol).GetRubyString().Encoding;
                if (obj.BaseObject is FuzzyString)
                    e = (obj.BaseObject as FuzzyString).Encoding;
                if (obj.BaseObject is RubyRegexp)
                    e = (obj.BaseObject as RubyRegexp).Pattern.Encoding;

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
                else if (obj.BaseObject is RubyRegexp)
                {
                    this.WriteRegex(obj.BaseObject as RubyRegexp);
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
        class FuzzyExtendedObject : FuzzyObject { public object BaseObject; public RubyModule ExtendedModule;}
        private void WriteExtendedObject(FuzzyExtendedObject obj)
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

        private void WriteRegex(RubyRegexp value)
        {
            this.m_writer.Write((byte)0x2f);
            this.WriteStringValue(value.Pattern.Raw);
            this.m_writer.Write((byte)value.Options);
        }

        private void WriteFloat(RubyFloat v)
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
        }*/
    }
}
