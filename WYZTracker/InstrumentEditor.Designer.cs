namespace WYZTracker
{
    partial class InstrumentEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstrumentEditor));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lboxInstruments = new System.Windows.Forms.ListBox();
            this.instrumentsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportarInstrumentoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.instrumentsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.instrumentStrip = new System.Windows.Forms.ToolStrip();
            this.newInstrument = new System.Windows.Forms.ToolStripButton();
            this.deleteInstrument = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.importInstruments = new System.Windows.Forms.ToolStripButton();
            this.exportInstruments = new System.Windows.Forms.ToolStripButton();
            this.lblInstrList = new System.Windows.Forms.Label();
            this.chkLockMovement = new System.Windows.Forms.CheckBox();
            this.chkTesting = new System.Windows.Forms.CheckBox();
            this.numId = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.numLoopStart = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numLength = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.chkLoop = new System.Windows.Forms.CheckBox();
            this.pbWave = new System.Windows.Forms.PictureBox();
            this.ctxtModificadores = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menosUnaOctava = new System.Windows.Forms.ToolStripMenuItem();
            this.masUnaOctava = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.instrumentsContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.instrumentsBindingSource)).BeginInit();
            this.instrumentStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLoopStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbWave)).BeginInit();
            this.ctxtModificadores.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lboxInstruments);
            this.splitContainer1.Panel1.Controls.Add(this.instrumentStrip);
            this.splitContainer1.Panel1.Controls.Add(this.lblInstrList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.chkLockMovement);
            this.splitContainer1.Panel2.Controls.Add(this.chkTesting);
            this.splitContainer1.Panel2.Controls.Add(this.numId);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this.txtName);
            this.splitContainer1.Panel2.Controls.Add(this.numLoopStart);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.numLength);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.chkLoop);
            this.splitContainer1.Panel2.Controls.Add(this.pbWave);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            // 
            // lboxInstruments
            // 
            this.lboxInstruments.ContextMenuStrip = this.instrumentsContextMenu;
            this.lboxInstruments.DataSource = this.instrumentsBindingSource;
            this.lboxInstruments.DisplayMember = "Name";
            resources.ApplyResources(this.lboxInstruments, "lboxInstruments");
            this.lboxInstruments.FormattingEnabled = true;
            this.lboxInstruments.Name = "lboxInstruments";
            this.lboxInstruments.SelectedIndexChanged += new System.EventHandler(this.lstInstruments_SelectedIndexChanged);
            // 
            // instrumentsContextMenu
            // 
            this.instrumentsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportarInstrumentoToolStripMenuItem});
            this.instrumentsContextMenu.Name = "instrumentsContextMenu";
            resources.ApplyResources(this.instrumentsContextMenu, "instrumentsContextMenu");
            // 
            // exportarInstrumentoToolStripMenuItem
            // 
            this.exportarInstrumentoToolStripMenuItem.Name = "exportarInstrumentoToolStripMenuItem";
            resources.ApplyResources(this.exportarInstrumentoToolStripMenuItem, "exportarInstrumentoToolStripMenuItem");
            this.exportarInstrumentoToolStripMenuItem.Click += new System.EventHandler(this.exportarInstrumentoToolStripMenuItem_Click);
            // 
            // instrumentsBindingSource
            // 
            this.instrumentsBindingSource.DataSource = typeof(WYZTracker.Instruments);
            this.instrumentsBindingSource.CurrentChanged += new System.EventHandler(this.instrumentsBindingSource_CurrentChanged);
            // 
            // instrumentStrip
            // 
            resources.ApplyResources(this.instrumentStrip, "instrumentStrip");
            this.instrumentStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newInstrument,
            this.deleteInstrument,
            this.toolStripSeparator1,
            this.importInstruments,
            this.exportInstruments});
            this.instrumentStrip.Name = "instrumentStrip";
            // 
            // newInstrument
            // 
            this.newInstrument.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newInstrument.Image = global::WYZTracker.Properties.Resources.filenew;
            resources.ApplyResources(this.newInstrument, "newInstrument");
            this.newInstrument.Name = "newInstrument";
            this.newInstrument.Click += new System.EventHandler(this.newInstrument_Click);
            // 
            // deleteInstrument
            // 
            this.deleteInstrument.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.deleteInstrument.Image = global::WYZTracker.Properties.Resources.button_cancel;
            resources.ApplyResources(this.deleteInstrument, "deleteInstrument");
            this.deleteInstrument.Name = "deleteInstrument";
            this.deleteInstrument.Click += new System.EventHandler(this.deleteInstrument_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // importInstruments
            // 
            this.importInstruments.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.importInstruments.Image = global::WYZTracker.Properties.Resources.fileopen;
            resources.ApplyResources(this.importInstruments, "importInstruments");
            this.importInstruments.Name = "importInstruments";
            this.importInstruments.Click += new System.EventHandler(this.importInstruments_Click);
            // 
            // exportInstruments
            // 
            this.exportInstruments.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.exportInstruments.Image = global::WYZTracker.Properties.Resources.filesave;
            resources.ApplyResources(this.exportInstruments, "exportInstruments");
            this.exportInstruments.Name = "exportInstruments";
            this.exportInstruments.Click += new System.EventHandler(this.exportInstruments_Click);
            // 
            // lblInstrList
            // 
            resources.ApplyResources(this.lblInstrList, "lblInstrList");
            this.lblInstrList.Name = "lblInstrList";
            // 
            // chkLockMovement
            // 
            resources.ApplyResources(this.chkLockMovement, "chkLockMovement");
            this.chkLockMovement.Name = "chkLockMovement";
            this.chkLockMovement.UseVisualStyleBackColor = true;
            // 
            // chkTesting
            // 
            resources.ApplyResources(this.chkTesting, "chkTesting");
            this.chkTesting.Image = global::WYZTracker.Properties.Resources.keyboard;
            this.chkTesting.Name = "chkTesting";
            this.chkTesting.UseVisualStyleBackColor = true;
            this.chkTesting.CheckedChanged += new System.EventHandler(this.chkTesting_CheckedChanged);
            // 
            // numId
            // 
            resources.ApplyResources(this.numId, "numId");
            this.numId.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numId.Name = "numId";
            this.numId.ValueChanged += new System.EventHandler(this.numId_ValueChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // txtName
            // 
            resources.ApplyResources(this.txtName, "txtName");
            this.txtName.Name = "txtName";
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // numLoopStart
            // 
            resources.ApplyResources(this.numLoopStart, "numLoopStart");
            this.numLoopStart.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.numLoopStart.Name = "numLoopStart";
            this.numLoopStart.ValueChanged += new System.EventHandler(this.numLoopStart_ValueChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
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
            1,
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
            // chkLoop
            // 
            resources.ApplyResources(this.chkLoop, "chkLoop");
            this.chkLoop.Name = "chkLoop";
            this.chkLoop.UseVisualStyleBackColor = true;
            this.chkLoop.CheckedChanged += new System.EventHandler(this.chkLoop_CheckedChanged);
            // 
            // pbWave
            // 
            resources.ApplyResources(this.pbWave, "pbWave");
            this.pbWave.BackColor = System.Drawing.Color.Black;
            this.pbWave.ContextMenuStrip = this.ctxtModificadores;
            this.pbWave.Name = "pbWave";
            this.pbWave.TabStop = false;
            this.pbWave.Paint += new System.Windows.Forms.PaintEventHandler(this.pbWave_Paint);
            this.pbWave.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbWave_MouseDown);
            this.pbWave.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbWave_MouseMove);
            this.pbWave.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbWave_MouseUp);
            this.pbWave.Resize += new System.EventHandler(this.pbWave_Resize);
            // 
            // ctxtModificadores
            // 
            this.ctxtModificadores.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menosUnaOctava,
            this.masUnaOctava});
            this.ctxtModificadores.Name = "ctxtModificadores";
            resources.ApplyResources(this.ctxtModificadores, "ctxtModificadores");
            this.ctxtModificadores.Opening += new System.ComponentModel.CancelEventHandler(this.ctxtModificadores_Opening);
            // 
            // menosUnaOctava
            // 
            this.menosUnaOctava.Name = "menosUnaOctava";
            resources.ApplyResources(this.menosUnaOctava, "menosUnaOctava");
            this.menosUnaOctava.Click += new System.EventHandler(this.mediaFrecuencia_Click);
            // 
            // masUnaOctava
            // 
            this.masUnaOctava.Name = "masUnaOctava";
            resources.ApplyResources(this.masUnaOctava, "masUnaOctava");
            this.masUnaOctava.Click += new System.EventHandler(this.masUnaOctava_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // InstrumentEditor
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.KeyPreview = true;
            this.Name = "InstrumentEditor";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.instrumentsContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.instrumentsBindingSource)).EndInit();
            this.instrumentStrip.ResumeLayout(false);
            this.instrumentStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLoopStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbWave)).EndInit();
            this.ctxtModificadores.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip ctxtModificadores;
        private System.Windows.Forms.ToolStripMenuItem menosUnaOctava;
        private System.Windows.Forms.ToolStripMenuItem masUnaOctava;
        private System.Windows.Forms.BindingSource instrumentsBindingSource;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label lblInstrList;
        private System.Windows.Forms.ListBox lboxInstruments;
        private System.Windows.Forms.NumericUpDown numId;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.NumericUpDown numLoopStart;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numLength;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkLoop;
        private System.Windows.Forms.PictureBox pbWave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStrip instrumentStrip;
        private System.Windows.Forms.ToolStripButton newInstrument;
        private System.Windows.Forms.ToolStripButton deleteInstrument;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton importInstruments;
        private System.Windows.Forms.ToolStripButton exportInstruments;
        private System.Windows.Forms.CheckBox chkTesting;
        private System.Windows.Forms.CheckBox chkLockMovement;
        private System.Windows.Forms.ContextMenuStrip instrumentsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem exportarInstrumentoToolStripMenuItem;
    }
}