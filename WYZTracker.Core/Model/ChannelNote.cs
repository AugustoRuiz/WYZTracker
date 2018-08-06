using System;
using System.Collections.Generic;
using System.Text;

namespace WYZTracker
{
    public class ChannelNote
    {
        private const int MIN_OCTAVE = 1;
        private const int MAX_OCTAVE = 6;

        public ChannelNote()
        {
            octave = int.MinValue;
            note = char.MinValue;
            seminote = char.MinValue;
            instrument = string.Empty;
            envData = new EnvelopeData();
        }

        public bool HasValue
        {
            get
            {
                return (this.HasOctave || this.HasNote || this.HasSeminote || this.HasInstrument);
            }
        }

        public bool HasOctave
        {
            get { return octave != int.MinValue; }
        }

        public bool HasNote
        {
            get { return note != char.MinValue; }
        }

        public bool HasSeminote
        {
            get { return seminote != char.MinValue; }
        }

        public bool HasInstrument
        {
            get { return !string.IsNullOrEmpty(instrument); }
        }

        public bool IsSawtooth
        {
            get { return HasInstrument && instrument.ToUpperInvariant() == "R"; }
        }

        private int octave;

        public int Octave
        {
            get { return octave; }
            set
            {
                if (value > Char.MinValue)
                {
                    if (value > MAX_OCTAVE)
                    {
                        value = MAX_OCTAVE;
                    }
                    if (value < MIN_OCTAVE)
                    {
                        value = MIN_OCTAVE;
                    }
                }
                octave = value;
            }
        }

        private char note;

        public char Note
        {
            get { return note; }
            set { note = value; }
        }

        private char seminote;

        public char Seminote
        {
            get { return seminote; }
            set { seminote = value; }
        }

        private string instrument;

        public string Instrument
        {
            get { return instrument; }
            set { instrument = value; }
        }

        private EnvelopeData envData;

        public EnvelopeData EnvData
        {
            get { return envData; }
            set { envData = value; }
        }

        private SByte? volModifier;

        public SByte? VolModifier
        {
            get { return volModifier; }
            set { volModifier = value; }
        }

        public void Transpose(int semitones, int currentOctave)
        {
            if (this.HasNote)
            {
                int currentValue = this.GetNote();
                if (currentValue >= 0)
                {
                    if (this.HasSeminote)
                    {
                        currentValue++;
                    }

                    int octaveOffset = 0;
                    currentValue += semitones;

                    while (currentValue < 0)
                    {
                        currentValue += 12;
                        octaveOffset -= 1;
                    }
                    while (currentValue >= 12)
                    {
                        currentValue -= 12;
                        octaveOffset += 1;
                    }
                    if (octaveOffset != 0)
                    {
                        int newOctave = currentOctave + octaveOffset;
                        if(newOctave <= MIN_OCTAVE)
                        {
                            newOctave = MIN_OCTAVE + 1;
                        }
                        if(newOctave > MAX_OCTAVE)
                        {
                            newOctave = MAX_OCTAVE;
                        }
                        if(newOctave != currentOctave)
                        {
                            this.Octave = newOctave;
                        }
                    }
                    updateFromValue(currentValue);
                }
            }
        }

        private void updateFromValue(int currentValue)
        {
            switch (currentValue)
            {
                case 0:
                    this.Seminote = Char.MinValue;
                    this.Note = 'C';
                    break;
                case 1:
                    this.Seminote = '+';
                    this.Note = 'C';
                    break;
                case 2:
                    this.Seminote = Char.MinValue;
                    this.Note = 'D';
                    break;
                case 3:
                    this.Seminote = '+';
                    this.Note = 'D';
                    break;
                case 4:
                    this.Seminote = Char.MinValue;
                    this.Note = 'E';
                    break;
                case 5:
                    this.Seminote = Char.MinValue;
                    this.Note = 'F';
                    break;
                case 6:
                    this.Seminote = '+';
                    this.Note = 'F';
                    break;
                case 7:
                    this.Seminote = Char.MinValue;
                    this.Note = 'G';
                    break;
                case 8:
                    this.Seminote = '+';
                    this.Note = 'G';
                    break;
                case 9:
                    this.Seminote = Char.MinValue;
                    this.Note = 'A';
                    break;
                case 10:
                    this.Seminote = '+';
                    this.Note = 'A';
                    break;
                case 11:
                    this.Seminote = Char.MinValue;
                    this.Note = 'B';
                    break;
            }
        }

        public int GetNote()
        {
            int value = -1;
            if (this.HasNote)
            {
                switch (this.Note)
                {
                    case 'C':
                        value = 0;
                        break;
                    case 'D':
                        value = 2;
                        break;
                    case 'E':
                        value = 4;
                        break;
                    case 'F':
                        value = 5;
                        break;
                    case 'G':
                        value = 7;
                        break;
                    case 'A':
                        value = 9;
                        break;
                    case 'B':
                        value = 11;
                        break;
                }
            }
            return value;
        }

        public ChannelNote Clone()
        {
            return new ChannelNote()
            {
                Octave = this.Octave,
                Note = this.Note,
                Seminote = this.Seminote,
                Instrument = this.Instrument,
                EnvData = this.EnvData.Clone(),
                VolModifier = this.VolModifier
            };
        }

        public void CopyFrom(ChannelNote newValues)
        {
            this.octave = newValues.octave;
            this.note = newValues.note;
            this.seminote = newValues.seminote;
            this.instrument = newValues.instrument;
            this.envData = newValues.EnvData.Clone();
            this.volModifier = newValues.volModifier;
        }

        public static ChannelNote SilenceNote
        {
            get
            {
                ChannelNote result = new ChannelNote();
                result.Note = 'P';
                return result;
            }
        }

        public static ChannelNote EmptyNote
        {
            get
            {
                return new ChannelNote();
            }
        }
    }
}
