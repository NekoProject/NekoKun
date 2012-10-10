using System;
using System.Collections.Generic;
using System.Text;
using NekoKun.FuzzyData;

namespace NekoKun.RPGMaker
{
    public class ScriptListFile : NekoKun.ScriptListFile
    {
        protected string scriptDir;
        public ScriptListFile(string filename)
            : base(filename)
        {
            Program.Logger.Log("加载脚本索引文件：{0}", filename);

            using (System.IO.FileStream scriptFile = System.IO.File.Open(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                List<object> scripts = NekoKun.FuzzyData.Serialization.RubyMarshal.RubyMarshal.Load(scriptFile) as List<object>;

                foreach (List<object> item in scripts)
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
						Program.Logger.Log("唯一 ID 遭到破坏。{0}", item[0].ToString());
		                Program.Logger.Log("因为脚本文件中存在错误，现已经被修复，因此文件脏了。");
					}
						
                    while (containsID(id))
					{
                        id = ran.Next(1, 100000000);
						this.MakeDirty();
					}

                    if (item[1] is FuzzyData.FuzzyExpendObject)
                        title = UnicodeStringFromUTF8Bytes((byte[])((FuzzyData.FuzzyExpendObject)item[1]).BaseObject);
                    else
                        title = UnicodeStringFromUTF8Bytes((byte[])item[1]);

                    if (item[2] is FuzzyData.FuzzyExpendObject)
                        bytes = (byte[])((FuzzyData.FuzzyExpendObject)item[2]).BaseObject;
                    else
                        bytes = (byte[])item[2];

                    byte[] inflated = Ionic.Zlib.ZlibStream.UncompressBuffer(bytes);
                    string code = "";

                    if (inflated.Length > 0) code = System.Text.Encoding.UTF8.GetString(inflated);

                    this.scripts.Add(new ScriptFile(this, code, title, id));
                }

                scriptFile.Close();
            }
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
            List<object> rawFile = new List<object>();
            foreach (ScriptFile item in this.scripts)
            {
                if (item.Editor != null)
                    item.Editor.Commit();
            }

            FuzzyData.FuzzyExpendObject obj;
            foreach (ScriptFile item in this.scripts)
            {
                List<object> rawItem = new List<object>();
                rawItem.Add(item.ID);

                obj = new FuzzyExpendObject();
                obj.BaseObject = UTF8BytesFromUnicodeString(item.Title);
                obj.InstanceVariables[FuzzySymbol.GetSymbol("E")] = true;
                rawItem.Add(obj);

                obj = new FuzzyExpendObject();
                obj.BaseObject = Ionic.Zlib.ZlibStream.CompressBuffer(UTF8BytesFromUnicodeString(item.Code));
                if (((byte[])obj.BaseObject).Length == 0)
                {
                    obj.BaseObject = new byte[] { 120, 156, 3, 0, 0, 0, 0, 1 };
                }
                obj.InstanceVariables[FuzzySymbol.GetSymbol("E")] = true;
                rawItem.Add(obj);

                rawFile.Add(rawItem);
            }
            System.IO.FileStream file = System.IO.File.OpenWrite(this.filename);
            NekoKun.FuzzyData.Serialization.RubyMarshal.RubyMarshal.Dump(file, rawFile);
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
    }
}
