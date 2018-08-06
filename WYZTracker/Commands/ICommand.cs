using System;
using System.Collections.Generic;
using System.Text;

namespace WYZTracker.Commands
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}
