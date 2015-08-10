using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WYZTracker
{
    public partial class SongProperties : UserControl, ISongConsumer
    {
        public event EventHandler<SongChannelsChangedEventArgs> SongChannelsChanged;
        public event EventHandler<SongFrequencyChangedEventArgs> SongFrequencyChanged;

        public SongProperties()
        {
            InitializeComponent();
            this.loadComboFrequencies();
        }

        private Song currentSong;

        public Song CurrentSong
        {
            get
            {
                return currentSong;
            }
            set
            {
                this.currentSong = value;
                this.bindSong();
            }
        }

        private Pattern currentPattern;

        public Pattern CurrentPattern
        {
            get { return currentPattern; }
            set
            {
                currentPattern = value;
            }
        }

        private int currentTempo;

        public int CurrentTempo
        {
            set
            {
                currentTempo = value;
                this.lblCurrentTempo.Text = value.ToString();
                updateCurrentTempoColor();
            }
        }

        private void updateCurrentTempoColor()
        {
            if (this.currentSong != null && this.currentTempo != this.currentSong.Tempo)
            {
                this.lblCurrentTempo.ForeColor = Color.Red;
            }
            else
            {
                this.lblCurrentTempo.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
            }
        }

        private void bindSong()
        {
            if (this.currentSong != null)
            {
                this.cboChannels.SelectedItem = this.currentSong.Channels.ToString();
                this.songBindingSource.DataSource = this.currentSong;
                this.cboFrequency.SelectedValue = this.currentSong.ChipFrequency;
            }
            updateCurrentTempoColor();
        }

        private void cboChannels_SelectedIndexChanged(object sender, EventArgs e)
        {
            byte oldChannelCount = currentSong.Channels;
            byte newChannelCount;

            if (byte.TryParse(this.cboChannels.Text, out newChannelCount))
            {
                EventHandler<SongChannelsChangedEventArgs> tmp = this.SongChannelsChanged;
                if (tmp != null)
                {
                    tmp(this, new SongChannelsChangedEventArgs(oldChannelCount, newChannelCount));
                }
            }
        }

        protected virtual void OnSongFrequencyChanged()
        {
            EventHandler<SongFrequencyChangedEventArgs> tmp = this.SongFrequencyChanged;
            if (tmp != null)
            {
                tmp(this, new SongFrequencyChangedEventArgs());
            }
        }

        private void loadComboFrequencies()
        {
            List<KeyValuePair<string, int>> items = new List<KeyValuePair<string, int>>();

            foreach (LibAYEmu.ChipSpeedsByMachine enumItem in Enum.GetValues(typeof(LibAYEmu.ChipSpeedsByMachine)))
            {
                KeyValuePair<string, int> item =
                    new KeyValuePair<string, int>(Properties.Resources.ResourceManager.GetString(enumItem.ToString()), (int)enumItem);
                items.Add(item);
            }

            cboFrequency.DataSource = items;
            cboFrequency.DisplayMember = "Key";
            cboFrequency.ValueMember = "Value";
        }

        private void numLoopTo_ValueChanged(object sender, EventArgs e)
        {
            if (this.CurrentSong != null)
            {
                if (numLoopTo.Value >= this.CurrentSong.PlayOrder.Count)
                {
                    numLoopTo.Value = this.CurrentSong.PlayOrder.Count - 1;
                }
            }
        }

    }

    public class SongChannelsChangedEventArgs : EventArgs
    {
        byte oldChannelCount;
        byte newChannelCount;

        public SongChannelsChangedEventArgs(byte oldCount, byte newCount)
        {
            oldChannelCount = oldCount;
            newChannelCount = newCount;
        }

        public byte OldChannelCount
        {
            get
            {
                return oldChannelCount;
            }
        }

        public byte NewChannelCount
        {
            get
            {
                return newChannelCount;
            }
        }
    }

    public class SongFrequencyChangedEventArgs : EventArgs
    {
        public SongFrequencyChangedEventArgs()
        {
        }
    }
}