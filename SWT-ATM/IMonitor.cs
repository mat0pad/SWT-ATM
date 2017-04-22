using System.Collections.Generic;

namespace SWT_ATM
{
    public interface IMonitor
    {
        bool InsideBounds(Data data);
        Notification Notification(Data data);
        List<Warning> Conflicting(List<Data> listdata);
        void SetY(int min, int max);
        void SetZ(int min, int max);
        void SetX(int min, int max);

    }
}