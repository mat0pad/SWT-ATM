using System;
using System.Collections.Generic;

namespace SWT_ATM
{
    public class Airspace : IObserver<Data>
    {
        private List<Data> Tracks;

        private IDisplay Display { get; set; }

        private ILog Log { get; set; }

        private IMonitor Monitor { get; set; }

        public Airspace(IMonitor monitor, IDisplay display, ILog log)
        {
            Monitor = monitor;
            Display = display;
            Log = log;

            Tracks = new List<Data>();
            Monitor.SetShareList(ref Tracks);
            
        }


        public void Update(Data data)
        {
            EventType type = Monitor.EventTracker(data);

            // Add/Remove track to list
            UpdateListAfterEvent(type, data);

            string s = "";

            switch (type)
            {
                case EventType.ENTERING:
                    s = "ENTERING";
                    Console.WriteLine(data.Tag + " " + s);
                    Log.WriteNotification(data, false);
                    break;
    
                case EventType.LEAVING:
                    s = "LEAVING";
                    Console.WriteLine(data.Tag + " " + s);
                    Log.WriteNotification(data, true);
                    break;

                case EventType.INSIDE:
                    s = "INSIDE";
                    Console.WriteLine(data.Tag + " " + s);
                    break;

                case EventType.CONFLICTING:

                    s = "CONFLICTING";
                    Console.WriteLine(data.Tag + " " + s);

                    List<Data> list1 = Monitor.GetTracksInConflict();
                    if (list1.Count > 1)
                        Log.WriteWarning(list1);
                    
                    break;

                case EventType.CONFLICTING_ENTERING:

                    s = "CONFLICTING ENTERING";
                    Console.WriteLine(data.Tag + " " + s);

                    List<Data> list2 = Monitor.GetTracksInConflict();
                    if (list2.Count > 1)
                        Log.WriteWarning(list2);
                    break;

                case EventType.CONFLICTING_LEAVING:

                    s = "CONFLICTING ENTERING";
                    Console.WriteLine(data.Tag + " " + s);

                    List<Data> list3 = Monitor.GetTracksInConflict();
                    if (list3.Count > 1)
                        Log.WriteWarning(list3);
                    break;

                default:
                    break;
                
            }
               
        }


        private void UpdateListAfterEvent(EventType eventType, Data data)
        {
            switch (eventType)
            {
                case EventType.ENTERING:
                    Tracks.Add(data);
                    break;
                case EventType.LEAVING:
                    RemoveItem(data);
                    break;
                default:
                    break;
            }
        }

        private void RemoveItem(Data data)
        {
            for (int i = 0; i < Tracks.Count; i++)
            {
                if (Tracks[i].Tag == data.Tag)
                {
                    Tracks.RemoveAt(i);
                    break;
                }
            }
        }
    }



}