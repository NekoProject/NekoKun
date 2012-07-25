using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace NekoKun.UI
{
    public class LynnNumericUpDown : NumericUpDown
    {
        internal NativeWindow native;

        public LynnNumericUpDown()
        {
            native = new NativeBorder(this, 0xf /* WM_PAINT */, true, false);
        }

    }
}
