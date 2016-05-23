using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Drawing.Text;

namespace WYZTracker
{
    public class PatternView : Control
    {
        private static PrivateFontCollection pfc;

        static PatternView()
        {
            pfc = new PrivateFontCollection();
            byte[] fontdata = Properties.Resources.WHITRABT;
            unsafe
            {
                fixed (byte* pFontData = fontdata)
                {
                    pfc.AddMemoryFont((System.IntPtr)pFontData, fontdata.Length);
                }
            }
        }

        private const int MIN_OCTAVE = 2;
        private const int MAX_OCTAVE = 6;

        public event EventHandler<PlayEventArgs> Play;
        public event EventHandler Stop;
        public event EventHandler IncreaseOctave;
        public event EventHandler DecreaseOctave;
        public event EventHandler EditionIncrementChanged;
        public event EventHandler HighlightRangeChanged;
        public event EventHandler IncreaseInstrument;
        public event EventHandler DecreaseInstrument;
        public event EventHandler NextPattern;
        public event EventHandler PreviousPattern;
        public event EventHandler<ActiveFxEventArgs> SetActiveFx;
        public event EventHandler<ActiveInstrumentEventArgs> SetActiveInstrument;

        private ChannelLine[] clipboardBuffer = { };
        private bool clipboardBufferHasFx = false;

        private int selectionStart = 0;
        private int selectionEnd = 0;
        private int selectionChanStart = 0;
        private int selectionChanEnd = 0;

        private VirtualPiano virtPiano;

        private List<Keys> hexKeys = new List<Keys>(new Keys[] {
                Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9,
                Keys.A, Keys.B, Keys.C, Keys.D, Keys.E, Keys.F
            });

        public PatternView()
        {
            UpdateFont();
            this.SetStyle(ControlStyles.Selectable | ControlStyles.OptimizedDoubleBuffer, true);
            this.highlightRange = 4;
            this.highlightBackColor = Color.DarkGreen;
            this.highlightBackColor2 = Color.Transparent;
            this.highlightForeColor = Color.Yellow;
            this.selectionBackColor = Color.FromKnownColor(KnownColor.Highlight);
            this.selectionForeColor = Color.FromKnownColor(KnownColor.HighlightText);

            virtPiano = new VirtualPiano(this);
            virtPiano.Mode = VirtualPiano.PianoMode.Instrument;
            virtPiano.NoteFxPressed += virtPiano_NoteFxPressed;
        }

        ~PatternView()
        {
            virtPiano.NoteFxPressed -= virtPiano_NoteFxPressed;
        }

        private Pattern currentPattern;

        public Pattern CurrentPattern
        {
            get { return currentPattern; }
            set
            {
                currentPattern = value;
                initParameters();
                this.Invalidate(this.ClientRectangle, false);
            }
        }

        private Song currentSong;

        public Song CurrentSong
        {
            get { return currentSong; }
            set
            {
                currentSong = value;
            }
        }

        private int editionIncrement;

        public int EditionIncrement
        {
            get { return editionIncrement; }
            set
            {
                if (value != editionIncrement)
                {
                    editionIncrement = value;
                    this.OnEditionIncrementChanged(this, EventArgs.Empty);
                }
            }
        }

        public int DelayDecrement { get; set; }

        private void initParameters()
        {
            this.selectedIndex = 0;
            this.selectionStart = 0;
            this.selectionEnd = 0;
            this.FirstVisibleNoteIndex = 0;
        }

        private int selectedIndex;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                if (value != selectedIndex)
                {
                    if (value >= this.FirstVisibleNoteIndex + this.visibleNoteCount || selectedIndex < this.FirstVisibleNoteIndex)
                    {
                        selectedIndex = value;
                        while (selectedIndex > this.FirstVisibleNoteIndex + this.visibleNoteCount)
                        {
                            this.FirstVisibleNoteIndex++;
                        }
                        this.selectionStart = value;
                        this.selectionEnd = value;
                        this.Invalidate();
                    }
                    else
                    {
                        if (value < this.FirstVisibleNoteIndex)
                        {
                            selectedIndex = value;
                            this.FirstVisibleNoteIndex = value;
                            this.selectionStart = value;
                            this.selectionEnd = value;
                            this.Invalidate();
                        }
                        else
                        {
                            if (selectionStart == selectionEnd)
                            {
                                Graphics g = Graphics.FromHwnd(this.Handle);
                                paintLine(selectedIndex, g, false, true);
                                selectedIndex = value;
                                this.selectionStart = value;
                                this.selectionEnd = value;
                                paintLine(selectedIndex, g, true, true);
                                g.Dispose();
                            }
                            else
                            {
                                selectedIndex = value;
                                this.selectionStart = value;
                                this.selectionEnd = value;
                                this.Invalidate();
                            }

                        }
                    }
                }
            }
        }

        private int currentChannel;

        public int CurrentChannel
        {
            get { return currentChannel; }
            set
            {
                currentChannel = value;
                Graphics g = Graphics.FromHwnd(this.Handle);
                paintLine(selectedIndex, g, true, true);
                g.Dispose();

                if (currentSong != null)
                {
                    if (currentChannel == currentSong.Channels)
                    {
                        virtPiano.Mode = VirtualPiano.PianoMode.Fx;
                    }
                    else
                    {
                        virtPiano.Mode = VirtualPiano.PianoMode.Instrument;
                    }
                }
            }
        }

        private Instrument currentInstrument;

        public Instrument CurrentInstrument
        {
            get { return currentInstrument; }
            set
            {
                currentInstrument = value;
                this.virtPiano.CurrentInstrument = currentInstrument;
            }
        }

        private int firstVisibleIndex;

        public int FirstVisibleNoteIndex
        {
            get { return firstVisibleIndex; }
            set
            {
                firstVisibleIndex = value;

                if (currentPattern != null && firstVisibleIndex > currentPattern.Length - visibleNoteCount)
                    firstVisibleIndex = currentPattern.Length - visibleNoteCount;

                if (firstVisibleIndex < 0)
                    firstVisibleIndex = 0;
            }
        }

        private int highlightRange;

        public int HighlightRange
        {
            get { return highlightRange; }
            set
            {
                if (value != highlightRange)
                {
                    highlightRange = value;
                    OnHighlightRangeChanged(this, EventArgs.Empty);
                    this.Invalidate();
                }
            }
        }

        private Color highlightBackColor;

        public Color HighlightBackColor
        {
            get { return highlightBackColor; }
            set { highlightBackColor = value; }
        }

        private Color highlightBackColor2;

        public Color HighlightBackColor2
        {
            get { return highlightBackColor2; }
            set { highlightBackColor2 = value; }
        }

        private Color highlightForeColor;

        public Color HighlightForeColor
        {
            get { return highlightForeColor; }
            set { highlightForeColor = value; }
        }

        private Color selectionBackColor;

        public Color SelectionBackColor
        {
            get { return selectionBackColor; }
            set { selectionBackColor = value; }
        }

        private Color selectionForeColor;

        public Color SelectionForeColor
        {
            get { return selectionForeColor; }
            set { selectionForeColor = value; }
        }

        private bool advancedMode = false;

        public bool AdvancedMode
        {
            get { return advancedMode; }
            set { advancedMode = value; }
        }

        private int visibleNoteCount;
        private int marginWidth;

        protected override void OnSizeChanged(EventArgs e)
        {
            updateNoteCount();
            this.Invalidate();
            base.OnSizeChanged(e);
        }

        private void updateNoteCount()
        {
            visibleNoteCount = (int)Math.Floor((double)this.Height / (double)this.FontHeight) - 1;
            if (visibleNoteCount < 0)
                visibleNoteCount = 0;
        }

        private void doLayout()
        {
            int oldNoteCount = visibleNoteCount;
            updateNoteCount();
            int oldFirst = this.FirstVisibleNoteIndex;

            if (this.FirstVisibleNoteIndex != oldFirst || oldNoteCount != visibleNoteCount)
            {
                this.Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                selectionStart = getSelectedIndex(e);
                selectionChanStart = getSelectedChannel(e);
                selectionEnd = selectionStart;
                selectionChanEnd = selectionChanStart;

                if (selectionStart != -1)
                {
                    this.selectedIndex = selectionStart;
                }
                if (selectionChanStart != -1)
                {
                    this.CurrentChannel = selectionChanStart;
                }

                this.Invalidate();
            }
            this.Focus();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button == MouseButtons.Left)
            {
                int line, channel;
                line = getSelectedIndex(e);
                channel = getSelectedChannel(e);

                int min, max;
                getMinMax(line, selectedIndex, out min, out max);
                this.selectionStart = min;
                this.selectionEnd = max;

                getMinMax(channel, currentChannel, out min, out max);
                this.selectionChanStart = min;
                this.selectionChanEnd = max;

                while ((line < this.FirstVisibleNoteIndex) && (this.FirstVisibleNoteIndex >= 0))
                    this.FirstVisibleNoteIndex = line;

                // Quietos o bajando
                while (line > this.FirstVisibleNoteIndex + this.visibleNoteCount)
                {
                    FirstVisibleNoteIndex++;
                }
                this.Invalidate();
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            this.virtPiano.Enabled = true;
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            this.virtPiano.Enabled = false;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            int upLine, upChannel;
            if (e.Button == MouseButtons.Left)
            {
                int line, channel;
                line = getSelectedIndex(e);
                channel = getSelectedChannel(e);

                int min, max;
                getMinMax(line, selectedIndex, out min, out max);
                this.selectionStart = min;
                this.selectionEnd = max;

                getMinMax(channel, currentChannel, out min, out max);
                this.selectionChanStart = min;
                this.selectionChanEnd = max;

                upLine = getSelectedIndex(e);
                this.selectedIndex = upLine;

                upChannel = getSelectedChannel(e);
                this.CurrentChannel = upChannel;
            }
            this.Invalidate();
        }

        private void getMinMax(int upLine, int downLine, out int min, out int max)
        {
            if (upLine < downLine)
            {
                min = upLine;
                max = downLine;
            }
            else
            {
                min = downLine;
                max = upLine;
            }
        }

        private int getSelectedChannel(MouseEventArgs e)
        {
            int selected = 0;
            if (this.currentSong != null)
            {
                selected = (e.X - marginWidth) / Properties.Settings.Default.ColumnWidth;

                if (selected < 0)
                    selected = 0;

                if (selected > this.currentSong.Channels)
                    selected = this.currentSong.Channels;
            }
            return selected;
        }

        private int getSelectedIndex(MouseEventArgs e)
        {
            int selected;

            selected = this.FirstVisibleNoteIndex + (int)Math.Floor((double)e.Y / this.FontHeight);

            if (selected >= this.currentPattern.Length)
                selected = this.currentPattern.Length - 1;

            if (selected < 0)
                selected = 0;

            return selected;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            int desplazamiento = e.Delta > 0 ? -(this.HighlightRange) : this.highlightRange;

            this.FirstVisibleNoteIndex += desplazamiento;

            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            marginWidth = (int)e.Graphics.MeasureString("9999:", this.Font).Width;

            Brush backBrush = new SolidBrush(this.BackColor);
            PointF currentPoint = PointF.Empty;
            Brush currentBrush = new SolidBrush(this.ForeColor);
            Brush selectedBackBrush = new SolidBrush(this.selectionBackColor);
            Brush selectedBrush = new SolidBrush(this.selectionForeColor);
            Brush highlightBrush = new SolidBrush(this.highlightForeColor);

            e.Graphics.FillRectangle(backBrush, this.ClientRectangle);

            if (this.currentPattern != null)
            {
                doLayout();

                int lastVisibleIndex = FirstVisibleNoteIndex + visibleNoteCount;

                if (lastVisibleIndex >= this.currentPattern.Length)
                    lastVisibleIndex = this.currentPattern.Length - 1;

                for (int i = FirstVisibleNoteIndex; i <= lastVisibleIndex; i++)
                {
                    paintLine(i, e.Graphics, (i >= selectionStart && i <= selectionEnd), false, currentPoint, backBrush, currentBrush, selectedBackBrush, selectedBrush, highlightBrush);
                }
            }
            currentBrush.Dispose();
            backBrush.Dispose();

            selectedBackBrush.Dispose();
            selectedBrush.Dispose();

            highlightBrush.Dispose();
        }

        private void paintLine(int index, Graphics g, bool selected, bool performLayout)
        {
            Brush backBrush = new SolidBrush(this.BackColor);
            PointF currentPoint = PointF.Empty;
            Brush currentBrush = new SolidBrush(this.ForeColor);
            Brush selectedBackBrush = new SolidBrush(this.selectionBackColor);
            Brush selectedBrush = new SolidBrush(this.selectionForeColor);
            Brush highlightBrush = new SolidBrush(this.highlightForeColor);

            currentPoint = paintLine(index, g, selected, performLayout, currentPoint, backBrush, currentBrush, selectedBackBrush, selectedBrush, highlightBrush);

            currentBrush.Dispose();
            backBrush.Dispose();
            selectedBackBrush.Dispose();
            selectedBrush.Dispose();
            highlightBrush.Dispose();
        }

        private PointF paintLine(int index, Graphics g, bool selected, bool performLayout, PointF currentPoint, Brush backBrush, Brush currentBrush, Brush selectedBackBrush, Brush selectedBrush, Brush highlightBrush)
        {
            Brush textBrush;

            if (this.currentPattern != null)
            {
                if (performLayout)
                {
                    doLayout();
                }

                currentPoint.Y = this.FontHeight * (index - FirstVisibleNoteIndex);
                currentPoint.X = 0;

                if ((index % highlightRange) == 0)
                {
                    PointF nextPoint = new PointF(currentPoint.X, currentPoint.Y + this.FontHeight);

                    Brush highlightBackBrush = (highlightBackColor2 == Color.Transparent) ?
                        (Brush)new SolidBrush(this.highlightBackColor) :
                        (Brush)new LinearGradientBrush(currentPoint, nextPoint, this.highlightBackColor, this.highlightBackColor2);

                    g.FillRectangle(highlightBackBrush, currentPoint.X, currentPoint.Y, this.ClientRectangle.Width, this.FontHeight);

                    highlightBackBrush.Dispose();
                }
                else
                {
                    g.FillRectangle(backBrush, currentPoint.X, currentPoint.Y, this.ClientRectangle.Width, this.FontHeight);
                }

                if (selected)
                {
                    Pen selectedPen = new Pen(selectedBackBrush, 2);

                    g.FillRectangle(selectedBackBrush,
                        new RectangleF(marginWidth + (Properties.Settings.Default.ColumnWidth * this.selectionChanStart),
                                       currentPoint.Y,
                                       Properties.Settings.Default.ColumnWidth * (this.selectionChanEnd - this.selectionChanStart + 1),
                                       this.FontHeight));

                    textBrush = selectedBrush;
                    if (index == this.SelectedIndex)
                    {
                        Pen whitePen = new Pen(new SolidBrush(Color.White));
                        whitePen.DashStyle = DashStyle.Dot;
                        g.DrawRectangle(whitePen,
                                        marginWidth + (Properties.Settings.Default.ColumnWidth * this.CurrentChannel),
                                        currentPoint.Y,
                                        Properties.Settings.Default.ColumnWidth,
                                        this.FontHeight - 1);
                        whitePen.Dispose();
                    }
                    selectedPen.Dispose();
                }
                else
                {
                    if ((index % highlightRange) == 0)
                    {
                        textBrush = highlightBrush;
                    }
                    else
                    {
                        textBrush = currentBrush;
                    }
                }
                ChannelLine line = this.currentPattern.Lines[index];

                g.DrawString(
                    string.Format("{0:00}{1}",
                                  index,
                                  line.TempoModifier == 0 ? ":" : line.TempoModifier > 0 ? "^" : "v"),
                    this.Font, textBrush, currentPoint);

                currentPoint.X = marginWidth;

                for (int j = 0; j < this.currentPattern.Channels; j++)
                {
                    g.DrawString(getONSI(index, j), this.Font, textBrush, currentPoint);
                    currentPoint.X += Properties.Settings.Default.ColumnWidth;
                }

                if (line.Fx == int.MinValue)
                {
                    g.DrawString("..", this.Font, textBrush, currentPoint);
                }
                else
                {
                    g.DrawString(line.Fx.ToString("X2"), this.Font, textBrush, currentPoint);
                }
                currentPoint.X += Properties.Settings.Default.ColumnWidth;
            }
            return currentPoint;
        }

        private string getONSI(int lineIdx, int noteIdx)
        {
            StringBuilder val = new StringBuilder();

            ChannelNote currentNote = this.currentPattern.Lines[lineIdx].Notes[noteIdx];

            val.Append(currentNote.HasNote ? currentNote.Note.ToString() : ".");
            val.Append(currentNote.HasSeminote ? "#" : ".");
            val.Append(currentNote.HasOctave ? currentNote.Octave.ToString() : ".");
            val.Append(" ");
            val.Append(currentNote.HasInstrument ? ((currentNote.Instrument == "R") ? ".R" : string.Format("{0:X2}", int.Parse(currentNote.Instrument))) : "..");
            val.Append(currentNote.IsSawtooth ? currentNote.EnvData.Style.ToString("X") : ".");
            val.Append(currentNote.IsSawtooth ? currentNote.EnvData.FrequencyRatio.ToString("X") : ".");
            val.Append(currentNote.IsSawtooth ? (currentNote.EnvData.ActiveFrequencies ? "1" : "0") : ".");
            val.Append(" ");

            if ((currentNote.Instrument != "R") && (currentNote.VolModifier.HasValue))
            {
                val.AppendFormat("V{0}", getVolumeWithModifier(currentNote.VolModifier));
            }
            else
            {
                val.Append("...");
            }

            return val.ToString();
        }

        private string getVolumeWithModifier(sbyte? volModifier)
        {
            string result = "..";
            if (volModifier != null && volModifier.HasValue)
            {
                sbyte mod = volModifier.GetValueOrDefault();
                if (mod >= 0)
                {
                    result = string.Format("+{0}", mod.ToString("X"));
                }
                else
                {
                    result = string.Format("-{0}", Math.Abs(mod).ToString("X"));
                }
            }
            return result;
        }

        protected override bool IsInputKey(Keys keyData)
        {
            return (keyData != Keys.Tab);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (!e.Control)
            {
                if (e.Shift)
                {
                    processKeyDownWithShift(e);
                }
                else
                {
                    processKeyDownWithoutCtrl(e);
                }
                //Console.WriteLine("lastKeyWasHexVal = false (1)");
                lastKeyWasHexVal = false;
            }
            else
            {
                processKeyDownWithCtrl(e);
            }
        }

        private void processKeyDownWithCtrl(KeyEventArgs e)
        {
            if (!e.Shift)
            {
                switch (e.KeyCode)
                {
                    case Keys.PageDown:
                        // Avanzar al siguiente patrón.
                        this.OnNextPattern(this, EventArgs.Empty);
                        break;
                    case Keys.PageUp:
                        // Ir al patrón anterior.
                        this.OnPreviousPattern(this, EventArgs.Empty);
                        break;
                }
            }

            if (e.Alt)
            {
                processKeyDownWithCtrlAndAlt(e);
            }
            else
            {
                if (e.Shift)
                {
                    processKeyDownWithCtrlAndShift(e);
                }
                else
                {
                    processKeyDownWithCtrlNoShift(e);
                    //Console.WriteLine("lastKeyWasHexVal = false (3)");
                    lastKeyWasHexVal = false;
                }
            }

        }

        private bool lastKeyWasHexVal = false;
        private int lastHexVal = -1;

        private void processKeyDownWithCtrlAndAlt(KeyEventArgs e)
        {
            Console.WriteLine(e.KeyCode);
            int hexVal = hexKeys.IndexOf(e.KeyCode);
            Console.WriteLine(hexVal);
            Console.WriteLine(lastKeyWasHexVal);
            if (hexVal != -1)
            {
                if (lastKeyWasHexVal)
                {
                    lastHexVal = ((lastHexVal * 16) + hexVal) & 0xFF;
                }
                else
                {
                    lastHexVal = hexVal;
                }
                lastKeyWasHexVal = true;

                if (this.currentChannel != this.currentSong.Channels)
                {
                    // Set active instrument
                    string instrumentId = lastHexVal.ToString();
                    Instrument instr = this.currentSong.GetInstrument(instrumentId);
                    if (instr != null)
                    {
                        this.OnSetActiveInstrument(this, new ActiveInstrumentEventArgs(instr.ID));
                        this.SetInstrumentToSelection(instr.ID);
                    }
                    else if (instrumentId != "R")
                    {
                        // Probamos con un único dígito.
                        lastHexVal = lastHexVal & 0xF;
                        instr = this.currentSong.GetInstrument(instrumentId);
                        if (instr != null)
                        {
                            this.OnSetActiveInstrument(this, new ActiveInstrumentEventArgs(instr.ID));
                            this.SetInstrumentToSelection(instr.ID);
                        }
                    }
                }
                if (this.currentChannel == this.currentSong.Channels)
                {
                    // Set active Fx
                    Effect eff = this.currentSong.GetEffect(lastHexVal);
                    if (eff != null)
                    {
                        this.OnSetActiveFx(this, new ActiveFxEventArgs(lastHexVal));
                        this.SetEffectToSelection(lastHexVal);
                    }
                    else
                    {
                        lastHexVal = lastHexVal & 0xF;
                        eff = currentSong.GetEffect(lastHexVal);
                        if (eff != null)
                        {
                            this.OnSetActiveFx(this, new ActiveFxEventArgs(lastHexVal));
                            this.SetEffectToSelection(lastHexVal);
                        }
                    }
                }
            }
            else
            {
                if (e.KeyCode == Keys.R && this.currentChannel != this.currentSong.Channels)
                {
                    this.OnSetActiveInstrument(this, new ActiveInstrumentEventArgs("R"));
                    this.SetInstrumentToSelection("R");
                }
            }
        }

        private void processKeyDownWithCtrlAndShift(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.V:
                    OnPasteAsDelay();
                    break;
                case Keys.Down:
                    DecreaseTempoToSelection();
                    break;
                case Keys.Up:
                    IncreaseTempoToSelection();
                    break;
            }
        }

        public void DecreaseTempoToSelection()
        {
            for (int pos = this.selectionStart; pos <= this.selectionEnd; ++pos)
            {
                ChannelLine line = this.currentPattern.Lines[pos];
                foreach (ChannelNote n in line.Notes)
                {
                    if (n.HasNote || n.HasSeminote)
                    {
                        line.TempoModifier -= 1;
                        break;
                    }
                }
                if (line.TempoModifier < -1) { line.TempoModifier = -1; }
            }
            this.Invalidate();
        }

        public void IncreaseTempoToSelection()
        {
            for (int pos = this.selectionStart; pos <= this.selectionEnd; ++pos)
            {
                ChannelLine line = this.currentPattern.Lines[pos];
                foreach (ChannelNote n in line.Notes)
                {
                    if (n.HasNote || n.HasSeminote)
                    {
                        line.TempoModifier += 1;
                        break;
                    }
                }
                if (line.TempoModifier > 1) { line.TempoModifier = 1; }
            }
            this.Invalidate();
        }

        private void processKeyDownWithCtrlNoShift(KeyEventArgs e)
        {
            if (this.currentChannel != this.currentSong.Channels)
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        for (int chan = this.selectionChanStart; chan <= this.selectionChanEnd; ++chan)
                        {
                            for (int pos = this.selectionStart; pos <= this.selectionEnd; ++pos)
                            {
                                ChannelNote dstNote = this.currentPattern.Lines[pos].Notes[chan];
                                Instrument i = getActiveInstrument(pos, chan);
                                if (dstNote.HasNote && dstNote.Note != 'P' && i != null && i.Volumes != null && i.Volumes.Length > 0)
                                {
                                    dstNote.VolModifier = (sbyte)(dstNote.VolModifier.GetValueOrDefault() + 1);
                                    if (dstNote.VolModifier > 0xF) { dstNote.VolModifier = 0xF; }
                                }
                            }
                        }
                        this.Invalidate();
                        break;
                    case Keys.Down:
                        for (int chan = this.selectionChanStart; chan <= this.selectionChanEnd; ++chan)
                        {
                            for (int pos = this.selectionStart; pos <= this.selectionEnd; ++pos)
                            {
                                ChannelNote dstNote = this.currentPattern.Lines[pos].Notes[chan];
                                Instrument i = getActiveInstrument(pos, chan);
                                if (dstNote.HasNote && dstNote.Note != 'P' && i != null && i.Volumes != null && i.Volumes.Length > 0)
                                {
                                    dstNote.VolModifier = (sbyte)(dstNote.VolModifier.GetValueOrDefault() - 1);
                                    if (dstNote.VolModifier < -15) { dstNote.VolModifier = -15; }
                                }
                            }
                        }
                        this.Invalidate();
                        break;
                }
            }
        }

        private void processKeyDownWithShift(KeyEventArgs e)
        {
            int newIdx;
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (this.selectionStart == this.selectedIndex)
                    {
                        newIdx = this.selectionStart - 1;
                        if (newIdx < 0) newIdx = 0;
                        this.selectionStart = newIdx;
                        this.selectedIndex = newIdx;
                    }
                    else
                    {
                        newIdx = this.selectionEnd - 1;
                        if (newIdx < 0) newIdx = 0;
                        this.selectionEnd = newIdx;
                        this.selectedIndex = newIdx;
                    }
                    if (FirstVisibleNoteIndex > newIdx)
                        FirstVisibleNoteIndex = newIdx;
                    break;
                case Keys.Down:
                    if (this.selectionStart == this.selectedIndex)
                    {
                        newIdx = this.selectionStart + 1;
                        if (newIdx >= this.currentPattern.Length) newIdx = this.currentPattern.Length - 1;
                        this.selectionStart = newIdx;
                        this.selectedIndex = newIdx;
                    }
                    else
                    {
                        newIdx = this.selectionEnd + 1;
                        if (newIdx >= this.currentPattern.Length) newIdx = this.currentPattern.Length - 1;
                        this.selectionEnd = newIdx;
                        this.selectedIndex = newIdx;
                    }
                    if (newIdx > FirstVisibleNoteIndex + visibleNoteCount)
                    {
                        FirstVisibleNoteIndex = newIdx - visibleNoteCount;
                    }
                    break;
                case Keys.PageDown:
                    if (this.selectionStart == this.selectedIndex)
                    {
                        newIdx = this.selectionStart + 10;
                        if (newIdx >= this.currentPattern.Length) newIdx = this.currentPattern.Length - 1;
                        this.selectionStart = newIdx;
                        this.selectedIndex = newIdx;
                    }
                    else
                    {
                        newIdx = this.selectionEnd + 10;
                        if (newIdx >= this.currentPattern.Length) newIdx = this.currentPattern.Length - 1;
                        this.selectionEnd = newIdx;
                        this.selectedIndex = newIdx;
                    }
                    if (newIdx > FirstVisibleNoteIndex + visibleNoteCount)
                    {
                        FirstVisibleNoteIndex = newIdx - visibleNoteCount;
                    }
                    break;
                case Keys.PageUp:
                    if (this.selectionStart == this.selectedIndex)
                    {
                        newIdx = this.selectionStart - 10;
                        if (newIdx < 0) newIdx = 0;
                        this.selectionStart = newIdx;
                        this.selectedIndex = newIdx;
                    }
                    else
                    {
                        newIdx = this.selectionEnd - 10;
                        if (newIdx < 0) newIdx = 0;
                        this.selectionEnd = newIdx;
                        this.selectedIndex = newIdx;
                    }
                    if (FirstVisibleNoteIndex > newIdx)
                        FirstVisibleNoteIndex = newIdx;
                    break;
                case Keys.Left:
                    if (this.selectionChanStart == this.currentChannel)
                    {
                        newIdx = this.selectionChanStart - 1;
                        if (newIdx < 0) newIdx = 0;
                        this.selectionChanStart = newIdx;
                        this.CurrentChannel = newIdx;
                    }
                    else
                    {
                        newIdx = this.selectionChanEnd - 1;
                        if (newIdx < 0) newIdx = 0;
                        this.selectionChanEnd = newIdx;
                        this.CurrentChannel = newIdx;
                    }
                    break;
                case Keys.Right:
                    if (this.selectionChanStart == this.currentChannel)
                    {
                        newIdx = this.selectionChanStart + 1;
                        if (newIdx > this.currentPattern.Channels) newIdx = this.currentPattern.Channels;
                        this.selectionChanStart = newIdx;
                        this.CurrentChannel = newIdx;
                    }
                    else
                    {
                        newIdx = this.selectionChanEnd + 1;
                        if (newIdx >= this.currentPattern.Channels) newIdx = this.currentPattern.Channels;
                        this.selectionChanEnd = newIdx;
                        this.CurrentChannel = newIdx;
                    }
                    break;
                case Keys.Space:
                    setSelectionModifiers(null);
                    break;
                default:
                    int hexVal = hexKeys.IndexOf(e.KeyCode);
                    if (hexVal != -1)
                    {
                        setSelectionModifiers(hexVal);
                    }
                    break;
            }
            if (selectionStart > selectionEnd)
            {
                int tmp = selectionStart;
                selectionStart = selectionEnd;
                selectionEnd = tmp;
            }
            if (selectionChanStart > selectionChanEnd)
            {
                int tmp = selectionChanStart;
                selectionChanStart = selectionChanEnd;
                selectionChanEnd = tmp;
            }
            this.Invalidate();
        }

        private void setSelectionModifiers(int? targetVol)
        {
            for (int chan = this.selectionChanStart; chan <= this.selectionChanEnd; ++chan)
            {
                for (int pos = this.selectionStart; pos <= this.selectionEnd; ++pos)
                {
                    ChannelNote dstNote = this.currentPattern.Lines[pos].Notes[chan];
                    if (targetVol.HasValue)
                    {
                        Instrument i = getActiveInstrument(pos, chan);
                        if (dstNote.HasNote && dstNote.Note != 'P' && i != null && i.Volumes != null && i.Volumes.Length > 0)
                        {
                            dstNote.VolModifier = (sbyte)(targetVol - (i.Volumes[0] & 0xF));
                        }
                    }
                    else
                    {
                        dstNote.VolModifier = null;
                    }
                }
            }
        }

        private void processKeyDownWithoutCtrl(KeyEventArgs e)
        {
            if (e.Alt)
            {
                processKeyDownWithAlt(e);
            }
            else
            {
                processKeyDownWithoutCtrlAltNorShift(e);
            }
        }

        private void processKeyDownWithAlt(KeyEventArgs e)
        {

            switch (e.KeyCode)
            {
                // Up/Down: Change note
                case Keys.Up:
                    {
                        ChannelLine line = this.CurrentPattern.Lines[this.SelectedIndex];
                        if (this.CurrentChannel == this.CurrentPattern.Channels)
                        {
                            line.Fx = this.CurrentSong.Effects.GetNextFx(line.Fx);
                            this.Invalidate();
                        }
                        else
                        {
                            ChannelNote note = line.Notes[this.CurrentChannel];
                            if (note.HasNote)
                            {
                                if (note.HasSeminote || note.Note == 'E' || note.Note == 'B')
                                {
                                    note.Seminote = char.MinValue;
                                    note.Note = (char)(note.Note + 1);
                                    if (note.Note > 'G')
                                    {
                                        note.Note = 'A';
                                    }

                                }
                                else
                                {
                                    note.Seminote = '+';
                                }
                                this.Invalidate();
                            }
                        }
                        break;
                    }
                case Keys.Down:
                    {
                        ChannelLine line = this.CurrentPattern.Lines[this.SelectedIndex];
                        if (this.CurrentChannel == this.CurrentPattern.Channels)
                        {
                            line.Fx = this.CurrentSong.Effects.GetPreviousFx(line.Fx);
                            this.Invalidate();
                        }
                        else
                        {
                            ChannelNote note = line.Notes[this.CurrentChannel];
                            if (note.HasNote)
                            {
                                if (note.HasSeminote)
                                {
                                    note.Seminote = char.MinValue;
                                }
                                else {
                                    note.Note = (char)(note.Note - 1);
                                    if (note.Note < 'A')
                                    {
                                        note.Note = 'G';
                                    }
                                    if(note.Note != 'E' && note.Note != 'B')
                                    {
                                        note.Seminote = '+';
                                    }
                                }
                            }
                            this.Invalidate();
                        }
                        break;
                    }
                // Left/right: Change octave
                case Keys.Left:
                    {
                        ChannelLine line = this.CurrentPattern.Lines[this.SelectedIndex];
                        if (this.CurrentChannel != this.CurrentPattern.Channels)
                        {
                            ChannelNote note = line.Notes[this.CurrentChannel];
                            if (note.HasOctave)
                            {
                                note.Octave = note.Octave - 1;
                                if (note.Octave < 2)
                                {
                                    note.Octave = 2;
                                }
                            }
                            this.Invalidate();
                        }
                        break;
                    }
                case Keys.Right:
                    {
                        ChannelLine line = this.CurrentPattern.Lines[this.SelectedIndex];
                        if (this.CurrentChannel != this.CurrentPattern.Channels)
                        {
                            ChannelNote note = line.Notes[this.CurrentChannel];
                            if (note.HasOctave)
                            {
                                note.Octave = note.Octave + 1;
                                if (note.Octave > 8)
                                {
                                    note.Octave = 8;
                                }
                            }
                            this.Invalidate();
                        }
                        break;
                    }
            }
        }

        private void processKeyDownWithoutCtrlAltNorShift(KeyEventArgs e)
        {
            int newIdx;
            switch (e.KeyCode)
            {
                case Keys.Up:
                    newIdx = this.selectedIndex - 1;
                    if (newIdx < 0)
                        newIdx = 0;
                    this.SelectedIndex = newIdx;
                    clearMultipleSelection();
                    if (FirstVisibleNoteIndex > newIdx)
                        FirstVisibleNoteIndex = newIdx;
                    this.Invalidate();
                    break;
                case Keys.Down:
                    newIdx = this.selectedIndex + 1;
                    if (newIdx > this.currentPattern.Lines.Length - 1)
                        newIdx = this.currentPattern.Lines.Length - 1;
                    this.SelectedIndex = newIdx;
                    clearMultipleSelection();
                    if (newIdx > FirstVisibleNoteIndex + visibleNoteCount)
                    {
                        FirstVisibleNoteIndex = newIdx - visibleNoteCount;
                        this.Invalidate();
                    }
                    break;
                case Keys.PageDown:
                    newIdx = this.selectedIndex + 10;
                    if (newIdx > this.currentPattern.Lines.Length - 1)
                    {
                        newIdx = this.currentPattern.Lines.Length - 1;
                    }
                    this.SelectedIndex = newIdx;
                    clearMultipleSelection();
                    if (newIdx > FirstVisibleNoteIndex + visibleNoteCount)
                    {
                        FirstVisibleNoteIndex = newIdx - visibleNoteCount;
                        this.Invalidate();
                    }
                    break;
                case Keys.PageUp:
                    newIdx = this.selectedIndex - 10;
                    if (newIdx < 0)
                    {
                        newIdx = 0;
                    }
                    this.SelectedIndex = newIdx;
                    clearMultipleSelection();
                    if (FirstVisibleNoteIndex > newIdx)
                        FirstVisibleNoteIndex = newIdx;
                    this.Invalidate();
                    break;
                case Keys.Left:
                    if (this.currentChannel > 0)
                    {
                        this.CurrentChannel--;
                    }
                    clearMultipleSelection();
                    this.Invalidate();
                    break;
                case Keys.Right:
                    if (this.currentChannel < this.currentPattern.Channels)
                    {
                        this.CurrentChannel++;
                    }
                    clearMultipleSelection();
                    this.Invalidate();
                    break;
                case Keys.F8:
                    // quitar el instrumento a la nota actual.
                    if (this.currentChannel < this.CurrentSong.Channels)
                    {
                        ChannelNote dstNote = this.currentPattern.Lines[this.SelectedIndex].Notes[this.currentChannel];
                        dstNote.Instrument = string.Empty;
                        increasePosition();
                        this.Invalidate();
                    }
                    break;
                case Keys.Delete:
                case Keys.Back:
                    deleteNotesInSelection();
                    break;
                default:
                    break;
            }
        }

        private void clearMultipleSelection()
        {
            this.selectionChanStart = this.currentChannel;
            this.selectionChanEnd = this.selectionChanStart;
            this.selectionStart = this.selectedIndex;
            this.selectionEnd = this.selectionStart;
        }

        void virtPiano_NoteFxPressed(object sender, VirtualPiano.NoteFXEventArgs e)
        {
            clearMultipleSelection();
            if (e.Note != null && this.currentChannel < this.currentSong.Channels)
            {
                setNote(e.Note);
            }
            if (e.Fx != int.MinValue && this.currentChannel == this.currentSong.Channels)
            {
                setFx(e.Fx);
            }
        }

        private void setFx(int fx)
        {
            this.currentPattern.Lines[this.SelectedIndex].Fx = fx;
            increasePosition();
            this.Invalidate();
        }

        private void setNote(ChannelNote pressedNote)
        {
            ChannelNote dstNote = this.currentPattern.Lines[this.SelectedIndex].Notes[this.currentChannel];
            ChannelNote lastNoteWithInstrument = null;

            dstNote.Octave = pressedNote.Octave;
            dstNote.Note = pressedNote.Note;
            dstNote.Seminote = pressedNote.Seminote;

            if ((pressedNote.HasNote || pressedNote.HasSeminote || pressedNote.HasOctave) && (pressedNote.Note != 'P'))
            {
                string previousInstr = string.Empty;

                lastNoteWithInstrument = this.getLastNoteWithInstrument(this.SelectedIndex, this.currentChannel);
                if (lastNoteWithInstrument != null)
                {
                    previousInstr = lastNoteWithInstrument.Instrument;
                }
                if (this.CurrentInstrument.ID.ToString() != previousInstr)
                {
                    dstNote.Instrument = this.currentInstrument.ID.ToString();
                }
                else
                {
                    dstNote.Instrument = string.Empty;
                }

                if (this.currentInstrument.ID == "R")
                {
                    if (lastNoteWithInstrument != null && lastNoteWithInstrument.Instrument == "R")
                    {
                        if (!EnvelopeData.Compare(ApplicationState.Instance.CurrentEnvData, lastNoteWithInstrument.EnvData))
                        {
                            dstNote.Instrument = "R";
                        }
                    }

                    dstNote.EnvData.ActiveFrequencies = ApplicationState.Instance.CurrentEnvData.ActiveFrequencies;
                    dstNote.EnvData.FrequencyRatio = ApplicationState.Instance.CurrentEnvData.FrequencyRatio;
                    dstNote.EnvData.Style = ApplicationState.Instance.CurrentEnvData.Style;
                }
            }
            else
            {
                dstNote.Instrument = string.Empty;
            }
            increasePosition();
            this.Invalidate();
        }

        private Instrument getActiveInstrument(int currentIndex, int channel)
        {
            string instrumentId = null;
            ChannelNote currentNote = this.currentPattern.Lines[currentIndex].Notes[channel];
            if (currentNote.HasInstrument)
            {
                instrumentId = currentNote.Instrument;
            }
            else
            {
                ChannelNote lastNoteWithInstrument = getLastNoteWithInstrument(currentIndex, channel);
                if (lastNoteWithInstrument != null)
                {
                    instrumentId = lastNoteWithInstrument.Instrument;
                }
            }
            return this.currentSong.GetInstrument(instrumentId);
        }

        private ChannelNote getLastNoteWithInstrument(int currentIndex, int channel)
        {
            ChannelNote lastNoteWithInstrument = null;
            for (int pos = currentIndex - 1; pos >= 0; pos--)
            {
                ChannelNote tmpNote = this.currentPattern.Lines[pos].Notes[channel];
                if (tmpNote.HasInstrument)
                {
                    lastNoteWithInstrument = tmpNote;
                    break;
                }
            }
            return lastNoteWithInstrument;
        }

        private void increasePosition()
        {
            int newIndex = this.selectedIndex + this.editionIncrement;
            if (newIndex >= this.currentPattern.Length)
            {
                newIndex = this.currentPattern.Length - 1;
            }
            this.selectedIndex = newIndex;
            this.selectionStart = newIndex;
            this.selectionEnd = newIndex;

            if (newIndex > FirstVisibleNoteIndex + visibleNoteCount)
            {
                FirstVisibleNoteIndex = newIndex - visibleNoteCount;
                this.Invalidate();
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (!e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        break;
                    case Keys.Down:
                        break;
                    case Keys.Left:
                        break;
                    case Keys.Right:
                        break;
                    case Keys.Return:
                        if (e.Shift)
                        {
                            this.OnPlay(this, new PlayEventArgs(PlayMode.SinglePattern));
                        }
                        else
                        {
                            this.OnPlay(this, new PlayEventArgs(PlayMode.FullSong));
                        }
                        break;
                    case Keys.Escape:
                        this.OnStop(this, EventArgs.Empty);
                        break;
                    case Keys.F2:
                        this.OnDecreaseOctave(this, EventArgs.Empty);
                        break;
                    case Keys.F3:
                        this.OnIncreaseOctave(this, EventArgs.Empty);
                        break;
                    case Keys.F4:
                        if (this.editionIncrement > 0)
                        {
                            this.EditionIncrement--;
                        }
                        break;
                    case Keys.F5:
                        if (this.editionIncrement < this.currentPattern.Length - 1)
                        {
                            this.EditionIncrement++;
                        }
                        break;
                    case Keys.F6:
                        // Instrumento anterior
                        this.OnDecreaseInstrument(this, EventArgs.Empty);
                        break;
                    case Keys.F7:
                        // Instrumento siguiente.
                        this.OnIncreaseInstrument(this, EventArgs.Empty);
                        break;
                    case Keys.End:
                        this.SelectedIndex = this.currentPattern.Length - 1;
                        break;
                    case Keys.Home:
                        this.SelectedIndex = 0;
                        break;
                    default:
                        break;
                }
            }
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
            {
                temp(sender, eventArgs);
            }
        }

        protected void OnSetActiveInstrument(object sender, ActiveInstrumentEventArgs eventArgs)
        {
            EventHandler<ActiveInstrumentEventArgs> temp = SetActiveInstrument;
            if (temp != null)
            {
                temp(sender, eventArgs);
            }
        }

        public void OnPaste()
        {
            int channels;
            if (this.clipboardBuffer.Length > 0)
            {
                if (this.clipboardBufferHasFx)
                {
                    channels = this.clipboardBuffer[0].Channels + 1;
                }
                else
                {
                    channels = this.clipboardBuffer[0].Channels;
                }
                for (int i = 0; i < this.clipboardBuffer.Length; i++)
                {
                    for (int j = 0; j < channels; j++)
                    {
                        if (selectedIndex + i < this.currentPattern.Length)
                        {
                            if (this.currentSong.Channels == this.currentChannel + j && clipboardBufferHasFx)
                            {
                                this.currentPattern.Lines[selectedIndex + i].Fx = clipboardBuffer[i].Fx;
                            }
                            else
                            {
                                if (this.currentSong.Channels > this.currentChannel + j)
                                {
                                    if (clipboardBuffer[i].Notes.Count > j)
                                    {
                                        ChannelNote note = this.currentPattern.Lines[selectedIndex + i].Notes[this.currentChannel + j];
                                        ChannelNote clipBoardNote = clipboardBuffer[i].Notes[j];

                                        note.Instrument = clipBoardNote.Instrument;
                                        note.Note = clipBoardNote.Note;
                                        note.Octave = clipBoardNote.Octave;
                                        note.Seminote = clipBoardNote.Seminote;
                                        note.VolModifier = clipBoardNote.VolModifier;

                                        note.EnvData.ActiveFrequencies = clipBoardNote.EnvData.ActiveFrequencies;
                                        note.EnvData.FrequencyRatio = clipBoardNote.EnvData.FrequencyRatio;
                                        note.EnvData.Style = clipBoardNote.EnvData.Style;
                                    }
                                }
                            }
                        }
                    }
                }
                this.selectionEnd = this.selectionStart + this.clipboardBuffer.Length - 1;
                if (clipboardBuffer[0].Channels > 0)
                {
                    this.selectionChanEnd = this.selectionChanStart + this.clipboardBuffer[0].Channels - 1;
                    if (this.clipboardBufferHasFx) this.selectionChanEnd++;
                }
            }
            this.Invalidate();
        }

        internal void OnCopy()
        {
            this.clipboardBuffer = getSelectedBuffer(out clipboardBufferHasFx);
            this.Invalidate();
        }

        private ChannelLine[] getSelectedBuffer(out bool containsFx)
        {
            ChannelLine[] result = new ChannelLine[this.selectionEnd - this.selectionStart + 1];
            containsFx = false;
            int lineIdx = this.selectionStart;
            for (int i = 0; i < result.Length; i++)
            {
                if (this.selectionChanEnd >= this.CurrentSong.Channels)
                {
                    result[i] = new ChannelLine((byte)(this.selectionChanEnd - this.selectionChanStart));
                }
                else
                {
                    result[i] = new ChannelLine((byte)(this.selectionChanEnd - this.selectionChanStart + 1));
                }
                int chanIdx = this.selectionChanStart;
                for (int j = 0; j < this.selectionChanEnd - this.selectionChanStart + 1; j++)
                {
                    if (this.currentSong.Channels <= chanIdx)
                    {
                        result[i].Fx = this.currentPattern.Lines[lineIdx].Fx;
                        containsFx = true;
                    }
                    else
                    {
                        result[i].Notes[j] = cloneNote(this.currentPattern.Lines[lineIdx].Notes[chanIdx]);
                    }
                    chanIdx++;
                }
                lineIdx++;
            }
            return result;
        }

        public void OnPasteAsDelay()
        {
            int channels;
            if (this.clipboardBuffer.Length > 0)
            {
                if (this.clipboardBufferHasFx)
                {
                    channels = this.clipboardBuffer[0].Channels + 1;
                }
                else
                {
                    channels = this.clipboardBuffer[0].Channels;
                }

                // Ponemos instrumentos a todas las notas.
                for (int j = 0; j < channels; j++)
                {
                    string instrId = string.Empty;
                    string clipInstrId = string.Empty;

                    for (int i = 0; i < this.clipboardBuffer.Length; i++)
                    {
                        int line = selectedIndex + i;
                        int chan = this.currentChannel + j;
                        if (line < this.currentPattern.Length)
                        {
                            ChannelNote note = this.currentPattern.Lines[line].Notes[chan];
                            if (note.HasNote && !note.HasInstrument)
                            {
                                if (instrId == string.Empty)
                                {
                                    Instrument activeInstrument = getActiveInstrument(line, chan);
                                    if (activeInstrument != null)
                                    {
                                        instrId = activeInstrument.ID;
                                    }
                                }
                                note.Instrument = instrId;
                            }
                            else if (note.HasInstrument)
                            {
                                instrId = note.Instrument;
                            }

                            ChannelNote clipBoardNote = clipboardBuffer[i].Notes[j];
                            if (clipBoardNote.HasNote && !clipBoardNote.HasInstrument)
                            {
                                if (clipInstrId == string.Empty)
                                {
                                    for (int clipLine = i - 1; i >= 0; i--)
                                    {
                                        ChannelNote otherClipNote = clipboardBuffer[clipLine].Notes[j];
                                        if (!string.IsNullOrEmpty(otherClipNote.Instrument))
                                        {
                                            clipInstrId = otherClipNote.Instrument;
                                            break;
                                        }
                                    }
                                }
                                clipBoardNote.Instrument = clipInstrId;
                            }
                            else if (clipBoardNote.HasInstrument)
                            {
                                clipInstrId = clipBoardNote.Instrument;
                            }
                        }
                    }
                }

                for (int i = 0; i < this.clipboardBuffer.Length; i++)
                {
                    for (int j = 0; j < channels; j++)
                    {
                        if (selectedIndex + i < this.currentPattern.Length)
                        {
                            if (this.currentSong.Channels == this.currentChannel + j && clipboardBufferHasFx)
                            {
                                if (this.currentPattern.Lines[selectedIndex + i].Fx != int.MinValue)
                                {
                                    this.currentPattern.Lines[selectedIndex + i].Fx = clipboardBuffer[i].Fx;
                                }
                            }
                            else
                            {
                                if (this.currentSong.Channels > this.currentChannel + j)
                                {
                                    if (clipboardBuffer[i].Notes.Count > j)
                                    {
                                        ChannelNote note = this.currentPattern.Lines[selectedIndex + i].Notes[this.currentChannel + j];
                                        ChannelNote clipBoardNote = clipboardBuffer[i].Notes[j];

                                        if (!note.HasNote)
                                        {
                                            note.Instrument = clipBoardNote.Instrument;
                                            note.Note = clipBoardNote.Note;
                                            note.Octave = clipBoardNote.Octave;
                                            note.Seminote = clipBoardNote.Seminote;
                                            if (clipBoardNote.HasNote)
                                            {
                                                if (clipBoardNote.Note != 'P')
                                                {
                                                    note.VolModifier = (sbyte)((int)(clipBoardNote.VolModifier.GetValueOrDefault() - DelayDecrement) & 0xFF);
                                                }
                                                else
                                                {
                                                    note.VolModifier = null;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (!note.VolModifier.HasValue && DelayDecrement != 0)
                                            {
                                                note.VolModifier = 0;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // Limpiamos instrumentos sobrantes:
                for (int j = 0; j < channels; j++)
                {
                    string instrumentId = string.Empty;
                    for (int i = 0; i < this.clipboardBuffer.Length; i++)
                    {
                        int line = selectedIndex + i;
                        int chan = this.currentChannel + j;
                        if (line < this.currentPattern.Length)
                        {
                            ChannelNote note = this.currentPattern.Lines[line].Notes[chan];
                            if (instrumentId == string.Empty)
                            {
                                ChannelNote lastWithInstrument = getLastNoteWithInstrument(line, chan);
                                if (lastWithInstrument != null)
                                {
                                    instrumentId = lastWithInstrument.Instrument;
                                }
                            }
                            if (note.HasInstrument && note.Instrument == instrumentId)
                            {
                                note.Instrument = string.Empty;
                            }
                            if (note.HasInstrument)
                            {
                                instrumentId = note.Instrument;
                            }
                        }
                    }
                }

                this.selectionEnd = this.selectionStart + this.clipboardBuffer.Length - 1;
                if (clipboardBuffer[0].Channels > 0)
                {
                    this.selectionChanEnd = this.selectionChanStart + this.clipboardBuffer[0].Channels - 1;
                    if (this.clipboardBufferHasFx) this.selectionChanEnd++;
                }
            }
            this.Invalidate();
        }

        private void deleteNotesInSelection()
        {
            for (int chanIdx = selectionChanStart; chanIdx <= this.selectionChanEnd; chanIdx++)
            {
                for (int lineIdx = selectionStart; lineIdx <= this.selectionEnd; lineIdx++)
                {
                    if (lineIdx >= 0 && lineIdx < this.currentPattern.Length)
                    {
                        clearNote(lineIdx, chanIdx);
                    }
                }
            }
            clearNote(this.SelectedIndex, this.currentChannel);
            this.Invalidate();
        }

        internal void OnCut()
        {
            this.clipboardBuffer = getSelectedBuffer(out this.clipboardBufferHasFx);

            int lineIdx = this.selectionStart;
            for (int i = 0; i < this.clipboardBuffer.Length; i++)
            {
                int chanIdx = this.selectionChanStart;
                for (int j = 0; j < this.selectionChanEnd - this.selectionChanStart + 1; j++)
                {
                    clearNote(lineIdx, chanIdx);
                    chanIdx++;
                }
                lineIdx++;
            }
            this.Invalidate();
        }

        private void clearNote(int lineIdx, int chanIdx)
        {
            ChannelLine line = this.currentPattern.Lines[lineIdx];
            if (this.currentSong.Channels > chanIdx)
            {
                ChannelNote currentNote = line.Notes[chanIdx];
                currentNote.Instrument = "";
                currentNote.Note = char.MinValue;
                currentNote.Octave = int.MinValue;
                currentNote.Seminote = char.MinValue;
                currentNote.VolModifier = null;
            }
            else
            {
                line.Fx = int.MinValue;
            }
            bool notesRemaining = false;
            for (int i = 0; i < this.currentSong.Channels; ++i)
            {
                if (line.Notes[i].HasValue)
                {
                    notesRemaining = true;
                    break;
                }
            }
            if (!notesRemaining)
            {
                if (line.Fx == int.MinValue)
                {
                    line.TempoModifier = 0;
                }
            }
        }

        private ChannelNote cloneNote(ChannelNote note)
        {
            ChannelNote newNote = new ChannelNote();
            newNote.Instrument = note.Instrument;
            newNote.Note = note.Note;
            newNote.Octave = note.Octave;
            newNote.Seminote = note.Seminote;
            newNote.EnvData = new EnvelopeData()
            {
                ActiveFrequencies = note.EnvData.ActiveFrequencies,
                FrequencyRatio = note.EnvData.FrequencyRatio,
                Style = note.EnvData.Style
            };
            newNote.VolModifier = note.VolModifier;
            return newNote;
        }

        internal void Transpose(int semitones)
        {
            int currentOctave = -1;
            int lineIdx = this.selectionStart;
            for (int i = 0; i < this.selectionEnd - selectionStart + 1; i++)
            {
                int chanIdx = this.selectionChanStart;
                for (int j = 0; j < this.selectionChanEnd - this.selectionChanStart + 1; j++)
                {
                    if (this.currentSong.Channels > chanIdx)
                    {
                        ChannelNote currentNote = this.currentPattern.Lines[lineIdx].Notes[chanIdx];
                        if (currentNote.HasOctave)
                        {
                            currentOctave = currentNote.Octave;
                        }
                        currentNote.Transpose(semitones, currentOctave);
                    }
                    chanIdx++;
                }
                lineIdx++;
            }
            this.Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            virtPiano.Dispose();
            base.Dispose(disposing);
        }

        public void SetInstrumentToSelection(string instrumentId)
        {
            for (int chanIdx = selectionChanStart; chanIdx <= this.selectionChanEnd; chanIdx++)
            {
                for (int lineIdx = selectionStart; lineIdx <= this.selectionEnd; lineIdx++)
                {
                    if (this.currentSong.Channels > chanIdx)
                    {
                        ChannelNote currentNote = this.currentPattern.Lines[lineIdx].Notes[chanIdx];

                        currentNote.Instrument = instrumentId;
                        if (instrumentId == "R")
                        {
                            currentNote.EnvData.ActiveFrequencies = ApplicationState.Instance.CurrentEnvData.ActiveFrequencies;
                            currentNote.EnvData.FrequencyRatio = ApplicationState.Instance.CurrentEnvData.FrequencyRatio;
                            currentNote.EnvData.Style = ApplicationState.Instance.CurrentEnvData.Style;
                        }
                    }
                }
            }
            this.Invalidate();
        }

        public void SetEffectToSelection(int effectId)
        {
            for (int chanIdx = selectionChanStart; chanIdx <= this.selectionChanEnd; chanIdx++)
            {
                for (int lineIdx = selectionStart; lineIdx <= this.selectionEnd; lineIdx++)
                {
                    if (this.currentSong.Channels > chanIdx)
                    {
                        if (this.currentPattern.Lines[lineIdx].Fx != int.MinValue)
                        {
                            this.currentPattern.Lines[lineIdx].Fx = effectId;
                        }
                    }
                }
            }
            this.Invalidate();
        }

        public void IncreaseOctaveToSelection()
        {
            changeOctavesToSelection(1);
        }

        public void DecreaseOctaveToSelection()
        {
            changeOctavesToSelection(-1);
        }

        private void changeOctavesToSelection(int offset)
        {
            int[] currentOctaves = new int[this.currentSong.Channels];

            for (int chanIdx = selectionChanStart; chanIdx <= this.selectionChanEnd; chanIdx++)
            {
                for (int lineIdx = 0; lineIdx <= this.selectionEnd; lineIdx++)
                {
                    if (this.currentSong.Channels > chanIdx)
                    {
                        ChannelNote currentNote = this.currentPattern.Lines[lineIdx].Notes[chanIdx];

                        if (currentNote.HasValue)
                        {
                            if (currentNote.HasOctave)
                            {
                                currentOctaves[chanIdx] = currentNote.Octave;
                            }
                            currentNote.Octave = currentOctaves[chanIdx] + offset;

                            if (currentNote.Octave > MAX_OCTAVE)
                            {
                                currentNote.Octave = MAX_OCTAVE;
                            }
                            if (currentNote.Octave < MIN_OCTAVE)
                            {
                                currentNote.Octave = MIN_OCTAVE;
                            }
                        }
                    }
                }
            }
            this.Invalidate();
        }

        public void UpdateFont()
        {
            if (Properties.Settings.Default.UseCustomFont && pfc.Families != null && pfc.Families.Length > 0)
            {
                this.Font = new Font(pfc.Families[0], Properties.Settings.Default.FontSize, FontStyle.Regular);
            }
            else
            {
                this.Font = new Font(new FontFamily("Courier New"), Properties.Settings.Default.FontSize - 2, FontStyle.Regular);
            }
        }
    }
}
