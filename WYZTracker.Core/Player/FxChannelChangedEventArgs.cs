using System;
using System.Collections.Generic;
using System.Text;

namespace WYZTracker
{
    public class FxChannelChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the effects channel
        /// </summary>
        public int FxChannel { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fxChannel">Effects channel</param>
        public FxChannelChangedEventArgs(int fxChannel)
        {
            this.FxChannel = fxChannel;
        }

    }
}
