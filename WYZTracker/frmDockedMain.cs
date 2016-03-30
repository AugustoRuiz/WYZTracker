using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using LibAYEmu;
using System.IO.Compression;

namespace WYZTracker
{
    public partial class frmDockedMain : Form, ISongConsumer
    {
        public frmDockedMain(string[] args)
        {
            InitializeComponent();
            loadCombos();

            this.loadingSong = false;

            this.songPlayer = new Player();
            this.songPlayer.NextLine += new Player.NextLineEventHandler(songPlayer_NextLine);
            this.songPlayer.SongFinished += new EventHandler(songPlayer_SongFinished);
            this.songPlayer.CurrentTempoChanged += songPlayer_CurrentTempoChanged;

            initializeSong();
            bindControls();

            this.lboxPatterns.SelectedIndex = 0;
            this.cmbOctavaBase.SelectedIndex = 1;
            this.patEditor.EditionIncrement = 1;
            this.patEditor.HighlightRange = 4;
            this.patEditor.DelayDecrement = 2;
            this.cboStereo.SelectedIndex = 0;
            this.txtResalte.Text = "4";

            if (args.Length > 0)
            {
                loadSongFromFile(args[0]);
            }

            asyncThreadSemaphore = new System.Threading.Semaphore(1, 1);

            if (Properties.Settings.Default.ShowSplash)
            {
                ApplicationState.Instance.SplashScreen.FadeOut();
            }
        }

        void songPlayer_CurrentTempoChanged(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker) delegate { songPlayer_CurrentTempoChanged(sender, e); });
            }
            else
            {
                if (this.songPlayer != null && this.songProperties != null)
                {
                    this.songProperties.CurrentTempo = this.songPlayer.CurrentTempo;
                }
            }
        }

        private void loadCombos()
        {
            cboEnvRatio.Items.Clear();
            cboEnvRatio.Items.Add(2);
            cboEnvRatio.Items.Add(4);
            cboEnvRatio.Items.Add(8);
            cboEnvRatio.Items.Add(16);

            cboEnvShape.Items.Clear();
            cboEnvShape.Items.Add(8);
            cboEnvShape.Items.Add(10);
            cboEnvShape.Items.Add(12);
            cboEnvShape.Items.Add(14);

            cboStereo.Items.Clear();
            string[] stereoValues = Enum.GetNames(typeof(Stereo));
            foreach (string s in stereoValues)
            {
                if (s != "StereoCustom")
                {
                    cboStereo.Items.Add(s);
                }
            }
        }

        void songPlayer_NextLine(object sender, NextLineEventArgs evArgs)
        {
            if (this.InvokeRequired)
            {
                doNextLineDelegate dlgDoNextLine = new doNextLineDelegate(doNextLine);
                try
                {
                    this.BeginInvoke(dlgDoNextLine, evArgs);
                }
                catch
                {
                }
            }
            else
            {
                doNextLine(evArgs);
            }
        }

        private delegate void doNextLineDelegate(NextLineEventArgs evArgs);

        private void doNextLine(NextLineEventArgs evArgs)
        {
            // This semaphore makes sure this is not processed if the UI thread 
            // closes the form - avoid try to access disposed resources.
            asyncThreadSemaphore.WaitOne();
            if (!loadingSong)
            {
                if (evArgs.PatternNumber != lboxPatterns.SelectedIndex)
                {
                    if (evArgs.PatternNumber < this.lboxPatterns.Items.Count)
                    {
                        this.lboxPatterns.SelectedIndex = evArgs.PatternNumber;
                    }
                }
                if (evArgs.LineNumber != this.patEditor.SelectedIndex &&
                    this.patEditor.CurrentPattern != null &&
                    this.patEditor.CurrentPattern.Lines.Length > evArgs.LineNumber)
                {
                    this.patEditor.SelectedIndex = evArgs.LineNumber;
                }
            }
            asyncThreadSemaphore.Release();
        }

        private void songPlayer_SongFinished(object sender, EventArgs e)
        {
            this.songPlayer.Stop();
        }

        private bool loadingSong;
        private Song currentSong;
        private Pattern currentPattern;
        private Instrument currentInstrument;
        private Effect currentEffect;
        private byte currentChannel;

        private string _theSongFileName;

        private System.Threading.Semaphore asyncThreadSemaphore;

        private string songFileName
        {
            get
            {
                return _theSongFileName;
            }
            set
            {
                _theSongFileName = value;

                ApplicationState.Instance.FileName = _theSongFileName;

                this.Text = string.Format(WYZTracker.Properties.Resources.WYZTrackerTitle,
                    _theSongFileName ?? WYZTracker.Properties.Resources.New);
            }
        }

        private Player songPlayer;

        public Song CurrentSong
        {
            get
            {
                return currentSong;
            }
            set
            {
                if (currentSong != value)
                {
                    loadingSong = true;

                    currentSong = value;

                    ApplicationState.Instance.CurrentSong = currentSong;
                    this.songPlayer.CurrentSong = currentSong;
                    FormFactory.UpdateCurrentSong(currentSong);
                    fillPatternList();

                    loadingSong = false;
                }
            }
        }

        public Pattern CurrentPattern
        {
            get
            {
                return currentPattern;
            }
            set
            {
                if (currentPattern != value)
                {
                    currentPattern = value;
                }
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
                if (currentInstrument != value)
                {
                    currentInstrument = value;
                    this.patEditor.CurrentInstrument = this.currentInstrument;
                }
            }
        }

        public Effect CurrentEffect
        {
            get
            {
                return currentEffect;
            }
            set
            {
                if (currentEffect != value)
                {
                    currentEffect = value;
                }
            }
        }

        public string CurrentInstrumentName
        {
            get
            {
                if (currentInstrument != null)
                {
                    return currentInstrument.Name;
                }
                return "";
            }
            set
            {
                if (!String.IsNullOrEmpty(value) && currentInstrument != null && currentInstrument.Name != value)
                {
                    currentInstrument.Name = value;
                }
            }
        }

        public string CurrentEffectName
        {
            get
            {
                if (currentEffect != null)
                {
                    return currentEffect.Name;
                }
                return "";
            }
            set
            {
                if (!String.IsNullOrEmpty(value) && currentEffect != null && currentEffect.Name != value)
                {
                    currentEffect.Name = value;
                }
            }
        }

        public byte CurrentChannel
        {
            get
            {
                return currentChannel;
            }
            set
            {
                if (currentChannel != value)
                {
                    currentChannel = value;
                }
            }
        }

        private void initializeSong()
        {
            this.CurrentSong = new Song(Song.DEFAULT_CHANNELS_COUNT);
            this.currentSong.Name = WYZTracker.Properties.Resources.NewSong;
            this.currentSong.Tempo = 4;
            this.currentPattern = this.currentSong.Patterns[0];
            this.currentSong.PlayOrder.Add(0);
            this.currentSong.Frequencies = NoteFileLoader.LoadDefaultNotes();

            this.CurrentInstrument = this.currentSong.Instruments[0];
            this.CurrentEffect = null;
        }

        private void bindControls()
        {
            //this.patEditor.Envelope = this.envData;

            if (this.currentSong != null)
            {
                this.patEditor.CurrentSong = this.currentSong;
                this.songProperties.CurrentSong = this.currentSong;
                this.songPlayer.CurrentSong = this.currentSong;
            }
            if (this.currentPattern != null)
            {
                this.patEditor.CurrentPattern = this.currentPattern;
                this.songProperties.CurrentPattern = this.currentPattern;
            }
            this.patEditor.CurrentChannel = this.currentChannel;

            this.instrumentsBindingSource.DataSource = this.currentSong.Instruments;
            this.effectsBindingSource.DataSource = this.currentSong.Effects;
            this.envelopeDataBindingSource.DataSource = ApplicationState.Instance.CurrentEnvData;

            if (this.DataBindings.Count == 0)
            {
                this.DataBindings.Add("CurrentInstrumentName", this.txtInstrumentName, "Text");
                this.DataBindings.Add("CurrentEffectName", this.txtFXName, "Text");
            }

            fillPatternList();
        }

        private void fillPatternList()
        {
            this.lboxPatterns.Items.Clear();
            for (int i = 0; i < this.currentSong.PlayOrder.Count; i++)
            {
                this.lboxPatterns.Items.Add(this.currentSong.PlayOrder[i]);
            }
            if (this.currentSong.PlayOrder.Count > 0)
            {
                this.lboxPatterns.SelectedIndex = 0;
            }
        }

        #region Gestión de eventos de menú

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile(sender, e);
        }

        private void saveFile(object sender, EventArgs e)
        {
            if (this.songFileName == null)
            {
                this.saveAsToolStripMenuItem_Click(sender, e);
            }
            else
            {
                saveSong(this.songFileName);
            }
        }

        private void saveSong(string fileName)
        {
            SongManager.SaveSong(this.currentSong, fileName);
            this.songFileName = fileName;
            setFocusToEditor();
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exportarCancion();
        }

        private void exportarCancion()
        {
            this.sfd.Filter = WYZTracker.Properties.Resources.MUSFilter;
            if (this.sfd.ShowDialog() == DialogResult.OK)
            {
                Stream str = new FileStream(this.sfd.FileName, FileMode.Create);
                BinaryWriter writer = new BinaryWriter(str);

                byte[] result = SongBinProvider.GenerateSong(this.currentSong);

                writer.Write(result);
                writer.Close();

                Stream textStr = new FileStream(string.Format("{0}.asm", this.sfd.FileName), FileMode.Create);
                StreamWriter textWriter = new StreamWriter(textStr);
                textWriter.Write(SongBinProvider.GenerateInstrumentsAndFX(this.currentSong));
                textWriter.Close();

                str.Dispose();
            }
            setFocusToEditor();
        }

        private void exportarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InstrumentsManager.Export(currentSong);
        }

        private void importarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InstrumentsManager.Import(currentSong);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.sfd.Filter = WYZTracker.Properties.Resources.WYZFilter;
            if (this.sfd.ShowDialog() == DialogResult.OK)
            {
                saveSong(this.sfd.FileName);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFile();
        }

        private void openFile()
        {
            this.ofd.Filter = WYZTracker.Properties.Resources.WYZFilter;
            if (this.ofd.ShowDialog() == DialogResult.OK)
            {
                loadSongFromFile(this.ofd.FileName);
            }
            setFocusToEditor();
        }

        private void loadSongFromFile(string fileName)
        {
            this.CurrentSong = SongManager.LoadSong(fileName);

            if (this.currentSong.Patterns.Count > 0)
            {
                if (this.currentSong.PlayOrder.Count > 0)
                {
                    this.currentPattern = this.currentSong.Patterns[this.currentSong.PlayOrder[0]];
                }
                else
                {
                    this.currentPattern = this.currentSong.Patterns[0];
                }
            }

            this.songFileName = fileName;
            this.bindControls();

            if (this.currentSong.Effects.Count > 0)
            {
                this.lstFX.ClearSelected();
                this.lstFX.SelectedIndex = 0;
            }

            checkSawtoothExists();

            this.lstInstruments.ClearSelected();
            this.lstInstruments.SelectedIndex = 0;
        }

        private void checkSawtoothExists()
        {
            bool sawtoothFound = false;
            foreach (Instrument i in this.currentSong.Instruments)
            {
                if (i.ID == "R")
                {
                    sawtoothFound = true;
                    break;
                }
            }

            if (!sawtoothFound)
            {
                Instrument saw = new Instrument();
                saw.ID = "R";
                saw.Name = "Sawtooth";
                saw.Looped = false;
                saw.LoopStart = 0;
                this.currentSong.Instruments.Add(saw);
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newFile();
        }

        private void newFile()
        {
            this.initializeSong();
            this.bindControls();
            this.songFileName = null;
            setFocusToEditor();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion

        #region Gestión de instrumentos

        private void newInstrument_Click(object sender, EventArgs e)
        {
            InstrumentsManager.AddNew(currentSong);
        }

        private void deleteInstrument_Click(object sender, EventArgs e)
        {
            InstrumentsManager.Remove(currentSong, (Instrument)lstInstruments.SelectedItem);
        }

        private void importInstruments_Click(object sender, EventArgs e)
        {
            InstrumentsManager.Import(currentSong);
        }

        private void exportInstruments_Click(object sender, EventArgs e)
        {
            InstrumentsManager.Export(currentSong);
        }

        private void instrumentProperties_Click(object sender, EventArgs e)
        {
            mostrarEditorInstrumentos();
        }

        private void lstInstruments_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.deleteInstrument.Enabled = lstInstruments.SelectedItem != null && ((Instrument)lstInstruments.SelectedItem).ID != "R";
            this.instrumentProperties.Enabled = lstInstruments.SelectedItem != null && ((Instrument)lstInstruments.SelectedItem).ID != "R";

            if (this.lstInstruments.SelectedItem != null)
            {
                this.CurrentInstrument = (Instrument)lstInstruments.SelectedItem;
            }
            if (!this.txtInstrumentName.Focused)
            {
                this.txtInstrumentName.Text = this.lstInstruments.SelectedItem == null ? "" : ((Instrument)this.lstInstruments.SelectedItem).Name;
            }

            exportInstrumentToolStripMenuItem.Enabled = this.currentInstrument != null && this.currentInstrument.ID != "R";

            setFocusToEditor();
        }

        private void setFocusToEditor()
        {
            if (this == ActiveForm)
            {
                this.patEditor.Focus();
            }
        }

        private void lstInstruments_DoubleClick(object sender, EventArgs e)
        {
            mostrarEditorInstrumentos();
        }

        private void mostrarEditorInstrumentos()
        {
            if (lstInstruments.SelectedItem != null)
            {
                InstrumentEditor editor = FormFactory.CreateOrActivateFormOfType<InstrumentEditor>();
                editor.FormClosed -= new FormClosedEventHandler(instrumentEditor_FormClosed);
                editor.InstrumentChanged -= new EventHandler(editor_InstrumentChanged);

                editor.CurrentSong = this.currentSong;
                editor.CurrentInstrument = (Instrument)lstInstruments.SelectedItem;

                editor.InstrumentChanged += new EventHandler(editor_InstrumentChanged);
                editor.FormClosed += new FormClosedEventHandler(instrumentEditor_FormClosed);
            }
        }

        private void instrumentEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            InstrumentEditor editor = sender as InstrumentEditor;
            if (editor != null)
            {
                editor.InstrumentChanged -= new EventHandler(editor_InstrumentChanged);
                editor.FormClosed -= new FormClosedEventHandler(instrumentEditor_FormClosed);
            }
        }

        private void editor_InstrumentChanged(object sender, EventArgs e)
        {
            this.patEditor.Invalidate();

            Form senderForm = sender as Form;
            if (senderForm != null)
            {
                senderForm.Activate();
            }
        }

        private void mostrarEditorFX()
        {
            if (lstFX.SelectedItem != null)
            {
                EffectEditor editor = FormFactory.CreateOrActivateFormOfType<EffectEditor>();
                editor.FormClosed -= new FormClosedEventHandler(fxEditor_FormClosed);
                editor.EffectChanged -= new EventHandler(editor_InstrumentChanged);
                editor.CurrentSong = this.currentSong;
                editor.CurrentEffect = (Effect)this.lstFX.SelectedItem;
                editor.EffectChanged += new EventHandler(editor_InstrumentChanged);
                editor.FormClosed += new FormClosedEventHandler(fxEditor_FormClosed);
            }
        }

        private void fxEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            EffectEditor editor = sender as EffectEditor;
            if (editor != null)
            {
                editor.EffectChanged -= new EventHandler(editor_InstrumentChanged);
                editor.FormClosed -= new FormClosedEventHandler(instrumentEditor_FormClosed);
            }
        }

        #endregion

        private void songProperties_SongChannelsChanged(object sender, SongChannelsChangedEventArgs e)
        {
            this.currentSong.Channels = e.NewChannelCount;
            this.patEditor.UpdateChannels();
        }

        private void txtInstrumentName_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = (lstInstruments.SelectedItem != null && String.IsNullOrEmpty(txtInstrumentName.Text));
        }

        private void txtInstrumentName_Validated(object sender, EventArgs e)
        {
            if (lstInstruments.SelectedItem != null)
            {
                ((Instrument)lstInstruments.SelectedItem).Name = txtInstrumentName.Text;
            }
        }

        private void txtFXName_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = (lstFX.SelectedItem != null && String.IsNullOrEmpty(txtFXName.Text));
        }

        private void txtFXName_Validated(object sender, EventArgs e)
        {
            if (lstFX.SelectedItem != null)
            {
                ((Effect)lstFX.SelectedItem).Name = txtFXName.Text;
            }
        }

        private void fxProperties_Click(object sender, EventArgs e)
        {
            mostrarEditorFX();
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
            this.currentSong.Effects.Remove((Effect)lstFX.SelectedItem);
        }

        private void newFX_Click(object sender, EventArgs e)
        {
            FxManager.AddNew(currentSong);
        }

        private void lstFX_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.deleteFX.Enabled = lstFX.SelectedItem != null;
            this.fxProperties.Enabled = lstFX.SelectedItem != null;

            if (this.lstFX.SelectedItem != null)
            {
                this.CurrentEffect = (Effect)lstFX.SelectedItem;
            }
            if (!this.txtFXName.Focused)
            {
                this.txtFXName.Text = this.lstFX.SelectedItem == null ? "" : ((Effect)this.lstFX.SelectedItem).Name;
            }

            exportarEfectoToolStripMenuItem.Enabled = (this.currentEffect != null);

            setFocusToEditor();
        }

        private void lstFX_DoubleClick(object sender, EventArgs e)
        {
            mostrarEditorFX();
        }

        private void nuevoToolStrip_Click(object sender, EventArgs e)
        {
            newFile();
        }

        private void abrirToolStrip_Click(object sender, EventArgs e)
        {
            openFile();
        }

        private void guardarToolStrip_Click(object sender, EventArgs e)
        {
            saveFile(sender, e);
        }

        private void playToolStrip_Click(object sender, EventArgs e)
        {
            PlayMode mode = PlayMode.FullSong;
            if (e is PlayEventArgs)
            {
                mode = ((PlayEventArgs)e).PlayMode;
            }
            if (this.songPlayer.Status == PlayStatus.Playing)
            {
                this.songPlayer.Stop();
            }
            else
            {
                this.songPlayer.PlayMode = mode;
                this.songPlayer.Play((int)this.lboxPatterns.SelectedIndex);
            }
            setFocusToEditor();
        }

        private void stopToolStrip_Click(object sender, EventArgs e)
        {
            if (this.songPlayer.Status == PlayStatus.Playing)
            {
                this.songPlayer.Stop();
            }
            else
            {
                this.lboxPatterns.SelectedIndex = 0;
                this.patEditor.SelectedIndex = 0;
            }
            setFocusToEditor();
        }

        private void addPatronToolStrip_Click(object sender, EventArgs e)
        {
            int currentPatternIdx = ((int?)lboxPatterns.SelectedItem).GetValueOrDefault();
            this.currentSong.PlayOrder.Insert(lboxPatterns.SelectedIndex + 1, currentPatternIdx);
            this.lboxPatterns.Items.Insert(lboxPatterns.SelectedIndex + 1, currentPatternIdx);
            setFocusToEditor();
        }

        private void removePatronToolStrip_Click(object sender, EventArgs e)
        {
            if (this.lboxPatterns.SelectedIndex != -1)
            {
                this.currentSong.PlayOrder.RemoveAt(this.lboxPatterns.SelectedIndex);
                this.lboxPatterns.Items.RemoveAt(this.lboxPatterns.SelectedIndex);
            }
            setFocusToEditor();
        }

        private void lboxPatterns_SelectedIndexChanged(object sender, EventArgs e)
        {
            int patIdx = -1;

            if (lboxPatterns.SelectedIndex != -1)
            {
                patIdx = (int)lboxPatterns.SelectedItem;
                this.cmdNextPattern.Enabled = true;
                this.cmdPreviousPattern.Enabled = (patIdx > 0);
                this.patEditor.CurrentPattern = this.currentSong.Patterns[patIdx];
                this.patternBindingSource.DataSource = this.currentSong.Patterns[patIdx];
            }
            else
            {
                this.cmdNextPattern.Enabled = false;
                this.cmdPreviousPattern.Enabled = false;
            }
            this.removePatronToolStrip.Enabled = (lboxPatterns.SelectedIndex != -1);
            this.cmdNextPattern.Enabled = (lboxPatterns.SelectedIndex != -1);
            this.cmdPreviousPattern.Enabled = (lboxPatterns.SelectedIndex != -1) && (patIdx > 0);
            this.cmdSubirPatron.Enabled = (lboxPatterns.SelectedIndex > 0);
            this.cmdBajarPatron.Enabled = (lboxPatterns.SelectedIndex < this.lboxPatterns.Items.Count - 1);
        }

        private void cmdNextPattern_Click(object sender, EventArgs e)
        {
            int patIdx = (int)lboxPatterns.SelectedItem;
            if (this.currentSong.Patterns.Count <= patIdx + 1)
            {
                this.currentSong.Patterns.Add(new Pattern(this.currentSong.Channels));
            }
            this.currentSong.PlayOrder[lboxPatterns.SelectedIndex] = patIdx + 1;
            lboxPatterns.Items[lboxPatterns.SelectedIndex] = patIdx + 1;
        }

        private void cmdPreviousPattern_Click(object sender, EventArgs e)
        {
            int patIdx = (int)lboxPatterns.SelectedItem;
            if (patIdx > 0)
            {
                this.currentSong.PlayOrder[lboxPatterns.SelectedIndex] = patIdx - 1;
                lboxPatterns.Items[lboxPatterns.SelectedIndex] = patIdx - 1;
            }
        }

        private void cmdSubirPatron_Click(object sender, EventArgs e)
        {
            int idx = lboxPatterns.SelectedIndex;
            int tmp = (int)lboxPatterns.Items[idx];
            lboxPatterns.Items[idx] = lboxPatterns.Items[idx - 1];
            lboxPatterns.Items[idx - 1] = tmp;
            this.currentSong.PlayOrder[idx] = (int)lboxPatterns.Items[idx];
            this.currentSong.PlayOrder[idx - 1] = (int)lboxPatterns.Items[idx - 1];
            this.lboxPatterns.SelectedIndex = idx - 1;
        }

        private void cmdBajarPatron_Click(object sender, EventArgs e)
        {
            int idx = lboxPatterns.SelectedIndex;
            int tmp = (int)lboxPatterns.Items[idx];
            lboxPatterns.Items[idx] = lboxPatterns.Items[idx + 1];
            lboxPatterns.Items[idx + 1] = tmp;
            this.currentSong.PlayOrder[idx] = (int)lboxPatterns.Items[idx];
            this.currentSong.PlayOrder[idx + 1] = (int)lboxPatterns.Items[idx + 1];
            this.lboxPatterns.SelectedIndex = idx + 1;
        }

        private void cmbOctavaBase_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbOctavaBase.SelectedIndex != -1)
            {
                ApplicationState.Instance.BaseOctave = int.Parse((string)cmbOctavaBase.SelectedItem);
                setFocusToEditor();
            }
        }

        private void patEditor_Play(object sender, PlayEventArgs e)
        {
            this.playToolStrip_Click(sender, e);
        }

        private void patEditor_Stop(object sender, EventArgs e)
        {
            this.stopToolStrip_Click(sender, e);
        }

        private void patEditor_IncreaseOctave(object sender, EventArgs e)
        {
            if (this.cmbOctavaBase.SelectedIndex > 0)
            {
                cmbOctavaBase.SelectedIndex -= 1;
            }
        }

        private void patEditor_DecreaseOctave(object sender, EventArgs e)
        {
            if (this.cmbOctavaBase.SelectedIndex < this.cmbOctavaBase.Items.Count - 1)
            {
                cmbOctavaBase.SelectedIndex += 1;
            }
        }

        private void patEditor_EditionIncrementChanged(object sender, EventArgs e)
        {
            incrementoAutomatico.Text = patEditor.EditionIncrement.ToString();
        }

        private void patEditor_FxChannelChanged(object sender, FxChannelChangedEventArgs e)
        {
            if (this.currentSong != null)
            {
                this.currentSong.FxChannel = e.FxChannel;
            }
        }

        private void incrementoAutomatico_TextChanged(object sender, EventArgs e)
        {
            int incremento;
            if (int.TryParse(incrementoAutomatico.Text, out incremento))
            {
                patEditor.EditionIncrement = incremento;
            }
        }

        private void exportarWYZToolStrip_Click(object sender, EventArgs e)
        {
            this.exportarCancion();
        }

        private void txtResalte_TextChanged(object sender, EventArgs e)
        {
            int resalte;
            if (int.TryParse(txtResalte.Text, out resalte))
            {
                patEditor.HighlightRange = resalte;
            }
        }

        private void patEditor_HighlightRangeChanged(object sender, EventArgs e)
        {
            this.txtResalte.Text = patEditor.HighlightRange.ToString();
        }

        private void clonePattern_Click(object sender, EventArgs e)
        {
            if (lboxPatterns.SelectedIndex != -1)
            {
                Pattern src = this.CurrentSong.Patterns[(int)lboxPatterns.SelectedItem];
                Pattern cloned = SerializationUtils.Clone(src);

                this.currentSong.Patterns.Add(cloned);
                int idx = this.currentSong.Patterns.IndexOf(cloned);

                this.currentSong.PlayOrder.Insert(lboxPatterns.SelectedIndex + 1, idx);
                lboxPatterns.Items.Insert(lboxPatterns.SelectedIndex + 1, idx);
            }
            setFocusToEditor();
        }

        private void patEditor_DecreaseInstrument(object sender, EventArgs e)
        {
            if (lstInstruments.SelectedIndex > 0)
            {
                lstInstruments.SelectedIndex -= 1;
            }
            else
            {
                lstInstruments.SelectedIndex = lstInstruments.Items.Count - 1;
            }
        }

        private void patEditor_IncreaseInstrument(object sender, EventArgs e)
        {
            if (lstInstruments.SelectedIndex == lstInstruments.Items.Count - 1)
            {
                lstInstruments.SelectedIndex = 0;
            }
            else
            {
                lstInstruments.SelectedIndex += 1;
            }
        }

        private void patEditor_NextPattern(object sender, EventArgs e)
        {
            int newIndex = -1;
            if (lboxPatterns.Items.Count > 0)
            {
                newIndex = lboxPatterns.SelectedIndex + 1;
                if (newIndex >= lboxPatterns.Items.Count)
                {
                    newIndex = 0;
                }
            }
            lboxPatterns.SelectedIndex = newIndex;
        }

        private void patEditor_PreviousPattern(object sender, EventArgs e)
        {
            int newIndex = -1;
            if (lboxPatterns.Items.Count > 0)
            {
                newIndex = lboxPatterns.SelectedIndex - 1;
                if (newIndex < 0)
                {
                    newIndex = lboxPatterns.Items.Count - 1;
                }
            }
            lboxPatterns.SelectedIndex = newIndex;
        }

        private void patEditor_SetActiveInstrument(object sender, ActiveInstrumentEventArgs e)
        {
            if (e.Id != null)
            {
                string idLowerInvariant = e.Id.ToLowerInvariant();
                foreach (Instrument instrument in this.currentSong.Instruments)
                {
                    if (instrument.ID.ToLowerInvariant() == idLowerInvariant)
                    {
                        this.lstInstruments.SelectedItem = instrument;
                        break;
                    }
                }
            }
        }

        private void patEditor_SetActiveFx(object sender, ActiveFxEventArgs e)
        {
            foreach (Effect effect in this.currentSong.Effects)
            {
                if (effect.ID == e.Id)
                {
                    this.lstFX.SelectedItem = effect;
                    break;
                }
            }
        }

        private void importarMUS_Click(object sender, EventArgs e)
        {
            this.ofd.Filter = WYZTracker.Properties.Resources.INSFilter;
            if (this.ofd.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show(WYZTracker.Properties.Resources.YetToCome,
                    WYZTracker.Properties.Resources.Error,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            setFocusToEditor();
        }

        private void playAllClick_Click(object sender, EventArgs e)
        {
            if (this.songPlayer.Status == PlayStatus.Playing)
            {
                this.songPlayer.Stop();
            }
            else
            {
                this.songPlayer.PlayMode = PlayMode.FullSong;
                this.songPlayer.Play();
            }
            setFocusToEditor();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            for (; Application.OpenForms.Count > 1; )
            {
                if (Application.OpenForms[0] == this)
                {
                    Application.OpenForms[1].Close();
                }
                else
                {
                    Application.OpenForms[0].Close();
                }
            }

            this.songPlayer.Stop();

            base.OnClosing(e);
        }

        private void cboStereo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplicationState.Instance.Stereo = (Stereo)Enum.Parse(typeof(Stereo), (string)cboStereo.SelectedItem);
            this.songPlayer.Stereo = ApplicationState.Instance.Stereo;
            setFocusToEditor();
        }

        private void playPattern_Click(object sender, EventArgs e)
        {
            if (this.lboxPatterns.SelectedIndex != -1)
            {
                this.songPlayer.PlayMode = PlayMode.SinglePattern;
                this.songPlayer.Play(this.lboxPatterns.SelectedIndex);
            }
            setFocusToEditor();
        }

        private void lboxPatterns_Click(object sender, EventArgs e)
        {
            setFocusToEditor();
        }

        private void lboxPatterns_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.songPlayer.Status == PlayStatus.Playing)
            {
                for (int patIdx = 0; patIdx < lboxPatterns.Items.Count; patIdx++)
                {
                    Rectangle rect = this.lboxPatterns.GetItemRectangle(patIdx);
                    if (rect.Contains(e.Location))
                    {
                        this.songPlayer.GoToPattern(patIdx);
                        break;
                    }
                }
            }
        }

        private void patEditor_FileDropped(object sender, FileDroppedEventArgs e)
        {
            this.loadSongFromFile(e.FilePath);
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.patEditor.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.patEditor.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.patEditor.Paste();
        }

        private void pasteAsDelay_Click(object sender, EventArgs e)
        {
            this.patEditor.PasteAsDelay();
        }

        private void editorDeFrecuenciasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrequenciesTableEditor editor = FormFactory.CreateOrActivateFormOfType<FrequenciesTableEditor>();
            editor.CurrentSong = this.currentSong;
        }

        private void exportInstrumentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentInstrument != null && currentInstrument.ID != "R")
            {
                InstrumentsManager.Export(currentInstrument);
            }
        }

        private void exportarEfectoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentEffect != null)
            {
                FxManager.Export(this.currentEffect);
            }
        }

        private void frmDockedMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Block access to asynchronous processing of NextLine events, the form is 
            // about to be disposed.
            asyncThreadSemaphore.WaitOne();
            this.songPlayer.Stop();
        }

        private void cboEnvShape_SelectedIndexChanged(object sender, EventArgs e)
        {
            setFocusToEditor();
        }

        private void cboEnvRatio_SelectedIndexChanged(object sender, EventArgs e)
        {
            setFocusToEditor();
        }

        private void chkActiveFreqs_CheckedChanged(object sender, EventArgs e)
        {
            setFocusToEditor();
        }

        private void editorDeArpegiosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ArpeggioEditor editor = FormFactory.CreateOrActivateFormOfType<ArpeggioEditor>();
            editor.FormClosed -= new FormClosedEventHandler(arpeggioEditorClosed);
            editor.FormClosed += new FormClosedEventHandler(arpeggioEditorClosed);
            editor.CurrentSong = this.currentSong;
        }

        private void arpeggioEditorClosed(object sender, FormClosedEventArgs e)
        {
            ArpeggioEditor editor = sender as ArpeggioEditor;
            if (editor != null)
            {
                editor.FormClosed -= new FormClosedEventHandler(arpeggioEditorClosed);
            }
            this.fillPatternList();
            this.patEditor.Invalidate();
        }

        private void indexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "http://sites.google.com/site/augustoruiz";
            p.Start();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsForm f = FormFactory.CreateOrActivateFormOfType<OptionsForm>();
            f.FormClosed += new FormClosedEventHandler(optionsFormClosed);
        }

        void optionsFormClosed(object sender, FormClosedEventArgs e)
        {
            OptionsForm f = (OptionsForm)sender;
            if (f.DialogResult == DialogResult.OK)
            {
                this.patEditor.Invalidate();
            }
            f.FormClosed -= new FormClosedEventHandler(optionsFormClosed);
        }

        private void exportarAWAVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormFactory.CreateOrActivateFormOfType<ExportForm>();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlayStatus oldStatus = this.songPlayer.Status;
            if (oldStatus == PlayStatus.Playing)
            {
                this.songPlayer.Pause();
            }

            About about = new About();
            about.ShowDialog();

            if (oldStatus == PlayStatus.Playing)
            {
                this.songPlayer.Pause();
            }
        }

        private void txtDecrementoDelay_TextChanged(object sender, EventArgs e)
        {
            int value;
            if (!int.TryParse(txtDecrementoDelay.Text, out value))
            {
                txtDecrementoDelay.Text = "0";
            }
            else
            {
                this.patEditor.DelayDecrement = value;
            }
        }

        private void lboxPatterns_DrawItem(object sender, DrawItemEventArgs e)
        {
            Brush b;
            string text = lboxPatterns.Items[e.Index].ToString();

            e.DrawBackground();
            if (this.CurrentSong != null && this.currentSong.Looped && e.Index == this.CurrentSong.LoopToPattern)
            {
                b = new SolidBrush(Color.FromKnownColor(KnownColor.Red));
                text = "* " + text;
            } else
            {
                b = new SolidBrush(e.ForeColor);
            }
            e.Graphics.DrawString(text, e.Font, b, e.Bounds, StringFormat.GenericDefault);
            e.DrawFocusRectangle();
            b.Dispose();
        }
    }
}