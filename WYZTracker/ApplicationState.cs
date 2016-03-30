using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WYZTracker
{
    internal class ApplicationState : INotifyPropertyChanged
    {
        private static ApplicationState _instance = new ApplicationState();
        public static ApplicationState Instance { get { return _instance; } }

        private Song _currentSong;
        private int _baseOctave;
        private EnvelopeData _envData;
        private Splash _splashScreen;
        private string _fileName;
        private LibAYEmu.Stereo _stereo;

        private ApplicationState()
        {
            BaseOctave = 3;
            Stereo = LibAYEmu.Stereo.Mono;
            CurrentEnvData = new EnvelopeData();
        }

        public int BaseOctave
        {
            get
            {
                return _baseOctave;
            }
            set
            {
                if (value != _baseOctave)
                {
                    this._baseOctave = value;
                    OnPropertyChanged("BaseOctave");
                }
            }
        }

        public EnvelopeData CurrentEnvData
        {
            get
            {
                return _envData;
            }
            set
            {
                if (value != _envData)
                {
                    this._envData = value;
                    OnPropertyChanged("CurrentEnvData");
                }
            }
        }

        public Splash SplashScreen
        {
            get
            {
                return _splashScreen;
            }
            set
            {
                if (value != _splashScreen)
                {
                    this._splashScreen = value;
                    OnPropertyChanged("SplashScreen");
                }
            }
        }

        public Song CurrentSong
        {
            get
            {
                return _currentSong;
            }
            set
            {
                if (value != CurrentSong)
                {
                    _currentSong = value;
                    OnPropertyChanged("CurrentSong");
                }
            }
        }

        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                if (value != _fileName)
                {
                    this._fileName = value;
                    this.OnPropertyChanged("FileName");
                }
            }
        }

        public LibAYEmu.Stereo Stereo
        {
            get
            {
                return _stereo;
            }
            set
            {
                if (value != this._stereo)
                {
                    this._stereo = value;
                    this.OnPropertyChanged("Stereo");
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler tmp = PropertyChanged;
            if (tmp != null)
            {
                tmp(null, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
