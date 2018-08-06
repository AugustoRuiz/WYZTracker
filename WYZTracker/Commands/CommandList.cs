using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WYZTracker.Commands
{
    public class CommandList : Stack<ICommand>, INotifyPropertyChanged
    {
        private Stack<ICommand> _undoneCommands = new Stack<ICommand>();

        public event PropertyChangedEventHandler PropertyChanged;

        public bool CanUndo
        {
            get
            {
                return this.Count > 0;
            }
        }

        public bool CanRedo
        {
            get
            {
                return this._undoneCommands.Count > 0;
            }
        }

        public void Undo()
        {
            if (this.Peek() != null)
            {
                ICommand cmd = this.Pop();
                cmd.Undo();
                this._undoneCommands.Push(cmd);
                this.Notify();
            }
        }

        public void Redo()
        {
            if (this._undoneCommands.Peek() != null)
            {
                ICommand cmd = this._undoneCommands.Pop();
                cmd.Execute();
                this.Push(cmd);
                this.Notify();
            }
        }

        public void Execute(ICommand cmd)
        {
            cmd.Execute();
            this.Push(cmd);
            this.Notify();
            this._undoneCommands.Clear();
        }

        private void Notify()
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CanUndo"));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CanRedo"));
        }
    }
}
