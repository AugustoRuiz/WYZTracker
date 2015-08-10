namespace WYZPlayer
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.lstSongs = new System.Windows.Forms.ListBox();
            this.PlaylistItemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.pbPrevious = new System.Windows.Forms.PictureBox();
            this.pbStop = new System.Windows.Forms.PictureBox();
            this.pbPlay = new System.Windows.Forms.PictureBox();
            this.pbNext = new System.Windows.Forms.PictureBox();
            this.pbOpen = new System.Windows.Forms.PictureBox();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.pbRemove = new System.Windows.Forms.PictureBox();
            this.numLoops = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.tbVolume = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PlaylistItemBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPrevious)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbStop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNext)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOpen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRemove)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLoops)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbVolume)).BeginInit();
            this.SuspendLayout();
            // 
            // lstSongs
            // 
            this.lstSongs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstSongs.DataSource = this.PlaylistItemBindingSource;
            this.lstSongs.FormattingEnabled = true;
            this.lstSongs.Location = new System.Drawing.Point(12, 12);
            this.lstSongs.Name = "lstSongs";
            this.lstSongs.Size = new System.Drawing.Size(271, 173);
            this.lstSongs.TabIndex = 0;
            // 
            // PlaylistItemBindingSource
            // 
            this.PlaylistItemBindingSource.DataSource = typeof(WYZPlayer.PlaylistItem);
            // 
            // pbPrevious
            // 
            this.pbPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pbPrevious.Image = global::WYZPlayer.Properties.Resources.control_start;
            this.pbPrevious.Location = new System.Drawing.Point(34, 212);
            this.pbPrevious.Name = "pbPrevious";
            this.pbPrevious.Size = new System.Drawing.Size(16, 16);
            this.pbPrevious.TabIndex = 1;
            this.pbPrevious.TabStop = false;
            this.pbPrevious.Click += new System.EventHandler(this.pbPrevious_Click);
            this.pbPrevious.MouseEnter += new System.EventHandler(this.pbPrevious_MouseEnter);
            this.pbPrevious.MouseLeave += new System.EventHandler(this.pbPrevious_MouseLeave);
            // 
            // pbStop
            // 
            this.pbStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pbStop.Image = global::WYZPlayer.Properties.Resources.control_stop;
            this.pbStop.Location = new System.Drawing.Point(12, 212);
            this.pbStop.Name = "pbStop";
            this.pbStop.Size = new System.Drawing.Size(16, 16);
            this.pbStop.TabIndex = 2;
            this.pbStop.TabStop = false;
            this.pbStop.Click += new System.EventHandler(this.pbStop_Click);
            this.pbStop.MouseEnter += new System.EventHandler(this.pbStop_MouseEnter);
            this.pbStop.MouseLeave += new System.EventHandler(this.pbStop_MouseLeave);
            // 
            // pbPlay
            // 
            this.pbPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pbPlay.Image = global::WYZPlayer.Properties.Resources.control_play;
            this.pbPlay.Location = new System.Drawing.Point(56, 212);
            this.pbPlay.Name = "pbPlay";
            this.pbPlay.Size = new System.Drawing.Size(16, 16);
            this.pbPlay.TabIndex = 1;
            this.pbPlay.TabStop = false;
            this.pbPlay.Click += new System.EventHandler(this.pbPlay_Click);
            this.pbPlay.MouseEnter += new System.EventHandler(this.pbPlay_MouseEnter);
            this.pbPlay.MouseLeave += new System.EventHandler(this.pbPlay_MouseLeave);
            // 
            // pbNext
            // 
            this.pbNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pbNext.Image = global::WYZPlayer.Properties.Resources.control_end;
            this.pbNext.Location = new System.Drawing.Point(78, 212);
            this.pbNext.Name = "pbNext";
            this.pbNext.Size = new System.Drawing.Size(16, 16);
            this.pbNext.TabIndex = 1;
            this.pbNext.TabStop = false;
            this.pbNext.Click += new System.EventHandler(this.pbNext_Click);
            this.pbNext.MouseEnter += new System.EventHandler(this.pbNext_MouseEnter);
            this.pbNext.MouseLeave += new System.EventHandler(this.pbNext_MouseLeave);
            // 
            // pbOpen
            // 
            this.pbOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pbOpen.Image = global::WYZPlayer.Properties.Resources.folder;
            this.pbOpen.Location = new System.Drawing.Point(267, 212);
            this.pbOpen.Name = "pbOpen";
            this.pbOpen.Size = new System.Drawing.Size(16, 16);
            this.pbOpen.TabIndex = 1;
            this.pbOpen.TabStop = false;
            this.pbOpen.Click += new System.EventHandler(this.pbOpen_Click);
            // 
            // ofd
            // 
            this.ofd.Multiselect = true;
            // 
            // pbRemove
            // 
            this.pbRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pbRemove.Image = global::WYZPlayer.Properties.Resources.cross;
            this.pbRemove.Location = new System.Drawing.Point(245, 212);
            this.pbRemove.Name = "pbRemove";
            this.pbRemove.Size = new System.Drawing.Size(16, 16);
            this.pbRemove.TabIndex = 1;
            this.pbRemove.TabStop = false;
            this.pbRemove.Click += new System.EventHandler(this.pbRemove_Click);
            // 
            // numLoops
            // 
            this.numLoops.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numLoops.Location = new System.Drawing.Point(145, 211);
            this.numLoops.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numLoops.Name = "numLoops";
            this.numLoops.Size = new System.Drawing.Size(49, 20);
            this.numLoops.TabIndex = 3;
            this.numLoops.ValueChanged += new System.EventHandler(this.numLoops_ValueChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(100, 213);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Loops:";
            // 
            // tbVolume
            // 
            this.tbVolume.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbVolume.Location = new System.Drawing.Point(34, 187);
            this.tbVolume.Maximum = 100;
            this.tbVolume.Name = "tbVolume";
            this.tbVolume.Size = new System.Drawing.Size(249, 45);
            this.tbVolume.TabIndex = 5;
            this.tbVolume.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbVolume.Value = 100;
            this.tbVolume.ValueChanged += new System.EventHandler(this.tbVolume_ValueChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 190);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Vol:";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(295, 235);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numLoops);
            this.Controls.Add(this.pbStop);
            this.Controls.Add(this.pbRemove);
            this.Controls.Add(this.pbOpen);
            this.Controls.Add(this.pbNext);
            this.Controls.Add(this.pbPlay);
            this.Controls.Add(this.pbPrevious);
            this.Controls.Add(this.lstSongs);
            this.Controls.Add(this.tbVolume);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(311, 150);
            this.Name = "frmMain";
            this.Text = "WYZPlayer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.PlaylistItemBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPrevious)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbStop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNext)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOpen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRemove)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLoops)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbVolume)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstSongs;
        private System.Windows.Forms.PictureBox pbPrevious;
        private System.Windows.Forms.PictureBox pbStop;
        private System.Windows.Forms.PictureBox pbPlay;
        private System.Windows.Forms.PictureBox pbNext;
        private System.Windows.Forms.BindingSource PlaylistItemBindingSource;
        private System.Windows.Forms.PictureBox pbOpen;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.PictureBox pbRemove;
        private System.Windows.Forms.NumericUpDown numLoops;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar tbVolume;
        private System.Windows.Forms.Label label2;
    }
}

