using System;
using System.Collections.Generic;
using System.Text;

namespace WYZTracker
{
    public class ActiveFxEventArgs :EventArgs
    {
        public int Id { get; private set; }

        public ActiveFxEventArgs(int id)
        {
            this.Id = id;
        }
    }
}
