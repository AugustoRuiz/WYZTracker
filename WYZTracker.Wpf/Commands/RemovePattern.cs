using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WYZTracker.Wpf.ViewModels;

namespace WYZTracker.Wpf.Commands
{
    public class RemovePattern : UndoableAction
    {
        private MainViewModel _model;
        private int _currentPatIndex;
        private int _oldValue;

        public RemovePattern(MainViewModel model)
        {
            this._model = model;
            this._currentPatIndex = this._model.PatternIndex;
            this._oldValue = this._model.PlayOrder[this._currentPatIndex];

            this.ExecuteAction = this.executeAction;
            this.UndoAction = this.undoAction;
        }

        private void executeAction()
        {
            this._model.PlayOrder.RemoveAt(this._currentPatIndex);
            if (this._model.PlayOrder.Count > this._currentPatIndex)
            {
                this._model.PatternIndex = this._currentPatIndex;
            } else
            {
                this._model.PatternIndex = this._model.PlayOrder.Count - 1;
            }
        }

        private void undoAction()
        {
            this._model.PlayOrder.Insert(this._currentPatIndex, this._oldValue);
            this._model.PatternIndex = this._currentPatIndex;
        }
    }
}
