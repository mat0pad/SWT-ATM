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

<<<<<<< HEAD
=======
		public void Update(EventSubject subject)
		{
			throw new NotImplementedException();
		}

		public void Write()
        {
            throw new NotImplementedException();
        }
>>>>>>> 136c0ca207fc983fc27522b16ecd19414a89ce45
    }
}