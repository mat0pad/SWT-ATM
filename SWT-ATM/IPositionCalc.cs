using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWT_ATM
{
    public interface IPositionCalc
    {
        string CalcVelocity(Data current, Data prev);
        string CalcCourse(Data current, Data prev);
        IEnumerable<string> FormatTrackData(Data current, Data prev);
    }
}
