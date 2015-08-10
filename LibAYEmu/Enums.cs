using System;

namespace LibAYEmu
{
    public enum ChipSpeedsByMachine
    {
        MSX = 1774000, // 1773400,
        CPC = 1000000
    }

	public enum Stereo
	{
		Mono = 0,
		ABC,
		ACB,
		BAC,
		BCA,
		CAB,
		CBA,
		StereoCustom = 255
	}

	public enum Chip
	{
		AY,			/*< default AY chip (lion17 for now) */
		YM,			/*< default YM chip (lion17 for now) */
		AY_Lion17,		/*< emulate AY with Lion17 table */
		YM_Lion17,		/*< emulate YM with Lion17 table */
		AY_Kay,			/*< emulate AY with HACKER KAY table */
		YM_Kay,			/*< emulate YM with HACKER KAY table */
		AY_Custom,		/*< use AY with custom table. */
		YM_Custom		/*< use YM with custom table. */
	}

    public enum ChannelControl
    {
        NONE = 0,
        TONE_A = 1,
        TONE_B = 2,
        TONE_C = 4,
        NOISE_A = 8,
        NOISE_B = 16,
        NOISE_C = 32
    }


}
