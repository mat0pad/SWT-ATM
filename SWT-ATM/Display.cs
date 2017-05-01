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
        public static int InnerRightLineBound { get; private set; }
        public static int OuterRightLineBound { get; private set; }

        private int _prevTrackCount;
        private int _outerBoundHeight;
        private int _rowSeperation;


        public int GetRowSeperation()
        {
            return _rowSeperation;
        }
        

        //For testing
        public Display()
        {
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

        public void WriteRow(IEnumerable<string> toWrite, int seperation, int startLeft, int startTop)
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


        public void ShowTracks(List<IEnumerable<string>> d)
        {
            lock (TracksLock)
            {
                ClearAll(_prevTrackCount);
                
                WriteTracks(d);

                _prevTrackCount = d.Count;
            }
        }

        private void ClearAll(int prevCount)
        {
            string clear = new string(' ', _rowSeperation);

            // Clear all
            int i;
                for (i = 0; i < prevCount; i++)
                {
                    WriteRow(new List<string> { clear, clear, clear, clear, clear, clear }, _rowSeperation, 1, i + 2);
                }
        }


        private void WriteTracks(List<IEnumerable<string>> d)
        {
            var i = 2;

            foreach (var track in d)
            {
                    WriteRow(track, _rowSeperation, 1, i++);
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
