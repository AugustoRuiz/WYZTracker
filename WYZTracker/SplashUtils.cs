using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WYZTracker
{

    internal class SplashUtils
    {
        public event EventHandler FadeInCompleted;
        public event EventHandler FadeOutCompleted;

        public TimeSpan FadeInLength { get; set; }
        public TimeSpan FadeOutLength { get; set; }
        public Form SplashForm { get; set; }

        private System.Diagnostics.Stopwatch fadeStopwatch;

        private ManualResetEvent fadeInCompletedEvent;

        public SplashUtils(Form theForm)
        {
            SplashForm = theForm;
            fadeStopwatch = new System.Diagnostics.Stopwatch();
            fadeInCompletedEvent = new ManualResetEvent(false);
            this.FadeInLength = TimeSpan.FromMilliseconds(300);
            this.FadeOutLength = TimeSpan.FromMilliseconds(600);
        }

        public void FadeIn()
        {
            SplashForm.Opacity = 0f;
            SplashForm.Show();
            doFadeIn();
        }

        public void FadeOut()
        {
            System.Threading.Thread fadeOutThread = new System.Threading.Thread(new System.Threading.ThreadStart(doFadeOut));
            fadeOutThread.IsBackground = true;
            fadeOutThread.Name = "Fade out thread";
            fadeOutThread.Start();
        }

        private void doFadeIn()
        {
            fadeStopwatch.Reset();
            fadeStopwatch.Start();

            while (fadeStopwatch.Elapsed < FadeInLength)
            {
                double opacity = fadeStopwatch.ElapsedMilliseconds / FadeInLength.TotalMilliseconds;
                setOpacityAndWait(opacity);
            }
            setOpacityAndWait(1.0);
            fadeStopwatch.Stop();
            fadeInCompletedEvent.Set();
            onFadeInCompleted();
        }

        private void doFadeOut()
        {
            fadeInCompletedEvent.WaitOne();

            fadeStopwatch.Reset();
            fadeStopwatch.Start();

            while (fadeStopwatch.Elapsed < FadeOutLength)
            {
                double opacity = 1 - (fadeStopwatch.ElapsedMilliseconds / FadeOutLength.TotalMilliseconds);
                setOpacityAndWait(opacity);
            }
            setOpacityAndWait(0.0);
            fadeStopwatch.Stop();

            onFadeOutCompleted();
        }

        private void setOpacityAndWait(double opacity)
        {
            if (SplashForm.InvokeRequired)
                SplashForm.Invoke(new Action<double>(SetOpacity), opacity);
            else
            {
                SetOpacity(opacity);
            }
            System.Threading.Thread.Sleep(20);
        }

        private void SetOpacity(double opacity)
        {
            SplashForm.Opacity = opacity;
            SplashForm.Invalidate();
            Application.DoEvents();
        }

        private void onFadeInCompleted()
        {
            EventHandler tmp = this.FadeInCompleted;
            if (null != tmp)
            {
                tmp(this, EventArgs.Empty);
            }
        }

        private void onFadeOutCompleted()
        {
            EventHandler tmp = this.FadeOutCompleted;
            if (null != tmp)
            {
                tmp(this, EventArgs.Empty);
            }
        }
    }
}
