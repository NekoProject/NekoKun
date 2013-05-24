using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NekoKun
{
    public partial class FindAllDialog : UI.LynnForm
    {
        public string Keyword;

        public FindAllDialog()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Keyword = this.textBox1.Text;
            this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.button1.Enabled = this.textBox1.Text != null;
        }
    }
}
