using System;
using System.Collections.Generic;

namespace SWT_ATM
{
    public class Airspace : IObserver<List<Data>>
    {
        private List<Data> _tracks;

        private IDisplayFormatter DisplayFormatter { get; set; }

        private ILog Log { get; set; }

        private IMonitor Monitor { get; set; }

        public Airspace(IMonitor monitor, IDisplayFormatter display, ILog log)
        {
            Monitor = monitor;
            DisplayFormatter = display;
            Log = log;

            _tracks = new List<Data>();
            Monitor.SetShareList(ref _tracks);
            
        }

        public List<Data> GetTracks()
        {
            return _tracks;
        }

        public void Update(List<Data> data)
        {
            Console.WriteLine("Data received!");
            /*EventType type = Monitor.EventTracker(data);

            // Add/Remove track to list
            UpdateListAfterEvent(type, data);

            string s = "";

            switch (type)
            {
                case EventType.ENTERING:
                    
                    Log.WriteNotification(data, false);
                    DisplayFormatter.ShowNotification(data,EventType.ENTERING);
                    break;
    
                case EventType.LEAVING:

                    Log.WriteNotification(data, true);
                    DisplayFormatter.ShowNotification(data,EventType.LEAVING);
                    DisplayFormatter.ShowTracks(new List<Data>(_tracks));
                    break;

                case EventType.INSIDE:

                    DisplayFormatter.ShowTracks(new List<Data>(_tracks));
                    break;

                case EventType.CONFLICTING:

                    List<Data> list = Monitor.GetTracksInConflict();
                    DisplayFormatter.ShowWarning(Monitor.GetAllConflicts());
                    list.Add(data);
                    
                    if (list.Count > 1)
                    {
                        Log.WriteWarning(list);
                    }

                    break;

                case EventType.CONFLICTING_ENTERING:

                    var list1 = Monitor.GetTracksInConflict();
                    DisplayFormatter.ShowWarning(Monitor.GetAllConflicts());
                    list1.Add(data);

                    if (list1.Count > 1)
                    {
                        Log.WriteWarning(list1);
                    }
                    break;

                case EventType.CONFLICTING_LEAVING:
                    
                    var list2 = Monitor.GetTracksInConflict();
                    DisplayFormatter.ShowWarning(Monitor.GetAllConflicts());
                    list2.Add(data);

                    if (list2.Count > 1)
                    {
                        Log.WriteWarning(list2);
                    }

                    DisplayFormatter.ShowTracks(new List<Data>(_tracks));

                    break;   
            }*/
               
        }

        private void UpdateListAfterEvent(EventType eventType, Data data)
        {
            switch (eventType)
            {
                case EventType.ENTERING:
                    _tracks.Add(data);
                    break;
                case EventType.LEAVING:
                    RemoveItem(data);
                    break;
            }
        }

        private void RemoveItem(Data data)
        {
            for (int i = 0; i < _tracks.Count; i++)
            {
                if (_tracks[i].Tag == data.Tag)
                {
                    _tracks.RemoveAt(i);
                    break;
                }
            }
        }
    }



}