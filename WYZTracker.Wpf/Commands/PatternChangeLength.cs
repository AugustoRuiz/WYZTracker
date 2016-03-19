using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WYZTracker.Wpf.Commands
{
    public class PatternChangeLength : UndoableAction
    {
        private Pattern pattern;
        private int length;
        private ChannelLine[] oldLines;

        public PatternChangeLength(Pattern pattern, int newLength)
        {
            this.pattern = pattern;
            this.oldLines = Utils.Clone(pattern.Lines);
            this.length = newLength;
            this.ExecuteAction = this.changeLength;
            this.UndoAction = this.restoreLength;
        }

        private void changeLength()
        {
            pattern.Length = this.length;
        }

        private void restoreLength()
        {
            pattern.Lines = oldLines;
        }
    }
}
