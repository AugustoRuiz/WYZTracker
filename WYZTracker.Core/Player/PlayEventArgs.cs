using System;
using System.Collections.Generic;
using System.Text;

namespace WYZTracker
{
    public class PlayEventArgs : EventArgs
    {
        private PlayMode playMode;

        public PlayMode PlayMode
        {
            get { return playMode; }
            set { playMode = value; }
        }

        public PlayEventArgs(PlayMode p)
        {
            this.playMode = p;
        }
    }
}
