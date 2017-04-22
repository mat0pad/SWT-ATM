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

        public bool InsideBounds(Data data)
        {
            throw new System.NotImplementedException();
        }

        public Notification Notification(Data data)
        {
            throw new System.NotImplementedException();
        }

        public List<Warning> Conflicting(List<Data> listdata)
        {
            throw new System.NotImplementedException();
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
    }
}