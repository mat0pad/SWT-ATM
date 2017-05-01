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
        void EnqueNotification(List<string> item);

        void SetNotificationSignalHandle();

        void EnqueWarning(List<List<string>> item);

        void SetWarningsSignalHandle();

        Task ExecuteDelayed(Action action, int timeout);
    }
}
