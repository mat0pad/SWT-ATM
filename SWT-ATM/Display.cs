using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SWT_ATM
{
    public class Display : IDisplay
    {
        private int _width;
        private int _height;
        private int _innerRightLineBounds;
        private int _middleRightLineBounds;
        private int _outerRightLineBounds;
        private int _outerBoundHeight;
        private int _rowSeperation;
        private List<Data> _prevList;


        public void SetSize(int width, int height)
        {
            _width = width;
            _height = height;
            Configure();
            BuildFrame();
        }

        public void SetWidth(int width)
        {
            _width = width;
            Configure();
            BuildFrame();
        }

        public void SetHeight(int height)
        {
            _height = height;
            Configure();
            BuildFrame();
        }

        public Display(int width = 150, int height = 30)
        {
            _width = width;
            _height = height;

            Configure();
            BuildFrame();
        }

        private void Configure()
        {
            _outerBoundHeight = _height + 2;
            _rowSeperation = _width*3/5 / 6;

            _innerRightLineBounds = _width * 3 / 5 + 6;
            _middleRightLineBounds = _width * 4 / 5 + 4;
            _outerRightLineBounds = _width + 2;

            if (_width + 1 > Console.BufferWidth)
                Console.BufferWidth = _width + 2; // "+2" to accomodate right line
            if (_height + 2 > Console.BufferHeight)
                Console.BufferHeight = _height + 3; //  "+3" to accommodate the top and bottom line
        }

        public void ShowTracks(List<Data> d)
        {
            var i = 2;

            foreach (var track in d)
            {
                Data oldData = null;

                if (_prevList != null)
                    oldData = _prevList.FirstOrDefault(prevData => prevData.Tag == track.Tag);

                var trackInfo = FormatTrackData(track, oldData ?? new Data("", 0, 0, 0, "0000000000000000"));

                WriteRow(trackInfo, _rowSeperation, 1, i++);
            }

            _prevList = d;
        }

        private List<string> FormatTrackData(Data current, Data prev)
        {
            // Velocity
            var distance = Math.Sqrt(Math.Pow(current.YCord - prev.YCord, 2) + Math.Pow((current.XCord - prev.XCord), 2));

            var currentTime = current.Timestamp.Substring(10);
            var currentMinutes = int.Parse(currentTime.Substring(0, 2));
            var currentSeconds = int.Parse(currentTime.Substring(2, 2));
            var currentMiliseconds = int.Parse(currentTime.Substring(4));

            var oldTime = prev.Timestamp.Substring(10);
            var oldMinutes = int.Parse(oldTime.Substring(0, 2));
            var oldSeconds = int.Parse(oldTime.Substring(2, 2));
            var oldMiliseconds = int.Parse(oldTime.Substring(4));

            var minutesDiff = currentMinutes - oldMinutes;
            var secDiff = currentSeconds - oldSeconds;
            var miliDiff = currentMiliseconds - oldMiliseconds;

            var timeDiff = (minutesDiff * 60 * 1000 + secDiff * 1000 + miliDiff) / 1000;

            var velocity = distance / timeDiff;


            // Compass course
            var y1 = current.YCord;
            var x1 = current.XCord;
            var y2 = prev.YCord;
            var x2 = prev.XCord;
            
            // Line representing north
            var y3 = 0;
            var x3 = 0;
            var y4 = 1;
            var x4 = 0;

            var compassCourseRad = Math.Atan2(y4 - y3, x4 - x3) - Math.Atan2(y1-y2, x1-x2);
            var compassCourseDeg = compassCourseRad * 180 / Math.PI;

            return new List<string> {current.Tag, current.XCord.ToString(), current.YCord.ToString(), current.Altitude.ToString(),
                $"{velocity:0}",
                $"{compassCourseDeg:0}"
            };
        }

		private void BuildFrame() 
		{
            Console.Clear();

            // Top line
            Console.SetCursorPosition(1, 0);
            for (int i = 0; i < _width; i++)
				Console.Write("-");

            // Left line
			Console.SetCursorPosition(0, 1);
			for (int i = 0; i < _height; i++)
				Console.WriteLine("|");

            // Bottom line
            Console.SetCursorPosition(1, Console.CursorTop);
            for (int i = 0; i < _width; i++)
                Console.Write("-");


            // Right inner line
            for (int i = 1; i < _height + 1; i++)
            {
                Console.SetCursorPosition(_innerRightLineBounds - 1, i);
                Console.WriteLine("|");
            }

            // Right horizontal line
            Console.SetCursorPosition(1, Console.CursorTop);
            for (int i = 1; i < _outerRightLineBounds -_innerRightLineBounds + 1; i++)
            {
                Console.SetCursorPosition(_innerRightLineBounds-1+i, _height/2);
                Console.Write("-");
            }

            // Right outer line
            Console.SetCursorPosition(1, Console.CursorTop);
            for (int i = 1; i < _height + 1; i++)
            {
                Console.SetCursorPosition(_outerRightLineBounds-1, i);
                Console.WriteLine("|");
            }


            List<string> list = new List<string>{"Tag", "Position X", "Position Y", "Altitude", "Velocity", "Compass course"};

            WriteRow(list, _rowSeperation, 1, 1);
		}

        private void WriteRow(List<string> toWrite, int seperation, int startLeft, int startTop)
        {
            int i = 0;

            foreach (string s in toWrite)
            {
                string rowSeperationString = new string(' ', seperation);

                Console.SetCursorPosition(startLeft + seperation * i, startTop);
                Console.Write(rowSeperationString);
                Console.SetCursorPosition(startLeft + seperation * i++, startTop);
                Console.Write(s);
            }
        }

        ~Display()
        {
            Console.SetCursorPosition(0, _outerBoundHeight);
        }

        public void ShowNotification(Data d, EventType type)
        {
            Thread thread = new Thread(delegate () {

                /*string year = d.Timestamp.Substring(0, 4);
                string month = d.Timestamp.Substring(4, 2);
                string day = d.Timestamp.Substring(6, 2);*/
                string hour = d.Timestamp.Substring(8, 2);
                string minutes = d.Timestamp.Substring(10, 2);
                string seconds = d.Timestamp.Substring(12, 2);
                string miliseconds = d.Timestamp.Substring(14);

                WriteRow(new List<string> { d.Tag, hour + ":" + minutes + ":" + seconds + "," + miliseconds }, 11, _middleRightLineBounds, 2);
                Thread.Sleep(5000);
            });

            thread.Start();
        }

        public void ShowWarning(List<Warning> w)
        {
            throw new NotImplementedException();
        }
    }
}
