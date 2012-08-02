using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.UI
{
    public class Scintilla : ScintillaNet.Scintilla
    {
    	protected System.Drawing.Color back = System.Drawing.Color.FromArgb(191, 219, 255);
    	
        public Scintilla()
        {
            this.Font = Program.GetMonospaceFont();

            // left margin backcolor
			this.Styles[33].BackColor = back;
            this.Margins.FoldMarginColor = back;
        }
    }
}
