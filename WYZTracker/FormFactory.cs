using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WYZTracker
{
    public class FormFactory
    {
        public static T FindFormOfType<T>()
            where T : Form, new()
        {
            T result = null;
            foreach (Form f in Application.OpenForms)
            {
                if (f is T && !f.IsDisposed)
                {
                    result = (T)f;
                }
            }
            return result;
        }

        public static T CreateOrActivateFormOfType<T>()
            where T : Form, new()
        {
            T result = null;

            result = FindFormOfType<T>();

            if (result == null)
            {
                result = new T();
                result.Show();
            }
            else
            {
                result.Activate();
            }

            return result;
        }

        public static void UpdateCurrentSong(Song currentSong)
        {
            foreach (Form f in Application.OpenForms)
            {
                if (f is ISongConsumer && (!(f is frmDockedMain)))
                {
                    ((ISongConsumer)f).CurrentSong = currentSong;
                }
            }
        }
    }
}
