using System;
using System.Collections.Generic;
using System.Text;

namespace WYZTracker
{
    internal static class ApplicationState
    {
        static ApplicationState()
        {
            BaseOctave = 3;
            Stereo = LibAYEmu.Stereo.Mono;
            CurrentEnvData = new EnvelopeData();
        }

        public static int BaseOctave { get; set; }

        public static EnvelopeData CurrentEnvData { get; set; }

        public static Splash SplashScreen { get; set; }

        public static Song CurrentSong { get; set; }

        public static string FileName { get; set; }

        public static LibAYEmu.Stereo Stereo { get; set; }
    }
}
