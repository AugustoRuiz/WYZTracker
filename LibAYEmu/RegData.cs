using System;

namespace LibAYEmu
{
	/// <summary>
	/// Summary description for RegData.
	/// </summary>
	public class RegData
	{
		private int tone_a;		    /*< R0, R1 */
		private int tone_b;			/*< R2, R3 */	
		private int tone_c;			/*< R4, R5 */
		private int noise;			/*< R6 */
		private int R7_tone_a;		/*< R7 bit 0 */
		private int R7_tone_b;		/*< R7 bit 1 */
		private int R7_tone_c;		/*< R7 bit 2 */
		private int R7_noise_a;	/*< R7 bit 3 */
		private int R7_noise_b;	/*< R7 bit 4 */
		private int R7_noise_c;	/*< R7 bit 5 */
		private int vol_a;			/*< R8 bits 3-0 */
		private int vol_b;			/*< R9 bits 3-0 */
		private int vol_c;			/*< R10 bits 3-0 */
		private int env_a;			/*< R8 bit 4 */
		private int env_b;			/*< R9 bit 4 */
		private int env_c;			/*< R10 bit 4 */
		private int env_freq;		/*< R11, R12 */
		private int env_style;		/*< R13 */

		public RegData()
		{
		}

		/// <summary>
		/// Registers R0, R1
		/// </summary>
		public int ToneA
		{
			get { return tone_a; }
			set { tone_a = value; }
		}
		/// <summary>
		/// Registers R2, R3
		/// </summary>
		public int ToneB
		{
			get { return tone_b; }
			set { tone_b = value; }
		}
		/// <summary>
		/// Registers R4, R5
		/// </summary>
		public int ToneC
		{
			get { return tone_c; }
			set { tone_c = value; }
		}
		/// <summary>
		/// Register R6
		/// </summary>
		public int Noise
		{
			get { return noise; }
			set { noise = value; }
		}
		/// <summary>
		/// R7 bit 0
		/// </summary>
		public int R7ToneA
		{
			get { return R7_tone_a; }
			set { R7_tone_a = value; }
		}
		/// <summary>
		/// R7 bit 1
		/// </summary>
		public int R7ToneB
		{
			get { return R7_tone_b; }
			set { R7_tone_b = value; }
		}
		/// <summary>
		/// R7 bit 2
		/// </summary>
		public int R7ToneC
		{
			get { return R7_tone_c; }
			set { R7_tone_c = value; }
		}
		/// <summary>
		/// R7 bit 3
		/// </summary>
		public int R7NoiseA
		{
			get { return R7_noise_a; }
			set { R7_noise_a = value; }
		}
		/// <summary>
		/// R7 bit 4
		/// </summary>
		public int R7NoiseB
		{
			get { return R7_noise_b; }
			set { R7_noise_b = value; }
		}
		/// <summary>
		/// R7 bit 5
		/// </summary>
		public int R7NoiseC
		{
			get { return R7_noise_c; }
			set { R7_noise_c = value; }
		}
		/// <summary>
		/// R8 bits 3-0
		/// </summary>
		public int VolA
		{
			get { return vol_a; }
			set { vol_a = value; }
		}
		/// <summary>
		/// R9 bits 3-0
		/// </summary>
		public int VolB
		{
			get { return vol_b; }
			set { vol_b = value; }
		}
		/// <summary>
		/// R10 bits 3-0
		/// </summary>
		public int VolC
		{
			get { return vol_c; }
			set { vol_c = value; }
		}
		/// <summary>
		/// R8 bit 4
		/// </summary>
		public int EnvA
		{
			get { return env_a; }
			set { env_a = value; }
		}
		/// <summary>
		/// R9 bit 4
		/// </summary>
		public int EnvB
		{
			get { return env_b; }
			set { env_b = value; }
		}
		/// <summary>
		/// R10 bit 4
		/// </summary>
		public int EnvC
		{
			get { return env_c; }
			set { env_c = value; }
		}
		/// <summary>
		/// R11, R12
		/// </summary>
		public int EnvFreq
		{
			get { return env_freq; }
			set { env_freq = value; }
		}
		/// <summary>
		/// R13
		/// </summary>
		public int EnvStyle
		{
			get { return env_style; }
			set { env_style = value; }
		}
	}
}
