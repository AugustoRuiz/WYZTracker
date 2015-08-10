using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace WYZTracker
{
    public class NoteFileLoader
    {
        public static Int16[] LoadDefaultNotes()
        {
            return LoadNotes(System.IO.Path.Combine(Application.StartupPath, "NOTAS.DAT"));
        }

        public static Int16[] LoadNotes(string filePath)
        {
            FileStream stream = new FileStream(filePath, FileMode.Open);
            BinaryReader reader = new BinaryReader(stream);
            int noteCount = (int) (stream.Length / sizeof(UInt16));

            Int16[] result = new Int16[noteCount];

            for(int currentNote = 0; currentNote < noteCount; currentNote++)
            {
                result[currentNote] = reader.ReadInt16();
            }
            reader.Close();
            return result;
        }

        public static void SaveNotes(string filePath, Int16[] frequencies)
        {
            FileStream stream = new FileStream(filePath, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(stream);
            int noteCount = frequencies.Length;

            for (int currentNote = 0; currentNote < noteCount; currentNote++)
            {
                writer.Write((Int16)frequencies[currentNote]);
            }
            writer.Close();
        }
    }
}
