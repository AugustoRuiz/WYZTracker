namespace WYZTracker
{
    partial class SongProperties
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SongProperties));
            this.songTempo = new System.Windows.Forms.NumericUpDown();
            this.songBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblTempo = new System.Windows.Forms.Label();
            this.cboChannels = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSongName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkLooped = new System.Windows.Forms.CheckBox();
            this.cboFrequency = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.numLoopTo = new System.Windows.Forms.NumericUpDown();
            this.lblCurrentTempo = new System.Windows.Forms.Label();
            this.rbPAL = new System.Windows.Forms.RadioButton();
            this.rbNTSC = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.songTempo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.songBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLoopTo)).BeginInit();
            this.SuspendLayout();
            // 
            // songTempo
            // 
            resources.ApplyResources(this.songTempo, "songTempo");
            this.songTempo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.songBindingSource, "Tempo", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.songTempo.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.songTempo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.songTempo.Name = "songTempo";
            this.songTempo.Value = new decimal(new int[] {
            120,
            0,
            0,
            0});
            // 
            // songBindingSource
            // 
            this.songBindingSource.DataSource = typeof(WYZTracker.Song);
            // 
            // lblTempo
            // 
            resources.ApplyResources(this.lblTempo, "lblTempo");
            this.lblTempo.Name = "lblTempo";
            // 
            // cboChannels
            // 
            resources.ApplyResources(this.cboChannels, "cboChannels");
            this.cboChannels.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.songBindingSource, "Channels", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cboChannels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboChannels.FormattingEnabled = true;
            this.cboChannels.Items.AddRange(new object[] {
            resources.GetString("cboChannels.Items"),
            resources.GetString("cboChannels.Items1"),
            resources.GetString("cboChannels.Items2"),
            resources.GetString("cboChannels.Items3"),
            resources.GetString("cboChannels.Items4"),
            resources.GetString("cboChannels.Items5"),
            resources.GetString("cboChannels.Items6"),
            resources.GetString("cboChannels.Items7")});
            this.cboChannels.Name = "cboChannels";
            this.cboChannels.SelectedIndexChanged += new System.EventHandler(this.cboChannels_SelectedIndexChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txtSongName
            // 
            resources.ApplyResources(this.txtSongName, "txtSongName");
            this.txtSongName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.songBindingSource, "Name", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtSongName.Name = "txtSongName";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // chkLooped
            // 
            resources.ApplyResources(this.chkLooped, "chkLooped");
            this.chkLooped.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.songBindingSource, "Looped", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkLooped.Name = "chkLooped";
            this.chkLooped.UseVisualStyleBackColor = true;
            // 
            // cboFrequency
            // 
            resources.ApplyResources(this.cboFrequency, "cboFrequency");
            this.cboFrequency.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.songBindingSource, "ChipFrequency", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cboFrequency.DisplayMember = "Key";
            this.cboFrequency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFrequency.FormattingEnabled = true;
            this.cboFrequency.Name = "cboFrequency";
            this.cboFrequency.ValueMember = "Value";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // numLoopTo
            // 
            resources.ApplyResources(this.numLoopTo, "numLoopTo");
            this.numLoopTo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.songBindingSource, "LoopToPattern", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numLoopTo.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numLoopTo.Name = "numLoopTo";
            this.numLoopTo.ValueChanged += new System.EventHandler(this.numLoopTo_ValueChanged);
            // 
            // lblCurrentTempo
            // 
            resources.ApplyResources(this.lblCurrentTempo, "lblCurrentTempo");
            this.lblCurrentTempo.Name = "lblCurrentTempo";
            // 
            // rbPAL
            // 
            resources.ApplyResources(this.rbPAL, "rbPAL");
            this.rbPAL.Name = "rbPAL";
            this.rbPAL.TabStop = true;
            this.rbPAL.UseVisualStyleBackColor = true;
            this.rbPAL.CheckedChanged += new System.EventHandler(this.rbPAL_CheckedChanged);
            // 
            // rbNTSC
            // 
            resources.ApplyResources(this.rbNTSC, "rbNTSC");
            this.rbNTSC.Name = "rbNTSC";
            this.rbNTSC.TabStop = true;
            this.rbNTSC.UseVisualStyleBackColor = true;
            this.rbNTSC.CheckedChanged += new System.EventHandler(this.rbNTSC_CheckedChanged);
            // 
            // SongProperties
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rbNTSC);
            this.Controls.Add(this.rbPAL);
            this.Controls.Add(this.lblCurrentTempo);
            this.Controls.Add(this.numLoopTo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboFrequency);
            this.Controls.Add(this.chkLooped);
            this.Controls.Add(this.songTempo);
            this.Controls.Add(this.lblTempo);
            this.Controls.Add(this.cboChannels);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSongName);
            this.Controls.Add(this.label1);
            this.Name = "SongProperties";
            ((System.ComponentModel.ISupportInitialize)(this.songTempo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.songBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLoopTo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown songTempo;
        private System.Windows.Forms.Label lblTempo;
        private System.Windows.Forms.ComboBox cboChannels;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSongName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.BindingSource songBindingSource;
        private System.Windows.Forms.CheckBox chkLooped;
        private System.Windows.Forms.ComboBox cboFrequency;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numLoopTo;
        private System.Windows.Forms.Label lblCurrentTempo;
        private System.Windows.Forms.RadioButton rbPAL;
        private System.Windows.Forms.RadioButton rbNTSC;
    }
}