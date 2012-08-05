using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using WeifenLuo.WinFormsUI.Docking;

namespace NekoKun.UI
{
    public class LynnDockContent : DockContent
    {
        //protected static Image backgroundImage = Program.DecodeBase64Image("iVBORw0KGgoAAAANSUhEUgAAAAYAAAAGCAMAAADXEh96AAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAAZQTFRF5/L90Oz9VJBYwwAAABhJREFUeNpiYGQAAkYYAhJgEkohiwMEGAABJgANVHFu+gAAAABJRU5ErkJggg==");
        protected Color back = Color.FromArgb(191, 219, 255); //Color.FromArgb(231, 242, 253);

        public LynnDockContent()
        {
            this.BackColor = back;
            //this.BackgroundImage = backgroundImage;
            //this.BackgroundImageLayout = ImageLayout.Tile;
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
            {
                Scintilla sci = e as Scintilla;
                e.Font = Program.GetMonospaceFont();
            }
            else
                e.Font = this.Font;

            if (e is TabPage)
            {
                e.BackColor = Color.White;
                //e.BackgroundImage = backgroundImage;
                //e.BackgroundImageLayout = ImageLayout.Tile;
            }
            else if (e is Panel || e is SplitContainer)
                e.BackColor = Color.Transparent;// back1;

            if (e.Controls.Count != 0)
                foreach (Control item in e.Controls)
                {
                    SetFont(item);
                }
        }
    }
}
