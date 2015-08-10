using System;
using System.Collections.Generic;
using System.Text;

namespace WYZTracker
{
    public class ChannelLine
    {
        public ChannelLine()
            : this(0)
        {
        }

        public ChannelLine(byte channelCount)
        {
            this.lineNotes = new List<ChannelNote>();
            this.Channels = channelCount;
            this.Fx = int.MinValue;
        }

        private List<ChannelNote> lineNotes;

        public List<ChannelNote> Notes
        {
            get
            {
                return lineNotes;
            }
            set
            {
                this.lineNotes = value;
            }
        }

        private int fx;

        public int Fx
        {
            get { return fx; }
            set { fx = value; }
        }

        private byte channels;

        public byte Channels
        {
            get
            {
                return this.channels;
            }
            set
            {
                byte oldChannelCount = (byte)this.lineNotes.Count;
                byte minChannelCount = (oldChannelCount < value) ? oldChannelCount : value;

                List<ChannelNote> newLineNotes = new List<ChannelNote>();

                for (byte i = 0; i < minChannelCount; i++)
                {
                    newLineNotes.Add(lineNotes[i]);
                }
                for (byte nextOnes = minChannelCount; nextOnes < value; nextOnes++)
                {
                    newLineNotes.Add(new ChannelNote());
                }
                lineNotes = newLineNotes;

                this.channels = value;
            }
        }

        public sbyte TempoModifier { get; set; }

        //public string GetLineText(Song currentSong)
        //{
        //    StringBuilder sbResult = new StringBuilder();
        //    foreach (ChannelNote n in this.Notes)
        //    {
        //        sbResult.AppendFormat("{0}   ", n.GetONSI(currentSong));
        //    }
        //    if (this.Fx == int.MinValue)
        //    {
        //        sbResult.Append("..");
        //    }
        //    else
        //    {
        //        sbResult.Append(Fx.ToString("X2"));
        //    }
        //    return sbResult.ToString();
        //}
    }
}
