using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun
{
    public static class FileManager
    {
        private static Dictionary<string, AbstractFile> dict = new Dictionary<string,AbstractFile>();

        static FileManager()
        {

        }

        internal static void Open(AbstractFile file)
        {
            dict.Add(file.filename, file);
        }

        internal static void Close(AbstractFile file)
        {
            try
            {
                dict.Remove(file.filename);
            }
            catch { }
        }

        public static AbstractFile Find(string identify)
        {
            return dict[identify];
        }

        public static NavPoint[] FindAll(string Keyword)
        {
            var result = new List<NavPoint>();
            var src = new AbstractFile[dict.Count];
            dict.Values.CopyTo(src, 0);

            Array.ForEach<AbstractFile>(src, delegate(AbstractFile file) {
                IFindAllProvider findAll = file as IFindAllProvider;

                if (findAll != null)
                    result.AddRange(findAll.FindAll(Keyword));
            });

            return result.ToArray();
        }
    }
}
