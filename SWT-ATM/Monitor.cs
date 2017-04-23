using System;
using System.Collections.Generic;

namespace SWT_ATM
{
    public class Monitor : IMonitor
    {
        private int XStart { get; set; }
        private int XSlut { get; set; }
        private int ZStart { get; set; }
        private int ZSlut { get; set; }
        private int YStart { get; set; }
        private int YSlut { get; set; }

        private List<Data> _list { get; set; }

        public bool InsideBounds(Data data)
        {
            return data.Altitude > ZStart && data.Altitude < ZSlut
                   && data.YCord > YStart && data.YCord < YSlut
                   && data.XCord > XStart && data.XCord < XSlut;
        }

        private bool ExistsInList(Data data)
        {
            var found = false;

            foreach (var item in _list)
            {
                if (item.Tag == data.Tag)
                {
                    found = true;
                    break;
                }
            }
            return found;
        }

        private bool IsConflicting()
        {
            
            return false;
        }

        public EventType EventTracker(Data data)
        {
            var wasInAirspace = ExistsInList(data);
            var inAirspace = InsideBounds(data);

            if (IsConflicting())
            {
                if (wasInAirspace && !inAirspace)
                    return EventType.CONFLICTING_LEAVING;
                else if (!wasInAirspace && inAirspace)
                    return EventType.CONFLICTING_ENTERING;
                else
                    return EventType.CONFLICTING;
            }
            else if(wasInAirspace && !inAirspace)
                 return EventType.LEAVING;
            else if(!wasInAirspace && inAirspace)
                return EventType.ENTERING;
            else if (inAirspace)
                return EventType.INSIDE;
            else
                return EventType.OUTSIDE;
        }

        public Notification Notification(Data data)
        {
           

            throw new System.NotImplementedException();
        }

        public List<Data> Conflicting(Data data)
        {
            List<Data> inConflict = new List<Data>();

            foreach (var item in _list)
            {
                if (data.Tag != item.Tag)
                {
                    if ((data.Altitude - item.Altitude) < 300 &&
                        ((data.XCord - item.XCord) < 5000 || (data.YCord - item.YCord) < 5000))
                    {
                        inConflict.Add(item);
                    }
                }
            }

            return inConflict;
        }

        public void SetY(int min, int max)
        {
            YSlut = max;
            YStart = min;
        }

        public void SetZ(int min, int max)
        {
            ZSlut = max;
            ZStart = min;
        }

        public void SetX(int min, int max)
        {
            XStart = min;
            XSlut = max;
        }

        public void SetShareList(ref List<Data> list)
        {
            _list = list;
        }
    }
}