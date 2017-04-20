using System;
using System.Collections.Generic;

namespace SWT_ATM
{
    public class CoordinateMapper: IObserver<Track>
    {
        private List<Track> List { get; set; }

        public void Update(Track subject)
        {
            List.Add(subject);
        }
    }
}