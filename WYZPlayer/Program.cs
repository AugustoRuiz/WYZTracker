using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WYZTracker;

namespace WYZPlayer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Logger.FileName = "WYZPlayer.log";

            PlaybackStreamer.InitializeAudio();
            Application.Run(new frmMain());
            PlaybackStreamer.StopAudio();
        }
    }
}
