using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.UI
{
    public class Scintilla : ScintillaNET.Scintilla
    {
    	protected System.Drawing.Color back = System.Drawing.Color.FromArgb(191, 219, 255);
    	
        public Scintilla()
        {
            this.Font = Program.GetMonospaceFont();
            this.Styles.Default.Font = this.Font;
            for (int i = 0; i < 200; i++)
            {
                this.Styles[i].Font = this.Font;
            }
            // left margin backcolor
			this.Styles[33].BackColor = back;
            this.Margins.FoldMarginColor = back;
        }
    }
}
