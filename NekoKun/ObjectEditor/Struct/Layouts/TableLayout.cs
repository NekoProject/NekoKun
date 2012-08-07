using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.ObjectEditor
{
    public class TableLayout : System.Windows.Forms.TableLayoutPanel, IStructEditorLayout
    {
        public TableLayout(System.Xml.XmlNode param, CreateControlDelegate createControlDelegate)
        {
            this.Margin = new System.Windows.Forms.Padding(0);
            this.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.None;
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RowCount = Int32.Parse(param.Attributes["RowCount"].Value);
            this.ColumnCount = Int32.Parse(param.Attributes["ColumnCount"].Value);
            this.AutoScroll = true;

            for (int i = 0; i < this.RowCount; i++)
                this.RowStyles.Add(new System.Windows.Forms.RowStyle());

            for (int i = 0; i < this.ColumnCount; i++)
                this.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());

            foreach (System.Xml.XmlNode item in param.ChildNodes)
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
                        this.ColumnStyles[x - 1].SizeType = System.Windows.Forms.SizeType.Percent;
                        this.ColumnStyles[x - 1].Width = 1.0f / Int32.Parse(str.Substring(2));
                    }
                    else
                    {
                        this.ColumnStyles[x - 1].SizeType = System.Windows.Forms.SizeType.Absolute;
                        this.ColumnStyles[x - 1].Width = Int32.Parse(str);
                    }
                }

                if (item.Attributes["Height"] != null)
                {
                    string str = item.Attributes["Height"].Value;
                    if (str.StartsWith("1/"))
                    {
                        this.RowStyles[y - 1].SizeType = System.Windows.Forms.SizeType.Percent;
                        this.RowStyles[y - 1].Height = 1.0f / Int32.Parse(str.Substring(2));
                    }
                    else
                    {
                        this.RowStyles[y - 1].SizeType = System.Windows.Forms.SizeType.Absolute;
                        this.RowStyles[y - 1].Height = Int32.Parse(str);
                    }
                }

                System.Windows.Forms.Control con = null;
                if (item.FirstChild is System.Xml.XmlText)
                {
                    con = new UI.LynnLabel();
                    con.Text = item.FirstChild.Value;
                }
                else if (item.FirstChild.Name == "Control" || item.FirstChild.Name == "Layout")
                {
                    con = createControlDelegate(item.FirstChild);
                }

                if (con != null)
                {
                    //con.Margin = new System.Windows.Forms.Padding(5);
                    con.Dock = System.Windows.Forms.DockStyle.Fill;
                    this.Controls.Add(con, x - 1, y - 1);

                    if (item.Attributes["ColumnSpan"] != null)
                    {
                        this.SetColumnSpan(con, Int32.Parse(item.Attributes["ColumnSpan"].Value));
                    }

                    if (item.Attributes["RowSpan"] != null)
                    {
                        this.SetRowSpan(con, Int32.Parse(item.Attributes["RowSpan"].Value));
                    }
                }
            }
        }
    }
}
