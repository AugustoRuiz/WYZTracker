using System;
using System.Collections.Generic;
using System.Text;

namespace WYZTracker
{
    public class ArpeggioDefinition : NotifierBase
    {
        public ArpeggioDefinition()
        {
            baseNote = new ChannelNote()
            {
                Note = 'C',
                Octave = 3
            };
            targetPatternIdx = -1;
            highlightByScale = -1;
            targetChannelIdx = 0;
            this.Length = 16;
            this.NewPatternLength = 32;
            this.OctavesInArpeggio = 2;
        }

        private ChannelNote baseNote;

        /// <summary>
        /// Gets or sets the base note for the arpeggio.
        /// </summary>
        public ChannelNote BaseNote
        {
            get
            {
                return baseNote;
            }
            set
            {
                if (value != baseNote)
                {
                    baseNote = value;
                    OnPropertyChanged("BaseNote");
                }
            }
        }

        private int targetPatternIdx;

        /// <summary>
        /// Gets or sets the target pattern name.
        /// </summary>
        public int TargetPatternIdx
        {
            get
            {
                return targetPatternIdx;
            }
            set
            {
                if (value != targetPatternIdx)
                {
                    targetPatternIdx = value;
                    OnPropertyChanged("TargetPatternIdx");
                }
            }
        }

        private int targetChannelIdx;

        /// <summary>
        /// Gets or sets the target pattern name.
        /// </summary>
        public int TargetChannelIdx
        {
            get
            {
                return targetChannelIdx;
            }
            set
            {
                if (value != targetChannelIdx)
                {
                    targetChannelIdx = value;
                    OnPropertyChanged("TargetChannelIdx");
                }
            }
        }

        private int newPatternLength;

        /// <summary>
        /// Gets or sets the new pattern length.
        /// </summary>
        public int NewPatternLength
        {
            get
            {
                return newPatternLength;
            }
            set
            {
                if (value != newPatternLength)
                {
                    newPatternLength = value;
                    OnPropertyChanged("NewPatternLength");
                }
            }
        }

        /// <summary>
        /// Gets or sets the arpeggio length.
        /// </summary>
        public int Length
        {
            get
            {
                return Intervals == null ? 0 : Intervals.Length;
            }
            set
            {
                if (value != Length)
                {
                    Array.Resize(ref intervals, value);
                    Array.Resize(ref activeNotes, value);
                    Array.Resize(ref silencedNotes, value);
                    OnPropertyChanged("Intervals");
                    OnPropertyChanged("ActiveNotes");
                    OnPropertyChanged("SilencedNotes");
                    OnPropertyChanged("Length");
                }
            }
        }

        private int[] intervals;

        /// <summary>
        /// Gets or sets the intervals to use in the arpeggio.
        /// </summary>
        public int[] Intervals
        {
            get
            {
                return intervals;
            }
            set
            {
                if(value != intervals)
                {
                    intervals = value;
                    OnPropertyChanged("Intervals");
                }
            }
        }

        private bool[] activeNotes;

        public bool[] ActiveNotes
        {
            get
            {
                return activeNotes;
            }
            set
            {
                if (value != activeNotes)
                {
                    activeNotes = value;
                    OnPropertyChanged("ActiveNotes");
                }
            }
        }

        private bool[] silencedNotes;

        public bool[] SilencedNotes
        {
            get
            {
                return silencedNotes;
            }
            set
            {
                if (value != silencedNotes)
                {
                    silencedNotes = value;
                    OnPropertyChanged("SilencedNotes");
                }
            }
        }

        private int highlightByScale;

        public int HighlightByScale
        {
            get
            {
                return highlightByScale;
            }
            set
            {
                if (value != highlightByScale)
                {
                    highlightByScale = value;
                    OnPropertyChanged("HighlightByScale");
                }
            }
        }

        private int octavesInArpeggio;

        public int OctavesInArpeggio
        {
            get
            {
                return octavesInArpeggio;
            }
            set
            {
                if (value != octavesInArpeggio)
                {
                    octavesInArpeggio = value;
                    OnPropertyChanged("OctavesInArpeggio");
                }
            }
        }

    }
}
