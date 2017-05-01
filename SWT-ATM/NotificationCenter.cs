using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Threading.Tasks;

namespace SWT_ATM
{
    public class NotificationCenter
    {
        private static readonly object TimerCreationLock = new object();
        private readonly ConcurrentQueue<List<string>> _notificationsQueue = new ConcurrentQueue<List<string>>();
        private readonly AutoResetEvent _notficationSignal = new AutoResetEvent(false);
        private readonly AutoResetEvent _warningSignal = new AutoResetEvent(false);
        private readonly ConcurrentQueue<List<string>> _notificationsToShow = new ConcurrentQueue<List<string>>();
        private readonly ConcurrentQueue<List<List<string>>> _warnings = new ConcurrentQueue<List<List<string>>>();
        private const int Seperation = 10; // Distance betweeen notification data in the same row
        private int _prevWarningCount = 0, maxConflictsInWarningMsg = 5; // Used for clearing the current warnings before rewriting the new ones

        
        public ConcurrentQueue<List<string>> GetNotificationQueue()
        {
            return _notificationsQueue;
        }

        public AutoResetEvent GetNotificationSignalHandle()
        {
            return _notficationSignal;
        }

        public ConcurrentQueue<List<List<string>>> GetWwarningsQueue()
        {
            return _warnings;
        }

        public AutoResetEvent GetWarningsSignalHandle()
        {
            return _warningSignal;
        }

        public NotificationCenter()
        {
            var thread1 = new Thread(NotificationThread);
            var thread2 = new Thread(WarningThread);
            thread1.Start();
            thread2.Start();
        }

        private void NotificationThread()
        {
            while (true)
            {
                _notficationSignal.WaitOne();

                List<string> item;
                while (_notificationsQueue.TryDequeue(out item))
                {
                    var msg = item;

                    WriteNotification(msg);
                    ExecuteDelayed(DeleteNotification, 5000);
                }
            }
        }

        public async Task ExecuteDelayed(Action action, int timeoutInMilliseconds)
        {
            await Task.Delay(timeoutInMilliseconds);
            action();
        }

        private void WarningThread()
        {
            while (true)
            {
                _warningSignal.WaitOne();

                List<List<string>> item;
                while (_warnings.TryDequeue(out item))
                {
                    UpdateWarnings(item);
                }
            }
        }

        private void WriteNotification(List<string> notification)
        {
            lock (_notificationsToShow)
            {
                _notificationsToShow.Enqueue(notification);

                var i = 2;

                foreach (var n in _notificationsToShow)
                {
                    Display.WriteRow(n, Seperation, Display.InnerRightLineBound, i++);
                }
            }
        }

        private void DeleteNotification()
        {
            lock (_notificationsToShow)
            {
                var s = new string(' ', Seperation);

                // Clear current
                for (var i = 0; i < _notificationsToShow.Count; i++)
                    Display.WriteRow(new List<string> { s, s }, Seperation, Display.InnerRightLineBound, i + 2);

                List<string> dummyList;
                _notificationsToShow.TryDequeue(out dummyList);


                var j = 2;
                // Write new
                foreach (var n in _notificationsToShow)
                    Display.WriteRow(n, Seperation, Display.InnerRightLineBound, j++);
            }
        }

        private void UpdateWarnings(IEnumerable<List<string>> warnings)
        {
            var s = new string(' ', Display.OuterRightLineBound-Display.InnerRightLineBound-1);

            int i;
            // Clear current warnings
            for (i = 0; i < _prevWarningCount; i++)
                Display.WriteRow(new List<string> { s }, 0, Display.InnerRightLineBound,
                    DisplayFormatter.Height / 2 + 1 + i);

            // Rewrite (new) warnings
            i = 0;
            foreach (var w in warnings)
            {
                List<string> tmpList = new List<string>(w);
                tmpList.Add("CONFLICTING");
                Display.WriteRow(tmpList, Seperation, Display.InnerRightLineBound, DisplayFormatter.Height / 2 + ++i);
            }

            _prevWarningCount = i;
        }
    }
}
