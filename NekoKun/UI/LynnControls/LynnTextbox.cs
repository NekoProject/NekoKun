using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace NekoKun.UI
{
    public class LynnTextbox : TextBox
    {
        internal NativeWindow native, native2;

        public LynnTextbox()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            native2 = new NativeBorder(this, 0xf /* WM_PAINT */, true, false);
            native = new NativeBorder(this, 0x85 /* WM_NCPAINT */);
        }

    }
}
