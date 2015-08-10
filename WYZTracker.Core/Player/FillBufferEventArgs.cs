using System;
using System.Collections.Generic;
using System.Text;

namespace WYZTracker
{
    /// <summary>
    /// Event arguments for FillBuffer event
    /// </summary>
    public class FillBufferEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="buffer">Buffer to be filled.</param>
        public FillBufferEventArgs(double[] buffer)
        {
            this.Buffer = buffer;
        }

        /// <summary>
        /// Gets the buffer to fill.
        /// </summary>
        public double[] Buffer { get; private set; }
    }
}
