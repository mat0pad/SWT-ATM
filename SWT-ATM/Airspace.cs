using System;
using System.Collections.Generic;

namespace SWT_ATM
{
    public class Airspace : IAirspace
    {
        private List<TransponderDataFormat> List { get; set; }

        private IDisplay Display { get; set; }

        private ILog Log { get; set; }

        private IMonitor Monitor { get; set; }

    }
}