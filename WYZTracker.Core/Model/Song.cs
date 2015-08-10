using System;
using System.Collections.Generic;
using System.Text;

namespace WYZTracker
{
    public class Song : NotifierBase
    {
        public Song()
        {
            this.Channels = DEFAULT_CHANNELS_COUNT;
            initializeSong(false);
        }

        public Song(byte numChannels)
        {
            this.Channels = numChannels;
            initializeSong(true);
        }

        public bool[] MutedChannels
        {
            get { return mutedChannels; }
            set 
            { 
                mutedChannels = value; 
            }
        }

        private void initializeSong(bool defaultValues)
        {
            Pattern defaultPattern;
            Instrument defaultInstrument;

            this.Looped = true;

            if (defaultValues)
            {
                defaultPattern = new Pattern(this.channels);
                defaultPattern.Name = WYZTracker.Core.Properties.Resources.New;

                this.patterns.Add(defaultPattern);

                defaultInstrument = new Instrument();
                defaultInstrument.ID = "0";
                defaultInstrument.Name = WYZTracker.Core.Properties.Resources.Piano;
                defaultInstrument.SetVolumeLength(4);
                defaultInstrument.Looped = true;
                defaultInstrument.LoopStart = 3;
                defaultInstrument.Volumes[0] = 8;
                defaultInstrument.Volumes[1] = 7;
                defaultInstrument.Volumes[2] = 6;
                defaultInstrument.Volumes[3] = 5;

                this.Instruments.Add(defaultInstrument);

                defaultInstrument = new Instrument();
                defaultInstrument.ID = "R";
                defaultInstrument.Name = "Sawtooth";
                defaultInstrument.Looped = false;
                defaultInstrument.LoopStart = 0;
                this.Instruments.Add(defaultInstrument);

                this.mutedChannels = new bool[this.channels + 1];

                this.ChipFrequency = (int) LibAYEmu.ChipSpeedsByMachine.MSX;
                this.defaultMsxFreqs = true;
                this.parameterValue = this.chipFrequency;
            }
        }

        public const int DEFAULT_CHANNELS_COUNT = 3;
        private string name;
        private byte tempo;
        private byte channels;
        private List<Pattern> patterns = new List<Pattern>();
        private List<int> playOrder = new List<int>();
        private Instruments instruments = new Instruments();
        private Effects effects = new Effects();
        private bool[] mutedChannels = { };
        private Int16[] frequencies = { };
        private int chipFrequency;
        private bool looped;
        private bool defaultMsxFreqs;
        private bool defaultCpcFreqs;
        private bool customFreqs;
        private int parameterValue;
        private bool parameterizedFreqs;
        private int fxChannel;

        private int loopToPattern;

        public string Name
        {
            get { return name; }
            set
            {
                if (value != name)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public List<Pattern> Patterns
        {
            get { return patterns; }
            set
            {
                if (value != patterns)
                {
                    patterns = value;
                    OnPropertyChanged("Patterns");
                }
            }
        }

        public List<int> PlayOrder
        {
            get { return playOrder; }
            set
            {
                if (value != playOrder)
                {
                    playOrder = value;
                    OnPropertyChanged("PlayOrder");
                }
            }
        }

        public byte Tempo
        {
            get { return tempo; }
            set
            {
                if (value != tempo)
                {
                    tempo = value;
                    OnPropertyChanged("Tempo");
                }
            }
        }

        public byte Channels
        {
            get 
            { 
                return channels; 
            }
            set
            {
                if (value != channels)
                {
                    channels = value;
                    setChannels(); 
                    OnPropertyChanged("Channels");
                }
            }
        }

        public Instruments Instruments
        {
            get { return instruments; }
            set
            {
                if (value != instruments)
                {
                    instruments = value;
                    OnPropertyChanged("Instruments");
                }
            }
        }

        public Effects Effects
        {
            get { return effects; }
            set
            {
                if (value != effects)
                {
                    effects = value;
                    OnPropertyChanged("Effects");
                }
            }
        }

        public Int16[] Frequencies
        {
            get
            {
                return frequencies;
            }
            set
            {
                if (value != frequencies)
                {
                    frequencies = value;
                    OnPropertyChanged("Frequencies");
                }
            }
        }

        public int ChipFrequency
        {
            get
            {
                return chipFrequency;
            }
            set
            {
                if (value != chipFrequency)
                {
                    chipFrequency = value;
                    OnPropertyChanged("ChipFrequency");
                }
            }
        }

        public bool Looped
        {
            get
            {
                return looped;
            }
            set
            {
                if (value != looped)
                {
                    looped = value;
                    OnPropertyChanged("Looped");
                }
            }
        }

        public bool DefaultMsxFreqs
        {
            get
            {
                return defaultMsxFreqs;
            }
            set
            {
                if (value != defaultMsxFreqs)
                {
                    defaultMsxFreqs = value;
                    OnPropertyChanged("DefaultMsxFreqs");
                }
            }
        }

        public bool DefaultCpcFreqs
        {
            get
            {
                return defaultCpcFreqs;
            }
            set
            {
                if (value != defaultCpcFreqs)
                {
                    defaultCpcFreqs = value;
                    OnPropertyChanged("DefaultCpcFreqs");
                }
            }
        }

        public bool CustomFreqs
        {
            get
            {
                return customFreqs;
            }
            set
            {
                if (value != customFreqs)
                {
                    customFreqs = value;
                    OnPropertyChanged("CustomFreqs");
                }
            }
        }

        public bool ParameterizedFreqs
        {
            get
            {
                return parameterizedFreqs;
            }
            set
            {
                if (value != parameterizedFreqs)
                {
                    parameterizedFreqs = value;
                    OnPropertyChanged("ParameterizedFreqs");
                }
            }
        }

        public int ParameterValue
        {
            get
            {
                return parameterValue;
            }
            set
            {
                if (value != parameterValue)
                {
                    parameterValue = value;
                    OnPropertyChanged("ParameterValue");
                }
            }
        }

        public int FxChannel
        {
            get
            {
                return fxChannel;
            }
            set
            {
                if (value != fxChannel)
                {
                    fxChannel = value;
                    OnPropertyChanged("FxChannel");
                }
            }
        }

        public int LoopToPattern
        {
            get
            {
                return loopToPattern;
            }
            set
            {
                if (value != loopToPattern)
                {
                    loopToPattern = value;
                    OnPropertyChanged("LoopToPattern");
                }
            }
        }

        private void setChannels()
        {
            foreach (Pattern p in this.patterns)
            {
                p.Channels = this.channels;
            }
            Array.Resize(ref this.mutedChannels, this.channels + 1);
        }

        public Instrument GetInstrument(string id)
        {
            Instrument instr = null;
            foreach (Instrument i in this.Instruments)
            {
                if (i.ID == id)
                {
                    instr = i;
                    break;
                }
            }
            return instr;
        }

        public Effect GetEffect(int id)
        {
            Effect eff = null;
            foreach (Effect e in this.Effects)
            {
                if (e.ID == id)
                {
                    eff = e;
                    break;
                }
            }
            return eff;
        }
    }
}
