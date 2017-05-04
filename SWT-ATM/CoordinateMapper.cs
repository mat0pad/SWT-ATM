using System;
using System.Collections.Generic;

namespace SWT_ATM
{
    public class CoordinateMapper : Subject<List<Data>>, ICoordinateMapper
    {
        private ITransponderDataFormat Formatter;
        private List<Data> _incomingData;

        public CoordinateMapper(ITransponderDataFormat formatter)
        {
            Formatter = formatter;
            _incomingData = new List<Data>();
        }

        public void MapTrack(List<string> rawData)
        {
            _incomingData.Clear();

            foreach (var item in rawData)
            {
                _incomingData.Add(Formatter.FormatData(item));
            }

            Notify(_incomingData);            
        }
    }
}