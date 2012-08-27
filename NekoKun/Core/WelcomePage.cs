using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NekoKun
{
    public partial class WelcomePage : NekoKun.UI.LynnForm
    {
        public string Result;
        protected List<string> projects;

        public WelcomePage()
        {
            InitializeComponent();

            projects = SettingsManager.GlobalSettings["DocumentFramework.MRUProjects"] as List<string> ?? new List<string>();
            ReloadListbox();

            this.listbox.MouseMove += new MouseEventHandler(listbox_MouseMove);
            this.listbox.MouseClick += new MouseEventHandler(listbox_MouseClick);
            this.listbox.KeyDown += new KeyEventHandler(listbox_KeyDown);
        }

        private void ReloadListbox()
        {
            this.listbox.Items.Clear();
            this.listbox.Items.Add("新建工程 . . . ");
            this.listbox.Items.Add("打开已有工程 . . . ");
            this.listbox.Items.AddRange(projects.ToArray());
        }

        void listbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.None && e.KeyCode == Keys.Enter)
            {
                ParseIndex(this.listbox.SelectedIndex);
            }
        }

        void listbox_MouseClick(object sender, MouseEventArgs e)
        {
            ParseIndex(this.listbox.IndexFromPoint(e.Location));
        }

        private void ParseIndex(int p)
        {
            if (p == 0)
            {
                CreateProjectWizard wizard = new CreateProjectWizard();
                if (wizard.ShowDialog(this) == DialogResult.OK)
                {
                    SetResult(wizard.FileName);
                }
            }
            else if (p == 1)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.AddExtension = true;
                //dialog.AutoUpgradeEnabled = true;
                dialog.CheckFileExists = true;
                dialog.CheckPathExists = true;
                dialog.DefaultExt = "nkproj";
                dialog.DereferenceLinks = true;
                dialog.Filter = "NekoKun 工程 (*.nkproj)|*.nkproj";
                dialog.Multiselect = false;
                dialog.ShowReadOnly = false;
                dialog.SupportMultiDottedExtensions = true;
                dialog.ValidateNames = true;
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    SetResult(dialog.FileName);
                }
            }
            else if (p >= 2 && p <= this.projects.Count + 2)
            {
                SetResult(this.projects[p - 2]);
            }
            //throw new NotImplementedException();
        }

        void AddMRU(string filename)
        {
            List<string> newp = new List<string>();
            if (System.IO.File.Exists(filename))
                newp.Add(System.IO.Path.GetFullPath(filename));

            foreach (string item in this.projects)
            {
                if (System.IO.File.Exists(item) && (!newp.Contains(System.IO.Path.GetFullPath(item))))
                {
                    newp.Add(item);
                }
            }
            this.projects = newp;

            SettingsManager.GlobalSettings["DocumentFramework.MRUProjects"] = this.projects;
            SettingsManager.GlobalSettings.Commit();
        }

        void SetResult(string filename)
        {
            AddMRU(filename);
            if (!System.IO.File.Exists(filename))
            {
                MessageBox.Show(this, "无法找到工程文件“" + filename + "”。", "NekoKun", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.ReloadListbox();
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Result = filename;
            this.Close();
        }

        void listbox_MouseMove(object sender, MouseEventArgs e)
        {
            this.listbox.SelectedItem = null;
        }
    }
}
