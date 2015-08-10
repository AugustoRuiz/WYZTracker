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

        public EffectEditor()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.virtPiano = new VirtualPiano(this);
            this.virtPiano.Mode = VirtualPiano.PianoMode.Fx;
            this.virtPiano.Enabled = chkTesting.Checked;
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
                this.loadControls();
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

        private List<Slider> volumeSliders = new List<Slider>();
        private List<Slider> freqLoSliders = new List<Slider>();
        private List<Slider> freqHiSliders = new List<Slider>();
        private List<Slider> noiseSliders = new List<Slider>();
        private List<Slider> envFreqLoSliders = new List<Slider>();
        private List<Slider> envFreqHiSliders = new List<Slider>();

        private List<PictureBox> pbEnvTypes = new List<PictureBox>();

        private int sliderWidth = 24;

        private void reloadEditPanel()
        {
            Control oldPanel = this.pnlEditorBars.Controls.Count > 0 ? this.pnlEditorBars.Controls[0] : null;

            Panel thePanel = new Panel();
            thePanel.Size = this.pnlEditorBars.Size;
            thePanel.AutoScroll = true;
            thePanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

            clearSliders();
            int x = 0;
            for (int i = 0, li = this.CurrentEffect.Volumes.Length; i < li; ++i)
            {
                this.pbEnvTypes.Add(
                    createPbEnvType(x,
                        this.CurrentEffect.Noises[i],
                        this.CurrentEffect.EnvTypes[i],
                        this.showEnvTypes,
                        thePanel
                    )
                );

                thePanel.Controls.Add(
                    new Label() { Text = "V", ForeColor = Color.Lime, BackColor = Color.Transparent, Left = x, Top = 32, Height = 12, Width = sliderWidth, TextAlign = ContentAlignment.MiddleCenter }
                );
                this.volumeSliders.Add(
                    createSlider(x,
                        0, 0x0F,
                        this.CurrentEffect.Volumes[i],
                        Color.Lime,
                        this.updateVolume,
                        vf_array,
                        thePanel
                    )
                );
                x += sliderWidth;

                thePanel.Controls.Add(
                    new Label() { Text = "F", ForeColor = Color.Yellow, BackColor = Color.Transparent, Left = x, Top = 32, Height = 12, Width = 2 * sliderWidth, TextAlign = ContentAlignment.MiddleCenter }
                );
                this.freqHiSliders.Add(
                    createSlider(x, 0, 0x0F,
                        this.CurrentEffect.Frequencies[i] >> 8,
                        Color.Yellow,
                        this.updateFrequency,
                        vf_array,
                        thePanel
                    )
                );
                x += sliderWidth;

                this.freqLoSliders.Add(
                    createSlider(x, 0, 0xFF,
                        this.CurrentEffect.Frequencies[i] & 0xFF,
                        Color.Yellow,
                        this.updateFrequency,
                        vff_array,
                        thePanel
                    )
                );
                x += sliderWidth;

                thePanel.Controls.Add(
                    new Label() { Text = "N", ForeColor = Color.Red, BackColor = Color.Transparent, Left = x, Top = 32, Height = 12, Width = sliderWidth, TextAlign = ContentAlignment.MiddleCenter }
                );
                this.noiseSliders.Add(
                    createSlider(x, 0, 0x1F,
                        this.CurrentEffect.Noises[i] & 0x7F, Color.Red,
                        this.updateNoise,
                        v1f_array,
                        thePanel
                    )
                );
                x += sliderWidth;

                thePanel.Controls.Add(
                    new Label() { Text = "E", ForeColor = Color.SlateGray, BackColor = Color.Transparent, Left = x, Top = 32, Height = 12, Width = 2 * sliderWidth, TextAlign = ContentAlignment.MiddleCenter }
                );
                this.envFreqHiSliders.Add(
                    createSlider(x, 0, 0x0F,
                        this.CurrentEffect.EnvFreqs[i] >> 8, Color.SlateGray,
                        this.updateEnvFreq,
                        vf_array,
                        thePanel
                    )
                );
                x += sliderWidth;
                this.envFreqLoSliders.Add(
                    createSlider(x, 0, 0xFF,
                        this.CurrentEffect.EnvFreqs[i] & 0xFF, Color.SlateGray,
                        this.updateEnvFreq,
                        vff_array,
                        thePanel
                    )
                );

                x += sliderWidth + 2;
            }

            this.pnlEditorBars.SuspendLayout();
            this.pnlEditorBars.Controls.Add(thePanel);
            if (oldPanel != null) this.pnlEditorBars.Controls.Remove(oldPanel);
            this.pnlEditorBars.ResumeLayout();
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
            this.volumeSliders.Clear();
            this.freqHiSliders.Clear();
            this.freqLoSliders.Clear();
            this.noiseSliders.Clear();
            this.envFreqHiSliders.Clear();
            this.envFreqLoSliders.Clear();
            this.pbEnvTypes.Clear();
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
            envelopeContextMenu.Show(source as Control, 0, 0);
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
            }
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
                }
                idx = this.freqLoSliders.IndexOf(s);
                if (idx > -1)
                {
                    int currentFreq = this.CurrentEffect.Frequencies[idx] & 0xF00;
                    int loFreq = (int)s.Value;
                    currentFreq = currentFreq | loFreq;
                    this.CurrentEffect.Frequencies[idx] = currentFreq;
                }
            }
        }

        private void updateNoise(object source, PropertyChangedEventArgs args)
        {
            Slider s = source as Slider;
            if (s != null && args.PropertyName == "Value")
            {
                int idx = this.noiseSliders.IndexOf(s);
                this.CurrentEffect.Noises[idx] = (byte)((this.CurrentEffect.Noises[idx] & 0xE0) | (((byte)s.Value) & 0x1F));
            }
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
                }
                idx = this.envFreqLoSliders.IndexOf(s);
                if (idx > -1)
                {
                    int currentFreq = this.CurrentEffect.EnvFreqs[idx] & 0xF00;
                    int loFreq = (int)s.Value;
                    currentFreq = currentFreq | loFreq;
                    this.CurrentEffect.EnvFreqs[idx] = currentFreq;
                }
            }
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
            }
        }

        private void effectsBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            loadControls();
            this.exportarEfectoToolStripMenuItem.Enabled = (effectsBindingSource.Current != null);
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
            if (source != null)
            {
                tipCtl.SetToolTip(source, e.ClickedItem.Text);
                source.BackgroundImage = e.ClickedItem.Image;
                int posIdx = this.pbEnvTypes.IndexOf(source);
                int envType = int.Parse(e.ClickedItem.Tag.ToString());
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
        }

        private void chkTesting_CheckedChanged(object sender, EventArgs e)
        {
            this.virtPiano.Enabled = chkTesting.Checked;
        }
    }
}