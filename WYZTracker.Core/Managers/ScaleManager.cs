using System;
using System.Collections.Generic;
using System.Text;

namespace WYZTracker
{
    public class ScaleManager
    {
        private static Dictionary<Scales, int[]> offsetsByScale;

        public static Dictionary<Scales, int[]> OffsetsByScale
        {
            get
            {
                return offsetsByScale;
            }
        }

        static ScaleManager()
        {
            initializeOffsetsByScale();
        }

        private static void initializeOffsetsByScale()
        {
            offsetsByScale = new Dictionary<Scales, int[]>();
            offsetsByScale.Add(Scales.MajorTriad, new int[] { 0, 4, 7 });
            offsetsByScale.Add(Scales.MinorTriad, new int[] { 0, 3, 7 });
            offsetsByScale.Add(Scales.DiminishedTriad, new int[] { 0, 3, 6 });
            offsetsByScale.Add(Scales.AugmentedTriad, new int[] { 0, 4, 8 });
            offsetsByScale.Add(Scales.Major, new int[] { 0, 2, 4, 5, 7, 9, 11, 12 });
            offsetsByScale.Add(Scales.NaturalMinor, new int[] { 0, 2, 3, 5, 7, 8, 10, 12 });
            offsetsByScale.Add(Scales.MelodicMinor, new int[] { 0, 2, 3, 5, 7, 9, 11, 12 });
            offsetsByScale.Add(Scales.HarmonicMinor, new int[] { 0, 2, 3, 5, 7, 8, 11, 12 });
            offsetsByScale.Add(Scales.Chromatic, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 });
            offsetsByScale.Add(Scales.WholeTone, new int[] { 0, 2, 4, 6, 8, 10, 12 });
            offsetsByScale.Add(Scales.PentatonicMajor1, new int[] { 0, 2, 5, 7, 9, 12 });
            offsetsByScale.Add(Scales.PentatonicMajor2, new int[] { 0, 2, 4, 7, 9, 12 });
            offsetsByScale.Add(Scales.PentatonicMinor, new int[] { 0, 3, 5, 7, 10, 12 });
            offsetsByScale.Add(Scales.Mixolydian, new int[] { 0, 2, 4, 5, 7, 9, 10, 12 });
            offsetsByScale.Add(Scales.Lydian, new int[] { 0, 2, 4, 6, 7, 9, 11, 12 });
            offsetsByScale.Add(Scales.Phrygian, new int[] { 0, 1, 3, 5, 7, 8, 10, 12 });
            offsetsByScale.Add(Scales.Dorian, new int[] { 0, 2, 3, 5, 7, 9, 10, 12 });
        }
    }
}
