using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NekoKun.RubyBindings
{
    public static class RubyMarshal
    {
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
