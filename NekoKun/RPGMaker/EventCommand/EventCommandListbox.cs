using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public class EventCommandListbox : UI.LynnListbox, IObjectEditor
    {
        protected EventCommandProvider source;
        protected List<object> obj;
        protected string strCommand = "◆";
        protected string strIndent = "·";
        protected string strUnknown = "未定义的指令";

        public EventCommandListbox(Dictionary<string, object> Params)
        {
            if (RequestCommit != null) RequestCommit.ToString();
            source = ProjectManager.Components[Params["Source"] as string] as EventCommandProvider;
            this.SelectedItem = new List<object>();

            this.ContextMenuStrip = new EditContextMenuStrip(this);
            this.ContextMenuStrip.Items.Add(new System.Windows.Forms.ToolStripSeparator());
            this.ContextMenuStrip.Items.Add(new System.Windows.Forms.ToolStripMenuItem("复制全部为 UBB(&B)", null, delegate
            {
                System.Windows.Forms.Clipboard.Clear();
                System.Windows.Forms.Clipboard.SetText(ExportUBB(), System.Windows.Forms.TextDataFormat.UnicodeText);
            }));
        }

        public string ExportUBB()
        {
            StringBuilder sb = new StringBuilder();
            for (int id = 0; id < this.Items.Count; id++)
            {
                EventCommand cmd = GetCodeCommand(id);
                int indent = GetIndent(id);
                sb.Append("[font=黑体]");
                if (indent > 0) sb.Append(new String(' ', indent * 2));
                sb.Append((cmd != null && cmd.IsGenerated) ? "·" : "◆");
                sb.Append("[/font]");
                if (GetCode(id) != "0")
                {
                    System.Drawing.Color color = cmd == null ? System.Drawing.Color.Black : cmd.Group.ForeColor;
                    sb.Append(String.Format("[color=#{0:x2}{1:x2}{2:x2}]", color.R, color.G, color.B));
                    string drawing = cmd == null ? strUnknown : cmd.FormatParams(this.Items[id] as FuzzyData.FuzzyObject);
                    sb.Append(drawing.Replace("{hide}", "[color=white]").Replace("{/hide}", "[/color]"));
                    sb.Append("[/color]");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public void MakeDirty()
        {
            if (this.DirtyChanged != null)
                this.DirtyChanged(this, null);
        }

        protected void Load()
        {
            this.Items.Clear();
            this.Items.AddRange(this.obj.ToArray());
        }

        protected override void DrawText(int id, string str, System.Drawing.Font font, System.Drawing.Brush fc, System.Drawing.Rectangle bounds, System.Drawing.StringFormat sf, System.Drawing.Graphics g, bool selected)
        {
            float fix = g.MeasureString("傻", font).Width * 2 - g.MeasureString("傻逼", font).Width;
            int indentw = (int)(g.MeasureString(strCommand, font).Width - fix);
            int indent = GetIndent(id);
            int x, height, y;
            
            x = bounds.X; y = bounds.Y;
            height = bounds.Height;
            EventCommand cmd = GetCodeCommand(id);

            x += indent * indentw + indentw;
            if (cmd != null && cmd.IsGenerated)
            {
                int iw = (int)(g.MeasureString(strIndent, font).Width - fix);
                g.DrawString(strIndent, font, fc, new System.Drawing.Rectangle(x - iw, y, bounds.Right - x + iw, height));
            }
            else
            {
                g.DrawString(strCommand, font, fc, new System.Drawing.Rectangle(x - indentw, y, bounds.Right - x + indentw, height));
            }

            if (GetCode(id) == "0")
                return;

            if (cmd != null && !selected)
                fc = new System.Drawing.SolidBrush(cmd.Group.ForeColor);

            string drawing;
            if (cmd == null)
                drawing = strUnknown;
            else
            {
                drawing = cmd.FormatParams(this.Items[id] as FuzzyData.FuzzyObject);
            }

            string draw;
            while (drawing.Length > 0)
            {
                int pos = drawing.IndexOf("{hide}");
                if (pos > 0)
                {
                    draw = drawing.Substring(0, pos);
                    drawing = drawing.Substring(pos);
                    g.DrawString(draw, font, fc, new System.Drawing.Rectangle(x, y, bounds.Right - x, height), sf);
                    x += (int)(g.MeasureString(draw, font).Width - fix);
                }
                else if (pos == 0)
                {
                    pos = drawing.IndexOf("{/hide}");
                    draw = drawing.Substring(6, pos - 6);
                    drawing = drawing.Substring(pos + 7);
                    x += (int)(g.MeasureString(draw, font).Width - fix);
                }
                else
                {
                    g.DrawString(drawing, font, fc, new System.Drawing.Rectangle(x, y, bounds.Right - x, height), sf);
                    drawing = "";
                }
            }            
        }


        protected string GetCode(int id)
        {
            return (this.Items[id] as FuzzyData.FuzzyObject).InstanceVariable["@code"].ToString();
        }

        protected EventCommand GetCodeCommand(int id)
        {
            string code = GetCode(id);
            if (this.source.Commands.ContainsKey(code))
            {
                return this.source.Commands[code];
            }
            else
            {
                return null;
            }
        }

        protected int GetIndent(int id)
        {
            object o = (this.Items[id] as FuzzyData.FuzzyObject).InstanceVariable["@indent"];
            if (o == null)
                return 0;
            else
                return (int)o;
        }

        public new object SelectedItem
        {
            get
            {
                return obj;
            }
            set
            {
                this.obj = value as List<object>;
                this.Load();
            }
        }

        public event EventHandler RequestCommit;
        public event EventHandler DirtyChanged;
        public void Commit() { }
    }
}
