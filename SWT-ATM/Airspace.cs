using System;
using System.Collections.Generic;

namespace SWT_ATM
{
    public class Airspace : IObserver<Data>
    {
        private List<Data> _tracks;

        private List<Data> _tracksInConlict;

        private IDisplay Display { get; set; }

        private ILog Log { get; set; }

        private IMonitor Monitor { get; set; }

        public Airspace(IMonitor monitor, IDisplay display, ILog log)
        {
            Monitor = monitor;
            Display = display;
            Log = log;

            _tracks = new List<Data>();
            Monitor.SetShareList(ref _tracks);
            
            _tracksInConlict = new List<Data>();
        }

        public List<Data> GetTracks()
        {
            return _tracks;
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
                    Log.WriteNotification(data, false);
                    //Display.ShowTracks(_tracks);
                    Display.ShowNotification(data,EventType.ENTERING);

                    break;
    
                case EventType.LEAVING:
                    s = "LEAVING";
                    Log.WriteNotification(data, true);
                    Display.ShowNotification(data,EventType.LEAVING);
                    break;

                case EventType.INSIDE:
                    s = "INSIDE";
                    Display.ShowTracks(new List<Data>(_tracks));
                    // Console.WriteLine(data.Tag + " " + s);
                    break;

                case EventType.CONFLICTING:

                    s = "CONFLICTING";
                    //Console.WriteLine(data.Tag + " " + s);

                    _tracksInConlict = Monitor.GetTracksInConflict(); // Get others
                    _tracksInConlict.Add(data); // Add self

                    if (_tracksInConlict.Count > 1)
                    {
                        //Log.WriteWarning(_tracksInConlict);
                        //Display.ShowWarning(templist);
                    }

                    break;

                case EventType.CONFLICTING_ENTERING:

                    s = "CONFLICTING ENTERING";
                   // Console.WriteLine(data.Tag + " " + s);

                    _tracksInConlict = Monitor.GetTracksInConflict(); // Get others
                    _tracksInConlict.Add(data); // Add self

                    if (_tracksInConlict.Count > 1)
                    {
                     //   Log.WriteWarning(_tracksInConlict);
                        //Display.ShowWarning(templist);
                    }
                    break;

                case EventType.CONFLICTING_LEAVING:

                    s = "CONFLICTING LEAVING";
                   // Console.WriteLine(data.Tag + " " + s);

                    _tracksInConlict = Monitor.GetTracksInConflict(); // Get others
                    _tracksInConlict.Add(data); // Add self

                    if (_tracksInConlict.Count > 1)
                    {
                       // Log.WriteWarning(_tracksInConlict);
                       // Display.ShowWarning(templist);
                    }

                    break;   
            }
               
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