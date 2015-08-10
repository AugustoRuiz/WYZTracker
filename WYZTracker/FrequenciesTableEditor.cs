using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WYZTracker
{
    public partial class FrequenciesTableEditor : Form, ISongConsumer
    {
        private bool initializing = false;

        private FrequenciesDataSet freqDataSet = new FrequenciesDataSet();

        public FrequenciesTableEditor()
        {
            InitializeComponent();
        }

        private Song currentSong;

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
                    currentSong = value;
                    initializeControls();
                }
            }
        }

        private void initializeControls()
        {
            initializing = true;

            if (currentSong.DefaultMsxFreqs)
            {
                rbFreqMSX.Checked = currentSong.DefaultMsxFreqs;
            }
            if (currentSong.DefaultCpcFreqs)
            {
                rbFreqCPC.Checked = currentSong.DefaultCpcFreqs;
            }
            if (currentSong.ParameterizedFreqs)
            {
                tbParamFreq.Value = currentSong.ParameterValue; 
                rbFreqSlider.Checked = currentSong.ParameterizedFreqs;
            }
            if (currentSong.CustomFreqs)
            {
                tbParamFreq.Value = currentSong.ParameterValue;
                rbCustom.Checked = currentSong.CustomFreqs;
            }

            initializing = false;

            calculateFrequencies();
        }

        private void initializeBindingSource()
        {
            int notesCount = freqDataSet.NotesTable.Columns.Count - 1;
            freqDataSet.NotesTable.Clear();
            for (int rowIndex = 0; rowIndex < 5; rowIndex++)
            {
                FrequenciesDataSet.NotesTableRow row = freqDataSet.NotesTable.NewNotesTableRow();
                
                row.Octave = (Int16)(rowIndex + 2);

                for (int col = 0; col < notesCount; col++)
                {
                    row[col] = currentSong.Frequencies[rowIndex * notesCount + col + 2];
                }

                freqDataSet.NotesTable.Rows.Add(row);
            }
            this.gridNotas.DataSource = this.freqDataSet;
            this.gridNotas.DataMember = this.freqDataSet.NotesTable.TableName;

            this.gridNotas.Invalidate();
        }

        private void gridNotas_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int notesCount = freqDataSet.NotesTable.Columns.Count - 1;
            int noteIndex = (e.RowIndex *  notesCount) + e.ColumnIndex + 2;

            Int16 theValue;
            if(Int16.TryParse(gridNotas.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out theValue))
            {
                currentSong.Frequencies[noteIndex] = theValue;
                this.freqDataSet.AcceptChanges();
            }
        }

        private void cmdLoadFrequencies_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                currentSong.Frequencies = NoteFileLoader.LoadNotes(ofd.FileName);
                rbCustom.Checked = true;
            }
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                NoteFileLoader.SaveNotes(sfd.FileName, this.currentSong.Frequencies);
            }
        }

        private void frequenciesRadio_CheckedChanged(object sender, EventArgs e)
        {
            calculateFrequencies();
        }

        private void tbParamFreq_ValueChanged(object sender, EventArgs e)
        {
            calculateFrequencies();
        }

        private void calculateFrequencies()
        {
            if (!initializing && currentSong != null)
            {
                if (rbFreqMSX.Checked)
                {
                    tbParamFreq.Value = (int)LibAYEmu.ChipSpeedsByMachine.MSX;
                }

                if (rbFreqCPC.Checked)
                {
                    tbParamFreq.Value = (int)LibAYEmu.ChipSpeedsByMachine.CPC;
                }

                if (!rbCustom.Checked)
                {
                    Int16[] frequencies = NoteFileLoader.LoadDefaultNotes();
                    double factor = 1.0;
                    if (rbFreqCPC.Checked)
                    {
                        factor = (double)LibAYEmu.ChipSpeedsByMachine.CPC / (double)LibAYEmu.ChipSpeedsByMachine.MSX;
                    }
                    if (rbFreqSlider.Checked)
                    {
                        factor = (double)tbParamFreq.Value / (double)LibAYEmu.ChipSpeedsByMachine.MSX;
                    }
                    for (int i = 0; i < frequencies.Length; i++)
                    {
                        long newVal = (long)Math.Round(frequencies[i] * factor);
                        frequencies[i] = (newVal > Int16.MaxValue) ? Int16.MaxValue : newVal < Int16.MinValue ? Int16.MinValue : (Int16)newVal;
                    }
                    currentSong.Frequencies = frequencies;
                }
                gridNotas.ReadOnly = !(rbCustom.Checked);
                tbParamFreq.Enabled = rbFreqSlider.Checked;
                saveLastSelectedValues();
                initializeBindingSource();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            saveLastSelectedValues();
            base.OnClosed(e);
        }

        private void saveLastSelectedValues()
        {
            currentSong.DefaultMsxFreqs = rbFreqMSX.Checked;
            currentSong.DefaultCpcFreqs = rbFreqCPC.Checked;
            currentSong.ParameterizedFreqs = rbFreqSlider.Checked;
            currentSong.CustomFreqs = rbCustom.Checked;
            currentSong.ParameterValue = tbParamFreq.Value;
        }

    }
}
