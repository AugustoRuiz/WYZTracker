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
    public partial class InstrumentEditor : Form, ISongConsumer
    {
        private int modifiersVolIdx;
        private int initialVolIdx = -1;

        private List<byte> volumes = new List<byte>();
        private string oldInstrumentId;
        public event EventHandler InstrumentChanged;

        private VirtualPiano virtPiano;

        private bool loadingControls = false;

        public InstrumentEditor()
        {
            InitializeComponent();

            this.virtPiano = virtPiano = new VirtualPiano(this);
            this.virtPiano.Mode = VirtualPiano.PianoMode.Instrument;
            this.virtPiano.Enabled = chkTesting.Checked;
        }

        private Song currentSong;

        public Song CurrentSong
        {
            get { return currentSong; }
            set
            {
                currentSong = value;
                loadInstrumentsCombo();
            }
        }

        public Instrument CurrentInstrument
        {
            get
            {
                return (Instrument)this.instrumentsBindingSource.Current;
            }
            set
            {
                this.instrumentsBindingSource.Position = this.instrumentsBindingSource.IndexOf(value);
            }
        }

        private void loadControls()
        {
            splitContainer1.Panel2.Enabled = this.CurrentInstrument.ID != "R";

            loadingControls = true;
            if (splitContainer1.Panel2.Enabled)
            {
                numLength.Value = this.CurrentInstrument.Volumes.Length;
                numId.Value = int.Parse(this.CurrentInstrument.ID);
            }
            else
            {
                numLength.Value = numLength.Minimum;
            }
            txtName.Text = this.CurrentInstrument.Name;
            numLoopStart.Value = this.CurrentInstrument.LoopStart;
            chkLoop.Checked = this.CurrentInstrument.Looped;

            oldInstrumentId = this.CurrentInstrument.ID;

            loadingControls = false;

            pbWave.Invalidate();
        }

        private void loadInstrumentsCombo()
        {
            if (currentSong != null)
            {
                this.instrumentsBindingSource.DataSource = currentSong.Instruments;
            }
        }

        private void chkLoop_CheckedChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                this.CurrentInstrument.Looped = chkLoop.Checked;
            }
            numLoopStart.Enabled = chkLoop.Checked;
            pbWave.Invalidate();
        }

        private void numLength_ValueChanged(object sender, EventArgs e)
        {
            if (this.CurrentInstrument.ID != "R")
            {
                int oldLength = this.CurrentInstrument.Volumes.Length;
                float width = 100 / (float)numLength.Value;

                for (int i = 0; i < oldLength; i++)
                {
                    if (this.volumes.Count <= i)
                    {
                        this.volumes.Add(this.CurrentInstrument.Volumes[i]);
                    }
                    else
                    {
                        this.volumes[i] = this.CurrentInstrument.Volumes[i];
                    }
                }

                this.CurrentInstrument.SetVolumeLength((int)numLength.Value);

                if (oldLength < this.CurrentInstrument.Volumes.Length)
                {
                    for (int j = oldLength; j < this.CurrentInstrument.Volumes.Length && j < this.volumes.Count; j++)
                    {
                        this.CurrentInstrument.Volumes[j] = (byte)this.volumes[j];
                    }
                }

                if (this.numLoopStart.Value > numLength.Value - 1)
                {
                    numLoopStart.Value = numLength.Value - 1;
                }
                numLoopStart.Maximum = numLength.Value - 1;
                pbWave.Invalidate();
            }
        }

        private void pbWave_Paint(object sender, PaintEventArgs e)
        {
            if (CurrentInstrument != null)
            {
                if (CurrentInstrument.ID != "R")
                {
                    Point pt1 = Point.Empty;
                    Point pt2 = Point.Empty;

                    int instrumentLength = this.CurrentInstrument.Volumes.Length;

                    float width = pbWave.ClientRectangle.Width;
                    float height = pbWave.ClientRectangle.Height;

                    int numRects = (2 * instrumentLength);

                    RectangleF rectVol = new RectangleF(0, 0, width / numRects, height);
                    RectangleF rectModifier = new RectangleF(0, width / 2, rectVol.Width, 0);

                    float stepHeight = height / 16.0f;
                    float modifierStepHeight = height / 64.0f;
                    float midYPoint = height / 2.0f;

                    for (int i = 0; i < 64; i++)
                    {
                        float y = (height) * (i / 64.0f);
                        if ((i & 3) == 0)
                        {
                            e.Graphics.DrawLine(Pens.LimeGreen, 0, y, pbWave.ClientRectangle.Width, y);
                        }
                        else
                        {
                            e.Graphics.DrawLine(Pens.DarkGreen, 0, y, pbWave.ClientRectangle.Width, y);
                        }
                    }

                    for (int i = 0; i < instrumentLength; i++)
                    {
                        Brush currentBrush;

                        int volValue = this.CurrentInstrument.Volumes[i] & 0xF;
                        rectVol.Height = (int)(volValue * stepHeight);
                        rectVol.Y = height - rectVol.Height;

                        pt1.X = (int)rectVol.Left;
                        pt2.X = (int)rectVol.Right;

                        if ((this.CurrentInstrument.Volumes[i] & 0x40) != 0)
                        {
                            currentBrush = new LinearGradientBrush(pt1, pt2, Color.Red, Color.Yellow);//Brushes.Red;
                        }
                        else if ((this.CurrentInstrument.Volumes[i] & 0x20) != 0)
                        {
                            currentBrush = new LinearGradientBrush(pt1, pt2, Color.Blue, Color.Aqua);// Brushes.Aqua;
                        }
                        else
                        {
                            currentBrush = new LinearGradientBrush(pt1, pt2, Color.Green, Color.Lime);//Brushes.Lime;
                        }

                        e.Graphics.FillRectangle(currentBrush, rectVol);

                        TextRenderer.DrawText(e.Graphics, (this.CurrentInstrument.Volumes[i] & 0x0F).ToString(),
                            this.Font, Rectangle.Round(rectVol), Color.LightYellow,
                            TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter | TextFormatFlags.NoClipping);

                        e.Graphics.DrawLine(Pens.DarkGreen, rectVol.X, 0, rectVol.X, pbWave.ClientRectangle.Height);

                        currentBrush.Dispose();

                        if (this.CurrentInstrument.Looped && this.CurrentInstrument.LoopStart == i)
                        {
                            e.Graphics.DrawLine(Pens.Yellow, rectVol.X, 0, rectVol.X, pbWave.Height);
                        }

                        rectModifier.X = rectVol.Left + rectVol.Width;
                        rectModifier = updateModifierRect(rectModifier, modifierStepHeight, midYPoint, i);

                        pt1.X = (int)rectModifier.Left;
                        pt2.X = (int)rectModifier.Right;

                        if ((this.CurrentInstrument.Volumes[i] & 0x10) == 0)
                        {
                            currentBrush = new LinearGradientBrush(pt1, pt2, Color.DarkBlue, Color.Blue);// Brushes.Blue;
                        }
                        else
                        {
                            currentBrush = new LinearGradientBrush(pt1, pt2, Color.DarkRed, Color.Red);// Brushes.Red;
                        }
                        e.Graphics.FillRectangle(currentBrush, rectModifier);
                        currentBrush.Dispose();

                        TextRenderer.DrawText(e.Graphics, (this.CurrentInstrument.PitchModifiers[i]).ToString(),
                            this.Font, Rectangle.Round(rectModifier), Color.LightYellow,
                            TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter | TextFormatFlags.NoClipping);

                        e.Graphics.DrawLine(Pens.DarkGreen, rectModifier.X, 0, rectModifier.X, pbWave.ClientRectangle.Height);

                        rectVol.X = rectModifier.Left + rectVol.Width;
                    }
                }
                else
                {
                    e.Graphics.FillRectangle(Brushes.LightGray, pbWave.ClientRectangle);
                }
            }
        }

        private RectangleF updateModifierRect(RectangleF rectModifier, float stepHeight, float midYPoint, int i)
        {
            int modifierValue = this.CurrentInstrument.PitchModifiers[i];
            rectModifier.Height = Math.Abs(modifierValue * stepHeight);

            if (modifierValue <= 0)
            {
                rectModifier.Y = (int)midYPoint;
            }
            else
            {
                rectModifier.Y = (int)midYPoint - rectModifier.Height;
            }

            if (rectModifier.Height == 0)
            {
                rectModifier.Height = 1;
            }

            return rectModifier;
        }

        private void numLoopStart_ValueChanged(object sender, EventArgs e)
        {
            if (chkLoop.Checked && !loadingControls)
            {
                this.CurrentInstrument.LoopStart = (int)numLoopStart.Value;
            }
            pbWave.Invalidate();
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

        private void pbWave_Resize(object sender, EventArgs e)
        {
            pbWave.Invalidate();
        }

        private void pbWave_MouseUp(object sender, MouseEventArgs e)
        {
            if (CurrentInstrument != null &&
                (e.Button == MouseButtons.Left ||
                (e.Button == MouseButtons.Right && mouseInModifier(e.Location))))
            {
                updateBars(e.Location, e.Button);
                initialVolIdx = -1;
            }
            else if (e.Button == MouseButtons.Right && CurrentInstrument != null)
            {
                Point pt = e.Location;
                int volIdx = getBarIndex(pt);
                int realIdx = volIdx / 2;

                if (realIdx >= 0 && realIdx < this.CurrentInstrument.Volumes.Length)
                {
                    if ((volIdx & 1) == 0)
                    {
                        // Volume.
                        bool bit5, bit6;

                        bit5 = (this.CurrentInstrument.Volumes[realIdx] & 0x20) != 0;
                        bit6 = (this.CurrentInstrument.Volumes[realIdx] & 0x40) != 0;

                        menosUnaOctava.Checked = bit5;
                        masUnaOctava.Checked = bit6;

                        ctxtModificadores.Show(pbWave, e.Location);
                    }
                }
                this.modifiersVolIdx = realIdx;
            }
        }

        private int getBarIndex(Point pt)
        {
            float anchoVol = (this.pbWave.ClientRectangle.Width) /
                (2.0f * this.CurrentInstrument.Volumes.Length);
            if (initialVolIdx == -1)
            {
                initialVolIdx = (int)(pt.X / anchoVol);
            }
            int volIdx = initialVolIdx;

            if (!chkLockMovement.Checked)
            {
                volIdx = (int)(pt.X / anchoVol);
            }
            return volIdx;
        }

        private void pbWave_MouseMove(object sender, MouseEventArgs e)
        {
            if (CurrentInstrument != null &&
                (e.Button == MouseButtons.Left ||
                (e.Button == MouseButtons.Right && mouseInModifier(e.Location))))
            {
                updateBars(e.Location, e.Button);
            }
        }

        private void pbWave_MouseDown(object sender, MouseEventArgs e)
        {
            if (CurrentInstrument != null &&
                (e.Button == MouseButtons.Left ||
                (e.Button == MouseButtons.Right && mouseInModifier(e.Location))))
            {
                updateBars(e.Location, e.Button);
            }
        }

        private bool mouseInModifier(Point pt)
        {
            bool result = false;
            int volIdx = getBarIndex(pt);
            int realIdx = volIdx / 2;

            if (realIdx >= 0 && realIdx < this.CurrentInstrument.Volumes.Length)
            {
                result = ((volIdx & 1) == 1);
            }
            return result;
        }

        private void updateBars(Point pt, MouseButtons btn)
        {
            int volIdx = getBarIndex(pt);

            float altoVol = (this.pbWave.ClientRectangle.Height) / 16.0f;
            float volFloat = (pbWave.ClientRectangle.Height - pt.Y) / altoVol;
            int volValue = (int)Math.Ceiling(volFloat);

            int realIdx = volIdx / 2;

            if (realIdx >= 0 && realIdx < this.CurrentInstrument.Volumes.Length)
            {
                if ((volIdx & 1) == 0 && volValue >= 0 && volValue <= 15)
                {
                    // Volume.
                    this.CurrentInstrument.Volumes[realIdx] = (byte)((this.CurrentInstrument.Volumes[realIdx] & 0xF0) + volValue);
                }
                else
                {
                    float altoMod = (this.pbWave.ClientRectangle.Height) / 64.0f;
                    float modFloat = (pbWave.ClientRectangle.Height - pt.Y) / altoMod;
                    int modValue = (int)Math.Ceiling(modFloat) - 32;

                    if (Math.Abs(modValue) < 32)
                    {
                        // Modifier value
                        this.CurrentInstrument.PitchModifiers[realIdx] = modValue;
                        if (btn == MouseButtons.Left)
                        {
                            this.CurrentInstrument.Volumes[realIdx] &= 0xEF;
                        }
                        else if (btn == MouseButtons.Right)
                        {
                            this.CurrentInstrument.Volumes[realIdx] |= 0x10;
                        }
                    }
                }
                pbWave.Invalidate();
            }
        }

        private void mediaFrecuencia_Click(object sender, EventArgs e)
        {
            if (this.modifiersVolIdx != -1)
            {
                // Borrar el bit de media frecuencia.
                this.CurrentInstrument.Volumes[this.modifiersVolIdx] &= (byte)0xBF;

                if (menosUnaOctava.Checked)
                {
                    // Borrar el bit de mitad de frecuencia.
                    this.CurrentInstrument.Volumes[this.modifiersVolIdx] &= (byte)0xDF;
                }
                else
                {
                    // Poner el bit de mitad de frecuencia.
                    this.CurrentInstrument.Volumes[this.modifiersVolIdx] |= (byte)0x20;
                }
                pbWave.Invalidate();
                this.modifiersVolIdx = -1;
            }
        }

        private void masUnaOctava_Click(object sender, EventArgs e)
        {
            if (this.modifiersVolIdx != -1)
            {
                // Borrar el bit de doble frecuencia.
                this.CurrentInstrument.Volumes[this.modifiersVolIdx] &= (byte)0xDF;

                if (masUnaOctava.Checked)
                {
                    // Borrar el bit de media frecuencia.
                    this.CurrentInstrument.Volumes[this.modifiersVolIdx] &= (byte)0xBF;
                }
                else
                {
                    // Poner el bit de media frecuencia.
                    this.CurrentInstrument.Volumes[this.modifiersVolIdx] |= (byte)0x40;
                }
                pbWave.Invalidate();
                this.modifiersVolIdx = -1;
            }
        }

        private void numId_ValueChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                bool err = false;
                foreach (Instrument i in this.currentSong.Instruments)
                {
                    if (i != this.CurrentInstrument)
                    {
                        if (i.ID == numId.Value.ToString())
                        {
                            MessageBox.Show(
                                string.Format(WYZTracker.Properties.Resources.InstrIdAlreadyInUse, i.Name),
                                WYZTracker.Properties.Resources.Error,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            err = true;
                            break;
                        }
                    }
                }
                if (!err)
                {
                    string newId = numId.Value.ToString();
                    this.CurrentInstrument.ID = newId;
                    foreach (Pattern p in this.currentSong.Patterns)
                    {
                        foreach (ChannelLine l in p.Lines)
                        {
                            foreach (ChannelNote n in l.Notes)
                            {
                                if (n.Instrument == oldInstrumentId)
                                {
                                    n.Instrument = newId;
                                }
                            }
                        }
                    }
                    oldInstrumentId = newId;
                    this.OnInstrumentChanged(EventArgs.Empty);
                }
            }
        }

        protected virtual void OnInstrumentChanged(EventArgs e)
        {
            InstrumentChanged?.Invoke(this, e);
        }

        #region Instrument list management

        private void newInstrument_Click(object sender, EventArgs e)
        {
            InstrumentsManager.AddNew(currentSong);
        }

        private void deleteInstrument_Click(object sender, EventArgs e)
        {
            InstrumentsManager.Remove(currentSong, (Instrument)lboxInstruments.SelectedItem);
        }

        private void importInstruments_Click(object sender, EventArgs e)
        {
            InstrumentsManager.Import(currentSong);
        }

        private void exportInstruments_Click(object sender, EventArgs e)
        {
            InstrumentsManager.Export(currentSong);
        }

        private void lstInstruments_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.deleteInstrument.Enabled = lboxInstruments.SelectedItem != null && ((Instrument)lboxInstruments.SelectedItem).ID != "R";
        }

        #endregion

        private void instrumentsBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            this.loadControls();
            exportarInstrumentoToolStripMenuItem.Enabled =
                (this.CurrentInstrument != null && this.CurrentInstrument.ID != "R");
            virtPiano.CurrentInstrument = this.CurrentInstrument;
        }

        private void exportarInstrumentoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.CurrentInstrument != null && this.CurrentInstrument.ID != "R")
            {
                InstrumentsManager.Export(this.CurrentInstrument);
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (!loadingControls)
            {
                this.CurrentInstrument.Name = txtName.Text;
            }
        }

        private void ctxtModificadores_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = false;

            Point mouseAbsolutePos = Cursor.Position;
            Point pt = pbWave.PointToClient(mouseAbsolutePos);

            int volIdx = getBarIndex(pt);
            int realIdx = volIdx / 2;

            if (realIdx >= 0 && realIdx < this.CurrentInstrument.Volumes.Length)
            {
                if ((volIdx & 1) == 1)
                {
                    e.Cancel = true;
                }
            }
        }

        private void chkTesting_CheckedChanged(object sender, EventArgs e)
        {
            this.virtPiano.Enabled = chkTesting.Checked;
        }
    }
}