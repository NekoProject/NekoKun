using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun
{
    public static class ResourceManager
    {
        private static List<string> searchPaths = new List<string>();
        private static DictionaryWithDefaultProc<string, SyntaxSugarIndexer<string, object>> dict;

        static ResourceManager()
        {
            //searchPaths.Add(@"E:\Yichen Lu\rmxp\RPG Maker XP\RGSS\Standard\");
            searchPaths.Add(@"c:\Program Files\Common Files\Enterbrain\RGSS3\RPGVXAce\");
            dict = new DictionaryWithDefaultProc<string,SyntaxSugarIndexer<string,object>>(delegate(string item)
            {
                var indexer = new SyntaxSugarIndexer<string, object>(null);
                indexer.Runtime["Category"] = item;
                indexer.Indexer = delegate(string filename)
                {
                    string[] result;
                    foreach (string path in searchPaths)
                    {
                        string mypath = System.IO.Path.Combine(path, (indexer.Runtime["Category"] as string).Replace('/', System.IO.Path.DirectorySeparatorChar));
                        if (System.IO.Directory.Exists(mypath))
                        {
                            result = System.IO.Directory.GetFiles(
                                mypath,
                                filename + ".*"
                            );
                            if (result.Length > 0)
                                return System.Drawing.Image.FromFile(result[0]);
                        }
                    }
                    return null;
                };
                return indexer;
            });
        }

        public static DictionaryWithDefaultProc<string, SyntaxSugarIndexer<string, object>> Caches
        {
            get
            {
                return dict;
            }
        }

        public static List<string> SearchPaths
        {
            get { return searchPaths; }
        }
    }
}
