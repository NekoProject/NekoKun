using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public class DatabaseItemEditorTableLayout : DatabaseItemEditor
    {
        protected System.Windows.Forms.TableLayoutPanel panel;

        public DatabaseItemEditorTableLayout(System.Xml.XmlNode param, DatabaseFile file)
            : base(param, file)
        {
            panel = new System.Windows.Forms.TableLayoutPanel();
            this.panel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.None;

            panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Controls.Add(panel);

            this.panel.Margin = new System.Windows.Forms.Padding(0);
            this.panel.RowCount = Int32.Parse(param.Attributes["RowCount"].Value);
            this.panel.ColumnCount = Int32.Parse(param.Attributes["ColumnCount"].Value);

            for (int i = 0; i < this.panel.RowCount; i++)
                this.panel.RowStyles.Add(new System.Windows.Forms.RowStyle());

            for (int i = 0; i < this.panel.ColumnCount; i++)
                this.panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());

            foreach (System.Xml.XmlNode item in this.param.ChildNodes)
            {
                if (item.Name != "TableCell") continue;
                int x, y;
                x = Int32.Parse(item.Attributes["X"].Value);
                y = Int32.Parse(item.Attributes["Y"].Value);

                if (item.Attributes["Width"] != null)
                {
                    string str = item.Attributes["Width"].Value;
                    
                    if (str.StartsWith("1/"))
                    {
                        this.panel.ColumnStyles[x - 1].SizeType = System.Windows.Forms.SizeType.Percent;
                        this.panel.ColumnStyles[x - 1].Width = 1.0f / Int32.Parse(str.Substring(2));
                    }
                    else
                    {
                        this.panel.ColumnStyles[x - 1].SizeType = System.Windows.Forms.SizeType.Absolute;
                        this.panel.ColumnStyles[x - 1].Width = Int32.Parse(str);
                    }
                }

                if (item.Attributes["Height"] != null)
                {
                    string str = item.Attributes["Height"].Value;
                    if (str.StartsWith("1/"))
                    {
                        this.panel.RowStyles[y - 1].SizeType = System.Windows.Forms.SizeType.Percent;
                        this.panel.RowStyles[y - 1].Height = 1.0f / Int32.Parse(str.Substring(2));
                    }
                    else
                    {
                        this.panel.RowStyles[y - 1].SizeType = System.Windows.Forms.SizeType.Absolute;
                        this.panel.RowStyles[y - 1].Height = Int32.Parse(str);
                    }
                }

                System.Windows.Forms.Control con = null;
                if (item.FirstChild is System.Xml.XmlText)
                {
                    con = new UI.LynnLabel();
                    con.Text = item.FirstChild.Value;
                }
                else if (item.FirstChild.Name == "Control")
                {
                    con = this.editors[file.Fields[item.FirstChild.Attributes["ID"].Value]] as System.Windows.Forms.Control;
                }

                if (con != null)
                {
                    con.Margin = new System.Windows.Forms.Padding(0);
                    con.Dock = System.Windows.Forms.DockStyle.Fill;
                    this.panel.Controls.Add(con, x - 1, y - 1);

                    if (item.Attributes["ColumnSpan"] != null)
                    {
                        this.panel.SetColumnSpan(con, Int32.Parse(item.Attributes["ColumnSpan"].Value));
                    }

                    if (item.Attributes["RowSpan"] != null)
                    {
                        this.panel.SetRowSpan(con, Int32.Parse(item.Attributes["RowSpan"].Value));
                    }
                }
            }
        }
    }
}
