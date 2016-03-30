using System;
using System.Collections.Generic;
using System.Text;

namespace WYZTracker
{
    public class Pattern
    {
        public const int DEFAULT_PATTERN_LENGTH = 32;

        public Pattern()
        {
            this.length = DEFAULT_PATTERN_LENGTH;
            this.channels = 0;
            initChannelLines();
        }

        public Pattern(byte channelCount)
        {
            this.length = DEFAULT_PATTERN_LENGTH;
            this.channels = channelCount;
            initChannelLines();
        }

        private ChannelLine[] lines;
        private int length;
        private string name;
        private byte channels;

        /// <summary>
        /// Gets or sets the name of the pattern.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Gets or sets the length of the pattern.
        /// </summary>
        public int Length
        {
            get { return length; }
            set
            {
                length = value;
                resizeNotesList();
            }
        }

        /// <summary>
        /// Gets or sets the notes of the pattern.
        /// </summary>
        public ChannelLine[] Lines
        {
            get { return lines; }
            set
            {
                lines = value;
                length = lines.Length;
            }
        }

        private void initChannelLines()
        {
            lines = new ChannelLine[this.length];
            for (int i = 0; i < this.length; i++)
            {
                this.lines[i] = new ChannelLine(this.channels);
            }
        }

        private void resizeNotesList()
        {
            ChannelLine[] newLinesList = new ChannelLine[length];

            int maxIdx = (length > lines.Length) ? lines.Length : length;

            Array.Copy(lines, newLinesList, maxIdx);

            for (int idx = maxIdx; idx < length; idx++)
            {
                newLinesList[idx] = new ChannelLine(channels);
            }

            // Nos quedamos con la nueva...
            lines = newLinesList;
        }

        public byte Channels
        {
            get
            {
                return this.channels;
            }
            set
            {
                foreach (ChannelLine l in lines)
                {
                    l.Channels = value;
                }
                channels = value;
            }
        }

        public void OptimizeInstrumentUsage()
        {
            for (int channel = 0; channel < this.Channels; channel++)
            {
                this.OptimizeInstrumentUsage(channel);
            }
        }

        public void OptimizeInstrumentUsage(int channel)
        {
            string runningInstrumentId = string.Empty;
            int patternIdx = 0;
            while (patternIdx < this.Length)
            {
                ChannelNote currentNote = this.Lines[patternIdx].Notes[channel];
                if (currentNote.HasInstrument)
                {
                    if (runningInstrumentId == currentNote.Instrument)
                    {
                        currentNote.Instrument = string.Empty;
                    }
                    else
                    {
                        runningInstrumentId = currentNote.Instrument;
                    }
                }
                patternIdx++;
            }
        }

        public void PopulateInstruments(int channel)
        {
            string runningInstrumentId = string.Empty;
            int patternIdx = 0;
            while (patternIdx < this.Length)
            {
                ChannelNote currentNote = this.Lines[patternIdx].Notes[channel];
                if (currentNote.HasValue)
                {
                    if (currentNote.HasInstrument)
                    {
                        runningInstrumentId = currentNote.Instrument;
                    }
                    else
                    {
                        currentNote.Instrument = runningInstrumentId;
                    }
                }
                patternIdx++;
            }
        }

    }
}
