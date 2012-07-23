using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public class DatabaseEditor : AbstractEditor
    {
        System.Windows.Forms.ListBox list;
        System.Windows.Forms.SplitContainer split;
        DatabaseItemEditor con;
        DatabaseFile dbf;

        public DatabaseEditor(AbstractFile file)
            : base(file)
        {
            dbf = file as DatabaseFile;

            con = Program.CreateInstanceFromTypeName(dbf.Layout, dbf.LayoutInfo, dbf) as DatabaseItemEditor;
            con.Dock = System.Windows.Forms.DockStyle.Fill;

            if (dbf.ArrayMode)
            {
                con.SelectedItem = dbf.Contents[0];
                
                split = new System.Windows.Forms.SplitContainer();
                split.Dock = System.Windows.Forms.DockStyle.Fill;
                split.Panel1MinSize = 100;
                split.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
                split.SplitterDistance = 150;

                list = new DatabaseListbox();
                list.Items.AddRange((file as DatabaseFile).Contents.ToArray());
                list.Dock = System.Windows.Forms.DockStyle.Fill;

                list.SelectedIndexChanged += new EventHandler(list_SelectedIndexChanged);

                split.Panel1.Controls.Add(list);
                split.Panel2.Controls.Add(con);

                this.Controls.Add(split);
            }
            else
            {
                con.SelectedItem = dbf.Contents[0];
                this.Controls.Add(con);
            }
        }

        void list_SelectedIndexChanged(object sender, EventArgs e)
        {
            con.SelectedItem = list.SelectedItem as DatabaseItem;
        }
    }
}
