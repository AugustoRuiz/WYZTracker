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
    public partial class EffectEditor : Form, ISongConsumer
    {
        public event EventHandler EffectChanged;

        private int oldFxId;

        private VirtualPiano virtPiano;

        private bool loadingControls = false;

        static double[] vf_array;
        static double[] v1f_array;
        static double[] vff_array;

        private List<Slider> volumeSliders = new List<Slider>();
        private List<Slider> freqLoSliders = new List<Slider>();
        private List<Slider> freqHiSliders = new List<Slider>();
        private List<Slider> noiseSliders = new List<Slider>();
        private List<Slider> envFreqLoSliders = new List<Slider>();
        private List<Slider> envFreqHiSliders = new List<Slider>();

        private List<PictureBox> pbEnvTypes = new List<PictureBox>();
        private List<Label> labels = new List<Label>();

        private int sliderWidth = 24;

        static EffectEditor()
        {
            vf_array = new double[17];
            vff_array = new double[17];
            for (int i = 0; i < 17; ++i)
            {
                vf_array[i] = i;
                vff_array[i] = 16 * i;
            }
            v1f_array = new double[33];
            for (int i = 0; i < 33; ++i)
            {
                v1f_array[i] = i;
            }
        }

        private int _selectedIndex;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (value != _selectedIndex)
                {
                    this._selectedIndex = value;
                    this.updateSelection();
                }
            }
        }

        public EffectEditor()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.virtPiano = new VirtualPiano(this);
            this.virtPiano.Mode = VirtualPiano.PianoMode.Fx;
            this.virtPiano.Enabled = chkTesting.Checked;
            this.virtPiano.NoteFxPressed += OnNoteOrFxPressed;
        }

        private Song currentSong;

        public Song CurrentSong
        {
            get { return currentSong; }
            set
            {
                currentSong = value;
                loadEffectsCombo();
            }
        }

        public Effect CurrentEffect
        {
            get
            {
                return (Effect)this.effectsBindingSource.Current;
            }
            set
            {
                this.effectsBindingSource.Position = this.effectsBindingSource.IndexOf(value);
            }
        }

        private void loadControls()
        {
            this.loadingControls = true;

            this.txtName.Text = this.CurrentEffect.Name;
            this.numId.Value = this.CurrentEffect.ID;
            this.numLength.Value = this.CurrentEffect.Volumes.Length;

            this.reloadEditPanel();

            this.loadingControls = false;

            this.pnlEditorBars.Invalidate();
        }

        private void reloadEditPanel()
        {
            FocusablePanel thePanel = this.pnlEditorBars;

            clearSliders();

            int x = 0;
            for (int i = 0, li = this.CurrentEffect.Volumes.Length; i < li; ++i)
            {
                Panel groupPanel = new Panel();
                int innerX = 2;

                groupPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
                groupPanel.Left = x;
                groupPanel.Top = 0;
                groupPanel.Height = thePanel.ClientRectangle.Height;
                groupPanel.Width = 6 * sliderWidth + 4;
                groupPanel.Click += GroupPanel_Click;

                this.pbEnvTypes.Add(
                    createPbEnvType(innerX,
                        this.CurrentEffect.Noises[i],
                        this.CurrentEffect.EnvTypes[i],
                        this.showEnvTypes,
                        groupPanel
                    )
                );

                this.labels.Add(
                    createLabel("V",
                        Color.Lime,
                        Color.Transparent,
                        innerX,
                        32,
                        12,
                        sliderWidth,
                        ContentAlignment.MiddleCenter,
                        groupPanel)
                );

                this.volumeSliders.Add(
                    createSlider(innerX,
                        0, 0x0F,
                        this.CurrentEffect.Volumes[i],
                        Color.Lime,
                        this.updateVolume,
                        vf_array,
                        groupPanel
                    )
                );
                innerX += sliderWidth;

                this.labels.Add(
                    createLabel("F",
                        Color.Yellow,
                        Color.Transparent,
                        innerX,
                        32,
                        12,
                        2 * sliderWidth,
                        ContentAlignment.MiddleCenter,
                        groupPanel)
                );

                this.freqHiSliders.Add(
                    createSlider(innerX, 0, 0x0F,
                        this.CurrentEffect.Frequencies[i] >> 8,
                        Color.Yellow,
                        this.updateFrequency,
                        vf_array,
                        groupPanel
                    )
                );
                innerX += sliderWidth;

                this.freqLoSliders.Add(
                    createSlider(innerX, 0, 0xFF,
                        this.CurrentEffect.Frequencies[i] & 0xFF,
                        Color.Yellow,
                        this.updateFrequency,
                        vff_array,
                        groupPanel
                    )
                );
                innerX += sliderWidth;

                this.labels.Add(
                    createLabel("N",
                        Color.Red,
                        Color.Transparent,
                        innerX,
                        32,
                        12,
                        sliderWidth,
                        ContentAlignment.MiddleCenter,
                        groupPanel)
                );

                this.noiseSliders.Add(
                    createSlider(innerX, 0, 0x1F,
                        this.CurrentEffect.Noises[i] & 0x7F, Color.Red,
                        this.updateNoise,
                        v1f_array,
                        groupPanel
                    )
                );
                innerX += sliderWidth;

                this.labels.Add(
                    createLabel("E",
                        Color.SlateGray,
                        Color.Transparent,
                        innerX,
                        32,
                        12,
                        2 * sliderWidth,
                        ContentAlignment.MiddleCenter,
                        groupPanel)
                );

                this.envFreqHiSliders.Add(
                    createSlider(innerX, 0, 0x0F,
                        this.CurrentEffect.EnvFreqs[i] >> 8, Color.SlateGray,
                        this.updateEnvFreq,
                        vf_array,
                        groupPanel
                    )
                );
                innerX += sliderWidth;
                this.envFreqLoSliders.Add(
                    createSlider(innerX, 0, 0xFF,
                        this.CurrentEffect.EnvFreqs[i] & 0xFF, Color.SlateGray,
                        this.updateEnvFreq,
                        vff_array,
                        groupPanel
                    )
                );

                innerX += sliderWidth;
                x += groupPanel.Width;

                thePanel.Controls.Add(groupPanel);
            }

            this.pnlEditorBars.SuspendLayout();

            this.updateSelection();

            this.pnlEditorBars.ResumeLayout();
        }

        private Label createLabel(string text, Color foreColor, Color backColor, int x, int y, int height, int width, ContentAlignment alignment, Control parent)
        {
            Label result = new Label() { Text = text, ForeColor = foreColor, BackColor = backColor, Left = x, Top = y, Height = height, Width = width, TextAlign = alignment };
            result.Click += lblSlider_Click;
            parent.Controls.Add(result);

            return result;
        }

        private void lblSlider_Click(object sender, EventArgs e)
        {
            Label source = sender as Label;
            if (source != null && this.pnlEditorBars.Controls.Count > 0)
            {
                Panel container = source.Parent as Panel;
                this.SelectedIndex = this.pnlEditorBars.Controls.IndexOf(container);
            }
        }

        private void GroupPanel_Click(object sender, EventArgs e)
        {
            Control ctl = sender as Control;
            if (ctl != null)
            {
                this.SelectedIndex = this.pnlEditorBars.Controls.IndexOf(ctl);
            }
            this.pnlEditorBars.Focus();
        }

        private void clearSliders()
        {
            foreach (Slider s in volumeSliders) { s.PropertyChanged -= this.updateVolume; }
            foreach (Slider s in freqHiSliders) { s.PropertyChanged -= this.updateFrequency; }
            foreach (Slider s in freqLoSliders) { s.PropertyChanged -= this.updateFrequency; }
            foreach (Slider s in noiseSliders) { s.PropertyChanged -= this.updateNoise; }
            foreach (Slider s in envFreqHiSliders) { s.PropertyChanged -= this.updateEnvFreq; }
            foreach (Slider s in envFreqLoSliders) { s.PropertyChanged -= this.updateEnvFreq; }
            foreach (PictureBox pb in pbEnvTypes) { pb.Click -= this.showEnvTypes; }
            foreach (Label l in labels) { l.Click -= this.lblSlider_Click; }
            this.volumeSliders.Clear();
            this.freqHiSliders.Clear();
            this.freqLoSliders.Clear();
            this.noiseSliders.Clear();
            this.envFreqHiSliders.Clear();
            this.envFreqLoSliders.Clear();
            this.pbEnvTypes.Clear();
            this.labels.Clear();
            this.pnlEditorBars.Controls.Clear();
        }

        private PictureBox createPbEnvType(int posX, byte noise, byte envType, EventHandler clickHandler, Panel panel)
        {
            PictureBox result = new PictureBox();
            result.BackColor = Color.LightGray;
            result.BorderStyle = BorderStyle.FixedSingle;
            result.Click += clickHandler;
            result.BackgroundImageLayout = ImageLayout.Center;
            result.Width = 24;
            result.Height = 24;
            result.Left = posX + (3 * sliderWidth) - 12;
            result.Top = 0;
            if ((noise & 0x80) == 0x80)
            {
                Envelopes envValue = (Envelopes)envType;
                ToolStripMenuItem item = getItemByTagValue((int)envValue);
                tipCtl.SetToolTip(result, item == null ? "N/A" : item.Text);
                result.BackgroundImage =
                    WYZTracker.Properties.Resources.ResourceManager.GetObject(envValue.ToString()) as Image;
            }
            else
            {
                result.BackgroundImage = WYZTracker.Properties.Resources.env_none;
                tipCtl.SetToolTip(result, "No Envelope");
            }
            panel.Controls.Add(result);
            return result;
        }

        private void showEnvTypes(object source, EventArgs args)
        {
            PictureBox pb = source as PictureBox;
            if (pb != null)
            {
                this.SelectedIndex = pbEnvTypes.IndexOf(pb);
                envelopeContextMenu.Show(pb, 0, 0);
            }
        }

        private Slider createSlider(int posX,
                                    int min, int max, int value,
                                    Color color, PropertyChangedEventHandler valueChangedHandler,
                                    double[] steps,
                                    Panel panel)
        {
            Slider newCtl = new Slider();

            newCtl.Orientation = Orientation.Vertical;

            newCtl.Top = 48;
            newCtl.Left = posX;
            newCtl.Height = pnlEditorBars.ClientRectangle.Height - 48; ;
            newCtl.Width = sliderWidth;
            newCtl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom;

            newCtl.BackColor = Color.Black;
            newCtl.ForeColor = color;
            newCtl.PaintValue = true;
            newCtl.Hex = chkHex.Checked;

            newCtl.Logarithmic = false;
            newCtl.Minimum = 0;
            newCtl.Maximum = max;
            newCtl.Value = value;

            newCtl.Steps = steps;
            newCtl.StepColor = Color.Snow;

            newCtl.PropertyChanged += valueChangedHandler;
            panel.Controls.Add(newCtl);

            return newCtl;
        }

        private void updateVolume(object source, PropertyChangedEventArgs args)
        {
            Slider s = source as Slider;
            if (s != null && args.PropertyName == "Value")
            {
                int idx = this.volumeSliders.IndexOf(s);
                this.CurrentEffect.Volumes[idx] = (byte)s.Value;
                this.SelectedIndex = idx;
            }
            this.pnlEditorBars.Focus();
        }

        private void updateFrequency(object source, PropertyChangedEventArgs args)
        {
            Slider s = source as Slider;
            if (s != null && args.PropertyName == "Value")
            {
                int idx = this.freqHiSliders.IndexOf(s);
                if (idx > -1)
                {
                    int currentFreq = this.CurrentEffect.Frequencies[idx];
                    int hiFreq = (((int)s.Value) & 0xF) << 8;
                    currentFreq = (currentFreq & 0xFF) | hiFreq;
                    this.CurrentEffect.Frequencies[idx] = currentFreq;
                    this.SelectedIndex = idx;
                }
                idx = this.freqLoSliders.IndexOf(s);
                if (idx > -1)
                {
                    int currentFreq = this.CurrentEffect.Frequencies[idx] & 0xF00;
                    int loFreq = (int)s.Value;
                    currentFreq = currentFreq | loFreq;
                    this.CurrentEffect.Frequencies[idx] = currentFreq;
                    this.SelectedIndex = idx;
                }
            }
            this.pnlEditorBars.Focus();
        }

        private void updateNoise(object source, PropertyChangedEventArgs args)
        {
            Slider s = source as Slider;
            if (s != null && args.PropertyName == "Value")
            {
                int idx = this.noiseSliders.IndexOf(s);
                this.CurrentEffect.Noises[idx] = (byte)((this.CurrentEffect.Noises[idx] & 0xE0) | (((byte)s.Value) & 0x1F));
                this.SelectedIndex = idx;
            }
            this.pnlEditorBars.Focus();
        }

        private void updateEnvFreq(object source, PropertyChangedEventArgs args)
        {
            Slider s = source as Slider;
            if (s != null && args.PropertyName == "Value")
            {
                int idx = this.envFreqHiSliders.IndexOf(s);
                if (idx > -1)
                {
                    int currentFreq = this.CurrentEffect.EnvFreqs[idx];
                    int hiFreq = (((int)s.Value) & 0xF) << 8;
                    currentFreq = (currentFreq & 0xFF) | hiFreq;
                    this.CurrentEffect.EnvFreqs[idx] = currentFreq;
                    this.SelectedIndex = idx;
                }
                idx = this.envFreqLoSliders.IndexOf(s);
                if (idx > -1)
                {
                    int currentFreq = this.CurrentEffect.EnvFreqs[idx] & 0xF00;
                    int loFreq = (int)s.Value;
                    currentFreq = currentFreq | loFreq;
                    this.CurrentEffect.EnvFreqs[idx] = currentFreq;
                    this.SelectedIndex = idx;
                }
            }
            this.pnlEditorBars.Focus();
        }

        private void loadEffectsCombo()
        {
            if (currentSong != null)
            {
                this.effectsBindingSource.DataSource = currentSong.Effects;
            }
        }

        private void numLength_ValueChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                this.CurrentEffect.SetEffectLength((int)numLength.Value);
                this.reloadEditPanel();
                this.pnlEditorBars.Focus();
            }
        }

        protected override void OnResizeBegin(EventArgs e)
        {
            this.SuspendLayout();
            base.OnResizeBegin(e);
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            this.ResumeLayout();
        }

        protected virtual void OnEffectChanged(EventArgs e)
        {
            EventHandler temp = EffectChanged;
            if (temp != null)
                temp(this, e);
        }

        private void numId_ValueChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                bool err = false;
                foreach (Effect eff in this.currentSong.Effects)
                {
                    if (eff != this.CurrentEffect)
                    {
                        if (eff.ID == numId.Value)
                        {
                            MessageBox.Show(
                                string.Format(WYZTracker.Properties.Resources.IdAlreadyInUse, eff.Name),
                                WYZTracker.Properties.Resources.Error,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            err = true;
                        }
                    }
                }
                if (!err)
                {
                    int newValue = (int)numId.Value;
                    this.CurrentEffect.ID = newValue;
                    foreach (Pattern p in this.currentSong.Patterns)
                    {
                        foreach (ChannelLine l in p.Lines)
                        {
                            if (l.Fx == oldFxId)
                            {
                                l.Fx = newValue;
                            }
                        }
                    }
                    oldFxId = newValue;
                    this.OnEffectChanged(EventArgs.Empty);
                }
                this.pnlEditorBars.Focus();
            }
        }

        private void effectsBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            this.exportarEfectoToolStripMenuItem.Enabled = (effectsBindingSource.Current != null);
            this.SelectedIndex = (effectsBindingSource.Current != null) ? 0 : -1;
            loadControls();
        }

        private void exportFX_Click(object sender, EventArgs e)
        {
            FxManager.Export(currentSong);
        }

        private void importFX_Click(object sender, EventArgs e)
        {
            FxManager.Import(currentSong);
        }

        private void deleteFX_Click(object sender, EventArgs e)
        {
            this.currentSong.Effects.Remove((Effect)lboxEffects.SelectedItem);
        }

        private void newFX_Click(object sender, EventArgs e)
        {
            FxManager.AddNew(currentSong);
        }

        private void exportarEfectoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.CurrentEffect != null)
            {
                FxManager.Export(this.CurrentEffect);
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            this.CurrentEffect.Name = txtName.Text;
        }

        private void envelopeContextMenu_Opening(object sender, CancelEventArgs e)
        {
            PictureBox sourcePb = envelopeContextMenu.SourceControl as PictureBox;
            int idx = this.pbEnvTypes.IndexOf(sourcePb);
        }

        private void EffectEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            clearSliders();
        }

        private void envelopeContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            PictureBox source = envelopeContextMenu.SourceControl as PictureBox;
            setEnvelopeValue(e.ClickedItem, source);
        }

        private void setEnvelopeValue(ToolStripItem item, PictureBox source)
        {
            if (source != null)
            {
                tipCtl.SetToolTip(source, item.Text);
                source.BackgroundImage = item.Image;
                int posIdx = this.pbEnvTypes.IndexOf(source);
                int envType = int.Parse(item.Tag.ToString());
                if (posIdx != -1)
                {
                    if (envType == ((int)Envelopes.env_none))
                    {
                        this.CurrentEffect.EnvTypes[posIdx] = (byte)Envelopes.env_continue;
                        this.CurrentEffect.Noises[posIdx] &= 0x7F;
                    }
                    else
                    {
                        this.CurrentEffect.EnvTypes[posIdx] = (byte)envType;
                        this.CurrentEffect.Noises[posIdx] |= 0x80;
                    }
                }
            }
        }

        private ToolStripMenuItem getItemByTagValue(object value)
        {
            foreach (ToolStripMenuItem child in envelopeContextMenu.Items)
            {
                if (child.Tag == value)
                {
                    return child;
                }
            }
            return null;
        }

        private void chkHex_CheckedChanged(object sender, EventArgs e)
        {
            this.reloadEditPanel();
            this.pnlEditorBars.Focus();
        }

        private void chkTesting_CheckedChanged(object sender, EventArgs e)
        {
            this.virtPiano.Enabled = chkTesting.Checked;
            this.pnlEditorBars.Focus();
        }

        private void OnNoteOrFxPressed(object sender, VirtualPiano.NoteFXEventArgs e)
        {
            if (!this.chkTesting.Checked && e.Note != null)
            {
                int freq = 0;
                if (e.Note.Note != 'P')
                {
                    int tone = (12 * e.Note.Octave - 2) + e.Note.GetNote() + (e.Note.Seminote == '+' ? 1 : 0) + 2;
                    int num = 0;

                    while (tone >= this.currentSong.Frequencies.Length)
                    {
                        tone -= 12;
                        num++;
                    }
                    freq = this.currentSong.Frequencies[tone];

                    freq = (int)(freq / Math.Pow(2, num));
                }

                this.freqHiSliders[this.SelectedIndex].Value = (freq & 0x0000FF00) >> 8;
                this.freqLoSliders[this.SelectedIndex].Value = (freq & 0x00FF);

                this.virtPiano.PlayEffect(this.CurrentEffect.ID);
            }
        }

        private void updateSelection()
        {
            if (this.pnlEditorBars.Controls.Count > 0)
            {
                for (int i = 0, li = this.pnlEditorBars.Controls.Count; i < li; ++i)
                {
                    Panel sliderGroup = this.pnlEditorBars.Controls[i] as Panel;
                    if (i == this.SelectedIndex)
                    {
                        sliderGroup.BackColor = SystemColors.Highlight;
                        this.pnlEditorBars.ScrollControlIntoView(sliderGroup);
                    }
                    else
                    {
                        sliderGroup.BackColor = Color.Transparent;
                    }
                }
            }
        }

        private void EffectEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.SelectedIndex != -1)
            {
                if (e.Control)
                {
                    handleKeyPressControl(e);
                }
                else if (e.Alt)
                {
                    handleKeyPressAlt(e);
                }
                else if (e.Shift)
                {
                    handleKeyPressShift(e);
                }
            }

            if (!e.Control && !e.Alt && !e.Shift)
            {
                handleKeyPressNoModifier(e);
            }
        }

        private void handleKeyPressShift(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.PageUp:
                    {
                        Slider lo = this.envFreqLoSliders[this.SelectedIndex];
                        Slider hi = this.envFreqHiSliders[this.SelectedIndex];
                        setSliderValue(lo, hi, +16);
                    }
                    break;
                case Keys.PageDown:
                    {
                        Slider lo = this.envFreqLoSliders[this.SelectedIndex];
                        Slider hi = this.envFreqHiSliders[this.SelectedIndex];
                        setSliderValue(lo, hi, -16);
                    }
                    break;
                case Keys.Up:
                    {
                        Slider lo = this.envFreqLoSliders[this.SelectedIndex];
                        Slider hi = this.envFreqHiSliders[this.SelectedIndex];
                        setSliderValue(lo, hi, +1);
                    }
                    break;
                case Keys.Down:
                    {
                        Slider lo = this.envFreqLoSliders[this.SelectedIndex];
                        Slider hi = this.envFreqHiSliders[this.SelectedIndex];
                        setSliderValue(lo, hi, -1);
                    }
                    break;
                case Keys.Right:
                    {
                        PictureBox currentPb = pbEnvTypes[this.SelectedIndex];
                        if ((this.CurrentEffect.Noises[this.SelectedIndex] & 0x80) == 0x80)
                        {
                            // Tenía env. Seleccionar la siguiente.
                            int envType = this.CurrentEffect.EnvTypes[this.SelectedIndex];
                            for (int i = 0, li = envelopeContextMenu.Items.Count; i < li; ++i)
                            {
                                ToolStripItem m = envelopeContextMenu.Items[i];
                                if (m.Tag.ToString() == envType.ToString())
                                {
                                    if (i == li - 1)
                                    {
                                        setEnvelopeValue(envelopeContextMenu.Items[0], currentPb);
                                    }
                                    else
                                    {
                                        setEnvelopeValue(envelopeContextMenu.Items[i + 1], currentPb);
                                    }
                                }
                            }
                        }
                        else
                        {
                            // Sin env, seleccionar la primera.
                            setEnvelopeValue(envelopeContextMenu.Items[1], currentPb);
                        }
                    }
                    break;
                case Keys.Left:
                    {
                        PictureBox currentPb = pbEnvTypes[this.SelectedIndex];
                        if ((this.CurrentEffect.Noises[this.SelectedIndex] & 0x80) == 0x80)
                        {
                            // Tenía env. Seleccionar la siguiente.
                            int envType = this.CurrentEffect.EnvTypes[this.SelectedIndex];
                            for (int i = 0, li = envelopeContextMenu.Items.Count; i < li; ++i)
                            {
                                ToolStripItem m = envelopeContextMenu.Items[i];
                                if (m.Tag.ToString() == envType.ToString())
                                {
                                    setEnvelopeValue(envelopeContextMenu.Items[i - 1], currentPb);
                                }
                            }
                        }
                        else
                        {
                            setEnvelopeValue(envelopeContextMenu.Items[envelopeContextMenu.Items.Count - 1], currentPb);
                        }
                    }
                    break;
            }
        }

        private static void setSliderValue(Slider lo, Slider hi, int offset)
        {
            int val = lo.Value + hi.Value * 256 + offset;
            if (val < 0)
            {
                val = 0;
            }
            if (val > 0x0FFF)
            {
                val = 0x0FFF;
            }
            lo.Value = val % 256;
            hi.Value = val / 256;
        }

        private void handleKeyPressAlt(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    {
                        this.noiseSliders[this.SelectedIndex].Value++;
                    }
                    break;
                case Keys.Down:
                    {
                        this.noiseSliders[this.SelectedIndex].Value--;
                    }
                    break;
            }
        }

        private void handleKeyPressControl(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.PageUp:
                    {
                        Slider lo = this.freqLoSliders[this.SelectedIndex];
                        Slider hi = this.freqHiSliders[this.SelectedIndex];
                        setSliderValue(lo, hi, +16);
                    }
                    break;
                case Keys.PageDown:
                    {
                        Slider lo = this.freqLoSliders[this.SelectedIndex];
                        Slider hi = this.freqHiSliders[this.SelectedIndex];
                        setSliderValue(lo, hi, -16);
                    }
                    break;
                case Keys.Up:
                    {
                        Slider lo = this.freqLoSliders[this.SelectedIndex];
                        Slider hi = this.freqHiSliders[this.SelectedIndex];
                        setSliderValue(lo, hi, +1);
                    }
                    break;
                case Keys.Down:
                    {
                        Slider lo = this.freqLoSliders[this.SelectedIndex];
                        Slider hi = this.freqHiSliders[this.SelectedIndex];
                        setSliderValue(lo, hi, -1);
                    }
                    break;
            }
        }

        private void handleKeyPressNoModifier(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    {
                        int newIdx = this._selectedIndex - 1;
                        if (newIdx < 0) { newIdx = this.CurrentEffect.Volumes.Length - 1; };
                        this.SelectedIndex = newIdx;
                    }
                    break;
                case Keys.Right:
                    {
                        int newIdx = this._selectedIndex + 1;
                        if (newIdx >= this.CurrentEffect.Volumes.Length) { newIdx = 0; };
                        this.SelectedIndex = newIdx;
                    }
                    break;
                case Keys.Up:
                    {
                        this.volumeSliders[this.SelectedIndex].Value++;
                    }
                    break;
                case Keys.Down:
                    {
                        this.volumeSliders[this.SelectedIndex].Value--;
                    }
                    break;
            }
        }
    }
}