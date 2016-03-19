using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WYZTracker.Wpf.ViewModels;

namespace WYZTracker.Wpf.Commands
{
    public class NextPattern : UndoableAction
    {
        private MainViewModel _model;
        private List<Pattern> _oldPatterns;
        private int _oldPatVal;
        private int _currentPatIndex;
        private int _direction;

        public NextPattern(MainViewModel model, int direction)
        {
            this._model = model;
            this._oldPatterns = Utils.Clone(this._model.Song.Patterns);
            this._currentPatIndex = this._model.PatternIndex;
            this._direction = direction;
            this._oldPatVal = this._model.PlayOrder[this._currentPatIndex];

            this.ExecuteAction = this.setNext;
            this.UndoAction = this.restore;
        }

        private void setNext()
        {
            int newVal = this._oldPatVal + _direction;
            if (this._model.Song.Patterns.Count <= newVal)
            {
                this._model.Song.Patterns.Add(new Pattern(this._model.Song.Channels));
            }
            if(newVal >=0 && newVal < this._model.Song.Patterns.Count)
            {
                this._model.PlayOrder[this._currentPatIndex] = newVal;
            }
        }

        private void restore()
        {
            this._model.PlayOrder[this._currentPatIndex] = this._oldPatVal;
            this._model.Song.Patterns = this._oldPatterns;
        }

    }
}
