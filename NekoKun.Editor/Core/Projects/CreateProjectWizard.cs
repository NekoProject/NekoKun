using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace NekoKun
{
    public partial class CreateProjectWizard : UI.LynnForm
    {
        public string FileName;
        public List<ArchiveFile> files;
        protected System.Text.RegularExpressions.Regex invalidChars = new System.Text.RegularExpressions.Regex("[{" + System.Text.RegularExpressions.Regex.Escape(new string(Path.GetInvalidFileNameChars())) + "}]", System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.CultureInvariant);

        public CreateProjectWizard()
        {
            InitializeComponent();

            string[] filelist = StorageManager.GetVirtualDirectoryFiles("Templates", "*.nkar", System.IO.SearchOption.AllDirectories);

            this.templateList.LargeImageList = new ImageList();
            this.templateList.LargeImageList.ColorDepth = ColorDepth.Depth32Bit;
            this.templateList.LargeImageList.ImageSize = new Size(48, 48);
            this.templateList.SmallImageList = new ImageList();
            this.templateList.SmallImageList.ColorDepth = ColorDepth.Depth32Bit;
            this.templateList.SmallImageList.ImageSize = new Size(16, 16);
            this.templateList.Columns.Add("名称");
            this.templateList.Columns.Add("版本");
            this.templateList.Columns.Add("语言");
            this.templateList.Columns.Add("备注");
            this.templateList.View = View.Details;
            this.templateList.MultiSelect = false;
            this.templateList.FullRowSelect = true;
            this.templateList.HideSelection = false;
            this.templateList.Sorting = SortOrder.Ascending;

            foreach (string filename in filelist)
            {
                try
                {
                    ArchiveFile file = new ArchiveFile(filename);
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(file.Manifest);

                    XmlNode root = xmldoc["NekoKunProjectTemplate"];
                    if (new Version(root.Attributes["Version"].Value) > new Version(((System.Reflection.Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(System.Reflection.AssemblyFileVersionAttribute), false)[0] as System.Reflection.AssemblyFileVersionAttribute).Version)))
                        throw new ArgumentException("The project template file was created in a newer version of NekoKun which is not supported by this version of NekoKun.");

                    ListViewItem lvi = new ListViewItem();
                    lvi.Tag = file;
                    lvi.Text = root["Title"].InnerText;
                    lvi.SubItems.Add(root["Version"] != null ? root["Version"].InnerText : "");
                    lvi.SubItems.Add(root["Language"] != null ? root["Language"].InnerText : "");
                    lvi.SubItems.Add(root["Description"] != null ? root["Description"].InnerText : "");
                    lvi.ToolTipText = root["Description"] != null ? root["Description"].InnerText : "";
                    if (root["Icon"] != null)
                    {
                        Image image = Program.DecodeBase64Image(root["Icon"].InnerText);

                        this.templateList.LargeImageList.Images.Add(filename, image);
                        this.templateList.SmallImageList.Images.Add(filename, image);
                        lvi.ImageKey = filename;
                    }
                    
                    this.templateList.Items.Add(lvi);
                }
                catch { }
            }

            if (this.templateList.Items.Count >= 1)
                this.templateList.Items[0].Selected = true;

            this.templateList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            FillFields();
            this.fieldName.TextChanged += new EventHandler(fieldName_TextChanged);
            this.browseFolderButton.Click += new EventHandler(browseFolderButton_Click);
            UpdateAccept();

            this.templateList.ItemSelectionChanged += delegate { UpdateAccept(); };
            this.fieldLocation.TextChanged += delegate { UpdateAccept(); };
            this.fieldName.TextChanged += delegate { UpdateAccept(); };

            this.accpetButton.Click += new EventHandler(accpetButton_Click);
        }

        void accpetButton_Click(object sender, EventArgs e)
        {
            UpdateAccept();
            if (!this.accpetButton.Enabled)
                return;

            try
            {
                this.FileName = ProjectManager.CreateProject(this.fieldLocation.Text, this.templateList.SelectedItems[0].Tag as ArchiveFile);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "无法创建工程。\n\n" + Program.ExceptionMessage(ex), "NekoKun", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void browseFolderButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请为工程选择一个目录，工程将创建于所选目录之下。";
            try
            {
                dialog.SelectedPath = System.IO.Path.GetDirectoryName(this.fieldLocation.Text);
            }
            catch { }
            dialog.ShowNewFolderButton = true;
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                this.fieldLocation.Text = System.IO.Path.Combine(
                    dialog.SelectedPath,
                    invalidChars.Replace(this.fieldName.Text, "")
                );
            }
        }

        void fieldName_TextChanged(object sender, EventArgs e)
        {
            string name = invalidChars.Replace(this.fieldName.Text, "");
            if (name.Length == 0)
                return;

            try
            {
                this.fieldLocation.Text = System.IO.Path.Combine(
                    System.IO.Path.GetDirectoryName(this.fieldLocation.Text),
                    name
                );
            }
            catch { }
        }

        void FillFields()
        {
            this.fieldName.Text = StorageManager.GetNextName(StorageManager.GetUserDirectory("Projects"), "Project{0}");
            this.fieldLocation.Text = System.IO.Path.Combine(
                StorageManager.GetUserDirectory("Projects"),
                invalidChars.Replace(this.fieldName.Text, "")
            );
        }

        void UpdateAccept()
        {
            bool can = true;
            can = can && (this.fieldName.Text.Length > 0);
            can = can && !System.IO.Directory.Exists(this.fieldLocation.Text);
            try
            {
                System.IO.Path.GetFullPath(System.IO.Path.GetDirectoryName(this.fieldLocation.Text));
                can = can && true;
            } catch {
                can = can && false;
            }
            can = can && this.templateList.SelectedItems.Count == 1;
            this.accpetButton.Enabled = can;
        }
    }
}
