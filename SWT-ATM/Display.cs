using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWT_ATM
{
    public class Display : IDisplay
    {
        private readonly int _width;
        private readonly int _height;
        private readonly int _outerBoundHeight;
        private readonly int _rowSeperation;
        private List<Data> prevList;

        public Display(int width = 70, int height = 30)
        {
            _width = width;
            _height = height;
            _outerBoundHeight = _height + 2;
            _rowSeperation = width / 6;

            if (_width + 1 > Console.BufferWidth)
                Console.BufferWidth = _width + 1; // "+1" to accomodate right line
            if (_height + 2 > Console.BufferHeight)
                Console.BufferHeight = _height + 2; //  "+2" to accommodate the top and bottom line

            BuildFrame();
        }

        public void ShowTracks(List<Data> d)
        {
            int i = 2;

            foreach (Data track in d)
            {
                List<string> trackInfo = new List<string> {track.Tag, track.Timestamp, track.Altitude.ToString(), track.XCord.ToString(), track.YCord.ToString()};
                WriteRow(trackInfo, _rowSeperation, 1, i++);
            }

            prevList = d;
        }

        private List<string> formatTrackData(Data current, Data prev)
        {

            var currentPosition = new GeoCoordinate(current.XCord, current.YCord);
            var oldPosition = new GeoCoordinate(prev.XCord, prev.YCord);
            var distance = currentPosition.GetDistanceTo(oldPosition);

            var currentTime = current.Timestamp.Substring(10);
            var currentMinutes = int.Parse(currentTime.Substring(0, 1));
            var currentSeconds = int.Parse(currentTime.Substring(2, 4));
            var currentMiliseconds = int.Parse(currentTime.Substring(5));

            var oldTime = prev.Timestamp.Substring(10);
            var oldMinutes = int.Parse(oldTime.Substring(0, 1));
            var oldSeconds = int.Parse(oldTime.Substring(2, 4));
            var oldMiliseconds = int.Parse(oldTime.Substring(5));

            var minutesDiff = currentMinutes - oldMinutes;
            var secDiff = currentSeconds - oldSeconds;
            var miliDiff = currentMiliseconds - oldMiliseconds;

            var timeDiff = (minutesDiff * 60 * 1000 + secDiff * 1000 + miliDiff) / 1000;

            var velocity = distance / timeDiff;

            //var vector2 = new Vector(current.XCord, current.YCord) - new Point();
            //var vector1 = new Point(0, 1) // 12 o'clock == 0°, assuming that y goes from bottom to top

            //double angleInRadians = Math.Atan2(vector2.Y, vector2.X) - Math.Atan2(vector1.Y, vector1.X);

            return new List<string> {current.Tag, current.XCord.ToString(), current.YCord.ToString(), current.Altitude.ToString(), velocity.ToString(), "N/A"};

        }



		private void BuildFrame() 
		{
            Console.SetCursorPosition(1, 0);
            for (int i = 0; i < _width; i++)
				Console.Write("-");

			Console.SetCursorPosition(0, 1);
			for (int i = 0; i < _height; i++)
				Console.WriteLine("|");

			;
			for (int i = 1; i < _height+1; i++)
			{
			    Console.SetCursorPosition(_width+1, i);
                Console.WriteLine("|");
			}

            Console.SetCursorPosition(1, Console.CursorTop);
            for (int i = 0; i < _width; i++)
                Console.Write("-");


		    List<string> list = new List<string>{"Tag", "Position X", "Position Y", "Altitude", "Velocity", "Compass course"};

            WriteRow(list, _rowSeperation, 1, 1);
		}

        private void WriteRow(List<string> toWrite, int seperation, int startLeft, int startTop)
        {
            int i = 0;

            foreach (string s in toWrite)
            {
                Console.SetCursorPosition(startLeft + _rowSeperation * i++, startTop);
                Console.Write(s);
            }
        }

        ~Display()
        {
            Console.SetCursorPosition(0, _outerBoundHeight);
        }

		private void InitFrame(int width, int height)
		{
			
		}

        public void ShowNotification(Notification n)
        {
            throw new NotImplementedException();
        }

        public void ShowWarning(List<Warning> w)
        {
            throw new NotImplementedException();
        }
    }
}
