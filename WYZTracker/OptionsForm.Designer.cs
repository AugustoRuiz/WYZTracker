namespace WYZTracker
{
    partial class OptionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.lblLanguage = new System.Windows.Forms.Label();
            this.chkCheckFileAssociation = new System.Windows.Forms.CheckBox();
            this.cmdOk = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.chkSplash = new System.Windows.Forms.CheckBox();
            this.chkDigitalFont = new System.Windows.Forms.CheckBox();
            this.numFontSize = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numColWidth = new System.Windows.Forms.NumericUpDown();
            this.lblTipoTeclado = new System.Windows.Forms.Label();
            this.cboKeyboardLayout = new System.Windows.Forms.ComboBox();
            this.cboIdioma = new WYZTracker.FlagComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.numFontSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numColWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // lblLanguage
            // 
            resources.ApplyResources(this.lblLanguage, "lblLanguage");
            this.lblLanguage.Name = "lblLanguage";
            // 
            // chkCheckFileAssociation
            // 
            resources.ApplyResources(this.chkCheckFileAssociation, "chkCheckFileAssociation");
            this.chkCheckFileAssociation.Name = "chkCheckFileAssociation";
            this.chkCheckFileAssociation.UseVisualStyleBackColor = true;
            // 
            // cmdOk
            // 
            this.cmdOk.Image = global::WYZTracker.Properties.Resources.accept;
            resources.ApplyResources(this.cmdOk, "cmdOk");
            this.cmdOk.Name = "cmdOk";
            this.cmdOk.UseVisualStyleBackColor = true;
            this.cmdOk.Click += new System.EventHandler(this.cmdOk_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Image = global::WYZTracker.Properties.Resources.button_cancel;
            resources.ApplyResources(this.cmdCancel, "cmdCancel");
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // chkSplash
            // 
            resources.ApplyResources(this.chkSplash, "chkSplash");
            this.chkSplash.Name = "chkSplash";
            this.chkSplash.UseVisualStyleBackColor = true;
            // 
            // chkDigitalFont
            // 
            resources.ApplyResources(this.chkDigitalFont, "chkDigitalFont");
            this.chkDigitalFont.Name = "chkDigitalFont";
            this.chkDigitalFont.UseVisualStyleBackColor = true;
            // 
            // numFontSize
            // 
            resources.ApplyResources(this.numFontSize, "numFontSize");
            this.numFontSize.Maximum = new decimal(new int[] {
            96,
            0,
            0,
            0});
            this.numFontSize.Minimum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.numFontSize.Name = "numFontSize";
            this.numFontSize.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // numColWidth
            // 
            resources.ApplyResources(this.numColWidth, "numColWidth");
            this.numColWidth.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numColWidth.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numColWidth.Name = "numColWidth";
            this.numColWidth.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // lblTipoTeclado
            // 
            resources.ApplyResources(this.lblTipoTeclado, "lblTipoTeclado");
            this.lblTipoTeclado.Name = "lblTipoTeclado";
            // 
            // cboKeyboardLayout
            // 
            this.cboKeyboardLayout.FormattingEnabled = true;
            this.cboKeyboardLayout.Items.AddRange(new object[] {
            resources.GetString("cboKeyboardLayout.Items"),
            resources.GetString("cboKeyboardLayout.Items1"),
            resources.GetString("cboKeyboardLayout.Items2"),
            resources.GetString("cboKeyboardLayout.Items3")});
            resources.ApplyResources(this.cboKeyboardLayout, "cboKeyboardLayout");
            this.cboKeyboardLayout.Name = "cboKeyboardLayout";
            // 
            // cboIdioma
            // 
            this.cboIdioma.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboIdioma.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboIdioma.FormattingEnabled = true;
            resources.ApplyResources(this.cboIdioma, "cboIdioma");
            this.cboIdioma.Name = "cboIdioma";
            // 
            // OptionsForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cboKeyboardLayout);
            this.Controls.Add(this.lblTipoTeclado);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numColWidth);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numFontSize);
            this.Controls.Add(this.chkDigitalFont);
            this.Controls.Add(this.chkSplash);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOk);
            this.Controls.Add(this.chkCheckFileAssociation);
            this.Controls.Add(this.cboIdioma);
            this.Controls.Add(this.lblLanguage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "OptionsForm";
            ((System.ComponentModel.ISupportInitialize)(this.numFontSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numColWidth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLanguage;
        private FlagComboBox cboIdioma;
        private System.Windows.Forms.CheckBox chkCheckFileAssociation;
        private System.Windows.Forms.Button cmdOk;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.SaveFileDialog sfd;
        private System.Windows.Forms.CheckBox chkSplash;
        private System.Windows.Forms.CheckBox chkDigitalFont;
        private System.Windows.Forms.NumericUpDown numFontSize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numColWidth;
        private System.Windows.Forms.Label lblTipoTeclado;
        private System.Windows.Forms.ComboBox cboKeyboardLayout;
    }
}