using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace NekoKun.Core
{
    [PadDefaultLocation(WeifenLuo.WinFormsUI.Docking.DockState.DockRight)]
    public class ProjectExplorerPad : AbstractPad
    {
        public ProjectExplorerPad()
        {
            this.Text = "工程资源管理器";
            this.Controls.Add(new ProjectExplorerTreeView());
        }

        private class ProjectExplorerTreeView : UI.LynnTreeView
        {
            TreeNode root;
            System.IO.FileSystemWatcher watcher;
            bool OnlyProjectFile = true;
            List<string> projectFiles;

            public ProjectExplorerTreeView()
            {
                this.Dock = DockStyle.Fill;
                this.ImageList = CreateImageList();

                RefillTree();
                //watcher = new System.IO.FileSystemWatcher(ProjectManager.ProjectDir);
                //watcher.EnableRaisingEvents = false;
            }

            protected override void OnKeyDown(KeyEventArgs e)
            {
                if (e.KeyCode == Keys.O && e.Modifiers == Keys.None)
                {
                    OnlyProjectFile = !OnlyProjectFile;
                    RefillTree();
                }
                base.OnKeyDown(e);
            }

            void RefillTree()
            {
                projectFiles = new List<string>();
                FileManager.ForEach((o) =>
                {
                    if (o.filename.ToLowerInvariant().StartsWith(ProjectManager.ProjectDir.ToLowerInvariant()))
                    {
                        projectFiles.Add(o.filename.ToLowerInvariant());
                    }
                });

                this.Nodes.Clear();

                root = new TreeNode();
                root.Text = ProjectManager.ProjectDocument.ToString();
                root.Name = ProjectManager.ProjectDir;
                root.ImageKey = "ProjectGeneric";
                root.SelectedImageKey = "ProjectGeneric";
                root.Tag = ProjectManager.ProjectDocument;
                this.Nodes.Add(root);
                PopulateFolder(root);
                root.Expand();
            }

            protected override void OnAfterCollapse(TreeViewEventArgs e)
            {
                if (e.Node.ImageKey.IndexOf("Open") >= 0)
                {
                    e.Node.ImageKey = e.Node.ImageKey.Replace("Open", "Closed");
                    e.Node.SelectedImageKey = e.Node.ImageKey;
                }
                base.OnAfterCollapse(e);
            }

            protected override void OnAfterExpand(TreeViewEventArgs e)
            {
                if (e.Node.ImageKey.IndexOf("Closed") >= 0)
                {
                    e.Node.ImageKey = e.Node.ImageKey.Replace("Closed", "Open");
                    e.Node.SelectedImageKey = e.Node.ImageKey;
                }
                base.OnAfterExpand(e);
            }

            protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
            {
                if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Name == "*" && e.Node.Nodes[0].Text == "*")
                {
                    PopulateFolder(e.Node);
                }
                base.OnBeforeExpand(e);
            }

            bool IsContaindInProject(string f)
            {
                string filename = f.ToLowerInvariant();
                return !projectFiles.TrueForAll((o) => !o.StartsWith(filename));
            }

            TreeNode CreateFileNode(string filename, TreeNode parent)
            {
                if (parent == root && filename == ProjectManager.ProjectDocument.filename) return null;
                bool cont = IsContaindInProject(filename);
                if (OnlyProjectFile && !cont) return null;

                AbstractFile f;
                if (FileManager.TryFind(filename, out f))
                {
                    if (OnlyProjectFile && f.IsHidden) return null;
                }

                TreeNode node = new TreeNode();
                node.ImageKey = GetFileIcon(filename);
                if (!cont || (f != null && f.IsHidden))
                    node.ImageKey = "FileHidden";

                node.SelectedImageKey = node.ImageKey;
                node.Name = filename;
                node.Text = filename.Substring(parent.Name.Length + 1);

                if (f != null)
                {
                    node.Tag = f;
                    node.ImageKey = "FileForm";
                    node.SelectedImageKey = node.ImageKey;
                    node.Text = f.ToString() + " (" + node.Text + ")";
                    if (f.IsSubfileProvider)
                        node.Nodes.Add(CreateDummyNode());
                }
                parent.Nodes.Add(node);
                return node;
            }

            private string GetFileIcon(string filename)
            {
                string ext = System.IO.Path.GetExtension(filename).ToLowerInvariant().Replace(".", "");
                if (ext == "jpg" || ext == "jpeg" || ext == "bmp" || ext == "png" || ext == "gif")
                    return "FileImage";
                if (ext == "rb" || ext == "py" || ext == "js" || ext == "vbs")
                    return "FileScript";

                /*
                string key = "File." + ext;
                if (ext == "exe" || ext == "ico" || ext == "cur" || ext == "ani")
                    key = "File" + BitConverter.ToString(System.Text.Encoding.Unicode.GetBytes(filename));

                if (this.ImageList.Images.ContainsKey(key))
                    return key;
                else
                {
                    this.ImageList.Images.Add(key, System.Drawing.Icon.ExtractAssociatedIcon(filename));
                    return key;
                }
                */

                return "File";
            }

            TreeNode CreateDummyNode()
            {
                TreeNode node = new TreeNode();
                node.Name = "*";
                node.Text = "*";
                return node;
            }

            TreeNode CreateFolderNode(string path, TreeNode parent)
            {
                bool cont = IsContaindInProject(path);
                if (OnlyProjectFile && !cont) return null;

                TreeNode node = new TreeNode();
                node.ImageKey = "FolderClosed";
                if (!OnlyProjectFile && !cont)
                    node.ImageKey = "FolderClosedHidden";

                node.SelectedImageKey = node.ImageKey;
                node.Name = path;
                node.Text = path.Substring(parent.Name.Length + 1);
                if (System.IO.Directory.GetFileSystemEntries(node.Name).Length > 0)
                    node.Nodes.Add(CreateDummyNode());
                parent.Nodes.Add(node);
                return node;
            }

            void PopulateFolder(TreeNode node)
            {
                node.Nodes.Clear();
                if (node == root || node.Tag == null)
                {
                    string[] subdirs = System.IO.Directory.GetDirectories(node.Name);
                    Array.ForEach<string>(subdirs, (o) => CreateFolderNode(o, node));

                    string[] files = System.IO.Directory.GetFiles(node.Name);
                    Array.ForEach<string>(files, (o) => CreateFileNode(o, node));
                }
                else
                {
                    AbstractFile[] files = (node.Tag as AbstractFile).Subfiles;
                    if (files != null)
                        Array.ForEach<AbstractFile>(files, (o) => CreateVirtualFileNode(o, node));
                }
            }

            TreeNode CreateVirtualFileNode(AbstractFile f, TreeNode parent)
            {
                TreeNode node = new TreeNode();
                node.ImageKey = "FileForm";
                node.SelectedImageKey = node.ImageKey;
                node.Tag = f;
                node.Name = f.filename;
                node.Text = f.ToString();

                if (f.IsSubfileProvider)
                    node.Nodes.Add(CreateDummyNode());

                parent.Nodes.Add(node);

                return node;
            }

            ImageList CreateImageList()
            {
                var t = new ImageList();
                t.ColorDepth = ColorDepth.Depth32Bit;
                t.ImageSize = new System.Drawing.Size(16, 16);
                t.TransparentColor = Color.Fuchsia;
                t.Images.Add("ProjectGeneric", global::NekoKun.UI.Properties.Resources.ProjectGeneric);
                t.Images.Add("FolderClosed", global::NekoKun.UI.Properties.Resources.FolderClosed);
                t.Images.Add("FolderClosedHidden", global::NekoKun.UI.Properties.Resources.FolderClosedHidden);
                t.Images.Add("FolderClosedVirtual", global::NekoKun.UI.Properties.Resources.FolderClosedVirtual);
                t.Images.Add("FolderOpen", global::NekoKun.UI.Properties.Resources.FolderOpen);
                t.Images.Add("FolderOpenHidden", global::NekoKun.UI.Properties.Resources.FolderOpenHidden);
                t.Images.Add("FolderOpenVirtual", global::NekoKun.UI.Properties.Resources.FolderOpenVirtual);
                t.Images.Add("File", global::NekoKun.UI.Properties.Resources.File);
                t.Images.Add("FileHidden", global::NekoKun.UI.Properties.Resources.FileHidden);
                t.Images.Add("FileGenerated", global::NekoKun.UI.Properties.Resources.FileGenerated);
                t.Images.Add("FileForm", global::NekoKun.UI.Properties.Resources.FileForm);
                t.Images.Add("FileImage", global::NekoKun.UI.Properties.Resources.FileImage);
                t.Images.Add("FileScript", global::NekoKun.UI.Properties.Resources.FileScript);
                t.Images.Add("FileXML", global::NekoKun.UI.Properties.Resources.FileXML);
                return t;
            }
        }
    }
}
