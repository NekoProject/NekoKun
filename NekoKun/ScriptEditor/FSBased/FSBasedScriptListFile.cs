using System;
using System.Collections.Generic;

using System.Text;

namespace NekoKun
{
    public class FSBasedScriptListFile : ScriptListFile
    {
        protected string scriptDir;
        public FSBasedScriptListFile(string filename)
            : base(filename)
        {
            scriptDir = System.IO.Path.GetDirectoryName(filename);
            string[] scripts = System.IO.File.ReadAllText(filename).Split(new string[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            Program.Logger.Log("加载脚本索引文件：{0}", filename);

            foreach (string script in scripts)
            {
                string filen = System.IO.Path.Combine(scriptDir, script + ".rb");
                if (System.IO.File.Exists(filen))
                {
                    FSBasedScriptFile item = new FSBasedScriptFile(filen);
                    this.scripts.Add(item);
                }
                else
                {
                    Program.Logger.Log("  找不到脚本文件：{0}", filen);
                    this.MakeDirty();
                }
            }

            if (this.IsDirty)
                Program.Logger.Log("因为脚本索引文件中存在无效条目，且无效条目未被加载，索引文件脏了。");
        }

        public FSBasedScriptListFile(Dictionary<string, object> node)
            : this(
                System.IO.Path.Combine(
                    ProjectManager.ProjectDir,
                    node["FileName"].ToString()
                )
              )
        {
            
        }

        protected override void Save()
        {
            StringBuilder sb = new StringBuilder();
            foreach (FSBasedScriptFile file in this.scripts)
            {
                sb.AppendLine(System.IO.Path.GetFileNameWithoutExtension(file.filename));
            }
            sb.Remove(sb.Length - System.Environment.NewLine.Length, System.Environment.NewLine.Length);

            System.IO.File.WriteAllText(this.filename, sb.ToString(), Encoding.ASCII);
        }

        public override ScriptFile InsertFile(string pageName, int index)
        {
            string pathName = GenerateFileName(pageName);

            FSBasedScriptFile scriptFile = new FSBasedScriptFile(pathName, "");
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

            string pathName = System.IO.Path.Combine(scriptDir, pageName + ".rb");

            if (System.IO.File.Exists(pathName))
                throw new ArgumentException(String.Format("文件已存在：{0}", pageName));

            return pathName;
        }
    }
}
