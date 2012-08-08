using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public class MapInfoEditor : AbstractEditor
    {
        UI.LynnTreeView tree = new NekoKun.UI.LynnTreeView();
        MapInfoFile info;
        public MapInfoEditor(MapInfoFile file)
            : base(file)
        {
            info = file;

            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop | WeifenLuo.WinFormsUI.Docking.DockAreas.Document | WeifenLuo.WinFormsUI.Docking.DockAreas.Float;

            tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Controls.Add(tree);

            foreach (var item in info.maps)
            {
                System.Windows.Forms.TreeNode node = new System.Windows.Forms.TreeNode(item.Value.Title);
                node.Tag = item.Value;
                tree.Nodes.Add(node);
            }
            tree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(tree_NodeMouseDoubleClick);
        }

        void tree_NodeMouseDoubleClick(object sender, System.Windows.Forms.TreeNodeMouseClickEventArgs e)
        {
            (e.Node.Tag as MapFile).ShowEditor();
        }


    }
}
