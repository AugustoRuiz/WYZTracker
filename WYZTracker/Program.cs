using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BrendanGrant.Helpers.FileAssociation;
using System.Diagnostics;

namespace WYZTracker
{
    static class Program
    {
        private static string CREATE_ASSOC_PARAM = "/createFileAssociation";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += 
                new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            bool audioInitialized = PlaybackStreamer.IsAudioAvailable();
            if (!audioInitialized)
            {
                bool installed = installOpenAL();
                if (installed)
                {
                    Restart(string.Empty);
                }
            }

            if (audioInitialized)
            {
                try
                {
                    if (args.Length > 1)
                    {
                        startSeveralInstances(args);
                    }
                    else
                    {
                        startSingleInstance(args);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.ToString());

                    string errorMessage = string.Format(WYZTracker.Properties.Resources.ApplicationError, Logger.LogPath);

                    MessageBox.Show(errorMessage,
                        WYZTracker.Properties.Resources.Error,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    if (audioInitialized)
                    {
                        PlaybackStreamer.StopAudio();
                    }
                }
            }

            VirtualPiano.ReleaseDevices();
            Application.Exit();
        }

        private static bool isMono
        {
            get
            {
                int p = (int) Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
            }
        }

        private static void startSingleInstance(string[] args)
        {
            if (args.Length == 1 && args[0] == CREATE_ASSOC_PARAM)
            {
                try
                {
                    createWYZFileAssociation();
                }
                catch(Exception ex)
                {
                    Logger.Log(ex.ToString());
                }
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Logger.FileName = "WYZTracker.log";

                LocalizationManager.LocalizeApplication(Properties.Settings.Default.Language);

                if (Properties.Settings.Default.ShowSplash)
                {
                    ApplicationState.Instance.SplashScreen = new Splash();
                    ApplicationState.Instance.SplashScreen.FadeIn();
                }

                try
                {
                    checkWYZFileAssociation();
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.ToString());
                }

                // Speed the cloning of songs a little...
                SerializationUtils.Clone(new Song());
                PlaybackStreamer.InitializeAudio();
                Application.Run(new frmDockedMain(args));
                PlaybackStreamer.StopAudio();
            }
        }

        private static void startSeveralInstances(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                Restart(args[i]);
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e != null && e.ExceptionObject != null)
            {
                Logger.Log(e.ExceptionObject.ToString());
            }
        }

        private static bool installOpenAL()
        {
            bool result = false;
            if (MessageBox.Show(WYZTracker.Properties.Resources.DownloadOpenAL,
                WYZTracker.Properties.Resources.Error,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Error) == DialogResult.Yes)
            {
                try
                {
                    if (isMono)
                    {
                        Process.Start("http://www.openal.org/creative-installers/");
                    }
                    else
                    {
                        ProcessStartInfo pInfo = new ProcessStartInfo("oalinst.exe");
                        Process setupProcess = Process.Start(pInfo);
                        setupProcess.WaitForExit();
                        result = true;
                    }
                }
                catch (Exception e)
                {
                    Logger.Log(e.ToString());
                    Logger.Log("OpenAL couldn't be installed properly.\nPlease install OpenAL before running WYZTracker.");
                    MessageBox.Show(WYZTracker.Properties.Resources.OpenALInstallError,
                                    WYZTracker.Properties.Resources.Error,
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
            return result;
        }

        private static void checkWYZFileAssociation()
        {
            if (Properties.Settings.Default.CheckFileAssociation)
            {
                FileAssociationInfo fai = new FileAssociationInfo(".wyz");

                bool refreshAssociation = !fai.Exists;
                if (!refreshAssociation)
                {
                    ProgramAssociationInfo pai = new ProgramAssociationInfo(fai.ProgID);
                    refreshAssociation = (!pai.Exists ||
                        pai.Verbs.Length == 0 ||
                        !pai.Verbs[0].Command.ToLower().Contains(Application.ExecutablePath.ToLower()));
                }

                if (refreshAssociation)
                {
                    DlgFileAssociationCheck dlgFileAssocCheck = new DlgFileAssociationCheck();
                    if (dlgFileAssocCheck.ShowDialog() == DialogResult.Yes)
                    {
                        Process newProcess = new Process();
                        newProcess.StartInfo.FileName = Application.ExecutablePath;
                        newProcess.StartInfo.Verb = "runas";
                        newProcess.StartInfo.Arguments = CREATE_ASSOC_PARAM;
                        newProcess.Start();
                        newProcess.WaitForExit();
                    }
                }
            }
        }

        private static void createWYZFileAssociation()
        {
            FileAssociationInfo fai = new FileAssociationInfo(".wyz");
            if (!fai.Exists)
            {
                fai.Create("WYZTracker");
                fai.ContentType = "application/wyz-song-file";
                fai.OpenWithList = new string[] {
                        "WYZTracker.exe"
                    };
            }

            ProgramAssociationInfo pai = new ProgramAssociationInfo(fai.ProgID);
            if (!pai.Exists ||
                pai.Verbs.Length == 0 ||
                !pai.Verbs[0].Command.ToLower().Contains(Application.ExecutablePath.ToLower()))
            {
                pai.Create(
                    WYZTracker.Properties.Resources.WYZFileDesc,
                    new ProgramVerb(
                        WYZTracker.Properties.Resources.OpenVerb,
                        string.Format("\"{0}\" \"%1\"", Application.ExecutablePath)
                        )
                    );
                pai.DefaultIcon = new ProgramIcon(Application.ExecutablePath);
            }

        }

        public static void Restart(string fileName)
        {
            Process newProcess = new Process();
            string arguments = string.Empty;
            if (!string.IsNullOrEmpty(fileName))
            {
                arguments = string.Format("\"{0}\"", fileName);
            }

            newProcess.StartInfo = new ProcessStartInfo(
                Application.ExecutablePath,
                arguments
            );

            newProcess.Start();
            Application.Exit();
        }
    }
}