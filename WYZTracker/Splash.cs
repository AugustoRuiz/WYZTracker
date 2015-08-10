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
    public partial class Splash : Form
    {
        SplashUtils splashUtils;

        public Splash()
        {
            InitializeComponent();

            splashUtils = new SplashUtils(this);
            splashUtils.FadeOutCompleted += new EventHandler(splashUtils_FadeOutCompleted);
        }

        public void FadeIn()
        {
            splashUtils.FadeIn();
        }

        public void FadeOut()
        {
            splashUtils.FadeOut();
        }

        void splashUtils_FadeOutCompleted(object sender, EventArgs e)
        {
            // Finiquitar esta instancia cuando haya terminado de hacer fade out.
            this.Invoke(new MethodInvoker(delegate() { this.Dispose(); ApplicationState.SplashScreen = null; }));
        }
    }
}
