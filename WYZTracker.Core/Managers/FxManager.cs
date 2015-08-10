using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

namespace WYZTracker
{
    public class FxManager
    {
        public static void Import(Song currentSong)
        {
            Effects loadedFX = null;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = WYZTracker.Core.Properties.Resources.FXFilter;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                XmlSerializer formatter = new XmlSerializer(typeof(Effects));
                Stream objFileStream = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.None);
                try
                {
                    loadedFX = (Effects)formatter.Deserialize(objFileStream);
                }
                catch
                {
                    try
                    {
                        formatter = new XmlSerializer(typeof(Song));
                        objFileStream.Seek(0, SeekOrigin.Begin);
                        objFileStream = SongManager.GetStream(objFileStream);
                        loadedFX = ((Song)formatter.Deserialize(objFileStream)).Effects;
                    }
                    catch
                    {
                        loadedFX = null;
                    }
                }
                objFileStream.Close();
            }
            if (loadedFX != null)
            {
                foreach (Effect fx in loadedFX)
                {
                    fx.ID = currentSong.Effects.GetSafeId();
                    currentSong.Effects.Add(fx);
                }
            }
        }

        public static void Export(Effect effect)
        {
            Export(new Effects() { effect });
        }

        public static void Export(Song currentSong)
        {
            Export(currentSong.Effects);
        }

        public static void Export(Effects effects)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = WYZTracker.Core.Properties.Resources.FXFilter;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                XmlSerializer formatter = new XmlSerializer(typeof(Effects));
                Stream objFileStream = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(objFileStream, effects);
                objFileStream.Close();
            }
        }

        public static void AddNew(Song currentSong)
        {
            Effect nuevo = new Effect();

            nuevo.SetEffectLength(5);

            nuevo.Name = WYZTracker.Core.Properties.Resources.New;
            nuevo.Volumes = new byte[] { 8, 8, 8, 8, 8 };
            nuevo.Frequencies = new int[] { 440, 440, 440, 440, 440 };
            nuevo.Noises = new byte[] { 0, 1, 2, 3, 4 };
            nuevo.ID = currentSong.Effects.GetSafeId();
            currentSong.Effects.Add(nuevo);
        }

        public static void Remove(Song currentSong, Effect effect)
        {
            currentSong.Effects.Remove(effect);
        }
    }
}
