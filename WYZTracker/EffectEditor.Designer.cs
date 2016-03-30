namespace WYZTracker
{
    partial class EffectEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EffectEditor));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lboxEffects = new System.Windows.Forms.ListBox();
            this.fxContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportarEfectoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.effectsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblEffectList = new System.Windows.Forms.Label();
            this.fxStrip = new System.Windows.Forms.ToolStrip();
            this.newFX = new System.Windows.Forms.ToolStripButton();
            this.deleteFX = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.importFX = new System.Windows.Forms.ToolStripButton();
            this.exportFX = new System.Windows.Forms.ToolStripButton();
            this.chkHex = new System.Windows.Forms.CheckBox();
            this.pnlEditorBars = new WYZTracker.FocusablePanel();
            this.chkTesting = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numId = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numLength = new System.Windows.Forms.NumericUpDown();
            this.txtName = new System.Windows.Forms.TextBox();
            this.envelopeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.noEnvelopeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.continueEnvelopeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.envelope00xxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.envelope01xxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.envelope1000ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.envelope1001ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.envelope1010ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.envelope1011ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.envelope1100ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.envelope1101ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.envelope1110ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.envelope1111ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tipCtl = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.fxContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.effectsBindingSource)).BeginInit();
            this.fxStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLength)).BeginInit();
            this.envelopeContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lboxEffects);
            this.splitContainer1.Panel1.Controls.Add(this.lblEffectList);
            this.splitContainer1.Panel1.Controls.Add(this.fxStrip);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.chkHex);
            this.splitContainer1.Panel2.Controls.Add(this.pnlEditorBars);
            this.splitContainer1.Panel2.Controls.Add(this.chkTesting);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.numId);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.numLength);
            this.splitContainer1.Panel2.Controls.Add(this.txtName);
            // 
            // lboxEffects
            // 
            this.lboxEffects.ContextMenuStrip = this.fxContextMenu;
            this.lboxEffects.DataSource = this.effectsBindingSource;
            this.lboxEffects.DisplayMember = "Name";
            resources.ApplyResources(this.lboxEffects, "lboxEffects");
            this.lboxEffects.FormattingEnabled = true;
            this.lboxEffects.Name = "lboxEffects";
            // 
            // fxContextMenu
            // 
            this.fxContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportarEfectoToolStripMenuItem});
            this.fxContextMenu.Name = "fxContextMenu";
            resources.ApplyResources(this.fxContextMenu, "fxContextMenu");
            // 
            // exportarEfectoToolStripMenuItem
            // 
            this.exportarEfectoToolStripMenuItem.Name = "exportarEfectoToolStripMenuItem";
            resources.ApplyResources(this.exportarEfectoToolStripMenuItem, "exportarEfectoToolStripMenuItem");
            this.exportarEfectoToolStripMenuItem.Click += new System.EventHandler(this.exportarEfectoToolStripMenuItem_Click);
            // 
            // effectsBindingSource
            // 
            this.effectsBindingSource.DataSource = typeof(WYZTracker.Effects);
            this.effectsBindingSource.CurrentChanged += new System.EventHandler(this.effectsBindingSource_CurrentChanged);
            // 
            // lblEffectList
            // 
            resources.ApplyResources(this.lblEffectList, "lblEffectList");
            this.lblEffectList.Name = "lblEffectList";
            // 
            // fxStrip
            // 
            resources.ApplyResources(this.fxStrip, "fxStrip");
            this.fxStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newFX,
            this.deleteFX,
            this.toolStripSeparator9,
            this.importFX,
            this.exportFX});
            this.fxStrip.Name = "fxStrip";
            // 
            // newFX
            // 
            this.newFX.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.newFX, "newFX");
            this.newFX.Name = "newFX";
            this.newFX.Click += new System.EventHandler(this.newFX_Click);
            // 
            // deleteFX
            // 
            this.deleteFX.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.deleteFX, "deleteFX");
            this.deleteFX.Name = "deleteFX";
            this.deleteFX.Click += new System.EventHandler(this.deleteFX_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            resources.ApplyResources(this.toolStripSeparator9, "toolStripSeparator9");
            // 
            // importFX
            // 
            this.importFX.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.importFX, "importFX");
            this.importFX.Name = "importFX";
            this.importFX.Click += new System.EventHandler(this.importFX_Click);
            // 
            // exportFX
            // 
            this.exportFX.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.exportFX, "exportFX");
            this.exportFX.Name = "exportFX";
            this.exportFX.Click += new System.EventHandler(this.exportFX_Click);
            // 
            // chkHex
            // 
            resources.ApplyResources(this.chkHex, "chkHex");
            this.chkHex.Checked = true;
            this.chkHex.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHex.Name = "chkHex";
            this.chkHex.UseVisualStyleBackColor = true;
            this.chkHex.CheckedChanged += new System.EventHandler(this.chkHex_CheckedChanged);
            // 
            // pnlEditorBars
            // 
            resources.ApplyResources(this.pnlEditorBars, "pnlEditorBars");
            this.pnlEditorBars.BackColor = System.Drawing.Color.Black;
            this.pnlEditorBars.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnlEditorBars.Name = "pnlEditorBars";
            this.pnlEditorBars.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EffectEditor_KeyDown);
            // 
            // chkTesting
            // 
            resources.ApplyResources(this.chkTesting, "chkTesting");
            this.chkTesting.Image = global::WYZTracker.Properties.Resources.keyboard;
            this.chkTesting.Name = "chkTesting";
            this.chkTesting.UseVisualStyleBackColor = true;
            this.chkTesting.CheckedChanged += new System.EventHandler(this.chkTesting_CheckedChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
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
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
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
            // txtName
            // 
            resources.ApplyResources(this.txtName, "txtName");
            this.txtName.Name = "txtName";
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // envelopeContextMenu
            // 
            this.envelopeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noEnvelopeToolStripMenuItem,
            this.continueEnvelopeToolStripMenuItem,
            this.envelope00xxToolStripMenuItem,
            this.envelope01xxToolStripMenuItem,
            this.envelope1000ToolStripMenuItem,
            this.envelope1001ToolStripMenuItem,
            this.envelope1010ToolStripMenuItem,
            this.envelope1011ToolStripMenuItem,
            this.envelope1100ToolStripMenuItem,
            this.envelope1101ToolStripMenuItem,
            this.envelope1110ToolStripMenuItem,
            this.envelope1111ToolStripMenuItem});
            this.envelopeContextMenu.Name = "envelopeContextMenu";
            resources.ApplyResources(this.envelopeContextMenu, "envelopeContextMenu");
            this.envelopeContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.envelopeContextMenu_Opening);
            this.envelopeContextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.envelopeContextMenu_ItemClicked);
            // 
            // noEnvelopeToolStripMenuItem
            // 
            this.noEnvelopeToolStripMenuItem.Image = global::WYZTracker.Properties.Resources.env_none;
            this.noEnvelopeToolStripMenuItem.Name = "noEnvelopeToolStripMenuItem";
            resources.ApplyResources(this.noEnvelopeToolStripMenuItem, "noEnvelopeToolStripMenuItem");
            this.noEnvelopeToolStripMenuItem.Tag = "-1";
            // 
            // continueEnvelopeToolStripMenuItem
            // 
            this.continueEnvelopeToolStripMenuItem.Image = global::WYZTracker.Properties.Resources.env_continue;
            this.continueEnvelopeToolStripMenuItem.Name = "continueEnvelopeToolStripMenuItem";
            resources.ApplyResources(this.continueEnvelopeToolStripMenuItem, "continueEnvelopeToolStripMenuItem");
            this.continueEnvelopeToolStripMenuItem.Tag = "1";
            // 
            // envelope00xxToolStripMenuItem
            // 
            this.envelope00xxToolStripMenuItem.Image = global::WYZTracker.Properties.Resources.env_0000;
            this.envelope00xxToolStripMenuItem.Name = "envelope00xxToolStripMenuItem";
            resources.ApplyResources(this.envelope00xxToolStripMenuItem, "envelope00xxToolStripMenuItem");
            this.envelope00xxToolStripMenuItem.Tag = "0";
            // 
            // envelope01xxToolStripMenuItem
            // 
            this.envelope01xxToolStripMenuItem.Image = global::WYZTracker.Properties.Resources.env_0100;
            this.envelope01xxToolStripMenuItem.Name = "envelope01xxToolStripMenuItem";
            resources.ApplyResources(this.envelope01xxToolStripMenuItem, "envelope01xxToolStripMenuItem");
            this.envelope01xxToolStripMenuItem.Tag = "4";
            // 
            // envelope1000ToolStripMenuItem
            // 
            this.envelope1000ToolStripMenuItem.Image = global::WYZTracker.Properties.Resources.env_1000;
            this.envelope1000ToolStripMenuItem.Name = "envelope1000ToolStripMenuItem";
            resources.ApplyResources(this.envelope1000ToolStripMenuItem, "envelope1000ToolStripMenuItem");
            this.envelope1000ToolStripMenuItem.Tag = "8";
            // 
            // envelope1001ToolStripMenuItem
            // 
            this.envelope1001ToolStripMenuItem.Image = global::WYZTracker.Properties.Resources.env_1001;
            this.envelope1001ToolStripMenuItem.Name = "envelope1001ToolStripMenuItem";
            resources.ApplyResources(this.envelope1001ToolStripMenuItem, "envelope1001ToolStripMenuItem");
            this.envelope1001ToolStripMenuItem.Tag = "9";
            // 
            // envelope1010ToolStripMenuItem
            // 
            this.envelope1010ToolStripMenuItem.Image = global::WYZTracker.Properties.Resources.env_1010;
            this.envelope1010ToolStripMenuItem.Name = "envelope1010ToolStripMenuItem";
            resources.ApplyResources(this.envelope1010ToolStripMenuItem, "envelope1010ToolStripMenuItem");
            this.envelope1010ToolStripMenuItem.Tag = "10";
            // 
            // envelope1011ToolStripMenuItem
            // 
            this.envelope1011ToolStripMenuItem.Image = global::WYZTracker.Properties.Resources.env_1011;
            this.envelope1011ToolStripMenuItem.Name = "envelope1011ToolStripMenuItem";
            resources.ApplyResources(this.envelope1011ToolStripMenuItem, "envelope1011ToolStripMenuItem");
            this.envelope1011ToolStripMenuItem.Tag = "11";
            // 
            // envelope1100ToolStripMenuItem
            // 
            this.envelope1100ToolStripMenuItem.Image = global::WYZTracker.Properties.Resources.env_1100;
            this.envelope1100ToolStripMenuItem.Name = "envelope1100ToolStripMenuItem";
            resources.ApplyResources(this.envelope1100ToolStripMenuItem, "envelope1100ToolStripMenuItem");
            this.envelope1100ToolStripMenuItem.Tag = "12";
            // 
            // envelope1101ToolStripMenuItem
            // 
            this.envelope1101ToolStripMenuItem.Image = global::WYZTracker.Properties.Resources.env_1101;
            this.envelope1101ToolStripMenuItem.Name = "envelope1101ToolStripMenuItem";
            resources.ApplyResources(this.envelope1101ToolStripMenuItem, "envelope1101ToolStripMenuItem");
            this.envelope1101ToolStripMenuItem.Tag = "13";
            // 
            // envelope1110ToolStripMenuItem
            // 
            this.envelope1110ToolStripMenuItem.Image = global::WYZTracker.Properties.Resources.env_1110;
            this.envelope1110ToolStripMenuItem.Name = "envelope1110ToolStripMenuItem";
            resources.ApplyResources(this.envelope1110ToolStripMenuItem, "envelope1110ToolStripMenuItem");
            this.envelope1110ToolStripMenuItem.Tag = "14";
            // 
            // envelope1111ToolStripMenuItem
            // 
            this.envelope1111ToolStripMenuItem.Image = global::WYZTracker.Properties.Resources.env_1111;
            this.envelope1111ToolStripMenuItem.Name = "envelope1111ToolStripMenuItem";
            resources.ApplyResources(this.envelope1111ToolStripMenuItem, "envelope1111ToolStripMenuItem");
            this.envelope1111ToolStripMenuItem.Tag = "15";
            // 
            // EffectEditor
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.KeyPreview = true;
            this.Name = "EffectEditor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EffectEditor_FormClosed);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.fxContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.effectsBindingSource)).EndInit();
            this.fxStrip.ResumeLayout(false);
            this.fxStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLength)).EndInit();
            this.envelopeContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numLength;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.BindingSource effectsBindingSource;
        private System.Windows.Forms.NumericUpDown numId;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox lboxEffects;
        private System.Windows.Forms.Label lblEffectList;
        private System.Windows.Forms.ToolStrip fxStrip;
        private System.Windows.Forms.ToolStripButton newFX;
        private System.Windows.Forms.ToolStripButton deleteFX;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripButton importFX;
        private System.Windows.Forms.ToolStripButton exportFX;
        private System.Windows.Forms.CheckBox chkTesting;
        private System.Windows.Forms.ContextMenuStrip fxContextMenu;
        private System.Windows.Forms.ToolStripMenuItem exportarEfectoToolStripMenuItem;
        private FocusablePanel pnlEditorBars;
        private System.Windows.Forms.ContextMenuStrip envelopeContextMenu;
        private System.Windows.Forms.ToolStripMenuItem noEnvelopeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem continueEnvelopeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem envelope00xxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem envelope01xxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem envelope1000ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem envelope1001ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem envelope1010ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem envelope1011ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem envelope1100ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem envelope1101ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem envelope1110ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem envelope1111ToolStripMenuItem;
        private System.Windows.Forms.ToolTip tipCtl;
        private System.Windows.Forms.CheckBox chkHex;
    }
}