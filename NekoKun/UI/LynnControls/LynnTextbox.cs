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
        internal NativeWindow native;

        public LynnTextbox()
        {
            native = new LynnTextboxNative(this);
        }

    }
}
