using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace WYZTracker
{
    public class SongManager
    {
        public static void SaveSong(Song currentSong, string fileName)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(Song));

            Stream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
            GZipStream gzip = new GZipStream(fileStream, CompressionMode.Compress, true);

            formatter.Serialize(gzip, currentSong);
            gzip.Flush();
            gzip.Close();

            fileStream.Close();
            fileStream.Dispose();
        }

        public static Song LoadSong(string fileName)
        {
            Song result = null;
            using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                result = LoadSong(fileStream);
            }
            return result;
        }

        public static Song LoadSong(Stream stream)
        {
            Song result = null; 
            using (Stream objFileStream = SongManager.GetStream(stream))
            {
                XmlSerializer formatter = new XmlSerializer(typeof(Song));
                result = (Song)formatter.Deserialize(objFileStream);
            }

            // Fix para cargar instrumentos de archivos antiguos y que el tema no se 
            // fastidie.
            foreach (Instrument instrument in result.Instruments)
            {
                if (instrument.Volumes != null && instrument.PitchModifiers == null)
                {
                    instrument.SetVolumeLength(instrument.Volumes.Length);
                }
            }
            foreach (Effect effect in result.Effects)
            {
                if (effect.Volumes != null && effect.EnvTypes == null)
                {
                    effect.SetEffectLength(effect.Volumes.Length);
                }
            }

            if (result.Frequencies == null || result.Frequencies.Length == 0)
            {
                result.Frequencies = NoteFileLoader.LoadDefaultNotes();
            }

            if (result.ChipFrequency == 0)
            {
                result.ChipFrequency = (int) LibAYEmu.ChipSpeedsByMachine.MSX;
            }

            if (!result.DefaultCpcFreqs && !result.DefaultMsxFreqs && !result.CustomFreqs && !result.ParameterizedFreqs)
            {
                result.DefaultMsxFreqs = true;
                result.ParameterValue = result.ChipFrequency;
            }

            return result;
        }

        public static Stream GetStream(Stream fileStream)
        {
            Stream objFileStream = null;
            try
            {
                GZipStream gzip = new GZipStream(fileStream, CompressionMode.Decompress);

                MemoryStream memStream = new MemoryStream();

                byte[] bytes = new byte[1024];
                int bytesRead = -1;
                while (bytesRead != 0)
                {
                    bytesRead = gzip.Read(bytes, 0, 1024);
                    memStream.Write(bytes, 0, bytesRead);
                    memStream.Flush();
                }
                memStream.Position = 0;
                objFileStream = memStream;

                gzip.Close();
                gzip.Dispose();

                fileStream.Dispose();
            }
            catch (Exception)
            {
                fileStream.Position = 0;
                objFileStream = fileStream;
            }
            return objFileStream;
        }

    }
}
