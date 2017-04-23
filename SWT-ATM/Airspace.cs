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

            string s = "";

            UpdateListAfterEvent(type, data);

            if (type == EventType.ENTERING)
            {
                s = "ENTERING";
                Console.WriteLine(data.Tag + " " + s);
                Log.WriteNotification(data,false);

                // Test Log conflict
                List<Data> list = new List<Data>();
                list.Add(data);
                list.Add(data);
                Log.WriteWarning(list);
            }
            else if (type == EventType.LEAVING)
            {
                s = "LEAVING";
                Console.WriteLine(data.Tag + " " + s);
                Log.WriteNotification(data, true);
            }
                
            else if (type == EventType.INSIDE)
            {
                s = "INSIDE";
                Console.WriteLine(data.Tag + " " + s);
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