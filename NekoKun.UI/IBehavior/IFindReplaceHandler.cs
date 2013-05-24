using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun
{
    public interface IFindReplaceHandler
    {
        bool CanShowFindDialog { get; }
        bool CanShowReplaceDialog { get; }
        
        void ShowFindDialog();
        void ShowReplaceDialog();
    }
}
