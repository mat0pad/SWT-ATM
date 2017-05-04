using System.Collections.Generic;

namespace SWT_ATM
{
    public interface ICoordinateMapper
    {
        void MapTrack(List<string> rawData);
    }
}