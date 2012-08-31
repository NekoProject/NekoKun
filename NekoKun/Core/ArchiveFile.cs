using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NekoKun
{
    public class ArchiveFile: AbstractFile
    {
        protected List<string> files;
        protected Dictionary<string, byte[]> contents;
        protected string manifest;

        public ArchiveFile(string filename)
            : base(filename, false)
        {
            FileStream fs = new FileStream(filename,  FileMode.Open, FileAccess.Read);
            manifest = RubyBindings.RubyMarshal.Load(fs) as string;
            contents = new Dictionary<string, byte[]>();
            files = new List<string> (Array.ConvertAll<object, string>(
                (RubyBindings.RubyMarshal.Load(fs) as List<object>).ToArray(),
                (object o) => {
                    contents.Add(o.ToString(), null);
                    return o.ToString();
                }
            ));

            fs.Close();
        }

        public string Manifest { get { return this.manifest; } }

        protected override void Save()
        {
            throw new NotImplementedException();
        }

        public override AbstractEditor CreateEditor()
        {
            throw new NotImplementedException();
        }

        public byte[] this[string key]
        {
            get
            {
                if (contents.ContainsKey(key))
                {
                    if (contents[key] == null)
                        LoadData();
                    return contents[key];
                }
                return null;
            }
        }

        private void LoadData()
        {
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            RubyBindings.RubyMarshal.Load(fs);
            RubyBindings.RubyMarshal.Load(fs);
            byte[] buffer = Ionic.Zlib.ZlibStream.UncompressBuffer((RubyBindings.RubyMarshal.Load(fs) as RubyBindings.RubyString).Raw);
            MemoryStream ms = new MemoryStream(buffer, false);
            Dictionary<object, object> con = RubyBindings.RubyMarshal.Load(ms) as Dictionary<object, object>;
            fs.Close();
            foreach (var item in con)
            {
                string key = (item.Key as RubyBindings.RubyString).Text;
                this.contents[key] = (item.Value as RubyBindings.RubyString).Raw;
            }
        }

        public void Extract(string dir)
        {
            foreach (string item in this.files)
            {
                string name = item.Replace('/', System.IO.Path.DirectorySeparatorChar);
                string fullname = System.IO.Path.Combine(dir, name);
                string dirname = System.IO.Path.GetDirectoryName(fullname);
                if (!System.IO.Directory.Exists(dirname))
                    System.IO.Directory.CreateDirectory(dirname);

                System.IO.File.WriteAllBytes(fullname, this[item]);
            }
        }
    }
}
