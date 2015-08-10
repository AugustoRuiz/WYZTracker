using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using LibAYEmu;
using System.ComponentModel;

namespace WYZTracker
{
    public class EffectPlayer : IDisposable
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

        private Effect currentEffect;
        private Status playState;
        private int periodCount;
        private LibAYEmu.AY ay = new LibAYEmu.AY();

        private PlaybackStreamer streamer = null;
        private Song currentSong;

        public EffectPlayer()
        {
            streamer = new PlaybackStreamer();
            streamer.BufferLengthInMs = GENSOUND_PERIOD;
            streamer.FillBuffer += new EventHandler<FillBufferEventArgs>(OnFillBuffer);

            this.playState = Status.Stopped;

            ay.SetChipType(LibAYEmu.Chip.AY_Lion17, null);
        }

        public bool Playing
        {
            get { return this.playState == Status.Playing; }
        }

        public bool Paused
        {
            get { return this.playState == Status.Paused; }
        }

        public Effect CurrentEffect
        {
            get
            {
                return currentEffect;
            }
            set
            {
                currentEffect = value;
                this.Reset();
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

        /// <summary>
        /// Fills the provided buffer with audio data. Stereo 16-bits data is required.
        /// </summary>
        /// <param name="sender">Streamer object.</param>
        /// <param name="instr">FillBufferEventArgs.</param>
        private void OnFillBuffer(object sender, FillBufferEventArgs e)
        {
            if (this.currentEffect != null && (this.playState == Status.Playing))
            {
                int envFreq = 0;
                int envStyle = 0xFF;

                try
                {
                    if (periodCount >= 0 && periodCount < this.currentEffect.Volumes.Length)
                    {
                        int noise = this.currentEffect.Noises[periodCount];
                        int vol = this.currentEffect.Volumes[periodCount];
                        int toneFreq = this.currentEffect.Frequencies[periodCount];

                        if ((noise & 0x80) == 0x80)
                        {
                            vol |= 0x10;
                            envStyle = this.currentEffect.EnvTypes[periodCount];
                            if (envStyle == 0x01)
                            {
                                envStyle = 0xFF;
                            }
                            envFreq = this.currentEffect.EnvFreqs[periodCount];
                            noise = noise ^ 0x80;
                        }

                        ChannelControl ctrl = ChannelControl.NONE;
                        if (vol > 0)
                        {
                            ctrl |= ChannelControl.TONE_A;
                        }
                        if (noise > 0)
                        {
                            ctrl |= ChannelControl.NOISE_A;
                        }

                        this.genSound(toneFreq,
                                        0,
                                        0,
                                        noise,
                                        (int)ctrl,
                                        vol,
                                        0,
                                        0,
                                        envFreq,
                                        envStyle,
                                        e.Buffer,
                                        ay,
                                        true
                                     );
                    }
                    else
                    {
                        for (long i = 0; i < e.Buffer.LongLength; i++)
                        {
                            e.Buffer[i] = (byte)0;
                        }
                    }

                    this.periodCount++;

                    if (this.periodCount > this.currentEffect.Volumes.Length)
                    {
                        this.Stop();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.ToString());
                }
            }
            else
            {
                for (long i = 0; i < e.Buffer.LongLength; i++)
                {
                    e.Buffer[i] = (byte)0;
                }
            }
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

        private void currentSong_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ChipFrequency")
            {
                this.ay.SetChipFreq(this.currentSong.ChipFrequency);
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
        ~EffectPlayer()
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
            if (this.currentSong != null)
            {
                ay.SetChipFreq(this.currentSong.ChipFrequency);
            }
            ay.Reset();
        }
    }
}
