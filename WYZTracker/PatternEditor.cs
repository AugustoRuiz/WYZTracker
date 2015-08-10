using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WYZTracker
{
    public partial class PatternEditor : UserControl
    {
        public event EventHandler<PlayEventArgs> Play;
        public event EventHandler Stop;
        public event EventHandler IncreaseOctave;
        public event EventHandler DecreaseOctave;
        public event EventHandler IncreaseInstrument;
        public event EventHandler DecreaseInstrument;
        public event EventHandler EditionIncrementChanged;
        public event EventHandler HighlightRangeChanged;
        public event EventHandler NextPattern;
        public event EventHandler PreviousPattern;
        public event EventHandler<FileDroppedEventArgs> FileDropped;
        public event EventHandler<FxChannelChangedEventArgs> FxChannelChanged;
        public event EventHandler<ActiveFxEventArgs> SetActiveFx;
        public event EventHandler<ActiveInstrumentEventArgs> SetActiveInstrument;

        public PatternEditor()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private Song currentSong;
        private Pattern currentPattern;
        private Instrument currentInstrument;
        private byte currentChannel;

        private int marginWidth;

        public int SelectedIndex
        {
            get
            {
                return this.patView.SelectedIndex;
            }
            set
            {
                this.patView.SelectedIndex = value;
            }
        }

        public Pattern CurrentPattern
        {
            get
            {
                return this.currentPattern;
            }
            set
            {
                this.currentPattern = value;
                this.patView.CurrentPattern = value;
            }
        }

        public Song CurrentSong
        {
            get
            {
                return currentSong;
            }
            set
            {
                this.currentSong = value;
                this.patView.CurrentSong = value;
                this.UpdateChannels();
            }
        }

        public Instrument CurrentInstrument
        {
            get
            {
                return currentInstrument;
            }
            set
            {
                this.currentInstrument = value;
                this.patView.CurrentInstrument = value;
            }
        }

        public byte CurrentChannel
        {
            get
            {
                return this.currentChannel;
            }
            set
            {
                this.currentChannel = value;
            }
        }

        public int EditionIncrement
        {
            get { return this.patView.EditionIncrement; }
            set { this.patView.EditionIncrement = value; }
        }

        public int DelayDecrement
        {
            get { return this.patView.DelayDecrement; }
            set { this.patView.DelayDecrement = value; }
        }

        public int HighlightRange
        {
            get
            {
                return this.patView.HighlightRange;
            }
            set
            {
                this.patView.HighlightRange = value;
            }
        }

        public void Cut()
        {
            this.patView.OnCut();
        }

        public void Copy()
        {
            this.patView.OnCopy();
        }

        public void Paste()
        {
            this.patView.OnPaste();
        }

        public void PasteAsDelay()
        {
            this.patView.OnPasteAsDelay();
        }

        public void Transpose(int semitones)
        {
            this.patView.Transpose(semitones);
        }

        #region Event launchers

        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            base.OnInvalidated(e);
            this.patView.Invalidate(e.InvalidRect, true);
        }

        protected void OnFileDropped(object sender, FileDroppedEventArgs eventArgs)
        {
            EventHandler<FileDroppedEventArgs> temp = FileDropped;
            if (temp != null)
                temp(sender, eventArgs);
        }

        protected void OnPlay(object sender, PlayEventArgs eventArgs)
        {
            EventHandler<PlayEventArgs> temp = Play;
            if (temp != null)
                temp(sender, eventArgs);
        }

        protected void OnStop(object sender, EventArgs eventArgs)
        {
            EventHandler temp = Stop;
            if (temp != null)
                temp(sender, eventArgs);
        }

        protected void OnDecreaseOctave(object sender, EventArgs eventArgs)
        {
            EventHandler temp = DecreaseOctave;
            if (temp != null)
                temp(sender, eventArgs);
        }

        protected void OnIncreaseOctave(object sender, EventArgs eventArgs)
        {
            EventHandler temp = IncreaseOctave;
            if (temp != null)
                temp(sender, eventArgs);
        }

        protected void OnEditionIncrementChanged(object sender, EventArgs eventArgs)
        {
            EventHandler temp = EditionIncrementChanged;
            if (temp != null)
                temp(sender, eventArgs);
        }

        protected void OnHighlightRangeChanged(object sender, EventArgs eventArgs)
        {
            EventHandler temp = HighlightRangeChanged;
            if (temp != null)
                temp(sender, eventArgs);
        }

        protected void OnDecreaseInstrument(object sender, EventArgs eventArgs)
        {
            EventHandler temp = DecreaseInstrument;
            if (temp != null)
                temp(sender, eventArgs);
        }

        protected void OnIncreaseInstrument(object sender, EventArgs eventArgs)
        {
            EventHandler temp = IncreaseInstrument;
            if (temp != null)
                temp(sender, eventArgs);
        }

        protected void OnNextPattern(object sender, EventArgs eventArgs)
        {
            EventHandler temp = NextPattern;
            if (temp != null)
                temp(sender, eventArgs);
        }

        protected void OnPreviousPattern(object sender, EventArgs eventArgs)
        {
            EventHandler temp = PreviousPattern;
            if (temp != null)
                temp(sender, eventArgs);
        }

        protected void OnSetActiveFx(object sender, ActiveFxEventArgs eventArgs)
        {
            EventHandler<ActiveFxEventArgs> temp = SetActiveFx;
            if (temp != null)
                temp(sender, eventArgs);
        }

        protected void OnSetActiveInstrument(object sender, ActiveInstrumentEventArgs eventArgs)
        {
            EventHandler<ActiveInstrumentEventArgs> temp = SetActiveInstrument;
            if (temp != null)
                temp(sender, eventArgs);
        }

        #endregion

        #region PatternView Effect Handlers

        private void patView_Play(object sender, PlayEventArgs e)
        {
            this.OnPlay(sender, e);
        }

        private void patView_Stop(object sender, EventArgs e)
        {
            this.OnStop(sender, e);
        }

        private void patView_IncreaseOctave(object sender, EventArgs e)
        {
            this.OnIncreaseOctave(sender, e);
        }

        private void patView_DecreaseOctave(object sender, EventArgs e)
        {
            this.OnDecreaseOctave(sender, e);
        }

        private void patView_EditionIncrementChanged(object sender, EventArgs e)
        {
            this.OnEditionIncrementChanged(sender, e);
        }

        private void patView_HighlightRangeChanged(object sender, EventArgs e)
        {
            this.OnHighlightRangeChanged(sender, e);
        }

        private void patView_IncreaseInstrument(object sender, EventArgs e)
        {
            this.OnIncreaseInstrument(sender, e);
        }

        private void patView_DecreaseInstrument(object sender, EventArgs e)
        {
            this.OnDecreaseInstrument(sender, e);
        }

        private void patView_NextPattern(object sender, EventArgs e)
        {
            this.OnNextPattern(sender, e);
        }

        private void patView_PreviousPattern(object sender, EventArgs e)
        {
            this.OnPreviousPattern(sender, e);
        }

        private void patView_SetActiveInstrument(object sender, ActiveInstrumentEventArgs e)
        {
            this.OnSetActiveInstrument(sender, e);
        }

        private void patView_SetActiveFx(object sender, ActiveFxEventArgs e)
        {
            this.OnSetActiveFx(sender, e);
        }

        #endregion

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            this.patView.Focus();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (this.currentSong != null)
            {
                if (e.X >= marginWidth && e.X < (marginWidth + ((this.currentSong.Channels + 1) * Properties.Settings.Default.ColumnWidth)) && e.Y >= 0 && e.Y < 26)
                {
                    int channel = (e.X - marginWidth) / Properties.Settings.Default.ColumnWidth;
                    if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
                    {
                        this.currentSong.MutedChannels[channel] = !this.currentSong.MutedChannels[channel];
                        this.Invalidate();
                    }
                    if ((e.Button & MouseButtons.Right) == MouseButtons.Right)
                    {
                        this.currentSong.FxChannel = channel;
                        cboFxChannel.SelectedItem = this.currentSong.FxChannel;
                    }
                }
            }
            base.OnMouseUp(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            this.Invalidate();
            base.OnSizeChanged(e);
        }

        public void UpdateChannels()
        {
            if (this.currentSong != null)
            {
                int? oldSelection = (int?)cboFxChannel.SelectedItem;
                this.cboFxChannel.Items.Clear();
                for (int i = 0; i < this.currentSong.Channels; i++)
                {
                    cboFxChannel.Items.Add(i);
                }
                cboFxChannel.SelectedItem = this.currentSong.FxChannel;
            }
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            patView.UpdateFont();
            patView.Invalidate();
            marginWidth = (int) e.Graphics.MeasureString("9999:", patView.Font).Width;

            int fxWidth = (int)e.Graphics.MeasureString(WYZTracker.Properties.Resources.FX + "M", this.Font).Width;
            Rectangle rect = new Rectangle(0, 0, this.ClientRectangle.Width, 26);
            rect.Height -= 1;
            rect.Width -= 1;

            e.Graphics.FillRectangle(Brushes.Black, rect);
            e.Graphics.DrawRectangle(Pens.Lime, rect);

            if (this.currentSong != null)
            {
                this.cboFxChannel.Left = marginWidth + ((this.currentSong.Channels) * Properties.Settings.Default.ColumnWidth) + fxWidth;

                int currentX = marginWidth;
                for (int i = 0; i < this.currentSong.Channels; i++)
                {
                    e.Graphics.DrawString(
                        string.Format(WYZTracker.Properties.Resources.ChannelWithNumber, i), 
                        this.Font, 
                        this.currentSong.MutedChannels[i] ? Brushes.Red : Brushes.Lime, 
                        currentX, 
                        6);
                    currentX += Properties.Settings.Default.ColumnWidth;
                }
                e.Graphics.DrawString(WYZTracker.Properties.Resources.FX, this.Font, this.currentSong.MutedChannels[this.currentSong.Channels] ? Brushes.Red : Brushes.Lime, currentX, 6);
            }
        }

        private void patView_DragEnter(object sender, DragEventArgs e)
        {
            // make sure they're actually dropping files (not text or anything else)
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
                // allow them to continue
                // (without this, the cursor stays a "NO" symbol
                e.Effect = DragDropEffects.All;
        }

        private void patView_DragDrop(object sender, DragEventArgs e)
        {
            // transfer the filenames to a string array
            // (yes, everything to the left of the "=" can be put in the 
            // foreach loop in place of "files", but this is easier to understand.)
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            // loop through the string array, adding each filename to the ListBox
            foreach (string file in files)
            {
                OnFileDropped(this, new FileDroppedEventArgs(file));
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            ToolStripMenuItem parent = establecerInstrumentoToolStripMenuItem;
            foreach (Instrument i in this.currentSong.Instruments)
            {
                ToolStripMenuItem childItem = new ToolStripMenuItem(i.Name);
                childItem.Tag = i;
                childItem.Click += new EventHandler(setInstrument);
                establecerInstrumentoToolStripMenuItem.DropDownItems.Add(childItem);
            }
        }

        private void setInstrument(object sender, EventArgs e)
        {
            ToolStripMenuItem source = sender as ToolStripMenuItem;
            if (source != null)
            {
                this.patView.SetInstrumentToSelection(((Instrument)source.Tag).ID);
            }
        }

        // +1
        private void transponer1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.patView.Transpose(1);
        }

        // -1
        private void transponer1ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.patView.Transpose(-1);
        }

        // +1
        private void octava1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.patView.IncreaseOctaveToSelection();
        }

        // -1
        private void octava1ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.patView.DecreaseOctaveToSelection();
        }

        private void tempoPlusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.patView.IncreaseTempoToSelection();
        }

        private void tempoMinusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.patView.DecreaseTempoToSelection();
        }

        private void cboFxChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.OnFxChannelChanged(new FxChannelChangedEventArgs((int)this.cboFxChannel.SelectedItem));
        }

        private void OnFxChannelChanged(FxChannelChangedEventArgs evArgs)
        {
            EventHandler<FxChannelChangedEventArgs> tmp = this.FxChannelChanged;
            if (tmp != null)
            {
                tmp(this, evArgs);
            }
        }

        private void transposeTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter)
            {
                transposeByNumber();
                e.Handled = true;
            }
        }

        private void transposeByNumber()
        {
            int offset;
            if (int.TryParse(transposeTextBox.Text, out offset))
            {
                this.patView.Transpose(offset);
            }
            else
            {
                MessageBox.Show(WYZTracker.Properties.Resources.TransposeError,
                    WYZTracker.Properties.Resources.Error, 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            contextMenuPatView.Close();
        }

        private void contextMenuPatView_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            System.Threading.Thread cleanerThread = new System.Threading.Thread(
                new System.Threading.ParameterizedThreadStart(
                    new Action<object>((s) =>
                                        {
                                            System.Threading.Thread.Sleep(200);
                                            this.Invoke(new Action<object>(limpiarHijosDinamicos), 0);
                                        }
                                      )
                                  )
                              );
            cleanerThread.IsBackground = true;
            cleanerThread.Start();
        }

        private void limpiarHijosDinamicos(object state)
        {
            foreach (ToolStripMenuItem child in establecerInstrumentoToolStripMenuItem.DropDownItems)
            {
                child.Click -= new EventHandler(setInstrument);
            }
            establecerInstrumentoToolStripMenuItem.DropDownItems.Clear();
        }
    }
}