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
        NativeWindow native;
        public LynnButton()
        {
            native = new LynnButtonNative(this);
        }
    }
}
