using System.Collections.Generic;

namespace SWT_ATM
{
    public interface ILog
    {
        void WriteNotification(Data data, bool isLeaving);
        void WriteWarning(List<Data> list);
    }
}