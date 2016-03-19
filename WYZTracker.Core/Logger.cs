using System;
using System.Collections.Generic;
using System.Text;

namespace WYZTracker
{
    public class Logger
    {
        static Logger()
        {
            FileName = "WYZTracker.log";
        }

        public static string FileName { get; set; }

        public static string LogPath
        {
            get
            {
                return System.IO.Path.Combine(Environment.CurrentDirectory, FileName);
                //return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), FileName);
            }
        }

        public static void Log(string format, params object[] parameters)
        {
            string finalText = string.Format("{0}{1}{2}{1}", 
                createEntryHeader(), 
                Environment.NewLine, 
                string.Format(format, parameters));

            System.IO.File.AppendAllText(LogPath, finalText);
#if DEBUG
            System.Diagnostics.Debug.WriteLine(finalText); 
#endif
        }

        private static string createEntryHeader()
        {
            return DateTime.Now.ToString();
        }
    }
}
