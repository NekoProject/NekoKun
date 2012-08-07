using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.ObjectEditor
{
    public static class EditorFactory
    {
        public static string CreateFromDefaultValue(object value)
        {
            if (value is string)
                return typeof(SingleTextEditor).FullName;

            if (value is Decimal || value is int || value is Int32 || value is Int64 || value is float || value is double || value is byte || value is sbyte)
                return typeof(DecimalEditor).FullName;

            if (value is RubyBindings.RubyObject)
            {
                RubyBindings.RubyObject obj = value as RubyBindings.RubyObject;
                if (obj.ClassName.GetString() == "RPG::AudioFile")
                    return typeof(RPGMaker.AudioFileEditor).FullName;
            }

            return typeof(UnknownEditor).FullName;
        }
    }
}
