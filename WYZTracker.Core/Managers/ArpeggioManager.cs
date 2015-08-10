using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace WYZTracker
{
    public class ArpeggioManager
    {
        public static void Save(ArpeggioDefinition arpeggio, string fileName)
        {
            Stream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
            GZipStream gzip = new GZipStream(fileStream, CompressionMode.Compress, true);
            
            SerializationUtils.Serialize(arpeggio, gzip);

            gzip.Flush();
            gzip.Close();
            fileStream.Close();
            fileStream.Dispose();
        }

        public static ArpeggioDefinition Load(string fileName)
        {
            Stream objFileStream;
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
            objFileStream = SongManager.GetStream(fileStream);

            ArpeggioDefinition result = SerializationUtils.Deserialize<ArpeggioDefinition>(objFileStream);

            objFileStream.Close();
            objFileStream.Dispose();

            return result;
        }
    }
}
