using Sanford.Multimedia;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public class NoteFXEventArgs : EventArgs
        {
            public ChannelNote Note { get; private set; }
            public int Fx { get; private set; }
            public NoteFXEventArgs(ChannelNote note, int fx)
                : base()
            {
                this.Note = note;
                this.Fx = fx;
            }

        }

        private static List<ChannelNote> virtTemplateNotes = new List<ChannelNote>();
        private static Dictionary<Keys, ChannelNote> virtPianoNotes = new Dictionary<Keys, ChannelNote>();
        private static Dictionary<Keys, int> virtPianoFx = new Dictionary<Keys, int>();

        private Player player;

        public event EventHandler<NoteFXEventArgs> NoteFxPressed;

        public Instrument CurrentInstrument { get; set; }

        public bool Enabled { get; set; }
        public PianoMode Mode { get; set; }

        private static List<InputDevice> _devices;
        private static List<VirtualPiano> _pianoInstances;

        private Control _control;

        private int _lastFx = int.MinValue;
        private ChannelNote _lastNote;

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

            player = new Player();

            _pianoInstances.Add(this);
        }

        private void initEventHandlers(Control ctrl)
        {
            this._control = ctrl;
            ctrl.KeyDown += handleKeyDown;
            ctrl.KeyUp += handleKeyUp;

            ApplicationState.Instance.PropertyChanged += OnApplicationStatePropertyChanged;
        }

        private void OnApplicationStatePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentSong")
            {

            }
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
                        try
                        {
                            inDevice.StartRecording();
                            _devices.Add(inDevice);
                        }
                        catch (Sanford.Multimedia.Midi.InputDeviceException e)
                        {
                            try
                            {
                                inDevice.StopRecording();
                            }
                            catch(Exception e2)
                            {
                                Logger.Log(e2.ToString());
                            }
                            inDevice.Dispose();
                            inDevice.ChannelMessageReceived -= HandleChannelMessageReceived;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
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
            if (string.IsNullOrEmpty(kbdLayout))
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
                    addNoteToDictionary(1, 'C', char.MinValue, Keys.Oemcomma);
                    addNoteToDictionary(1, 'C', char.MinValue, Keys.Q);
                    // C#-1
                    addNoteToDictionary(1, 'C', '+', Keys.L);
                    addNoteToDictionary(1, 'C', '+', Keys.D2);
                    // D-1
                    addNoteToDictionary(1, 'D', char.MinValue, Keys.OemPeriod);
                    addNoteToDictionary(1, 'D', char.MinValue, Keys.W);
                    // D#-1
                    addNoteToDictionary(1, 'D', '+', Keys.Oem3);     // Ñ
                    addNoteToDictionary(1, 'D', '+', Keys.D3);
                    // E-1
                    addNoteToDictionary(1, 'E', char.MinValue, Keys.OemMinus);
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
                    // C-2
                    addNoteToDictionary(2, 'C', char.MinValue, Keys.I);
                    // C#-2
                    addNoteToDictionary(2, 'C', '+', Keys.D9);
                    // D-2
                    addNoteToDictionary(2, 'D', char.MinValue, Keys.O);
                    // D#-2
                    addNoteToDictionary(2, 'D', '+', Keys.D0);
                    // E-2
                    addNoteToDictionary(2, 'E', char.MinValue, Keys.P);
                    // F-2
                    addNoteToDictionary(2, 'F', char.MinValue, Keys.Oem1);
                    // F#-2
                    addNoteToDictionary(2, 'F', '#', Keys.Oem6);
                    // G-2
                    addNoteToDictionary(2, 'G', char.MinValue, Keys.Oemplus);
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
            this.player.Stop();
        }

        private void StopEffect(int fxIndex)
        {
            this.player.Stop();
            this._lastFx = int.MinValue;
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
            if (note != null && _lastNote != null &&
                _lastNote.Octave == note.Octave && _lastNote.Note == note.Note &&
                _lastNote.Seminote == note.Seminote && _lastNote.Instrument == note.Instrument)
            {
                this.player.Stop();
                this._lastNote = null;
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

        private void PlayNote(ChannelNote dstNote)
        {
            if (dstNote != null &&
                (_lastNote == null || _lastNote.Octave != dstNote.Octave || _lastNote.Note != dstNote.Note ||
                _lastNote.Seminote != dstNote.Seminote || _lastNote.Instrument != dstNote.Instrument))
            {
                player.Stop();

                Song tempSong = SerializationUtils.Clone(ApplicationState.Instance.CurrentSong);
                tempSong.PlayOrder.Clear();
                tempSong.Patterns.Clear();
                tempSong.Looped = true;
                tempSong.Patterns.Add(new Pattern() { Length = 256, Channels = tempSong.Channels });
                tempSong.PlayOrder.Add(0);
                ChannelNote tmpNote = tempSong.Patterns[0].Lines[0].Notes[0];
                tmpNote.EnvData = dstNote.EnvData;
                tmpNote.Instrument = this.CurrentInstrument.ID;
                tmpNote.Note = dstNote.Note;
                tmpNote.Octave = dstNote.Octave;
                tmpNote.Seminote = dstNote.Seminote;
                tmpNote.VolModifier = dstNote.VolModifier;

                player.CurrentSong = tempSong;
                player.CurrentTempo = tempSong.Tempo;
                player.Play();

                _lastNote = dstNote;
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
                    dstNote.Octave = pressedNote.Octave + ApplicationState.Instance.BaseOctave;
                else
                    dstNote.Octave = pressedNote.Octave;

                dstNote.Note = pressedNote.Note;
                dstNote.Seminote = pressedNote.Seminote;

                dstNote.EnvData.ActiveFrequencies = ApplicationState.Instance.CurrentEnvData.ActiveFrequencies;
                dstNote.EnvData.FrequencyRatio = ApplicationState.Instance.CurrentEnvData.FrequencyRatio;
                dstNote.EnvData.Style = ApplicationState.Instance.CurrentEnvData.Style;
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

            dstNote.EnvData.ActiveFrequencies = ApplicationState.Instance.CurrentEnvData.ActiveFrequencies;
            dstNote.EnvData.FrequencyRatio = ApplicationState.Instance.CurrentEnvData.FrequencyRatio;
            dstNote.EnvData.Style = ApplicationState.Instance.CurrentEnvData.Style;
            return dstNote;
        }

        private bool IsFx(Keys key)
        {
            return virtPianoFx.ContainsKey(key) && virtPianoFx[key] < ApplicationState.Instance.CurrentSong.Effects.Count;
        }

        public void PlayEffect(int effectIndex)
        {
            if (effectIndex != _lastFx)
            {
                player.Stop();

                Song tempSong = SerializationUtils.Clone(ApplicationState.Instance.CurrentSong);
                tempSong.Patterns.Clear();
                tempSong.Looped = true;
                tempSong.Patterns.Add(new Pattern() { Length = 256, Channels = tempSong.Channels });
                tempSong.Patterns[0].Lines[0].Fx = effectIndex;

                player.CurrentSong = tempSong;
                player.CurrentTempo = tempSong.Tempo;
                player.Play();
                _lastFx = effectIndex;
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
            ChannelNote note = GetNoteFromKey(e.KeyCode);
            int fx = IsFx(e.KeyCode) ? virtPianoFx[e.KeyCode] : int.MinValue;
            if (this.Enabled)
            {
                switch (this.Mode)
                {
                    case PianoMode.Fx:
                        this.PlayEffect(fx);
                        break;
                    case PianoMode.Instrument:
                        this.PlayNote(note);
                        break;
                }
            }
            if (note != null || fx != int.MinValue)
            {
                this.OnNoteOrFxPressed(note, fx);
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
                        if (realNote > 0 && realNote < ApplicationState.Instance.CurrentSong.Effects.Count)
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
                    if (realNote > 0 && realNote < ApplicationState.Instance.CurrentSong.Effects.Count)
                        this.StopEffect(realNote);
                    break;
                case PianoMode.Instrument:
                    this.StopNote(note);
                    break;
            }
        }

        protected virtual void OnNoteOrFxPressed(ChannelNote note, int fx)
        {
            EventHandler<NoteFXEventArgs> tmp = this.NoteFxPressed;
            if (tmp != null)
            {
                NoteFXEventArgs e = new NoteFXEventArgs(note, fx);
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
            if (this.player != null)
            {
                this.player.Dispose();
                this.player = null;
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
