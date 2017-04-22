using System;
using System.Collections.Generic;

namespace SWT_ATM
{
    public class CoordinateMapper : Subject<Data>, ICoordinateMapper
    {
        private List<IObserver<Data>> List { get; set; }

        private ITransponderDataFormat Formatter;

        public CoordinateMapper(ITransponderDataFormat formatter)
        {
            Formatter = formatter;
        }

        public void MapTrack(string rawData)
        {
            Notify(Formatter.FormatData(rawData));
        }
    }
}