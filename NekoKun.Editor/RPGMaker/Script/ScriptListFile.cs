using System;
using System.Collections.Generic;
using System.Text;
using NekoKun.Serialization.RubyMarshal;

namespace NekoKun.RPGMaker
{
    public class ScriptListFile : NekoKun.ScriptListFile
    {
        protected string scriptDir;
        public ScriptListFile(string filename)
            : base(filename)
        {
            this.IsSubfileProvider = true;
            NekoKun.Core.Application.Logger.Log("加载脚本索引文件：{0}", filename);

            using (System.IO.FileStream scriptFile = System.IO.File.Open(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                RubyArray scripts = NekoKun.Serialization.RubyMarshal.RubyMarshal.Load(scriptFile) as RubyArray;

                foreach (RubyArray item in scripts)
                {
                    string title;
                    byte[] bytes;

                    var ran = new System.Random();
                    int id;

                    if (item[0] is int)
                        id = (int)item[0];
                    else
                    {
                        id = ran.Next(1, 100000000);
                        this.MakeDirty();
                        NekoKun.Core.Application.Logger.Log("唯一 ID 遭到破坏。{0}", item[0].ToString());
                    }

                    while (containsID(id))
                    {
                        id = ran.Next(1, 100000000);
                        this.MakeDirty();
                    }

                    title = (item[1] as RubyString).ForceEncoding(Encoding.UTF8).Text;
                    bytes = (item[2] as RubyString).Raw;
                    
                    string code = "";
                    try
                    {
                        byte[] inflated = Ionic.Zlib.ZlibStream.UncompressBuffer(bytes);
                        if (inflated != null && inflated.Length > 0)
                            code = System.Text.Encoding.UTF8.GetString(inflated);
                    }
                    catch
                    {
                        NekoKun.Core.Application.Logger.Log("无法读取脚本“{0}：{1}”，文件可能已损坏，此页已加载为残存物十六进制堆存。", item[0].ToString(), title);
                        code = "=begin" + System.Environment.NewLine + Program.BuildHexDump(bytes) + System.Environment.NewLine + "=end";
                        this.MakeDirty();
                    }

                    this.scripts.Add(new ScriptFile(this, code, title, id));
                }

                scriptFile.Close();
            }

            if (this.isDirty)
                NekoKun.Core.Application.Logger.Log("因为脚本文件中存在错误，且采取了一定措施解决/缓解此矛盾，因此产生了未保存的更改。");
        }

        public ScriptListFile(Dictionary<string, object> node)
            : this(
                System.IO.Path.Combine(
                    ProjectManager.ProjectDir,
                    node["FileName"].ToString()
                )
              )
        {
            
        }

        private static string UnicodeStringFromUTF8Bytes(byte[] bytes)
        {
            return System.Text.Encoding.UTF8.GetString(bytes);
        }

        private static byte[] UTF8BytesFromUnicodeString(string str)
        {
            return Encoding.Convert(Encoding.Unicode, Encoding.UTF8, Encoding.Unicode.GetBytes(str));
        }

        protected override void Save()
        {
            RubyArray rawFile = new RubyArray();
            foreach (ScriptFile item in this.scripts)
            {
                if (item.Editor != null)
                    item.Editor.Commit();
            }

            foreach (ScriptFile item in this.scripts)
            {
                RubyArray rawItem = new RubyArray();
                rawItem.Add(item.ID);
                rawItem.Add(new RubyString(item.Title).Encode(Encoding.UTF8));
                byte[] code = Ionic.Zlib.ZlibStream.CompressBuffer(UTF8BytesFromUnicodeString(item.Code));
                if (code.Length == 0)
                {
                    code = new byte[] { 120, 156, 3, 0, 0, 0, 0, 1 };
                }
                rawItem.Add(new RubyString(code));
                rawFile.Add(rawItem);
            }
            System.IO.FileStream file = System.IO.File.OpenWrite(this.filename);
            NekoKun.Serialization.RubyMarshal.RubyMarshal.Dump(file, rawFile);
            file.Close();
        }

        private bool containsID(int id)
        {
            foreach (ScriptFile item in this.scripts)
            {
                if (id == item.ID) return true;
            }
            return false;
        }

        public override NekoKun.ScriptFile InsertFile(string pageName, int index)
        {
            string pathName = GenerateFileName(pageName);

            var ran = new System.Random();
            int id = ran.Next(1, 100000000);
            while (containsID(id))
                id = ran.Next(1, 100000000);

            ScriptFile scriptFile = new ScriptFile(this, "", pageName, id);
            this.scripts.Insert(index, scriptFile);
            scriptFile.MakeDirty();

            this.MakeDirty();

            return scriptFile;
        }

        public override void DeleteFile(NekoKun.ScriptFile file)
        {
            if (!this.scripts.Contains(file))
                return;

            this.scripts.Remove(file);
            file.PendingDelete();

            this.MakeDirty();
        }

        public override string GenerateFileName(string pageName)
        {
            return filename;
        }

        public override AbstractFile[] Subfiles
        {
            get
            {
                return this.scripts.ToArray();
            }
        }
    }
}
