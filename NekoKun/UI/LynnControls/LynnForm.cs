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
        protected LinearGradientBrush brush;

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
            if (this.ClientRectangle.Width == 0 && this.ClientRectangle.Height == 0)
                return;

            if (brush == null || !brush.Rectangle.Equals(this.ClientRectangle))
                brush = new System.Drawing.Drawing2D.LinearGradientBrush(this.ClientRectangle, this.back1, this.back2, LinearGradientMode.Vertical);
            
            try
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
            catch { }
        }
    }
}
