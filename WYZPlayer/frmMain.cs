using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WYZTracker;

namespace WYZPlayer
{
    public partial class frmMain : Form
    {
        private Player _songPlayer;

        public frmMain()
        {
            InitializeComponent();
            initialize();
        }

        private void initialize()
        {
            this._songPlayer = new Player();
            this._songPlayer.SongFinished += new EventHandler(_songPlayer_SongFinished);
            numLoops.Value = WYZPlayer.Properties.Settings.Default.LoopedSongsRepeat;
            loadSongs();
            this.Size = WYZPlayer.Properties.Settings.Default.WindowSize;
        }

        private void loadSongs()
        {
            System.Collections.Specialized.StringCollection lastSongs = WYZPlayer.Properties.Settings.Default.LastPlayedSongs;
            if (lastSongs != null)
            {
                foreach (string songPath in lastSongs)
                {
                    loadSong(songPath);
                }
            }
            PlaylistItemBindingSource.CurrentChanged += new EventHandler(PlaylistItemBindingSource_CurrentChanged);
            PlaylistItemBindingSource.MoveFirst();
        }

        private void loadSong(string songPath)
        {
            try
            {
                PlaylistItem item = new PlaylistItem();
                item.FilePath = songPath;
                item.Song = SongManager.LoadSong(songPath);
                PlaylistItemBindingSource.Add(item);

                if (PlaylistItemBindingSource.Current == null)
                {
                    PlaylistItemBindingSource.MoveFirst();
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ex.ToString());
            }
        }

        private void pbPrevious_MouseEnter(object sender, EventArgs e)
        {
            pbPrevious.Image = Properties.Resources.control_start_blue;
        }

        private void pbPrevious_MouseLeave(object sender, EventArgs e)
        {
            pbPrevious.Image = Properties.Resources.control_start;
        }

        private void pbStop_MouseEnter(object sender, EventArgs e)
        {
            pbStop.Image = Properties.Resources.control_stop_blue;
        }

        private void pbStop_MouseLeave(object sender, EventArgs e)
        {
            pbStop.Image = Properties.Resources.control_stop;
        }

        private void pbPlay_MouseEnter(object sender, EventArgs e)
        {
            if (this._songPlayer.Status == PlayStatus.Playing)
            {
                pbPlay.Image = Properties.Resources.control_pause_blue;
            }
            else
            {
                pbPlay.Image = Properties.Resources.control_play_blue;
            }
        }

        private void pbPlay_MouseLeave(object sender, EventArgs e)
        {
            if (this._songPlayer.Status == PlayStatus.Playing)
            {
                pbPlay.Image = Properties.Resources.control_pause;
            }
            else
            {
                pbPlay.Image = Properties.Resources.control_play;
            }
        }

        private void pbNext_MouseEnter(object sender, EventArgs e)
        {
            pbNext.Image = Properties.Resources.control_end_blue;
        }

        private void pbNext_MouseLeave(object sender, EventArgs e)
        {
            pbNext.Image = Properties.Resources.control_end;
        }

        private void pbPrevious_Click(object sender, EventArgs e)
        {
            playPreviousSong();
        }

        private void pbStop_Click(object sender, EventArgs e)
        {
            stop();
        }

        private void pbPlay_Click(object sender, EventArgs e)
        {
            playOrPause();
        }

        private void pbNext_Click(object sender, EventArgs e)
        {
            playNextSong();
        }

        private void _songPlayer_SongFinished(object sender, EventArgs e)
        {
            playNextSong();
            this._songPlayer.Play();
        }

        private void pbOpen_Click(object sender, EventArgs e)
        {
            this.ofd.Filter = WYZPlayer.Properties.Resources.WYZFilter;
            if (this.ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in this.ofd.FileNames)
                {
                    loadSong(file);
                }
            }
            saveLastPlayedSongs();
        }

        private void stop()
        {
            this._songPlayer.Stop();
        }

        private void playOrPause()
        {
            if (this._songPlayer.Status == PlayStatus.Playing)
            {
                this._songPlayer.Pause();
                pbPlay.Image = WYZPlayer.Properties.Resources.control_play;
            }
            else
            {
                if (PlaylistItemBindingSource.Count > 0)
                {
                    if (PlaylistItemBindingSource.Current == null)
                    {
                        PlaylistItemBindingSource.MoveFirst();
                    }
                    pbPlay.Image = WYZPlayer.Properties.Resources.control_pause;
                    this._songPlayer.PlayMode = PlayMode.FullSong;
                    if (this._songPlayer.CurrentSong == null)
                    {
                        PlaylistItem item = (PlaylistItem)PlaylistItemBindingSource.Current;
                        if (item != null)
                        {
                            this._songPlayer.CurrentSong = item.Song;
                        }
                    }
                    if (this._songPlayer.CurrentSong != null)
                    {
                        this._songPlayer.Play();
                    }
                }
            }
        }

        private void playPreviousSong()
        {
            if (this.InvokeRequired)
            {
                MethodInvoker method = playPreviousSong;
                this.Invoke(method);
            }
            else
            {
                if (this.PlaylistItemBindingSource.Position == 0)
                {
                    this.PlaylistItemBindingSource.MoveLast();
                }
                else
                {
                    this.PlaylistItemBindingSource.MovePrevious();
                }
            }
        }

        private void playNextSong()
        {
            if (this.InvokeRequired)
            {
                MethodInvoker method = playNextSong;
                this.Invoke(method);
            }
            else
            {
                if (this.PlaylistItemBindingSource.Position == this.PlaylistItemBindingSource.Count - 1)
                {
                    this.PlaylistItemBindingSource.MoveFirst();
                }
                else
                {
                    this.PlaylistItemBindingSource.MoveNext();
                }
            }
        }

        private void PlaylistItemBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            PlaylistItem item = (PlaylistItem) PlaylistItemBindingSource.Current;
            if (item != null)
            {
                this._songPlayer.CurrentSong = item.Song;
            }
            else
            {
                this.PlaylistItemBindingSource.MoveFirst();
                this._songPlayer.Stop();
            }
            this.lstSongs.Invalidate();
        }

        private void pbRemove_Click(object sender, EventArgs e)
        {
            PlaylistItem item = (PlaylistItem)PlaylistItemBindingSource.Current;
            if (item != null)
            {
                PlaylistItemBindingSource.Remove(item);
                this.lstSongs.Invalidate();
            }
            saveLastPlayedSongs();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveLastPlayedSongs();
        }

        private void saveLastPlayedSongs()
        {
            if (WYZPlayer.Properties.Settings.Default.LastPlayedSongs == null)
            {
                WYZPlayer.Properties.Settings.Default.LastPlayedSongs = new System.Collections.Specialized.StringCollection();
            }

            WYZPlayer.Properties.Settings.Default.LastPlayedSongs.Clear();
            foreach (PlaylistItem item in PlaylistItemBindingSource.List)
            {
                WYZPlayer.Properties.Settings.Default.LastPlayedSongs.Add(item.FilePath);
            }
            WYZPlayer.Properties.Settings.Default.Save();
        }

        private void numLoops_ValueChanged(object sender, EventArgs e)
        {
            WYZPlayer.Properties.Settings.Default.LoopedSongsRepeat = (int)numLoops.Value;
            WYZPlayer.Properties.Settings.Default.Save();
            this._songPlayer.LimitLoops = WYZPlayer.Properties.Settings.Default.LoopedSongsRepeat;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            WYZPlayer.Properties.Settings.Default.WindowSize = this.Size;
            WYZPlayer.Properties.Settings.Default.Save();
        }

        private void tbVolume_ValueChanged(object sender, EventArgs e)
        {
            this._songPlayer.Volume = ((double)tbVolume.Value) / ((double)tbVolume.Maximum);
        }
    }
}
