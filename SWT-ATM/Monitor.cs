using System;
using System.Collections.Generic;
using System.Linq;

namespace SWT_ATM
{
    public class Monitor : IMonitor
    {
        private int XStart;
        private int XSlut;
        private int ZStart;
        private int ZSlut;
        private int YStart;
        private int YSlut;

        public Monitor()
        {
            _tracksInConflict = new List<Data>();
        }

        private List<Data> _list { get; set; }

        private List<Data> _tracksInConflict;

        public List<Data> GetTracksInConflict()
        {

            lock (_tracksInConflict)
            {
                return _tracksInConflict.ToList();
            }
        }


        private bool InsideBounds(Data data)
        {
            return data.Altitude > ZStart && data.Altitude < ZSlut
                   && data.YCord > YStart && data.YCord < YSlut
                   && data.XCord > XStart && data.XCord < XSlut;
        }

        private bool ExistsInList(Data data)
        {
            var found = false;

            for (int i = 0; i < _list.Count; i++)
            {
                if (_list[i].Tag == data.Tag)
                {
                    found = true;
                    _list[i].Timestamp = data.Timestamp;
                    _list[i].XCord = data.XCord;
                    _list[i].YCord = data.YCord;
                    _list[i].Altitude = data.Altitude;
                    break;
                }
            }
            return found;
        }

        private bool IsConflicting(Data data)
        {
            lock (_tracksInConflict)
            {
                _tracksInConflict.Clear();

                foreach (var item in _list.ToList())
                {
                    if (data.Tag != item.Tag)
                    {
                        if (Math.Abs((data.Altitude - item.Altitude)) < 300 &&
                            (Math.Abs(data.XCord - item.XCord)) < 5000 && (Math.Abs(data.YCord - item.YCord) < 5000))
                        {
                            _tracksInConflict.Add(item);
                        }
                    }
                }
            }
            return _tracksInConflict.Count >= 1;
        }

        public List<List<Data>> GetAllConflicts()
        {
            List<Data> foundConflictsForCurrent = new List<Data>();
            List<Data> tracksToCheck = new List<Data>(_list);
            List<List<Data>> conflictList = new List<List<Data>>();

            for (int i = tracksToCheck.Count - 1; i > 0; i--)
            {
                var item = tracksToCheck[i];
                tracksToCheck.Remove(item);
                foundConflictsForCurrent.Clear();
                foundConflictsForCurrent.Add(item);

                foreach (var data in tracksToCheck)
                {
                    if (data.Tag != item.Tag)
                    {
                        if (Math.Abs((data.Altitude - item.Altitude)) < 300 &&
                            (Math.Abs(data.XCord - item.XCord)) < 5000 && (Math.Abs(data.YCord - item.YCord) < 5000))
                        {
                            foundConflictsForCurrent.Add(data);
                        }
                    }
                }
                if (foundConflictsForCurrent.Count != 1)
                    conflictList.Add(new List<Data>(foundConflictsForCurrent));
            }
            return conflictList;
        }




        public EventType EventTracker(Data data)
        {
            var wasInAirspace = ExistsInList(data);
            var inAirspace = InsideBounds(data);

            if (IsConflicting(data))
            {
                if (wasInAirspace && !inAirspace)
                    return EventType.CONFLICTING_LEAVING;
                else if (!wasInAirspace && inAirspace)
                    return EventType.CONFLICTING_ENTERING;
                else
                    return EventType.CONFLICTING;
            }
            else if (wasInAirspace && !inAirspace)
                return EventType.LEAVING;
            else if (!wasInAirspace && inAirspace)
                return EventType.ENTERING;
            else if (inAirspace)
                return EventType.INSIDE;
            else
                return EventType.OUTSIDE;
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

        public int[] GetY()
        {
            int[] array = new int[2];
            array[0] = YStart;
            array[1] = YSlut;
            return array;
        }

        public int[] GetX()
        {
            int[] array = new int[2];
            array[0] = XStart;
            array[1] = XSlut;
            return array;
        }

        public int[] GetZ()
        {
            int[] array = new int[2];
            array[0] = ZStart;
            array[1] = ZSlut;
            return array;
        }

    }
}