using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

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

            Dictionary<string, System.Windows.Forms.TreeNode> nodes = new Dictionary<string,System.Windows.Forms.TreeNode>();
            List<System.Windows.Forms.TreeNode> order = new List<System.Windows.Forms.TreeNode>();

            foreach (var item in info.maps)
            {
                System.Windows.Forms.TreeNode node = new System.Windows.Forms.TreeNode(item.Value.Title);
                node.Tag = item.Value;
                nodes.Add(item.Key, node);
                order.Add(node);
            }

            order.Sort(
                delegate(System.Windows.Forms.TreeNode me, System.Windows.Forms.TreeNode other)
                {
                    return (me.Tag as MapFile).Order.CompareTo((other.Tag as MapFile).Order);
                }
            );

            foreach (System.Windows.Forms.TreeNode item in order)
            {
                MapFile map = item.Tag as MapFile;
                if (map.ParentID != null && nodes.ContainsKey(map.ParentID))
                    nodes[map.ParentID].Nodes.Add(item);
            }

            foreach (System.Windows.Forms.TreeNode item in order)
            {
                if (item.Parent == null)
                    this.tree.Nodes.Add(item);
            }

            tree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(tree_ItemDrag);
            tree.AllowDrop = true;
            tree.DragEnter += new System.Windows.Forms.DragEventHandler(tree_DragEnter);
            tree.DragDrop += new System.Windows.Forms.DragEventHandler(tree_DragDrop);
            tree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(tree_NodeMouseDoubleClick);
        }

        void tree_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            System.Windows.Forms.TreeNode NewNode;

            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
            {
                Point pt = ((System.Windows.Forms.TreeView)sender).PointToClient(new Point(e.X, e.Y));
                if (sender != tree)
                    return;
                TreeNode DestinationNode = ((TreeView)sender).GetNodeAt(pt);
                if (DestinationNode != null)
                {
                    NewNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
                    if (DestinationNode != NewNode)
                    {
                        NewNode.Remove();
                        DestinationNode.Nodes.Add(NewNode);
                    }
                }
            }
        }

        void tree_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false) && sender == this.tree)
            {
                e.Effect = System.Windows.Forms.DragDropEffects.Move;
            }
        }

        void tree_ItemDrag(object sender, System.Windows.Forms.ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, System.Windows.Forms.DragDropEffects.Move);
        }

        void tree_NodeMouseDoubleClick(object sender, System.Windows.Forms.TreeNodeMouseClickEventArgs e)
        {
            (e.Node.Tag as MapFile).ShowEditor();
        }


    }
}
