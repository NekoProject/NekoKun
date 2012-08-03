using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun
{
    public static class StorageManager
    {
        private static string userDirectory;
        private static string globalDirectory;

        static StorageManager()
        {
            userDirectory = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "NekoKun");
            globalDirectory = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
        }

        public static string GetGlobalDirectory(string storageCategory)
        {
            string path = System.IO.Path.Combine(globalDirectory, storageCategory);
            return path;
        }

        public static string GetUserDirectory(string storageCategory)
        {
            string path = System.IO.Path.Combine(userDirectory, storageCategory);
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            return path;
        }

        public static string[] GetVirtualDirectoryFiles(string storageCategory, string searchPattern, System.IO.SearchOption searchOption)
        {
            List<string> result = new List<string>();
            if (System.IO.Directory.Exists(GetUserDirectory(storageCategory)))
                result.AddRange(System.IO.Directory.GetFiles(GetUserDirectory(storageCategory), searchPattern, searchOption));
            if (System.IO.Directory.Exists(GetGlobalDirectory(storageCategory)))
                result.AddRange(System.IO.Directory.GetFiles(GetGlobalDirectory(storageCategory), searchPattern, searchOption));
            return result.ToArray();
        }

        public static string[] GetVirtualDirectoryFiles(string storageCategory, string searchPattern)
        {
            List<string> result = new List<string>();
            if (System.IO.Directory.Exists(GetUserDirectory(storageCategory)))
                result.AddRange(System.IO.Directory.GetFiles(GetUserDirectory(storageCategory), searchPattern));
            if (System.IO.Directory.Exists(GetGlobalDirectory(storageCategory)))
                result.AddRange(System.IO.Directory.GetFiles(GetGlobalDirectory(storageCategory), searchPattern));
            return result.ToArray();
        }

        public static string[] GetVirtualDirectoryFiles(string storageCategory)
        {
            List<string> result = new List<string>();
            if (System.IO.Directory.Exists(GetUserDirectory(storageCategory)))
                result.AddRange(System.IO.Directory.GetFiles(GetUserDirectory(storageCategory)));
            if (System.IO.Directory.Exists(GetGlobalDirectory(storageCategory)))
                result.AddRange(System.IO.Directory.GetFiles(GetGlobalDirectory(storageCategory)));
            return result.ToArray();
        }

        public static string GetNextName(string baseDirectory, string format)
        {
            string name;
            int counter = 0;
            do
            {
                counter += 1;
                name = String.Format(format, counter);
            }
            while (System.IO.Directory.Exists(System.IO.Path.Combine(baseDirectory, name)));
            return name;
        }
    }
}
