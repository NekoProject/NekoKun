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

            this.listbox.Items.Add("新建工程 . . . ");
            this.listbox.Items.Add("打开已有工程 . . . ");

            if ((projects = SettingsManager.GlobalSettings["DocumentFramework.MRUProjects"] as List<string>) != null)
            {
                this.listbox.Items.AddRange(projects.ToArray());
            }

            this.listbox.MouseMove += new MouseEventHandler(listbox_MouseMove);
            this.listbox.MouseClick += new MouseEventHandler(listbox_MouseClick);
            this.listbox.KeyDown += new KeyEventHandler(listbox_KeyDown);
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
            throw new NotImplementedException();
        }

        void listbox_MouseMove(object sender, MouseEventArgs e)
        {
            this.listbox.SelectedItem = null;
        }
    }
}
