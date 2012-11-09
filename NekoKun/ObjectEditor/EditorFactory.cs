using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.ObjectEditor
{
    public static class EditorFactory
    {
        public static string CreateFromDefaultValue(object value)
        {
            if (value is FuzzyData.FuzzyString || value is string)
                return typeof(SingleTextEditor).FullName;

            if (value is int)
                return typeof(DecimalEditor).FullName;

            if (value is FuzzyData.FuzzyObject)
            {
                FuzzyData.FuzzyObject obj = value as FuzzyData.FuzzyObject;
                //if (obj.ClassName.GetString() == "RPG::AudioFile")
                //    return typeof(RPGMaker.AudioFileEditor).FullName;
            }

            return typeof(UnknownEditor).FullName;
        }
    }
}
