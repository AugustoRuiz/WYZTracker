using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using LibAYEmu;
using System.ComponentModel;

namespace WYZTracker
{
    public class NotePlayer : IDisposable
    {
        private enum Status
        {
            Paused,
            Playing,
            Stopped
        }

        private const int GENSOUND_PERIOD = 20;
        private byte[] regs = new byte[14];

        // Longitud del buffer: Frecuencia * número de canales * bytes por sample * milisegundos / 1000
        private const int BUFFER_LENGTH = 44100 * 2 * (16 >> 3) * GENSOUND_PERIOD / 1000;

        private Instrument currentInstrument;
        private ChannelNote currentNote;
        private byte tempo;

        private long instrPosition;
        private int note;
        private int tone;
        private int octave;
        private int noteFreq;
        private int lastReadOctave;
        private int semiNote;

        private Status playState;
        private int periodCount;

        private LibAYEmu.AY ay = new LibAYEmu.AY();

        private PlaybackStreamer streamer = null;

        private int totalModifier;
        private int lastModifier;
        private Song currentSong;

        private bool inOrnament;

        public NotePlayer()
        {
            streamer = new PlaybackStreamer();
            streamer.BufferLengthInMs = GENSOUND_PERIOD;
            streamer.FillBuffer += new EventHandler<FillBufferEventArgs>(OnFillBuffer);

            this.playState = Status.Stopped;
            ay.SetChipType(LibAYEmu.Chip.AY_Lion17, null);
            ay.SetStereo(Stereo.Mono, null);
        }

        public bool Playing
        {
            get { return this.playState == Status.Playing; }
        }

        public bool Paused
        {
            get { return this.playState == Status.Paused; }
        }

        public Instrument CurrentInstrument
        {
            get
            {
                return currentInstrument;
            }
            set
            {
                currentInstrument = value;
                initializeParams();
            }
        }

        public ChannelNote CurrentNote
        {
            get
            {
                return currentNote;
            }
            set
            {
                currentNote = value;
                initializeParams();
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
                        this.ay.SetChipFreq(currentSong.ChipFrequency);
                    }
                }
            }
        }

        public byte Tempo
        {
            get
            {
                return tempo;
            }
            set
            {
                tempo = value;
                initializeParams();
            }
        }

        /// <summary>
        /// Fills the provided buffer with audio data. Stereo 16-bits data is required.
        /// </summary>
        /// <param name="sender">Streamer object.</param>
        /// <param name="instr">FillBufferEventArgs.</param>
        private void OnFillBuffer(object sender, FillBufferEventArgs e)
        {
            if (this.currentNote != null && this.currentInstrument != null && (this.playState == Status.Playing))
            {
                ChannelControl ctrl = ChannelControl.NONE;
                int vol = 0;
                int noise = 0;
                bool noteChanged = false;

                try
                {
                    if (this.periodCount == 0)
                    {
                        // Hemos de aplicar la nota actual, y apuntar a la siguiente.
                        if (currentNote.HasInstrument)
                        {
                            int instrNumber;
                            if (int.TryParse(currentNote.Instrument, out instrNumber))
                            {
                                instrPosition = 0;
                            }
                            noteChanged = true;
                        }
                        if (currentNote.HasOctave)
                        {
                            octave = currentNote.Octave - 2;
                            lastReadOctave = octave;
                            instrPosition = 0;
                        }
                        if (currentNote.HasSeminote)
                        {
                            semiNote = (currentNote.Seminote == '+') ? 1 : -1;
                            instrPosition = 0;
                            noteChanged = true;
                        }
                        else
                        {
                            if (currentNote.HasValue)
                            {
                                semiNote = 0;
                            }
                        }
                        if (currentNote.HasNote)
                        {
                            if (currentNote.Note == 'P')
                            {
                                note = -1;
                            }
                            else
                            {
                                note = getNote(currentNote.Note);
                                instrPosition = 0;
                            }
                            noteChanged = true;
                        }
                        if (note == -1)
                        {
                            tone = 0;
                        }
                        else
                        {
                            if (noteChanged)
                            {
                                octave = lastReadOctave;
                                tone = 12 * octave + (note + semiNote) + 2;
                                setFrequencyForTone(tone);
                                totalModifier = 0;
                            }
                        }
                    }

                    ctrl = ChannelControl.TONE_A;

                    if (note == -1)
                    {
                        // Si está en silencio, ponemos el volumen a 0.
                        vol = 0;
                    }
                    else
                    {
                        vol = this.getVolFromNote(currentNote, ref this.currentInstrument, ref instrPosition);

                        if (tone != 0)
                        {
                            if ((vol & 0x10) != 0 && this.CurrentInstrument.ID != "R")
                            {
                                int newTone = tone + lastModifier;
                                inOrnament = true;
                                setFrequencyForTone(newTone);
                                noteFreq += totalModifier;
                            }
                            else if (inOrnament && !noteChanged)
                            {
                                inOrnament = false;
                                setFrequencyForTone(tone);
                                noteFreq += totalModifier;
                            }

                            if ((vol & 0x20) != 0)
                            {
                                if ((long)noteFreq * 2 > int.MaxValue)
                                {
                                    noteFreq = int.MaxValue;
                                }
                                else
                                {
                                    noteFreq *= 2;
                                }
                            }
                            else
                            {
                                if ((vol & 0x40) != 0)
                                {
                                    noteFreq /= 2;
                                }
                            }
                        }
                    }

                    if (this.currentInstrument.ID != "R" && ((vol & 0x10) == 0))
                    {
                        noteFreq += lastModifier;
                        totalModifier += lastModifier;
                    }

                    if (this.CurrentInstrument.ID != "R")
                    {
                        vol &= 0xF;
                    }

                    EnvelopeData envData = this.currentNote.EnvData;

                    this.genSound((this.currentInstrument.ID != "R" || envData.ActiveFrequencies) ? noteFreq : 0,
                                    0,
                                    0,
                                    noise,
                                    (int)ctrl,
                                    vol,
                                    0,
                                    0,
                                    (vol != 0x10) ? 0 : noteFreq / envData.FrequencyRatio,
                                    (vol != 0x10) ? 0 : (noteChanged ? envData.Style : 0xFF),
                                    e.Buffer,
                                    ay,
                                    true
                                 );
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.ToString());
                }

                this.periodCount++;
            }
            else
            {
                for (long i = 0; i < e.Buffer.LongLength; i++)
                {
                    e.Buffer[i] = (byte)0;
                }
            }
        }

        private void setFrequencyForTone(int newTone)
        {
            if (newTone >= this.CurrentSong.Frequencies.Length)
            {
                int num = 0;
                while (newTone >= this.CurrentSong.Frequencies.Length)
                {
                    newTone -= 12;
                    num++;
                }
                int freq = this.CurrentSong.Frequencies[newTone];

                noteFreq = (int)(freq / Math.Pow(2, num));
            }
            else
            {
                if (newTone >= 0)
                {
                    noteFreq = this.CurrentSong.Frequencies[newTone];
                }
                else
                {
                    noteFreq = 0;
                }
            }
        }

        private int getNote(char p)
        {
            int value = 0;

            switch (p)
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

            return value;
        }

        private int getVolFromNote(ChannelNote note, ref Instrument currentInstrument, ref long instrPos)
        {
            int vol = 0;
            int instIdx;

            if ((note.HasInstrument) && (periodCount == 0))
            {
                if (int.TryParse(note.Instrument, out instIdx))
                {
                    instrPos = 0;
                }
                else
                {
                    // Sawtooth... Como?
                    vol = 0x10;
                }
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
                    if (this.currentInstrument.PitchModifiers != null &&
                        instrPosition < this.currentInstrument.PitchModifiers.Length)
                    {
                        lastModifier = this.currentInstrument.PitchModifiers[instrPosition];
                    }

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

        private void initializeParams()
        {
            if (this.currentSong != null)
            {
                ay.SetChipFreq(this.currentSong.ChipFrequency);
            }
            this.totalModifier = 0;
            this.lastModifier = 0;
            this.note = 0;
            this.tone = 0;
            this.octave = 0;
            this.lastReadOctave = 0;
            this.semiNote = 0;
            this.instrPosition = 0;
            this.inOrnament = false;
        }

        public void Play()
        {
            this.Reset();
            this.playState = Status.Playing;
        }

        public void Stop()
        {
            this.playState = Status.Stopped;
        }

        public void Pause()
        {
            switch (this.playState)
            {
                case Status.Playing:
                    this.playState = Status.Paused;
                    break;
                case Status.Paused:
                    this.playState = Status.Playing;
                    break;
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

        private void currentSong_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ChipFrequency")
            {
                ay.SetChipFreq(this.currentSong.ChipFrequency);
            }
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
        ~NotePlayer()
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
            ay.Reset();
        }
    }
}
