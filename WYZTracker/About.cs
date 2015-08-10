using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;

namespace WYZTracker
{
    public partial class About : Form
    {
        private Player songPlayer;

        public About()
        {
            InitializeComponent();
            initializePlayer();
            initializeScroller();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.DoubleBuffer, true);
        }

        private void initializeScroller()
        {
        }

        private void initializePlayer()
        {
            songPlayer = new Player();
            songPlayer.CurrentSong = loadSongFromResources();
            songPlayer.Play();
        }

        private Song loadSongFromResources()
        {
            Song result = null;

            using (MemoryStream songStream = new MemoryStream(Properties.Resources.OX))
            {
                result = SongManager.LoadSong(songStream);
            }
            return result;
        }

        private void cmdOk_Click(object sender, EventArgs e)
        {
            songPlayer.Stop();
            this.Close();
        }
    }
}
