using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun
{
    public class FindAllResultFile : AbstractFile
    {
        public string Keyword;
        public NavPoint[] Result;

        public FindAllResultFile(string Keyword, NavPoint[] Result)
            : base(@"\\.\NekoKun\FindAllResult\" + System.DateTime.Now.ToBinary())
        {
            this.Keyword = Keyword;
            this.Result = Result;
        }

        public override string ToString()
        {
            return string.Format("查找全部结果 - {0} - {1} 个匹配项", this.Keyword, this.Result.Length);
        }

        protected override void Save()
        {
            return;
        }

        public override AbstractEditor CreateEditor()
        {
            return new FindAllResultEditor(this);
        }
    }
}
