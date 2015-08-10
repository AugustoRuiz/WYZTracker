using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

namespace WYZTracker
{
    public class InstrumentsManager
    {
        public static void Import(Song currentSong)
        {
            Instruments loadedInstruments = null;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = WYZTracker.Core.Properties.Resources.INSFilter;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                XmlSerializer formatter = new XmlSerializer(typeof(Instruments));
                Stream objFileStream = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.None);
                try
                {
                    loadedInstruments = (Instruments)formatter.Deserialize(objFileStream);
                }
                catch
                {
                    try
                    {
                        formatter = new XmlSerializer(typeof(Song));
                        objFileStream.Seek(0, SeekOrigin.Begin);
                        objFileStream = SongManager.GetStream(objFileStream);
                        loadedInstruments = ((Song)formatter.Deserialize(objFileStream)).Instruments;
                    }
                    catch
                    {
                        loadedInstruments = null;
                    }
                }
                objFileStream.Close();
            }
            if (loadedInstruments != null)
            {
                foreach (Instrument i in loadedInstruments)
                {
                    if (i.ID != "R")
                    {
                        i.ID = currentSong.Instruments.GetSafeId().ToString();
                        currentSong.Instruments.Add(i);
                    }
                }
            }
        }

        public static void Export(Song currentSong)
        {
            Export(currentSong.Instruments);
        }

        public static void Export(Instrument instrument)
        {
            Export(new Instruments() { instrument });
        }

        public static void Export(Instruments instruments)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = WYZTracker.Core.Properties.Resources.INSFilter;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                XmlSerializer formatter = new XmlSerializer(typeof(Instruments));
                Stream objFileStream = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(objFileStream, instruments);
                objFileStream.Close();
            }
        }

        public static void AddNew(Song currentSong)
        {
            Instrument nuevo = new Instrument();
            nuevo.SetVolumeLength(5);
            nuevo.Name = WYZTracker.Core.Properties.Resources.New;
            nuevo.Looped = false;
            nuevo.Volumes = new byte[] { 8, 8, 8, 8, 8 };
            nuevo.ID = currentSong.Instruments.GetSafeId().ToString();
            currentSong.Instruments.Add(nuevo);
        }

        public static void Remove(Song currentSong, Instrument instrument)
        {
            currentSong.Instruments.Remove(instrument);
        }

    }
}
