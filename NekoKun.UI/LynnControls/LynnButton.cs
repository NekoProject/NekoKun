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
        StringFormat sf;

        public LynnButton()
        {
            if (UIManager.Enabled)
            {
                RecreateObjects();

                this.SetStyle(ControlStyles.UserPaint, true);
                this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            }
        }

        protected void RecreateObjects()
        {
            if (UIManager.Enabled)
            {
                this.size = new Size(this.ClientSize.Width, this.ClientSize.Height);

                pen = new Pen(Color.FromArgb(140, 160, 180));

                brushNormal = new LinearGradientBrush(
                    ClientRectangle,
                    Color.FromArgb(238, 246, 252),
                    Color.FromArgb(177, 185, 191),
                    LinearGradientMode.Vertical
                );

                brushDisabled = new SolidBrush(Color.FromArgb(228, 240, 252));

                sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;
                sf.Trimming = StringTrimming.EllipsisCharacter;
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            if (UIManager.Enabled)
            {
                if (!this.size.Equals(this.ClientSize))
                    RecreateObjects();

                Rectangle bounds = new Rectangle(0, 0, this.ClientSize.Width - 1, this.ClientSize.Height - 1);
                Rectangle bounds2 = new Rectangle(Point.Empty, this.ClientSize);
                pevent.Graphics.DrawRectangle(pen, bounds);
                bounds2.Inflate(-1, -1);

                if (!this.Enabled)
                    pevent.Graphics.FillRectangle(brushDisabled, bounds2);
                else
                {
                    pevent.Graphics.FillRectangle(brushNormal, bounds2);
                }

                pevent.Graphics.DrawString(this.Text, this.Font, Brushes.Black, bounds2, this.sf);
            }
            else
            {
                base.OnPaint(pevent); 
            }
        }
    }
}
