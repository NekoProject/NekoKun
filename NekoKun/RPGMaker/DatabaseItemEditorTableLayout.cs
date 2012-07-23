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

            this.panel.RowCount = file.Fields.Count - 1;
            this.panel.ColumnCount = 1;

            foreach (var item in this.editors)
            {
                (item.Value as System.Windows.Forms.Control).Dock = System.Windows.Forms.DockStyle.Fill;
                this.panel.Controls.Add(item.Value as System.Windows.Forms.Control);
            }
        }

    }
}
