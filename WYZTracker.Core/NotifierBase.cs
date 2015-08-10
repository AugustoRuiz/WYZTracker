using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace WYZTracker
{
    public abstract class NotifierBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implementation

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler tmp = this.PropertyChanged;
            if (tmp != null)
            {
                tmp(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
