using System;
using System.Collections.Generic;
using System.Text;

namespace WYZTracker
{
   public class ActiveInstrumentEventArgs: EventArgs
    {
        public string Id { get; private set; }

        public ActiveInstrumentEventArgs(string id)
        {
            this.Id = id;
        }
    }
}
