﻿using System;
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
    public class NotificationCenter : INotificationCenter
    {
        private static readonly object WarningLock = new object();
        private readonly ConcurrentQueue<List<string>> _notificationsQueue = new ConcurrentQueue<List<string>>();
        private readonly AutoResetEvent _notficationSignal = new AutoResetEvent(false);
        private readonly AutoResetEvent _warningSignal = new AutoResetEvent(false);
        private readonly ConcurrentQueue<List<string>> _notificationsToShow = new ConcurrentQueue<List<string>>();
        private readonly ConcurrentQueue<List<List<string>>> _warnings = new ConcurrentQueue<List<List<string>>>();

        private const int Seperation = 10; // Distance betweeen notification data in the same row
        private int _prevWarningCount = 0, maxConflictsInWarningMsg = 5; // Used for clearing the current warnings before rewriting the new ones
        private readonly IDisplay _display;
        private IDisplayFormatter _formatter;

        public void EnqueNotification(List<string> item)
        {
            _notificationsQueue.Enqueue(item);
        }

        public void SetNotificationSignalHandle()
        {
            _notficationSignal.Set();
        }

        public void EnqueWarning(List<List<string>> item)
        {
            _warnings.Enqueue(item);
        }

        public void SetWarningsSignalHandle()
        {
            _warningSignal.Set();
        }

        public NotificationCenter(IDisplay display)
        {
            _display = display;
            var thread1 = new Thread(NotificationThread);
            var thread2 = new Thread(WarningThread);
            thread1.Start();
            thread2.Start();
        }

        public void SetFormatter(IDisplayFormatter formatter)
        {
            _formatter = formatter;
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

        public async Task ExecuteDelayed(Action action, int timeoutInMilliseconds)
        {
            await Task.Delay(timeoutInMilliseconds);
            action();
        }

        private void WriteNotification(List<string> notification)
        {
            lock (_notificationsToShow)
            {
                _notificationsToShow.Enqueue(notification);

                var i = 2;

                foreach (var n in _notificationsToShow)
                {
                    _display.WriteRow(n, Seperation, _display.GetInnerRightLineBound(), i++);
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
                    _display.WriteRow(new List<string> { s, s }, Seperation, _display.GetInnerRightLineBound(), i + 2);

                List<string> dummyList;
                _notificationsToShow.TryDequeue(out dummyList);


                var j = 2;
                // Write new
                foreach (var n in _notificationsToShow)
                    _display.WriteRow(n, Seperation, _display.GetInnerRightLineBound(), j++);
            }
        }

        private void UpdateWarnings(IEnumerable<List<string>> warnings)
        {
            var s = new string(' ', _display.GetOuterRightLineBound() - _display.GetInnerRightLineBound() - 1);

            int i;
            // Clear current warnings
            for (i = 0; i < _prevWarningCount; i++)
                _display.WriteRow(new List<string> { s }, 0, _display.GetInnerRightLineBound(),
                    _formatter.GetHeight() / 2 + 1 + i);

            // Rewrite (new) warnings
            i = 0;
            foreach (var w in warnings)
            {
                List<string> tmpList = new List<string>(w);
                tmpList.Add("CONFLICTING");
                _display.WriteRow(tmpList, Seperation, _display.GetInnerRightLineBound(), _formatter.GetHeight() / 2 + ++i);
            }

            lock (WarningLock)
                _prevWarningCount = i;
        }
    }
}
