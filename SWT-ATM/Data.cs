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
        public int XCord { get; set; }
        public int YCord { get; set; }
        public int Altitude { get; set; }
        public string Timestamp { get; set; }

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
