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
        private readonly ConcurrentQueue<List<string>> _queue = new ConcurrentQueue<List<string>>();
        private readonly AutoResetEvent _signal = new AutoResetEvent(false);
        private readonly ConcurrentQueue<List<string>> _notifications = new ConcurrentQueue<List<string>>();
        private const int Seperation = 10; // Distance betweeen notification data in the same row
        private int _prevWarningCount = 0, maxConflictsInWarningMsg = 5; // Used for clearing the current warnings before rewriting the new ones


        public ConcurrentQueue<List<string>> GetNotificationQueue()
        {
            return _queue;
        }

        public AutoResetEvent GetSignalHandle()
        {
            return _signal;
        }

        public NotificationCenter()
        {
            Thread thread = new Thread(Start);
            thread.Start();
        }

        private void Start()
        {
            while (true)
            {
                _signal.WaitOne();

                List<string> item;
                while (_queue.TryDequeue(out item))
                {
                    var msg = item;

                    if (msg[0] == "n")
                    {
                        msg.RemoveAt(0);
                        WriteNotification(msg);
                        TimerCallback timerDelegate = delegate (object obj)
                        {
                            DeleteNotification();  
                        };

                        var timer = new Timer(timerDelegate, null, 5000, Timeout.Infinite);
                    }
                    else
                    {
                        msg.RemoveAt(0);

                        //UpdateWarnings(msg);
                       

                    }  
                }
            }
        }

        private void WriteNotification(List<string> notification)
        {
            lock(_notifications)
            { 
            _notifications.Enqueue(notification);

            var j = 2;

            foreach (var n in _notifications)
            {
                Display.WriteRow(n, 10, Display.InnerRightLineBound, j++);
            }
            }
        }

        private void DeleteNotification()
        {
            lock(_notifications)
            { 
                var s = new string(' ', Seperation);

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
