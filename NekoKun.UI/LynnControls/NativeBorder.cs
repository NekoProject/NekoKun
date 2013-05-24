using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace NekoKun.UI
{
    internal class NativeBorder : NativeWindow
    {
        Control CON;
        Pen pen, pen2;
        int msg;
        bool before, after;

        public NativeBorder(Control con, int msg)
        {
            CON = con;
            
            this.msg = msg;

            pen = new Pen(Color.FromArgb(140, 160, 180));
            pen2 = new Pen(CON.BackColor);

            this.AssignHandle(CON.Handle);
        }

        public NativeBorder(Control con, int msg, bool before, bool after)
            : this(con, msg)
        {
            this.before = before;
            this.after = after;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == msg)
            {
                if (this.before) base.WndProc(ref m);

                if (pen2.Color != CON.BackColor) { pen2 = new Pen(CON.BackColor); }

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
                if (!this.after) return;
            }
            base.WndProc(ref m);
        }
    }
}
