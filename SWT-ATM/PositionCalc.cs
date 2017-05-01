using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWT_ATM
{
    public class PositionCalc : IPositionCalc
    {
        public string CalcVelocity(Data current, Data prev)
        {
            // Velocity
            var distance = Math.Sqrt(Math.Pow(current.YCord - prev.YCord, 2) + Math.Pow((current.XCord - prev.XCord), 2));

            var velocity = 0.00;

            if (current.Timestamp.Length >= 10)
            {
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

                velocity = distance / timeDiff;
            }

            return $"{velocity:0}";
        }

        public string CalcCourse(Data current, Data prev)
        {

            var compassCourseRad = Math.Atan2(1, 0) - Math.Atan2(current.YCord - prev.YCord, current.XCord - prev.XCord);

            var compassCourseDeg = compassCourseRad * 180 / Math.PI;

            current.CompassCourse = compassCourseDeg;

            return $"{compassCourseDeg:0}";
        }

        public IEnumerable<string> FormatTrackData(Data current, Data prev)
        {
            return new List<string> {current.Tag, current.XCord.ToString(), current.YCord.ToString(), current.Altitude.ToString(),
                $"{CalcVelocity(current,prev):0}",
                $"{CalcCourse(current,prev):0}"
            };
        }

    }
}
