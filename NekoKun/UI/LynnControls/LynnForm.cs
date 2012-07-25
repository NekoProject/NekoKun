using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace NekoKun.UI
{
    public class LynnForm : Form
    {
        protected Color back1 = Color.FromArgb(227, 239, 251);
        protected Color back2 = Color.FromArgb(196, 208, 220);

        public LynnForm()
        {
            this.Font = System.Drawing.SystemFonts.MessageBoxFont;
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            SetFont(e.Control);
            base.OnControlAdded(e);
        }

        protected void SetFont(Control e)
        {
            if (e is Scintilla)
                e.Font = Program.GetMonospaceFont();
            else
                e.Font = this.Font;

            if (e is Panel || e is SplitContainer)
            {
                e.BackColor = Color.Transparent;// back1;
            }

            if (e.Controls.Count != 0)
                foreach (Control item in e.Controls)
                {
                    SetFont(item);
                }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // base.OnPaintBackground(e);
            e.Graphics.FillRectangle(new System.Drawing.Drawing2D.LinearGradientBrush(this.ClientRectangle, this.back1, this.back2, LinearGradientMode.Vertical), this.ClientRectangle);
        }
    }
}
