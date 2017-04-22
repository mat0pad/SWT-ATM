using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWT_ATM
{
    public class Data
    {
        public readonly string Tag;
        public readonly int XCord;
        public readonly int YCord;
        public readonly int Altitude;
        public readonly string Timestamp;

        public Data(string tag, int x, int y, int alt, string time)
        {
            Tag = tag;
            XCord = x;
            YCord = y;
            Altitude = alt;
            Timestamp = time;
        }
    }
}
