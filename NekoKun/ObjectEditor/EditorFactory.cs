using System;
using System.Collections.Generic;
using System.Text;
using NekoKun.Serialization.RubyMarshal;

namespace NekoKun.ObjectEditor
{
    public static class EditorFactory
    {
        public static string CreateFromDefaultValue(object value)
        {
            if (value is RubyString || value is string)
                return typeof(SingleTextEditor).FullName;

            if (value is int)
                return typeof(DecimalEditor).FullName;

            if (value is RubyObject)
            {
                RubyObject obj = value as RubyObject;
                //if (obj.ClassName.GetString() == "RPG::AudioFile")
                //    return typeof(RPGMaker.AudioFileEditor).FullName;
            }

            return typeof(UnknownEditor).FullName;
        }
    }
}
