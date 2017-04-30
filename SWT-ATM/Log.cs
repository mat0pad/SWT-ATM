using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SWT_ATM
{
    public class Log: ILog
    {
        private static string _path;
        private Object locker = new Object(); // Used for locking/mutex

        public Log()
        {
            _path = Directory.GetCurrentDirectory() + @"\log.txt";

            if (File.Exists(_path))
            {
                // Use a try block to catch IOExceptions, to
                // handle the case of the file already being opened by another process.
                try
                {
                    File.Delete(_path);
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            File.WriteAllText(_path, "Log Of Events" + Environment.NewLine + Environment.NewLine + "Timestamp        \tEvent Type\tEvent Category\tTag of Track(s)");
        }

        public Log(string path)
        {
            _path = path;

            if (File.Exists(_path))
            {
                // Use a try block to catch IOExceptions, to
                // handle the case of the file already being opened by another process.
                try
                {
                    File.Delete(_path);
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            File.WriteAllText(_path, "Log Of Events" + Environment.NewLine + Environment.NewLine + "Timestamp        \tEvent Type\tEvent Category\tTag of Track(s)");
        }

        public void WriteNotification(Data data, bool isLeaving)
        {
            WriteToLog(Environment.NewLine + data.Timestamp + "\t" + (isLeaving ? "LEAVING    " : "ENTERING   ") + "\t" + "NOTIFICATION" + "\t" + data.Tag);
        }

        public void WriteWarning(List<Data> list)
        {
            string tracksInConflict = "";
            string timestamp = "";
            
            /*
            foreach (var item in list)
            {
                tracksInConflict += (item.Tag + "    ");
                timestamp = item.Timestamp;
            }
            */
            
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (i == 0 && String.IsNullOrEmpty(list[0].Timestamp))
                    timestamp = list[0].Timestamp;
                
                if(String.IsNullOrWhiteSpace(list[i].Tag))
                    tracksInConflict += list[i].Tag + "   ";
            }
            
            
            WriteToLog(Environment.NewLine + timestamp + "\t" + "CONFLICTING" + "\t" + "WARNING     " + "\t" + tracksInConflict);
        }

        private void WriteToLog(string text2Write)
        {
            // The using statement automatically flushes AND CLOSES the stream and calls 
            // IDisposable.Dispose on the stream object.
            lock (locker)
            {
                using (StreamWriter file = new StreamWriter(_path, true))
                {
                    file.WriteLine(text2Write);
                }
            }
        }
    }
}
