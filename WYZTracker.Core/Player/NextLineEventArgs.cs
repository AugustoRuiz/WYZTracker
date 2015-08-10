using System;
using System.Collections.Generic;
using System.Text;

namespace WYZTracker
{
    public class NextLineEventArgs
    {
        public NextLineEventArgs(int patternNumber, int lineNumber)
        {
            _patternNumber = patternNumber;
            _lineNumber = lineNumber;
        }

        private int _patternNumber;

        public int PatternNumber
        {
            get { return _patternNumber; }
        }

        private int _lineNumber;

        public int LineNumber
        {
            get { return _lineNumber; }
        }
    }
}
