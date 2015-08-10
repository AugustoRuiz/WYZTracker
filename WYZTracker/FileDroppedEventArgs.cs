using System;
using System.Collections.Generic;
using System.Text;

namespace WYZTracker
{
    public class FileDroppedEventArgs : EventArgs
    {
        public FileDroppedEventArgs(string path)
        {
            filePath = path;
        }

        private string filePath;

        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }
    }
}
