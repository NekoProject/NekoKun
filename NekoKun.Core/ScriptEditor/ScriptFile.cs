using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun
{
    public abstract class ScriptFile : AbstractFile, IFindAllProvider
    {
        public string Code;

        public ScriptFile(string filename)
            : base(filename)
        {
        }
        
        public ScriptFile(string filename, string code)
            : base(filename)
        {
            this.Code = code;
        }

        protected override void Save()
        {
            throw new NotImplementedException();
        }

        public override AbstractEditor CreateEditor()
        {
            return new ScriptEditor(this);
        }

        public NavPoint[] FindAll(string Keyword)
        {
            if (this.Editor != null)
                this.Editor.Commit();

            var lines = this.Code.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var result = new List<NavPoint>();

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].IndexOf(Keyword, 0) >= 0)
                    result.Add(new ScriptFileNavPoint(this) { LineNo = i, LineText = lines[i], Keyword = Keyword });
            }

            return result.ToArray();
        }

        public override void Goto(NavPoint pt)
        {
            ScriptFileNavPoint s = pt as ScriptFileNavPoint;
            if (s != null)
            {
                this.ShowEditor();
                try
                {
                    (this.editor as ScriptEditor).editor.Lines[s.LineNo].Select();
                    if (s.Keyword != null)
                        (this.editor as ScriptEditor).editor.FindReplace.HighlightAll((this.editor as ScriptEditor).editor.FindReplace.FindAll(s.Keyword));
                    (this.editor as ScriptEditor).editor.Focus();
                }
                catch { }
                return;
            }
            base.Goto(pt);
        }

    }

    public class ScriptFileNavPoint : NavPoint
    {
        public int LineNo;
        public string LineText;
        public string Keyword;

        public ScriptFileNavPoint(AbstractFile file) : base(file) { }

        public override string ToString()
        {
            return String.Format("{0}({1}): {2}", base.ToString(), this.LineNo + 1, this.LineText);
        }
    }
}
