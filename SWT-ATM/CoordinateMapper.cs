using System.Collections.Generic;

namespace SWT_ATM
{
    public class CoordinateMapper: EventSubject
    {
        private List<TransponderDataFormat> list { get; set; }

        private List<ITrackObserver> tracks { get; set; }
    }
}