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
            return Load(input, false);
        }

        public static object Load(Stream input, bool TreatStringAsBytes)
        {
            RubyMarshalReader reader = new RubyMarshalReader(input);
            reader.TreatStringAsBytes = TreatStringAsBytes;
            return reader.Load();
        }

        public static void Dump(Stream output, object param)
        {
            RubyMarshalWriter writer = new RubyMarshalWriter(output);
            writer.Dump(param);
        }
    }
}
