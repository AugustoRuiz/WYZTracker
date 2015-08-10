namespace WYZTracker
{
    partial class PatternEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PatternEditor));
            this.contextMenuPatView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.transponer1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transponer1ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.transponerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transposeTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.octava1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.octava1ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.establecerInstrumentoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tempoPlusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tempoMinusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cboFxChannel = new System.Windows.Forms.ComboBox();
            this.patView = new WYZTracker.PatternView();
            this.contextMenuPatView.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuPatView
            // 
            this.contextMenuPatView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.transponer1ToolStripMenuItem,
            this.transponer1ToolStripMenuItem1,
            this.transponerToolStripMenuItem,
            this.toolStripSeparator2,
            this.octava1ToolStripMenuItem,
            this.octava1ToolStripMenuItem1,
            this.toolStripSeparator1,
            this.establecerInstrumentoToolStripMenuItem,
            this.toolStripSeparator3,
            this.tempoPlusToolStripMenuItem,
            this.tempoMinusToolStripMenuItem});
            this.contextMenuPatView.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuPatView, "contextMenuPatView");
            this.contextMenuPatView.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.contextMenuPatView_Closed);
            this.contextMenuPatView.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // transponer1ToolStripMenuItem
            // 
            this.transponer1ToolStripMenuItem.Name = "transponer1ToolStripMenuItem";
            resources.ApplyResources(this.transponer1ToolStripMenuItem, "transponer1ToolStripMenuItem");
            this.transponer1ToolStripMenuItem.Click += new System.EventHandler(this.transponer1ToolStripMenuItem_Click);
            // 
            // transponer1ToolStripMenuItem1
            // 
            this.transponer1ToolStripMenuItem1.Name = "transponer1ToolStripMenuItem1";
            resources.ApplyResources(this.transponer1ToolStripMenuItem1, "transponer1ToolStripMenuItem1");
            this.transponer1ToolStripMenuItem1.Click += new System.EventHandler(this.transponer1ToolStripMenuItem1_Click);
            // 
            // transponerToolStripMenuItem
            // 
            this.transponerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.transposeTextBox});
            this.transponerToolStripMenuItem.Name = "transponerToolStripMenuItem";
            resources.ApplyResources(this.transponerToolStripMenuItem, "transponerToolStripMenuItem");
            // 
            // transposeTextBox
            // 
            this.transposeTextBox.Name = "transposeTextBox";
            resources.ApplyResources(this.transposeTextBox, "transposeTextBox");
            this.transposeTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.transposeTextBox_KeyUp);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // octava1ToolStripMenuItem
            // 
            this.octava1ToolStripMenuItem.Name = "octava1ToolStripMenuItem";
            resources.ApplyResources(this.octava1ToolStripMenuItem, "octava1ToolStripMenuItem");
            this.octava1ToolStripMenuItem.Click += new System.EventHandler(this.octava1ToolStripMenuItem_Click);
            // 
            // octava1ToolStripMenuItem1
            // 
            this.octava1ToolStripMenuItem1.Name = "octava1ToolStripMenuItem1";
            resources.ApplyResources(this.octava1ToolStripMenuItem1, "octava1ToolStripMenuItem1");
            this.octava1ToolStripMenuItem1.Click += new System.EventHandler(this.octava1ToolStripMenuItem1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // establecerInstrumentoToolStripMenuItem
            // 
            this.establecerInstrumentoToolStripMenuItem.Name = "establecerInstrumentoToolStripMenuItem";
            resources.ApplyResources(this.establecerInstrumentoToolStripMenuItem, "establecerInstrumentoToolStripMenuItem");
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // tempoPlusToolStripMenuItem
            // 
            this.tempoPlusToolStripMenuItem.Name = "tempoPlusToolStripMenuItem";
            resources.ApplyResources(this.tempoPlusToolStripMenuItem, "tempoPlusToolStripMenuItem");
            this.tempoPlusToolStripMenuItem.Click += new System.EventHandler(this.tempoPlusToolStripMenuItem_Click);
            // 
            // tempoMinusToolStripMenuItem
            // 
            this.tempoMinusToolStripMenuItem.Name = "tempoMinusToolStripMenuItem";
            resources.ApplyResources(this.tempoMinusToolStripMenuItem, "tempoMinusToolStripMenuItem");
            this.tempoMinusToolStripMenuItem.Click += new System.EventHandler(this.tempoMinusToolStripMenuItem_Click);
            // 
            // cboFxChannel
            // 
            this.cboFxChannel.FormattingEnabled = true;
            resources.ApplyResources(this.cboFxChannel, "cboFxChannel");
            this.cboFxChannel.Name = "cboFxChannel";
            this.cboFxChannel.SelectedIndexChanged += new System.EventHandler(this.cboFxChannel_SelectedIndexChanged);
            // 
            // patView
            // 
            this.patView.AdvancedMode = false;
            this.patView.AllowDrop = true;
            resources.ApplyResources(this.patView, "patView");
            this.patView.BackColor = System.Drawing.Color.Black;
            this.patView.ContextMenuStrip = this.contextMenuPatView;
            this.patView.CurrentChannel = 0;
            this.patView.CurrentInstrument = null;
            this.patView.CurrentPattern = null;
            this.patView.CurrentSong = null;
            this.patView.DelayDecrement = 0;
            this.patView.EditionIncrement = 0;
            this.patView.FirstVisibleNoteIndex = 0;
            this.patView.ForeColor = System.Drawing.Color.Lime;
            this.patView.HighlightBackColor = System.Drawing.Color.Green;
            this.patView.HighlightBackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.patView.HighlightForeColor = System.Drawing.Color.Yellow;
            this.patView.HighlightRange = 4;
            this.patView.Name = "patView";
            this.patView.SelectedIndex = 0;
            this.patView.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.patView.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.patView.Play += new System.EventHandler<WYZTracker.PlayEventArgs>(this.patView_Play);
            this.patView.Stop += new System.EventHandler(this.patView_Stop);
            this.patView.IncreaseOctave += new System.EventHandler(this.patView_IncreaseOctave);
            this.patView.DecreaseOctave += new System.EventHandler(this.patView_DecreaseOctave);
            this.patView.EditionIncrementChanged += new System.EventHandler(this.patView_EditionIncrementChanged);
            this.patView.HighlightRangeChanged += new System.EventHandler(this.patView_HighlightRangeChanged);
            this.patView.IncreaseInstrument += new System.EventHandler(this.patView_IncreaseInstrument);
            this.patView.DecreaseInstrument += new System.EventHandler(this.patView_DecreaseInstrument);
            this.patView.NextPattern += new System.EventHandler(this.patView_NextPattern);
            this.patView.PreviousPattern += new System.EventHandler(this.patView_PreviousPattern);
            this.patView.SetActiveFx += new System.EventHandler<WYZTracker.ActiveFxEventArgs>(this.patView_SetActiveFx);
            this.patView.SetActiveInstrument += new System.EventHandler<WYZTracker.ActiveInstrumentEventArgs>(this.patView_SetActiveInstrument);
            this.patView.DragDrop += new System.Windows.Forms.DragEventHandler(this.patView_DragDrop);
            this.patView.DragEnter += new System.Windows.Forms.DragEventHandler(this.patView_DragEnter);
            // 
            // PatternEditor
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.patView);
            this.Controls.Add(this.cboFxChannel);
            this.Name = "PatternEditor";
            this.contextMenuPatView.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PatternView patView;
        private System.Windows.Forms.ContextMenuStrip contextMenuPatView;
        private System.Windows.Forms.ToolStripMenuItem transponer1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem transponer1ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem transponerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem establecerInstrumentoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem octava1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem octava1ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripTextBox transposeTextBox;
        private System.Windows.Forms.ComboBox cboFxChannel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem tempoPlusToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tempoMinusToolStripMenuItem;
    }
}