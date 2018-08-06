using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WYZTracker.Wpf
{
    public class UndoableAction 
    {
        public Action ExecuteAction { get; set; }
        public Action UndoAction { get; set; }

        public void Execute()
        {
            this.ExecuteAction();
        }

        public void Undo()
        {
            this.UndoAction?.Invoke();
        }
    }
}
