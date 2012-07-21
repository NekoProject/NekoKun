using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun
{
    public interface IClipboardHandler
    {
        bool CanCut { get; }
        bool CanCopy { get; }
        bool CanPaste { get; }
        void Cut();
        void Copy();
        void Paste();
    }
}
