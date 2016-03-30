using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WYZTracker
{
    public partial class ExportForm : Form
    {
        private int noteCount;

        public ExportForm()
        {
            InitializeComponent();
        }

        private void cmdOk_Click(object sender, EventArgs e)
        {
            noteCount = 0;
            
            pbProgreso.Minimum = 0;
            pbProgreso.Value = 0;

            cmdOk.Enabled = false;
            cmdCancel.Enabled = false;

            pbProgreso.Maximum = getNoteCount();

            string filePath = txtFilePath.Text;
            WorkerThread t = new WorkerThread()
            {
                FileName = filePath,
                NumRepeats = ApplicationState.Instance.CurrentSong.Looped ? (int)numRepeats.Value : 1,
                Parent = this
            };
            t.Export();
        }

        private int getNoteCount()
        {
            int total = 0;

            Song currSong = ApplicationState.Instance.CurrentSong;

            int multiplier = currSong.Looped ? (int) numRepeats.Value : 1;

            foreach (int patternIdx in currSong.PlayOrder)
            {
                Pattern current = currSong.Patterns[patternIdx];
                total += current.Length;
            }

            total *= multiplier;

            return total;
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdBrowse_Click(object sender, EventArgs e)
        {
            sfd.Filter = Properties.Resources.ExportFilter;
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtFilePath.Text = sfd.FileName;
            }
        }

        private void txtFilePath_TextChanged(object sender, EventArgs e)
        {
            cmdOk.Enabled = !string.IsNullOrEmpty(txtFilePath.Text);
        }

        public void NotifyCompleted()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new System.Threading.ThreadStart(this.NotifyCompleted));
            }
            else
            {
                MessageBox.Show("Ok", "WYZTracker", MessageBoxButtons.OK, MessageBoxIcon.Information);
                pbProgreso.Value = 0;
                cmdOk.Enabled = true;
                cmdCancel.Enabled = true;
            }
        }

        public void NotifyNextLine()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new System.Threading.ThreadStart(this.NotifyNextLine));
            }
            else
            {
                noteCount++;
                pbProgreso.Value = noteCount;
            }
        }

        private class WorkerThread
        {
            public ExportForm Parent { get; set; }

            public string FileName { get; set; }

            public int NumRepeats { get; set; }

            public void Export()
            {
                System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(doExport));
                t.IsBackground = true;
                t.Name = "WYZTracker Export thread";
                t.Start();
            }

            private void doExport()
            {
                if (!string.IsNullOrEmpty(FileName))
                {
                    string wavFileName = FileName;

                    if (System.IO.Path.GetExtension(FileName).ToLowerInvariant() == ".ogg")
                    {
                        wavFileName =
                            System.IO.Path.Combine(
                                System.IO.Path.GetDirectoryName(FileName),
                                string.Format("{0}.wav", System.IO.Path.GetFileNameWithoutExtension(FileName))
                            );
                    }

                    WaveFile wav = new WaveFile(2, 16, 44100, wavFileName);

                    Player p = new Player();
                    p.Stereo = ApplicationState.Instance.Stereo;
                    p.NextLine += new Player.NextLineEventHandler(updateNextLine);
                    p.SongFinished += new EventHandler(songFinished);
                    p.CurrentSong = ApplicationState.Instance.CurrentSong;
                    p.DumpToWavFile(wav, NumRepeats);

                    wav.Close();

                    if(wavFileName != FileName)
                    {
                        convertToOgg(wavFileName);
                    }

                    this.Parent.NotifyCompleted();
                }
            }

            private static void convertToOgg(string wavFileName)
            {
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = new System.Diagnostics.ProcessStartInfo(
                    System.IO.Path.Combine(
                        System.IO.Path.GetDirectoryName(Application.ExecutablePath),
                        "oggenc2.exe"));
                proc.StartInfo.Arguments = string.Format("\"{0}\"", wavFileName);
                proc.Start();
                
                while (!proc.HasExited)
                {
                    System.Threading.Thread.Sleep(0);
                }

                try
                {
                    System.IO.File.Delete(wavFileName);
                }
                catch { }
            }

            private void updateNextLine(object sender, NextLineEventArgs evArgs)
            {
                this.Parent.NotifyNextLine();
            }

            private void songFinished(object sender, EventArgs e)
            {
                Player p = (Player)sender;
                p.Stop();
            }
        }
    }
}
