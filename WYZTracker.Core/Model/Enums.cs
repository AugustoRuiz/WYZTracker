using System;
using System.Collections.Generic;
using System.Text;

namespace WYZTracker
{
    public enum Scales
    {
        MajorTriad,
        MinorTriad,
        AugmentedTriad,
        DiminishedTriad,
        Major,
        NaturalMinor,
        MelodicMinor,
        HarmonicMinor,
        Chromatic,
        WholeTone,
        PentatonicMajor1,
        PentatonicMajor2,
        PentatonicMinor,
        Mixolydian,
        Phrygian,
        Lydian,
        Dorian
    }

    public enum Envelopes
    {
        env_none = -1,
        env_0000 = 0,
        env_0100 = 4,
        env_1000 = 8,
        env_1001 = 9,
        env_1010 = 10,
        env_1011 = 11,
        env_1100 = 12,
        env_1101 = 13,
        env_1110 = 14,
        env_1111 = 15,
        env_continue = 0x01
    }
}
