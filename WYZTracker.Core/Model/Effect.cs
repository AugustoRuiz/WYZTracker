using System;
using System.Collections.Generic;
using System.Text;

namespace WYZTracker
{
    public class Effect : System.ComponentModel.INotifyPropertyChanged
    {
        private int id;
        public int ID
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

        private byte[] noises;
        public byte[] Noises
        {
            get { return noises; }
            set
            {
                if (noises != value)
                {
                    noises = value;
                    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("Noises"));
                }
            }
        }

        private int[] frequencies;
        public int[] Frequencies
        {
            get { return frequencies; }
            set
            {
                if (frequencies != value)
                {
                    frequencies = value;
                    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("Frequencies"));
                }
            }
        }

        private int[] envFreqs;
        public int[] EnvFreqs
        {
            get { return envFreqs; }
            set
            {
                if (envFreqs!= value)
                {
                    envFreqs = value;
                    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("EnvFreqs"));
                }
            }
        }

        private byte[] envTypes;
        public byte[] EnvTypes
        {
            get { return envTypes; }
            set
            {
                if (envTypes != value)
                {
                    envTypes = value;
                    OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("EnvTypes"));
                }
            }
        }

        public void SetEffectLength(int p)
        {
            if (this.vols == null || p != this.vols.Length)
            {
                Array.Resize(ref this.vols, p);
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("Volumes"));
            }
            if (this.noises == null || p != this.noises.Length)
            {
                Array.Resize(ref this.noises, p);
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("Noises"));
            }
            if (this.frequencies == null || p != this.frequencies.Length)
            {
                Array.Resize(ref this.frequencies, p);
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("Frequencies"));
            }
            if (this.envFreqs == null || p != this.envFreqs.Length)
            {
                Array.Resize(ref this.envFreqs, p);
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("EnvFreqs"));
            }
            if (this.envTypes == null || p != this.envTypes.Length)
            {
                Array.Resize(ref this.envTypes, p);
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("EnvTypes"));
            }
        }

        #region Miembros de INotifyPropertyChanged

        protected void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
