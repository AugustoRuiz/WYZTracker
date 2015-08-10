using System;
using System.Collections.Generic;
using System.Text;
using WYZTracker;

namespace WYZPlayer
{
    public class PlaylistItem
    {
        public string FilePath { get; set; }
        public Song Song { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - [{1}]", Song.Name, FilePath);
        }
    }
}
