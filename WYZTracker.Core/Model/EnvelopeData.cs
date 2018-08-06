using System;
using System.Collections.Generic;
using System.Text;

namespace WYZTracker
{
    public class EnvelopeData
    {
        private int frequencyRatio = 8;

        public int FrequencyRatio
        {
            get { return frequencyRatio; }
            set { frequencyRatio = value; }
        }

        private int style = 0xC;

        public int Style
        {
            get { return style; }
            set { style = value; }
        }

        private bool activeFreqs = false;

        public bool ActiveFrequencies
        {
            get { return activeFreqs; }
            set { activeFreqs = value; }
        }

        public static bool Compare(EnvelopeData e1, EnvelopeData e2)
        {
            return (null == e1 && null == e2) ||
                   ((null != e1 && null != e2) &&
                    (e1.ActiveFrequencies == e2.ActiveFrequencies) &&
                    (e1.FrequencyRatio == e2.FrequencyRatio) &&
                    (e1.Style == e2.Style));
        }

        public EnvelopeData Clone()
        {
            return new EnvelopeData()
            {
                ActiveFrequencies = this.ActiveFrequencies,
                FrequencyRatio = this.FrequencyRatio,
                Style = this.Style
            };
        }
    }
}
