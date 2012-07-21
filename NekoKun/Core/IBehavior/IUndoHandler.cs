using System;
using System.Collections.Generic;
using System.Text;

namespace NekoKun
{
    public interface IUndoHandler
    {
        bool CanUndo { get; }
        bool CanRedo { get; }
        void Undo();
        void Redo();
    }
}
