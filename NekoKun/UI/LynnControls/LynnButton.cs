using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace NekoKun.UI
{
    public class LynnButton : Button
    {
        Size size;
        Pen pen;
        Brush brushNormal;
        Brush brushActive;
        Brush brushDisabled;

        public LynnButton()
        {
            RecreateObjects();

            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected void RecreateObjects()
        {
            this.size = new Size(this.ClientSize.Width, this.ClientSize.Height);

            pen = new Pen(Color.FromArgb(140, 160, 180));

            brushNormal = new LinearGradientBrush(
                ClientRectangle,
                Color.FromArgb(238, 246,  252),
                Color.FromArgb(177, 185, 191),
                LinearGradientMode.Vertical
            );
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            if (!this.size.Equals(this.ClientSize))
                RecreateObjects();

            Rectangle bounds = new Rectangle(0, 0, this.ClientSize.Width - 1, this.ClientSize.Height - 1);
            Rectangle bounds2 = new Rectangle(Point.Empty, this.ClientSize);
            pevent.Graphics.DrawRectangle(pen, bounds);
            //bounds.Inflate(-1, -1);
            bounds2.Inflate(-1, -1);
            pevent.Graphics.FillRectangle(brushNormal, bounds2);

            // base.OnPaint(pevent);
        }
    }
}
