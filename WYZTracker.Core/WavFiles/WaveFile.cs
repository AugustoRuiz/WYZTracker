﻿/* Wave File Format
Reference : http://ccrma.stanford.edu/CCRMA/Courses/422/projects/WaveFormat/
The canonical WAVE format starts with the RIFF header:
0         4   ChunkID          Contains the letters "RIFF" in ASCII form
                               (0x52494646 big-endian form).
4         4   ChunkSize        36 + SubChunk2Size, or more precisely:
                               4 + (8 + SubChunk1Size) + (8 + SubChunk2Size)
                               This is the size of the rest of the chunk 
                               following this number.  This is the size of the 
                               entire file in bytes minus 8 bytes for the
                               two fields not included in this count:
                               ChunkID and ChunkSize.
8         4   Format           Contains the letters "WAVE"
                               (0x57415645 big-endian form).

The "WAVE" format consists of two subchunks: "fmt " and "data":
The "fmt " subchunk describes the sound data's format:
12        4   Subchunk1ID      Contains the letters "fmt "
                               (0x666d7420 big-endian form).
16        4   Subchunk1Size    16 for PCM.  This is the size of the
                               rest of the Subchunk which follows this number.
20        2   AudioFormat      PCM = 1 (instrument.instr. Linear quantization)
                               Values other than 1 indicate some 
                               form of compression.
22        2   NumChannels      Mono = 1, Stereo = 2, etc.
24        4   SampleRate       8000, 44100, etc.
28        4   ByteRate         == SampleRate * NumChannels * BitsPerSample/8
32        2   BlockAlign       == NumChannels * BitsPerSample/8
                               The number of bytes for one sample including
                               all channels. I wonder what happens when
                               this number isn't an integer?
34        2   BitsPerSample    8 bits = 8, 16 bits = 16, etc.
          2   ExtraParamSize   if PCM, then doesn't exist
          X   ExtraParams      space for extra parameters

The "data" subchunk contains the size of the data and the actual sound:
36        4   Subchunk2ID      Contains the letters "data"
                               (0x64617461 big-endian form).
40        4   Subchunk2Size    == NumSamples * NumChannels * BitsPerSample/8
                               This is the number of bytes in the data.
                               You can also think of this as the size
                               of the read of the subchunk following this 
                               number.
44        *   Data             The actual sound data.
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WYZTracker
{
    public class WaveFile
    {
        private WaveHeader header;
        private WavefmtSubChunk fmt;
        private FileStream file;
        private Int32 dataSizeOffset;
        private Int32 dataByteCount;

        public WaveFile(int channels, int bitsPerSample, int sampleRate, string fileName)
        {
            header = new WaveHeader();
            fmt = new WavefmtSubChunk(channels, bitsPerSample, sampleRate);

            this.checkDirectoryExists(fileName);

            file = new FileStream(fileName, FileMode.Create, FileAccess.Write);

            header.WriteHeader(file);
            fmt.Writefmt(file);

            byte[] subchunk2ID = Encoding.ASCII.GetBytes("data");
            file.Write(subchunk2ID, 0, subchunk2ID.Length);

            dataSizeOffset = (Int32) file.Position;

            //Chunk Size
            byte[] subChunk2Size = BitConverter.GetBytes((Int32)0);
            file.Write(subChunk2Size, 0, subChunk2Size.Length);

            dataByteCount = 0;
        }

        private void checkDirectoryExists(string fileName)
        {
            string dir = Path.GetDirectoryName(fileName);

            if (!Directory.Exists(dir))
            {
                checkDirectoryExists(dir);
                Directory.CreateDirectory(dir);
            }
        }

        public void AppendData(Byte[] data)
        {
            int byteCount = data.Length;
            file.Write(data, 0, byteCount);
            dataByteCount += byteCount;
        }

        public void Close()
        {
            // Update chunk size.
            file.Seek(4, SeekOrigin.Begin);
            byte[] chunkSize = BitConverter.GetBytes((Int32)(36 + dataByteCount));
            file.Write(chunkSize, 0, chunkSize.Length);

            // Update subchunk size.
            file.Seek(dataSizeOffset, SeekOrigin.Begin);
            byte[] subChunk2Size = BitConverter.GetBytes(dataByteCount);
            file.Write(subChunk2Size, 0, subChunk2Size.Length);
            file.Close();
            file.Dispose();
        }

        public int NumChannels { get { return fmt.NumChannels; }}
        public int BitsPerSample { get { return fmt.BitsPerSample; }}
    }
}
