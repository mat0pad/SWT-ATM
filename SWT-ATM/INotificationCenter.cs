using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SWT_ATM
{
    public interface INotificationCenter
    {
        ConcurrentQueue<List<String>> GetNotificationQueue();

        AutoResetEvent GetNotificationSignalHandle();

        ConcurrentQueue<List<List<String>>> GetWwarningsQueue();

        AutoResetEvent GetWarningsSignalHandle();

        Task ExecuteDelayed(Action action, int timeout);
    }
}
