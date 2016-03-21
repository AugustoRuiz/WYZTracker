using Sanford.Multimedia;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WYZTracker
{
    public class VirtualPiano : IDisposable
    {
        public enum PianoMode
        {
            Instrument,
            Fx
        }

        public class ChannelNoteEventArgs : EventArgs
        {
            public ChannelNote Note { get; private set; }

            public ChannelNoteEventArgs(ChannelNote note)
                : base()
            {
                this.Note = note;
            }
        }

        public class FxEventArgs : EventArgs
        {
            public int Fx { get; private set; }

            public FxEventArgs(int fx)
                : base()
            {
                this.Fx = fx;
            }
        }

        private static List<ChannelNote> virtTemplateNotes = new List<ChannelNote>();
        private static Dictionary<Keys, ChannelNote> virtPianoNotes = new Dictionary<Keys, ChannelNote>();
        private static Dictionary<Keys, int> virtPianoFx = new Dictionary<Keys, int>();

        private NotePlayer notePlayer;
        private EffectPlayer effectPlayer;

        public event EventHandler<ChannelNoteEventArgs> NotePressed;
        public event EventHandler<FxEventArgs> FxPressed;

        public Instrument CurrentInstrument { get; set; }

        public bool Enabled { get; set; }
        public PianoMode Mode { get; set; }

        private static List<InputDevice> _devices;
        private static List<VirtualPiano> _pianoInstances;

        private ChannelNote currentNote;
        private int currentFx;

        private Control _control;

        static VirtualPiano()
        {
            initPianoNotes();
            InitPianoKeys();
            initPianoFx();
            _pianoInstances = new List<VirtualPiano>();
            initMidiDevices();
        }

        public VirtualPiano(Control ctrl)
        {
            this.Mode = PianoMode.Instrument;
            this.initEventHandlers(ctrl);

            notePlayer = new NotePlayer();
            effectPlayer = new EffectPlayer();

            _pianoInstances.Add(this);
        }

        private void initEventHandlers(Control ctrl)
        {
            this._control = ctrl;
            ctrl.KeyDown += handleKeyDown;
            ctrl.KeyUp += handleKeyUp;
        }

        private static void initMidiDevices()
        {
            try
            {
                if (InputDevice.DeviceCount > 0)
                {
                    SynchronizationContext context = SynchronizationContext.Current;

                    _devices = new List<InputDevice>();
                    for (int iDevice = 0; iDevice < InputDevice.DeviceCount; ++iDevice)
                    {
                        InputDevice inDevice = new InputDevice(0);
                        inDevice.ChannelMessageReceived += HandleChannelMessageReceived;
                        _devices.Add(inDevice);

                        inDevice.StartRecording();
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    foreach (InputDevice d in _devices)
                    {
                        d.Dispose();
                    }
                    _devices.Clear();
                }
                catch(Exception exc2)
                {
                    Logger.Log(exc2.ToString());
                }

                Logger.Log(ex.ToString());
            }
        }

        private static void initPianoFx()
        {
            addFxToDictionary(new Keys[] 
                    { Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.D0, 
                      Keys.Q, Keys.W, Keys.E, Keys.R, Keys.T, Keys.Y, Keys.U, Keys.I, Keys.O, Keys.P, 
                      Keys.A, Keys.S, Keys.D, Keys.F, Keys.G, Keys.H, Keys.J, Keys.K, Keys.L, 
                      Keys.Z, Keys.X, Keys.C, Keys.V, Keys.B, Keys.N, Keys.M }
                );
        }

        private static void addFxToDictionary(Keys[] keys)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                virtPianoFx.Add(keys[i], i);
            }
        }

        private static void initPianoNotes()
        {
            virtTemplateNotes.Add(new ChannelNote() { Octave = 0, Note = 'C', Seminote = char.MinValue });
            virtTemplateNotes.Add(new ChannelNote() { Octave = 0, Note = 'C', Seminote = '+' });
            virtTemplateNotes.Add(new ChannelNote() { Octave = 0, Note = 'D', Seminote = char.MinValue });
            virtTemplateNotes.Add(new ChannelNote() { Octave = 0, Note = 'D', Seminote = '+' });
            virtTemplateNotes.Add(new ChannelNote() { Octave = 0, Note = 'E', Seminote = char.MinValue });
            virtTemplateNotes.Add(new ChannelNote() { Octave = 0, Note = 'F', Seminote = char.MinValue });
            virtTemplateNotes.Add(new ChannelNote() { Octave = 0, Note = 'F', Seminote = '+' });
            virtTemplateNotes.Add(new ChannelNote() { Octave = 0, Note = 'G', Seminote = char.MinValue });
            virtTemplateNotes.Add(new ChannelNote() { Octave = 0, Note = 'G', Seminote = '+' });
            virtTemplateNotes.Add(new ChannelNote() { Octave = 0, Note = 'A', Seminote = char.MinValue });
            virtTemplateNotes.Add(new ChannelNote() { Octave = 0, Note = 'A', Seminote = '+' });
            virtTemplateNotes.Add(new ChannelNote() { Octave = 0, Note = 'B', Seminote = char.MinValue });
        }

        public static void InitPianoKeys()
        {
            string kbdLayout = Properties.Settings.Default.KeyboardLayout;
            if(string.IsNullOrEmpty(kbdLayout))
            {
                kbdLayout = "QWERTY";
            }

            virtPianoNotes.Clear();

            switch (kbdLayout)
            {
                case "QWERTY":
                    // C-0
                    addNoteToDictionary(0, 'C', char.MinValue, Keys.Z);
                    // C#-0
                    addNoteToDictionary(0, 'C', '+', Keys.S);
                    // D-0
                    addNoteToDictionary(0, 'D', char.MinValue, Keys.X);
                    // D#-0
                    addNoteToDictionary(0, 'D', '+', Keys.D);
                    // E-0
                    addNoteToDictionary(0, 'E', char.MinValue, Keys.C);
                    // F-0
                    addNoteToDictionary(0, 'F', char.MinValue, Keys.V);
                    // F#-0
                    addNoteToDictionary(0, 'F', '+', Keys.G);
                    // G-0
                    addNoteToDictionary(0, 'G', char.MinValue, Keys.B);
                    // G#-0
                    addNoteToDictionary(0, 'G', '+', Keys.H);
                    // A-0
                    addNoteToDictionary(0, 'A', char.MinValue, Keys.N);
                    // A#-0
                    addNoteToDictionary(0, 'A', '+', Keys.J);
                    // B-0
                    addNoteToDictionary(0, 'B', char.MinValue, Keys.M);
                    // Siguiente Octava:
                    // C-1
                    addNoteToDictionary(1, 'C', char.MinValue, Keys.Q);
                    // C#-1
                    addNoteToDictionary(1, 'C', '+', Keys.D2);
                    // D-1
                    addNoteToDictionary(1, 'D', char.MinValue, Keys.W);
                    // D#-1
                    addNoteToDictionary(1, 'D', '+', Keys.D3);
                    // E-1
                    addNoteToDictionary(1, 'E', char.MinValue, Keys.E);
                    // F-1
                    addNoteToDictionary(1, 'F', char.MinValue, Keys.R);
                    // F#-1
                    addNoteToDictionary(1, 'F', '+', Keys.D5);
                    // G-1
                    addNoteToDictionary(1, 'G', char.MinValue, Keys.T);
                    // G#-1
                    addNoteToDictionary(1, 'G', '+', Keys.D6);
                    // A-1
                    addNoteToDictionary(1, 'A', char.MinValue, Keys.Y);
                    // A#-1
                    addNoteToDictionary(1, 'A', '+', Keys.D7);
                    // B-1
                    addNoteToDictionary(1, 'B', char.MinValue, Keys.U);
                    // Silencio
                    // P
                    addNoteToDictionary(int.MinValue, 'P', char.MinValue, Keys.Space);
                    break;
                case "QWERTZ":
                    // C-0
                    addNoteToDictionary(0, 'C', char.MinValue, Keys.Y);
                    // C#-0
                    addNoteToDictionary(0, 'C', '+', Keys.S);
                    // D-0
                    addNoteToDictionary(0, 'D', char.MinValue, Keys.X);
                    // D#-0
                    addNoteToDictionary(0, 'D', '+', Keys.D);
                    // E-0
                    addNoteToDictionary(0, 'E', char.MinValue, Keys.C);
                    // F-0
                    addNoteToDictionary(0, 'F', char.MinValue, Keys.V);
                    // F#-0
                    addNoteToDictionary(0, 'F', '+', Keys.G);
                    // G-0
                    addNoteToDictionary(0, 'G', char.MinValue, Keys.B);
                    // G#-0
                    addNoteToDictionary(0, 'G', '+', Keys.H);
                    // A-0
                    addNoteToDictionary(0, 'A', char.MinValue, Keys.N);
                    // A#-0
                    addNoteToDictionary(0, 'A', '+', Keys.J);
                    // B-0
                    addNoteToDictionary(0, 'B', char.MinValue, Keys.M);
                    // Siguiente Octava:
                    // C-1
                    addNoteToDictionary(1, 'C', char.MinValue, Keys.Q);
                    // C#-1
                    addNoteToDictionary(1, 'C', '+', Keys.D2);
                    // D-1
                    addNoteToDictionary(1, 'D', char.MinValue, Keys.W);
                    // D#-1
                    addNoteToDictionary(1, 'D', '+', Keys.D3);
                    // E-1
                    addNoteToDictionary(1, 'E', char.MinValue, Keys.E);
                    // F-1
                    addNoteToDictionary(1, 'F', char.MinValue, Keys.R);
                    // F#-1
                    addNoteToDictionary(1, 'F', '+', Keys.D5);
                    // G-1
                    addNoteToDictionary(1, 'G', char.MinValue, Keys.T);
                    // G#-1
                    addNoteToDictionary(1, 'G', '+', Keys.D6);
                    // A-1
                    addNoteToDictionary(1, 'A', char.MinValue, Keys.Z);
                    // A#-1
                    addNoteToDictionary(1, 'A', '+', Keys.D7);
                    // B-1
                    addNoteToDictionary(1, 'B', char.MinValue, Keys.U);
                    // Silencio
                    // P
                    addNoteToDictionary(int.MinValue, 'P', char.MinValue, Keys.Space);
                    break;
                case "AZERTY":
                    // C-0
                    addNoteToDictionary(0, 'C', char.MinValue, Keys.W);
                    // C#-0
                    addNoteToDictionary(0, 'C', '+', Keys.S);
                    // D-0
                    addNoteToDictionary(0, 'D', char.MinValue, Keys.X);
                    // D#-0
                    addNoteToDictionary(0, 'D', '+', Keys.D);
                    // E-0
                    addNoteToDictionary(0, 'E', char.MinValue, Keys.C);
                    // F-0
                    addNoteToDictionary(0, 'F', char.MinValue, Keys.V);
                    // F#-0
                    addNoteToDictionary(0, 'F', '+', Keys.G);
                    // G-0
                    addNoteToDictionary(0, 'G', char.MinValue, Keys.B);
                    // G#-0
                    addNoteToDictionary(0, 'G', '+', Keys.H);
                    // A-0
                    addNoteToDictionary(0, 'A', char.MinValue, Keys.N);
                    // A#-0
                    addNoteToDictionary(0, 'A', '+', Keys.J);
                    // B-0
                    addNoteToDictionary(0, 'B', char.MinValue, Keys.Oemcomma);
                    // Siguiente Octava:
                    // C-1
                    addNoteToDictionary(1, 'C', char.MinValue, Keys.A);
                    // C#-1
                    addNoteToDictionary(1, 'C', '+', Keys.D2);
                    // D-1
                    addNoteToDictionary(1, 'D', char.MinValue, Keys.Z);
                    // D#-1
                    addNoteToDictionary(1, 'D', '+', Keys.D3);
                    // E-1
                    addNoteToDictionary(1, 'E', char.MinValue, Keys.E);
                    // F-1
                    addNoteToDictionary(1, 'F', char.MinValue, Keys.R);
                    // F#-1
                    addNoteToDictionary(1, 'F', '+', Keys.D5);
                    // G-1
                    addNoteToDictionary(1, 'G', char.MinValue, Keys.T);
                    // G#-1
                    addNoteToDictionary(1, 'G', '+', Keys.D6);
                    // A-1
                    addNoteToDictionary(1, 'A', char.MinValue, Keys.Y);
                    // A#-1
                    addNoteToDictionary(1, 'A', '+', Keys.D7);
                    // B-1
                    addNoteToDictionary(1, 'B', char.MinValue, Keys.U);
                    // Silencio
                    // P
                    addNoteToDictionary(int.MinValue, 'P', char.MinValue, Keys.Space);
                    break;
                case "DVORAK":
                    // C-0
                    addNoteToDictionary(0, 'C', char.MinValue, Keys.OemSemicolon);
                    // C#-0
                    addNoteToDictionary(0, 'C', '+', Keys.O);
                    // D-0
                    addNoteToDictionary(0, 'D', char.MinValue, Keys.Q);
                    // D#-0
                    addNoteToDictionary(0, 'D', '+', Keys.E);
                    // E-0
                    addNoteToDictionary(0, 'E', char.MinValue, Keys.J);
                    // F-0
                    addNoteToDictionary(0, 'F', char.MinValue, Keys.K);
                    // F#-0
                    addNoteToDictionary(0, 'F', '+', Keys.I);
                    // G-0
                    addNoteToDictionary(0, 'G', char.MinValue, Keys.X);
                    // G#-0
                    addNoteToDictionary(0, 'G', '+', Keys.D);
                    // A-0
                    addNoteToDictionary(0, 'A', char.MinValue, Keys.B);
                    // A#-0
                    addNoteToDictionary(0, 'A', '+', Keys.H);
                    // B-0
                    addNoteToDictionary(0, 'B', char.MinValue, Keys.M);
                    // Siguiente Octava:
                    // C-1
                    addNoteToDictionary(1, 'C', char.MinValue, Keys.OemQuotes);
                    // C#-1
                    addNoteToDictionary(1, 'C', '+', Keys.D2);
                    // D-1
                    addNoteToDictionary(1, 'D', char.MinValue, Keys.Oemcomma);
                    // D#-1
                    addNoteToDictionary(1, 'D', '+', Keys.D3);
                    // E-1
                    addNoteToDictionary(1, 'E', char.MinValue, Keys.OemPeriod);
                    // F-1
                    addNoteToDictionary(1, 'F', char.MinValue, Keys.P);
                    // F#-1
                    addNoteToDictionary(1, 'F', '+', Keys.D5);
                    // G-1
                    addNoteToDictionary(1, 'G', char.MinValue, Keys.Y);
                    // G#-1
                    addNoteToDictionary(1, 'G', '+', Keys.D6);
                    // A-1
                    addNoteToDictionary(1, 'A', char.MinValue, Keys.F);
                    // A#-1
                    addNoteToDictionary(1, 'A', '+', Keys.D7);
                    // B-1
                    addNoteToDictionary(1, 'B', char.MinValue, Keys.G);
                    // Silencio
                    // P
                    addNoteToDictionary(int.MinValue, 'P', char.MinValue, Keys.Space);
                    break;
            }
        }

        private static void addNoteToDictionary(int octave, char note, char semiNote, Keys key)
        {
            ChannelNote tmp;

            tmp = new ChannelNote();
            tmp.Octave = octave;
            tmp.Note = note;
            tmp.Seminote = semiNote;
            virtPianoNotes.Add(key, tmp);
        }

        private void Stop()
        {
            this.notePlayer.Stop();
            this.effectPlayer.Stop();
        }

        private void StopEffect(int fxIndex)
        {
            if (fxIndex == this.currentFx)
            {
                this.effectPlayer.Stop();
                this.currentFx = int.MinValue;
            }
        }

        private void StopEffect(Keys keyCode)
        {
            if (virtPianoFx.ContainsKey(keyCode))
            {
                this.StopEffect(virtPianoFx[keyCode]);
            }
        }

        private void StopNote(ChannelNote note)
        {
            if (note != null && currentNote != null && note.Octave == currentNote.Octave && note.Note == currentNote.Note &&
                note.Seminote == currentNote.Seminote)
            {
                this.notePlayer.Stop();
                this.currentNote = null;
            }
        }

        private void StopNote(Keys keyCode)
        {
            ChannelNote note = this.GetNoteFromKey(keyCode);
            this.StopNote(note);
        }

        private void StopNote(int pianoNote)
        {
            ChannelNote note = this.GetNoteFromPiano(pianoNote);
            this.StopNote(note);
        }

        private void PlayNote(Keys key)
        {
            ChannelNote dstNote = GetNoteFromKey(key);
            if (dstNote != null)
            {
                PlayNote(dstNote);
            }
        }

        private void PlayNote(ChannelNote dstNote)
        {
            if (currentNote == null || 
                   (dstNote != null && currentNote != null && 
                      (dstNote.Octave != currentNote.Octave || dstNote.Note != currentNote.Note ||
                       dstNote.Seminote != currentNote.Seminote)
                   ))
            {
                if (currentNote != null)
                {
                    notePlayer.Stop();
                }
                this.notePlayer.CurrentSong = ApplicationState.CurrentSong;
                this.notePlayer.Tempo = ApplicationState.CurrentSong.Tempo;
                this.notePlayer.CurrentNote = dstNote;

                this.currentNote = dstNote;
                notePlayer.CurrentInstrument = this.CurrentInstrument;
                notePlayer.Play();
                this.OnNotePressed(dstNote);
            }
        }

        private ChannelNote GetNoteFromKey(Keys key)
        {
            ChannelNote dstNote = null;
            if (virtPianoNotes.ContainsKey(key))
            {
                ChannelNote pressedNote = virtPianoNotes[key];
                dstNote = new ChannelNote();

                if (pressedNote.HasOctave)
                    dstNote.Octave = pressedNote.Octave + ApplicationState.BaseOctave;
                else
                    dstNote.Octave = pressedNote.Octave;

                dstNote.Note = pressedNote.Note;
                dstNote.Seminote = pressedNote.Seminote;

                dstNote.EnvData.ActiveFrequencies = ApplicationState.CurrentEnvData.ActiveFrequencies;
                dstNote.EnvData.FrequencyRatio = ApplicationState.CurrentEnvData.FrequencyRatio;
                dstNote.EnvData.Style = ApplicationState.CurrentEnvData.Style;
            }
            return dstNote;
        }

        private void PlayNote(int note)
        {
            ChannelNote dstNote = GetNoteFromPiano(note);
            this.PlayNote(dstNote);
        }

        private ChannelNote GetNoteFromPiano(int note)
        {
            ChannelNote pressedNote = virtTemplateNotes[note % virtTemplateNotes.Count];
            ChannelNote dstNote = new ChannelNote();

            dstNote.Octave = note / virtTemplateNotes.Count;
            dstNote.Note = pressedNote.Note;
            dstNote.Seminote = pressedNote.Seminote;

            dstNote.EnvData.ActiveFrequencies = ApplicationState.CurrentEnvData.ActiveFrequencies;
            dstNote.EnvData.FrequencyRatio = ApplicationState.CurrentEnvData.FrequencyRatio;
            dstNote.EnvData.Style = ApplicationState.CurrentEnvData.Style;
            return dstNote;
        }

        private bool IsFx(Keys key)
        {
            return virtPianoFx.ContainsKey(key) && virtPianoFx[key] < ApplicationState.CurrentSong.Effects.Count;
        }

        private void PlayEffect(int effectIndex)
        {
            effectPlayer.Stop();
            effectPlayer.CurrentEffect = ApplicationState.CurrentSong.Effects[effectIndex];
            effectPlayer.Play();
            this.currentFx = effectIndex;
            this.OnFxPressed(effectIndex);
        }

        private void PlayEffect(Keys key)
        {
            if (IsFx(key))
            {
                this.PlayEffect(virtPianoFx[key]);
            }
        }

        private static void HandleChannelMessageReceived(object sender, ChannelMessageEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("{0}, {1}, {2}, {3}, {4}, {5}", e.Message.Command.ToString(),
                e.Message.Data1, e.Message.Data2, e.Message.MessageType.ToString(), e.Message.MidiChannel, e.Message.Status));

            switch (e.Message.Command)
            {
                case ChannelCommand.NoteOn:
                    foreach (VirtualPiano inst in _pianoInstances)
                    {
                        inst.handlePianoDown(e.Message.Data1);
                    }
                    break;
                case ChannelCommand.NoteOff:
                    foreach (VirtualPiano inst in _pianoInstances)
                    {
                        inst.handlePianoUp(e.Message.Data1);
                    }
                    break;
                default:
                    break;
            }
        }

        void handleKeyUp(object sender, KeyEventArgs e)
        {
            switch (this.Mode)
            {
                case PianoMode.Fx:
                    this.StopEffect(e.KeyCode);
                    break;
                case PianoMode.Instrument:
                    this.StopNote(e.KeyCode);
                    break;
            }
        }

        void handleKeyDown(object sender, KeyEventArgs e)
        {
            if (this.Enabled)
            {
                switch (this.Mode)
                {
                    case PianoMode.Fx:
                        this.PlayEffect(e.KeyCode);
                        break;
                    case PianoMode.Instrument:
                        this.PlayNote(e.KeyCode);
                        break;
                }
            }
        }

        void handlePianoDown(int note)
        {
            if (this.Enabled)
            {
                switch (this.Mode)
                {
                    case PianoMode.Fx:
                        int realNote = note - 48;
                        if (realNote > 0 && realNote < ApplicationState.CurrentSong.Effects.Count)
                            this.PlayEffect(realNote);
                        break;
                    case PianoMode.Instrument:
                        this.PlayNote(note);
                        break;
                }
            }
        }

        void handlePianoUp(int note)
        {
            switch (this.Mode)
            {
                case PianoMode.Fx:
                    int realNote = note - 48;
                    if (realNote > 0 && realNote < ApplicationState.CurrentSong.Effects.Count)
                        this.StopEffect(realNote);
                    break;
                case PianoMode.Instrument:
                    this.StopNote(note);
                    break;
            }
            //this.Stop();
        }

        protected virtual void OnNotePressed(ChannelNote note)
        {
            EventHandler<ChannelNoteEventArgs> tmp = this.NotePressed;
            if (tmp != null)
            {
                ChannelNoteEventArgs e = new ChannelNoteEventArgs(note);
                tmp(this, e);
            }
        }

        protected virtual void OnFxPressed(int fx)
        {
            EventHandler<FxEventArgs> tmp = this.FxPressed;
            if (tmp != null)
            {
                FxEventArgs e = new FxEventArgs(fx);
                tmp(this, e);
            }
        }

        #region IDisposable implementation

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~VirtualPiano()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.notePlayer != null)
            {
                this.notePlayer.Dispose();
                this.notePlayer = null;
            }
            if (this.effectPlayer != null)
            {
                this.effectPlayer.Dispose();
                this.effectPlayer = null;
            }

            if (this._control != null)
            {
                this._control.KeyDown -= handleKeyDown;
                this._control.KeyUp -= handleKeyUp;
                this._control = null;
            }

            if (VirtualPiano._pianoInstances.Contains(this))
            {
                VirtualPiano._pianoInstances.Remove(this);
            }
        }

        public static void ReleaseDevices()
        {
            if (_devices != null)
            {
                foreach (InputDevice d in _devices)
                {
                    d.StopRecording();
                    d.Dispose();
                }
                _devices.Clear();
                _devices = null;
            }
        }

        #endregion
    }
}
