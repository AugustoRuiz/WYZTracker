using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WYZTracker.Wpf.ViewModels;

namespace WYZTracker.Wpf.Commands
{
    public class MovePattern : UndoableAction
    {
        private MainViewModel _model;
        private List<Pattern> _oldPatterns;
        private int _currentPatIndex;
        private int _direction;

        public MovePattern(MainViewModel model, int direction)
        {
            this._model = model;
            this._oldPatterns = Utils.Clone(this._model.Song.Patterns);
            this._currentPatIndex = this._model.PatternIndex;
            this._direction = direction;

            this.ExecuteAction = this.executeAction;
            this.UndoAction = this.undoAction;
        }

        private void executeAction()
        {
            int tmp = this._model.PlayOrder[this._currentPatIndex];
            this._model.PlayOrder[this._currentPatIndex] = this._model.PlayOrder[this._currentPatIndex + this._direction];
            this._model.PlayOrder[this._currentPatIndex + this._direction] = tmp;
            this._model.PatternIndex = this._currentPatIndex + this._direction;
        }

        private void undoAction()
        {
            int tmp = this._model.PlayOrder[this._currentPatIndex];
            this._model.PlayOrder[this._currentPatIndex] = this._model.PlayOrder[this._currentPatIndex + this._direction];
            this._model.PlayOrder[this._currentPatIndex + this._direction] = tmp;
            this._model.PatternIndex = this._currentPatIndex;
        }
    }
}
