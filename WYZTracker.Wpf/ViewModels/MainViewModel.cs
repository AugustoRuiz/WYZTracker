using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WYZTracker.Wpf.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private const byte DEFAULT_NUM_CHANNELS = 3;

        public event PropertyChangedEventHandler PropertyChanged;

        private Song _song;
        private Pattern _pattern;
        private Instrument _instrument;
        private Effect _effect;
        private ObservableList<int> _playOrder;

        private int _patternIdx;
        private string _songFilePath;
        private bool _isDirty;

        public MainViewModel()
        {
            this.New();
        }

        #region Properties

        public Song Song
        {
            get { return _song; }
            set
            {
                if (value != _song)
                {
                    setSong(value);
                    OnPropertyChanged("Song");
                    OnPropertyChanged("WindowTitle");

                }
            }
        }

        public ObservableList<int> PlayOrder
        {
            get
            {
                return this._playOrder;
            }
        }

        public Pattern Pattern
        {
            get { return _pattern; }
            set
            {
                if (value != _pattern)
                {
                    int oldPatLength = this.PatternLength;

                    _pattern = value;
                    OnPropertyChanged("Pattern");

                    if (this.PlayOrder != null)
                    {
                        if (isValidPatternIndex() &&
                            this.Song.Patterns[this.PlayOrder[this._patternIdx]] != value)
                        {
                            this.PatternIndex = this.PlayOrder.IndexOf(this.Song.Patterns.IndexOf(value));
                        }
                    }

                    if (PatternLength != oldPatLength)
                    {
                        OnPropertyChanged("PatternLength");
                    }
                }
            }
        }

        public int PatternLength
        {
            get
            {
                if (this.Song != null && this.Song.Patterns != null && this.PlayOrder != null &&
                    isValidPatternIndex() && this.Song.Patterns[this.PlayOrder[this.PatternIndex]] != null)
                {
                    return this.Song.Patterns[this.PlayOrder[this.PatternIndex]].Length;
                }
                return 0;
            }
            set
            {
                int oldPatLength = this.PatternLength;
                UndoManager.Execute(new Commands.PatternChangeLength(this.Pattern, value));
                if (value != oldPatLength)
                {
                    OnPropertyChanged("Pattern");
                    OnPropertyChanged("PatternLength");
                }
            }
        }

        public Instrument Instrument
        {
            get { return _instrument; }
            set
            {
                if (value != _instrument)
                {
                    _instrument = value;
                    OnPropertyChanged("Instrument");
                }
            }
        }

        public Effect Effect
        {
            get { return _effect; }
            set
            {
                if (value != _effect)
                {
                    _effect = value;
                    OnPropertyChanged("Effect");
                }
            }
        }

        public string SongFilePath
        {
            get { return _songFilePath; }
            set
            {
                if (value != _songFilePath)
                {
                    _songFilePath = value;
                    OnPropertyChanged("SongFilePath");
                    OnPropertyChanged("WindowTitle");
                }
            }
        }

        public bool IsDirty
        {
            get
            {
                return _isDirty;
            }
            set
            {
                if (_isDirty != value)
                {
                    _isDirty = value;
                    OnPropertyChanged("IsDirty");
                    OnPropertyChanged("WindowTitle");
                }
            }
        }

        public string WindowTitle
        {
            get
            {
                string result = "WYZTracker 3";
                if (!string.IsNullOrEmpty(_songFilePath))
                {
                    result += string.Format(" - [{0}]", this._songFilePath);
                }
                if (this.IsDirty)
                {
                    result += "*";
                }
                return result;
            }
        }

        public int PatternIndex
        {
            get
            {
                return this._patternIdx;
            }
            set
            {
                if (this.Song != null && this.PlayOrder != null && value >= 0 &&
                    value < this.PlayOrder.Count && this._patternIdx != value)
                {
                    this._patternIdx = value;
                    OnPropertyChanged("PatternIndex");
                    this.Pattern = this.Song.Patterns[this.PlayOrder[this._patternIdx]];
                }
                if (value == -1)
                {
                    this.Pattern = null;
                }
            }
        }

        public void MovePatternDown()
        {
            UndoManager.Execute(new Commands.MovePattern(this, 1));
            this.IsDirty = true;
            this.OnPropertyChanged("Pattern");
        }

        public void MovePatternUp()
        {
            UndoManager.Execute(new Commands.MovePattern(this, -1));
            this.IsDirty = true;
            this.OnPropertyChanged("Pattern");
        }

        public void NextPattern()
        {
            UndoManager.Execute(new Commands.NextPattern(this, 1));
            this.IsDirty = true;
            this.OnPropertyChanged("Pattern");
        }

        public void PreviousPattern()
        {
            UndoManager.Execute(new Commands.NextPattern(this, -1));
            this.IsDirty = true;
            this.OnPropertyChanged("Pattern");
        }

        public void AddPattern()
        {
            UndoManager.Execute(new Commands.AddPattern(this));
            this.IsDirty = true;
            this.OnPropertyChanged("Pattern");
        }

        public void RemovePattern()
        {
            UndoManager.Execute(new Commands.RemovePattern(this));
            this.IsDirty = true;
            this.OnPropertyChanged("Pattern");
        }

        public void ClonePattern()
        {
            UndoManager.Execute(new Commands.ClonePattern(this));
            this.IsDirty = true;
            this.OnPropertyChanged("Pattern");
        }

        #endregion

        #region VM Actions

        public void New()
        {
            this.Song = new Song(DEFAULT_NUM_CHANNELS);
            this.Song.Name = "New Song";
            this.Song.Tempo = 4;

            this.Song.PlayOrder.Add(0);

            this.SongFilePath = null;

            this.Song.Frequencies = NoteFileLoader.LoadDefaultNotes();
        }

        public void Open(string filePath)
        {
            this.Song = SongManager.LoadSong(filePath);
            this.SongFilePath = filePath;
        }

        public void Save(string filePath)
        {
            SongManager.SaveSong(this.Song, filePath);
            this.SongFilePath = filePath;
            this.IsDirty = false;
        }

        public void ExportMus(string filePath)
        {
            Stream str = new FileStream(filePath, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(str);

            byte[] result = SongBinProvider.GenerateSong(this.Song);

            writer.Write(result);
            writer.Close();

            Stream textStr = new FileStream(string.Format("{0}.asm", filePath), FileMode.Create);
            StreamWriter textWriter = new StreamWriter(textStr);
            textWriter.Write(SongBinProvider.GenerateInstrumentsAndFX(this.Song));
            textWriter.Close();

            str.Dispose();
        }

        private void setSong(Song value)
        {
            _song = value;
            if (_song.Patterns != null && _song.Patterns.Count > 0)
            {
                this.Pattern = this._song.Patterns[0];
            }

            if (_song.Instruments != null && _song.Instruments.Count > 0)
            {
                this.Instrument = this.Song.Instruments[0];
            }
            this.Effect = null;
            this._playOrder = new ObservableList<int>(value.PlayOrder);
            this.OnPropertyChanged("PlayOrder");
        }

        private bool isValidPatternIndex()
        {
            return this.PatternIndex >= 0 && this.PatternIndex < this.PlayOrder.Count;
        }

        #endregion

        #region Property changed notification

        protected void OnPropertyChanged(string propName)
        {
            PropertyChangedEventHandler tmp = this.PropertyChanged;
            if (tmp != null)
            {
                PropertyChangedEventArgs args = new PropertyChangedEventArgs(propName);
                tmp(this, args);
            }
        }

        #endregion 

    }
}
