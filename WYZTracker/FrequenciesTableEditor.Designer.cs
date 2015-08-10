namespace WYZTracker
{
    partial class FrequenciesTableEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrequenciesTableEditor));
            this.gridNotas = new System.Windows.Forms.DataGridView();
            this.octaveDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cSharpDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dSharpDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fSharpDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gSharpDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.aDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.aSharpDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.frequenciesDataSet = new WYZTracker.FrequenciesDataSet();
            this.lblTitle = new System.Windows.Forms.Label();
            this.cmdLoadFrequencies = new System.Windows.Forms.Button();
            this.cmdSave = new System.Windows.Forms.Button();
            this.frequenciesDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.rbFreqMSX = new System.Windows.Forms.RadioButton();
            this.rbFreqCPC = new System.Windows.Forms.RadioButton();
            this.rbFreqSlider = new System.Windows.Forms.RadioButton();
            this.tbParamFreq = new System.Windows.Forms.TrackBar();
            this.rbCustom = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.gridNotas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.frequenciesDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.frequenciesDataSetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbParamFreq)).BeginInit();
            this.SuspendLayout();
            // 
            // gridNotas
            // 
            resources.ApplyResources(this.gridNotas, "gridNotas");
            this.gridNotas.AllowUserToAddRows = false;
            this.gridNotas.AllowUserToDeleteRows = false;
            this.gridNotas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridNotas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.octaveDataGridViewTextBoxColumn,
            this.cDataGridViewTextBoxColumn,
            this.cSharpDataGridViewTextBoxColumn,
            this.dDataGridViewTextBoxColumn,
            this.dSharpDataGridViewTextBoxColumn,
            this.eDataGridViewTextBoxColumn,
            this.fDataGridViewTextBoxColumn,
            this.fSharpDataGridViewTextBoxColumn,
            this.gDataGridViewTextBoxColumn,
            this.gSharpDataGridViewTextBoxColumn,
            this.aDataGridViewTextBoxColumn,
            this.aSharpDataGridViewTextBoxColumn,
            this.bDataGridViewTextBoxColumn});
            this.gridNotas.DataMember = "NotesTable";
            this.gridNotas.Name = "gridNotas";
            this.gridNotas.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridNotas_CellEndEdit);
            // 
            // octaveDataGridViewTextBoxColumn
            // 
            this.octaveDataGridViewTextBoxColumn.DataPropertyName = "Octave";
            this.octaveDataGridViewTextBoxColumn.Frozen = true;
            resources.ApplyResources(this.octaveDataGridViewTextBoxColumn, "octaveDataGridViewTextBoxColumn");
            this.octaveDataGridViewTextBoxColumn.Name = "octaveDataGridViewTextBoxColumn";
            this.octaveDataGridViewTextBoxColumn.ReadOnly = true;
            this.octaveDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // cDataGridViewTextBoxColumn
            // 
            this.cDataGridViewTextBoxColumn.DataPropertyName = "C";
            resources.ApplyResources(this.cDataGridViewTextBoxColumn, "cDataGridViewTextBoxColumn");
            this.cDataGridViewTextBoxColumn.Name = "cDataGridViewTextBoxColumn";
            this.cDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // cSharpDataGridViewTextBoxColumn
            // 
            this.cSharpDataGridViewTextBoxColumn.DataPropertyName = "CSharp";
            resources.ApplyResources(this.cSharpDataGridViewTextBoxColumn, "cSharpDataGridViewTextBoxColumn");
            this.cSharpDataGridViewTextBoxColumn.Name = "cSharpDataGridViewTextBoxColumn";
            this.cSharpDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // dDataGridViewTextBoxColumn
            // 
            this.dDataGridViewTextBoxColumn.DataPropertyName = "D";
            resources.ApplyResources(this.dDataGridViewTextBoxColumn, "dDataGridViewTextBoxColumn");
            this.dDataGridViewTextBoxColumn.Name = "dDataGridViewTextBoxColumn";
            this.dDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // dSharpDataGridViewTextBoxColumn
            // 
            this.dSharpDataGridViewTextBoxColumn.DataPropertyName = "DSharp";
            resources.ApplyResources(this.dSharpDataGridViewTextBoxColumn, "dSharpDataGridViewTextBoxColumn");
            this.dSharpDataGridViewTextBoxColumn.Name = "dSharpDataGridViewTextBoxColumn";
            this.dSharpDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // eDataGridViewTextBoxColumn
            // 
            this.eDataGridViewTextBoxColumn.DataPropertyName = "E";
            resources.ApplyResources(this.eDataGridViewTextBoxColumn, "eDataGridViewTextBoxColumn");
            this.eDataGridViewTextBoxColumn.Name = "eDataGridViewTextBoxColumn";
            this.eDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // fDataGridViewTextBoxColumn
            // 
            this.fDataGridViewTextBoxColumn.DataPropertyName = "F";
            resources.ApplyResources(this.fDataGridViewTextBoxColumn, "fDataGridViewTextBoxColumn");
            this.fDataGridViewTextBoxColumn.Name = "fDataGridViewTextBoxColumn";
            this.fDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // fSharpDataGridViewTextBoxColumn
            // 
            this.fSharpDataGridViewTextBoxColumn.DataPropertyName = "FSharp";
            resources.ApplyResources(this.fSharpDataGridViewTextBoxColumn, "fSharpDataGridViewTextBoxColumn");
            this.fSharpDataGridViewTextBoxColumn.Name = "fSharpDataGridViewTextBoxColumn";
            this.fSharpDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // gDataGridViewTextBoxColumn
            // 
            this.gDataGridViewTextBoxColumn.DataPropertyName = "G";
            resources.ApplyResources(this.gDataGridViewTextBoxColumn, "gDataGridViewTextBoxColumn");
            this.gDataGridViewTextBoxColumn.Name = "gDataGridViewTextBoxColumn";
            this.gDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // gSharpDataGridViewTextBoxColumn
            // 
            this.gSharpDataGridViewTextBoxColumn.DataPropertyName = "GSharp";
            resources.ApplyResources(this.gSharpDataGridViewTextBoxColumn, "gSharpDataGridViewTextBoxColumn");
            this.gSharpDataGridViewTextBoxColumn.Name = "gSharpDataGridViewTextBoxColumn";
            this.gSharpDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // aDataGridViewTextBoxColumn
            // 
            this.aDataGridViewTextBoxColumn.DataPropertyName = "A";
            resources.ApplyResources(this.aDataGridViewTextBoxColumn, "aDataGridViewTextBoxColumn");
            this.aDataGridViewTextBoxColumn.Name = "aDataGridViewTextBoxColumn";
            this.aDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // aSharpDataGridViewTextBoxColumn
            // 
            this.aSharpDataGridViewTextBoxColumn.DataPropertyName = "ASharp";
            resources.ApplyResources(this.aSharpDataGridViewTextBoxColumn, "aSharpDataGridViewTextBoxColumn");
            this.aSharpDataGridViewTextBoxColumn.Name = "aSharpDataGridViewTextBoxColumn";
            this.aSharpDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // bDataGridViewTextBoxColumn
            // 
            this.bDataGridViewTextBoxColumn.DataPropertyName = "B";
            resources.ApplyResources(this.bDataGridViewTextBoxColumn, "bDataGridViewTextBoxColumn");
            this.bDataGridViewTextBoxColumn.Name = "bDataGridViewTextBoxColumn";
            this.bDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // frequenciesDataSet
            // 
            this.frequenciesDataSet.DataSetName = "FrequenciesDataSet";
            this.frequenciesDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // lblTitle
            // 
            resources.ApplyResources(this.lblTitle, "lblTitle");
            this.lblTitle.Name = "lblTitle";
            // 
            // cmdLoadFrequencies
            // 
            resources.ApplyResources(this.cmdLoadFrequencies, "cmdLoadFrequencies");
            this.cmdLoadFrequencies.Image = global::WYZTracker.Properties.Resources.fileopen;
            this.cmdLoadFrequencies.Name = "cmdLoadFrequencies";
            this.cmdLoadFrequencies.UseVisualStyleBackColor = true;
            this.cmdLoadFrequencies.Click += new System.EventHandler(this.cmdLoadFrequencies_Click);
            // 
            // cmdSave
            // 
            resources.ApplyResources(this.cmdSave, "cmdSave");
            this.cmdSave.Image = global::WYZTracker.Properties.Resources.filesave;
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // frequenciesDataSetBindingSource
            // 
            this.frequenciesDataSetBindingSource.DataSource = this.frequenciesDataSet;
            this.frequenciesDataSetBindingSource.Position = 0;
            // 
            // rbFreqMSX
            // 
            resources.ApplyResources(this.rbFreqMSX, "rbFreqMSX");
            this.rbFreqMSX.Checked = true;
            this.rbFreqMSX.Name = "rbFreqMSX";
            this.rbFreqMSX.TabStop = true;
            this.rbFreqMSX.UseVisualStyleBackColor = true;
            this.rbFreqMSX.CheckedChanged += new System.EventHandler(this.frequenciesRadio_CheckedChanged);
            // 
            // rbFreqCPC
            // 
            resources.ApplyResources(this.rbFreqCPC, "rbFreqCPC");
            this.rbFreqCPC.Name = "rbFreqCPC";
            this.rbFreqCPC.UseVisualStyleBackColor = true;
            this.rbFreqCPC.CheckedChanged += new System.EventHandler(this.frequenciesRadio_CheckedChanged);
            // 
            // rbFreqSlider
            // 
            resources.ApplyResources(this.rbFreqSlider, "rbFreqSlider");
            this.rbFreqSlider.Name = "rbFreqSlider";
            this.rbFreqSlider.UseVisualStyleBackColor = true;
            this.rbFreqSlider.CheckedChanged += new System.EventHandler(this.frequenciesRadio_CheckedChanged);
            // 
            // tbParamFreq
            // 
            resources.ApplyResources(this.tbParamFreq, "tbParamFreq");
            this.tbParamFreq.Maximum = 3548000;
            this.tbParamFreq.Minimum = 500000;
            this.tbParamFreq.Name = "tbParamFreq";
            this.tbParamFreq.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbParamFreq.Value = 500000;
            this.tbParamFreq.ValueChanged += new System.EventHandler(this.tbParamFreq_ValueChanged);
            // 
            // rbCustom
            // 
            resources.ApplyResources(this.rbCustom, "rbCustom");
            this.rbCustom.Name = "rbCustom";
            this.rbCustom.UseVisualStyleBackColor = true;
            this.rbCustom.CheckedChanged += new System.EventHandler(this.frequenciesRadio_CheckedChanged);
            // 
            // FrequenciesTableEditor
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rbCustom);
            this.Controls.Add(this.gridNotas);
            this.Controls.Add(this.tbParamFreq);
            this.Controls.Add(this.rbFreqSlider);
            this.Controls.Add(this.rbFreqCPC);
            this.Controls.Add(this.rbFreqMSX);
            this.Controls.Add(this.cmdSave);
            this.Controls.Add(this.cmdLoadFrequencies);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FrequenciesTableEditor";
            ((System.ComponentModel.ISupportInitialize)(this.gridNotas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.frequenciesDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.frequenciesDataSetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbParamFreq)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView gridNotas;
        private FrequenciesDataSet frequenciesDataSet;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button cmdLoadFrequencies;
        private System.Windows.Forms.Button cmdSave;
        private System.Windows.Forms.BindingSource frequenciesDataSetBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn octaveDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cSharpDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dSharpDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fSharpDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn gDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn gSharpDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn aDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn aSharpDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bDataGridViewTextBoxColumn;
        private System.Windows.Forms.RadioButton rbFreqMSX;
        private System.Windows.Forms.RadioButton rbFreqCPC;
        private System.Windows.Forms.RadioButton rbFreqSlider;
        private System.Windows.Forms.TrackBar tbParamFreq;
        private System.Windows.Forms.RadioButton rbCustom;

    }
}