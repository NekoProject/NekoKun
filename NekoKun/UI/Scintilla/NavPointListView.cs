using System;
using System.Collections.Generic;
using System.Text;
using ScintillaNet;

namespace NekoKun.UI
{
    public class NavPointListView : Scintilla
    {
        private List<NavPoint> items;

        public NavPointListView()
        {
            items = new List<NavPoint>();

            this.IsReadOnly = true;
            this.Margins[0].Width = 20;
            this.Margins[1].Width = 0;

            this.Lexing.Lexer = Lexer.Container;
            this.StyleNeeded += new EventHandler<StyleNeededEventArgs>(NavPointListView_StyleNeeded);
            this.Lexing.StyleNameMap.Add("orz", 32);
            this.Styles["orz"].IsHotspot = true;
            this.Styles["orz"].Font = this.Font = new System.Drawing.Font("雅黑宋体", 10);

            this.HotspotDoubleClick += new EventHandler<ScintillaMouseEventArgs>(NavPointListView_HotspotDoubleClick);
        }

        void NavPointListView_HotspotDoubleClick(object sender, ScintillaMouseEventArgs e)
        {
            this.Lines.FromPosition(e.Position).Select();
            try
            {
                items[this.Lines.FromPosition(e.Position).Number].Goto();
            }
            catch { }
        }

        public void AddItem(NavPoint pt)
        {
            if (items.Contains(pt)) return;
            items.Add(pt);
            this.IsReadOnly = false;
            this.AppendText(pt.ToString());
            this.AppendText(Environment.NewLine);
            this.IsReadOnly = true;
        }

        public void SetKeyword(string Keyword)
        {
            this.FindReplace.HighlightAll(this.FindReplace.FindAll(Keyword));
        }

        void NavPointListView_StyleNeeded(object sender, StyleNeededEventArgs e)
        {
            e.Range.SetStyle("orz");
        }
    }
}
