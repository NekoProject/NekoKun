using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun
{
    public static class ResourceManager
    {
        private static List<string> searchPaths = new List<string>();
        private static Dictionary<string, SyntaxSugarIndexer<string, object>> dict = new Dictionary<string, SyntaxSugarIndexer<string, object>>();

        static ResourceManager()
        {
            searchPaths.Add(@"E:\Yichen Lu\rmxp\RPG Maker XP\RGSS\Standard\");
            foreach (string item in new string[] {
                @"Autotiles", @"Tilesets"
            })
            {
                var indexer = new SyntaxSugarIndexer<string, object>(null);
                indexer.Runtime["Category"] = item;
                indexer.Indexer = delegate(string filename)
                {
                    string[] result;
                    foreach (string path in searchPaths)
                    {
                        result = System.IO.Directory.GetFiles(
                            System.IO.Path.Combine(System.IO.Path.Combine(path, "Graphics"), indexer.Runtime["Category"] as string),
                            filename + ".*"
                        );
                        if (result.Length > 0)
                            return System.Drawing.Image.FromFile(result[0]);
                    }
                    return null;
                };
                dict.Add(item, indexer);
            }
        }

        public static Dictionary<string, SyntaxSugarIndexer<string, object>> Caches
        {
            get
            {
                return dict;
            }
        }
    }
}
