using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NekoKun.FuzzyData.Serialization.RubyMarshal
{
    public static class RubyMarshal
    {
        public const byte MarshalMajor = 4;
        public const byte MarshalMinor = 8;

        public abstract class IDs
        {
            public static FuzzySymbol encoding = FuzzySymbol.GetSymbol("encoding");
            public static FuzzySymbol E = FuzzySymbol.GetSymbol("E");   
        }

        public abstract class Types
        {
            public const byte Nil = (byte)'0';
            public const byte True = (byte)'T';
            public const byte False = (byte)'F';
            public const byte Fixnum = (byte)'i';
            public const byte Extended = (byte)'e';
            public const byte UserClass = (byte)'C';
            public const byte Object = (byte)'o';
            public const byte Data = (byte)'d';
            public const byte UserDefined = (byte)'u';
            public const byte UserMarshal = (byte)'U';
            public const byte Float = (byte)'f';
            public const byte Bignum = (byte)'l';
            public const byte String = (byte)'"';
            public const byte Regexp = (byte)'/';
            public const byte Array = (byte)'[';
            public const byte Hash = (byte)'{';
            public const byte HashWithDefault = (byte)'}';
            public const byte Struct = (byte)'S';
            public const byte ModuleOld = (byte)'M';
            public const byte Class = (byte)'c';
            public const byte Module = (byte)'m';
            public const byte Symbol = (byte)':';
            public const byte SymbolLink = (byte)';';
            public const byte InstanceVariable = (byte)'I';
            public const byte Link = (byte)'@';
        }

        public static object Load(Stream input)
        {
            RubyMarshalReader reader = new RubyMarshalReader(input);
            return reader.Load();
        }

        public static void Dump(Stream output, object param)
        {
            RubyMarshalWriter writer = new RubyMarshalWriter(output);
            writer.Dump(param);
        }
    }
}
