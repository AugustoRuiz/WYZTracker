using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WYZTracker.Wpf.ViewModels;

namespace WYZTracker.Wpf.Commands
{
    public class ClonePattern : UndoableAction
    {
        private MainViewModel _model;
        private List<Pattern> _oldPatterns;
        private int _currentPatIndex;

        public ClonePattern(MainViewModel model)
        {
            this._model = model;

            this._oldPatterns = Utils.Clone(model.Song.Patterns);
            this._currentPatIndex = model.PatternIndex;

            this.ExecuteAction = this.executeAction;
            this.UndoAction = this.undoAction;
        }

        private void executeAction()
        {
            this._model.Song.Patterns.Add(Utils.Clone(this._model.Song.Patterns[this._model.PlayOrder[this._currentPatIndex]]));
            this._model.PlayOrder.Insert(this._currentPatIndex + 1, this._model.Song.Patterns.Count - 1);
            this._model.PatternIndex = this._currentPatIndex + 1;
        }

        private void undoAction()
        {
            this._model.Song.Patterns = _oldPatterns;
            this._model.PlayOrder.RemoveAt(this._currentPatIndex + 1);
            this._model.PatternIndex = this._currentPatIndex;
        }
    }
}
