using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun.RPGMaker
{
    public class DatabaseListbox : UI.LynnListbox
    {
        protected override string GetString(int id)
        {
            return String.Format("{0:D3}: {1}", id + 1, this.Items[id].ToString());
        }
    }
}
