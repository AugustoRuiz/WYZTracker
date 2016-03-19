using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WYZTracker.Wpf.ViewModels;

namespace WYZTracker.Wpf.Commands
{
    public class AddPattern : UndoableAction
    {
        private int _currentPatIndex;
        private MainViewModel _model;

        public AddPattern(MainViewModel model)
        {
            this._model = model;
            this._currentPatIndex = model.PatternIndex;

            this.ExecuteAction = this.executeAction;
            this.UndoAction = this.undoAction;
        }

        private void executeAction()
        {
            this._model.PlayOrder.Insert(this._currentPatIndex + 1, this._model.PlayOrder[this._currentPatIndex]);
            this._model.PatternIndex = this._currentPatIndex + 1;
        }

        private void undoAction()
        {
            this._model.PlayOrder.RemoveAt(this._currentPatIndex + 1);
            this._model.PatternIndex = this._currentPatIndex;
        }
    }
}
