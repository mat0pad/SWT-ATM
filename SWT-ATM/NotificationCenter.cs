using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SWT_ATM
{
    public class NotificationCenter
    {
        private readonly AutoResetEvent _notficationSignal = new AutoResetEvent(false);
        private readonly AutoResetEvent _warningSignal = new AutoResetEvent(false);
        private readonly ConcurrentQueue<List<string>> _notifications = new ConcurrentQueue<List<string>>();
        private readonly ConcurrentQueue<List<List<string>>> _warnings = new ConcurrentQueue<List<List<string>>>();
        private const int Seperation = 10; // Distance betweeen notification data in the same row
        private int _prevWarningCount = 0, maxConflictsInWarningMsg = 5; // Used for clearing the current warnings before rewriting the new ones

        
        public ConcurrentQueue<List<string>> GetNotificationQueue()
        {
            return _notifications;
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
                while (_notifications.TryDequeue(out item))
                {
                    var msg = item;

                    WriteNotification(msg);
                    TimerCallback timerDelegate = delegate (object obj)
                    {
                        DeleteNotification();
                    };

                    var timer = new Timer(timerDelegate, null, 5000, Timeout.Infinite);
                }
            }
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
            lock (_notifications)
            {
                _notifications.Enqueue(notification);

                var i = 2;

                foreach (var n in _notifications)
                {
                    Display.WriteRow(n, 10, Display.InnerRightLineBound, i++);
                }
            }
        }

        private void DeleteNotification()
        {
            lock (_notifications)
            {
                var s = new string(' ', Seperation);

                // Clear old
                for (var i = 0; i < _notifications.Count; i++)
                    Display.WriteRow(new List<string> { s, s + ' ' }, Seperation, Display.InnerRightLineBound, i + 2);

                List<string> dummyList;
                _notifications.TryDequeue(out dummyList);

                var j = 2;

                foreach (var n in _notifications)
                    Display.WriteRow(n, 10, Display.InnerRightLineBound, j++);
            }
        }

        /*private void WriteWarning(List<string> warnings)
        {
            lock(_warnings)
            { 
            _warnings.Enqueue(warnings);

            var i = 1;

            foreach (var w in _warnings)
            {
                Display.WriteRow(w, 10, Display.InnerRightLineBound, Display.Height/2 + i++);
            }
            }
        }*/

        private void UpdateWarnings(IEnumerable<List<string>> warnings)
        {
            var s = new string(' ', Seperation);

            List<string> prevMaxConflictsInWarningMsgList = new List<string>();

            int i;
            for (i = 0; i < maxConflictsInWarningMsg; i++)
                prevMaxConflictsInWarningMsgList.Add(s);

            // Clear current warnings
            for (i = 0; i < _prevWarningCount; i++)
                Display.WriteRow(prevMaxConflictsInWarningMsgList, Seperation, Display.InnerRightLineBound,
                    Display.Height / 2 + 1 + i);

            // Rewrite (new) warnings
            i = 0;
            foreach (var w in warnings)
                Display.WriteRow(w, 10, Display.InnerRightLineBound, Display.Height / 2 + ++i);

            _prevWarningCount = i;
        }
    }
}
