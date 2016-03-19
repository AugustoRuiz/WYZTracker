using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WYZTracker.Wpf
{
    public class UndoManager : System.ComponentModel.INotifyPropertyChanged
    {
        private Stack<UndoableAction> _undoCommands = new Stack<UndoableAction>();
        private Stack<UndoableAction> _redoCommands = new Stack<UndoableAction>();

        public static readonly UndoManager Instance = new UndoManager();

        public event PropertyChangedEventHandler PropertyChanged;

        public static void Execute(UndoableAction cmd)
        {
            cmd.Execute();
            Instance.addToUndo(cmd);
        }

        public static void Undo()
        {
            UndoableAction cmd = Instance.removeFromUndo();
            cmd.Undo();
            Instance.addToRedo(cmd);
        }

        public static void Redo()
        {
            UndoableAction cmd = Instance.removeFromRedo();
            cmd.Execute();
            Instance.addToUndo(cmd);
        }

        public static bool CanUndo { get { return Instance._undoCommands.Count > 0; } }
        public static bool CanRedo { get { return Instance._redoCommands.Count > 0; } }

        #region Stack management functions 

        private void addToUndo(UndoableAction c)
        {
            _undoCommands.Push(c);
            if (_undoCommands.Count == 1)
            {
                OnPropertyChanged("CanUndo");
            }
        }

        private UndoableAction removeFromUndo()
        {
            UndoableAction result = _undoCommands.Pop();
            if (_undoCommands.Count == 0)
            {
                OnPropertyChanged("CanUndo");
            }
            return result;
        }

        private void addToRedo(UndoableAction c)
        {
            _redoCommands.Push(c);
            if (_redoCommands.Count == 1)
            {
                OnPropertyChanged("CanRedo");
            }
        }

        private UndoableAction removeFromRedo()
        {
            UndoableAction result = _redoCommands.Pop();
            if (_redoCommands.Count == 0)
            {
                OnPropertyChanged("CanRedo");
            }
            return result;
        }

        #endregion

        #region Property changed notification

        protected void OnPropertyChanged(string propName)
        {
            PropertyChangedEventHandler tmp = this.PropertyChanged;
            if (tmp != null)
            {
                PropertyChangedEventArgs args = new PropertyChangedEventArgs(propName);
                tmp(this, args);
            }
        }

        #endregion 

    }
}
