using System;
using System.Collections.Generic;
using System.Text;

namespace WYZTracker
{
    public class Instrument : System.ComponentModel.INotifyPropertyChanged
    {
        private string id;

        public string ID
        {
            get { return id; }
            set 
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("ID"));
                }
            }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set 
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("Name"));
                }
            }
        }

        private byte[] vols;

        public byte[] Volumes
        {
            get { return vols; }
            set 
            {
                if (vols != value)
                {
                    vols = value;
                    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("Volumes"));
                }
            }
        }

        private int[] pitchModifiers;

        public int[] PitchModifiers
        {
            get { return pitchModifiers; }
            set
            {
                if (pitchModifiers != value)
                {
                    pitchModifiers = value;
                    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("PitchModifiers"));
                }
            }
        }

        private bool looped;

        public bool Looped
        {
            get { return looped; }
            set 
            { 
                looped = value;
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("Looped"));
            }
        }

        private int loopStart;

        public int LoopStart
        {
            get { return loopStart; }
            set 
            { 
                loopStart = value;
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("LoopStart"));
            }
        }

        public void SetVolumeLength(int p)
        {
            if (this.vols == null || p != this.vols.Length)
            {
                Array.Resize(ref this.vols, p);
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("Volumes"));
            }

            if (this.pitchModifiers == null || p != this.pitchModifiers.Length)
            {
                Array.Resize(ref this.pitchModifiers, p);
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("PitchModifiers"));
            }
        }

        #region Miembros de INotifyPropertyChanged

        protected void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
