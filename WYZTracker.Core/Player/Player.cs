using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using LibAYEmu;
using System.ComponentModel;

namespace WYZTracker
{
    public class Player : IDisposable
    {
        private const int TICK_PERIOD = 20;
        private const int GENSOUND_PERIOD = 20;
        private byte[] regs = new byte[14];

        // Longitud del buffer: Frecuencia * número de canales * milisegundos / 1000
        private const int TICK_BUF_LENGTH = 44100 * 2 * TICK_PERIOD / 1000;

        private Song currentSong;
        private PlayMode playMode = PlayMode.FullSong;

        private Instrument[] currentInstruments;
        private int[] lastPitchModifiers;
        private int[] totalModifiers;
        private Pattern currentPattern;
        private ChannelLine currentLine;

        private ChannelLine CurrentLine
        {
            get
            {
                return currentLine;
            }
            set
            {
                if (value != currentLine)
                {
                    currentLine = value;
                    if (this.currentLine != null)
                    {
                        this.CurrentTempo += this.currentLine.TempoModifier;
                    }
                }
            }
        }

        private Effect currentEffect;
        private int lineNumber;
        private int patternNumber;
        private int currentTempo;

        public int CurrentTempo
        {
            get { return this.currentTempo; }
            set
            {
                if (value != currentTempo)
                {
                    currentTempo = value;
                    if (currentTempo < 1)
                    {
                        currentTempo = 1;
                    }
                    EventHandler tmp = this.CurrentTempoChanged;
                    if (tmp != null)
                    {
                        tmp(this, EventArgs.Empty);
                    }
                }
            }
        }

        private bool[] inOrnament;
        private long[] instrPositions;
        private int effectPosition = int.MinValue;
        private int[] notes;
        private int[] tones;
        private int[] octaves;
        private int[] lastReadOctaves;
        private int[] semiNotes;
        private int[] frequencies;
        private sbyte?[] volModifiers;
        private EnvelopeData[] lastEnvData;

        private PlayStatus playState;
        private int periodCount;

        public delegate void NextLineEventHandler(object sender, NextLineEventArgs evArgs);
        public event NextLineEventHandler NextLine;
        public event EventHandler SongFinished;
        public event EventHandler CurrentTempoChanged;

        private Stereo currentStereo;

        private LibAYEmu.AY[] ayEmus = new LibAYEmu.AY[3];
        private PlaybackStreamer streamer;

        private int loopCount;

        public int LimitLoops { get; set; }

        public Player()
        {
            this.playState = PlayStatus.Stopped;
            for (int i = 0; i < ayEmus.Length; i++)
            {
                ayEmus[i] = new AY();
                ayEmus[i].SetChipType(LibAYEmu.Chip.AY_Kay, null);
            }
            streamer = new PlaybackStreamer();
            streamer.BufferLengthInMs = 2 * GENSOUND_PERIOD;
            streamer.FillBuffer += new EventHandler<FillBufferEventArgs>(OnFillBuffer);

            this.LimitLoops = -1;
            this.Volume = 1.0;
        }

        public double Volume
        {
            get
            {
                return this.streamer.Volume;
            }
            set
            {
                this.streamer.Volume = value;
            }
        }

        public Stereo Stereo
        {
            get
            {
                return currentStereo;
            }
            set
            {
                currentStereo = value;
                for (int i = 0; i < ayEmus.Length; i++)
                {
                    ayEmus[i].SetStereo(value, null);
                }
            }
        }

        public PlayMode PlayMode
        {
            get
            {
                return playMode;
            }
            set
            {
                playMode = value;
            }
        }

        public PlayStatus Status
        {
            get
            {
                return playState;
            }
        }

        public Song CurrentSong
        {
            get
            {
                return currentSong;
            }
            set
            {
                if (value != currentSong)
                {
                    if (currentSong != null)
                    {
                        currentSong.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(currentSong_PropertyChanged);
                    }
                    currentSong = value;
                    if (currentSong != null)
                    {
                        currentSong.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(currentSong_PropertyChanged);
                    }
                    initializeParams();
                }
            }
        }

        /// <summary>
        /// Fills the provided buffer with audio data. Stereo 16-bits data is required.
        /// </summary>
        /// <param name="sender">Streamer object.</param>
        /// <param name="instr">FillBufferEventArgs.</param>
        private void OnFillBuffer(object sender, FillBufferEventArgs e)
        {
            if (this.playState == PlayStatus.Playing)
            {
                long tickCount = e.Buffer.LongLength / TICK_BUF_LENGTH;

                long currPos = 0;
                for (long i = 0; i < tickCount; i++)
                {
                    double[] tickBuf = getTick();
                    Array.Copy(tickBuf, 0, e.Buffer, currPos, tickBuf.LongLength);
                    currPos += tickBuf.LongLength;
                }
            }
            else
            {
                Array.Copy(new UInt16[e.Buffer.Length], e.Buffer, e.Buffer.Length);
            }
        }

        private double[] getTick()
        {
            int numSamples = TICK_BUF_LENGTH;
            int envStyle = 0xFF;
            double[][] buffer = new double[3][] { new double[numSamples], 
                                                  new double[numSamples], 
                                                  new double[numSamples] };
            double[] result = null;

            if (this.currentSong != null &&
                (playState == PlayStatus.Playing || playState == PlayStatus.Dumping) &&
                this.CurrentLine != null)
            {
                int noise = 0;
                int ayChipsNeeded = (int)Math.Ceiling(this.currentSong.Channels / 3m);

                bool[] noteChanged = new bool[this.currentSong.Channels];

                try
                {
                    if (this.periodCount == 0)
                    {
                        parseNextNote(noteChanged);
                    }

                    ChannelControl[] ctrl = setToneInChannelControl();
                    int[] vols = calculateVolumesAndFrequencies(noteChanged);

                    bool hasEffects = this.areEffectsActive();

                    // Actualizamos aquí abajo, ya que vuelve a hacer falta la nota justo encima.
                    if (this.periodCount == 0 && (playState == PlayStatus.Playing || playState == PlayStatus.Dumping))
                    {
                        updateCurrentLine();
                    }

                    for (int i = 0; i < ayChipsNeeded; i++)
                    {
                        int channel = i * 3; ;
                        int toneA = 0, toneB = 0, toneC = 0;
                        int volA = 0, volB = 0, volC = 0;
                        bool useEnvA = false, useEnvB = false, useEnvC = false;
                        bool activeFreqA = false, activeFreqB = false, activeFreqC = false;
                        int envFreq = 0;

                        noise = this.getNoise(ctrl);

                        if ((ctrl[i] & ChannelControl.TONE_A) == ChannelControl.TONE_A)
                        {
                            toneA = getFrequencyForChannel(channel);
                            volA = getVolumeForChannel(channel, vols);
                            useEnvA = updateEnvelopeForChannel(channel, noise, noteChanged, ref volA, ref envStyle, ref envFreq, ref activeFreqA);
                        }
                        channel++;

                        if ((ctrl[i] & ChannelControl.TONE_B) == ChannelControl.TONE_B)
                        {
                            toneB = getFrequencyForChannel(channel);
                            volB = getVolumeForChannel(channel, vols);
                            useEnvB = updateEnvelopeForChannel(channel, noise, noteChanged, ref volB, ref envStyle, ref envFreq, ref activeFreqB);
                        }
                        channel++;

                        if ((ctrl[i] & ChannelControl.TONE_C) == ChannelControl.TONE_C)
                        {
                            toneC = getFrequencyForChannel(channel);
                            volC = getVolumeForChannel(channel, vols);
                            useEnvC = updateEnvelopeForChannel(channel, noise, noteChanged, ref volC, ref envStyle, ref envFreq, ref activeFreqC);
                        }

                        envFreq = (useEnvA || useEnvB || useEnvC) ? envFreq : 0;

                        int fxChannel = this.currentSong != null ? this.currentSong.FxChannel : 0;

                        bool thisAyHasFx = (i == (fxChannel / 3));
                        int fxChannelInCurrentAy = (fxChannel % 3);

                        this.genSound((!useEnvA || activeFreqA || (thisAyHasFx && fxChannelInCurrentAy == 0 && hasEffects)) ? toneA : 0,
                                      (!useEnvB || activeFreqB || (thisAyHasFx && fxChannelInCurrentAy == 1 && hasEffects)) ? toneB : 0,
                                      (!useEnvC || activeFreqC || (thisAyHasFx && fxChannelInCurrentAy == 2 && hasEffects)) ? toneC : 0,
                                      thisAyHasFx ? noise : 0,
                                      (int)ctrl[i],
                                      volA,
                                      volB,
                                      volC,
                                      envFreq,
                                      envStyle,
                                      buffer[i],
                                      ayEmus[i],
                                      true
                                     );
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                this.periodCount++;
                if (this.periodCount >= this.CurrentTempo)
                {
                    this.periodCount = 0;
                }

                if (this.effectPosition >= 0)
                {
                    this.effectPosition++;
                    if (this.currentEffect == null || effectPosition >= this.currentEffect.Volumes.Length)
                    {
                        effectPosition = int.MinValue;
                    }
                }

                result = mixChannels(buffer, ayChipsNeeded, numSamples);
            }

            if (result == null)
            {
                result = new double[numSamples];
            }

            return result;
        }

        /// <summary>
        /// Mixes the source channels.
        /// </summary>
        /// <param name="source">Source channels.</param>
        /// <param name="numSources">Number of source channels. Must be </param>
        /// <returns></returns>
        private double[] mixChannels(double[][] source, int numSources, int numSamples)
        {
            double[] result = new double[numSamples];

            if (numSources == 1)
            {
                for (int i = 0; i < numSamples; i++)
                {
                    result[i] = source[0][i];
                }
            }
            else if (numSources == 2)
            {
                for (int i = 0; i < numSamples; i++)
                {
                    double a = source[0][i];
                    double b = source[1][i];
                    result[i] = a + b - a * b;
                }
            }
            else
            {
                for (int i = 0; i < numSamples; i++)
                {
                    double a = source[0][i];
                    double b = source[1][i];
                    double c = source[2][i];
                    result[i] = a + b + c - a * b - a * c - b * c + a * b * c;
                }
            }

            return result;
        }

        private bool lastCycleHadFx = false;

        private bool updateEnvelopeForChannel(int channel, int noise, bool[] noteChanged, ref int vol, ref int envStyle, ref int envFreq, ref bool activeFreqs)
        {
            bool useEnv = false;
            if (this.currentSong.FxChannel == channel && this.areEffectsActive())
            {
                if ((noise & 0x80) == 0x80)
                {
                    envStyle = this.currentEffect.EnvTypes[this.effectPosition];
                    if (envStyle == 0x01)
                    {
                        envStyle = 0xFF;
                    }
                    envFreq = this.currentEffect.EnvFreqs[this.effectPosition];
                    vol |= 0x10;
                    useEnv = true;
                    lastCycleHadFx = true;
                    activeFreqs = false;
                }
            }
            else
            {
                int tone = getFrequencyForChannel(channel);
                ChannelNote currentNote = this.CurrentLine.Notes[channel];
                useEnv = (this.currentInstruments[channel] != null && this.currentInstruments[channel].ID == "R");
                if (useEnv)
                {
                    envFreq = tone / lastEnvData[channel].FrequencyRatio;
                    activeFreqs = lastEnvData[channel].ActiveFrequencies;
                    if (noteChanged[channel] || lastCycleHadFx)
                    {
                        lastCycleHadFx = false;
                        envStyle = lastEnvData[channel].Style;
                    }
                    else
                    {
                        envStyle = 0xFF;
                    }
                }
            }
            return useEnv;
        }

        private int getFrequencyForChannel(int channel)
        {
            int result = 0;
            int fxChannel = this.currentSong == null ? 0 : this.currentSong.FxChannel;
            if (channel == fxChannel && this.areEffectsActive())
            {
                result = this.currentEffect.Frequencies[effectPosition];
            }
            else if (this.currentSong.Channels > channel && (!this.currentSong.MutedChannels[channel]))
            {
                result = frequencies[channel];
            }
            return result;
        }

        private int getVolumeForChannel(int channel, int[] vols)
        {
            int result = 0;
            int fxChannel = this.currentSong == null ? 0 : this.currentSong.FxChannel;
            if (channel == fxChannel && this.areEffectsActive())
            {
                result = this.currentEffect.Volumes[this.effectPosition];
            }
            else if (this.currentSong.Channels > channel && (!this.currentSong.MutedChannels[channel]))
            {
                result = vols[channel];
            }
            return result;
        }

        private bool areEffectsActive()
        {
            return (effectPosition >= 0 &&
                    !this.currentSong.MutedChannels[this.currentSong.Channels] &&
                    effectPosition < this.currentEffect.Volumes.Length);
        }

        private int getNoise(ChannelControl[] ctrl)
        {
            int result = 0;
            if (this.areEffectsActive())
            {
                int fxChannel = currentSong.FxChannel;
                result = this.currentEffect.Noises[effectPosition];
                if ((result & 0x7F) > 0)
                {
                    int ctrlIndex = fxChannel / 3;
                    int channelRemainder = fxChannel % 3;

                    if (channelRemainder == 0)
                    {
                        ctrl[ctrlIndex] |= ChannelControl.NOISE_A;
                    }
                    if (channelRemainder == 1)
                    {
                        ctrl[ctrlIndex] |= ChannelControl.NOISE_B;
                    }
                    if (channelRemainder == 2)
                    {
                        ctrl[ctrlIndex] |= ChannelControl.NOISE_C;
                    }
                }
            }
            return result;
        }

        private int[] calculateVolumesAndFrequencies(bool[] noteChanged)
        {
            int[] vols = new int[this.currentSong.Channels];

            ChannelNote note;

            for (int i = 0; i < this.currentSong.Channels; i++)
            {
                if (notes[i] == -1)
                {
                    // Si está en silencio, ponemos el volumen a 0.
                    vols[i] = 0;
                }
                else
                {
                    note = this.CurrentLine.Notes[i];

                    if (this.currentInstruments[i] != null &&
                        this.currentInstruments[i].ID != null &&
                        this.currentInstruments[i].PitchModifiers != null &&
                        instrPositions[i] < this.currentInstruments[i].PitchModifiers.Length)
                    {
                        lastPitchModifiers[i] = this.currentInstruments[i].PitchModifiers[instrPositions[i]];
                    }

                    vols[i] = this.getVolFromNote(note, ref this.currentInstruments[i], ref instrPositions[i]);

                    if (tones[i] != 0)
                    {
                        if ((vols[i] & 0x10) != 0)
                        {
                            // Ornament
                            int newTone = tones[i] + lastPitchModifiers[i];
                            setFrequencyForTone(i, newTone);
                            frequencies[i] += totalModifiers[i];
                            inOrnament[i] = true;
                        }
                        else if (inOrnament[i] && !noteChanged[i])
                        {
                            inOrnament[i] = false;
                            setFrequencyForTone(i, tones[i]);
                            frequencies[i] += totalModifiers[i];
                        }

                        if ((vols[i] & 0x20) != 0)
                        {
                            if ((long)frequencies[i] * 2 > int.MaxValue)
                            {
                                frequencies[i] = int.MaxValue;
                            }
                            else
                            {
                                frequencies[i] *= 2;
                            }
                        }
                        else
                        {
                            if ((vols[i] & 0x40) != 0)
                            {
                                frequencies[i] /= 2;
                            }
                        }
                    }

                    if (this.currentInstruments[i] != null && this.currentInstruments[i].ID != "R" && ((vols[i] & 0x10) == 0))
                    {
                        frequencies[i] += lastPitchModifiers[i];
                        totalModifiers[i] += lastPitchModifiers[i];
                    }

                    if (currentInstruments[i] != null && currentInstruments[i].ID != "R")
                    {
                        vols[i] &= 0xF;
                        if (volModifiers[i] != null) // TODO:  && vols[i] != 0, cuando el player lo implemente.
                        {
                            int targetVol = vols[i] + volModifiers[i].GetValueOrDefault();
                            if (targetVol < 0) { targetVol = 0; }
                            if (targetVol > 0xF) { targetVol = 0xF; }
                            if (vols[i] > 0 && targetVol == 0) { targetVol = 1; }
                            vols[i] = targetVol;
                        }
                    }
                }
            }
            return vols;
        }

        private ChannelControl[] setToneInChannelControl()
        {
            ChannelControl[] ctrl = new ChannelControl[3];
            int fxChannel = this.currentSong == null ? 0 : this.currentSong.FxChannel;

            for (int i = 0; i < 3; i++)
            {
                // Calculamos los volumenes de los canales:
                if (this.currentSong.Channels > i * 3 && !this.currentSong.MutedChannels[i * 3])
                {
                    ctrl[i] |= ChannelControl.TONE_A;
                }
                if (i * 3 == fxChannel && !this.currentSong.MutedChannels[this.currentSong.Channels])
                {
                    ctrl[i] |= ChannelControl.TONE_A;
                }

                if (this.currentSong.Channels > i * 3 + 1 && !this.currentSong.MutedChannels[i * 3 + 1])
                {
                    ctrl[i] |= ChannelControl.TONE_B;
                }
                if (i * 3 + 1 == fxChannel && !this.currentSong.MutedChannels[this.currentSong.Channels])
                {
                    ctrl[i] |= ChannelControl.TONE_B;
                }

                if (this.currentSong.Channels > i * 3 + 2 && !this.currentSong.MutedChannels[i * 3 + 2])
                {
                    ctrl[i] |= ChannelControl.TONE_C;
                }
                if (i * 3 + 2 == fxChannel && !this.currentSong.MutedChannels[this.currentSong.Channels])
                {
                    ctrl[i] |= ChannelControl.TONE_C;
                }
            }
            return ctrl;
        }

        private void parseNextNote(bool[] noteChanged)
        {
            ChannelNote note;
            if (this.CurrentLine.Fx != int.MinValue)
            {
                currentEffect = getEffect(this.CurrentLine.Fx);
                effectPosition = 0;
            }

            // Hemos de aplicar la nota actual, y apuntar a la siguiente.
            for (int i = 0; i < this.currentSong.Channels; i++)
            {
                noteChanged[i] = false;

                note = this.CurrentLine.Notes[i];
                if (note.HasInstrument)
                {
                    Instrument currentInstrument;
                    if ((currentInstrument = this.currentSong.GetInstrument(note.Instrument)) != null)
                    {
                        instrPositions[i] = 0;
                        this.currentInstruments[i] = currentInstrument;
                        if (currentInstrument.ID == "R")
                        {
                            lastEnvData[i] = note.EnvData;
                        }
                        volModifiers[i] = note.VolModifier;
                    }
                    noteChanged[i] = true;
                }
                else
                {
                    if (note.VolModifier.HasValue)
                    {
                        volModifiers[i] = note.VolModifier;
                    }
                }
                if (note.HasOctave)
                {
                    octaves[i] = note.Octave - 2;
                    lastReadOctaves[i] = octaves[i];
                    instrPositions[i] = 0;
                }
                if (note.HasSeminote)
                {
                    semiNotes[i] = (note.Seminote == '+') ? 1 : -1;
                    instrPositions[i] = 0;
                    noteChanged[i] = true;
                }
                else
                {
                    if (note.HasValue)
                    {
                        semiNotes[i] = 0;
                    }
                }
                if (note.HasNote)
                {
                    notes[i] = note.GetNote();
                    instrPositions[i] = 0;
                    noteChanged[i] = true;
                }
                if (notes[i] == -1)
                {
                    tones[i] = 0;
                }
                else
                {
                    if (noteChanged[i])
                    {
                        octaves[i] = lastReadOctaves[i];
                        tones[i] = 12 * octaves[i] + (notes[i] + semiNotes[i]) + 2;
                        totalModifiers[i] = 0;
                        setFrequencyForTone(i, tones[i]);
                    }
                }
            }
        }

        private void setFrequencyForTone(int channel, int tone)
        {
            if (tone >= this.currentSong.Frequencies.Length)
            {
                int num = 0;
                while (tone >= this.currentSong.Frequencies.Length)
                {
                    tone -= 12;
                    num++;
                }
                int freq = this.currentSong.Frequencies[tone];
                frequencies[channel] = (int)(freq / Math.Pow(2, num));
            }
            else
            {
                if (tone >= 0)
                {
                    frequencies[channel] = this.currentSong.Frequencies[tone];
                }
                else
                {
                    frequencies[channel] = 0;
                }
            }
        }

        private Effect getEffect(int p)
        {
            Effect eff = null;
            foreach (Effect e in this.currentSong.Effects)
            {
                if (e.ID == p)
                {
                    eff = e;
                    break;
                }
            }
            return eff;
        }

        private void updateCurrentLine()
        {
            this.lineNumber++;
            if (this.lineNumber >= this.currentPattern.Lines.Length)
            {
                if (playMode == PlayMode.FullSong)
                {
                    this.patternNumber++;
                    if (this.patternNumber >= this.currentSong.PlayOrder.Count)
                    {
                        if (this.currentSong.Looped && (this.LimitLoops == -1 || loopCount < this.LimitLoops))
                        {
                            this.patternNumber = this.currentSong.LoopToPattern;
                            this.loopCount++;
                        }
                        else
                        {
                            this.OnSongFinished();
                        }
                    }
                }
                GoToPattern(this.patternNumber);
            }
            else
            {
                this.CurrentLine = this.currentPattern.Lines[this.lineNumber];
            }
            this.OnNextLine(new NextLineEventArgs(this.patternNumber, this.lineNumber));
        }

        private int getVolFromNote(ChannelNote note, ref Instrument currentInstrument, ref long instrPos)
        {
            int vol;

            if ((note.HasInstrument) && (periodCount == 0))
            {
                currentInstrument = this.currentSong.GetInstrument(note.Instrument);
            }
            if (currentInstrument != null && currentInstrument.ID != "R")
            {
                if (instrPos >= currentInstrument.Volumes.Length)
                {
                    vol = 0;
                }
                else
                {
                    vol = currentInstrument.Volumes[instrPos];
                    instrPos++;
                    if (instrPos >= currentInstrument.Volumes.Length)
                    {
                        if (currentInstrument.Looped)
                        {
                            instrPos = currentInstrument.LoopStart;
                        }
                    }
                }
            }
            else if (currentInstrument == null)
            {
                vol = 0;
            }
            else
            {
                vol = 0x10;
            }
            return vol;
        }

        private void currentSong_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ChipFrequency")
            {
                UpdateChipFrequencies();
            }
            if (e.PropertyName == "Tempo")
            {
                this.CurrentTempo = this.currentSong.Tempo;
            }
        }

        private void initializeParams()
        {
            int numChannels = this.currentSong.Channels;

            this.inOrnament = new bool[numChannels];
            this.currentInstruments = new Instrument[numChannels];
            this.lastPitchModifiers = new int[numChannels];
            this.totalModifiers = new int[numChannels];

            this.notes = new int[numChannels];
            this.tones = new int[numChannels];
            this.octaves = new int[numChannels];
            this.lastReadOctaves = new int[numChannels];
            this.semiNotes = new int[numChannels];
            this.instrPositions = new long[numChannels];
            this.frequencies = new int[numChannels];
            this.lastEnvData = new EnvelopeData[numChannels];
            this.volModifiers = new sbyte?[numChannels];
            this.loopCount = 0;
            this.lineNumber = 0;

            this.CurrentTempo = this.currentSong.Tempo;

            for (int i = 0; i < numChannels; i++)
            {
                this.currentInstruments[i] = null;
                this.lastPitchModifiers[i] = 0;

                // Arranca en silencio...
                this.notes[i] = -1;
                this.tones[i] = 0;
                this.octaves[i] = 0;
                this.semiNotes[i] = 0;
                this.instrPositions[i] = 0;
                this.lastEnvData[i] = new EnvelopeData();
                this.volModifiers[i] = null;
            }
            UpdateChipFrequencies();
            GoToPattern(0);
        }

        public void UpdateChipFrequencies()
        {
            for (int i = 0; i < ayEmus.Length; i++)
            {
                ayEmus[i].SetChipFreq(this.currentSong.ChipFrequency);
            }
        }

        public void Play()
        {
            Play(0);
        }

        public void Play(int patternIdx)
        {
            this.playState = PlayStatus.Playing;

            periodCount = 0;
            effectPosition = int.MinValue;
            this.CurrentLine = null;
            this.initializeParams();
            this.GoToPattern(patternIdx);
            for (int i = 0; i < ayEmus.Length; i++)
            {
                ayEmus[i].Reset();
            }
        }

        public void Stop()
        {
            this.playState = PlayStatus.Stopped;
        }

        public void Pause()
        {
            switch (this.playState)
            {
                case PlayStatus.Playing:
                    this.playState = PlayStatus.Paused;
                    break;
                case PlayStatus.Paused:
                    this.playState = PlayStatus.Playing;
                    break;
                case PlayStatus.Stopped:
                    this.initializeParams();
                    this.playState = PlayStatus.Stopped;
                    break;
            }
        }

        public void GoToPattern(int patternOrder)
        {
            if (patternOrder < this.currentSong.PlayOrder.Count)
            {
                this.lineNumber = 0;
                this.patternNumber = patternOrder;
                this.currentPattern = this.currentSong.Patterns[this.currentSong.PlayOrder[patternOrder]];
                this.CurrentLine = this.currentPattern.Lines[0];
                this.periodCount = 0;
            }
        }

        protected virtual void OnNextLine(NextLineEventArgs e)
        {
            NextLineEventHandler tmp = this.NextLine;
            if (tmp != null)
            {
                tmp(this, e);
            }
        }

        protected virtual void OnSongFinished()
        {
            EventHandler tmp = this.SongFinished;
            if (tmp != null)
            {
                tmp(this, EventArgs.Empty);
            }
        }

        private void genSound(int tonea, int toneb, int tonec, int noise, int control, int vola,
                              int volb, int volc, int envfreq, int envstyle, double[] buffer,
                              LibAYEmu.AY emu, bool setRegs)
        {
            byte[] regs = new byte[14];

            /* setup regs */
            regs[0] = (byte)(tonea & 0xff);
            regs[1] = (byte)(tonea >> 8);
            regs[2] = (byte)(toneb & 0xff);
            regs[3] = (byte)(toneb >> 8);
            regs[4] = (byte)(tonec & 0xff);
            regs[5] = (byte)(tonec >> 8);
            regs[6] = (byte)(noise);
            regs[7] = (byte)((~control) & 0x3f); 	/* invert bits 0-5 */
            regs[8] = (byte)(vola); 				/* included bit 4 */
            regs[9] = (byte)(volb);
            regs[10] = (byte)(volc);
            regs[11] = (byte)(envfreq & 0xff);
            regs[12] = (byte)(envfreq >> 8);
            regs[13] = (byte)(envstyle);

            if (setRegs)
            {
                emu.SetRegs(regs);
            }
            emu.GenSound(buffer, buffer.Length, 0);
        }

        #region Miembros de IDisposable

        /// <summary>
        /// Releases resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~Player()
        {
            Dispose(false);
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        /// <param name="disposing">Indicates if the user called the dispose method or the finalizer did it.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (streamer != null)
            {
                streamer.Dispose();
                streamer = null;
            }
        }

        #endregion

        internal void Reset()
        {
            periodCount = 0;
            this.initializeParams();
            this.GoToPattern(0);
            for (int i = 0; i < ayEmus.Length; i++)
            {
                ayEmus[i].Reset();
            }
        }

        public void DumpToWavFile(WaveFile wav, int repeats)
        {
            periodCount = 0;
            for (int i = 0; i < ayEmus.Length; i++)
            {
                ayEmus[i].Reset();
            }
            effectPosition = int.MinValue;
            this.initializeParams();

            this.LimitLoops = repeats - 1;

            this.playState = PlayStatus.Dumping;
            while (PlayStatus.Dumping == this.playState)
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                System.IO.BinaryWriter writer = new System.IO.BinaryWriter(ms);

                int tickNumber = 0;
                do
                {
                    double[] tickData = getTick();
                    foreach (double sample in tickData)
                    {
                        writer.Write((UInt16)(sample * Int16.MaxValue));
                    }
                } while ((tickNumber++ < 1500) && PlayStatus.Dumping == this.playState);

                wav.AppendData(ms.ToArray());
            }
        }
    }
}
