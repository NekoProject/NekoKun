using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.UI
{
    public class Scintilla : ScintillaNet.Scintilla
    {
        public Scintilla()
        {
            this.Font = Program.GetMonospaceFont();
        }
    }
}
