using System;
using System.Collections.Generic;

namespace SWT_ATM
{
    public class Airspace : IObserver<Data>
    {
        private List<Data> List { get; set; }

        private IDisplay Display { get; set; }

        private ILog Log { get; set; }

        private IMonitor Monitor { get; set; }

        public Airspace(IMonitor monitor, IDisplay display, ILog log)
        {
            Monitor = monitor;
            Display = display;
            Log = log;
        }

        private void CheckIfRelevant(Data data)
        {
            throw new NotImplementedException();
        }

        public void Update(Data subject)
        {
           Console.WriteLine("Track: " + subject.Tag);
        }
    }
}