using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NekoKun
{
    public partial class ScriptListEditorRenameDialog : UI.LynnForm
    {
        ScriptListFile scriptListFile;
        ScriptFile file;
        public string result = "";

        public ScriptListEditorRenameDialog(ScriptListFile scriptListFile, ScriptFile file)
        {
            this.scriptListFile = scriptListFile;
            this.file = file;

            InitializeComponent();

            this.textBox3.Text = this.file.ToString();
            this.textBox1.Text = this.file.ToString();

            this.Shown += new EventHandler(ScriptListEditorRenameDialog_Shown);
        }

        void ScriptListEditorRenameDialog_Shown(object sender, EventArgs e)
        {
            this.textBox1.Focus();
            this.textBox1.SelectAll();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.textBox2.Text = this.scriptListFile.GenerateFileName(this.textBox1.Text);
                this.button1.Enabled = true;
            }
            catch(Exception ex)
            {
                this.textBox2.Text = ex.Message;
                this.button1.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            result = this.textBox1.Text;
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
