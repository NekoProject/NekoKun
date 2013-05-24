using System;
using System.Collections.Generic;

using System.Text;

namespace NekoKun
{
    public class VirtualScriptListFile : ScriptListFile
    {
        public VirtualScriptListFile()
            : base(@"\\.\NekoKun\VirtualScriptsIndex")
        {
            var rand = new System.Random();
            var count = rand.Next(25, 50);
            for (int i = 0; i < count; i++)
            {
                var bytes = new byte[200];
                rand.NextBytes(bytes);
                var code = System.Text.Encoding.Default.GetString(bytes);
                this.scripts.Add(new VirtualScriptFile(@"\\.\NekoKun\VirtualScripts\" + rand.Next(10000, 99999).ToString(), code));
            }
        }

        public VirtualScriptListFile(Dictionary<string, object> node)
            : this()
        {
            
        }

        protected override void Save()
        {
        }

        public override ScriptFile InsertFile(string pageName, int index)
        {
            string pathName = GenerateFileName(pageName);

            VirtualScriptFile scriptFile = new VirtualScriptFile(pathName, "");
            this.scripts.Insert(index, scriptFile);
            scriptFile.MakeDirty();

            this.MakeDirty();

            return scriptFile;
        }

        public override void DeleteFile(ScriptFile file)
        {
            if (!this.scripts.Contains(file))
                return;

            this.scripts.Remove(file);
            file.PendingDelete();

            this.MakeDirty();
        }

        public override string GenerateFileName(string pageName)
        {
            pageName = pageName.Trim();

            if (pageName == "")
                throw new ArgumentException(String.Format("必须键入文件名", pageName));

            if (pageName.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) >= 0)
                throw new ArgumentException(String.Format("名称无效，请不要使用无法在文件名中使用的字符。", pageName));

            return @"\\.\NekoKun\VirtualScripts\" + pageName;
        }
    }
}
