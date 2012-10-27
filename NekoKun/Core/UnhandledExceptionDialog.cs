using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NekoKun.Core
{
    public partial class UnhandledExceptionDialog : NekoKun.UI.LynnForm
    {
        public Exception e;
        public UnhandledExceptionDialog(Exception e)
        {
            this.e = e;

            InitializeComponent();

            this.Text = "Unhandled Exception occured in NekoKun";
            this.StartPosition = FormStartPosition.CenterParent;
            this.lynnTextbox1.Font = Program.GetMonospaceFont();
            this.lynnTextbox1.Text = Program.ExceptionMessage(e).Replace("\r", "").Replace("\n", System.Environment.NewLine);
            this.lynnTextbox1.ReadOnly = true;
        }
    }
}
