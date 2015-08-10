using System;
using System.Collections.Generic;
using System.Text;

namespace WYZTracker
{
    public interface ISongConsumer
    {
        Song CurrentSong { get; set; }
    }
}
