using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SWT_ATM
{
    public class Display : IDisplay
    {
        private static readonly object TracksLock = new object();
        private static readonly object ConsoleWriterLock = new object();
        private static readonly object WarningLock = new object();
        public static int InnerRightLineBound { get; private set; }
        public static int OuterRightLineBound { get; private set; }

        private int _outerBoundHeight;
        private int _rowSeperation;

        private List<Data> _prevList;
        private readonly INotificationCenter _notificationCenter;
        private IPositionCalc _calc;

        //For testing
        public Display(INotificationCenter notificationCenter, IPositionCalc calc)
        {
            _notificationCenter = notificationCenter;
            _calc = calc;

            Thread t = new Thread(Rebuild);
            t.Start();
        }

        ~Display()
        {
            Console.SetCursorPosition(0, _outerBoundHeight);
        }

        private void Rebuild()
        {
            while (true)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.R)
                    BuildFrame(DisplayFormatter.Width,DisplayFormatter.Height);
            }
        }

        public static void WriteRow(IEnumerable<string> toWrite, int seperation, int startLeft, int startTop)
        {
            var i = 0;
            var rowSeperationString = new string(' ', seperation);

            lock (ConsoleWriterLock)
            {
                foreach (var s in toWrite)
                {
                    Console.SetCursorPosition(startLeft + seperation * i, startTop);
                    Console.Write(rowSeperationString);
                    Console.SetCursorPosition(startLeft + seperation * i++, startTop);
                    Console.Write(s);
                }
            }

        }

        public void ShowNotification(List<string> item)
        {
            _notificationCenter.GetNotificationQueue().Enqueue(item);
            _notificationCenter.GetNotificationSignalHandle().Set();
        }

        public void ShowWarning(List<List<string>> w)
        {
            _notificationCenter.GetWwarningsQueue().Enqueue(w);
            _notificationCenter.GetWarningsSignalHandle().Set();
        }

        public void ShowTracks(List<Data> d)
        {
            lock (TracksLock)
            {
                ClearAll();
                
                WriteTracks(d);

                _prevList = d;
            }
        }

        private void ClearAll()
        {
            string clear = new string(' ', _rowSeperation);

            // Clear all
            int i;
            if (_prevList != null)
                for (i = 0; i < _prevList.Count; i++)
                {
                    WriteRow(new List<string> { clear, clear, clear, clear, clear, clear }, _rowSeperation, 1, i + 2);
                }
        }


        private void WriteTracks(List<Data> d)
        {
            var i = 2;

            foreach (var track in d)
            {
                Data oldData = null;

                if (_prevList != null)
                    oldData = _prevList.FirstOrDefault(prevData => prevData.Tag == track.Tag);

                if (oldData != null && track.XCord != oldData.XCord && track.YCord != oldData.YCord)
                {
                    var trackInfo = _calc.FormatTrackData(track, oldData);
                    WriteRow(trackInfo, _rowSeperation, 1, i++);
                }
                else
                {
                    var trackInfo = _calc.FormatTrackData(track, new Data("", 0, 0, 0, "0000000000000000"));
                    WriteRow(trackInfo, _rowSeperation, 1, i++);
                }

            }
        }


        public void Configure(int width, int height)
        {
            _outerBoundHeight = height + 2;
            _rowSeperation = width * 3 / 5 / 6;

            InnerRightLineBound = width * 3 / 5 + 6;
            OuterRightLineBound = width + 2;

            if (width + 1 > Console.BufferWidth)
                Console.BufferWidth = width + 2; // "+2" to accomodate right line
            if (height + 2 > Console.BufferHeight)
                Console.BufferHeight = height + 3; //  "+3" to accommodate the top and bottom line
        }


        public void BuildFrame(int width, int height)
        {
            lock (ConsoleWriterLock)
            {
                Configure(width, height);
                Console.Clear();

                // Top line
                Console.SetCursorPosition(1, 0);
                for (int i = 0; i < width; i++)
                    Console.Write("-");

                // Left line
                Console.SetCursorPosition(0, 1);
                for (int i = 0; i < height; i++)
                    Console.WriteLine("|");

                // Bottom line
                Console.SetCursorPosition(1, Console.CursorTop);
                for (int i = 0; i < width; i++)
                    Console.Write("-");


                // Right inner line
                for (int i = 1; i < height + 1; i++)
                {
                    Console.SetCursorPosition(InnerRightLineBound - 1, i);
                    Console.WriteLine("|");
                }

                // Right horizontal line
                Console.SetCursorPosition(1, Console.CursorTop);
                for (int i = 1; i < OuterRightLineBound - InnerRightLineBound + 1; i++)
                {
                    Console.SetCursorPosition(InnerRightLineBound - 1 + i, height / 2);
                    Console.Write("-");
                }

                // Right outer line
                Console.SetCursorPosition(1, Console.CursorTop);
                for (int i = 1; i < height + 1; i++)
                {
                    Console.SetCursorPosition(OuterRightLineBound - 1, i);
                    Console.WriteLine("|");
                }

            }

            List<string> list = new List<string> { "Tag", "Position X", "Position Y", "Altitude", "Velocity", "Compass course" };

            WriteRow(list, _rowSeperation, 1, 1);
        }
    }
}
