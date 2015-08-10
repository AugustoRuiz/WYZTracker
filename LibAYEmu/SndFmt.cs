using System;

namespace LibAYEmu
{
	/// <summary>
	/// Summary description for SndFmt.
	/// </summary>
	public class SndFmt
	{
		private int freq;		/*< sound freq */
		private int channels;	/*< channels (1-mono, 2-stereo) */

		public SndFmt()
		{
		}

		/// <summary>
		/// Chip frequency
		/// </summary>
		public int Freq
		{
			get { return freq; }
			set 
			{
				if(value<50)
				{
					throw new ArgumentException("Output sound frequency must be higher than 50.");
				}
				freq = value; 
			}
		}

		/// <summary>
		/// Number of channels to use (1-mono, 2-stereo)
		/// </summary>
		public int Channels
		{
			get { return channels; }
			set 
			{
				if(!(value==1 || value==2))
				{
					throw new ArgumentException("Incorrect number of channels. Values supported are 1 (Mono) or 2 (Stereo).");
				}
				channels = value; 
			}
		}
	}
}
