using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace WYZTracker
{
    public partial class ArpeggioEditor : Form, ISongConsumer
    {
        private const int STEP_WIDTH = 20;
        private const int STEP_HEIGHT = 12;

        private ArpeggioDefinition currentArpeggioDefinition;

        private Song testSong;
        private Song currentSong;

        private Player songPlayer;
        private int initialIntervalIdx;

        private bool loadingControls;

        public ArpeggioEditor()
        {
            InitializeComponent();

            songPlayer = new Player();
            songPlayer.Stereo = LibAYEmu.Stereo.Mono;

            initialIntervalIdx = -1;

            this.currentArpeggioDefinition = new ArpeggioDefinition();
            this.loadControls();
        }

        private void loadControls()
        {
            this.loadingControls = true;

            this.arpeggioDefBindingSource.DataSource = this.currentArpeggioDefinition;

            this.cboInstruments.SelectedItem = findInstrument(this.currentArpeggioDefinition.BaseNote.Instrument);

            this.numOctave.Value = currentArpeggioDefinition.BaseNote.Octave;

            string cboNoteSelection = currentArpeggioDefinition.BaseNote.HasSeminote ?
                string.Format("{0}#", currentArpeggioDefinition.BaseNote.Note) : currentArpeggioDefinition.BaseNote.Note.ToString();
            this.cboNote.SelectedItem = cboNoteSelection;

            this.numLength.Value = currentArpeggioDefinition.Length;

            this.cboPattern.SelectedValue = currentArpeggioDefinition.TargetPatternIdx;

            this.numNewPatternLength.Value = currentArpeggioDefinition.NewPatternLength;

            this.cboChannel.SelectedValue = currentArpeggioDefinition.TargetChannelIdx;

            this.cboHighlightByScale.SelectedValue = currentArpeggioDefinition.HighlightByScale;

            this.numMaxOctaves.Value = currentArpeggioDefinition.OctavesInArpeggio;

            this.loadingControls = false;
            this.buildTestPattern();
            this.updateWaveSize();
        }

        private Instrument findInstrument(string id)
        {
            Instrument result = null;
            foreach (Instrument i in cboInstruments.Items)
            {
                if (i.ID == id)
                {
                    result = i;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Gets or sets the current song.
        /// </summary>
        public Song CurrentSong
        {
            get
            {
                return currentSong;
            }
            set
            {
                if (value != currentSong)
                {
                    currentSong = value;
                    updateCurrentSong();
                }
            }
        }

        private int MaxArpeggioInterval
        {
            get
            {
                return this.currentArpeggioDefinition == null ? 0 : 12 * this.currentArpeggioDefinition.OctavesInArpeggio;
            }
        }

        private void updateCurrentSong()
        {
            songPlayer.CurrentSong = currentSong;
            initializeTestSong();
            initializeCombos();
            this.songBindingSource.DataSource = currentSong;

            if (this.currentSong.Instruments.Count > 0)
            {
                this.currentArpeggioDefinition.BaseNote.Instrument = this.currentSong.Instruments[0].ID;
            }

            pbWave.Invalidate();
        }

        private void initializeTestSong()
        {
            testSong = new Song(this.currentSong.Channels);
            testSong.Tempo = this.currentSong.Tempo;
            testSong.Instruments = currentSong.Instruments;
            testSong.Effects = currentSong.Effects;
            testSong.Frequencies = currentSong.Frequencies;
            testSong.ChipFrequency = currentSong.ChipFrequency;

            testSong.PlayOrder.Clear();
            testSong.PlayOrder.Add(0);

            buildTestPattern();

            songPlayer.CurrentSong = testSong;
        }

        private void initializeTestPattern()
        {
            if (this.currentArpeggioDefinition.TargetPatternIdx == -1)
            {
                testSong.Patterns.Clear();
                testSong.Patterns.Add(new Pattern(this.currentSong.Channels));
                testSong.Patterns[0].Length = (int)numNewPatternLength.Value;
            }
            else
            {
                Pattern targetPattern = this.currentSong.Patterns[this.currentArpeggioDefinition.TargetPatternIdx];
                Pattern clonedPattern = SerializationUtils.Clone(targetPattern);
                testSong.Patterns.Clear();
                testSong.Patterns.Add(clonedPattern);
            }
        }

        private void initializeCombos()
        {
            cboPattern.Items.Clear();
            cboPattern.Items.Add(new KeyValuePair<int, string>(-1, Properties.Resources.NewPattern));
            for (int i = 0; i < this.currentSong.Patterns.Count; i++)
            {
                cboPattern.Items.Add(
                    new KeyValuePair<int, string>(
                        i,
                        string.Format(Properties.Resources.PatternWithNumber, i)));
            }
            cboPattern.SelectedIndex = 0;

            cboChannel.Items.Clear();
            for (int i = 0; i < this.currentSong.Channels; i++)
            {
                cboChannel.Items.Add(
                    new KeyValuePair<int, string>(
                        i,
                        string.Format(Properties.Resources.ChannelWithNumber, i)));
            }
            cboChannel.SelectedIndex = 0;

            cboHighlightByScale.Items.Clear();
            cboHighlightByScale.Items.Add(new KeyValuePair<int, string>(-1, Properties.Resources.NoScale));
            foreach (Scales s in Enum.GetValues(typeof(Scales)))
            {
                cboHighlightByScale.Items.Add(
                    new KeyValuePair<int, string>(
                        (int)s,
                        Properties.Resources.ResourceManager.GetString(Enum.GetName(typeof(Scales), s))
                    )
                );
            }
            cboHighlightByScale.SelectedIndex = 0;
        }

        private void buildTestPattern()
        {
            if (testSong != null && currentArpeggioDefinition != null && !loadingControls)
            {
                initializeTestPattern();
                int patternIdx = 0;
                Pattern current = testSong.Patterns[0];
                int targetChannel = currentArpeggioDefinition.TargetChannelIdx;

                if (currentArpeggioDefinition.Length > 0)
                {
                    bool noteActive = false;

                    current.PopulateInstruments(targetChannel);

                    int i = 0;
                    while (patternIdx < current.Length)
                    {
                        ChannelNote oldNote = current.Lines[patternIdx].Notes[targetChannel];

                        if (currentArpeggioDefinition.ActiveNotes[i])
                        {
                            ChannelNote note = SerializationUtils.Clone(currentArpeggioDefinition.BaseNote);
                            note.Transpose(currentArpeggioDefinition.Intervals[i], currentArpeggioDefinition.BaseNote.Octave);
                            current.Lines[patternIdx].Notes[targetChannel] = note;
                            noteActive = true;
                        }
                        else if (currentArpeggioDefinition.SilencedNotes[i])
                        {
                            noteActive = false;
                            if (!oldNote.HasValue)
                            {
                                current.Lines[patternIdx].Notes[targetChannel] = ChannelNote.SilenceNote;
                            }
                        }
                        else if (noteActive)
                        {
                            current.Lines[patternIdx].Notes[targetChannel] = ChannelNote.EmptyNote;
                        }

                        patternIdx++;
                        i = (i + 1) % currentArpeggioDefinition.Length;
                    }

                    current.OptimizeInstrumentUsage(targetChannel);
                }
            }
        }

        private void numLength_ValueChanged(object sender, EventArgs e)
        {
            this.currentArpeggioDefinition.Length = (int)numLength.Value;
            buildTestPattern();
            updateWaveSize();
        }

        private void cmdOk_Click(object sender, EventArgs e)
        {
            buildTestPattern();

            Pattern newPattern = SerializationUtils.Clone(this.testSong.Patterns[0]);
            if (cboPattern.SelectedIndex == 0)
            {
                // Add new pattern.
                this.currentSong.Patterns.Add(newPattern);
                this.currentSong.PlayOrder.Add(this.currentSong.Patterns.IndexOf(newPattern));
            }
            else
            {
                KeyValuePair<int, string> selected = (KeyValuePair<int, string>)cboPattern.SelectedItem;
                this.currentSong.Patterns[selected.Key] = newPattern;
            }
            this.Close();
        }

        private void cboPattern_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboPattern.SelectedIndex != -1)
            {
                KeyValuePair<int, string> selected = (KeyValuePair<int, string>)cboPattern.SelectedItem;
                currentArpeggioDefinition.TargetPatternIdx = selected.Key;
                buildTestPattern();
            }
        }

        private void cboChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboChannel.SelectedIndex != -1)
            {
                KeyValuePair<int, string> selected = (KeyValuePair<int, string>)cboChannel.SelectedItem;
                currentArpeggioDefinition.TargetChannelIdx = selected.Key;
                buildTestPattern();
            }
        }

        private void pbWave_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                updateBars(e.Location, false);
            }
            if (e.Button == MouseButtons.Right)
            {
                updateBars(e.Location, true);
            }

            buildTestPattern();
            initialIntervalIdx = -1;
        }

        private void pbWave_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                updateBars(e.Location, false);
            }
        }

        private void pbWave_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                updateBars(e.Location, false);
            }
        }

        private void updateBars(Point pt, bool isDelete)
        {
            float steps = ((2.0f * MaxArpeggioInterval) + 1);

            if (initialIntervalIdx == -1)
            {
                initialIntervalIdx = (int)(pt.X / STEP_WIDTH) - 1;
            }

            int intervalIdx = initialIntervalIdx;

            if (!chkLockMovement.Checked)
            {
                intervalIdx = (int)(pt.X / STEP_WIDTH) - 1;
            }

            float intervalFloat = (pbWave.ClientRectangle.Height - pt.Y) / STEP_HEIGHT;
            int intervalValue = (int)intervalFloat;
            int realValue = intervalValue - MaxArpeggioInterval;

            if (intervalIdx >= 0 && intervalIdx < this.currentArpeggioDefinition.Length &&
                Math.Abs(realValue) < (MaxArpeggioInterval + 1))
            {
                this.currentArpeggioDefinition.Intervals[intervalIdx] = isDelete ? 0 : realValue;

                this.currentArpeggioDefinition.SilencedNotes[intervalIdx] =
                    (isDelete &
                     !this.currentArpeggioDefinition.ActiveNotes[intervalIdx] &
                     !this.currentArpeggioDefinition.SilencedNotes[intervalIdx]);

                this.currentArpeggioDefinition.ActiveNotes[intervalIdx] = !isDelete;
                pbWave.Invalidate();
            }
        }

        private void pbWave_Paint(object sender, PaintEventArgs e)
        {
            float steps = ((2.0f * MaxArpeggioInterval) + 1);

            RectangleF rectInterval =
                new RectangleF(0, 0, STEP_WIDTH, STEP_HEIGHT);

            Brush currentBrush;
            Point brushPt1 = Point.Empty, brushPt2 = Point.Empty;

            List<int> intervals = getIntervalsForScale();

            float y = 0;
            
            ChannelNote noteToPaint = SerializationUtils.Clone(this.currentArpeggioDefinition.BaseNote);
            noteToPaint.Transpose(MaxArpeggioInterval, noteToPaint.Octave);

            for (int i = 0; i < steps; i++)
            {
                int remainder = (int) (MaxArpeggioInterval - i) % 12;
                if (remainder < 0) remainder += 12;

                if (intervals.Contains(remainder))
                {
                    e.Graphics.FillRectangle(Brushes.DarkGreen, 0, y, pbWave.ClientRectangle.Width, STEP_HEIGHT);
                }

                e.Graphics.DrawLine(Pens.LimeGreen, 0, y, pbWave.ClientRectangle.Width, y);

                string noteText = string.Format("{0}{1}{2}", noteToPaint.Octave, noteToPaint.Note, noteToPaint.HasSeminote ? "#" : "");
                e.Graphics.DrawString(noteText, this.Font, Brushes.LimeGreen, 0, y);

                y += STEP_HEIGHT;
                noteToPaint.Transpose(-1, noteToPaint.Octave);
            }
            e.Graphics.DrawLine(Pens.LimeGreen, 0, y, pbWave.ClientRectangle.Width, y);

            e.Graphics.DrawLine(Pens.LimeGreen, 0, 0, 0, pbWave.ClientRectangle.Height);

            rectInterval.X = rectInterval.Width;
            for (int i = 0; i < this.currentArpeggioDefinition.Length; i++)
            {
                e.Graphics.DrawLine(Pens.LimeGreen, rectInterval.X, 0, rectInterval.X, pbWave.ClientRectangle.Height);

                if (this.currentArpeggioDefinition.ActiveNotes[i] || this.currentArpeggioDefinition.SilencedNotes[i])
                {
                    int interval = this.currentArpeggioDefinition.Intervals[i];
                    rectInterval.Y = (MaxArpeggioInterval - interval) * STEP_HEIGHT;

                    brushPt1.X = (int)Math.Round(rectInterval.Left + 1);
                    brushPt2.X = (int)Math.Round(rectInterval.Right - 1);

                    if (this.currentArpeggioDefinition.ActiveNotes[i])
                    {
                        currentBrush = new LinearGradientBrush(brushPt1, brushPt2, 
                            Color.DarkRed, Color.Red);// Brushes.Aqua;

                        e.Graphics.FillRectangle(currentBrush,
                            rectInterval.X + 1, rectInterval.Y + 1,
                            rectInterval.Width - 2, rectInterval.Height - 2);

                        currentBrush.Dispose();
                    }
                    else
                    {
                        currentBrush = new LinearGradientBrush(brushPt1, brushPt2, 
                            Color.FromArgb(192, 64, 0), Color.Orange);// Brushes.Aqua;

                        e.Graphics.FillRectangle(currentBrush,
                            rectInterval.X + 1, rectInterval.Y + 1,
                            rectInterval.Width - 2, rectInterval.Height - 2);

                        currentBrush.Dispose();
                    }
                }
                rectInterval.X += rectInterval.Width;
            }
            e.Graphics.DrawLine(Pens.LimeGreen, rectInterval.X, 0, rectInterval.X, pbWave.ClientRectangle.Height);
        }

        private List<int> getIntervalsForScale()
        {
            List<int> intervals = null;

            if (currentArpeggioDefinition.HighlightByScale != -1)
            {
                intervals = new List<int>(ScaleManager.OffsetsByScale[(Scales)currentArpeggioDefinition.HighlightByScale]);
            }
            else
            {
                intervals = new List<int>(new int[] { 0 });
            }
            return intervals;
        }

        private void numOctave_ValueChanged(object sender, EventArgs e)
        {
            this.currentArpeggioDefinition.BaseNote.Octave = (int)numOctave.Value;
            buildTestPattern();
            this.pbWave.Invalidate();
        }

        private void cboNote_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedItem = (string)cboNote.SelectedItem;
            this.currentArpeggioDefinition.BaseNote.Note = selectedItem[0];
            if (selectedItem.Contains("#"))
            {
                this.currentArpeggioDefinition.BaseNote.Seminote = '+';
            }
            else
            {
                this.currentArpeggioDefinition.BaseNote.Seminote = char.MinValue;
            }
            buildTestPattern();
            this.pbWave.Invalidate();
        }

        private void ArpeggioEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (songPlayer != null)
            {
                songPlayer.Dispose();
            }
        }

        private void pbWave_Resize(object sender, EventArgs e)
        {
            pbWave.Invalidate();
        }

        private void cboInstruments_SelectedIndexChanged(object sender, EventArgs e)
        {
            Instrument current = cboInstruments.SelectedItem as Instrument;

            if (current != null)
            {
                this.currentArpeggioDefinition.BaseNote.Instrument = current.ID;
            }
            else
            {
                this.currentArpeggioDefinition.BaseNote.Instrument = string.Empty;
            }
            buildTestPattern();
        }

        private void numNewPatternLength_ValueChanged(object sender, EventArgs e)
        {
            this.currentArpeggioDefinition.NewPatternLength = (int)numNewPatternLength.Value;
            this.buildTestPattern();
        }

        private void chkTesting_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTesting.Checked)
            {
                songPlayer.Play();
            }
            else
            {
                songPlayer.Stop();
            }
        }

        private void cmdLoadArpeggio_Click(object sender, EventArgs e)
        {
            this.ofd.Filter = WYZTracker.Properties.Resources.ArpeggioFilter;
            if (this.ofd.ShowDialog() == DialogResult.OK)
            {
                this.currentArpeggioDefinition = ArpeggioManager.Load(this.ofd.FileName);
                this.loadControls();
            }
        }

        private void cmdSaveArpeggio_Click(object sender, EventArgs e)
        {
            this.sfd.Filter = WYZTracker.Properties.Resources.ArpeggioFilter;
            if (this.sfd.ShowDialog() == DialogResult.OK)
            {
                ArpeggioManager.Save(this.currentArpeggioDefinition, this.sfd.FileName);
            }
        }

        private void cboHighlightByScale_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                object selectedItem = cboHighlightByScale.SelectedItem;

                if (selectedItem != null)
                {
                    KeyValuePair<int, string> pair = (KeyValuePair<int, string>)selectedItem;
                    this.currentArpeggioDefinition.HighlightByScale = pair.Key;
                }
                else
                {
                    this.currentArpeggioDefinition.HighlightByScale = -1;
                }

                this.pbWave.Invalidate();
            }
        }

        private void cmdRandomize_Click(object sender, EventArgs e)
        {
            int currentPosition = 0;
            Random rnd = new Random();
            
            List<int> intervals = getIntervalsForScale();

            while (currentPosition < this.currentArpeggioDefinition.Length)
            {
                int currentInterval = rnd.Next((-2 * MaxArpeggioInterval), 2 * MaxArpeggioInterval);
                // -2 = nothing
                // -1 = silence
                // [0 - 2 * MAX_ARPEGGIO] = interval.
                if (currentInterval < -MaxArpeggioInterval)
                {
                    this.currentArpeggioDefinition.SilencedNotes[currentPosition] = false;
                    this.currentArpeggioDefinition.ActiveNotes[currentPosition] = false;
                    this.currentArpeggioDefinition.Intervals[currentPosition] = 0;
                    currentPosition++;
                }
                else if (currentInterval < 0)
                {
                    this.currentArpeggioDefinition.SilencedNotes[currentPosition] = true;
                    this.currentArpeggioDefinition.ActiveNotes[currentPosition] = false;
                    this.currentArpeggioDefinition.Intervals[currentPosition] = 0;
                    currentPosition++;
                }
                else
                {
                    int intervalValue = MaxArpeggioInterval - currentInterval;

                    int remainder = intervalValue % 12;
                    while (remainder < 0) remainder += 12;

                    if (intervals.Contains(remainder) || intervals.Count == 1)
                    {
                        this.currentArpeggioDefinition.SilencedNotes[currentPosition] = false;
                        this.currentArpeggioDefinition.ActiveNotes[currentPosition] = true;
                        this.currentArpeggioDefinition.Intervals[currentPosition] = intervalValue;
                        currentPosition++;
                    }
                }
            }
            this.buildTestPattern();
            this.pbWave.Invalidate();
        }

        private void numMaxOctaves_ValueChanged(object sender, EventArgs e)
        {
            if (!this.loadingControls && this.currentArpeggioDefinition != null)
            {
                this.currentArpeggioDefinition.OctavesInArpeggio = (int) numMaxOctaves.Value;
                updateWaveSize();                
            }
        }

        private void updateWaveSize()
        {
            pbWave.Size = new System.Drawing.Size(
                (int)(STEP_WIDTH * (this.currentArpeggioDefinition.Length + 1.0f)) + 1,
                (int)(STEP_HEIGHT * (2 * MaxArpeggioInterval + 1)) + 1);
        }
    }
}
