using System.Collections.Generic;

namespace SWT_ATM
{
    public interface IMonitor
    {
        bool InsideBounds(Data data);
        List<Data> GetTracksInConflict();
        void SetY(int min, int max);
        void SetZ(int min, int max);
        void SetX(int min, int max);
        int[] GetX();
        int[] GetY();
        int[] GetZ();
        void SetShareList(ref List<Data> list);
        EventType EventTracker(Data data);

    }
}