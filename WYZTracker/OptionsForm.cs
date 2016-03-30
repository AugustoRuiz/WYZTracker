using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace WYZTracker
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
            loadControls();
        }

        private void loadControls()
        {
            cboIdioma.DataSource = LocalizationManager.SupportedCultures;
            cboIdioma.SelectedItem = LocalizationManager.GetCurrentCultureName();

            if (string.IsNullOrEmpty(Properties.Settings.Default.KeyboardLayout))
            {
                Properties.Settings.Default.KeyboardLayout = "QWERTY";
            }

            cboKeyboardLayout.SelectedItem = Properties.Settings.Default.KeyboardLayout;

            chkCheckFileAssociation.Checked = Properties.Settings.Default.CheckFileAssociation;
            chkSplash.Checked = Properties.Settings.Default.ShowSplash;
            chkDigitalFont.Checked = Properties.Settings.Default.UseCustomFont;

            numColWidth.Value = Properties.Settings.Default.ColumnWidth;
            numFontSize.Value = Properties.Settings.Default.FontSize;
            //numBufSize.Value = Properties.Settings.Default.SoundBufSize;
        }

        private void cmdOk_Click(object sender, EventArgs e)
        {
            string selectedLanguage = cboIdioma.SelectedItem as string;

            bool continueWithOk = true;
            bool languageChanged = selectedLanguage != LocalizationManager.GetCurrentCultureName();

            if (languageChanged)
            {

                System.Windows.Forms.DialogResult result = MessageBox.Show(
                    Properties.Resources.RestartMessage,
                    Properties.Resources.Warning,
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);

                switch (result)
                {
                    case System.Windows.Forms.DialogResult.Yes:
                        if (string.IsNullOrEmpty(ApplicationState.Instance.FileName))
                        {
                            this.sfd.Filter = WYZTracker.Properties.Resources.WYZFilter;
                            if (this.sfd.ShowDialog() == DialogResult.OK)
                            {
                                SongManager.SaveSong(ApplicationState.Instance.CurrentSong, this.sfd.FileName);
                            }
                        }
                        else
                        {
                            SongManager.SaveSong(ApplicationState.Instance.CurrentSong, ApplicationState.Instance.FileName);
                        }
                        break;
                    case System.Windows.Forms.DialogResult.Cancel:
                        continueWithOk = false;
                        break;
                }
            }

            if (continueWithOk)
            {
                Properties.Settings.Default.CheckFileAssociation = chkCheckFileAssociation.Checked;
                Properties.Settings.Default.ShowSplash = chkSplash.Checked;
                Properties.Settings.Default.UseCustomFont = chkDigitalFont.Checked;
                Properties.Settings.Default.Language = selectedLanguage;

                Properties.Settings.Default.ColumnWidth = (int)numColWidth.Value;
                Properties.Settings.Default.FontSize = (int)numFontSize.Value;
                Properties.Settings.Default.KeyboardLayout = cboKeyboardLayout.SelectedItem as string;

                //Properties.Settings.Default.SoundBufSize = (int)numBufSize.Value;

                Properties.Settings.Default.Save();

                LocalizationManager.LocalizeApplication(Properties.Settings.Default.Language);
                VirtualPiano.InitPianoKeys();
                //Player.BufferLengthInMs = Properties.Settings.Default.SoundBufSize;

                if (languageChanged)
                {
                    Program.Restart(ApplicationState.Instance.FileName);
                }
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
