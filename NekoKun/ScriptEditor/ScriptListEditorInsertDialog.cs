using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NekoKun
{
    public partial class ScriptListEditorInsertDialog : UI.LynnForm
    {
        ScriptListFile scriptListFile;
        public string result = "";

        public ScriptListEditorInsertDialog(ScriptListFile scriptListFile)
        {
            this.scriptListFile = scriptListFile;

            InitializeComponent();
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
