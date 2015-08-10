namespace WYZTracker
{
    partial class ArpeggioEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArpeggioEditor));
            this.pbWave = new System.Windows.Forms.PictureBox();
            this.arpeggioDefBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.numLength = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.lblInstrument = new System.Windows.Forms.Label();
            this.cboInstruments = new System.Windows.Forms.ComboBox();
            this.instrumentsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.songBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.chkLockMovement = new System.Windows.Forms.CheckBox();
            this.lblPattern = new System.Windows.Forms.Label();
            this.cboPattern = new System.Windows.Forms.ComboBox();
            this.cmdOk = new System.Windows.Forms.Button();
            this.cboChannel = new System.Windows.Forms.ComboBox();
            this.lblChannel = new System.Windows.Forms.Label();
            this.lblPatternLength = new System.Windows.Forms.Label();
            this.numNewPatternLength = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.numOctave = new System.Windows.Forms.NumericUpDown();
            this.cboNote = new System.Windows.Forms.ComboBox();
            this.lblNote = new System.Windows.Forms.Label();
            this.chkTesting = new System.Windows.Forms.CheckBox();
            this.cmdLoadArpeggio = new System.Windows.Forms.Button();
            this.cmdSaveArpeggio = new System.Windows.Forms.Button();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.cboHighlightByScale = new System.Windows.Forms.ComboBox();
            this.cmdRandomize = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.numMaxOctaves = new System.Windows.Forms.NumericUpDown();
            this.pnlContainer = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pbWave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.arpeggioDefBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.instrumentsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.songBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNewPatternLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOctave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxOctaves)).BeginInit();
            this.pnlContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbWave
            // 
            resources.ApplyResources(this.pbWave, "pbWave");
            this.pbWave.BackColor = System.Drawing.Color.Black;
            this.pbWave.Name = "pbWave";
            this.pbWave.TabStop = false;
            this.pbWave.Paint += new System.Windows.Forms.PaintEventHandler(this.pbWave_Paint);
            this.pbWave.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbWave_MouseDown);
            this.pbWave.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbWave_MouseMove);
            this.pbWave.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbWave_MouseUp);
            this.pbWave.Resize += new System.EventHandler(this.pbWave_Resize);
            // 
            // arpeggioDefBindingSource
            // 
            this.arpeggioDefBindingSource.DataSource = typeof(WYZTracker.ArpeggioDefinition);
            // 
            // numLength
            // 
            resources.ApplyResources(this.numLength, "numLength");
            this.numLength.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.numLength.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numLength.Name = "numLength";
            this.numLength.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numLength.ValueChanged += new System.EventHandler(this.numLength_ValueChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // lblInstrument
            // 
            resources.ApplyResources(this.lblInstrument, "lblInstrument");
            this.lblInstrument.Name = "lblInstrument";
            // 
            // cboInstruments
            // 
            resources.ApplyResources(this.cboInstruments, "cboInstruments");
            this.cboInstruments.DataSource = this.instrumentsBindingSource;
            this.cboInstruments.DisplayMember = "Name";
            this.cboInstruments.FormattingEnabled = true;
            this.cboInstruments.Name = "cboInstruments";
            this.cboInstruments.ValueMember = "ID";
            this.cboInstruments.SelectedIndexChanged += new System.EventHandler(this.cboInstruments_SelectedIndexChanged);
            // 
            // instrumentsBindingSource
            // 
            this.instrumentsBindingSource.DataMember = "Instruments";
            this.instrumentsBindingSource.DataSource = this.songBindingSource;
            // 
            // songBindingSource
            // 
            this.songBindingSource.DataSource = typeof(WYZTracker.Song);
            // 
            // chkLockMovement
            // 
            resources.ApplyResources(this.chkLockMovement, "chkLockMovement");
            this.chkLockMovement.Name = "chkLockMovement";
            this.chkLockMovement.UseVisualStyleBackColor = true;
            // 
            // lblPattern
            // 
            resources.ApplyResources(this.lblPattern, "lblPattern");
            this.lblPattern.Name = "lblPattern";
            // 
            // cboPattern
            // 
            resources.ApplyResources(this.cboPattern, "cboPattern");
            this.cboPattern.DisplayMember = "Value";
            this.cboPattern.FormattingEnabled = true;
            this.cboPattern.Name = "cboPattern";
            this.cboPattern.ValueMember = "Key";
            this.cboPattern.SelectedIndexChanged += new System.EventHandler(this.cboPattern_SelectedIndexChanged);
            // 
            // cmdOk
            // 
            resources.ApplyResources(this.cmdOk, "cmdOk");
            this.cmdOk.Image = global::WYZTracker.Properties.Resources.accept;
            this.cmdOk.Name = "cmdOk";
            this.cmdOk.UseVisualStyleBackColor = true;
            this.cmdOk.Click += new System.EventHandler(this.cmdOk_Click);
            // 
            // cboChannel
            // 
            resources.ApplyResources(this.cboChannel, "cboChannel");
            this.cboChannel.DisplayMember = "Value";
            this.cboChannel.FormattingEnabled = true;
            this.cboChannel.Name = "cboChannel";
            this.cboChannel.ValueMember = "Key";
            this.cboChannel.SelectedIndexChanged += new System.EventHandler(this.cboChannel_SelectedIndexChanged);
            // 
            // lblChannel
            // 
            resources.ApplyResources(this.lblChannel, "lblChannel");
            this.lblChannel.Name = "lblChannel";
            // 
            // lblPatternLength
            // 
            resources.ApplyResources(this.lblPatternLength, "lblPatternLength");
            this.lblPatternLength.Name = "lblPatternLength";
            // 
            // numNewPatternLength
            // 
            resources.ApplyResources(this.numNewPatternLength, "numNewPatternLength");
            this.numNewPatternLength.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numNewPatternLength.Name = "numNewPatternLength";
            this.numNewPatternLength.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numNewPatternLength.ValueChanged += new System.EventHandler(this.numNewPatternLength_ValueChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // numOctave
            // 
            resources.ApplyResources(this.numOctave, "numOctave");
            this.numOctave.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.numOctave.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numOctave.Name = "numOctave";
            this.numOctave.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numOctave.ValueChanged += new System.EventHandler(this.numOctave_ValueChanged);
            // 
            // cboNote
            // 
            resources.ApplyResources(this.cboNote, "cboNote");
            this.cboNote.FormattingEnabled = true;
            this.cboNote.Items.AddRange(new object[] {
            resources.GetString("cboNote.Items"),
            resources.GetString("cboNote.Items1"),
            resources.GetString("cboNote.Items2"),
            resources.GetString("cboNote.Items3"),
            resources.GetString("cboNote.Items4"),
            resources.GetString("cboNote.Items5"),
            resources.GetString("cboNote.Items6"),
            resources.GetString("cboNote.Items7"),
            resources.GetString("cboNote.Items8"),
            resources.GetString("cboNote.Items9"),
            resources.GetString("cboNote.Items10"),
            resources.GetString("cboNote.Items11")});
            this.cboNote.Name = "cboNote";
            this.cboNote.SelectedIndexChanged += new System.EventHandler(this.cboNote_SelectedIndexChanged);
            // 
            // lblNote
            // 
            resources.ApplyResources(this.lblNote, "lblNote");
            this.lblNote.Name = "lblNote";
            // 
            // chkTesting
            // 
            resources.ApplyResources(this.chkTesting, "chkTesting");
            this.chkTesting.Image = global::WYZTracker.Properties.Resources.keyboard;
            this.chkTesting.Name = "chkTesting";
            this.chkTesting.UseVisualStyleBackColor = true;
            this.chkTesting.CheckedChanged += new System.EventHandler(this.chkTesting_CheckedChanged);
            // 
            // cmdLoadArpeggio
            // 
            resources.ApplyResources(this.cmdLoadArpeggio, "cmdLoadArpeggio");
            this.cmdLoadArpeggio.Image = global::WYZTracker.Properties.Resources.fileopen;
            this.cmdLoadArpeggio.Name = "cmdLoadArpeggio";
            this.cmdLoadArpeggio.UseVisualStyleBackColor = true;
            this.cmdLoadArpeggio.Click += new System.EventHandler(this.cmdLoadArpeggio_Click);
            // 
            // cmdSaveArpeggio
            // 
            resources.ApplyResources(this.cmdSaveArpeggio, "cmdSaveArpeggio");
            this.cmdSaveArpeggio.Image = global::WYZTracker.Properties.Resources.filesave;
            this.cmdSaveArpeggio.Name = "cmdSaveArpeggio";
            this.cmdSaveArpeggio.UseVisualStyleBackColor = true;
            this.cmdSaveArpeggio.Click += new System.EventHandler(this.cmdSaveArpeggio_Click);
            // 
            // ofd
            // 
            resources.ApplyResources(this.ofd, "ofd");
            // 
            // sfd
            // 
            resources.ApplyResources(this.sfd, "sfd");
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cboHighlightByScale
            // 
            resources.ApplyResources(this.cboHighlightByScale, "cboHighlightByScale");
            this.cboHighlightByScale.DisplayMember = "Value";
            this.cboHighlightByScale.FormattingEnabled = true;
            this.cboHighlightByScale.Name = "cboHighlightByScale";
            this.cboHighlightByScale.ValueMember = "Key";
            this.cboHighlightByScale.SelectedIndexChanged += new System.EventHandler(this.cboHighlightByScale_SelectedIndexChanged);
            // 
            // cmdRandomize
            // 
            resources.ApplyResources(this.cmdRandomize, "cmdRandomize");
            this.cmdRandomize.Name = "cmdRandomize";
            this.cmdRandomize.UseVisualStyleBackColor = true;
            this.cmdRandomize.Click += new System.EventHandler(this.cmdRandomize_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // numMaxOctaves
            // 
            resources.ApplyResources(this.numMaxOctaves, "numMaxOctaves");
            this.numMaxOctaves.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numMaxOctaves.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMaxOctaves.Name = "numMaxOctaves";
            this.numMaxOctaves.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numMaxOctaves.ValueChanged += new System.EventHandler(this.numMaxOctaves_ValueChanged);
            // 
            // pnlContainer
            // 
            resources.ApplyResources(this.pnlContainer, "pnlContainer");
            this.pnlContainer.BackColor = System.Drawing.Color.Black;
            this.pnlContainer.Controls.Add(this.pbWave);
            this.pnlContainer.Name = "pnlContainer";
            // 
            // ArpeggioEditor
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlContainer);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmdRandomize);
            this.Controls.Add(this.cboHighlightByScale);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmdSaveArpeggio);
            this.Controls.Add(this.cmdLoadArpeggio);
            this.Controls.Add(this.chkTesting);
            this.Controls.Add(this.lblNote);
            this.Controls.Add(this.cboNote);
            this.Controls.Add(this.numMaxOctaves);
            this.Controls.Add(this.numOctave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numNewPatternLength);
            this.Controls.Add(this.lblPatternLength);
            this.Controls.Add(this.cboChannel);
            this.Controls.Add(this.lblChannel);
            this.Controls.Add(this.cmdOk);
            this.Controls.Add(this.cboPattern);
            this.Controls.Add(this.lblPattern);
            this.Controls.Add(this.chkLockMovement);
            this.Controls.Add(this.cboInstruments);
            this.Controls.Add(this.lblInstrument);
            this.Controls.Add(this.numLength);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ArpeggioEditor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ArpeggioEditor_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pbWave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.arpeggioDefBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.instrumentsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.songBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNewPatternLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOctave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxOctaves)).EndInit();
            this.pnlContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbWave;
        private System.Windows.Forms.BindingSource arpeggioDefBindingSource;
        private System.Windows.Forms.NumericUpDown numLength;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblInstrument;
        private System.Windows.Forms.ComboBox cboInstruments;
        private System.Windows.Forms.BindingSource instrumentsBindingSource;
        private System.Windows.Forms.BindingSource songBindingSource;
        private System.Windows.Forms.CheckBox chkLockMovement;
        private System.Windows.Forms.Label lblPattern;
        private System.Windows.Forms.ComboBox cboPattern;
        private System.Windows.Forms.Button cmdOk;
        private System.Windows.Forms.ComboBox cboChannel;
        private System.Windows.Forms.Label lblChannel;
        private System.Windows.Forms.Label lblPatternLength;
        private System.Windows.Forms.NumericUpDown numNewPatternLength;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numOctave;
        private System.Windows.Forms.ComboBox cboNote;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.CheckBox chkTesting;
        private System.Windows.Forms.Button cmdLoadArpeggio;
        private System.Windows.Forms.Button cmdSaveArpeggio;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.SaveFileDialog sfd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboHighlightByScale;
        private System.Windows.Forms.Button cmdRandomize;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numMaxOctaves;
        private System.Windows.Forms.Panel pnlContainer;
    }
}