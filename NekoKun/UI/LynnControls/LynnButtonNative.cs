using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace NekoKun.UI
{
    internal class LynnButtonNative : NativeWindow
    {
        Control CON;
        Pen pen, pen2;

        public LynnButtonNative(Control con)
        {
            CON = con;

            pen = new Pen(Color.FromArgb(140, 160, 180));
            pen2 = new Pen(CON.BackColor);

            this.AssignHandle(CON.Handle);
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0xf: // WM_PAINT
                    if (pen2.Color != CON.BackColor) { pen2 = new Pen(CON.BackColor); }
                    base.WndProc(ref m);

                    IntPtr hDC = NativeMethods.GetWindowDC(this.Handle);
                    Graphics g = Graphics.FromHdc(hDC);
                    Rectangle bounds = new Rectangle(0, 0, this.CON.Size.Width - 1, this.CON.Size.Height - 1);
                    g.DrawRectangle(pen, bounds);
                    bounds.Inflate(-1, -1);
                    g.DrawRectangle(pen2, bounds);
                    bounds.Inflate(-1, -1);
                    g.DrawRectangle(pen2, bounds);

                    NativeMethods.ReleaseDC(this.Handle, hDC);
                    g.Dispose();

                    m.Result = (IntPtr)1;
                    return;
                default:
                    break;
            }
            base.WndProc(ref m);
        }
    }
}
